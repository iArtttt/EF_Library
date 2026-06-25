namespace Library.Shared.Interfaces.DAL.Complex
{
    public interface IAuthor : IPerson
    {
        public string? SecondName { get; set; }
        public IEnumerable<IBook> Books { get; }
    }
}
