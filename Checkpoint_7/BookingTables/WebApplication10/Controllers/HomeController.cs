using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication10.Models;

namespace WebApplication10.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config) => _config = config;

        [HttpPost("bookTable")]
        public IActionResult BookTable([FromBody] Booking booking)
        {
            try
            {

                using var connection = new SqlConnection(_config.GetConnectionString("YourConnectionString"));
                connection.Open();

                var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Tables WHERE Id = @tableId", connection);
                checkCmd.Parameters.AddWithValue("@tableId", booking.TableId);
                if ((int)checkCmd.ExecuteScalar() == 0)
                    return BadRequest("Столик не найден");

                var busyCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Bookings WHERE TableId = @tableId AND Time = @time",
                    connection);
                busyCmd.Parameters.AddWithValue("@tableId", booking.TableId);
                busyCmd.Parameters.AddWithValue("@time", booking.Time);
                if ((int)busyCmd.ExecuteScalar() > 0)
                    return BadRequest("Столик уже забронирован на это время");

                var cmd = new SqlCommand(
                    "INSERT INTO Bookings (TableId, Time, Customer) VALUES (@tableId, @time, @customer); SELECT SCOPE_IDENTITY();", 
                    connection);

                cmd.Parameters.AddWithValue("@tableId", booking.TableId);
                cmd.Parameters.AddWithValue("@time", booking.Time);
                cmd.Parameters.AddWithValue("@customer", booking.Customer);

                var newId = cmd.ExecuteScalar();
                return Ok(new { Message = "Столик забронирован", BookingId = newId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка: {ex.Message}");
            }
        }

        [HttpGet("free")]
        public IActionResult GetFreeTables([FromQuery] DateTime time)
        {
            try
            {
                using var connection = new SqlConnection(_config.GetConnectionString("YourConnectionString"));
                connection.Open();

                var cmd = new SqlCommand(
                    "SELECT t.Id, t.Number, t.Seats FROM Tables t " +
                    "WHERE t.Id NOT IN (SELECT b.TableId FROM Bookings b WHERE b.Time = @time)",
                    connection);

                cmd.Parameters.AddWithValue("@time", time);
                using var reader = cmd.ExecuteReader();
                var tables = new List<object>();

                while (reader.Read())
                    tables.Add(new
                    {
                        Id = reader.GetInt32(0),
                        Number = reader.GetInt32(1),
                        Seats = reader.GetInt32(2)
                    });

                return Ok(tables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка: {ex.Message}");
            }
        }

        [HttpDelete("cancelTable/{id}")]
        public IActionResult Cancel(int id)
        {
            try
            {
                using var connection = new SqlConnection(_config.GetConnectionString("YourConnectionString"));
                connection.Open();

                var cmd = new SqlCommand("DELETE FROM Bookings WHERE Id = @id", connection);
                cmd.Parameters.AddWithValue("@id", id);

                return cmd.ExecuteNonQuery() > 0 ?
                    Ok("Бронирование отменено") :
                    NotFound("Бронирование не найдено");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка: {ex.Message}");
            }
        }

        [HttpPut("updateTime/{id}")]
        public IActionResult UpdateTime(int id, [FromBody] DateTime newTime)
        {
            try
            {
                using var connection = new SqlConnection(_config.GetConnectionString("YourConnectionString"));
                connection.Open();

                var checkCmd = new SqlCommand("SELECT TableId FROM Bookings WHERE Id = @id", connection);
                checkCmd.Parameters.AddWithValue("@id", id);
                var tableId = checkCmd.ExecuteScalar();

                var busyCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Bookings WHERE TableId = @tableId AND Time = @time AND Id != @id",
                    connection);
                busyCmd.Parameters.AddWithValue("@tableId", tableId);
                busyCmd.Parameters.AddWithValue("@time", newTime);
                busyCmd.Parameters.AddWithValue("@id", id);

                if ((int)busyCmd.ExecuteScalar() > 0)
                    return BadRequest("Столик уже занят на это время");
                var cmd = new SqlCommand("UPDATE Bookings SET Time = @time WHERE Id = @id", connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@time", newTime);

                return cmd.ExecuteNonQuery() > 0 ?
                    Ok("Время бронирования обновлено") :
                    NotFound("Бронирование не найдено");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка: {ex.Message}");
            }
        }
    }
}