using Manager.Views.BusinessCrud;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListObjectiveEdit : _ViewList
  {
    public string Description { get; set; }
    public string Detail { get; set; }
    public decimal AverageAchievement { get; set; }
    public decimal AverageTrust { get; set; }
    public List<ViewListPersonPhoto> Editors { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? StartDate { get; set; }
    public byte LevelAchievement { get; set; }
    public byte LevelTrust { get; set; }
    public ViewListPersonPhoto Responsible { get; set; }
    public long QuantityImpediments { get; set; }
    public long QuantityIniciatives { get; set; }
    public bool PendingCheckinTrust { get; set; }
    public bool PendingCheckinAchievement { get; set; }
    public List<ViewListKeyResultsEdit> KeyResults { get; set; }
  }
}
