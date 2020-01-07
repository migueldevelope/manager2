using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using System;

namespace Manager.Views.BusinessView
{
  public class ViewPersonNickName: _ViewList
  {
    public string Registration { get; set; }
    public string TypeUser { get; set; }
    public string StatusUser { get; set; }
    public string TypeJourney { get; set; }
    public ViewListCompany Company { get; set; }
    public ViewListEstablishment Establishment { get; set; }
    public ViewListOccupationResume Occupation { get; set; }
    public ViewBaseFields Manager { get; set; }
    public DateTime? DateResignation { get; set; }
  }
}
