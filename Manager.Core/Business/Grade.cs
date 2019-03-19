using Manager.Core.Base;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Grade : BaseEntity
  {
    public string Name { get; set; }
    public EnumSteps StepMedium { get; set; }
    public List<ListSteps> ListSteps { get; set; }
    public int Order { get; set; }
  }
}
