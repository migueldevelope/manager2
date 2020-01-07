using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessView
{
  public class ViewPersonNickName: _ViewList
  {
    public EnumStatusUser StatusUser { get; set; }
    public ViewListCompany Company { get; set; }
    public ViewListOccupationResume Occupation { get; set; }
    public ViewBaseFields Manager { get; set; }
    public DateTime? DateResignation { get; set; }
    public EnumTypeJourney TypeJourney { get; set; }
    public ViewListEstablishment Establishment { get; set; }
    public EnumTypeUser TypeUser { get; set; }
    public string Registration { get; set; }
    public int Workload { get; set; }
  }
}
