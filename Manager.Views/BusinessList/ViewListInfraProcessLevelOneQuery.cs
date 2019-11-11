using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListInfraProcessLevelOneQuery:_ViewListBase
  {
    public List<ViewListInfraProcessLevelTwoQuery> ProcessLevelTwos { get; set; }
    public string _idGroup { get; set; }
    public long? Order { get; set; }
  }
}
