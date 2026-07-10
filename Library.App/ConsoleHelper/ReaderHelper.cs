using Library.DAL.Models;

namespace Library.App.ConsoleHelper
{
    internal static class ReaderHelper
    {
        /// <summary>
        /// Interactive menu to choose reader from database.
        /// </summary>
        public static Reader? ReaderSelect(List<Reader> existingReaders)
        {
            if (existingReaders == null || existingReaders.Count == 0)
            {
                "No registered readers found in the database.".WriteLineError();
                Console.ReadKey(true);
                return null;
            }

            var readersDictionary = existingReaders.ToDictionary(
                r => $"[ID: {r.Id}] {r.LastName} {r.Name} | Birthday: {r.Birthday}",
                r => r
            );

            var selectReaderMenu = new HelpMenu<string>("Select a Reader from the list", readersDictionary.Keys);
            string? selectedKey = selectReaderMenu.Select();

            if (string.IsNullOrEmpty(selectedKey))
                return null;

            return readersDictionary[selectedKey];
        }
    }
}
