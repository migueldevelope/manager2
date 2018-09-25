using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationService.Api
{
  public class Authentication
  {
    public Boolean ValidAuthentication(string url, string mail, string password)
    {
      try
      {
        HttpClient clientAuthentication = new HttpClient()
        {
          BaseAddress = new Uri(string.Format("{0}/integrationserver/authentication", url))
        };
        var data = new
        {
          mail,
          password
        };
        StringContent content = new StringContent(JsonConvert.SerializeObject(data));
        content.Headers.ContentType.MediaType = "application/json";
        clientAuthentication.DefaultRequestHeaders.Add("ContentType", "application/json");
        var result = clientAuthentication.PostAsync("authentication", content).Result;
        var resultContent = result.Content.ReadAsStringAsync().Result;
      }
      catch (Exception)
      {
        throw;
      }


      //var auth = JsonConvert.DeserializeObject<ViewPerson>(resultContent);

      return true;

    }
  }
}
