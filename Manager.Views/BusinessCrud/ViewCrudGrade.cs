﻿using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGrade : _ViewCrudBase
  {
    public ViewListSalaryScale SalaryScale { get; set; }
    public EnumSteps StepMedium { get; set; }
    public int Order { get; set; }
    public int Workload { get; set; }
  }
}
