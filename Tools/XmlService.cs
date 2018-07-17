using System;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Tools
{
  public abstract class XmlService<TEntity> where TEntity : class
  {
    public readonly string FileName;

    protected XmlService(string fileName)
    {
      this.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Local_Data/{0}.xml", fileName));
    }

    protected XmlService(string hostName, string fileName)
    {
      this.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Local_Data/{0}/{1}.xml", hostName, fileName));
    }

    public virtual void Clear()
    {
      XmlTextWriter writer = new XmlTextWriter(FileName, null);
      writer.Flush();
      writer.Close();
    }

    public virtual TEntity Get()
    {
      try
      {
        XmlSerializer xs = new XmlSerializer(typeof(TEntity));
        using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
        {
          return (TEntity)xs.Deserialize(fs);
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public virtual bool Exists()
    {
      try
      {
        if (File.Exists(FileName))
          return true;
        else
          return false;
      }
      catch (Exception)
      {
        return false;
      }
    }


  }
}
