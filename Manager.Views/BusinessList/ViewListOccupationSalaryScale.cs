using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListOccupationSalaryScale : _ViewListBase
  {
    public int Wordload { get; set; }
    public List<ViewListStep> Steps { get; set; }
    public List<ViewListProcessLevelTwo> Process { get; set; }
    public string Description { get; set; }
    public EnumSteps StepLimit { get; set; }

  }
}
