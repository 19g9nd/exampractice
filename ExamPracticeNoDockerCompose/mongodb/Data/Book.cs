using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongodb.Data
{
    public class Book
    {
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }
        public int Author_Id { get; set; }
        public int Year { get; set; }
    }
}