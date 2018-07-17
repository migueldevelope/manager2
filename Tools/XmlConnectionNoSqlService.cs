using System;
using System.Linq;

namespace Tools
{
  public class XmlConnectionNoSqlService<TEntity> : XmlService<Tools.Data.ConnNoSql> where TEntity : class
  {
    public XmlConnectionNoSqlService(string hostName)
      : base(hostName, "ConnNoSql")
    {

    }
    public XmlConnectionNoSqlService()
      : base("ConnNoSql")
    {

    }
  }
}
