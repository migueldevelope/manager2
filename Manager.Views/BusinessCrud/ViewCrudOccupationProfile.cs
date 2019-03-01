using Manager.Views.BusinessList;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudOccupationProfile : _ViewCrudBase
  {
    public List<ViewListSkill> Skills { get; set; }
    public List<ViewListSchooling> Schooling { get; set; }
    public List<ViewListActivitie> Activities { get; set; }
    public string SpecificRequirements { get; set; }
  }
}
