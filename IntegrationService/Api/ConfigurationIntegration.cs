using System;
using System.Net.Http;
using Newtonsoft.Json;
using Manager.Views.Integration;
using Manager.Views.BusinessCrud;

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
        BaseAddress = new Uri(string.Format("{0}/", Person.Url).Replace("//", "//integrationserver."))
      };
      clientSkill.DefaultRequestHeaders.Add("ContentType", "application/json");
      clientSkill.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", Person.Token));
    }

    public ViewCrudIntegrationParameter GetParameter()
    {
      try
      {
        var result = clientSkill.GetAsync("configuration").Result;
        return JsonConvert.DeserializeObject<ViewCrudIntegrationParameter>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        return null;
      }
    }
    public ViewCrudIntegrationParameter SetParameter(ViewCrudIntegrationParameter view)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(view));
        content.Headers.ContentType.MediaType = "application/json";
        var result = clientSkill.PostAsync("configuration/mode", content).Result;
        return JsonConvert.DeserializeObject<ViewCrudIntegrationParameter>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
