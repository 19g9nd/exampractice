using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace webapi2.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            await using var connection = new NpgsqlConnection("User ID=myuser;Password=Pass1234!;Host=mypostgres;Port=5432;Database=mydb;");
            var products = await connection.QueryAsync("SELECT * FROM PRODUCTS");
            return Ok(products);
        }
    }
}