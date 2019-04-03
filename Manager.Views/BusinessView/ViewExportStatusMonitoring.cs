using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessView
{
  public class ViewExportStatusMonitoring
  {
    public string IdMonitoring { get; set; }
    public string NamePerson { get; set; }
    public EnumStatusMonitoring Status { get; set; }
    public string Occupation { get; set; }
    public DateTime? DataEnd { get; set; }
  }
}
