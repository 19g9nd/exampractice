using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using UsersApi.Models;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {

        private IConfiguration configuration;
        private NpgsqlConnection connection;
        public UserController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {
            var connectionStr = configuration.GetConnectionString("postgresDb");
            await using var connection = new NpgsqlConnection(connectionStr);
            await connection.OpenAsync();

            try
            {
                var result = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @id", new { id });
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "My server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User newUser)
        {
            var connectionStr = configuration.GetConnectionString("postgresDb");
            await using var connection = new NpgsqlConnection(connectionStr);
            await connection.OpenAsync();

            try
            {
                var sql = "INSERT INTO Users (Id, Name) VALUES (@Id, @Name) RETURNING *";
                var result = await connection.QuerySingleOrDefaultAsync<User>(sql, new { newUser.Id, newUser.Name });
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex}");
            }
        }

    }

}
