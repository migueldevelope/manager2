using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListEventDetail : ViewListEvent
  {
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
    public EnumStatusEvent StatusEvent { get; set; }
  }
}
