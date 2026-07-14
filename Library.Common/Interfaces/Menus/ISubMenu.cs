namespace Library.Shared.Interfaces.Menus
{
    public interface ISubMenu : IMenuElement
    {
        public IEnumerable<IMenuElement> MenuElements { get; }
    }
}
