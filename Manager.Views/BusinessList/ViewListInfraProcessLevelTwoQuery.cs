using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListInfraProcessLevelTwoQuery:_ViewListBase
  {
    public List<ViewListInfraOccupationQuery> Occupations { get; set; }
    public string _idProcessLevelOne { get; set; }
    public long? Order { get; set; }
  }
}
