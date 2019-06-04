using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Integration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace IntegrationService.Api
{
  public class InfraIntegration
  {
    private HttpClient clientSkill;
    private readonly ViewPersonLogin Person;

    #region Constructor
    public InfraIntegration(ViewPersonLogin person)
    {
      Person = person;
      clientSkill = new HttpClient() {
        BaseAddress = new Uri(string.Format("{0}/integrationserver/", Person.Url))
      };
      clientSkill.DefaultRequestHeaders.Add("ContentType", "application/json");
      clientSkill.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", Person.Token));
    }
    #endregion

    #region Company
    public List<ViewListCompany> GetCompanyList()
    {
      HttpResponseMessage result = clientSkill.GetAsync("integration/company/list/root").Result;
      return result.IsSuccessStatusCode == false
        ? null
        : JsonConvert.DeserializeObject<List<ViewListCompany>>(result.Content.ReadAsStringAsync().Result);
    }
    #endregion

    #region ProcessLevelTwo
    public List<ViewListProcessLevelTwo> GetProcessLevelTwoList()
    {
      HttpResponseMessage result = clientSkill.GetAsync("integration/processleveltwo/list").Result;
      return result.IsSuccessStatusCode == false
        ? null
        : JsonConvert.DeserializeObject<List<ViewListProcessLevelTwo>>(result.Content.ReadAsStringAsync().Result);
    }
    #endregion

    #region Skill
    public ViewCrudSkill IntegrationSkill(ViewCrudSkill view)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(view));
        content.Headers.ContentType.MediaType = "application/json";

        var result = clientSkill.PostAsync("integration/skill", content).Result;
        if (result.IsSuccessStatusCode == false)
        {
          throw new Exception("Skill não encontrada!");
        }
        return JsonConvert.DeserializeObject<ViewCrudSkill>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #region Occupation and Profile
    public ViewIntegrationProfileOccupation IntegrationProfile(ViewIntegrationProfileOccupation view)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(view));
        content.Headers.ContentType.MediaType = "application/json";

        var result = clientSkill.PostAsync("integration/profile", content).Result;
        if (result.IsSuccessStatusCode == false)
        {
          throw new Exception("Cargo não atualizado!");
        }
        return JsonConvert.DeserializeObject<ViewIntegrationProfileOccupation>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

  }
}
