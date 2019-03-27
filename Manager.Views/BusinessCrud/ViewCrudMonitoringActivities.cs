using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMonitoringActivities: _ViewCrud
  {
    public ViewListActivitie Activities { get; set; }
    public string Praise { get; set; }
    public List<ViewCrudPlan> Plans { get; set; }
    public EnumTypeAtivitie TypeAtivitie { get; set; }
    public List<ViewCrudComment> Comments { get; set; }
    public EnumStatusView StatusViewManager { get; set; }
    public EnumStatusView StatusViewPerson { get; set; }
  }
}
