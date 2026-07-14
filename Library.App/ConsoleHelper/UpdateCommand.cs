using Library.Shared.Interfaces.DAL;

namespace Library.App.ConsoleHelper
{
    public class UpdateCommand : IName
    {
        public int Id { get ; set ; }
        public string Name { get; set; } = null!;
        public Action Action { get; set; } = null!;

        public UpdateCommand(string name, Action action)
        {
            Name = name;
            Action = action;
        }
    }
}
