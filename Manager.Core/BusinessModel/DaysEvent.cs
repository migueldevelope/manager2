using Manager.Core.Base;
using System;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para dias de treinamento da turma
  /// </summary>
  public class DaysEvent : BaseEntityId
  {
    public DateTime Begin { get; set; }
    public DateTime End { get; set; }
  }
}
