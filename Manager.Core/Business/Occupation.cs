using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class Occupation : BaseEntity
  {
    public string Name { get; set; }
    public ViewListGroup Group { get; set; }
    public long Line { get; set; }
    public List<ViewListSkill> Skills { get; set; }
    public List<ViewCrudSchooling> Schooling { get; set; }
    public List<ViewListActivitie> Activities { get; set; }
    public string Template { get; set; }
    public ViewListCbo Cbo { get; set; }
    public string SpecificRequirements { get; set; }
    public List<ViewListProcessLevelTwo> Process { get; set; }
    public List<SalaryScaleGrade> SalaryScales { get; set; }
    public ViewListOccupation GetViewList()
    {
      return new ViewListOccupation()
      {
        _id = _id,
        Name = Name,
        Group = Group,
        Line = Line,
        Process = Process,
        Cbo = Cbo
      };
    }
    public ViewListOccupationResume GetViewListResume()
    {
      return new ViewListOccupationResume()
      {
        _id = _id,
        Name = Name,
        Cbo = Cbo,
        _idGroup = Group._id,
        NameGroup = Group.Name,
        _idArea = Process.FirstOrDefault()?.ProcessLevelOne.Area._id
      };
    }
  }
}
