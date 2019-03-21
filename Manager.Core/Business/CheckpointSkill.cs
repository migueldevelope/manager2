using Manager.Core.Base;

namespace Manager.Core.Business
{
  /// <summary>
  /// Coleção para o checkpoint
  /// </summary>
  public class CheckpointSkill : BaseEntity
  {
    public Skill Skill { get; set; }
    public byte Mark { get; set; }
  }
}
