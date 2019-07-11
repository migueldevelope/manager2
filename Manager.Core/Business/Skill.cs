using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class Skill : BaseEntity
  {
    public string Name { get; set; }
    public string Concept { get; set; }
    public EnumTypeSkill TypeSkill { get; set; }
    public string Template { get; set; }
    public ViewListSkill GetViewList()
    {
      return new ViewListSkill()
      {
        _id = _id,
        Concept = Concept,
        Name = Name,
        TypeSkill = TypeSkill
      };
    }
    public ViewCrudSkill GetViewCrud()
    {
      return new ViewCrudSkill()
      {
        _id = _id,
        Concept = Concept,
        Name = Name,
        TypeSkill = TypeSkill
      };
    }
  }
}
