using API_front;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace API_front.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string _connectionString = "Server=DESKTOP-SSEIQRS\\MSSQLSERVER01;Database=dbtest;User Id=sa;Password=12345678;TrustServerCertificate=true;";

        [HttpPost("login")]
        public IActionResult Login([FromBody] Users user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                var result = connection.QuerySingleOrDefault<Users>(sql, new { user.username, user.password });
                if (result != null)
                {

                    return Ok("Login successful.");
                }
                else
                {
                    return Unauthorized("Invalid credentials.");
                }
            }

        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] Users user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                var rowsAffected = connection.Execute(sql, new { user.username, user.password });
                Console.WriteLine(rowsAffected.ToString());
                if (rowsAffected > 0)
                {
                    return Ok("User registered successfully.");
                }
                else
                {
                    return StatusCode(500, "An error occurred while registering the user.");
                }
            }

        }
    }
}

