using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessView
{
  public class ViewExportStatusPlan
  {
    public string NameManager { get; set; }
    public string NamePerson { get; set; }
    public string What { get; set; }
    public string Description { get; set; }
    public DateTime? Deadline { get; set; }
    public string Status { get; set; }
    public string Obs { get; set; }
    public DateTime? DateEnd { get; set; }
  }
}
