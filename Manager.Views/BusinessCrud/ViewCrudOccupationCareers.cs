using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudOccupationCareers : _ViewCrudBase
  {
    public decimal Accuracy { get; set; }
    public EnumOccupationColor Color {get;set;}
    public List<ViewListActivitie> Activities { get; set; }
  }
}
