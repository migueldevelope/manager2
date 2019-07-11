using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessList;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para acreditação
  /// </summary>
  public class CertificationQuestions: BaseEntityId
  {
    public ViewListQuestions Question { get; set; }
    public string Answer { get; set; }
  }
}
