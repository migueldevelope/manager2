using Manager.Views.Integration;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace IntegrationClient.Api
{
  public class Authentication
  {
    public ViewPersonLogin Connect(string url, string mail, string password)
    {
      try
      {
        string pathUrl = string.Empty;
        if (url.Equals("https://analisa.solutions"))
          pathUrl = string.Format("{0}/authentication", url).Replace("//", "//integrationserver.");
        else
          pathUrl = string.Format("{0}/authentication", url).Replace("//test.", "//test_integrationserver.");
          //pathUrl = string.Format("{0}/authentication", "http://10.0.0.16:5200");

        HttpClient clientAuthentication = new HttpClient()
        {
          BaseAddress = new Uri(pathUrl)
        };
        var data = new
        {
          mail,
          password
        };
        StringContent content = new StringContent(JsonConvert.SerializeObject(data));
        content.Headers.ContentType.MediaType = "application/json";
        clientAuthentication.DefaultRequestHeaders.Add("ContentType", "application/json");
        var result = clientAuthentication.PostAsync("auth", content).Result;
        if (result.StatusCode != System.Net.HttpStatusCode.OK )
        {
          throw new Exception("Usuário inválido!");
        }
        ViewPersonLogin person = JsonConvert.DeserializeObject<ViewPersonLogin>(result.Content.ReadAsStringAsync().Result);
        person.Url = url;
        person.Email = mail;
        return person;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
}
