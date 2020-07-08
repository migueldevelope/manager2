using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessList
{
  public class ViewListIdIndicators : _ViewListBase
  {
    public EnumTypeJourney TypeJourney { get; set; }
    public string OccupationName { get; set; }
    public DateTime? DateAdm { get; set; }
    public string Manager { get; set; }
    public DateTime? DateLastOccupation { get; set; }
  }
}
