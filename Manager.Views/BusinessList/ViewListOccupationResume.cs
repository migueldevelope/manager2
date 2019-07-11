using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListOccupationResume: _ViewListBase
  {
    public string _idGroup { get; set; }
    public string NameGroup { get; set; }
    public string _idArea { get; set; }
    public ViewListCbo Cbo { get; set; }
  }
}
