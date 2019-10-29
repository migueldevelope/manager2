using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class FluidCareers: BaseEntity
  {
    public ViewListPersonInfo Person { get; set; }
    public List<ViewCrudOccupationCareers> OccupationCareers { get; set; }
    public List<ViewCrudSkillsCareers> SkillsCareers { get; set; }
    public DateTime? Date { get; set; }
  }
}
