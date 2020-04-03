using Manager.Core.Base;
using Manager.Views.Enumns;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  ///  coleção
  /// </summary>
  public class SalaryScaleGrade : BaseEntityId
  {
    public string _idGrade { get; set; }
    public string NameGrade { get; set; }
    public string _idSalaryScale { get; set; }
    public string NameSalaryScale { get; set; }
    public int Workload { get; set; }
    public EnumSteps StepLimit { get; set; }
  }
}
