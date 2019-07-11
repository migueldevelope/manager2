using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using System.Collections.Generic;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para perguntas do checkpoint
  /// </summary>
  public class CheckpointQuestions : BaseEntityId
  {
    public ViewCrudQuestions Question { get; set; }
    public byte Mark { get; set; }
    public List<CheckpointQuestions> Itens { get; set; }
  }
}
