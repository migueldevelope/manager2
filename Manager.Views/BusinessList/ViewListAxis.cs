using Manager.Views.Enumns;

namespace Manager.Views.BusinessList
{
  public class ViewListAxis : _ViewListBase
  {
    public string IdCompany { get; set; }
    public string NameCompany { get; set; }
    public string IdSphere { get; set; }
    public string NameSphere { get; set; }
    public EnumTypeAxis TypeAxis { get; set; }
  }
}
