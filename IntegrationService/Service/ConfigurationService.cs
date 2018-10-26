using IntegrationService.Api;
using Manager.Views.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationService.Service
{
  public class ConfigurationService
  {
    public ViewIntegrationParameter Param { get; private set; }
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
    public void SetParameter(ViewIntegrationParameterMode view)
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
    public void SetParameter(ViewIntegrationParameterPack view)
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
    public void SetParameter(ViewIntegrationParameterExecution view)
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
