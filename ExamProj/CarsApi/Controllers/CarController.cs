using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarsApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CarsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CarController : ControllerBase
    {

        private IConfiguration configuration;
        public CarController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetCar(int id)
        {

            var mongoDbConnectionString = this.configuration.GetConnectionString("MongoDb");
            var client = new MongoClient(mongoDbConnectionString);

            var carDb = client.GetDatabase("CarDb");
            var collection = carDb.GetCollection<Car>("Cars");
            var car = await collection
   .Find(c => c.Id == id).FirstOrDefaultAsync();
            return base.Ok(car);
        }


        [HttpPost]
        public async Task<IActionResult> AddCar(Car newCar)
        {
            var mongoDbConnectionString = this.configuration.GetConnectionString("MongoDb");
            var client = new MongoClient(mongoDbConnectionString);

            var carDb = client.GetDatabase("CarDb");
            var collection = carDb.GetCollection<Car>("Cars");
            await collection.InsertOneAsync(newCar);
            return base.Ok();
        }
    }
}