using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListInfraGroupQuery: _ViewListBase
  {
    public List<ViewListInfraProcessLevelOneQuery> ProcessLevelOnes { get; set; }
    public string _idSphere { get; set; }
    public long Line { get; set; }
  }
}
