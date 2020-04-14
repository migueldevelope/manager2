using System;
using System.Data;
using System.Data.Odbc;
using System.Globalization;

namespace IntegrationClient.DatabaseTools.OdbcTools
{
  public class OdbcConnectionTool
  {
    public string ConnectionString { get; private set; }

    private OdbcConnection mDbConnecion;
    public OdbcConnectionTool(string connectionString)
    {
      ConnectionString = connectionString;
    }

    private void Open()
    {
      try
      {
        if (mDbConnecion == null)
        {
          mDbConnecion = new OdbcConnection(ConnectionString);
          mDbConnecion.Open();
        }
      }
      catch (Exception)
      {

        throw;
      }
    }
    public void Close()
    {
      if (mDbConnecion != null)
      {
        if (mDbConnecion.State != ConnectionState.Closed)
        {
          mDbConnecion.Close();
          mDbConnecion.Dispose();
        }
        mDbConnecion = null;
      }
    }

    public int ExecuteScalar(string sqlText)
    {
      try
      {
        Open();
        OdbcCommand command = new OdbcCommand
        {
          Connection = mDbConnecion,
          CommandTimeout = mDbConnecion.ConnectionTimeout,
          CommandText = sqlText
        };
        int register = (int)command.ExecuteScalar();
        command.Dispose();
        return register;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public DataTable ExecuteQuery(string sqlText)
    {
      try
      {
        DataTable dttR = new DataTable
        {
          Locale = CultureInfo.CurrentCulture
        };
        Open();
        OdbcCommand oCommand = new OdbcCommand
        {
          Connection = mDbConnecion,
          CommandText = sqlText,
          CommandTimeout = mDbConnecion.ConnectionTimeout
        };
        dttR.Load(oCommand.ExecuteReader(CommandBehavior.SingleResult));
        return dttR;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
