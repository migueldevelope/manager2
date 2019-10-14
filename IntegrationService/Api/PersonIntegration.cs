using System;
using System.Net.Http;
using Newtonsoft.Json;
using Manager.Views.Integration;

namespace IntegrationService.Api
{
  public class PersonIntegration
  {
    private HttpClient clientSkill;
    private readonly ViewPersonLogin Person;

    public PersonIntegration(ViewPersonLogin person)
    {
      Person = person;
      string pathUrl = string.Empty;
      if (Person.Url.Equals("https://analisa.solutions"))
        pathUrl = string.Format("{0}/", Person.Url).Replace("//", "//integrationserver.");
      else
        pathUrl = string.Format("{0}/", Person.Url).Replace("//test.", "//test_integrationserver.");

      if (Person.Url == "https://analisa.unimednordesters.com.br")
        pathUrl = "https://analisa.unimednordesters.com.br/integrationserver/";

      clientSkill = new HttpClient()
      {
        BaseAddress = new Uri(pathUrl)
      };
      clientSkill.DefaultRequestHeaders.Add("ContentType", "application/json");
      clientSkill.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", Person.Token));
    }
    public ViewIntegrationColaborador PutPerson(ViewIntegrationColaborador view)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(view));
        content.Headers.ContentType.MediaType = "application/json";
        var result = clientSkill.PutAsync("person/update", content).Result;
        return JsonConvert.DeserializeObject<ViewIntegrationColaborador>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }

    public void GetStatusIntegration()
    {
      try
      {
        var result = clientSkill.GetAsync("integration/status").Result;
        if (!result.IsSuccessStatusCode)
          throw new Exception(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
