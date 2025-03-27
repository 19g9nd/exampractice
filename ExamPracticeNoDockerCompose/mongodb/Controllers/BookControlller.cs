using Microsoft.AspNetCore.Mvc;
using mongodb.Data;
using MongoDB.Driver;

namespace mongodb.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BookControlller : ControllerBase
    {

        private readonly MongoClient client;
        private readonly IMongoDatabase db;
        private readonly IMongoCollection<Book> collection;
        public BookControlller(IConfiguration configuration)
        {
            this.client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            this.db = client.GetDatabase("bookstore");
            this.collection = db.GetCollection<Book>("books");
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = collection.Find(_ => true).ToList();
            return Ok(books);
        }

        [HttpGet]
        public IActionResult GetBookByAuthor(int author_Id)
        {

            var booksByAuthor = collection.Find(book => book.Author_Id == author_Id)
                                          .ToList()
                                          .Select(book => new BookDto
                                          {
                                              Title = book.Title,
                                              Author_Id = book.Author_Id,
                                              Year = book.Year
                                          })
                                          .ToList();

            return booksByAuthor.Count > 0 ? Ok(booksByAuthor) : NotFound();
        }
        [HttpPost]
        public IActionResult AddBook(BookDto newBook)
        {

            collection.InsertOne(new Book
            {
                Title = newBook.Title,
                Author_Id = newBook.Author_Id,
                Year = newBook.Year
            });
            return base.Ok();
        }
    }


}