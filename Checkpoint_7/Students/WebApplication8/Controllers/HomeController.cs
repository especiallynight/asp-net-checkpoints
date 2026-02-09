using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;
using WebApplication8.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace WebApplication8.Controllers
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
        [HttpPost("createStudent")]
        public IActionResult CreateStudent([FromBody] Student student)
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
                        string query = @"INSERT INTO Students 
                        (Name, Email, Age) 
                        VALUES 
                        (@Name, @Email, @Age);
                        SELECT SCOPE_IDENTITY();"; 

                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@Name", student.Name);
                            command.Parameters.AddWithValue("@Email", student.Email);
                            command.Parameters.AddWithValue("@Age", student.Age);
                            var newId = command.ExecuteScalar();
                        }

                        transaction.Commit();
                        _logger.LogInformation("[{Time}] Студент успешно создан.", requestTime);
                        return Ok(new { Message = "Студент успешно создан." });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError(ex, "[{Time}] Ошибка: не удалось создать студента.", requestTime);
                        return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка сервера: {ex.Message}");
                    }
                }
            }
        }

        [HttpPut("updateStudent/{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] Student student) 
        {
            var requestTime = DateTime.UtcNow;
            string connectionString = _configuration.GetConnectionString("YourConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError($"[{DateTime.UtcNow}] Ошибка: строка подключения не найдена.");
                return StatusCode(500, "Ошибка конфигурации");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Students SET Name = @Name, Email = @Email, Age = @Age WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", student.Name);
                    command.Parameters.AddWithValue("@Email", student.Email);
                    command.Parameters.AddWithValue("@Age", student.Age);

                    int result = command.ExecuteNonQuery();

                    if (result < 1)
                    {
                        _logger.LogWarning("[{Time}] Студент с Id={StudentId} не найден.", requestTime, id);
                        return NotFound($"Студент с ID={id} не найден.");
                    }

                    _logger.LogInformation("[{Time}] Данные студента с Id={StudentId} обновлены.", requestTime, id);
                    return Ok($"Данные студента с ID={id} обновлены.");
                }
            }
        }


        [HttpGet("getStudents")]
        public IActionResult GetStudents()
        {
            string connectionString = _configuration.GetConnectionString("YourConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Name, Email, Age FROM Students";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Student> students = new List<Student>();

                        while (reader.Read())
                        {
                            students.Add(new Student
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Age = reader.GetInt32(3)
                            });
                        }
                        if (students.Count == 0)
                        {
                            return Ok(new { Message = "Ни одного студента не было добавлено", Students = students });
                        }

                        return Ok(students);
                    }
                }
            }
        }

        [HttpDelete("deleteStudent/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            string connectionString = _configuration.GetConnectionString("YourConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE from Students WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok($"Студент с ID={id} успешно удалён");
                    }
                    else
                    {
                        return NotFound($"Студент с ID={id} не найден");
                    }
                }
            }
        }
    }
}