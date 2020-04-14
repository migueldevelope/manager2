using IntegrationClient.Api;
using Manager.Views.BusinessCrud;
using Manager.Views.Integration;
using System;

namespace IntegrationClient.Service
{
  public class ConfigurationService
  {
    public ViewCrudIntegrationParameter Param { get; private set; }
    //private readonly ViewPersonLogin Person;
    private readonly ConfigurationIntegration Service;
    //public string test1;
    //public string test2;

    public ConfigurationService(ViewPersonLogin person)
    {
      try
      {
        //Person = person;
        Service = new ConfigurationIntegration(person);
        Param = Service.GetParameter();
        //test1 = Service.GetParameterTest();
        //test2 = Service.GetParameterTestReturn();

        if (Param == null)
        {
          Param = new ViewCrudIntegrationParameter();
        }
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
