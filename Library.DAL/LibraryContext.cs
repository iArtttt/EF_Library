using Library.DAL.Models;
using Library.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL
{
    public class LibraryContext : DbContext
    {
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Author> Autors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<PublisherCodeType> PublishingCodeTypes { get; set; }
        //public DbSet<BorrowedBook> BorrowedBooks { get; set; }
        public LibraryContext(DbContextOptions optionsBuilder)
            : base(optionsBuilder)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Librarian>()
                .HasData(
                new Librarian() { Id = 1, Login = "Admin", Password = "1234", Email = "admin@gmail.com" },
                new Librarian() { Id = 2, Login = "Admin1", Password = "4567", Email = "admin1@gmail.com" }
                );

            modelBuilder.Entity<PublisherCodeType>().HasData(
                new PublisherCodeType { Id = 1, PublisherCode = "ISBN" },
                new PublisherCodeType { Id = 2, PublisherCode = "ISSN" },
                new PublisherCodeType { Id = 3, PublisherCode = "ISRC" },
                new PublisherCodeType { Id = 4, PublisherCode = "ISWC" }
                );

            modelBuilder.Entity<Reader>().HasData(
               new Reader
               {
                   Id = 1,
                   Login = "Reader",
                   Password = "1234",
                   Email = "reader@gmail.com",
                   Name = "Bob",
                   LastName = "Lighter",
                   Birthday = DateTime.Today,
                   DocumentType = DocumentType.DrivingLicence,
                   DocumentNumber = "3354213",
               },
               new Reader
               {
                   Id = 2,
                   Login = "Reader1",
                   Password = "1423",
                   Email = "rEAr@gmail.com",
                   Name = "Alex",
                   LastName = "Zeroph",
                   Birthday = DateTime.Parse("18.04.1993"),
                   DocumentType = DocumentType.Passport,
                   DocumentNumber = "777789",
               }
               );


            var autor1 = new Author { Id = 1, Name = "Oleg", LastName = "Fiom" };
            var autor2 = new Author { Id = 2, Name = "Ivan", LastName = "Syropin", SecondName = "Grozniy" };
            var autor3 = new Author { Id = 3, Name = "Vasiliy", LastName = "Syropin", SecondName = "Krot" };
            
            modelBuilder.Entity<Author>().HasData(autor1, autor2, autor3);



            var book1 = new Book { Id = 1, Name = "C# for smart", Genre = Genre.Learning, Country = "Ukrain", City = "Kharkiv", PublisherTypeId = 1, Count = 2 };
            var book2 = new Book { Id = 2, Name = "World Story", Genre = Genre.History, Country = "Ukrain", City = "Kiev", PublisherTypeId = 2, Count = 1 };
            var book3 = new Book { Id = 3, Name = "Summer Time", Genre = Genre.Novel, Country = "Ukrain", City = "Kiev", PublisherTypeId = 3, Count = 5 };
            var book4 = new Book { Id = 4, Name = "Mgla", Genre = Genre.Horror, Country = "Poland", PublisherTypeId = 1, Count = 2 };
            
            modelBuilder.Entity<Book>().HasData( book1, book2, book3, book4 );

            modelBuilder.Entity("AuthorBook")
                .HasData(
                    new { AuthorsId = autor1.Id, BooksId = book1.Id },
                    new { AuthorsId = autor1.Id, BooksId = book2.Id },
                    new { AuthorsId = autor2.Id, BooksId = book2.Id },
                    new { AuthorsId = autor3.Id, BooksId = book3.Id },
                    new { AuthorsId = autor3.Id, BooksId = book4.Id }
                );



            base.OnModelCreating(modelBuilder);
        }
    }
}
