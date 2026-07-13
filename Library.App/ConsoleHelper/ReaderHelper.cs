using Library.DAL;
using Library.DAL.Models;
using Library.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.App.ConsoleHelper
{
    internal static class ReaderHelper
    {
        /// <summary>
        /// Requests a REQUIRED reader's first name.
        /// </summary>
        public static string SetName() => BaseHelper.GetString("Write your new Name: ", "Name cannot be empty!");

        /// <summary>
        /// Requests a REQUIRED reader's last name.
        /// </summary>
        public static string SetLastName() => BaseHelper.GetString("Write your new LastName: ", "LastName cannot be empty!");

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
        public static string? SetPassword() => BaseHelper.GetString("Write your new Password: ", "Password cannot be empty!");

        /// <summary>
        /// Requests a REQUIRED reader's document type.
        /// </summary>
        public static DocumentType SetDocumentType()
        {
            var documentSelector = new HelpMenu<DocumentType>("Select identity confirmation document", Enum.GetValues<DocumentType>());
            return documentSelector.Select();
        }

        /// <summary>
        /// Requests a REQUIRED reader's document number.
        /// </summary>
        public static string SetDocumentNumber() => BaseHelper.GetString("Write your Document number: ", "Document number cannot be empty!");

        /// <summary>
        /// Atomically saves a fully prepared Reader entity into the database.
        /// </summary>
        public static void Add(Reader reader)
        {
            using var context = new LibraryContext(DbConfig.Options);
            context.Readers.Add(reader);
            context.SaveChanges(); 
        }
        
        /// <summary>
        /// Updates a reader entity in the database by executing a custom modification action within a secure context.
        /// </summary>
        /// <param name="readerId">The identifier of the reader to update.</param>
        /// <param name="updateAction">The delegate containing specific property modifications.</param>
        /// <returns><c>true</c> if the reader was found and successfully updated; otherwise, <c>false</c>.</returns>
        public static bool Update(int readerId, Action<Reader, LibraryContext> updateAction)
        {
            using var context = new LibraryContext(DbConfig.Options);

            var dbReader = context.Readers.FirstOrDefault(b => b.Id == readerId);

            if (dbReader == null) return false;

            updateAction.Invoke(dbReader, context);

            context.SaveChanges();
            return true;
        }
        public static bool Remove(Reader reader)
        {
            using var context = new LibraryContext(DbConfig.Options);
            var readerToDelete = context.Readers.Find(reader.Id);
            if (readerToDelete != null)
            {
                context.Readers.Remove(readerToDelete);
                context.SaveChanges();
                $"\nReader '{reader.Name}' was successfully deleted!".WriteLineSuccess();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Interactive menu to choose reader from database.
        /// </summary>
        /// <param name="toSearchText">What text you see to search</param>
        /// <param name="toSelectText">What text you see to select</param>
        public static Reader? FindReader(string? toSearchText = null, string? toSelectText = null/*List<Reader> existingReaders*/)
        {
            string? searchText = (toSearchText ?? "Write Reader Name or Last Name to issue: ").Read(ConsoleColor.Yellow);
            List<Reader> existingReaders = FindReaders(searchText);
            
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
        public static List<Reader> FindReaders(string? toSearch = null)
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
