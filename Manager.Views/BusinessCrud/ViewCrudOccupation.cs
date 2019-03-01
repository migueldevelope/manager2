using Manager.Views.BusinessList;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudOccupation : _ViewCrudBase
  {
    public ViewListGroup Group { get; set; }
    public ViewListArea Area { get; set; }
    public long Line { get; set; }
    public ViewListCbo Cbo { get; set; }
    public ViewListGrade Grade { get; set; }
    public List<ViewListProcessLevelTwo> Process { get; set; }
  }
}
