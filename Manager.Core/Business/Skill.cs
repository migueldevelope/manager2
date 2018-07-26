using Manager.Core.Base;
using Manager.Core.Enumns;

namespace Manager.Core.Business
{
  public class Skill : BaseEntity
  {
    public string Name { get; set; }
    public string Concept { get; set; }
    public EnumTypeSkill TypeSkill { get; set; }
    public Skill Template { get; set; }
  }
}
