using Manager.Core.Base;

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
  }
}
