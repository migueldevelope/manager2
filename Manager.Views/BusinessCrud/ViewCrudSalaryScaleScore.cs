using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudSalaryScaleScore:_ViewCrudBase
  {
    public EnumSteps Step { get; set; }
    public byte CountSteps { get; set; }
    public decimal Value { get; set; }
  }
}
