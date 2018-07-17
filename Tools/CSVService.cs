using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
  public static class CSVService
  {

    public static List<Object> ImportCSV(string nameFile)
    {
      try
      {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Local_Data\{0}.csv", nameFile));
        StreamReader rd = new StreamReader(path);

        string row = null;
        string[] item = null;
        var list = new List<Object>();

        while ((row = rd.ReadLine()) != null)
        {
          item = row.Split(';');
          list.Add(item);
        }
        rd.Close();

        return list;
      }
      catch
      {
        Console.WriteLine("Error");
        return null;
      }
    }
  }
}
