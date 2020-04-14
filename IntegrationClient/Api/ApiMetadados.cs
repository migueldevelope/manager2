using Manager.Views.Integration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;

namespace IntegrationClient.Api
{
  public class ApiMetadados
  {

    public List<ViewIntegrationMetadadosV1> GetEmployee(string token, int offset, int limit)
    {
      try
      {
        using (HttpClient client = new HttpClient())
        {
          client.Timeout = TimeSpan.FromMinutes(60);
          //client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", token));
          client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", "YzhlNjEyOTA4NzI5NDZmODliNjhlZGZkOWE0OTBhMjE6OTZmZjI5OWMxNTUwNGViM2JhYjI4NGJkMzNlYWFlODg="));
          client.DefaultRequestHeaders.Add("ContentType", "application/json");
          HttpResponseMessage result = client.GetAsync(string.Format("https://metadados.linkapi.com.br/v2/analisaFluid?offset={0}&limit={1}", offset, limit)).Result;
          if (result.StatusCode != System.Net.HttpStatusCode.OK)
          {
            throw new Exception("API Integration Error!");
          }
          string resultContent = result.Content.ReadAsStringAsync().Result;
          IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
          return JsonConvert.DeserializeObject<List<ViewIntegrationMetadadosV1>>(resultContent, dateTimeConverter);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewIntegrationMetadadosV1> GetDemissionEmployee(string token, DateTime dateDemissionRef)
    {
      try
      {
        using (HttpClient client = new HttpClient())
        {
          client.Timeout = TimeSpan.FromMinutes(60);
          client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", token));
          client.BaseAddress = new Uri(string.Format("https://metadados.linkapi.com.br/v1/analisaFluid?rescindidos=true&dataRescisao={0}",
            dateDemissionRef.ToUniversalTime().ToString("s", CultureInfo.InvariantCulture)));
          StringContent content = new StringContent(string.Empty);
          content.Headers.ContentType.MediaType = "application/json";
          client.DefaultRequestHeaders.Add("ContentType", "application/json");
          HttpResponseMessage result = client.PostAsync("/", content).Result;
          if (result.StatusCode != System.Net.HttpStatusCode.OK)
          {
            throw new Exception("Integration Error!");
          }
          string resultContent = result.Content.ReadAsStringAsync().Result;
          IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
          return JsonConvert.DeserializeObject<List<ViewIntegrationMetadadosV1>>(resultContent, dateTimeConverter);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
}

