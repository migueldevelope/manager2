namespace Manager.Views.BusinessList
{
  public class ViewListGroup : _ViewListBase
  {
    public ViewListCompany Company { get; set; }
    public ViewListAxis Axis { get; set; }
    public ViewListSphere Sphere { get; set; }
    public long Line { get; set; }
  }
}
