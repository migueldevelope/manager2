using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class SalaryScaleGrade: BaseEntity
  {
    public string _idSalaryScale { get; set; }
    public string Name { get; set; }
    public Grade Grade { get; set; }
  }
}
