using Manager.Core.Base;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class ProcessLevelOne : BaseEntity
  {
    public string Name { get; set; }
    public Area Area { get; set; }
    public List<ProcessLevelTwo> Process { get; set; }
  }
}
