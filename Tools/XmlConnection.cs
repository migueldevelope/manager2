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
            TokenServer = "TokenServer",
            QueueName = "QueueName",
            ServiceBusConnectionString = "ServiceBusConnectionString"
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

    public static Config ReadVariablesSystem()
    {
      try
      {
        return new Config()
        {
          BlobKey = Environment.GetEnvironmentVariable("ANALISA_BLOBKEY", EnvironmentVariableTarget.User),
          DataBase = Environment.GetEnvironmentVariable("ANALISA_DATABASE", EnvironmentVariableTarget.User),
          DataBaseLog = Environment.GetEnvironmentVariable("ANALISA_DATABASELOG", EnvironmentVariableTarget.User),
          SendGridKey = Environment.GetEnvironmentVariable("ANALISA_SENDGRIDKEY", EnvironmentVariableTarget.User),
          Server = Environment.GetEnvironmentVariable("ANALISA_SERVER", EnvironmentVariableTarget.User),
          ServerLog = Environment.GetEnvironmentVariable("ANALISA_SERVERLOG", EnvironmentVariableTarget.User),
          SignalRService = Environment.GetEnvironmentVariable("ANALISA_SIGNALRSERVICE", EnvironmentVariableTarget.User),
          TokenServer = Environment.GetEnvironmentVariable("ANALISA_TOKENSERVER", EnvironmentVariableTarget.User),
          QueueName = Environment.GetEnvironmentVariable("ANALISA_QUEUENAME", EnvironmentVariableTarget.User),
          ServiceBusConnectionString = Environment.GetEnvironmentVariable("ANALISA_SERVICEBUSCONNECTIONSTRING", EnvironmentVariableTarget.User),
        };

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
