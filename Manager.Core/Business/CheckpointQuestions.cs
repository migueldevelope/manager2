using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class CheckpointQuestions : BaseEntity
  {
    public Questions Question { get; set; }
    public byte Mark { get; set; }
  }
}
