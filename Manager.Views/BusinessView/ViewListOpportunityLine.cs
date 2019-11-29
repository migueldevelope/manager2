using Manager.Views.Enumns;

namespace Manager.Views.BusinessView
{
  public class ViewListOpportunityLine
  {
    public string Area { get; set; }
    public string Shepre { get; set; }
    public EnumTypeSphere TypeShepre { get; set; }
    public string Axis { get; set; }
    public EnumTypeAxis TypeAxis { get; set; }
    public string Group { get; set; }
    public long LineGroup { get; set; }
    public string ProcessLevelOne { get; set; }
    public string ProcessLevelTwo { get; set; }
    public string Occupation { get; set; }
  }
}
