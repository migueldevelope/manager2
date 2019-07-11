using Manager.Core.Business;
using Manager.Views.BusinessList;

namespace Manager.Core.Views
{
  public class ViewAddGroup
  {
    public string Name { get; set; }
    public ViewListAxis Axis { get; set; }
    public ViewListSphere Sphere { get; set; }
    public ViewListCompany Company { get; set; }
    public long Line { get; set; }
  }
}
