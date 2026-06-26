namespace Library.Shared.Interfaces.Menus
{
    public interface IMenuElement
    {
        public string Title { get; }
        
        public string? Description { get; }
        
        public void Process();
    }
}
