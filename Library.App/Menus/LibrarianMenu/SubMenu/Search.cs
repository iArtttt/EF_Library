using Library.App.ConsoleHelper;
using Library.DAL.Models;
using Library.Shared.Attributes;

namespace Library.App.Menus.LibrarianMenu.SubMenu
{
    internal class Search
    {
        [MenuAction("Title", 0, "Search books by name")]
        public void ByTitle()
        {
            Book? selectedBook = BookHelper.GetBookFromMenu(
                "Please write Book Name you want to find: ", 
                "Found Books (Select one to Borrow)"
                );

            if (selectedBook != null)
                BookHelper.BorrowBook(selectedBook);

        }
        [MenuAction("Author", 0, "Search books by Author Name, Last Name or Second Name")]
        public void ByAuthor()
        {
            Book? selectedBook = BookHelper.GetBookFromMenu(
                "Please write Author Name or Lastname you want to find: ",
                "Books of this Author (Select one to Borrow)"
                );

            if (selectedBook != null)
                BookHelper.BorrowBook(selectedBook);
        }
        
    }
}
