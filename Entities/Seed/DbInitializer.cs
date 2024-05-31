using Microsoft.EntityFrameworkCore;
using Model.Context;
using Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Seed
{
    public static class DbInitializer
    {
        public static void Initialize(BookstoreContext context)
        {
            //context.Database.Migrate();
            context.Database.EnsureCreated();
            if (context.Books!.Any())
            {
                return;   // DB already exists
            }

            var authors = new Author[]
                {
                    new Author { Name="A"},
                    new Author {Name="B"},
                };

            var books = new Book[]
            {
                new Book {Title = "Book 1", AuthorId = 1 },
                new Book {Title = "Book 2", AuthorId = 1 },
                new Book {Title = "Book 3", AuthorId = 2 },
            };

            context.Authors!.AddRange(authors);
            context.Books!.AddRange(books);
            context.SaveChanges();
        }
    }

}
