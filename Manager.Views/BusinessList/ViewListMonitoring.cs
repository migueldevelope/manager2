using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListMonitoring: _ViewListBase
  {
    public string idPerson { get; set; }
    public EnumStatusMonitoring StatusMonitoring { get; set; }
    public string OccupationName { get; set; }
    public DateTime? DateEndEnd { get; set; }
  }
}
