using Library.DAL.Models;
using Library.Shared.Abstraction;
using Library.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL
{
    public class LibraryContext : DbContext
    {
        public DbSet<Librarian> Librarians { get; set; } = null!; 
        public DbSet<Reader> Readers { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Author> Autors { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<PublisherCodeType> PublishingCodeTypes { get; set; } = null!;
        public DbSet<BorrowedBook> BorrowedBooks { get; set; } = null!;
        public LibraryContext(DbContextOptions<LibraryContext> optionsBuilder)
            : base(optionsBuilder)
        {
            //Database.EnsureCreated();
        }
        public LibraryContext()
        {
            
        }
        //  
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LibraryDB;Integrated Security=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PublisherCodeType>().HasData(
                new PublisherCodeType { Id = 1, PublisherCode = "ISBN" },
                new PublisherCodeType { Id = 2, PublisherCode = "ISSN" },
                new PublisherCodeType { Id = 3, PublisherCode = "ISRC" },
                new PublisherCodeType { Id = 4, PublisherCode = "ISWC" }
                );
            
            modelBuilder.Entity<Reader>().ToTable("Readers");
            modelBuilder.Entity<Librarian>().ToTable("Librarians");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            //modelBuilder.Entity<User>().HasData(
                
            //    new { Id = -1 },
            //    new { Id = -2 },
            //    new { Id = -3 },
            //    new { Id = -4, }

            //    );


            modelBuilder.Entity<Librarian>()
                .HasData(
                new Librarian() { Id = -1, Login = "Admin", Password = "1234", Email = "admin@gmail.com" },
                new Librarian() { Id = -2, Login = "Admin1", Password = "4567", Email = "admin1@gmail.com" }
                );


            modelBuilder.Entity<Reader>().HasData(
               new Reader
               {
                   Id = -3,
                   
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
                   Id = -4,
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
