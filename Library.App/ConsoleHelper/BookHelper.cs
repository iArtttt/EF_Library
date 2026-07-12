using Library.DAL;
using Library.DAL.Models;
using Library.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.App.ConsoleHelper
{
    internal static class BookHelper
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


        public static void BorrowBook()
        {
            Book? selectedBook = GetBookFromMenu();
            if (selectedBook == null) return;

            Reader? selectedReader = ReaderHelper.GetReader();
            if (selectedReader == null) return;

            ExecuteBorrowTransaction(selectedReader.Id, selectedBook.Id, selectedBook.Name, selectedBook.ReturnedDays);
        }

        public static void BorrowBook(Reader currentReader)
        {
            Book? selectedBook = GetBookFromMenu();
            if (selectedBook == null) return;

            ExecuteBorrowTransaction(currentReader.Id, selectedBook.Id, selectedBook.Name, selectedBook.ReturnedDays);
        }
        public static void BorrowBook(Book selectedBook)
        {
            if (selectedBook.Count <= 0)
            {
                $"Error: '{selectedBook.Name}' is out of stock.".WriteLineError();
                Console.ReadKey(true);
                return;
            }

            Reader? selectedReader = ReaderHelper.GetReader();
            if (selectedReader == null) return;

            ExecuteBorrowTransaction(selectedReader.Id, selectedBook.Id, selectedBook.Name, selectedBook.ReturnedDays);
        }

        private static void ExecuteBorrowTransaction(int readerId, int bookId, string bookName, int returnedDays)
        {
            Console.Clear();

            using (var context = new LibraryContext(DbConfig.Options))
            {
                var dbBook = context.Books.Find(bookId);
                var dbReader = context.Readers.Find(readerId);

                if (dbBook != null && dbReader != null)
                {
                    if (dbBook.Count <= 0)
                    {
                        $"Error: '{bookName}' is currently out of stock!".WriteLineError();
                        Console.ReadKey(true);
                        return;
                    }

                    var loan = new BorrowedBook
                    {
                        BookId = dbBook.Id,
                        ReaderId = dbReader.Id,
                        Taken = DateTime.Now,
                        ToReturn = DateTime.Now.AddDays(returnedDays),
                        IsReturned = false
                    };

                    context.BorrowedBooks.Add(loan);
                    dbBook.Count--; 

                    context.SaveChanges();

                    Console.Clear();
                    $"Success: '{dbBook.Name}' has been successfully issued to {dbReader.Name} {dbReader.LastName}!".WriteLineSuccess();
                    $"Deadline to return: {loan.ToReturn.ToShortDateString()}".WriteLineInfo();
                }
                else
                {
                    "Error: Database sync issue. Transaction aborted.".WriteLineError();
                }
            }

            Console.ReadKey(true);
        }

        public static Book? GetBookFromMenu(string? toSearchText = null, string? toSelectText = null)
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
    }
}
