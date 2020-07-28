using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessList
{
  public class ViewListMailModel : _ViewListBase
  {
    public string Subject { get; set; }
    public string Link { get; set; }
    public EnumStatus StatusMail { get; set; }
    public EnumTypeFrequence TypeFrequence { get; set; }
    public byte Day { get; set; }
    public DayOfWeek Weekly { get; set; }
    public EnumTypeMailModel TypeMailModel { get; set; }
  }
}
