using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessView
{
  public class ViewUserNickName: _ViewListBase
  {
    public string Document { get; set; }
    public string Mail { get; set; }
    public string Phone { get; set; }
    public DateTime? DateBirth { get; set; }
    public DateTime? DateAdm { get; set; }
    public ViewListSchooling Schooling { get; set; }
    public string PhoneFixed { get; set; }
    public EnumSex Sex { get; set; }
    public string Nickname { get; set; }
    public List<ViewPersonNickName> Contracts { get; set; }
  }
}
