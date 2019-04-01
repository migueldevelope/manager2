using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMailModel : _ViewCrudBase
  {
    public string Message { get; set; }
    public string Subject { get; set; }
    public string Link { get; set; }
    public EnumStatus StatusMail { get; set; }
  }
}
