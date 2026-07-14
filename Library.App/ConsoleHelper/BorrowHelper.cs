using Library.DAL;
using Library.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.App.ConsoleHelper
{
    /// <summary>
    /// Static helper class managing the business logic and transactions for borrowing and returning books.
    /// </summary>
    public static class BorrowHelper
    {
        /// <summary>
        /// Retrieves all active loans that have exceeded their return deadline.
        /// </summary>
        public static List<BorrowedBook> GetActiveDebtors()
        {
            using var context = new LibraryContext(DbConfig.Options);

            return context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .Where(b => !b.IsReturned && b.ToReturn < DateTime.Now) 
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Retrieves the entire historical log of all book loans ever issued in the library.
        /// </summary>
        public static List<BorrowedBook> GetGlobalLoanHistory()
        {
            using var context = new LibraryContext(DbConfig.Options);

            return context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .OrderByDescending(b => b.Taken) 
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Retrieves the entire borrowing history for the specified reader, including current and past loans.
        /// </summary>
        /// <param name="reader">The reader entity whose loan history is being requested.</param>
        /// <returns>A list of <see cref="BorrowedBook"/> records associated with the reader.</returns>
        public static List<BorrowedBook> GetBorrowedBooks(this Reader reader)
        {
            using var context = new LibraryContext(DbConfig.Options);

            return context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .Where(b => b.ReaderId == reader.Id)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Executes an atomic database transaction to return a borrowed book back to the library inventory.
        /// </summary>
        /// <param name="loanId">The primary key ID of the BorrowedBook record to update.</param>
        /// <returns><c>true</c> if the book was successfully returned; <c>false</c> if the record was not found or already returned.</returns>
        public static bool ReturnBook(int loanId)
        {
            using var context = new LibraryContext(DbConfig.Options);

            var loan = context.BorrowedBooks.Include(l => l.Book).FirstOrDefault(l => l.Id == loanId);

            if (loan == null || loan.IsReturned)
                return false;

            loan.IsReturned = true;

            // Safely increase the inventory count back on the shelves
            if (loan.Book != null)
            {
                loan.Book.Count++;
            }

            context.SaveChanges();
            return true;
        }


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

            Reader? selectedReader = ReaderHelper.FindReader(null, "Which of them want to take the book?");
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
