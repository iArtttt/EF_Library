using Library.App.ConsoleHelper;
using Library.App.Menus.EntityMenu;
using Library.App.Menus.LibrarianMenu;
using Library.App.Menus.ReaderMenu;
using Library.DAL;
using Library.DAL.Models;
using Library.Shared.Abstractions;
using Library.Shared.Attributes;
using Library.Shared.Enums;

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

            User? user;
            using (var context = new LibraryContext(DbConfig.Options))
            {
                user = context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            }
            if (user != null)
            {
                Console.Clear();
                $"Welcome, {user.Login}!".WriteLineSuccess();
                Console.ReadKey(true);

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
            string? login = "Write New Login".ReadLine(ConsoleColor.Yellow);

            if (string.IsNullOrEmpty(login))
            {
                Console.WriteLine("String is Empty");
                Console.ReadKey(true);
                return;
            }
            using (var context = new LibraryContext(DbConfig.Options))
            {
                if (context.Users.Any(l => l.Login == login))
                {
                    "Already Exist".WriteLineError();
                    Console.ReadKey(true);
                    return;
                }
            }

            var password = "Write New Password".ReadLine(ConsoleColor.Yellow);
            if (string.IsNullOrEmpty(password))
            {
                "Password is Empty".WriteLineError();
                Console.ReadKey(true);
                return;
            }


            Reader reader = new Reader
            {
                Login = login,
                Password = password,
            };
            PrifileDataRegistrate(reader);

            using (var context = new LibraryContext(DbConfig.Options))
            {
                context.Readers.Add(reader);
                context.SaveChanges();
            }

            "Registration has succeeded".WriteLineSuccess();
            $"{reader.Login} Welcome to our Literature Club".WriteLineSuccess();
            Console.ReadKey(true);
        }
        private static void PrifileDataRegistrate(Reader reader)
        {
            bool isCorrect = false;
            reader.Name = ProfileHelper("Write your Name")!;
            reader.LastName = ProfileHelper("Write your Lastname")!;
            reader.Email = ProfileHelper("Write your Email", false);
            do
            {
                try
                {
                    reader.Birthday = DateTime.Parse(ProfileHelper("Write your Birthday Date in formate DD.MM.YYYY", "DD.MM.YYYY please write '.'")!);

                    var message = "Allowed Documents: " + string.Join(", ", Enum.GetNames<DocumentType>());
                    reader.DocumentType = Enum.Parse<DocumentType>(ProfileHelper($"Write your Document Type\t {message}", message)!, true);

                    isCorrect = true;
                }
                catch (Exception)
                {
                    "Invalid input format. Please try again.".WriteLineError();
                }
                
            }
            while (!isCorrect);

            reader.DocumentNumber = ProfileHelper("Write your Document Number")!;

        }
        private static string? ProfileHelper(string message, bool isRequired = true) => ProfileHelper(message, null, isRequired);
        private static string? ProfileHelper(string message, string? incorrectMessage, bool isRequired = true)
        {
            string? text;
            if (isRequired)
                text = $"{message} <Required>".ReadLine(ConsoleColor.DarkYellow);
            else
                text = message.ReadLine(ConsoleColor.Yellow);
            
            while (isRequired && string.IsNullOrEmpty(text))
            {
                incorrectMessage?.WriteLineError();
                text = message + " <Required>".ReadLine(ConsoleColor.DarkYellow);
            } 
            

            return text;
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
