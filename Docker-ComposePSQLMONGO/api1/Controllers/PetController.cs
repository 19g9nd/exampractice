using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace api1.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PetController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var client = new MongoClient("mongodb://mymongo:27017");
            var db = client.GetDatabase("mydb");
            var collection = db.GetCollection<Pet>("pets");
            var data = collection.Find(pet => true).ToList();
            return base.Ok(data);
        }


        [HttpGet("GetByOwnerId/{ownerId}")]
        public IActionResult GetByOwnerId([FromRoute]string ownerId)
        {
            var client = new MongoClient("mongodb://mymongo:27017");
            var db = client.GetDatabase("mydb");
            var collection = db.GetCollection<Pet>("pets");
            var data = collection.Find(pet => pet.OwnerId == ownerId).ToList();
            return base.Ok(data);
        }


        [HttpPost]
        public IActionResult Add(Pet newPet)
        {
            var client = new MongoClient("mongodb://mymongo:27017");
            var db = client.GetDatabase("mydb");
            var collection = db.GetCollection<Pet>("pets");
            collection.InsertOne(new Pet
            {
                Name = newPet.Name,
                Age = newPet.Age,
                Type = newPet.Type,
                OwnerId = newPet.OwnerId
            });
            return base.Ok();
        }

    }
    public class Pet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Type { get; set; }
        public string? OwnerId { get; set; }
    }

}