using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListPersonInfo : _ViewListBase
  {
    public EnumTypeJourney TypeJourney { get; set; }
    public string Occupation { get; set; }
    public string _idManager { get; set; }
    public string Manager { get; set; }
  }
}
