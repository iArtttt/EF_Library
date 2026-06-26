using Library.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Library.App
{
    internal static class DbConfig
    {
        public static DbContextOptions<LibraryContext> Options { get; private set; } = null!;

        static DbConfig()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("appsettings.json");

            var configuration = configurationBuilder.Build();

            var optionBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            Options = optionBuilder.Options;
        }
    }
}
