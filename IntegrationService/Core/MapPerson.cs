using IntegrationService.Data;
using IntegrationService.Views.Person;
using System;
using System.Collections.Generic;

namespace IntegrationService.Core
{
  public class MapPerson
  {
    public string Documento { get; set; }
    public string Empresa { get; set; }
    public string Matricula { get; set; }
    public string Mensagem { get; set; }
    public Colaborador Colaborador { get; set; }
    public ViewIntegrationPersonV1 MyProperty { get; set; }
    public ViewIntegrationMapOfV1 Schooling { get; set; }
    public ViewIntegrationMapOfV1 Company { get; set; }
    public ViewIntegrationMapOfV1 Establishment { get; set; }
    public ViewIntegrationMapOfV1 Occupation{ get; set; }
    public ViewIntegrationPersonV1 Person { get; set; }
    public ViewIntegrationContractV1 Contract { get; set; }
  }
}
