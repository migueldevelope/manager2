using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudStructPlan: _ViewCrud
  {
    public ViewListCourse Course { get; set; }
    public EnumTypeAction TypeAction { get; set; }
    public EnumTypeResponsible TypeResponsible { get; set; }
    public ViewPlanActivity PlanActivity { get; set; }
  }
}
