using Manager.Core.Enumns;

namespace Manager.Core.Business.Min
{
  public class GradeMin : _BaseMin
  {
    public CompanyMin Company { get; set; }
    public EnumSteps StepMedium { get; set; }
    public int Order { get; set; }
  }
}
