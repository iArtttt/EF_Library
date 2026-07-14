using Library.App.ConsoleHelper;
using Library.DAL.Models;
using Library.Shared.Attributes;

namespace Library.App.Menus.ReaderMenu
{
    internal class ReaderLobby
    {
        [MenuAction("Find Book", 0, "Search books by Title, Author Name, Last Name or Second Name and take It if you want")]
        public void Search(Reader reader)
        {
            var book = BookHelper.FindBook();
            if (book == null)
            {
                "There is no books that confirm your reqest".WriteLineError();
                "Press any buttom to continue...".WriteLineInfo();
                Console.ReadKey(true);
                return;
            }
            book.Information();


            "Do you want to take it? (Press 'Y' to take)".WriteInfoDark();
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Y)
            {
                reader.BorrowBook(book);
            }


        }
        [MenuAction("Loaned", 1, "List of borrowed books and books that were borrowed in the past")]
        public void Loaned(Reader reader)
        {

            var readerHistory = reader.GetBorrowedBooks();

            if (readerHistory == null || readerHistory.Count == 0)
            {
                "You has never borrowed any books from the library.".WriteLineInfoDark();
            }
            else
            {
                int totalOverdueCount = readerHistory.Count(l => !l.IsReturned && l.ToReturn < DateTime.Now);

                $"Total Books Ever Borrowed: {readerHistory.Count}".WriteLineInfo();
                $"Current Overdue Violations: {totalOverdueCount}".WriteLineError();
                "--------------------------------------------------".WriteLineUnknownDark();

                var orderedHostory = readerHistory.OrderBy(d => d.IsReturned).ThenByDescending(b => b.Taken);

                foreach (var loan in orderedHostory)
                {
                    if (loan.IsReturned)
                    {
                        $"[✓] '{loan.Book.Name}' — Returned successfully.".WriteLineSuccessDark();
                    }
                    else
                    {
                        if (loan.ToReturn < DateTime.Now)
                        {
                            int days = (DateTime.Now - loan.ToReturn).Days;
                            $"[✖] '{loan.Book.Name}' — OVERDUE by {days} days! (Deadline: {loan.ToReturn.ToShortDateString()})".WriteLineError();
                        }
                        else
                        {
                            $"[->] '{loan.Book.Name}' — On hands. (Deadline: {loan.ToReturn.ToShortDateString()})".WriteLineInfoDark();
                        }
                    }
                }
            }

            "\nPress any key to return...".WriteInfoDark();
            Console.ReadKey(true);
        }
        [MenuAction("To return", 1, "Return book to the Library")]
        public void ToReturn(Reader reader)
        {

            var readerHistory = reader.GetBorrowedBooks().Where(b => !b.IsReturned).ToList();

            if (readerHistory == null || readerHistory.Count == 0)
            {
                "You has never borrowed any books from the library or all of your loans are closed.".WriteLineSuccessDark();
            }
            else
            {
                int totalOverdueCount = readerHistory.Count(l => l.ToReturn < DateTime.Now);

                $"Total Books Borrowed: {readerHistory.Count}".WriteLineInfo();
                $"Current Overdue Violations: {totalOverdueCount}".WriteLineError();
                "--------------------------------------------------".WriteLineUnknownDark();

                var orderedHostory = readerHistory.OrderByDescending(b => b.ToReturn);

                var updateCommands = new List<UpdateCommand>(); 


                foreach (var loan in orderedHostory)
                {
                    string displayLabel;
                    if (loan.ToReturn < DateTime.Now)
                    {
                        int days = (DateTime.Now - loan.ToReturn).Days;
                        displayLabel = $"[✖] '{loan.Book.Name}' — OVERDUE by {days} days! (Deadline: {loan.ToReturn.ToShortDateString()})";
                    }
                    else
                    {
                        displayLabel = $"[->] '{loan.Book.Name}' — On hands. (Deadline: {loan.ToReturn.ToShortDateString()})";
                    }

                    updateCommands.Add(new UpdateCommand(displayLabel, () =>
                    {
                        bool success = BorrowHelper.ReturnBook(loan.Id);
                        if (success)
                            $"\nSuccess: '{loan.Book.Name}' was processed and restocked.".WriteLineSuccess();
                        else
                            $"\nError: Failed to process return for '{loan.Book.Name}'.".WriteLineError();
                    }));
                }
                var helper = new HelpMenu<UpdateCommand>("Select which books you want to return (Space/Enter to check)", updateCommands);

                var booksToReturn = helper.MultiSelect();
                
                if (booksToReturn.Count > 0)
                {
                    foreach (var item in booksToReturn)
                    {
                        item?.Action.Invoke();
                    }
                    "\nOperation completed.".WriteLineSuccess();
                }

            }

            "\nPress any key to return...".WriteInfoDark();
            Console.ReadKey(true);
        }

    }
}
