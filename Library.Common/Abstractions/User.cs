using Library.Shared.Interfaces.DAL;
using System.ComponentModel.DataAnnotations;

namespace Library.Shared.Abstractions
{
    public abstract class User : IUser
    {
        public int Id {  get; set; }

        [Required]
        [StringLength(100)]
        public string Login { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
    }
}
