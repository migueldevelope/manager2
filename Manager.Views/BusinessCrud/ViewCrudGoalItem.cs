using Manager.Views.BusinessList;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGoalItem : _ViewCrudBase
  {
    public ViewCrudGoal Goals { get; set; }
    public byte Weight { get; set; }
    public DateTime? Deadline { get; set; }
    public string Target { get; set; }
    public string Realized { get; set; }
    public string Result { get; set; }
    public decimal Achievement { get; set; }
    public double Points { get; set; }
    public double PointsWeight { get; set; }
    public double PointsTotal { get; set; }
  }
}
