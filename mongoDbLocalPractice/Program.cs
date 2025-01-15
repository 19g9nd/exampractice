using MongoDB.Driver;
using MongoDB.Bson;

Environment.SetEnvironmentVariable("MONGODB", "mongodb://localhost:27017");

var connectionString = Environment.GetEnvironmentVariable("MONGODB");
if (connectionString == null)
{
    Console.WriteLine("no connection string");
    Environment.Exit(0);
}

var client = new MongoClient(connectionString);
var collection = client.GetDatabase("practice").GetCollection<BsonDocument>("Dogs");

var documents = await collection.Find(new BsonDocument()).ToListAsync();

foreach (var doc in documents)
{
    Console.WriteLine(doc);
}

Console.WriteLine("All documents retrieved.");

try
{
    var newDocument = new BsonDocument
    {
        { "name", "Golden Retriever" },
        { "age", 3 }
    };

    await collection.InsertOneAsync(newDocument);
    Console.WriteLine("Document inserted successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error inserting document: {ex.Message}");
}

