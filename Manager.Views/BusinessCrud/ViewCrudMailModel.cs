using Manager.Views.Enumns;
using System;

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
    public DayOfWeek Weekly { get; set; }
    public EnumTypeMailModel TypeMailModel { get; set; }
  }
}
