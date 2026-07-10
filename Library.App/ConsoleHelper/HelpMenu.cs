using Library.Shared.Interfaces.DAL;

namespace Library.App.ConsoleHelper
{
    internal class HelpMenu<T>
    {
        /// <summary>
        /// It`s a Title, what you have to do
        /// </summary>
        public string? Title { get; }
        private List<T?> Results { get; set; } = new List<T?>();
        private int _index = 0;
        private bool _multiSelect;
        private bool _isExit = false;
        private readonly List<MenuWrapper> _elements = new();
        private Func<T, T?> _action;

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpMenu{T}"/> class with a custom action but without a title.
        /// </summary>
        /// <param name="action">The delegate executed when an item is selected.</param>
        /// <param name="values">The collection of items to display in the menu.</param>
        /// <param name="multiSelect">Specifies whether multiple items can be selected at once.</param>
        public HelpMenu(Func<T, T?> action, IEnumerable<T> values)
        {
            Title = null;
            _action = action;
            AddElements(values);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpMenu{T}"/> class using a default selector action and without a title.
        /// </summary>
        /// <param name="values">The collection of items to display in the menu.</param>
        /// <param name="multiSelect">Specifies whether multiple items can be selected at once.</param>
        public HelpMenu(IEnumerable<T> values)
        {
            Title = null;
            _action = a => a;
            AddElements(values);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpMenu{T}"/> class with a custom title and a default selector action.
        /// </summary>
        /// <param name="title">The header text displayed at the top of the menu.</param>
        /// <param name="values">The collection of items to display in the menu.</param>
        /// <param name="multiSelect">Specifies whether multiple items can be selected at once.</param>
        public HelpMenu(string title, IEnumerable<T> values)
        {
            Title = title;
            _action = a => a;
            AddElements(values);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpMenu{T}"/> class with a custom title and a custom selection action.
        /// </summary>
        /// <param name="title">The header text displayed at the top of the menu.</param>
        /// <param name="action">The delegate executed when an item is selected.</param>
        /// <param name="values">The collection of items to display in the menu.</param>
        /// <param name="multiSelect">Specifies whether multiple items can be selected at once.</param>
        public HelpMenu(string title, Func<T, T?> action, IEnumerable<T> values)
        {
            Title = title;
            _action = action;
            AddElements(values);
        }
        private class MenuWrapper
        {
            public string Name { get; set; } = null!;
            public T Value { get; set; } = default!;
            public bool IsChoosen { get; set; } = false;
        }
        public void AddElements(IEnumerable<T> elements)
        {
            foreach (var item in elements)
            {
                if (item == null) continue;

                string displayName = item.ToString()!; 

                if (item is IName namedItem)
                {
                    displayName = namedItem.Name;
                }
                else if (typeof(T).IsEnum)
                {
                    displayName = Enum.GetName(typeof(T), item) ?? item.ToString()!;
                }

                _elements.Add(new MenuWrapper { Name = displayName, Value = item });
            }
        }
        public T? Select(Func<T, T?>? action = null)
        {
            _multiSelect = false;
            return Process(action).FirstOrDefault();
        }

        public List<T?> MultiSelect(Func<T, T?>? action = null)
        {
            _multiSelect = true;
            return Process(action);
        }

        private List<T?> Process(Func<T, T?>? action)
        {
            if (action != null)
                _action = action;

            if (_elements.Count == 0)
            {
                $"\n=== {Title??"Make a choise"} ===\n".WriteLineInfo();
                "_".PadRight(Console.BufferWidth, '_').WriteLineUnknownDark();
                "No elements available in this list.".WriteLineError();
                "\nPress any key to return...".WriteInfoDark();
                Console.ReadKey(true);
                return Results;
            }

            while (!_isExit)
            {
                Console.Clear();

                $"\n=== {Title??"Make a choise"} ===\n".WriteLineInfo();
                "_".PadRight(Console.BufferWidth, '_').WriteLineUnknownDark();
                

                for (int i = 0; i < _elements.Count; i++)
                {
                    if (i == _index)
                    {
                        if (_elements[i].IsChoosen)
                            $"{_elements[i].Name} <--".WriteLineSuccessDark();
                        else
                            $"{_elements[i].Name} <--".WriteLineInfoDark();

                    }
                    else
                    {
                        if (_elements[i].IsChoosen)
                            $"{_elements[i].Name}".WriteLineSuccess();
                        else
                            $"{_elements[i].Name}".WriteLineInfo();

                    }
                }
                var chosenNames = _elements.Where(e => e.IsChoosen).Select(e => e.Name);
                $"\nChosen: {string.Join(", ", chosenNames)}".WriteLineSuccess();

                if (_multiSelect)
                    "(Press ESC, Backspace or Left Arrow to Confirm & Save)".WriteLineUnknownDark();

                MoveEnter();
            }
            _isExit = false;
            var finalResults = new List<T?>(Results);
            Results.Clear();
            return finalResults;
        }
        private void MoveEnter()
        {
            try
            {

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.W:
                        _index = (_index - 1 < 0) ? _elements.Count - 1 : _index - 1;
                        break;
                    case ConsoleKey.UpArrow: goto case ConsoleKey.W;

                    case ConsoleKey.S:
                        _index = (_index + 1 > _elements.Count - 1) ? 0 : _index + 1;
                        break;
                    case ConsoleKey.DownArrow: goto case ConsoleKey.S;

                    case ConsoleKey.D:
                        try
                        {
                            if (_elements.Count > 0)
                            {
                                var currentElement = _elements[_index];
                                var mappedValue = _action.Invoke(currentElement.Value);

                                if (!_multiSelect)
                                {
                                    Results.Add(mappedValue);
                                    _isExit = true; 
                                }
                                else
                                {
                                    if (!_elements[_index].IsChoosen)
                                        Results.Add(_action.Invoke(_elements[_index].Value));
                                    else
                                        Results.RemoveAll(r => r != null && r.Equals(mappedValue));

                                    _elements[_index].IsChoosen = !_elements[_index].IsChoosen;
                                }
                            }
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
                        if (!_multiSelect) 
                            Results.Clear();
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
    }
}
