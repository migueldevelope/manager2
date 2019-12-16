using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudSkillsCareers: _ViewCrudBase
  {
    public long Count { get; set; }
    public byte Order { get; set; }
    public string Concept { get; set; }
    public EnumTypeSkill TypeSkill { get; set; }
  }
}
