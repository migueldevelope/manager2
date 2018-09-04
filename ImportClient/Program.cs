using System;
using System.Security;

namespace ImportClient
{
  class Program
  {
    static void Main(string[] args)
    {
      var import = new ImportClient();
      try
      {

        Console.WriteLine("Enter your site address:");
        var url = Console.ReadLine();
        Console.WriteLine("Enter you mail:");
        var mail = Console.ReadLine();
        Console.WriteLine("Enter your password:");
        string password = new System.Net.NetworkCredential(string.Empty, GetPassword()).Password;

        import.import(url, mail, password);
      }
      catch (Exception e)
      {
        import.SaveLogs(e.Message);
      }
    }

    public static SecureString GetPassword()
    {
      var pwd = new SecureString();
      while (true)
      {
        ConsoleKeyInfo i = Console.ReadKey(true);
        if (i.Key == ConsoleKey.Enter)
        {
          break;
        }
        else if (i.Key == ConsoleKey.Backspace)
        {
          if (pwd.Length > 0)
          {
            pwd.RemoveAt(pwd.Length - 1);
            Console.Write("\b \b");
          }
        }
        else
        {
          pwd.AppendChar(i.KeyChar);
          Console.Write("*");
        }
      }
      return pwd;
    }
  }
}
