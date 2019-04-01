using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudTrainingPlan: _ViewCrud
  {
    public ViewListPersonResume Person { get; set; }
    public ViewListCourse Course { get; set; }
    public DateTime? Deadline { get; set; }
    public EnumOrigin Origin { get; set; }
    public DateTime? Include { get; set; }
    public EnumStatusTrainingPlan StatusTrainingPlan { get; set; }
    public string Observartion { get; set; }
    public ViewListEvent Event { get; set; }
  }
}
