using IntegrationService.Enumns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationService.Tools
{
  public class ConnectionString
  {
    public EnumDatabaseType DatabaseType { get; private set; }
    public string HostName { get; private set; }
    public string User { get; private set; }
    public string Password { get; private set; }
    public string DatabaseDefault { get; private set; }
    public string Sql { get; set; }

    public ConnectionString(string hostname, string user, string password, string databasedefault)
    {
      HostName = hostname;
      User = user;
      Password = password;
      DatabaseDefault = databasedefault;
      DatabaseType = EnumDatabaseType.SqlServer;
    }
    public ConnectionString(string hostname, string user, string password)
    {
      HostName = hostname;
      User = user;
      Password = password;
      DatabaseDefault = string.Empty;
      DatabaseType = EnumDatabaseType.Oracle;
    }
  }
}
