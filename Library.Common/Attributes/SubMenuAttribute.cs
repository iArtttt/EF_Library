namespace Library.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class SubMenuAttribute : Attribute
    {
        public string Title { get; } = null!;
        public int Order { get; }
        public string? Description { get; }

        public SubMenuAttribute(string title, int order, string description = null!)
        {
            Title = title;
            Order = order;
            Description = description;
        }
    }
}
