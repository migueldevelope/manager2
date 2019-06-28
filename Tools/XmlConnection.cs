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
        if (Environment.GetEnvironmentVariable("ANALISA_SERVER", EnvironmentVariableTarget.Machine) != null)
          return new Config()
          {
            BlobKey = Environment.GetEnvironmentVariable("ANALISA_BLOBKEY", EnvironmentVariableTarget.Machine),
            DataBase = Environment.GetEnvironmentVariable("ANALISA_DATABASE", EnvironmentVariableTarget.Machine),
            DataBaseLog = Environment.GetEnvironmentVariable("ANALISA_DATABASELOG", EnvironmentVariableTarget.Machine),
            SendGridKey = Environment.GetEnvironmentVariable("ANALISA_SENDGRIDKEY", EnvironmentVariableTarget.Machine),
            Server = Environment.GetEnvironmentVariable("ANALISA_SERVER", EnvironmentVariableTarget.Machine),
            ServerLog = Environment.GetEnvironmentVariable("ANALISA_SERVERLOG", EnvironmentVariableTarget.Machine),
            SignalRService = Environment.GetEnvironmentVariable("ANALISA_SIGNALRSERVICE", EnvironmentVariableTarget.Machine),
            TokenServer = Environment.GetEnvironmentVariable("ANALISA_TOKENSERVER", EnvironmentVariableTarget.Machine),
            QueueName = Environment.GetEnvironmentVariable("ANALISA_QUEUENAME", EnvironmentVariableTarget.Machine),
            ServiceBusConnectionString = Environment.GetEnvironmentVariable("ANALISA_SERVICEBUSCONNECTIONSTRING", EnvironmentVariableTarget.Machine),
          };
        else if (Environment.GetEnvironmentVariable("ANALISA_SERVER", EnvironmentVariableTarget.Process) != null)
          return new Config()
          {
            BlobKey = Environment.GetEnvironmentVariable("ANALISA_BLOBKEY", EnvironmentVariableTarget.Process),
            DataBase = Environment.GetEnvironmentVariable("ANALISA_DATABASE", EnvironmentVariableTarget.Process),
            DataBaseLog = Environment.GetEnvironmentVariable("ANALISA_DATABASELOG", EnvironmentVariableTarget.Process),
            SendGridKey = Environment.GetEnvironmentVariable("ANALISA_SENDGRIDKEY", EnvironmentVariableTarget.Process),
            Server = Environment.GetEnvironmentVariable("ANALISA_SERVER", EnvironmentVariableTarget.Process),
            ServerLog = Environment.GetEnvironmentVariable("ANALISA_SERVERLOG", EnvironmentVariableTarget.Process),
            SignalRService = Environment.GetEnvironmentVariable("ANALISA_SIGNALRSERVICE", EnvironmentVariableTarget.Process),
            TokenServer = Environment.GetEnvironmentVariable("ANALISA_TOKENSERVER", EnvironmentVariableTarget.Process),
            QueueName = Environment.GetEnvironmentVariable("ANALISA_QUEUENAME", EnvironmentVariableTarget.Process),
            ServiceBusConnectionString = Environment.GetEnvironmentVariable("ANALISA_SERVICEBUSCONNECTIONSTRING", EnvironmentVariableTarget.Process),
          };
        else if (Environment.GetEnvironmentVariable("ANALISA_SERVER", EnvironmentVariableTarget.User) != null)
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
        else
          return ReadConfig();

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
