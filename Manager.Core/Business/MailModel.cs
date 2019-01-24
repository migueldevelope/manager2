using Manager.Core.Base;
using Manager.Core.Enumns;

namespace Manager.Core.Business
{
  public class MailModel : BaseEntity
  {
    public string Name { get; set; }
    public string Message { get; set; }
    public string Subject { get; set; }
    public string Link { get; set; }
    public EnumStatus StatusMail { get; set; }
  }
}
