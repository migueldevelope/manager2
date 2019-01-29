using Manager.Views.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntegrationClient
{
  static class Program
  {
    public static string FileConfig = "config.txt";
    public static ViewPersonLogin PersonLogin;
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Login());
    }
  }
}
