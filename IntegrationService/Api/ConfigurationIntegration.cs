using System;
using System.Net.Http;
using Newtonsoft.Json;
using Manager.Views.Integration;

namespace IntegrationService.Api
{
  public class ConfigurationIntegration
  {
    private HttpClient clientSkill;
    private readonly ViewPersonLogin Person;

    public ConfigurationIntegration(ViewPersonLogin person)
    {
      Person = person;
      clientSkill = new HttpClient()
      {
        BaseAddress = new Uri(string.Format("{0}/integrationserver/", Person.Url))
      };
      clientSkill.DefaultRequestHeaders.Add("ContentType", "application/json");
      clientSkill.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", Person.Token));
    }

    public ViewIntegrationParameter GetParameter()
    {
      try
      {
        var result = clientSkill.GetAsync("configuration").Result;
        return JsonConvert.DeserializeObject<ViewIntegrationParameter>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        return null;
      }
    }
    public ViewIntegrationParameter SetParameter(ViewIntegrationParameterMode view)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(view));
        content.Headers.ContentType.MediaType = "application/json";
        var result = clientSkill.PostAsync("configuration/mode", content).Result;
        return JsonConvert.DeserializeObject<ViewIntegrationParameter>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        return null;
      }
    }
    public ViewIntegrationParameter SetParameter(ViewIntegrationParameterPack view)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(view));
        content.Headers.ContentType.MediaType = "application/json";
        var result = clientSkill.PostAsync("configuration/pack", content).Result;
        return JsonConvert.DeserializeObject<ViewIntegrationParameter>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        return null;
      }
    }
    public ViewIntegrationParameter SetParameter(ViewIntegrationParameterExecution view)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(view));
        content.Headers.ContentType.MediaType = "application/json";
        var result = clientSkill.PostAsync("configuration/execution", content).Result;
        return JsonConvert.DeserializeObject<ViewIntegrationParameter>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
