using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudStep
  {
    public string _idSalaryScale { get; set; }
    public string _idGrade { get; set; }
    public EnumSteps Step { get; set; }
    public decimal Salary { get; set; }
  }
}
