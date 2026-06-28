using Library.Shared.Attributes;

namespace Library.App.Menus
{
    internal class Lobby
    {
        [MenuAction("Enter", 1, "Enter In System")]
        public void Enter()
        {

        }


        [MenuAction("Register", 2, "New Reader")]
        public void Regestration()
        {

        }


        [MenuAction("Exit", int.MaxValue)]
        public void Exit()
        {

        }
    }
}
