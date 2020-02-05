using System;
using System.Net.Http;
using Newtonsoft.Json;
using Manager.Views.Integration;
using Manager.Views.Integration.V2;
using System.Collections.Generic;

namespace IntegrationService.Api
{
  public class PersonIntegration
  {
    private HttpClient clientSkill;
    private readonly ViewPersonLogin Person;

    #region Constructor
    public PersonIntegration(ViewPersonLogin person)
    {
      Person = person;
      string pathUrl = string.Empty;
      if (Person.Url.Equals("https://analisa.solutions"))
        pathUrl = string.Format("{0}/", Person.Url).Replace("//", "//integrationserver.");
      else
        pathUrl = string.Format("{0}/", Person.Url).Replace("//test.", "//test_integrationserver.");
        //pathUrl = string.Format("{0}/", "http://10.0.0.16:5203");
      clientSkill = new HttpClient()
      {
        BaseAddress = new Uri(pathUrl)
      };
      clientSkill.DefaultRequestHeaders.Add("ContentType", "application/json");
      clientSkill.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", Person.Token));
    }
    #endregion

    #region V1
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
    public ViewIntegrationColaborador PutPersonDemission(ViewIntegrationColaborador view)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(view));
        content.Headers.ContentType.MediaType = "application/json";
        var result = clientSkill.PutAsync("person/demission", content).Result;
        return JsonConvert.DeserializeObject<ViewIntegrationColaborador>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #region V2
    public ColaboradorV2Retorno PostV2Completo(ColaboradorV2Completo view)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(view));
        content.Headers.ContentType.MediaType = "application/json";
        HttpResponseMessage result = clientSkill.PostAsync("person/v2/completo", content).Result;
        return JsonConvert.DeserializeObject<ColaboradorV2Retorno>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public ColaboradorV2Retorno PutV2Demissao(ColaboradorV2Demissao view)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(view));
        content.Headers.ContentType.MediaType = "application/json";
        HttpResponseMessage result = clientSkill.PutAsync("person/v2/demissao", content).Result;
        return JsonConvert.DeserializeObject<ColaboradorV2Retorno>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public string PutV2PerfilGestor(ColaboradorV2Base view)
    {
      try
      {
        StringContent content = new StringContent(JsonConvert.SerializeObject(view));
        content.Headers.ContentType.MediaType = "application/json";
        HttpResponseMessage result = clientSkill.PutAsync("person/v2/perfilgestor", content).Result;
        return JsonConvert.DeserializeObject<string>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public List<ColaboradorV2Base> GetActiveV2()
    {
      try
      {
        HttpResponseMessage result = clientSkill.GetAsync("person/v2/ativos").Result;
        return result.IsSuccessStatusCode == false
          ? null
          : JsonConvert.DeserializeObject<List<ColaboradorV2Base>>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

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
