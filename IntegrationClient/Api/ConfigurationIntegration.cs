using System;
using System.Net.Http;
using Newtonsoft.Json;
using Manager.Views.Integration;
using Manager.Views.BusinessCrud;

namespace IntegrationClient.Api
{
  public class ConfigurationIntegration
  {
    private readonly HttpClient clientSkill;
    private readonly ViewPersonLogin Person;

    public ConfigurationIntegration(ViewPersonLogin person)
    {
      Person = person;
      string pathUrl;
      if (Person.Url.Equals("https://fluidstate.com.br"))
        pathUrl = string.Format("{0}/", Person.Url).Replace("//", "//integrationserver.");
      else
        pathUrl = string.Format("{0}/", Person.Url).Replace("//test.", "//test_integrationserver.");
        //pathUrl = string.Format("{0}/", "http://10.0.0.16:5203");


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
