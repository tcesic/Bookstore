using Microsoft.EntityFrameworkCore;
using Model.Entites;

namespace Model.Context
{
    public class BookstoreContext : DbContext
    {
        public BookstoreContext(DbContextOptions<BookstoreContext> options) : base(options)
        {
        }


        public DbSet<Book>? Books { get; set; }
        public DbSet<Author>? Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Book>()
               .ToTable("Book")
               .HasKey(x => x.BookId);

            builder.Entity<Book>()
               .Property(b => b.Title)
               .IsRequired()
               .HasMaxLength(100);

            builder.Entity<Book>()
                .Property(b => b.SubTitle);

            builder.Entity<Author>()
                .ToTable("Author")
                .HasKey(x => x.AuthorId);

            builder.Entity<Author>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);           

            builder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

        }

    }
}
