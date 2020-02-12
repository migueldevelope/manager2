using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessView
{
  public class ViewListHistoricTraining: _ViewListBase
  {
    public string Occupation { get; set; }
    public string Manager { get; set; }
    public string Schooling { get; set; }
    public string Course { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DateEnd { get; set; }
    public decimal Workload { get; set; }
    public string Entity { get; set; }
    public EnumTypeHistoricTraining Type { get; set; }
  }
}
