namespace Library.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var host = CreateHostBuilder(args).Build();
            //host.Run();
        }

        //private static object CreateHostBuilder(string[] args) =>
        //Host.CreateDefaultBuilder(args)
        //    .ConfigureServices((hostContext, services) =>
        //    {
        //        // Метод CreateDefaultBuilder автоматически загружает appsettings.json,
        //        // поэтому здесь мы можем безопасно прочитать строку подключения
        //        var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");

        //        // Регистрируем ваш контекст (замените YourDbContext на имя вашего класса)
        //        // services.AddDbContext<YourDbContext>(options => options.UseSqlServer(connectionString));
        //    });
    }
}
