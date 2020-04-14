using Manager.Views.Integration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace IntegrationClient.Api
{
  public class ApiUnimedNers
  {

    public List<ViewIntegrationUnimedNers> GetUnimedEmployee()
    {
      try
      {
        string username = "analisa";
        //        string password = "ad6072616b467db08f60918070e03622" + DateTime.Now.ToString("ddMMyyyyHHmm");
        string password = "ad6072616b467db08f60918070e03622" + DateTime.Now.ToString("ddMMyyyyHHmm");
        string password2 = GetMD5HashTypeTwo(password).ToLower();

        using (HttpClient client = new HttpClient())
        {
          client.Timeout = TimeSpan.FromMinutes(60);
          client.DefaultRequestHeaders.Add("Autorization", "Basic " + password);
          client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(new UTF8Encoding().GetBytes(username + ":" + password2)));
          client.BaseAddress = new Uri("https://api.unimednordesters.com.br");

          var data = new
          {
            channel = "analisa",
            parametros = new
            {
              dat_inicial = "",
              dat_final = "",
              des_situacao = "Ativo"
            }
          };
          string json = JsonConvert.SerializeObject(data);
          StringContent content = new StringContent(json);
          content.Headers.ContentType.MediaType = "application/json";
          client.DefaultRequestHeaders.Add("ContentType", "application/json");
          HttpResponseMessage result = client.PostAsync("/", content).Result;
          if (result.StatusCode != System.Net.HttpStatusCode.OK)
            throw new Exception("Integration Error!");

          string resultContent = result.Content.ReadAsStringAsync().Result;
          IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
          return JsonConvert.DeserializeObject<List<ViewIntegrationUnimedNers>>(resultContent, dateTimeConverter);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewIntegrationUnimedNers> GetUnimedEmployee(DateTime initial, DateTime final)
    {
      try
      {
        string username = "analisa";
        //        string password = "ad6072616b467db08f60918070e03622" + DateTime.Now.ToString("ddMMyyyyHHmm");
        string password = "ad6072616b467db08f60918070e03622" + DateTime.Now.ToString("ddMMyyyyHHmm");
        string password2 = GetMD5HashTypeTwo(password).ToLower();

        using (HttpClient client = new HttpClient())
        {
          client.Timeout = TimeSpan.FromMinutes(60);
          client.DefaultRequestHeaders.Add("Autorization", "Basic " + password);
          client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(new UTF8Encoding().GetBytes(username + ":" + password2)));
          client.BaseAddress = new Uri("https://api.unimednordesters.com.br");

          var data = new
          {
            channel = "analisa",
            parametros = new
            {
              dat_inicial = initial.ToString("dd/MM/yyyy"),
              dat_final = final.ToString("dd/MM/yyyy"),
              des_situacao = "Inativo"
            }
          };
          string json = JsonConvert.SerializeObject(data);
          StringContent content = new StringContent(json);
          content.Headers.ContentType.MediaType = "application/json";
          client.DefaultRequestHeaders.Add("ContentType", "application/json");
          HttpResponseMessage result = client.PostAsync("/", content).Result;
          if (result.StatusCode != System.Net.HttpStatusCode.OK)
            throw new Exception("Integration Error!");

          string resultContent = result.Content.ReadAsStringAsync().Result;
          IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
          return JsonConvert.DeserializeObject<List<ViewIntegrationUnimedNers>>(resultContent, dateTimeConverter);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string GetUnimedTest()
    {
      try
      {
        string username = "analisa";
        //        string password = "ad6072616b467db08f60918070e03622" + DateTime.Now.ToString("ddMMyyyyHHmm");
        string password = "ad6072616b467db08f60918070e03622" + DateTime.Now.ToString("ddMMyyyyHHmm");
        string password2 = GetMD5HashTypeTwo(password).ToLower();

        using (HttpClient client = new HttpClient())
        {
          client.Timeout = TimeSpan.FromMinutes(60);
          client.DefaultRequestHeaders.Add("Autorization", "Basic " + password);
          client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(new UTF8Encoding().GetBytes(username + ":" + password2)));
          client.BaseAddress = new Uri("https://api.unimednordesters.com.br");

          var data = new
          {
            channel = "analisa",
            parametros = new
            {
              dat_inicial = "",
              dat_final = "",
              des_situacao = "Ativo"
            }
          };
          string json = JsonConvert.SerializeObject(data);
          StringContent content = new StringContent(json);
          content.Headers.ContentType.MediaType = "application/json";
          client.DefaultRequestHeaders.Add("ContentType", "application/json");
          HttpResponseMessage result = client.PostAsync("/", content).Result;
          if (result.StatusCode != System.Net.HttpStatusCode.OK)
            throw new Exception("Integration Error!");

          string resultContent = result.Content.ReadAsStringAsync().Result;

          return resultContent.ToString();
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private static string GetMD5HashTypeTwo(string input)
    {
      System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
      byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
      byte[] hash = md5.ComputeHash(inputBytes);
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      for (int i = 0; i < hash.Length; i++)
      {
        sb.Append(hash[i].ToString("X2"));
      }
      return sb.ToString();
    }
  }
}
