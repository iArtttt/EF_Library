using Library.App.ConsoleHelper;
using Library.Shared.Attributes;
using Library.Shared.Interfaces.Menus;
using System.Reflection;

namespace Library.App.Menus.EntityMenu
{
    internal class Menu : MenuItem, ISubMenu
    {
        private int _index = 0;
        private bool _isExit = false;
        private readonly List<IMenuElement> _menuElements = new();
        public IEnumerable<IMenuElement> MenuElements => _menuElements;


        public Menu(string? title, Action? process = null, string? description = null)
           : base(title, process, description)
        { }
        public Menu(string? title, string? description)
            : base(title, null, description)
        { }
        public Menu(string? title)
            : base(title, null, null)
        { }

        public override void Process()
        {

            while (!_isExit)
            {
                Console.Clear();

                $"\n=== {Title} ===\n".WriteLineInfo();
                string.Empty.PadRight(Console.BufferWidth, '_').WriteLineUnknownDark();

                for (int i = 0; i < _menuElements.Count; i++)
                {
                    if (i == _index)
                        $"{_menuElements[i].Title} <-- {_menuElements[i].Description}".WriteLineInfoDark();
                    else
                        $"{_menuElements[i].Title}".WriteLineInfo();
                }

                MoveEnter();
            }
            _isExit = false;
        }
        private void MoveEnter()
        {
            try
            {

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.W:
                        _index = (_index - 1 < 0) ? _menuElements.Count - 1 : _index - 1;
                        break;
                    case ConsoleKey.UpArrow: goto case ConsoleKey.W;

                    case ConsoleKey.S:
                        _index = (_index + 1 > _menuElements.Count - 1) ? 0 : _index + 1;
                        break;
                    case ConsoleKey.DownArrow: goto case ConsoleKey.S;

                    case ConsoleKey.D:
                        try
                        {
                            if (_menuElements.Count > 0)
                                _menuElements[_index].Process();
                        }
                        catch (Exception ex)
                        {
                            $"Error: {ex.InnerException?.Message ?? ex.Message}".WriteLineError();
                            Console.ReadKey();
                            _index = 0;
                        }
                        break;
                    case ConsoleKey.RightArrow: goto case ConsoleKey.D;
                    case ConsoleKey.Enter: goto case ConsoleKey.D;

                    case ConsoleKey.A:
                        _isExit = true;
                        _index = 0;
                        break;
                    case ConsoleKey.Backspace: goto case ConsoleKey.A;
                    case ConsoleKey.LeftArrow: goto case ConsoleKey.A;
                    case ConsoleKey.Escape: goto case ConsoleKey.A;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void AddMenuItem(IMenuElement item)
        {
            _menuElements.Add(item);
        }

        private static Menu DetectMenu<T>() where T : new() => DetectMenu(new Menu("Main Menu"), typeof(T), DbConfig.Options);
        public static Menu DetectMenu<T>(params object[] values) where T : new()
            => DetectMenu(new Menu("Main Menu"), typeof(T), values.Append(DbConfig.Options).ToArray());

        private static Menu DetectMenu(Menu newMenu, Type typeMenu, params object[] values)
        {

            var obj = Activator.CreateInstance(typeMenu);

            var menuItems = typeMenu.GetMethods()
                .Where(m => m.GetCustomAttribute<MenuActionAttribute>() != null)
                .Select(m =>
                {
                    var attribute = m.GetCustomAttribute<MenuActionAttribute>();
                    return new MenuItem(attribute!.Title, () => { m.Invoke(obj, MapValues(m, values)); }, attribute.Description);
                });

            var subMenus = typeMenu.GetProperties()
                .Where(p => p.GetCustomAttribute<SubMenuAttribute>() != null)
                .Select(p =>
                {
                    var attribute = p.GetCustomAttribute<SubMenuAttribute>();
                    return new { Menu = new Menu(attribute!.Title ?? p.Name, attribute.Description), Type = p.PropertyType };
                });

            foreach (var menu in subMenus)
            {
                newMenu.AddMenuItem(DetectMenu(menu.Menu, menu.Type, values));
            }
            foreach (var item in menuItems)
            {
                newMenu.AddMenuItem(item);
            }


            return newMenu;
        }

        private static object?[] MapValues(MethodInfo m, object[] values)
        {
            List<object?> result = new List<object?>();

            var valuesList = values.ToList();
            foreach (var item in m.GetParameters())
            {
                var index = valuesList.FindIndex(t => t != null && t.GetType().IsAssignableTo(item.ParameterType));

                if (index != -1)
                {
                    result.Add(valuesList[index]);
                    valuesList.RemoveAt(index);
                }
                else
                    result.Add(null);
            }
            return result.ToArray();
        }

        internal static void Start()
        {
            DetectMenu<Lobby>().Process();
        }

        /*
                private static void LibrarianRegistration()
                {
                    Librarian librarian = new Librarian();
                    string? loggin = string.Empty;

                    loggin = ToWrite("Write New Loggin");
                    if (string.IsNullOrEmpty(loggin))
                    {
                        Console.WriteLine("String is Empty");
                        return;
                    }
                    if (librarians.Any(l => l.Login == loggin))
                    {
                        Console.WriteLine("Already Exist");
                        return;
                    }

                    var password = ToWrite("Write New Password");
                    if (string.IsNullOrEmpty(password))
                    {
                        Console.WriteLine("Password is Empty");
                        return;
                    }

                    var email = ToWrite("Write your Email");

                    librarian.Login = loggin;
                    librarian.Password = password;
                    librarian.Email = email;

                    librarians.Add(librarian);
                    using var context = new LibraryContext(DbConfig.Options);
                    context.Librarians.Add(librarian);
                    context.SaveChanges();
                }
                private static string? ToWrite(string msg)
                {
                    Console.WriteLine(msg);
                    return Console.ReadLine();
                }*/
    }
}
