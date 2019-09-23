using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListOccupation : _ViewListBase
  {
    public ViewListCompany Company { get; set; }
    public ViewListGroup Group { get; set; }
    public List<ViewListProcessLevelTwo> Process { get; set; }
    public long Line { get; set; }
    public ViewListCbo Cbo { get; set; }
    public string Description { get; set; }
  }
}
