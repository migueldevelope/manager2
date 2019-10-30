using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudFluidCareers : _ViewCrud
  {
    public string _idPerson { get; set; }
    public ViewFluidCareers FluidCareersView { get; set; }
    public List<ViewCrudSkillsCareers> SkillsCareers { get; set; }
  }
}
