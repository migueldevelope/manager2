using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessList
{
  public class ViewListMeritocracy : _ViewListBase
  {
    public string _idPerson { get; set; }
    public string OccupationName { get; set; }
    public EnumStatusMeritocracy StatusMeritocracy {get;set;}
  }
}
