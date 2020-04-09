using System;

namespace Manager.Views.BusinessList
{
  public class ViewListSalaryScaleLog : _ViewListBase
  {
    public ViewListCompany Company { get; set; }
    public DateTime? Date { get; set; }
    public string Description { get; set; }
  }
}
