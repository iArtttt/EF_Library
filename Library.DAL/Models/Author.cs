using Library.Shared.Interfaces.Complex;

namespace Library.DAL.Models
{
    public class Author : IAuthor
    {
        public int Id {  get; set; }
        
        public string Name { get; set; } = null!;
        
        public string LastName { get; set; } = null!;
        
        public string? SecondName { get; set; }
        
        public ICollection<Book> Books { get; set; } = new List<Book>();

        IEnumerable<IBook> IAuthor.Books => Books;
    }
}
