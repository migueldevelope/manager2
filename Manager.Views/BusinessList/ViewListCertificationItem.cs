using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListCertificationItem: _ViewListBase
  {
    public EnumItemCertification ItemCertification { get; set; }
    public string IdItem { get; set; }
    public string Concept { get; set; }

  }
}
