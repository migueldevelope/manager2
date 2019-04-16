using Manager.Views.BusinessCrud;
using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListProcessLevelOneByArea : _ViewListBase
  {
    public ViewListArea Area { get; set; }
    public long Order { get; set; }
    public List<ViewCrudProcessLevelTwo> Process { get; set; }
  }
}
