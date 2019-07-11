using Manager.Core.Base;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Business
{
  public class LogMessages : BaseEntity
  {
    public ViewListPersonBase Person { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTime Register { get; set; }
    public EnumStatusMessage StatusMessage { get; set; }
  }
}
