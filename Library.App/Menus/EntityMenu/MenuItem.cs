using Library.Shared.Interfaces.Menus;

namespace Library.App.Menus.EntityMenu
{
    internal class MenuItem : IMenuItem
    {
        private readonly Action _process;
        public string Title { get; } = null!;
        public string? Description {  get; }
        
        public MenuItem(string? title, Action? process = null, string? description = null)
        {
            _process = process ?? new Action(() => { });
            Title = title ?? "";
            Description = description;
        }
        public virtual void Process() => _process.Invoke();
    }
}
