using Manager.Core.Base;
using Manager.Core.Enumns;

namespace Manager.Core.Business
{
  public class StructPlan : BaseEntity
  {
    public Course Course { get; set; }
    public EnumTypeAction TypeAction { get; set; }
    public EnumTypeResponsible TypeResponsible { get; set; }
  }
}
