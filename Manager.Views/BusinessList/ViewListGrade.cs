using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListGrade : _ViewListBase
  {
    public EnumSteps StepMedium { get; set; }
    public List<ViewListStep> Steps { get; set; }
    public int Order { get; set; }
    public List<_ViewListBase> Occupation { get; set; }
  }
}
