namespace Library.App.ConsoleHelper
{
    public static class Writer
    {
        /// <summary>
        /// <para>Write text to Console and starts a new line</para>
        /// <para>This Method automatically <c>Reset Color</c></para>
        /// </summary>
        /// <param name="text">Text to write in Console.</param>
        public static void WriteLine(string text, ConsoleColor console = ConsoleColor.White) 
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = console;
            Console.WriteLine(text);
            Console.ForegroundColor = originalColor;
        }

        /// <inheritdoc cref="WriteLine(string, ConsoleColor)"/> 
        /// <remarks> Change console color to Yellow </remarks>
        public static void WriteLineInfo(this string text) => WriteLine(text, ConsoleColor.Yellow);

        /// <inheritdoc cref="WriteLine(string, ConsoleColor)"/>
        /// <remarks> Change console color to DarkYellow  </remarks>
        public static void WriteLineInfoDark(this string text) => WriteLine(text, ConsoleColor.DarkYellow);
        
        /// <inheritdoc cref="WriteLine(string, ConsoleColor)"/> 
        /// <remarks> Change console color to Red </remarks>
        public static void WriteLineError(this string text) => WriteLine(text, ConsoleColor.Red);

        /// <inheritdoc cref="WriteLine(string, ConsoleColor)"/>
        /// <remarks> Change console color to DarkRed </remarks>
        public static void WriteLineErrorDark(this string text) => WriteLine(text, ConsoleColor.DarkRed);
        
        /// <inheritdoc cref="WriteLine(string, ConsoleColor)"/>
        /// <remarks> Change console color to Green </remarks>
        public static void WriteLineSuccess(this string text) => WriteLine(text, ConsoleColor.Green);

        /// <inheritdoc cref="WriteLine(string, ConsoleColor)"/>
        /// <remarks> Change console color to DarkGreen </remarks>
        public static void WriteLineSuccessDark(this string text) => WriteLine(text, ConsoleColor.DarkGreen);

        /// <inheritdoc cref="WriteLine(string, ConsoleColor)"/>
        /// <remarks> Change console color to Blue </remarks>
        public static void WriteLineUnknown(this string text) => WriteLine(text, ConsoleColor.Blue);

        /// <inheritdoc cref="WriteLine(string, ConsoleColor)"/>
        /// <remarks> Change console color to DarkBlue </remarks>
        public static void WriteLineUnknownDark(this string text) => WriteLine(text, ConsoleColor.DarkBlue);
        
        
        /// <summary>
        /// <para>Write text to Console WITHOUT starting a new line</para>
        /// <para>This Method automatically <c>Reset Color</c></para>
        /// </summary>
        /// <param name="text">Text to write in Console.</param>
        public static void Write(string text, ConsoleColor console = ConsoleColor.White) 
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = console;
            Console.Write(text);
            Console.ForegroundColor = originalColor;
        }

        /// <inheritdoc cref="Write(string, ConsoleColor)"/> 
        /// <remarks> Change console color to Yellow </remarks>
        public static void WriteInfo(this string text) => Write(text, ConsoleColor.Yellow);

        /// <inheritdoc cref="Write(string, ConsoleColor)"/>
        /// <remarks> Change console color to DarkYellow  </remarks>
        public static void WriteInfoDark(this string text) => Write(text, ConsoleColor.DarkYellow);

        /// <inheritdoc cref="Write(string, ConsoleColor)"/> 
        /// <remarks> Change console color to Red </remarks>
        public static void WriteError(this string text) => Write(text, ConsoleColor.Red);

        /// <inheritdoc cref="Write(string, ConsoleColor)"/>
        /// <remarks> Change console color to DarkRed </remarks>
        public static void WriteErrorDark(this string text) => Write(text, ConsoleColor.DarkRed);

        /// <inheritdoc cref="Write(string, ConsoleColor)"/>
        /// <remarks> Change console color to Green </remarks>
        public static void WriteSuccess(this string text) => Write(text, ConsoleColor.Green);

        /// <inheritdoc cref="Write(string, ConsoleColor)"/>
        /// <remarks> Change console color to DarkGreen </remarks>
        public static void WriteSuccessDark(this string text) => Write(text, ConsoleColor.DarkGreen);

        /// <inheritdoc cref="Write(string, ConsoleColor)"/>
        /// <remarks> Change console color to Blue </remarks>
        public static void WriteUnknown(this string text) => Write(text, ConsoleColor.Blue);

        /// <inheritdoc cref="Write(string, ConsoleColor)"/>
        /// <remarks> Change console color to DarkBlue </remarks>
        public static void WriteUnknownDark(this string text) => Write(text, ConsoleColor.DarkBlue);
    }
}
