using IntegrationService.Core;
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
      clientSkill = new HttpClient()
      {
        BaseAddress = new Uri(string.Format("{0}/integrationserver/", Person.Url))
      };
      clientSkill.DefaultRequestHeaders.Add("ContentType", "application/json");
      clientSkill.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", Person.Token));
    }
    public ViewIntegrationMapOfV1 GetByName(EnumValidKey key, ViewIntegrationMapOfV1 map)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(map));
        content.Headers.ContentType.MediaType = "application/json";

        // Company = 0, Schooling = 1, Establishment = 2, Occupation = 3
        var result = clientSkill.PostAsync(string.Format("person/{0}",key.ToString().ToLower()), content).Result;
        return JsonConvert.DeserializeObject<ViewIntegrationMapOfV1>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception ex)
      {
        map.Id = string.Empty;
        map.Message = ex.Message;
        return map;
      }
    }

    public ViewIntegrationMapPersonV1 GetCollaboratorByKey(ViewIntegrationMapPersonV1 map)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(map));
        content.Headers.ContentType.MediaType = "application/json";

        var result = clientSkill.PostAsync("person", content).Result;
        return JsonConvert.DeserializeObject<ViewIntegrationMapPersonV1>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ViewIntegrationMapManagerV1 GetManagerByKey(ViewIntegrationMapManagerV1 map)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(map));
        content.Headers.ContentType.MediaType = "application/json";

        var result = clientSkill.PostAsync("person/manager", content).Result;
        return JsonConvert.DeserializeObject<ViewIntegrationMapManagerV1>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }

    public ViewIntegrationMapPersonV1 PostNewPerson(ViewIntegrationPersonV1 view)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(view));
        content.Headers.ContentType.MediaType = "application/json";
        var result = clientSkill.PostAsync("person/new", content).Result;
        if (result.IsSuccessStatusCode)
          return JsonConvert.DeserializeObject<ViewIntegrationMapPersonV1>(result.Content.ReadAsStringAsync().Result);
        return null;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ViewIntegrationMapPersonV1 PutPerson(ViewIntegrationPersonV1 view)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(view));
        content.Headers.ContentType.MediaType = "application/json";
        var result = clientSkill.PutAsync("person/update", content).Result;
        return JsonConvert.DeserializeObject<ViewIntegrationMapPersonV1>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
