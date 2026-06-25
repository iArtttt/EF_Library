namespace Library.Shared.Interfaces.Menus
{
    public interface ISubMenu : IMenu
    {
        public IMenu Menu { get; }
    }
}
