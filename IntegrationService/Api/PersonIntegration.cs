using IntegrationService.Views;
using IntegrationService.Core;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using IntegrationService.Views.Person;

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

    public ViewIntegrationMapCollaboratorV1 GetCollaboratorByKey(ViewIntegrationMapCollaboratorV1 map)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(map));
        content.Headers.ContentType.MediaType = "application/json";

        var result = clientSkill.PostAsync("person/collaborator", content).Result;
        if (result.IsSuccessStatusCode == false)
        {
          string messageResult = JsonConvert.DeserializeObject<string>(result.Content.ReadAsStringAsync().Result);
          if (messageResult == null)
            throw new Exception("Collaborator not found!");
          else
            throw new Exception(messageResult);
        }
        return JsonConvert.DeserializeObject<ViewIntegrationMapCollaboratorV1>(result.Content.ReadAsStringAsync().Result);
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
        if (result.IsSuccessStatusCode == false)
        {
          string messageResult = JsonConvert.DeserializeObject<string>(result.Content.ReadAsStringAsync().Result);
          if (messageResult == null)
            throw new Exception("Manager not found!");
          else
            throw new Exception(messageResult);
        }
        return JsonConvert.DeserializeObject<ViewIntegrationMapManagerV1>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
