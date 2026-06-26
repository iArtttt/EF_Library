namespace Library.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class MenuActionAttribute : Attribute
    {
        public string Title { get; } = null!;
        public int Order { get; }
        public string? Description { get; }

        public MenuActionAttribute(string title, int order, string description = null!)
        {
            Title = title;
            Order = order;
            Description = description;
        }
    }
}
