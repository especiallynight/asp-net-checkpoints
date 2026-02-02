using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Configuration;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPut("updateUser/{id}")]
        public IActionResult UpdateUser(int id, string name, string email, int age)
        {
            var requestTime = DateTime.UtcNow;
            string connectionString = _configuration.GetConnectionString("YourConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError($"[{DateTime.UtcNow}] Ошибка: строка подключения 'YourConnectionString' не найдена или пуста.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка конфигурации: строка подключения не найдена.");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = "UPDATE Users SET Name = @Name, Email = @Email, Age = @Age WHERE Id = @Id";

                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@Id", id);
                            command.Parameters.AddWithValue("@Name", name);
                            command.Parameters.AddWithValue("@Email", email);
                            command.Parameters.AddWithValue("@Age", age);

                            int result = command.ExecuteNonQuery();

                            if (result < 1)
                            {
                                transaction.Rollback();
                                _logger.LogWarning("[{Time}] Ошибка: пользователь с Id={UserId} не найден.", requestTime, id);
                                return BadRequest("Обновление не удалось.");
                            }
                        }
                        transaction.Commit();
                        _logger.LogInformation("[{Time}] Данные пользователя с Id={UserId} успешно обновлены.", requestTime, id);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError(ex, "[{Time}] Ошибка: не удалось обновить данные пользователя с Id={UserId}.", requestTime, id);
                        return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка сервера: {ex.Message}");
                    }
                }
            }
            return Ok("Данные пользователя обновлены.");
        }



        [HttpPut("UpdateEmailIfNameMatches")]
        public IActionResult UpdateEmailIfNameMatches(int id, string currentName, string newEmail)
        {
            var requestTime = DateTime.UtcNow;
            string connectionString = _configuration.GetConnectionString("YourConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError($"[{requestTime}] Ошибка: строка подключения не найдена.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка конфигурации.");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string query = "UPDATE Users SET Email = @Email WHERE Id = @Id AND Name = @Name";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Name", currentName);
                        command.Parameters.AddWithValue("@Email", newEmail);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            _logger.LogWarning("[{Time}] Обновление email не выполнено: имя не совпадает или пользователь не найден.", requestTime);
                            return BadRequest("Имя пользователя не совпадает или пользователь не найден.");
                        }
                    }
                    _logger.LogInformation("[{Time}] Email пользователя с Id={Id} успешно обновлен.", requestTime, id);
                    return Ok("Email успешно обновлен.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[{Time}] Ошибка при обновлении email пользователя с Id={Id}.", requestTime, id);
                    return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка сервера.");
                }
            }
        }


        [HttpGet("getUser/{id}")]
        public IActionResult GetUser(int id)
        {
            string connectionString = _configuration.GetConnectionString("YourConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Name, Email, Age FROM Users WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var user = new
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Age = reader.GetInt32(3)
                            };

                            return Ok(user);
                        }
                        else
                        {
                            return NotFound($"Пользователь с ID={id} не найден");
                        }
                    }
                }
            }
        }
    }
}

