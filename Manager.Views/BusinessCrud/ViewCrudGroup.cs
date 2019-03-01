using Manager.Views.BusinessList;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGroup : _ViewCrudBase
  {
    public ViewListCompany Company { get; set; }
    public ViewListAxis Axis { get; set; }
    public ViewListSphere Sphere { get; set; }
    public long Line { get; set; }

  }
}
