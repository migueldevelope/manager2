using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Manager
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
           .UseUrls("http://0.0.0.0:5200/")
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseKestrel(opts => 
                          opts.AllowSynchronousIO = true
                      )
            .UseStartup<Startup>()
            .Build();
  }
}
