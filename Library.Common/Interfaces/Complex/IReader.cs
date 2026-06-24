namespace Library.Shared.Interfaces.Complex
{
    public interface IReader : IUser, IPerson, IDocument
    {
        public DateTime Birthday { get; set; }
    }
}
