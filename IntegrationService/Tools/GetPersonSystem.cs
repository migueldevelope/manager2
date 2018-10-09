using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OracleTools;
using SqlServerTools;

namespace IntegrationService.Tools
{
  public class GetPersonSystem
  {
    private readonly ConnectionString Conn;
    public GetPersonSystem(ConnectionString conn)
    {
      Conn = conn;
    }
    public DataTable GetPerson()
    {
      DataTable dados;
      if (Conn.Oracle)
      {
        OracleConnectionTool conn = new OracleConnectionTool(Conn.HostName, Conn.User, Conn.Password);
        dados = conn.ExecuteQuery(Conn.Sql);
        conn.Close();
      }
      else
      {
        SqlConnectionTool conn = new SqlConnectionTool(Conn.HostName, Conn.User, Conn.Password, Conn.DatabaseDefault);
        dados = conn.ExecuteQuery(Conn.Sql);
        conn.Close();
      }
      return dados;
    }
  }
}
