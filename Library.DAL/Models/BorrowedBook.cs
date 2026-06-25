using Library.Shared.Interfaces;

namespace Library.DAL.Models
{
    public class BorrowedBook : IID
    {
        public int Id { get; set; }

        public int LibrarianId { get; set; }
        
        public Librarian Librarian { get; set; } = null!;

        public int BookId { get; set; }
        
        public Book Book { get; set; } = null!;

        public int ReaderId { get; set; }

        public Reader Reader { get; set; } = null!;

        public DateTime Taken { get; set; } = DateTime.UtcNow;

        public DateTime? WhenReturned { get; set; }

        public DateTime ToReturn { get; set; }

        public bool IsReturned { get; set; }
    }
}
