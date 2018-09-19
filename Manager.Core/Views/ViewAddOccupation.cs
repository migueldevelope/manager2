using Manager.Core.Business;
using System.Collections.Generic;

namespace Manager.Core.Views
{
  public class ViewAddOccupation
  {
    public Group Group { get; set; }
    public string Name { get; set; }
    public long Line { get; set; }
    public Area Area { get; set; }
    public ProcessLevelTwo ProcessLevelTwo { get; set; }
    public List<ProcessLevelTwo> Process { get; set; }
  }
}
