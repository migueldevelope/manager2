using IntegrationService.Enumns;
using System;
using System.IO;

namespace IntegrationService.Tools
{
  public static class FileClass
  {
    public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
    {
      using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
      {
        var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        binaryFormatter.Serialize(stream, objectToWrite);
      }
    }
    public static T ReadFromBinaryFile<T>(string filePath)
    {
      using (Stream stream = File.Open(filePath, FileMode.Open))
      {
        var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        return (T)binaryFormatter.Deserialize(stream);
      }
    }

    public static void SaveLog(string file, string register, EnumTypeLineOpportunityg typeLog)
    {
      bool newFile = true;
      if (File.Exists(file))
        newFile = false;
      using (TextWriter stream = new StreamWriter(file,true))
      {
        switch (typeLog)
        {
          case EnumTypeLineOpportunityg.Register:
            stream.WriteLine(register);
            break;
          default:
            if (newFile)
              stream.WriteLine("Data\tTipo\tMensagem");
            stream.WriteLine(string.Format("{0}\t{1}\t{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), typeLog, register));
            break;
        }
      }
    }
  }
}
