using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace IntegrationServer
{
  /// <summary>
  /// Controlador de inicialização do projeto
  /// </summary>
  public class Program
  {
    /// <summary>
    /// Rotina de entrada do programa
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
      BuildWebHost(args).Run();
    }
    /// <summary>
    /// Configurador de servidor web
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
           .UseUrls("http://0.0.0.0:5203/")

            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseKestrel()
            .UseStartup<Startup>()
            .Build();
  }
}
