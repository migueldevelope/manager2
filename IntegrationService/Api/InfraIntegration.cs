using Manager.Views.Integration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationService.Api
{
  public class InfraIntegration
  {
    private HttpClient clientSkill;
    private readonly ViewPersonLogin Person;

    public InfraIntegration(ViewPersonLogin person)
    {
      Person = person;
      clientSkill = new HttpClient() {
        BaseAddress = new Uri(string.Format("{0}/integrationserver/", Person.Url))
      };
      clientSkill.DefaultRequestHeaders.Add("ContentType", "application/json");
      clientSkill.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", Person.Token));
    }

    public ViewIntegrationSkill GetSkillByName(string name)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(new ViewIntegrationFilterName() { Name = name }));
        content.Headers.ContentType.MediaType = "application/json";

        var result = clientSkill.PostAsync("infra/skill", content).Result;
        if (result.IsSuccessStatusCode == false)
        {
          string messageResult = JsonConvert.DeserializeObject<string>(result.Content.ReadAsStringAsync().Result);
          if (messageResult == null)
            throw new Exception("Skill não encontrada!");
          else
            throw new Exception(messageResult);
        }
        return JsonConvert.DeserializeObject<ViewIntegrationSkill>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }

    public ViewIntegrationSkill AddSkill(ViewIntegrationSkill newSkill)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(newSkill));
        content.Headers.ContentType.MediaType = "application/json";
        var result = clientSkill.PostAsync("infra/skill/new", content).Result;
        if (result.IsSuccessStatusCode == false)
        {
          string messageResult = JsonConvert.DeserializeObject<string>(result.Content.ReadAsStringAsync().Result);
          throw new Exception(messageResult);
        }
        return JsonConvert.DeserializeObject<ViewIntegrationSkill>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }

    public ViewIntegrationProcessLevelTwo GetProcessByName(string id)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(new ViewIntegrationFilterName() { Id = id }));
        content.Headers.ContentType.MediaType = "application/json";

        var result = clientSkill.PostAsync("infra/processleveltwo", content).Result;
        if (result.IsSuccessStatusCode == false)
        {
          string messageResult = JsonConvert.DeserializeObject<string>(result.Content.ReadAsStringAsync().Result);
          if (messageResult == null)
            throw new Exception("Sub processo não encontrado!");
          else
            throw new Exception(messageResult);
        }
        return JsonConvert.DeserializeObject<ViewIntegrationProcessLevelTwo>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ViewIntegrationGroup GetGroupByName(string idCompany, string name)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(new ViewIntegrationFilterName() { IdCompany = idCompany, Name = name }));
        content.Headers.ContentType.MediaType = "application/json";

        var result = clientSkill.PostAsync("infra/group", content).Result;
        if (result.IsSuccessStatusCode == false)
        {
          string messageResult = JsonConvert.DeserializeObject<string>(result.Content.ReadAsStringAsync().Result);
          if (messageResult == null)
            throw new Exception("Grupo de cargo não encontrado!");
          else
            throw new Exception(messageResult);
        }
        return JsonConvert.DeserializeObject<ViewIntegrationGroup>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ViewIntegrationProfileOccupation GetOccupationByName(string idCompany, string name)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(new ViewIntegrationFilterName() { IdCompany = idCompany, Name = name }));
        content.Headers.ContentType.MediaType = "application/json";

        var result = clientSkill.PostAsync("infra/occupation", content).Result;
        if (result.IsSuccessStatusCode == false)
        {
          string messageResult = JsonConvert.DeserializeObject<string>(result.Content.ReadAsStringAsync().Result);
          if (messageResult == null)
            throw new Exception("Cargo não encontrado!");
          else
            throw new Exception(messageResult);
        }
        return JsonConvert.DeserializeObject<ViewIntegrationProfileOccupation>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ViewIntegrationProfileOccupation AddOccupation(ViewIntegrationProfileOccupation newOccupation)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(newOccupation));
        content.Headers.ContentType.MediaType = "application/json";
        var result = clientSkill.PostAsync("infra/occupation/new", content).Result;
        if (result.IsSuccessStatusCode == false)
        {
          string messageResult = JsonConvert.DeserializeObject<string>(result.Content.ReadAsStringAsync().Result);
          throw new Exception(messageResult);
        }
        return JsonConvert.DeserializeObject<ViewIntegrationProfileOccupation>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
