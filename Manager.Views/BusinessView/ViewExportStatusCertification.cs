using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessView
{
  public class ViewExportStatusCertification
  {
    public string NameManager {get;set;}
    public string NamePerson { get;set;}
    public string NameItem { get;set;}
    public string Status { get;set;}
    public DateTime? Date { get;set;}
  }
}
