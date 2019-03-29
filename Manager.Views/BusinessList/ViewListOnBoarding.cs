using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessList
{
  public class ViewListOnBoarding : _ViewListBase
  {
    public string _idPerson { get; set; }
    public string OccupationName { get; set; }
    public EnumStatusOnBoarding StatusOnBoarding { get; set; }
    public DateTime? DateEndEnd { get; set; }
  }
}
