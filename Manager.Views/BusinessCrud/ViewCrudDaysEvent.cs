using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudDaysEvent: _ViewCrud
  {
    public DateTime Begin { get; set; }
    public DateTime End { get; set; }
  }
}
