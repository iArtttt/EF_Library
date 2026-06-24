using Library.Shared.Enums;

namespace Library.Shared.Interfaces
{
    public interface IDocument : IID
    {
        public string DocumentNumber { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}
