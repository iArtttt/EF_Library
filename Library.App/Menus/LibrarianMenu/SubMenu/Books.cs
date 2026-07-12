using Library.App.ConsoleHelper;
using Library.DAL;
using Library.DAL.Models;
using Library.Shared.Attributes;
using Library.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Library.App.Menus.LibrarianMenu.SubMenu
{
    internal class Books
    {
        [MenuAction("Add Book", 0, "Add new book to Library")]
        public void Add()
        {
            string bookName = BookHelper.BookNameSet()??"Unnamed book";
            Genre genre = BookHelper.GenreSet();
            var publishDate = BookHelper.DateSet()??DateTime.Today;
            var country = BookHelper.CountrySet();
            var city = BookHelper.CitySet();
            var count = BookHelper.CountSet();

            using (var context = new LibraryContext(DbConfig.Options))
            {
                List<Author> existingAuthors = context.Autors.ToList();
                List<PublisherCodeType> existingPublisherCodes = context.PublishingCodeTypes.ToList();
                
                var authors = BookHelper.AuthorsSet(existingAuthors);
                foreach (var author in authors)
                {
                    context.Autors.Attach(author);
                }

                var publisherCodeType = BookHelper.PublishCodeSet(existingPublisherCodes);
                
                Book newBook = new Book()
                {
                    Name = bookName,
                    PublisherType = publisherCodeType!,
                    Authors = authors!,
                    PublishYear = publishDate,
                    Genre = genre,
                    Country = country,
                    City = city,
                    Count = count,
                };

                context.Books.Add(newBook);
                context.SaveChanges();
            }
            Console.Clear();
            $"Success: Book '{bookName}' has been added to the library catalog!".WriteLineSuccess();
            Console.ReadKey(true);
        }

        [MenuAction("Update", 0, "Updates existing book(s) in Library")]
        public void Update()
        {

            var selectedBook = BookHelper.GetBookFromMenu();

            if (selectedBook == null)
            {
                "No books were found".WriteErrorDark();
                return;
            }
            using (var context = new LibraryContext(DbConfig.Options))
            {
                var dbBook = context.Books.Include(b => b.Authors)
                                          .Include(b => b.PublisherType)
                                          .FirstOrDefault(b => b.Id == selectedBook.Id);

                if (dbBook == null) return;

                var existingAuthors = context.Autors.ToList();
                var existingPublisherCodes = context.PublishingCodeTypes.ToList();

                var updateOptions = new List<UpdateCommand>
                {
                    new UpdateCommand("Book Name", () => {
                        string? newName = BookHelper.BookNameSet();
                        if (newName != null) dbBook.Name = newName;
                    }),
                    new UpdateCommand("Genre", () => { dbBook.Genre = BookHelper.GenreSet(); }),
                    new UpdateCommand("Count", () => { dbBook.Count = BookHelper.CountSet(); }),
                    new UpdateCommand("Publish Date", () => {
                        DateTime? newDate = BookHelper.DateSet();
                        if (newDate.HasValue) dbBook.PublishYear = newDate.Value;
                    }),
                    new UpdateCommand("Country", () => { dbBook.Country = BookHelper.CountrySet(); }),
                    new UpdateCommand("City", () => { dbBook.City = BookHelper.CitySet(); }),
                    new UpdateCommand("Authors", () => {
                        var rawAuthors = BookHelper.AuthorsSet(existingAuthors);
                        if (rawAuthors == null) return; 
                        dbBook.Authors.Clear();
                        foreach (var author in rawAuthors)
                        {
                            var dbAuthor = context.Autors.Find(author.Id);
                            if (dbAuthor != null) dbBook.Authors.Add(dbAuthor);
                        }
                    }),
                    new UpdateCommand("Publisher Code", () => {
                        var rawCode = BookHelper.PublishCodeSet(existingPublisherCodes);
                        var dbCode = context.PublishingCodeTypes.Find(rawCode?.Id);
                        if (dbCode != null) dbBook.PublisherType = dbCode;
                    })
                };

                var fieldsSelector = new HelpMenu<UpdateCommand>($"What do you want to change in '{dbBook.Name}'?", updateOptions);
                var selectedCommands = fieldsSelector.MultiSelect();

                if (selectedCommands.Count == 0) return;

                foreach (var command in selectedCommands)
                {
                    if (command != null)
                    {
                        Console.Clear();
                        $"[Editing {command.Name}]".WriteLineInfo();
                        command.Action.Invoke();
                    }
                }

                context.SaveChanges();
            }

            Console.Clear();
            "Book changes successfully saved to database!".WriteLineSuccess();
            Console.ReadKey(true);


        }

        [MenuAction("Remove", 0, "Removes existing book(s) from Library")]
        public void Remove()
        {
            var selectedBook = BookHelper.GetBookFromMenu(null, "Select Book you want to REMOVE");

            if (selectedBook == null) return;

            var genres = selectedBook.Genre.ToString();
            var authors = string.Join(", ", selectedBook.Authors.Select(a => $"{a.SecondName ?? $"{a.Name} {a.LastName}"}"));
            
            $"Name: {selectedBook.Name} ".WriteLineInfo();
            $"Genre(s): {genres} ".WriteLineInfo();
            $"Author(s): {authors} ".WriteLineInfo();
            "Is this correct book? (Press 'Y' to delete, any other key to cancel)".WriteLineError();
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Y)
            {
                using (var context = new LibraryContext(DbConfig.Options))
                {
                    var bookToDelete = context.Books.Find(selectedBook.Id);
                    if (bookToDelete != null)
                    {
                        context.Books.Remove(bookToDelete);
                        context.SaveChanges();
                        $"\nBook '{selectedBook.Name}' was successfully deleted!".WriteLineSuccess();
                    }
                    else
                    {
                        "\nError: Book was already removed by another session.".WriteLineError();
                    }
                }
            }
            else
            {
                "Deletion cancelled.".WriteLineInfoDark();
            }

            "\nPress any key to return...".WriteInfoDark();
            Console.ReadKey(true);
        }
    }
}
