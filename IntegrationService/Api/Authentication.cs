using IntegrationService.Views;
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
    public ViewPersonLogin Person { get; private set; }
    public string BaseUrl { get; private set; }

    public void Connect(string url, string mail, string password)
    {
      try
      {
        this.BaseUrl = url;
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
        if (result.StatusCode != System.Net.HttpStatusCode.OK )
        {
          throw new Exception("Usuário inválido!");
        }
        this.Person = JsonConvert.DeserializeObject<ViewPersonLogin>(result.Content.ReadAsStringAsync().Result);
        this.Person.Url = url;
        this.Person.Mail = mail;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
