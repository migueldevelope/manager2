using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace IntegrationClient.DatabaseTools.SqlServerTools
{
  public class SqlConnectionTool
  {
    public string HostName { get; private set; }
    public string User { get; private set; }
    public string Password { get; private set; }
    public string DatabaseDefault { get; private set; }

    private SqlConnection mDbConnecion;
    public SqlConnectionTool(string hostName, string user, string password, string databaseDefault)
    {
      HostName = hostName;
      User = user;
      Password = password;
      DatabaseDefault = databaseDefault;
    }

    private string GetStringConnection()
    {
      StringBuilder wordConnection = new StringBuilder();
      wordConnection.Append("Password=");
      wordConnection.Append(Password);
      wordConnection.Append(";Persist Security Info=True;User ID=");
      wordConnection.Append(User);
      wordConnection.Append(";Initial Catalog=");
      wordConnection.Append(DatabaseDefault);
      wordConnection.Append(";Data Source=");
      wordConnection.Append(HostName);
      wordConnection.Append(";Connection Timeout=7200");
      return wordConnection.ToString();
    }
    private void Open()
    {
      try
      {
        if (mDbConnecion == null)
        {
          mDbConnecion = new SqlConnection(string.Concat(GetStringConnection()));
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
          SqlConnection.ClearAllPools();
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
        SqlCommand command = new SqlCommand
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
        SqlCommand oCommand = new SqlCommand
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
