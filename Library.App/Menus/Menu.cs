using Library.DAL;
using Library.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Library.App.Menus
{
    internal class Menu
    {
        private static List<Librarian> librarians = new List<Librarian>();
        private static DbContextOptionsBuilder<LibraryContext> optionBuilder;

        public static void Start()
        {
            Init();
            //LibrarianRegistration();
            //Enterence();
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
            using var context = new LibraryContext(optionBuilder.Options);
            context.Librarians.Add(librarian);
            context.SaveChanges();
        }
        private static string? ToWrite(string msg)
        {
            Console.WriteLine(msg);
            return Console.ReadLine();
        }

        private static void Enterence()
        {
            bool isExit = false;
            string? str = string.Empty;
            do
            {
                Console.WriteLine("Write Loggin");
                str = Console.ReadLine();
                if (string.IsNullOrEmpty(str)) 
                    isExit = true;
                else
                {
                    var librarian = librarians.FirstOrDefault(l => l.Login == str);

                    if (librarian != null)
                    {
                        Console.WriteLine("Write Password");
                        str = Console.ReadLine();
                        if (string.IsNullOrEmpty(str) || librarian.Password != str) 
                            Console.WriteLine("Incorrect Password");
                        else
                            Console.WriteLine("YEEEEEEEEEEY");
                    }
                    else
                        Console.WriteLine("Incorrect Loggin");
                }

            } while (!isExit);
        }

        private static void Init()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("appsettings.json");
            
            var configuration = configurationBuilder.Build();

            optionBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            using var context = new LibraryContext(optionBuilder.Options);

            librarians = context.Librarians.ToList();
        }
    }
}
