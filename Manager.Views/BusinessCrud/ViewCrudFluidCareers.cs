using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudFluidCareers : _ViewCrud
  {
    public string _idPerson { get; set; }
    public List<ViewFluidCareers> FluidCareersView { get; set; }
    public ViewCrudFluidCareerPlan Plan { get; set; }
  }
}
