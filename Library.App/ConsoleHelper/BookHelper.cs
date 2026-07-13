using Library.DAL;
using Library.DAL.Models;
using Library.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.App.ConsoleHelper
{
    public static class BookHelper
    {

        public static string? BookNameSet() => BaseHelper.GetString("Write the new book Name: ");
        public static string? CountrySet() => BaseHelper.GetString("Write Country where book from: ");
        public static string? CitySet() => BaseHelper.GetString("Write City where book from: ");
        public static int CountSet() => BaseHelper.GetInt("How much books was arrived? ");
        public static DateTime? DateSet() => BaseHelper.DateSet();

        public static Genre GenreSet()
        {
            Genre genre = Genre.None;
            var existingGenres = new HelpMenu<Genre>("What genre has this book?", Enum.GetValues<Genre>());
            foreach (var item in existingGenres.MultiSelect())
            {
                genre |= item;
            }

            return genre;
        }
        public static List<Author> AuthorsSet(List<Author> existingAuthors)
        {
            var selectAuthors = new HelpMenu<Author>("Who wrote this book?", existingAuthors);
            var authors = selectAuthors.MultiSelect();

            return authors == null || authors.Count == 0
                ? new List<Author> { existingAuthors.First() }
                : authors.Where(a => a != null).ToList()!;
        }
        public static PublisherCodeType? PublishCodeSet(List<PublisherCodeType> existingPublisherCodes)
        {
            var codesDictionary = existingPublisherCodes.ToDictionary(c => c.PublisherCode, c => c);
            var selectPublisherCodes = new HelpMenu<string>("What redaction was published by?", codesDictionary.Keys);

            var selectedPublisherCodeType = selectPublisherCodes.Select();
            if (string.IsNullOrEmpty(selectedPublisherCodeType))
            {
                "Incorrect Code type, was setted to default".WriteLineError();
                Console.ReadKey(true);
                return existingPublisherCodes.First();
            }

            return codesDictionary[selectedPublisherCodeType];
        }


        public static Book? FindBook(string? toSearchText = null, string? toSelectText = null)
        {
            string? toSearch = (toSearchText ?? "Write Book Name or Author to issue: ").Read(ConsoleColor.Yellow);
            List<Book> foundBooks = FindBooks(toSearch);
            var availableBooks = foundBooks.Where(b => b.Count > 0).ToList();

            var bookSelector = new HelpMenu<Book>(toSelectText ?? "Select Book to Issue", availableBooks);
            return bookSelector.Select();
        }
        public static List<Book> FindBooks(string? toSearch = null)
        {
            using var context = new LibraryContext(DbConfig.Options);
            var query = context.Books.Include(b => b.Authors).AsNoTracking();

            if (string.IsNullOrEmpty(toSearch))
                return query.ToList();
            else
                return query.Where(b =>
                    b.Name.Contains(toSearch) ||
                    b.Authors.Any(a => a.Name.Contains(toSearch) || a.LastName.Contains(toSearch))).ToList();

        }
        /// <summary>
        /// Updates a book entity in the database by executing a custom modification action within a secure context.
        /// </summary>
        /// <param name="bookId">The identifier of the book to update.</param>
        /// <param name="updateAction">The delegate containing specific property modifications.</param>
        /// <returns><c>true</c> if the book was found and successfully updated; otherwise, <c>false</c>.</returns>
        public static bool Update(int bookId, Action<Book, LibraryContext> updateAction)
        {
            using var context = new LibraryContext(DbConfig.Options);

            var dbBook = context.Books.Include(b => b.Authors)
                                      .Include(b => b.PublisherType)
                                      .FirstOrDefault(b => b.Id == bookId);

            if (dbBook == null) return false;

            updateAction.Invoke(dbBook, context);

            context.SaveChanges();
            return true;
        }
        public static bool Remove(Book book)
        {
            using var context = new LibraryContext(DbConfig.Options);
            var bookToDelete = context.Books.Find(book.Id);
            if (bookToDelete != null)
            {
                context.Books.Remove(bookToDelete);
                context.SaveChanges();
                $"\nBook '{book.Name}' was successfully deleted!".WriteLineSuccess();
                return true;
            }
            return false;
        }
    }
}
