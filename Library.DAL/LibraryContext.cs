using Library.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Library.DAL
{
    public class LibraryContext : DbContext
    {
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Autor> Autors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<PublishingCodeType> PublishingCodeTypes { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<BorrowedBook> BorrowedBooks { get; set; }
        public LibraryContext(DbContextOptions optionsBuilder)
            : base(optionsBuilder)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Librarian>()
                .HasData(
                new Librarian() { Login = "Admin", Password = "1234", Email = "admin@gmail.com" },
                new Librarian() { Login = "Admin1", Password = "4567", Email = "admin1@gmail.com" }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
