using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListInfraSphereQuery: _ViewListBase
  {
    public List<ViewListInfraGroupQuery> Groups { get; set; }
    public string _idArea { get; set; }
  }
}
