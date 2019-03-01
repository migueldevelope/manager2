using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudSphere : _ViewCrudBase
  {
    public EnumTypeSphere TypeSphere { get; set; }
    public ViewListCompany Company { get; set; }
  }
}
