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
            List<Book> books;

            books = FindBooks(toSearch);
            foreach (var book in books.OrderBy(b =>b.Name))
            {
                $"Name: {book.Name}\t|\tAuthor(s): {string.Join(", ", book.Authors.Select(a => a.Name))}\t|\tCount: {book.Count}".WriteLineInfo();
            }

            "\nPress any key to return...".WriteInfoDark();
            Console.ReadKey(true);
        }
        [MenuAction("Author", 0, "Search books by Author Name")]
        public void ByAuthor()
        {
            Console.Clear();
            string toSearch = "Please write Author Name you want to find: ".Read(ConsoleColor.Yellow);

            var books = FindBooks(toSearch);
            var authors = books.SelectMany(b => b.Authors.Select(a => a)).Distinct().ToDictionary(a => a, a => a.Books);

            foreach (var author in authors)
            {
                string booksList = string.Join(";\n\t", author.Value.Select(b => b.Name));
                $"Name: {author.Key.Name} {author.Key.LastName} {author.Key.SecondName??$"Pseudonym: {author.Key.SecondName}"}\nBook(s):".WriteLineInfo();
                $"\t{booksList};".WriteLineSuccessDark();
                $"Total Books was Writen: {author.Value.Count}\n".WriteLineInfo();
            }

            "\nPress any key to return...".WriteInfoDark();
            Console.ReadKey(true);
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
