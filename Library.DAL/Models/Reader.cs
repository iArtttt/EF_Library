using Library.Shared.Enums;
using Library.Shared.Interfaces.Complex;

namespace Library.DAL.Models
{
    public class Reader : IReader
    {
        public int Id { get ; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public string DocumentNumber { get; set; } = null!;
        public DocumentType DocumentType { get; set; }
    }
}