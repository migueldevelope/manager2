using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListItensDetailMobile:_ViewListBase
  {

    public string Concept { get; set; }
    public EnumTypeSkill TypeSkill { get; set; }
    public long Order { get; set; }
    public string Complement { get; set; }
    public EnumTypeSchooling Type { get; set; }
    public EnumTypeItem TypeItem { get; set; }
  }
}
