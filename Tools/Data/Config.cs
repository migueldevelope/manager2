using System;
using System.Xml.Serialization;

namespace Tools.Data
{
  [XmlRoot("Config")]
  [Serializable]
  public class Config
  {
    [XmlElement("Server")]
    public string Server { get; set; }
    [XmlElement("DataBase")]
    public string DataBase { get; set; }
    [XmlElement("TokenServer")]
    public string TokenServer { get; set; }
    [XmlElement("SignalRService")]
    public string SignalRService { get; set; }
    [XmlElement("BlobKey")]
    public string BlobKey { get; set; }
    [XmlElement("SendGridKey")]
    public string SendGridKey { get; set; }
    [XmlElement("ServerLog")]
    public string ServerLog { get; set; }
    [XmlElement("DataBaseLog")]
    public string DataBaseLog { get; set; }
    [XmlElement("ServerIntegration")]
    public string ServerIntegration { get; set; }
    [XmlElement("DataBaseIntegration")]
    public string DataBaseIntegration { get; set; }
    [XmlElement("ServiceBusConnectionString")]
    public string ServiceBusConnectionString { get; set; }
  }
}
