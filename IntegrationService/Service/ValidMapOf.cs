using IntegrationService.Api;
using IntegrationService.Core;
using IntegrationService.Views;
using IntegrationService.Views.Person;
using System;

namespace IntegrationService.Service
{
  public class ValidMapOf
  {
    public ViewIntegrationMapOfV1 Map { get; private set; }
    public ValidMapOf(ViewPersonLogin person, EnumValidKey key, string code, string name, string idCompany)
    {
      try
      {
        Map = new ViewIntegrationMapOfV1()
        {
          Code = code,
          Name = name,
          IdCompany = idCompany
        };
        PersonIntegration personIntegration = new PersonIntegration(person);
        Map = personIntegration.GetByName(key, Map);
      }
      catch (Exception)
      {

      }
    }
  }
}
