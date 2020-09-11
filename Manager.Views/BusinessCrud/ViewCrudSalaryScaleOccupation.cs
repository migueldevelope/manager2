using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudSalaryScaleOccupation : _ViewCrudBase
  {
    public string _idGrade { get; set; }
    public string NameGrade { get; set; }
    public int Workload { get; set; }
    public EnumSteps StepLimit { get; set; }
  }
}
