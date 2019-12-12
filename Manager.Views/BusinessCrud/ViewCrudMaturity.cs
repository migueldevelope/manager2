using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMaturity : _ViewCrud
  {
    public string _idPerson { get; set; }
    public long CountMonitoring { get; set; }
    public long CountPlan { get; set; }
    public long CountPraise { get; set; }
    public long CountCertification { get; set; }
    public long CountRecommendation { get; set; }
    public byte LevelMonitoring { get; set; }
    public byte LevelPlan { get; set; }
    public byte LevelPraise { get; set; }
    public byte LevelCertification { get; set; }
    public byte LevelRecommendation { get; set; }
    public byte Value { get; set; }
  }
}
