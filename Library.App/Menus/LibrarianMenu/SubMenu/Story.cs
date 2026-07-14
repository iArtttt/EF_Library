using Library.App.ConsoleHelper;
using Library.Shared.Attributes;

namespace Library.App.Menus.LibrarianMenu.SubMenu
{
    internal class Story
    {
        [MenuAction("Debtors", 0, "Show only debtors and their debt(s)")]
        public void Debtors()
        {
            Console.Clear();
            "=== Library Debtors Report ===".WriteLineError();
            "_".PadRight(Console.BufferWidth, '_').WriteLineUnknownDark();

            var debtors = BorrowHelper.GetActiveDebtors();

            if (debtors.Count == 0)
            {
                "\nExcellent! There are no debtors in the library right now.".WriteLineSuccess();
            }
            else
            {
                var groupedDebtors = debtors.GroupBy(d => d.Reader);

                foreach (var group in groupedDebtors)
                {
                    var reader = group.Key;
                    $"\nReader: {reader.LastName} {reader.Name} [ID: {reader.Id}] | Contact: {reader.Email ?? "No Email"}".WriteLineInfo();
                    "Overdue Book(s):".WriteLineError();

                    foreach (var loan in group)
                    {
                        int overdueDays = (DateTime.Now - loan.ToReturn).Days;
                        $"\t• '{loan.Book.Name}' (Deadline was: {loan.ToReturn.ToShortDateString()} | Overdue by {overdueDays} days)".WriteLineError();
                    }
                }
            }

            "\nPress any key to return...".WriteInfoDark();
            Console.ReadKey(true);
        }
        [MenuAction("Full Story", 1, "Shows everyone who has ever borrowed a book")]
        public void FullStory()
        {
            Console.Clear();
            "=== Global Library Loan History ===".WriteLineInfo();
            "_".PadRight(Console.BufferWidth, '_').WriteLineUnknownDark();

            var history = BorrowHelper.GetGlobalLoanHistory();

            if (history.Count == 0)
            {
                "No book loan transactions have been registered yet.".WriteLineInfoDark();
            }
            else
            {
                foreach (var loan in history)
                {

                    string dateInfo = loan.IsReturned
                        ? $"Taken: {loan.Taken.ToShortDateString()} | Returned"
                        : $"Taken: {loan.Taken.ToShortDateString()} | Deadline: {loan.ToReturn.ToShortDateString()}";

                    if (loan.IsReturned)
                        "[RETURNED] ".WriteError();
                    else
                        "[ACTIVE] ".WriteSuccess();
                    $"Book: '{loan.Book.Name}'".WriteLineInfo();
                    $"\tIssued to: {loan.Reader.LastName} {loan.Reader.Name} [ID: {loan.Reader.Id}]".WriteLineInfoDark();
                    $"\tTimeline: {dateInfo}\n".WriteLineUnknownDark();
                }
            }

            "\nPress any key to return...".WriteInfoDark();
            Console.ReadKey(true);
        }
        [MenuAction("Current Story", 2, "Shows story of a specific reader")]
        public void CurrentStory()
        {
            Console.Clear();

            var selectedReader = ReaderHelper.FindReader();
            if (selectedReader == null) return;

            Console.Clear();
            $"=== Loan History for: {selectedReader.LastName} {selectedReader.Name} ===".WriteLineInfo();
            "_".PadRight(Console.BufferWidth, '_').WriteLineUnknownDark();

            var readerHistory = selectedReader.GetBorrowedBooks();

            if (readerHistory == null || readerHistory.Count == 0)
            {
                "This reader has never borrowed any books from the library.".WriteLineInfoDark();
            }
            else
            {
                int totalOverdueCount = readerHistory.Count(l => !l.IsReturned && l.ToReturn < DateTime.Now);

                $"Total Books Ever Borrowed: {readerHistory.Count}".WriteLineInfo();
                $"Current Overdue Violations: {totalOverdueCount}".WriteLineError();
                "--------------------------------------------------".WriteLineUnknownDark();

                foreach (var loan in readerHistory)
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
    }
}
