using Manager.Views.Enumns;

namespace Manager.Views.BusinessList
{
  public class ViewListObjectiveResponsibleCard
  {
    public decimal AverageAchievement { get; set; }
    public decimal AverageTrust { get; set; }
    public EnumLevelTrust LevelTrust { get; set; }
    public long QtdObjective { get; set; }
  }
}
