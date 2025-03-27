using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using psql.Data;

namespace psql.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly string connectionString;

        public ProductController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("postgreDb") ??
                               throw new ArgumentNullException(nameof(configuration));
        }

        [HttpGet]
        public IActionResult GetProduct(int id)
        {
            const string query = "SELECT * FROM PRODUCT WHERE ID = @id;";
            using var connection = new NpgsqlConnection(connectionString);
            var product = connection.QueryFirstOrDefault<ProductDto>(query, new { id });
            return base.Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name) || product.Price <= -1)
                return BadRequest("Invalid product data.");

            const string query = "INSERT INTO PRODUCT (NAME, PRICE) VALUES (@name, @price) RETURNING ID;";

            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            var insertedId = await connection.ExecuteScalarAsync<int>(query, new { name = product.Name, price = product.Price });

            return Ok(new { Message = "Product added successfully", ProductId = insertedId });
        }
    }
}
