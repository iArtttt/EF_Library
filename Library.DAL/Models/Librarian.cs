using Library.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Library.DAL.Models
{
    public class Librarian : IUser
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Login { get; set; } = null!;
        
        [MaxLength(100)]
        public string Password { get; set; } = null!;
        
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        public ICollection<Reader> Readers { get; set; } = new List<Reader>();

    }
}
