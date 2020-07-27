using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMailModel : _ViewCrudBase
  {
    public string Message { get; set; }
    public string Subject { get; set; }
    public string Link { get; set; }

    public EnumTypeFrequence TypeFrequence { get; set; }
    public EnumStatus StatusMail { get; set; }
    public byte Day { get; set; }
    public byte Weekly { get; set; }
  }
}
