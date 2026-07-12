using Library.App.ConsoleHelper;
using Library.DAL;
using Library.DAL.Models;
using Library.Shared.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Library.App.Menus.LibrarianMenu.SubMenu
{
    internal class Search
    {
        [MenuAction("Title", 0, "Search books by name")]
        public void ByTitle()
        {
            Console.Clear();
            string? toSearch = "Please write Book Name you want to find (Enter to show All): ".Read(ConsoleColor.Yellow);
            List<Book> books = FindBooks(toSearch);

            if (books.Count == 0)
            {
                "No books found matching your criteria.".WriteLineError();
                "\nPress any key to return...".WriteInfoDark();
                Console.ReadKey(true);
                return;
            }
            var searchResultsMenu = new HelpMenu<Book>(
                $"Search Results for '{toSearch}' (Select a book to Borrow)",
                books.OrderBy(b => b.Name)
                );

            Book? selectedBook = searchResultsMenu.Select();

            if (selectedBook != null)
            {
                BookHelper.BorrowBook(selectedBook);
            }

        }
        [MenuAction("Author", 0, "Search books by Author Name")]
        public void ByAuthor()
        {
            Console.Clear();
            string? toSearch = "Please write Author Name or Lastname you want to find: ".Read(ConsoleColor.Yellow);

            if (string.IsNullOrEmpty(toSearch))
            {
                "Author name cannot be empty!".WriteLineError();
                "\nPress any key to return...".WriteInfoDark();
                Console.ReadKey(true);
                return;
            }

            List<Book> books = FindBooks(toSearch);

            if (books.Count == 0)
            {
                $"No books found for author matching '{toSearch}'.".WriteLineError();
                "\nPress any key to return...".WriteInfoDark();
                Console.ReadKey(true);
                return;
            }

            var searchResultsMenu = new HelpMenu<Book>(
                $"Books found for Author '{toSearch}' (Select to Borrow)",
                book => book,
                books.OrderBy(b => b.Name)
            );

            Book? selectedBook = searchResultsMenu.Select();

            if (selectedBook != null)
            {
                BookHelper.BorrowBook(selectedBook);
            }
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
