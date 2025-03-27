using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace front.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserBookController : ControllerBase
    {
        private HttpClient client;
        public async Task<IActionResult> GetUser(){
           var result = await client.GetAsync("https://localhost:5112/api/GetUser");
           return Ok(await result.Content.ReadFromJsonAsync<object>());
        }
    }
}