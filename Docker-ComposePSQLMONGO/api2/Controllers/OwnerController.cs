using System.Text.Json;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace api2.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OwnerController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var db = new Npgsql.NpgsqlConnection(
            "Host=mypostgres;Port=5432;User ID=admin;Password=Pass1234!;Database=ownersDb"
            );
            var cacheconnection = StackExchange.Redis.ConnectionMultiplexer.Connect("cache");
            var cache = cacheconnection.GetDatabase();
            var cacheKey = "owners_all";
            var cachedData = cache.StringGet(cacheKey);
            if (!cachedData.IsNullOrEmpty)
            {
            return Ok(JsonSerializer.Deserialize<List<Owner>>(cachedData));
            }
            var data = db.Query<Owner>("SELECT * FROM owners").ToList();
            var serialized = JsonSerializer.Serialize(data);
            cache.StringSet(cacheKey, serialized, TimeSpan.FromMinutes(10));
            return base.Ok(data);
        }

        [HttpGet("GetById/{ownerId}")]
        public IActionResult GetById(int ownerId)
        {
            using var db = new Npgsql.NpgsqlConnection(
                "Host=mypostgres;Port=5432;User ID=admin;Password=Pass1234!;Database=ownersDb"
            );
            var sql = "SELECT * FROM owners WHERE id = @Id";
            var data = db.Query<Owner>(sql, new { Id = ownerId }).FirstOrDefault();

            return Ok(data);
        }

        [HttpPost]
        public IActionResult Post(Owner newOwner)
        {
            var db = new Npgsql.NpgsqlConnection(
                "Host=mypostgres;Port=5432;User ID=admin;Password=Pass1234!;Database=ownersDb"
            );
            var sql = "INSERT INTO owners (name) VALUES (@Name)";
            var affected = db.Execute(sql, new { Name = newOwner.Name });
            return base.Ok(affected > 0 ? "Owner added successfully" : "Failed to add owner");
        }
    }

    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
