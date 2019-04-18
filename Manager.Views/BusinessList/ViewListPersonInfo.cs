using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListPersonInfo : _ViewListBase
  {
    public ViewListCompany Company { get; set; }
    public ViewListEstablishment Establishment { get; set; }
    public string Registration { get; set; }
    public ViewListUser User { get; set; }
    public EnumTypeJourney TypeJourney { get; set; }
    public string Occupation { get; set; }
    public string Manager { get; set; }
  }
}
