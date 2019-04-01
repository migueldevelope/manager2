using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudFrequencyEvent: _ViewCrud
  {
    public ViewCrudDaysEvent DaysEvent { get; set; }
    public bool Present { get; set; }
  }
}
