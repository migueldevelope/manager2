using System;
using System.Linq;

namespace Tools
{
  public static class ConnectionNoSqlService
  {
    public static Data.ConnNoSql GetConnetionServer()
    {
      try
      {
        XmlConnectionNoSqlService<Tools.Data.ConnNoSql> xml = new XmlConnectionNoSqlService<Tools.Data.ConnNoSql>();
        return xml.Get();
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static string GetConnetionServers(string hostName)
    {
      try
      {
        XmlConnectionNoSqlService<Tools.Data.ConnNoSql> xml = new XmlConnectionNoSqlService<Tools.Data.ConnNoSql>(hostName);
        Tools.Data.ConnNoSql connection = xml.Get();
        return connection.Server;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static string GetConnetionDataBase(string hostName)
    {
      try
      {
        XmlConnectionNoSqlService<Tools.Data.ConnNoSql> xml = new XmlConnectionNoSqlService<Tools.Data.ConnNoSql>(hostName);
        Tools.Data.ConnNoSql connection = xml.Get();
        return connection.DataBase;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static string GetTokenServer(string hostName)
    {
      try
      {
        XmlConnectionNoSqlService<Tools.Data.ConnNoSql> xml = new XmlConnectionNoSqlService<Tools.Data.ConnNoSql>(hostName);
        Tools.Data.ConnNoSql connection = xml.Get();
        return connection.TokenServer;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static string GetPath(string hostName)
    {
      try
      {
        XmlConnectionNoSqlService<Tools.Data.ConnNoSql> xml = new XmlConnectionNoSqlService<Tools.Data.ConnNoSql>(hostName);
        Tools.Data.ConnNoSql connection = xml.Get();
        return xml.FileName;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
