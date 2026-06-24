using Library.Shared.Enums;
using Library.Shared.Interfaces;
using Library.Shared.Interfaces.Complex;

namespace Library.DAL.Models
{
    public class Book : IBook
    {
        public int Id { get; set; }
        public string Name { get; set;  } = null!;
        public Genre Genre { get; set; }
        public int Count { get; set; }
        public int PublisherTypeId { get; set; }
        public PublisherCodeType PublisherType { get; set; } = null!;
        public ICollection<Author> Authors { get; set; } = new List<Author>();
        public DateTime PublishYear { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        IPublisherCodeType IBook.PublisherType
        {
            get => PublisherType;
            set => PublisherType = (PublisherCodeType)value;
        }
        IEnumerable<IAuthor> IBook.Authors => Authors;
    }
}
