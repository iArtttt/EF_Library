namespace Library.Shared.Interfaces.Menus
{
    public interface IMenu
    {
        public string Title { get; }
        
        public string? Description { get; }
        
        public void Do();
    }
}
