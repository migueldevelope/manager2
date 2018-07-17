using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class MailModel : BaseEntity
  {
    public string Name { get; set; }
    public string Message { get; set; }
    public string Subject { get; set; }
    public string Link { get; set; }
  }
}
