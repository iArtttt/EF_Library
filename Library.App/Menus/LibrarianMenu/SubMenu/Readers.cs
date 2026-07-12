using Library.DAL.Models;
using Library.Shared.Attributes;

namespace Library.App.Menus.LibrarianMenu.SubMenu
{
    internal class Readers
    {
        [MenuAction("New Reader", 0, "Add new reader to the Library")]
        public void Add()
        {
            Reader newReader = new Reader();
            newReader.
        }
        [MenuAction("Change existing Reader", 1, "Change information about Existing reader")]
        public void Update()
        {

        }
        [MenuAction("Remove Reader", 2, "Remove reader from the Library")]
        public void Remove()
        {

        }
    }
}
