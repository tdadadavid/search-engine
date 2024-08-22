using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using dotenv.net;

namespace SearchEngine
{
    /// <summary>
    /// The entry point for the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            // Configure Serilog for logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting up the host...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            
            }
        }

    /// <summary>
    /// Creates and configures a <see cref="IHostBuilder"/> for the application.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>An <see cref="IHostBuilder"/> instance.</returns
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
    }
  }
}
