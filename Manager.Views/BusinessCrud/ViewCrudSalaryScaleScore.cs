using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudSalaryScaleScore:_ViewCrudBase
  {
    public EnumSteps Step { get; set; }
    public byte Ranking { get; set; }
    public decimal Value { get; set; }
  }
}
