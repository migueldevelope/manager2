using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewFluidCareers : _ViewCrud
  {
    public string Sphere {get;set;}
    public string Group { get; set; }
    public string Occupation { get; set; }
    public decimal Accuracy { get; set; }
    public EnumOccupationColor Color { get; set; }
    public byte Order { get; set; }
  }
}
