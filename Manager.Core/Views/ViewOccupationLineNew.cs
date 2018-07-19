using Manager.Core.Enumns;

namespace Manager.Core.Views
{
  public class ViewOccupationLineNew
  {
    public string IdOccupation { get; set; }
    public string NameOccupation { get; set; }
    public string IdArea { get; set; }
    public string IdOccupationGroup { get; set; }
    public long Position { get; set; }
    public EnumTypeSphere TypeSphere { get; set; }
  }
}
