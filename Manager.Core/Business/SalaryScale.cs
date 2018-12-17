using Manager.Core.Base;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class SalaryScale : BaseEntity
  {
    public Establishment Establishment { get; set; }
    public Grade Grade { get; set; }
    public List<ListSteps> ListSteps { get; set; }
  }
}
