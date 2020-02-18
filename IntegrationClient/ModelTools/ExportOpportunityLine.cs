using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace IntegrationClient.ModelTools
{
  public class ExportOpportunityLine
  {
    public string Area { get; set; }
    public ViewListGroup Group { get; set; }
    public string GroupColumn { get; set; }
    public string OccupationName { get; set; }
    public ViewListProcessLevelOne Process { get; set; }
    public ViewListProcessLevelTwo SubProcess { get; set; }
    public long SubProcessLine { get; set; }
  }
}
