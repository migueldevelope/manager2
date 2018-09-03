using Manager.Core.Base;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class CheckpointQuestions : BaseEntity
  {
    public Questions Question { get; set; }
    public byte Mark { get; set; }
    public List<CheckpointQuestions> Itens { get; set; }
  }
}
