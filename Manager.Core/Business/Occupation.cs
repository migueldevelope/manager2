using Manager.Core.Base;
using Manager.Core.BusinessModel;
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
    public Group Group { get; set; }
    public long Line { get; set; }
    public List<Skill> Skills { get; set; }
    public List<Schooling> Schooling { get; set; }
    public List<Activitie> Activities { get; set; }
    public Occupation Template { get; set; }
    public Cbo CBO { get; set; }
    public string SpecificRequirements { get; set; }
    public List<ProcessLevelTwo> Process { get; set; }
    public List<SalaryScaleGrade> SalaryScales { get; set; }
    public ViewListOccupation GetViewList()
    {
      return new ViewListOccupation()
      {
        _id = _id,
        Name = Name,
        Company = Group.Company.GetViewList(),
        Group = Group.GetViewList(),
        Line = Line,
        Process = Process.Select(p => new ViewListProcessLevelTwo()
        {
          _id = p._id,
          Name = p.Name,
          Order = p.Order,
          ProcessLevelOne = p.ProcessLevelOne.GetViewList()
        }).ToList()
      };
    }
  }
}
