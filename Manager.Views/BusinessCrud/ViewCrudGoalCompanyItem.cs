using Manager.Views.BusinessList;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGoalCompanyItem : _ViewCrudBase
  {
    public ViewListGoal Goals { get; set; }
    public string Goal { get; set; }
    public byte Weight { get; set; }
    public DateTime? Deadline { get; set; }
    public string Realized { get; set; }
    public string Result { get; set; }
    public decimal Achievement { get; set; }
  }
}
