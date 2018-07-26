using Manager.Core.Business;

namespace Manager.Core.Views
{
  public class ViewAddGroup
  {
    public string Name { get; set; }
    public Axis Axis { get; set; }
    public Sphere Sphere { get; set; }
    public Company Company { get; set; }
    public long Line { get; set; }
  }
}
