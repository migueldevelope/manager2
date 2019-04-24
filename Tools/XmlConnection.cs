using System;
using System.IO;
using System.Xml.Serialization;
using Tools.Data;

namespace Tools
{
  public static class XmlConnection
  {
    public static Config ReadConfig()
    {
      try
      {
        string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../Config.xml");
        if (!File.Exists(fileName))
        {
          WriteConfig(new Config()
          {
            BlobKey = "BlobKey",
            DataBase = "DataBase",
            DataBaseLog = "DataBaseLog",
            SendGridKey = "SendGridKey",
            Server = "Server",
            ServerLog = "ServerLog",
            SignalRService = "SignalRService",
            TokenServer = "TokenServer"
          }, fileName);
        }
        XmlSerializer xs = new XmlSerializer(typeof(Config));
        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
          return (Config)xs.Deserialize(fs);
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static void WriteConfig(Config config, string fileName)
    {
      try
      {
        XmlSerializer xs = new XmlSerializer(typeof(Config));
        TextWriter writer = new StreamWriter(fileName);
        xs.Serialize(writer, config);
        writer.Close();
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
