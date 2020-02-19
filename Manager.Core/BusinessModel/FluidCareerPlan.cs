using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.BusinessModel
{
  public class FluidCareerPlan : BaseEntity
  {
    public string What { get; set; }
    public DateTime? Date { get; set; }
    public string Observation { get; set; }
    public EnumStatusFluidCareerPlan StatusFluidCareerPlan { get; set; }
    public ViewCrudFluidCareerPlan GetViewCrud()
    {
      return new ViewCrudFluidCareerPlan()
      {
        Date = Date,
        Observation = Observation,
        What = What,
        StatusFluidCareerPlan = StatusFluidCareerPlan,
        _id = _id
      };
    }
  }
}
