using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudKeyResult:_ViewCrudBase
  {
    public EnumTypeKeyResult TypeKeyResult { get; set; }
    public decimal QuantityGoal { get; set; }
    public string QualityGoal { get; set; }
    public decimal BeginProgressGoal { get; set; }
    public decimal EndProgressGoal { get; set; }
    public EnumSense Sense { get; set; }
    public string Description { get; set; }
    public byte Weight { get; set; }
    public ViewListObjective Objective { get; set; }
    public EnumTypeCheckin TypeCheckin { get; set; }
    public List<ViewListPersonPhoto> Participants { get; set; }
  }
}
