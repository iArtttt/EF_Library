using Library.App.ConsoleHelper;
using Library.DAL.Models;
using Library.Shared.Attributes;

namespace Library.App.Menus.LibrarianMenu.SubMenu
{
    internal class Readers
    {
        [MenuAction("New Reader", 0, "Add new reader to the Library")]
        public void Add()
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
        [MenuAction("Change existing Reader", 1, "Change information about Existing reader")]
        public void Update()
        {
            var reader = ReaderHelper.FindReader();
            if (reader == null) return;

            bool isUpdated = ReaderHelper.Update(reader.Id, (dbReader, context) =>
            {
                string helpString = "\t\t | Current: ";
                var updateOptions = new List<UpdateCommand>
                {

                    new UpdateCommand($"Name{helpString}{dbReader.Name}", () => {
                        string? newName = ReaderHelper.SetName();
                        if (newName != null) dbReader.Name = newName;
                    }),
                    new UpdateCommand($"Last name{helpString}{dbReader.LastName}", () => { dbReader.LastName = ReaderHelper.SetLastName(); }),
                    new UpdateCommand($"Email{helpString}{dbReader.Email ?? "None"}", () => { dbReader.Email = ReaderHelper.SetEmail(); }),
                    new UpdateCommand($"Birthday{helpString}{dbReader.Birthday.ToShortDateString()}", () => {
                        DateTime? newDate = ReaderHelper.SetBirthday();
                        if (newDate.HasValue) dbReader.Birthday = newDate.Value;
                    }),
                    new UpdateCommand($"Document Number{helpString}{dbReader.DocumentNumber}", () => { dbReader.DocumentNumber = ReaderHelper.SetDocumentNumber(); }),
                    new UpdateCommand($"Document Type{helpString}{dbReader.DocumentType}", () => { dbReader.DocumentType = ReaderHelper.SetDocumentType(); })
                };

                var fieldsSelector = new HelpMenu<UpdateCommand>($"What do you want to change in '{dbReader.Name}'?", updateOptions);
                var selectedCommands = fieldsSelector.MultiSelect();

                if (selectedCommands.Count == 0) return;

                foreach (var command in selectedCommands)
                {
                    if (command != null)
                    {
                        Console.Clear();
                        $"[Editing {command.Name}]".WriteLineInfo();
                        command.Action.Invoke();
                    }
                }
            });

            Console.Clear();
            if (isUpdated)
                "Reader changes successfully saved to database!".WriteLineSuccess();
            else
                "Error: Reader not found or already deleted.".WriteLineError();

            Console.ReadKey(true);
        }
        [MenuAction("Remove Reader", 2, "Remove reader from the Library")]
        public void Remove()
        {
            var reader = ReaderHelper.FindReader();
            if (reader == null) return;

            $"ID: {reader.Id} ".WriteLineInfo();
            $"Login: {reader.Login} ".WriteLineInfo();
            $"Full Name: {reader.LastName} {reader.Name} ".WriteLineInfo();
            $"Email: {reader.Email} ".WriteLineInfo();
            $"Document: {reader.DocumentType}: {reader.DocumentNumber}".WriteLineInfo();
            "Is this correct reader? (Press 'Y' to delete, any other key to cancel)".WriteLineError();
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Y)
            {
                bool isDeleted = ReaderHelper.Remove(reader);

                if (isDeleted)
                    $"\nReader '{reader.Name}' was successfully deleted!".WriteLineSuccess();
                else
                    "\nError: Reader was already removed by another session.".WriteLineError();

            }
            else
            {
                "Deletion cancelled.".WriteLineInfoDark();
            }

            "\nPress any key to return...".WriteInfoDark();
            Console.ReadKey(true);

        }
    }
}
