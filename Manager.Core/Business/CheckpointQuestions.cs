using Manager.Core.Base;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Coleção para perguntas do checkpoint
  /// </summary>
  public class CheckpointQuestions : BaseEntity
  {
    public Questions Question { get; set; }
    public byte Mark { get; set; }
    public List<CheckpointQuestions> Itens { get; set; }
  }
}
