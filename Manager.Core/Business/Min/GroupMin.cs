namespace Manager.Core.Business.Min
{
  public class GroupMin : _BaseMin
  {
    public CompanyMin Company { get; set; }
    public AxisMin Axis { get; set; }
    public SphereMin Sphere { get; set; }
    public long Line { get; set; }
  }
}
