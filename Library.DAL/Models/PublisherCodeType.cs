using Library.Shared.Interfaces;

namespace Library.DAL.Models
{
    public class PublisherCodeType : IPublisherCodeType
    {
        public int Id { get; set; }
        public string PublisherCode { get; set; } = null!;
    }
}
