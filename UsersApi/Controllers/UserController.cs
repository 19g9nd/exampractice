using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using StackExchange.Redis;
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
            var postgreConStr = configuration.GetConnectionString("postgresDb");
            var redisConStr = configuration.GetConnectionString("redisDb");
            await using var connection = new NpgsqlConnection(postgreConStr);
            await connection.OpenAsync();
            var redis = await ConnectionMultiplexer.ConnectAsync(redisConStr);
            var cacheDb = redis.GetDatabase();
            try
            {
                var redisKey =$"Car_{id}";
                var redisValue = await cacheDb.StringGetAsync(redisKey);
                if(redisValue.HasValue){
                    var carFromCache = JsonSerializer.Deserialize<User>(redisValue);
                    return Ok(carFromCache);
                }

                var car = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @id", new { id });
                if (car == null)
                {
                    return NotFound();
                }
                await cacheDb.StringSetAsync(redisKey,JsonSerializer.Serialize(car));
                return Ok(car);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User newUser)
        {
            var postgreConStr = configuration.GetConnectionString("postgresDb");
            await using var connection = new NpgsqlConnection(postgreConStr);
            await connection.OpenAsync();

            try
            {
                var sql = "INSERT INTO Users (Id, Name) VALUES (@Id, @Name) RETURNING *";
                var car = await connection.QuerySingleOrDefaultAsync<User>(sql, new { newUser.Id, newUser.Name });
                if (car == null)
                {
                    return NotFound();
                }
                return Ok(car);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex}");
            }
        }

    }

}
