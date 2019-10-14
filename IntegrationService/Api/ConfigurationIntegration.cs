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
      string pathUrl = string.Empty;
      if (Person.Url.Equals("https://analisa.solutions"))
        pathUrl = string.Format("{0}/", Person.Url).Replace("//", "//integrationserver.");
      else
        pathUrl = string.Format("{0}/", Person.Url).Replace("//test.", "//test_integrationserver.");

      if (person.Url == "https://analisa.unimednordesters.com.br")
        pathUrl = "https://analisa.unimednordesters.com.br/integrationserver/";

      clientSkill = new HttpClient()
      {
        BaseAddress = new Uri(pathUrl)
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

    public string GetParameterTest()
    {
      try
      {
        var result = clientSkill.GetAsync("configuration").Result;
        return result.Content.ReadAsStringAsync().Result;
      }
      catch (Exception)
      {
        return null;
      }
    }

    public string GetParameterTestReturn()
    {
      try
      {
        var result = clientSkill.GetAsync("configuration").Result;
        return result.ToString();
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
