using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
  
namespace webapi1
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> PullProducts()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://webapi2:81/api/Product/GetProducts");
            var content = await response.Content.ReadAsStringAsync();
            return base.Ok(content);
        }
    }
}