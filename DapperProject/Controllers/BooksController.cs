using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IConfiguration config;

        public BooksController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Books>>> GetAllBooks()
        {
            using var connection = new SqlConnection(this.config.GetConnectionString("DefaultConnection"));
            IEnumerable<Books> books = await SelectAllBooks(connection);
            return Ok(books);
        }

       

        [HttpGet("{bookId}")]
        public async Task<ActionResult<List<Books>>> GetBook(int bookId)
        {
            using var connection = new SqlConnection(this.config.GetConnectionString("DefaultConnection"));
            var book = await connection.QueryFirstAsync<Books>("select * from Book where id = @Id",
                new { Id = bookId });
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<List<Books>>> CreateBook(Books abook)
        {
            using var connection = new SqlConnection(this.config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into Book1 (title, author, year) values (@Title, @Author, @Year)",abook);
            return Ok(await SelectAllBooks(connection));
        }

        [HttpPut]
        public async Task<ActionResult<List<Books>>> UpdateBook(Books abook)
        {
            using var connection = new SqlConnection(this.config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update Book1 set title = @Title, author = @Author, year = @Year where id = @Id",abook);
            return Ok(await SelectAllBooks(connection));
        }

        [HttpDelete("{bookId}")]
        public async Task<ActionResult<List<Books>>> DeleteBook(int bookId)
        {
            using var connection = new SqlConnection(this.config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from Book1 where id = @Id", new { Id = bookId });
            return Ok(await SelectAllBooks(connection));
        }

        private static async Task<IEnumerable<Books>> SelectAllBooks(SqlConnection connection)
        {
            return await connection.QueryAsync<Books>("select * from Book1");
        }
    }
}
