using Manager.Core.Base;
using System;

namespace Manager.Core.Business
{
  /// <summary>
  /// Coleção para dias de treinamento da turma
  /// </summary>
  public class DaysEvent : BaseEntity
  {
    public DateTime Begin { get; set; }
    public DateTime End { get; set; }
  }
}
