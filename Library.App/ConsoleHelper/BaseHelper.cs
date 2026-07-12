namespace Library.App.ConsoleHelper
{
    /// <summary>
    /// Base helper class that manages core user input operations with built-in validation and formatting.
    /// </summary>
    public static class BaseHelper
    {
        /// <summary>
        /// Requests an OPTIONAL string input from the user. The user can skip this by pressing Enter.
        /// </summary>
        /// <param name="prompt">The text prompt to display to the user.</param>
        /// <returns>The string entered by the user, or <c>null</c> if the input was empty.</returns>
        public static string? GetString(string prompt) => GetStringInternal(prompt, isRequired: false, errorMessage: null);
        
        /// <summary>
        /// Requests a REQUIRED string input from the user. Keeps asking until a non-empty string is provided.
        /// </summary>
        /// <param name="prompt">The text prompt to display to the user.</param>
        /// <param name="errorMessage">The error message displayed when the user attempts to submit an empty input.</param>
        /// <returns>A guaranteed non-empty string entered by the user.</returns>
        public static string GetString(string prompt, string? errorMessage) => GetStringInternal(prompt, true, errorMessage)!;
        
        /// <summary>
        /// Internal implementation for processing and validating string inputs.
        /// </summary>
        private static string? GetStringInternal(string prompt, bool isRequired, string? errorMessage)
        {
            string fullPrompt = isRequired ? $"{prompt} <Required>" : prompt;
            ConsoleColor color = isRequired ? ConsoleColor.DarkYellow : ConsoleColor.Yellow;

            string? input = fullPrompt.Read(color);

            while (isRequired && string.IsNullOrEmpty(input))
            {
                errorMessage?.WriteLineError();
                input = fullPrompt.Read(color);
            }

            return string.IsNullOrEmpty(input) ? null : input;
        }

        /// <summary>
        /// Requests an integer input with a fallback value and minimum bounds validation.
        /// </summary>
        /// <param name="prompt">The text prompt to display to the user.</param>
        /// <param name="defaultValue">The fallback value used if the input is invalid or out of bounds.</param>
        /// <param name="minValue">The minimum acceptable integer value.</param>
        /// <returns>A validated integer value.</returns>
        public static int GetInt(string prompt, int defaultValue = 1, int minValue = 1)
        {
            string? inputString = prompt.Read(ConsoleColor.Yellow);

            if (!int.TryParse(inputString, out int result) || result < minValue)
            {
                $"Invalid input. Automatically set to default: {defaultValue}".WriteLineError();
                Console.ReadKey(true);
                return defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Provides an interactive step-by-step selection for Year, Month, and Day.
        /// </summary>
        /// <param name="titlePart">The context string to display in the menu headers (e.g., "Publish" or "Birthday").</param>
        /// <returns>A validated <see cref="DateTime"/> object, or <c>null</c> if the operation was cancelled.</returns>
        public static DateTime? DateSet(string titlePart = "Publish")
        {
            int currentYear = DateTime.Now.Year;
            var yearsRange = Enumerable.Range(1880, currentYear - 1880 + 1).Reverse().ToList();

            var yearPicker = new HelpMenu<int>($"Select {titlePart} Year", yearsRange);
            int? selectedYear = yearPicker.Select();

            if (selectedYear == null || selectedYear == 0) return null;


            var monthsRange = Enumerable.Range(1, 12).ToList();
            var monthPicker = new HelpMenu<int>($"Select Month ({titlePart} Year: {selectedYear})", monthsRange);
            int? selectedMonth = monthPicker.Select();

            if (selectedMonth == null || selectedMonth == 0) return null;


            int daysInMonth = DateTime.DaysInMonth(selectedYear.Value, selectedMonth.Value);
            var daysRange = Enumerable.Range(1, daysInMonth).ToList();

            var dayPicker = new HelpMenu<int>($"Select Day ({titlePart} Date: {selectedMonth}/{selectedYear})", daysRange);
            int? selectedDay = dayPicker.Select();

            if (selectedDay == null || selectedDay == 0) return null;

            DateTime finalPublishDate = new DateTime(selectedYear.Value, selectedMonth.Value, selectedDay.Value);
            return finalPublishDate;
        }
    }
}
