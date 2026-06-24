using Library.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Library.DAL.Models
{
    public class Librarian : IUser
    {
        [Key] 
        public int Id { get; set; }

        [Key]
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
