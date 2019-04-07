using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados e Coleção para planos
  /// </summary>
  public class StructPlan : BaseEntity
  {
    public Course Course { get; set; }
    public EnumTypeAction TypeAction { get; set; }
    public EnumTypeResponsible TypeResponsible { get; set; }
    public PlanActivity PlanActivity { get; set; }
  }
}
