using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
  public static class CSVService
  {

    public static List<Object> ImportCSV(string nameFile)
    {
      try
      {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Local_Data\{0}.csv", nameFile));
        StreamReader rd = new StreamReader(path);

        string row = null;
        string[] item = null;
        var list = new List<Object>();

        while ((row = rd.ReadLine()) != null)
        {
          item = row.Split(';');
          list.Add(item);
        }
        rd.Close();

        return list;
      }
      catch
      {
        Console.WriteLine("Error");
        return null;
      }
    }

    public static string ToCsv<T>(IList<T> list, string include, string exclude)
    {
      //Variables for build CSV string
      StringBuilder sb = new StringBuilder();
      List<string> propNames;
      List<string> propValues;
      bool isNameDone = false;

      //Get property collection and set selected property list
      PropertyInfo[] props = typeof(T).GetProperties();
      List<PropertyInfo> propList = GetSelectedProperties(props, include, exclude);

      //Add list name and total count
      string typeName = GetSimpleTypeName(list);
      sb.AppendLine(string.Format("{0} List - Total Count: {1}", typeName, list.Count.ToString()));

      //Iterate through data list collection
      foreach (var item in list)
      {
        sb.AppendLine("");
        propNames = new List<string>();
        propValues = new List<string>();

        //Iterate through property collection
        foreach (var prop in propList)
        {
          //Construct property name string if not done in sb
          if (!isNameDone) propNames.Add(prop.Name);

          //Construct property value string with double quotes for issue of any comma in string type data
          var val = prop.PropertyType == typeof(string) ? "\"{0}\"" : "{0}";
          propValues.Add(string.Format(val, prop.GetValue(item, null)));
        }
        //Add line for Names
        string line = string.Empty;
        if (!isNameDone)
        {
          line = string.Join(",", propNames);
          sb.AppendLine(line);
          isNameDone = true;
        }
        //Add line for the values
        line = string.Join(",", propValues);
        sb.Append(line);
      }
      return sb.ToString();
    }

    private static List<PropertyInfo> GetSelectedProperties(PropertyInfo[] props, string include, string exclude)
    {
      List<PropertyInfo> propList = new List<PropertyInfo>();
      if (include != "") //Do include first
      {
        var includeProps = include.ToLower().Split(',').ToList();
        foreach (var item in props)
        {
          var propName = includeProps.Where(a => a == item.Name.ToLower()).FirstOrDefault();
          if (!string.IsNullOrEmpty(propName))
            propList.Add(item);
        }
      }
      else if (exclude != "") //Then do exclude
      {
        var excludeProps = exclude.ToLower().Split(',');
        foreach (var item in props)
        {
          var propName = excludeProps.Where(a => a == item.Name.ToLower()).FirstOrDefault();
          if (string.IsNullOrEmpty(propName))
            propList.Add(item);
        }
      }
      else //Default
      {
        propList.AddRange(props.ToList());
      }
      return propList;
    }

    private static string GetSimpleTypeName<T>(IList<T> list)
    {
      string typeName = list.GetType().ToString();
      int pos = typeName.IndexOf("[") + 1;
      typeName = typeName.Substring(pos, typeName.LastIndexOf("]") - pos);
      typeName = typeName.Substring(typeName.LastIndexOf(".") + 1);
      return typeName;
    }
  }
}
