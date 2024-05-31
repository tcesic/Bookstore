using AutoMapper;
using BookStore.DTOModels;
using BookStore.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Context;
using Model.Entites;

namespace Bookstore.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _bookMapper;

        public BooksController(IMapper bookMapper, IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookMapper = bookMapper;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        // GET: api/Books
        [HttpGet]
        [Authorize(Policy = "ClientCredentialsPolicy")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _bookRepository.GetAll();
            var bookResponses = _bookMapper.Map<IEnumerable<BookResponse>>(books);
            return Ok(bookResponses);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        [Authorize(Policy = "ClientCredentialsPolicy")]
        public async Task<ActionResult<BookResponse>> GetBook(int id)
        {
            var book = await _bookRepository.GetById(id);

            if (book == null)
            {
                return NotFound();
            }

            var bookResponse = _bookMapper.Map<BookResponse>(book);
            return Ok(bookResponse);
        }

        // POST: api/Books
        [HttpPost]
        [Authorize(Policy = "ClientCredentialsPolicy")]
        public async Task<ActionResult<BookResponse>> PostBook([FromBody] CreateBookRequest book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            var bookEntity = _bookMapper.Map<Book>(book);
            await _bookRepository.AddAsync(bookEntity);
            bookEntity.Author = await _authorRepository.GetById(bookEntity.AuthorId);

            var bookResponse = _bookMapper.Map<BookResponse>(bookEntity);

            return CreatedAtAction(nameof(GetBook), new { id = bookEntity.BookId }, bookResponse);
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        [Authorize(Policy = "ClientCredentialsPolicy")]
        public async Task<IActionResult> PutBook(int id, [FromBody] UpdateBookRequest book)
        {
            if (book == null)
                return BadRequest();

            var existingBook = await _bookRepository.GetById(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            _bookMapper.Map(book, existingBook);

            await _bookRepository.UpdateAsync(existingBook);
            return NoContent();
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "ClientCredentialsPolicy")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookRepository.DeleteAsync(book);

            return NoContent();
        }

        // GET: api/Books/search?query=someTitle
        [HttpGet("search")]
        //[Authorize(Policy = "ImplicitPolicy")]
        public async Task<ActionResult<IEnumerable<BookResponse>>> SearchBooks([FromQuery] SearchParametersRequest searchParameters)
        {
            var books = await _bookRepository.SearchAsync(searchParameters.Title!, searchParameters.Author!,
                                                          searchParameters.PageNumber, searchParameters.PageSize);

            var bookResponses = _bookMapper.Map<IEnumerable<BookResponse>>(books);
            return Ok(bookResponses);
        }
    }
}