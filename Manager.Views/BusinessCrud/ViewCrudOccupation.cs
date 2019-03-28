using Manager.Views.BusinessList;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudOccupation : _ViewCrudBase
  {
    public ViewListGroup Group { get; set; }
    public long Line { get; set; }
    public ViewListCbo Cbo { get; set; }
    public List<ViewListProcessLevelTwo> Process { get; set; }
    public List<ViewCrudSalaryScaleOccupation> SalaryScales { get; set; }
  }
}
