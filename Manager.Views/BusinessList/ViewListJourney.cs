using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListJourney
  {
    public List<ViewListOnBoarding> listOnBoarding { get; set; }
    public List<ViewListMonitoring> listMonitoring { get; set; }
    public List<ViewListCheckpoint> listCheckpoint { get; set; }

    public long totalOnBoarding { get; set; }
    public long totalMonitoring { get; set; }
    public long totalCheckpoint { get; set; }
  }
}
