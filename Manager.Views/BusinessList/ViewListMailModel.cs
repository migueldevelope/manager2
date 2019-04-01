using Manager.Views.Enumns;

namespace Manager.Views.BusinessList
{
  public class ViewListMailModel : _ViewListBase
  {
    public string Subject { get; set; }
    public EnumStatus StatusMail { get; set; }
  }
}
