using Manager.Core.Base;
using Manager.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para acreditação
  /// </summary>
  public class CertificationQuestions: BaseEntity
  {
    public Questions Question { get; set; }
    public string Answer { get; set; }
  }
}
