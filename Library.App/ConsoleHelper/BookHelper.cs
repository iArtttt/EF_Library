using Library.App.Menus.LibrarianMenu.SubMenu;
using Library.DAL;
using Library.DAL.Models;
using Library.Shared.Enums;

namespace Library.App.ConsoleHelper
{
    internal static class BookHelper
    {

        public static string? BookNameSet()
        {
            string? bookName = "Write the new book Name: ".Read(ConsoleColor.Yellow);
            if (string.IsNullOrEmpty(bookName))
            {
                "Incorrect Name".WriteLineError();
                return null;
            }
            return bookName;
        }

        public static string? CountrySet() => "Write Country where book from: ".Read(ConsoleColor.Yellow);
        public static string? CitySet() => "Write City where book from: ".Read(ConsoleColor.Yellow);
        public static int CountSet()
        {
            var stringCount = "How much books was arrived? ".Read(ConsoleColor.Yellow);
            if (!int.TryParse(stringCount, out int count) || count < 1)
            {
                count = 1;
            }

            return count;
        }
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

        public static DateTime? DateSet()
        {
            int currentYear = DateTime.Now.Year;
            var yearsRange = Enumerable.Range(1880, currentYear - 1880 + 1).Reverse().ToList();

            var yearPicker = new HelpMenu<int>("Select Publish Year", yearsRange);
            int? selectedYear = yearPicker.Select();

            if (selectedYear == null || selectedYear == 0) return null;


            var monthsRange = Enumerable.Range(1, 12).ToList();
            var monthPicker = new HelpMenu<int>($"Select Month (Year: {selectedYear})", monthsRange);
            int? selectedMonth = monthPicker.Select();

            if (selectedMonth == null || selectedMonth == 0) return null;


            int daysInMonth = DateTime.DaysInMonth(selectedYear.Value, selectedMonth.Value);
            var daysRange = Enumerable.Range(1, daysInMonth).ToList();

            var dayPicker = new HelpMenu<int>($"Select Day (Date: {selectedMonth}/{selectedYear})", daysRange);
            int? selectedDay = dayPicker.Select();

            if (selectedDay == null || selectedDay == 0) return null;

            DateTime finalPublishDate = new DateTime(selectedYear.Value, selectedMonth.Value, selectedDay.Value);
            return finalPublishDate;
        }
        public static void BorrowBook(Book? bookToBorrow = null)
        {
            Console.Clear();

            Book? selectedBook = bookToBorrow ?? GetBookFromMenu(); 

            if (selectedBook == null) return;

            List<Reader> activeReaders;
            using (var context = new LibraryContext(DbConfig.Options))
            {
                activeReaders = context.Readers.ToList();
            }

            Reader? selectedReader = ReaderHelper.ReaderSelect(activeReaders);
            if (selectedReader == null) return; 

            using (var context = new LibraryContext(DbConfig.Options))
            {
                var dbBook = context.Books.Find(selectedBook.Id);
                var dbReader = context.Readers.Find(selectedReader.Id);

                if (dbBook != null && dbReader != null)
                {
                    var loan = new BorrowedBook
                    {
                        BookId = dbBook.Id,
                        ReaderId = dbReader.Id,
                        Taken = DateTime.Now,
                        ToReturn = DateTime.Now.AddDays(dbBook.ReturnedDays),
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
        private static Book? GetBookFromMenu()
        {
            string? toSearch = "Write Book Name or Author to issue: ".Read(ConsoleColor.Yellow);
            List<Book> foundBooks = Search.FindBooks(toSearch);
            var availableBooks = foundBooks.Where(b => b.Count > 0).ToList();

            var bookSelector = new HelpMenu<Book>("Select Book to Issue", availableBooks);
            return bookSelector.Select();
        }
    }
}
