using Library.DAL;
using Library.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.App.ConsoleHelper
{
    internal static class ReaderHelper
    {
        /// <summary>
        /// Requests a REQUIRED reader's first name.
        /// </summary>
        public static string? SetName() => BaseHelper.GetString("Write your new Name: ", "Name cannot be empty!");

        /// <summary>
        /// Requests a REQUIRED reader's last name.
        /// </summary>
        public static string? SetLastName() => BaseHelper.GetString("Write your new LastName: ", "LastName cannot be empty!");

        /// <summary>
        /// Requests an OPTIONAL reader's email address.
        /// </summary>
        public static string? SetEmail() => BaseHelper.GetString("Write your new Email: ");

        /// <inheritdoc cref="BaseHelper.DateSet(string)"/>
        public static DateTime? SetBirthday() => BaseHelper.DateSet("Birthday");

        /// <summary>
        /// Requests a REQUIRED and UNIQUE login name from the database.
        /// </summary>
        public static string? SetLogin()
        {
            var login = BaseHelper.GetString("Write your new Login: ", "Login cannot be empty!");
            
            if (string.IsNullOrEmpty(login))
            {
                Console.WriteLine("String is Empty");
                Console.ReadKey(true);
                return null;
            }
            using (var context = new LibraryContext(DbConfig.Options))
            {
                if (context.Users.Any(l => l.Login == login))
                {
                    "Already Exist".WriteLineError();
                    Console.ReadKey(true);
                    return null;
                }
            }
            return login;
        }

        /// <summary>
        /// Requests a REQUIRED password from the user.
        /// </summary>
        public static string? SetPassword()
        {
            //  To do...
            return BaseHelper.GetString("Write your new Password: ", "Password cannot be empty!");
        }

        /// <summary>
        /// Atomically saves a fully prepared Reader entity into the database.
        /// </summary>
        public static void AddReader(Reader reader)
        {
            try
            {
                using var context = new LibraryContext(DbConfig.Options);
                context.Readers.Add(reader);
                context.SaveChanges(); // EF Core executes TPT transaction under the hood [0.5]
            }
            catch (Exception ex)
            {
                $"Critical Error during database save: {ex.Message}".WriteLineError();
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Interactive menu to choose reader from database.
        /// </summary>
        /// <param name="toSearchText">What text you see to search</param>
        /// <param name="toSelectText">What text you see to select</param>
        public static Reader? GetReader(string? toSearchText = null, string? toSelectText = null/*List<Reader> existingReaders*/)
        {
            string? searchText = (toSearchText ?? "Write Reader Name or Last Name to issue: ").Read(ConsoleColor.Yellow);
            List<Reader> existingReaders = GetReaders(searchText);
            
            if (existingReaders == null || existingReaders.Count == 0)
            {
                "No registered readers found in the database.".WriteLineError();
                Console.ReadKey(true);
                return null;
            }

            var readersDictionary = existingReaders.ToDictionary(
                r => $"[ID: {r.Id}] {r.LastName} {r.Name} | Birthday: {r.Birthday.ToShortDateString}",
                r => r
            );

            string selectText = toSelectText ?? "Select a Reader from the list";
            var selectReaderMenu = new HelpMenu<string>(selectText, readersDictionary.Keys);
            string? selectedKey = selectReaderMenu.Select();

            if (string.IsNullOrEmpty(selectedKey))
                return null;

            return readersDictionary[selectedKey];
        }
        public static List<Reader> GetReaders(string? toSearch = null)
        {
            using var context = new LibraryContext(DbConfig.Options);
            var query = context.Readers.AsNoTracking();

            if (string.IsNullOrEmpty(toSearch))
                return query.ToList();
            else
                return query.Where(reader =>
                    reader.Name.Contains(toSearch) ||
                    reader.LastName.Contains(toSearch)
                    ).ToList();

        }
    }
}
