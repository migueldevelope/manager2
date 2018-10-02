using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using System.Text;

namespace OracleTools
{
  public class OracleConnectionTool
  {
    public string HostName { get; private set; }
    public string User { get; private set; }
    public string Password { get; private set; }

    private OracleConnection mDbConnecion;
    public OracleConnectionTool(string hostName, string user, string password)
    {
      HostName = hostName;
      User = user;
      Password = password;
    }

    private string GetStringConnection()
    {
      StringBuilder wordConnection = new StringBuilder();
      wordConnection.Append("Data Source=");
      wordConnection.Append(HostName);
      wordConnection.Append(";User ID=");
      wordConnection.Append(User);
      wordConnection.Append(";Password=");
      wordConnection.Append(Password);
      wordConnection.Append(";Pooling=true");
      wordConnection.Append(";Connection Timeout=7200");
      wordConnection.Append(";Min Pool Size=0");
      wordConnection.Append(";Pool Regulator=1");
      return wordConnection.ToString();
    }
    private void Open()
    {
      try
      {
        if (mDbConnecion == null)
        {
          mDbConnecion = new OracleConnection(String.Concat(GetStringConnection()));
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
          OracleConnection.ClearAllPools();
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
        OracleCommand command = new OracleCommand
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
        OracleCommand oCommand = new OracleCommand
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
