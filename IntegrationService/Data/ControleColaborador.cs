using Manager.Views.Integration;

namespace IntegrationService.Data
{
  public class ControleColaborador
  {
    public string ChaveColaborador { get; set; }
    public Colaborador Colaborador { get; set; }
    public ViewIntegrationMapOfV1 Company { get; set; }
    public ViewIntegrationMapOfV1 Establishment { get; set; }
    public ViewIntegrationMapOfV1 Schooling { get; set; }
    public ViewIntegrationMapOfV1 Occupation { get; set; }
    public ViewIntegrationMapOfV1 CompanyManager { get; set; }
    public ViewIntegrationMapManagerV1 Manager { get; set; }
    public string IdPerson { get; set; }
    public string IdContract { get; set; }
  }
}
