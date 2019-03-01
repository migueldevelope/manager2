using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudAxis : _ViewCrudBase
  {
    public ViewListCompany Company { get; set; }
    public ViewListSphere Sphere { get; set; }
    public EnumTypeAxis TypeAxis { get; set; }
  }
}
