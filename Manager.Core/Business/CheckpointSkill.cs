using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class CheckpointSkill : BaseEntity
  {
    public Skill Skill { get; set; }
    public byte Mark { get; set; }
  }
}
