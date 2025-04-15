using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace front.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class OwnerPetController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetOwners()
        {
            var client = new HttpClient();
            var response = client.GetAsync("http://apigateway:82/getOwners").Result;
            return base.Ok(response.Content.ReadAsStringAsync().Result);
        }

        [HttpGet]
        public IActionResult GetPets()
        {
            var client = new HttpClient();
            var response = client.GetAsync("http://apigateway:82/getPets").Result;
            return base.Ok(response.Content.ReadAsStringAsync().Result);
        }

        // [HttpGet]
        // public IActionResult GetPetAndOwnerInfo(int ownerId)
        // {
        //     var client = new HttpClient();
        //     var petInfo = client.GetAsync($"http://apigateway:82/GetPet/{ownerId}").Result;
        //     var ownerInfo = client.GetAsync($"http://apigateway:82/GetOwner/{ownerId}").Result;
        //     var petOwnerInfo = new
        //     {
        //         petInfo = petInfo.Content.ReadAsStringAsync().Result,
        //         ownerInfo = ownerInfo.Content.ReadAsStringAsync().Result
        //     };

        //     return base.Ok(petOwnerInfo);

        // }
    }
}
