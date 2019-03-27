using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListProcessLevelOneByArea : _ViewListBase
  {
    public ViewListArea Area { get; set; }
    public long Order { get; set; }
    public List<ViewListProcessLevelTwo> Process { get; set; }
  }
}
