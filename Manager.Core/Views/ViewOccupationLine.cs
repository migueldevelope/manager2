using Manager.Core.Enumns;

namespace Manager.Core.Views
{
  public class ViewOccupationLine
  {
    public string IdOccupation { get; set; }
    public string NameOccupation { get; set; }
    public string IdArea { get; set; }
    public string NameArea { get; set; }
    public string IdCompany { get; set; }
    public string NameCompany { get; set; }
    public string IdOccupationGroup { get; set; }
    public string NameOccupationGroup { get; set; }
    public EnumTypeSphere Sphere { get; set; }
    public long Position { get; set; }
  }
}
