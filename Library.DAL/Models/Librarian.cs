using Library.Shared.Abstraction;

namespace Library.DAL.Models
{
    public class Librarian : User
    {
        public ICollection<BorrowedBook> ManagedBorrows { get; set; } = new List<BorrowedBook>();

    }
}
