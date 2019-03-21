using Manager.Core.Base;
using Manager.Core.Enumns;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  ///  coleção
  /// </summary>
  public class SalaryScaleGrade : BaseEntity
  {
    public string _idGrade { get; set; }
    public string NameGrade { get; set; }
    public string _idSalaryScale { get; set; }
    public string NameSalaryScale { get; set; }
  }
}
