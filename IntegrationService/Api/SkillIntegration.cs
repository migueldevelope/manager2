﻿using IntegrationService.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationService.Api
{
  public class SkillIntegration
  {
    private readonly ViewPersonLogin Person;

    public SkillIntegration(ViewPersonLogin person)
    {
      Person = person;
    }

    public ViewIntegrationSkill GetSkillByName(string name)
    {
      try
      {
        HttpClient clientSkill = new HttpClient()
        {
          BaseAddress = new Uri(string.Format("{0}/integrationserver/", Person.Url))
        };
        clientSkill.DefaultRequestHeaders.Add("ContentType", "application/json");
        clientSkill.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", Person.Token));

        StringContent content = new StringContent(JsonConvert.SerializeObject(name));
        content.Headers.ContentType.MediaType = "application/json";
        var result = clientSkill.PostAsync("skill", content).Result;
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
        HttpClient clientSkill = new HttpClient()
        {
          BaseAddress = new Uri(string.Format("{0}/integrationserver/", Person.Url))
        };
        clientSkill.DefaultRequestHeaders.Add("ContentType", "application/json");
        clientSkill.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", Person.Token));

        StringContent content = new StringContent(JsonConvert.SerializeObject(newSkill));
        content.Headers.ContentType.MediaType = "application/json";
        var result = clientSkill.PostAsync("skill/new", content).Result;
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
  }
}
