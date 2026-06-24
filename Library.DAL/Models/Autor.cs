using Library.Shared.Interfaces;

namespace Library.DAL.Models
{
    public class Autor : IPerson
    {
        public int Id {  get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}
