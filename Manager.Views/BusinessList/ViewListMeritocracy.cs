using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessList
{
  public class ViewListMeritocracy : _ViewListBase
  {
    public string _idPerson { get; set; }
    public EnumStatusMeritocracy StatusMeritocracy {get;set;}
    public string OccupationName { get; set; }
    public DateTime? OccupationDate { get; set; }
    public DateTime? CompanyDate { get; set; }
  }
}
