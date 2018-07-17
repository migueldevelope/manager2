using System;
using System.Xml.Serialization;

namespace Tools.Data
{
  [XmlRoot("ConnNoSql")]
  [Serializable]
  public class ConnNoSql
  {
    [XmlElement("Server")]
    public string Server { get; set; }
    [XmlElement("DataBase")]
    public string DataBase { get; set; }
    [XmlElement("TokenServer")]
    public string TokenServer { get; set; }
    [XmlElement("BlobKey")]
    public string BlobKey { get; set; }
    [XmlElement("SendGridKey")]
    public string SendGridKey { get; set; }
  }
}
