using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ManagerMessages
{
  /// <summary>
  /// Configurador do programa
  /// </summary>
  public class Program
  {
    /// <summary>
    /// Ponto de entrada
    /// </summary>
    /// <param name="args">Argumentos da linha de comando</param>
    public static void Main(string[] args)
    {
      BuildWebHost(args).Run();
    }
    /// <summary>
    /// Configurador do servidor Web
    /// </summary>
    /// <param name="args">Argumentos da linha de comando</param>
    /// <returns></returns>
    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
           .UseUrls("http://0.0.0.0:5205/")
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .ConfigureKestrel(serverOptions =>
            {
                // Set properties and call methods on options
            })
            .UseStartup<Startup>()
            .Build();
  }

}
