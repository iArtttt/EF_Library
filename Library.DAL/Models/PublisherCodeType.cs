using Library.Shared.Interfaces.DAL;

namespace Library.DAL.Models
{
    public class PublisherCodeType : IPublisherCodeType
    {
        public int Id { get; set; }
        public string PublisherCode { get; set; } = null!;
    }
}
