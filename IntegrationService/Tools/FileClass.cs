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

    public static void SaveLog(string filePath, string register)
    {
      using (TextWriter stream = new StreamWriter(filePath,true))
      {
        stream.WriteLine(register);
      }
    }
  }
}
