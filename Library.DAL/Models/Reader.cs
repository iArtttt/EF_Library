using Library.Shared.Enums;
using Library.Shared.Interfaces;

namespace Library.DAL.Models
{
    public class Reader : IUser, IPerson, IDocument
    {
        public int Id { get ; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string DocumentNumber { get; set; } = null!;
        public DocumentType DocumentType { get; set; }
    }
}