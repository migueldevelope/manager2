using Manager.Core.Base;
using Manager.Core.Enumns;

namespace Manager.Core.Views
{
  public class ViewSkills: BaseEntity
  {
    public string Name { get; set; }
    public string Concept { get; set; }
    public EnumTypeSkill TypeSkill { get; set; }
    public bool Exists { get; set; }
    public bool ExistsGroup { get; set; }
    public bool ExistsOccupation { get; set; }
  }
}
