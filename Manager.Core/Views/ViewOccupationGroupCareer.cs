using Manager.Core.Enumns;

namespace Manager.Core.Views
{
  public class ViewOccupationGroupCareer
  {
    public string NameCompany { get; set; }
    public string IdCompany { get; set; }
    public string Name { get; set; }
    public string Id { get; set; }
    public EnumTypeAxis Axis { get; set; }
    public EnumTypeSphere Sphere { get; set; }
    public long Position { get; set; }
  }
}
