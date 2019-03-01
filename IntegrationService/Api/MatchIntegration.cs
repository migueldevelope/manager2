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
  public class MatchIntegration
  {
    private HttpClient clientSkill;
    private readonly ViewPersonLogin Person;

    public MatchIntegration(ViewPersonLogin person)
    {
      Person = person;
      clientSkill = new HttpClient()
      {
        BaseAddress = new Uri(string.Format("{0}/integrationserver/", Person.Url))
      };
      clientSkill.DefaultRequestHeaders.Add("ContentType", "application/json");
      clientSkill.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", Person.Token));
    }

    public List<ViewIntegrationCompany> GetIntegrationCompanies(int count = 10, int page = 1, string filter = "", bool all = false)
    {
      try
      {
        HttpResponseMessage result;
        if (string.IsNullOrEmpty(filter))
          result = clientSkill.GetAsync(string.Format("integration/company/list?count={0}&page={1}&all={2}", count, page, all)).Result;
        else
          result = clientSkill.GetAsync(string.Format("integration/company/list?count={0}&page={1}&filter={2}&all={3}", count, page, filter, all)).Result;

        return JsonConvert.DeserializeObject<List<ViewIntegrationCompany>>(result.Content.ReadAsStringAsync().Result);
      }
      catch (Exception)
      {
        throw;
      }
    }

  }
}
