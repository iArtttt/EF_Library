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

            using (var context = new LibraryContext(DbConfig.Options))
            {
                if (string.IsNullOrEmpty(toSearch))
                    books = context.Books.Include(b => b.Authors).ToList();
                else
                    books = context.Books.Where(b => b.Name.Contains(toSearch)).Include(b => b.Authors).ToList();
            }
            foreach (var book in books)
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

            List<Author> authors;

            using (var context = new LibraryContext(DbConfig.Options))
            {
                if (string.IsNullOrEmpty(toSearch))
                    authors = context.Autors.Include(a => a.Books).ToList();
                else
                    authors = context.Autors.Where(a => a.Name.Contains(toSearch) || a.LastName.Contains(toSearch)).Include(b => b.Books).ToList();
            }
            foreach (var author in authors)
            {
                string booksList = string.Join(";\n\t", author.Books.Select(a => a.Name));
                $"Name: {author.Name} {author.LastName}\nBook(s):".WriteLineInfo();
                $"\t{booksList};".WriteLineSuccessDark();
                $"Total Books was Writen: {author.Books.Count}\n".WriteLineInfo();
            }

            "\nPress any key to return...".WriteInfoDark();
            Console.ReadKey(true);
        }
        
    }
}
