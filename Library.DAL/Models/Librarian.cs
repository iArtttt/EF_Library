using Library.Shared.Abstractions;

namespace Library.DAL.Models
{
    public class Librarian : User
    {
        public ICollection<BorrowedBook> ManagedBorrows { get; set; } = new List<BorrowedBook>();

    }
}
