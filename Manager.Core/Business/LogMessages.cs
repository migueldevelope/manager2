using Manager.Core.Base;
using Manager.Core.Enumns;

namespace Manager.Core.Business
{
  public class LogMessages : BaseEntity
  {
    public Person Person { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public EnumStatusMessage StatusMessage { get; set; }
  }
}
