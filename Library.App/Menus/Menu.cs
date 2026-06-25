using Library.DAL;
using Library.DAL.Models;
using Library.Shared.Abstractions;

namespace Library.App.Menus
{
    internal class Menu
    {
        private static List<Librarian> librarians = new List<Librarian>();
        //private static DbContextOptionsBuilder<LibraryContext> optionBuilder;

        public static void Start()
        {
            Init();
            //LibrarianRegistration();
            Enterance();
        }

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
        }

        private static void Enterance()
        {
            string? login = null;
            string? password = null;

            login = ToWrite("Write Loggin");
            password = ToWrite("Write Password");


            User? user;

            using (var context = new LibraryContext(DbConfig.Options))
            {
                user = context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            }


        }
        //private static void Enterence()
        //{
        //    bool isExit = false;
        //    string? str = string.Empty;
        //    do
        //    {
        //        Console.WriteLine("Write Loggin");
        //        str = Console.ReadLine();
        //        if (string.IsNullOrEmpty(str)) 
        //            isExit = true;
        //        else
        //        {
        //            var librarian = librarians.FirstOrDefault(l => l.Login == str);

        //            if (librarian != null)
        //            {
        //                Console.WriteLine("Write Password");
        //                str = Console.ReadLine();
        //                if (string.IsNullOrEmpty(str) || librarian.Password != str) 
        //                    Console.WriteLine("Incorrect Password");
        //                else
        //                    Console.WriteLine("YEEEEEEEEEEY");
        //            }
        //            else
        //                Console.WriteLine("Incorrect Loggin");
        //        }

        //    } while (!isExit);
        //}

        private static void Init()
        {
            

            using var context = new LibraryContext(DbConfig.Options);

            librarians = context.Librarians.ToList();
        }
    }
}
