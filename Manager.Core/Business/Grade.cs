using Manager.Core.Base;
using Manager.Core.Enumns;

namespace Manager.Core.Business
{
  public class Grade : BaseEntity
  {
    public Company Company { get; set; }
    public string Name { get; set; }
    public EnumSteps StepMedium { get; set; }
    public int Order { get; set; }
  }
}
