using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Objective : BaseEntity
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

    public ViewCrudObjective GetViewCrud()
    {
      return new ViewCrudObjective
      {
        _id = _id,
        Description = Description,
        Detail = Detail,
        StartDate = StartDate,
        EndDate = EndDate,
        StausObjective = StausObjective,
        Dimension = Dimension,
        Responsible = Responsible,
        Editors = Editors,
        TypeCheckin = TypeCheckin
      };
    }

    public ViewListObjective GetViewList()
    {
      return new ViewListObjective
      {
        _id = _id,
        Description = Description
      };
    }

  }
}
