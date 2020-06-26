using System;

namespace Manager.Views.BusinessView
{
  public class ViewGetOffBoarding
  {
    public string Manager { get; set; }
    public string Person { get; set; }
    public DateTime? DateOff { get; set; }
    public DateTime? DateRealized { get; set; }
    public decimal ScoreManager { get; set; }
    public decimal ScoreHR { get; set; }
    public decimal Diff { get; set; }
  }
}
