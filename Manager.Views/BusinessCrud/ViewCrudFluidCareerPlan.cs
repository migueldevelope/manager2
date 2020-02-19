using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudFluidCareerPlan : _ViewCrud
  {
    public string What { get; set; }
    public DateTime? Date { get; set; }
    public string Observation { get; set; }
    public EnumStatusFluidCareerPlan StatusFluidCareerPlan { get; set; }
  }
}
