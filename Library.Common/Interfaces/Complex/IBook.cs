using Library.Shared.Enums;

namespace Library.Shared.Interfaces.Complex
{
    public interface IBook : IName
    {
        public int Count { get; set; }
        public Genre Genre { get; set; }
        public IEnumerable<IAuthor> Authors { get; }
        public int PublisherTypeId { get; set; }
        public IPublisherCodeType PublisherType { get; set; }
        public DateTime PublishYear { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }
}
