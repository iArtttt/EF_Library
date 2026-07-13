using Library.DAL;
using Library.DAL.Models;

namespace Library.App.ConsoleHelper
{
    /// <summary>
    /// Static helper class managing the business logic and transactions for borrowing and returning books.
    /// </summary>
    public static class BorrowHelper
    {
        /// <summary>
        /// Launches a complete interactive borrowing wizard by requesting both the book and the reader from scratch.
        /// </summary>
        public static void BorrowBook()
        {
            Book? selectedBook = BookHelper.FindBook();
            if (selectedBook == null) return;

            Reader? selectedReader = ReaderHelper.FindReader();
            if (selectedReader == null) return;

            ExecuteBorrowTransaction(selectedReader.Id, selectedBook.Id, selectedBook.Name, selectedBook.ReturnedDays);
        }

        /// <summary>
        /// Extension method to directly borrow a specific book for the given reader.
        /// </summary>
        public static void BorrowBook(this Reader currentReader, Book selectedBook)
            => ExecuteBorrowTransaction(currentReader.Id, selectedBook.Id, selectedBook.Name, selectedBook.ReturnedDays);

        /// <summary>
        /// Extension method for a reader to search and borrow a book from their workspace.
        /// </summary>
        public static void BorrowBook(this Reader currentReader)
        {
            Book? selectedBook = BookHelper.FindBook();
            if (selectedBook == null) return;

            ExecuteBorrowTransaction(currentReader.Id, selectedBook.Id, selectedBook.Name, selectedBook.ReturnedDays);
        }

        /// <summary>
        /// Extension method to issue a specific book to an interactively selected reader.
        /// </summary>
        public static void BorrowBook(this Book selectedBook)
        {
            if (selectedBook.Count <= 0)
            {
                $"Error: '{selectedBook.Name}' is out of stock.".WriteLineError();
                Console.ReadKey(true);
                return;
            }

            Reader? selectedReader = ReaderHelper.FindReader();
            if (selectedReader == null) return;

            ExecuteBorrowTransaction(selectedReader.Id, selectedBook.Id, selectedBook.Name, selectedBook.ReturnedDays);
        }

        /// <summary>
        /// Executes an atomic database transaction to register a book loan and update inventory.
        /// </summary>
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
    }
}
