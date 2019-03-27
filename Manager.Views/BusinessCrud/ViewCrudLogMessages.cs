using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudLogMessages : _ViewCrud
  {
    public ViewListPerson Person { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTime Register { get; set; }
    public EnumStatusMessage StatusMessage { get; set; }
  }
}
