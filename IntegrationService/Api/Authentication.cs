using Manager.Views.Integration;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace IntegrationService.Api
{
  public class Authentication
  {
    public ViewPersonLogin Connect(string url, string mail, string password)
    {
      try
      {
        HttpClient clientAuthentication = new HttpClient()
        {
          BaseAddress = new Uri(string.Format("{0}/manager/authentication", url))
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
        ViewPersonLogin person = JsonConvert.DeserializeObject<ViewPersonLogin>(result.Content.ReadAsStringAsync().Result);
        person.Url = url;
        person.Email = mail;
        return person;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
