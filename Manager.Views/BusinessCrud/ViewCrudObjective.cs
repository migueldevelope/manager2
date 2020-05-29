using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudObjective : _ViewCrud
  {
    public string Description { get; set; }
    public string Detail { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public EnumStausObjective StausObjective { get; set; }
    public ViewCrudDimension Dimension { get; set; }
    public ViewListPersonBase Responsible { get; set; }
    public List<ViewListPersonBase> Editors { get; set; }
    public EnumTypeCheckin TypeCheckin { get; set; }
  }
}
