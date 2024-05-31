using Microsoft.EntityFrameworkCore;
using Model.Context;
using Model.Entites;

namespace BookStore.Repositories
{
    public interface IBookRepository
    {
        Task<Book> GetById(int id);
        Task<IEnumerable<Book>> GetAll();
        Task<IEnumerable<Book>> SearchAsync(string title, string author, int pageNumber, int pageSize);
        Task AddAsync(Book book);
        Task DeleteAsync(Book book);
        Task UpdateAsync(Book book);
        bool BookExists(int id);
    }

    public class BookRepository : IBookRepository
    {
        private readonly BookstoreContext _context;

        public BookRepository(BookstoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            var books = await _context.Books!.Include(b=>b.Author).ToListAsync();
            return books;
        }

        public async Task<Book> GetById(int id)
        {
            var book = await _context.Books!.Include(b=>b.Author).FirstOrDefaultAsync(b=>b.BookId==id);
            return book!;
        }

        public async Task AddAsync(Book book)
        {
            _context.Books!.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Book book)
        {
            _context.Books!.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> SearchAsync(string title, string author, int pageNumber, int pageSize)
        {
            var query = _context.Books!.AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(b => b.Title!.Contains(title));
            }

            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(b => b.Author!.Name!.Contains(author));
            }

            // Pagination
            var books = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return books;
        }

        public bool BookExists(int id)
        {
            return _context.Books!.Any(e => e.BookId == id);
        }

       
    }
}
