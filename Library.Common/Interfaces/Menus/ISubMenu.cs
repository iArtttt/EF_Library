namespace Library.Shared.Interfaces.Menus
{
    public interface ISubMenu : IMenuElement
    {
        public IEnumerable<IMenuElement> Menus { get; }
    }
}
