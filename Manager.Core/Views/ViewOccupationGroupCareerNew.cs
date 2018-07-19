using Manager.Core.Enumns;

namespace Manager.Core.Views
{
  public class ViewOccupationGroupCareerNew
  {
    public string IdCareer { get; set; }
    public string NameCareer { get; set; }
    public string NameOccupationGroup { get; set; }
    public string IdCompany { get; set; }
    public string NameCompany { get; set; }
    public EnumTypeSphere TypeSphere { get; set; }
    public EnumTypeAxis TypeAxis { get; set; }
    public long Position { get; set; }
  }
}
