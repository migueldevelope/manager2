using Manager.Core.Base;

namespace Manager.Core.Business
{
  /// <summary>
  /// Coleção para envio de email's
  /// </summary>
  public class MailLogAddress: BaseEntity
  {
    public string Email { get; set; }
    public string Name { get; set; }

    public MailLogAddress(string email, string name)
    {
      Email = email;
      Name = name;
    }
  }
}
