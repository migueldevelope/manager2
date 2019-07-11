using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessList;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para o checkpoint
  /// </summary>
  public class CheckpointSkill : BaseEntityId
  {
    public ViewListSkill Skill { get; set; }
    public byte Mark { get; set; }
  }
}
