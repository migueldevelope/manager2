namespace IntegrationService.Views.Person
{
  public class ViewIntegrationMapPersonV1
  {
    public string Document { get; set; }
    public string IdCompany { get; set; }
    public long Registration { get; set; }
    public string Name { get; set; }
    public string Id { get; set; }
    public ViewIntegrationPersonV1 Person { get; set; }
    public ViewIntegrationContractV1 Contract { get; set; }
  }
}
