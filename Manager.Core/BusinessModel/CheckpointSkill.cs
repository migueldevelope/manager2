using Manager.Core.Base;
using Manager.Core.Business;

namespace Manager.Core.BusinessModel
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
