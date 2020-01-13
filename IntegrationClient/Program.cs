using Manager.Views.Integration;
using System;
using System.Windows.Forms;

namespace IntegrationClient
{
  static class Program
  {
    public static string FileConfig = "CONFIG.TXT";
    public static ViewPersonLogin PersonLogin;
    public static bool autoImport;
    public static string autoVersion;
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      Environment.ExitCode = 0;
      autoImport = false;
      autoVersion = string.Empty;
      switch (args.Length)
      {
        case 1:
          if (args[0].ToUpper().Equals("HELP"))
          {
            MessageBox.Show("Utilize as seguintes opções de linha de comando: \n AUTO: para ativar a rotina automática \n V1 ou V2: para ativar a versão de importação \n nome.txt: nome do arquivo de autenticação (opcional).", "Robo Ana", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Environment.ExitCode = 2;
          }
          else
          {
            MessageBox.Show("Utilize a palavra chave HELP.", "Robo Ana", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Environment.ExitCode = 1;
          }
          break;
        case 2:
          if (!args[0].ToUpper().Equals("AUTO"))
          {
            MessageBox.Show("Utilize a palavra chave HELP.", "Robo Ana", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Environment.ExitCode = 1;
          }
          if (!args[1].ToUpper().Equals("V1") && !args[1].ToUpper().Equals("V2"))
          {
            MessageBox.Show("Utilize a palavra chave HELP.", "Robo Ana", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Environment.ExitCode = 1;
          }
          if (Environment.ExitCode == 0)
          {
            autoVersion = args[1].ToUpper();
            autoImport = true;
          }
          break;
        case 3:
          if (!args[0].ToUpper().Equals("AUTO"))
          {
            MessageBox.Show("Utilize a palavra chave HELP.", "Robo Ana", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Environment.ExitCode = 1;
          }
          if (!args[1].ToUpper().Equals("V1") && !args[1].ToUpper().Equals("V2"))
          {
            MessageBox.Show("Utilize a palavra chave HELP.", "Robo Ana", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Environment.ExitCode = 1;
          }
          if (!args[2].ToUpper().Contains(".TXT"))
          {
            MessageBox.Show("Utilize a palavra chave HELP.", "Robo Ana", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Environment.ExitCode = 1;
          }
          if (Environment.ExitCode == 0)
          {
            autoVersion = args[1].ToUpper();
            FileConfig = args[2].ToUpper();
            autoImport = true;
          }
          break;
        default:
          break;
      }
      if (Environment.ExitCode == 0)
      {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Login());
      }
      else
      {
        Application.Exit();
      }
    }
  }
}
