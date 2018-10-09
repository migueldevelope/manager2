using IntegrationService.Api;
using IntegrationService.Core;
using IntegrationService.Data;
using IntegrationService.Views;
using IntegrationService.Views.Person;
using System;

namespace IntegrationService.Service
{
  public class ValidMapManager
  {
    public ViewIntegrationMapManagerV1 Map { get; private set; }
    public ValidMapManager(ViewPersonLogin person, string document, string company, string registration, string name)
    {
      try
      {
        Map = new ViewIntegrationMapManagerV1()
        {
          Document = document,
          CompanyCode = company,
          CompanyId = string.Empty,
          Registration = registration,
          Name = name
        };
        PersonIntegration personIntegration = new PersonIntegration(person);
        personIntegration.GetManagerByKey(Map);
      }
      catch (Exception ex)
      {
        Map = new ViewIntegrationMapManagerV1()
        {
          Document = document,
          CompanyCode = company,
          CompanyId = string.Empty,
          Registration = registration,
          Name = name,
          IdPerson = string.Empty,
          IdContract = string.Empty,
          Message = ex.Message
        };
      }
    }
  }
}
