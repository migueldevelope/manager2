using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados e Coleção para planos
  /// </summary>
  public class StructPlan : BaseEntity
  {
    public ViewListCourse Course { get; set; }
    public EnumTypeAction TypeAction { get; set; }
    public EnumTypeResponsible TypeResponsible { get; set; }
    public ViewPlanActivity PlanActivity { get; set; }
  }
}
