using Microsoft.AspNetCore.Mvc;
using Library.Infrastructure.Models;
using Library.Infrastructure.Services;
using LibraryREST.Models;

namespace LibraryREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ICrudServiceAsync<BookModel> _bookService;

        public BooksController(ICrudServiceAsync<BookModel> bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookApiModel>>> GetAll()
        {
            var books = await _bookService.ReadAllAsync();
            var result = books.Select(book => new BookApiModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author
            });

            return Ok(result);
        }

        // GET: api/Books/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookApiModel>> Get(int id)
        {
            var book = await _bookService.ReadAsync(id);
            if (book == null)
                return NotFound();

            var dto = new BookApiModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author
            };

            return Ok(dto);
        }

        // POST: api/Books
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookCreateModel model)
        {
            var newBook = new BookModel
            {
                Title = model.Title,
                Author = model.Author,
                LibraryMemberId = model.LibraryMemberId,
                BookTags = new List<BookTagModel>()
            };

            var created = await _bookService.CreateAsync(newBook);
            if (!created) return BadRequest("Could not create book.");

            await _bookService.SaveAsync();

            return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
        }

        // PUT: api/Books/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateModel model)
        {
            var book = await _bookService.ReadAsync(id);
            if (book == null)
                return NotFound();

            book.Title = model.Title;
            book.Author = model.Author;
            book.LibraryMemberId = model.LibraryMemberId;

            var updated = await _bookService.UpdateAsync(book);
            if (!updated) return BadRequest("Update failed.");

            await _bookService.SaveAsync();

            return NoContent();
        }

        // DELETE: api/Books/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.ReadAsync(id);
            if (book == null)
                return NotFound();

            var deleted = await _bookService.RemoveAsync(book);
            if (!deleted) return BadRequest("Delete failed.");

            await _bookService.SaveAsync();

            return NoContent();
        }
    }
}
