using Microsoft.EntityFrameworkCore;
using Model.Context;
using Model.Entites;

namespace BookStore.Repositories
{

    public interface IAuthorRepository
    {
        Task<Author> GetById(int id);
    }

    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookstoreContext _context;

        public AuthorRepository(BookstoreContext context)
        {
            _context = context;
        }

        public async Task<Author> GetById(int id)
        {
            var author = await _context.Authors!.FirstOrDefaultAsync(a => a.AuthorId == id);
            return author!;
        }

    }
}
