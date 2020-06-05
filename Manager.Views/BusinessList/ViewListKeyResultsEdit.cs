using Manager.Views.BusinessCrud;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListKeyResultsEdit : ViewCrudKeyResult
  {
    public byte LevelAchievement { get; set; }
    public byte LevelTrust { get; set; }
    public decimal Achievement { get; set; }
    public bool PendingChecking { get; set; }
  }
}
