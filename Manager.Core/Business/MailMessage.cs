using Manager.Core.Base;
using Manager.Core.Enumns;

namespace Manager.Core.Business
{
  public class MailMessage : BaseEntity
  {
    public string Name { get; set; }
    public string Url { get; set; }
    public string Body { get; set; }
    public EnumTypeMailMessage Type { get; set; }
    public string Token { get; set; }
  }
}
