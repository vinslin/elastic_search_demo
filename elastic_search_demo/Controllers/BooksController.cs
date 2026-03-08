using elastic_search_demo.Models;
using elastic_search_demo.Service;
using elastic_search_demo.ServiceRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace elastic_search_demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(BookRequest book)
        {
            var result = await _bookService.AddBook(book);
            if (result.Item2)
            {
                return Ok(result.Item1);
            }
            return BadRequest("Failed to index the book.");
        }

        [HttpGet("normal-search")]
        public async Task<ActionResult<List<Book>>> NormalSearch(string keyword)
        {
            var results = await _bookService.Search(keyword);
            if (results.Any())
            {
                return results;
            }
            return NotFound();
        }

        [HttpGet("ranking-search")]
        public async Task<IActionResult> RankingSearch(string keyword)
        {
            var results = await _bookService.RankingSearch(keyword);
            return Ok(results);
        }

        [HttpGet("fuzzy-search")]
        public async Task<IActionResult> FuzzySearch(string keyword)
        {
            var results = await _bookService.FuzzySearch(keyword);
            return Ok(results);
        }

        [HttpGet("highlight-search")]
        public async Task<IActionResult> HighlightSearch(string keyword)
        {
            var results = await _bookService.HighlightSearch(keyword);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(string id)
        {
            var results = await _bookService.GetAll();
            var book = results.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                return Ok(book);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var result = await _bookService.DeleteBook(id);
            if (result)
            {
                return Ok("Book deleted successfully.");
            }
            return NotFound("Book not found.");
        }

        [HttpGet("gat-all")]
        public async Task<IActionResult> GetAllBooks()
        {
            var results = await _bookService.GetAll();
            return Ok(results);
        }

        [HttpGet("unified-search")]
        public async Task<IActionResult> UnifiedSearch(string keyword)
        {
            var results = await _bookService.UnifiedSearch(keyword);
            return Ok(results);
        }
    }
}
