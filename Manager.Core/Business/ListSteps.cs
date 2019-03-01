using Manager.Core.Base;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  public class ListSteps : BaseEntity
  {
    public EnumSteps Step { get; set; }
    public decimal Salary { get; set; }
  }
}
