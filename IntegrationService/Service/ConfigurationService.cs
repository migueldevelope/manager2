using IntegrationService.Api;
using Manager.Views.BusinessCrud;
using Manager.Views.Integration;
using System;

namespace IntegrationService.Service
{
  public class ConfigurationService
  {
    public ViewCrudIntegrationParameter Param { get; private set; }
    private readonly ViewPersonLogin Person;
    private ConfigurationIntegration Service;
    public ConfigurationService(ViewPersonLogin person)
    {
      try
      {
        Person = person;
        Service = new ConfigurationIntegration(person);
        Param = Service.GetParameter();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public void SetParameter(ViewCrudIntegrationParameter view)
    {
      try
      {
        Param = Service.SetParameter(view);
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
