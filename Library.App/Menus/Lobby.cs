using Library.App.ConsoleHelper;
using Library.App.Menus.EntityMenu;
using Library.App.Menus.LibrarianMenu;
using Library.App.Menus.ReaderMenu;
using Library.DAL.Models;
using Library.Shared.Attributes;

namespace Library.App.Menus
{
    internal class Lobby
    {
        [MenuAction("Enter", 1, "Enter In System")]
        public void Enter()
        {
            Console.Clear();
            string? login = "Write Loggin".ReadLine(ConsoleColor.Yellow);
            string? password = "Write Password".ReadLine(ConsoleColor.Yellow);

            var user = BaseHelper.Authenticate(login, password);

            if (user != null)
            {
                Console.Clear();
                $"Welcome, {user.Login}!".WriteLineSuccess();
                Thread.Sleep(1000);

                if (user is Librarian librarian) Menu.DetectMenu<LibrarianLobby>(librarian).Process();
                else if (user is Reader reader)  Menu.DetectMenu<ReaderLobby>(reader).Process();
            }
            else
            {
                Writer.WriteLineError("Incorrect Login or Password");
                Console.ReadKey(true);
            }

        }


        [MenuAction("Register", 2, "New Reader")]
        public void Registration()
        {
            try
            {
                var login = ReaderHelper.SetLogin();
                if (string.IsNullOrEmpty(login)) return;
                var password = ReaderHelper.SetPassword();
                if (string.IsNullOrEmpty(password)) return;


                var name = ReaderHelper.SetName();
                var lastName = ReaderHelper.SetLastName();
                var email = ReaderHelper.SetEmail();
                var documentType = ReaderHelper.SetDocumentType();
                var documentNumber = ReaderHelper.SetDocumentNumber();
                var birthday = ReaderHelper.SetBirthday();

                if (birthday == null)
                {
                    "Registration cancelled by user or incorrect birthday.".WriteLineInfoDark();
                    Console.ReadKey(true);
                    return;
                }


                Reader newReader = new Reader
                {
                    Login = login,
                    Password = password,
                    Name = name,
                    LastName = lastName,
                    Email = email,
                    DocumentType = documentType,
                    DocumentNumber = documentNumber,
                    Birthday = birthday.Value

                };

                ReaderHelper.Add(newReader);

                $"{newReader.LastName} {newReader.Name} was register successfully".WriteLineSuccess();
            }
            catch (Exception ex)
            {
                $"Error: {ex.InnerException?.Message ?? ex.Message}".WriteLineError();
                Console.ReadKey(true);
            }

        }

        [MenuAction("Exit", int.MaxValue)]
        public void Exit()
        {
            "Thank you for using our Library System!".WriteLineSuccess();
            "Goodbye!".WriteLineInfo();
            Thread.Sleep(1200);

            Environment.Exit(0);
        }
    }
}
