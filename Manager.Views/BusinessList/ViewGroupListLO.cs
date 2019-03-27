
namespace Manager.Views.BusinessList
{
  public class ViewGroupListLO: _ViewListBase
  {
    public ViewListAxis Axis { get; set; }
    public ViewListSphere Sphere { get; set; }
    public ViewListCompany Company { get; set; }
    public long Line { get; set; }
    public long ScopeCount { get; set; }
    public long SkillCount { get; set; }
    public long SchollingCount { get; set; }
  }
}
