using System.Data;
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
      switch (Conn.DatabaseType)
      {
        case Enumns.EnumDatabaseType.Oracle:
          OracleConnectionTool connOra = new OracleConnectionTool(Conn.HostName, Conn.User, Conn.Password);
          dados = connOra.ExecuteQuery(Conn.Sql);
          connOra.Close();
          break;
        case Enumns.EnumDatabaseType.ODBC:
          SqlConnectionTool connOdbc = new SqlConnectionTool(Conn.HostName, Conn.User, Conn.Password, Conn.DatabaseDefault);
          dados = connOdbc.ExecuteQuery(Conn.Sql);
          connOdbc.Close();
          break;
        default:
          SqlConnectionTool connSql = new SqlConnectionTool(Conn.HostName, Conn.User, Conn.Password, Conn.DatabaseDefault);
          dados = connSql.ExecuteQuery(Conn.Sql);
          connSql.Close();
          break;
      }
      return dados;
    }
  }
}
