using Library.Shared.Abstraction;
using Library.Shared.Enums;
using Library.Shared.Interfaces.Complex;

namespace Library.DAL.Models
{
    public class Reader : User, IReader
    {
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public string DocumentNumber { get; set; } = null!;
        public DocumentType DocumentType { get; set; }
    }
}