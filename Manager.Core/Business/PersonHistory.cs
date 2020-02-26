using Manager.Core.Base;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Business
{
  public class PersonHistory : BaseEntity
  {
    public ViewListPerson Person { get; set; }
    public DateTime Register { get; set; }
    public EnumTypeHistory TypeHistory { get; set; }
    public EnumTypeHistoryChange TypeChange { get; set; }
    public string OldKey { get; set; }
    public string OldValue { get; set; }
    public string NewKey { get; set; }
    public string NewValue { get; set; }
  }
}
