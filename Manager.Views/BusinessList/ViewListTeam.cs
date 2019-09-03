using Manager.Views.BusinessView;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListTeam
  {
    public List<ViewListPersonTeam> Team { get; set; }
    public List<ViewAutoManagerPerson> AutoManager { get; set; }
    public List<ViewAutoManager> Approved { get; set; }
    public long totalTeam { get; set; }
    public long totalAutoManager { get; set; }
    public long totalApproved { get; set; }
  }
}
