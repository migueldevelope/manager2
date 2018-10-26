using IntegrationService.Data;
using Manager.Views.Integration;

namespace IntegrationService.Core
{
  public class MapPerson
  {
    public string Documento { get; set; }
    public string Empresa { get; set; }
    public string NomeEmpresa { get; set; }
    public long Matricula { get; set; }
    public ViewIntegrationPersonV1 Person { get; set; }
    public string Mensagem { get; set; }
    public Colaborador Colaborador { get; set; }
    public Colaborador ColaboradorAnterior { get; set; }
    public ViewIntegrationMapOfV1 Schooling { get; set; }
    public ViewIntegrationMapOfV1 Company { get; set; }
    public ViewIntegrationMapOfV1 Establishment { get; set; }
    public ViewIntegrationMapOfV1 Occupation{ get; set; }
    public ViewIntegrationMapOfV1 CompanyManager { get; set; }
    public ViewIntegrationMapManagerV1 Manager { get; set; }
  }
}
