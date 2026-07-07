using Library.App.Menus.EntityMenu;
using Library.App.Menus.LibrarianMenu.SubMenu;
using Library.Shared.Attributes;

namespace Library.App.Menus.LibrarianMenu
{
    internal class LibrarianLobby
    {
        [SubMenu("Search", 0, "Find Book by Author or Name")]
        public Search Search { get; set; }

        [SubMenu("Books", 0, "Add, Redact or Remove Books")]
        public Books Books { get; set; }
        
        [SubMenu("Readers", 0, "Add, Redact or Remove Readers")]
        public Readers Readers { get; set; }
        
        [SubMenu("Story", 0, "Look story and actual information about readers")]
        public Story Story { get; set; }

        [MenuAction("Exit", int.MaxValue, "Exit from cabinet")]
        public void Exit(Menu currentMenu) => currentMenu.Stop();

    }
}
