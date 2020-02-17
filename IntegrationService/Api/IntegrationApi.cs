using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Integration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace IntegrationService.Api
{
  public class IntegrationApi
  {
    private HttpClient clientSkill;
    private readonly ViewPersonLogin Person;

    #region Constructor
    public IntegrationApi(ViewPersonLogin person)
    {
      Person = person;
      string pathUrl = string.Empty;
      if (Person.Url.Equals("https://analisa.solutions"))
        pathUrl = string.Format("{0}/", Person.Url).Replace("//", "//integrationserver.");
      else
        pathUrl = string.Format("{0}/", Person.Url).Replace("//test.", "//test_integrationserver.");
        //pathUrl = string.Format("{0}/", "http://10.0.0.16:5203");

      if (person.Url == "https://analisa.unimednordesters.com.br")
        pathUrl = "https://analisa.unimednordesters.com.br/integrationserver/";

      clientSkill = new HttpClient() {
        BaseAddress = new Uri(pathUrl)
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
        HttpResponseMessage result = clientSkill.PostAsync("integration/profile", content).Result;
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
    public List<ViewListOccupationResume> OccupationExportList()
    {
      HttpResponseMessage result = clientSkill.GetAsync("integration/occupations").Result;
      return result.IsSuccessStatusCode == false
        ? null
        : JsonConvert.DeserializeObject<List<ViewListOccupationResume>>(result.Content.ReadAsStringAsync().Result);
    }
    public ViewMapOccupation OccupationExportProfile(string id)
    {
      HttpResponseMessage result = clientSkill.GetAsync(string.Format("integration/occupations/{0}",id)).Result;
      return result.IsSuccessStatusCode == false
        ? null
        : JsonConvert.DeserializeObject< ViewMapOccupation > (result.Content.ReadAsStringAsync().Result);
    }
    #endregion

  }
}
