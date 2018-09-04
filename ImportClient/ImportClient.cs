using ImportClient.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools;

namespace ImportClient
{
  public class ImportClient
  {
    public void import(string url, string mail, string password)
    {
      var fileCSV = CSVService.ImportCSV("import");
      var jsonCSV = JsonConvert.SerializeObject(fileCSV);
      var list = JsonConvert.DeserializeObject<List<Object>>(jsonCSV);

      var clientAuthentication = new HttpClient
      {
        BaseAddress = new Uri(url + "/evaluationconfiguration/")
      };

      var client = new HttpClient
      {
        BaseAddress = new Uri(url + "/integrationperson/")
      };

      var data = new
      {
        mail = mail,
        password = password
      };

      var json = JsonConvert.SerializeObject(data);
      var content = new StringContent(json);
      content.Headers.ContentType.MediaType = "application/json";
      client.DefaultRequestHeaders.Add("ContentType", "application/json");
      clientAuthentication.DefaultRequestHeaders.Add("ContentType", "application/json");

      var result = clientAuthentication.PostAsync("authentication", content).Result;
      var resultContent = result.Content.ReadAsStringAsync().Result;

      var auth = JsonConvert.DeserializeObject<ViewPerson>(resultContent);

      client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth.Token);

      ListPerson(list, url, auth.Token);

      updateManager(url, auth.Token);

      Console.Write("Ok");
      Console.ReadKey();
    }

    private async Task<string> sendApi(StringContent from, HttpClient client)
    {
      var response = client.PostAsync("import/service", from);
      /*if (response.IsSuccessStatusCode == false)
        SaveLogs(response.StatusCode + " - " + response.ReasonPhrase + " - " + response.RequestMessage.ToString());*/

      return "ok";
    }


    private async Task<string> updateManager(string url, string token)
    {
      var client = new HttpClient
      {
        BaseAddress = new Uri(url + "/integrationperson/")
      };
      client.DefaultRequestHeaders.Add("ContentType", "application/json");
      client.DefaultRequestHeaders.Add("ContentType", "application/json");
      client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

      var response = client.PostAsync("import/updatemanager", null).Result;
      if (response.IsSuccessStatusCode == false)
        SaveLogs(response.StatusCode + " - " + response.ReasonPhrase + " - " + response.RequestMessage.ToString());

      return "ok";
    }

    private async Task<List<ViewPersonImport>> ListPerson(List<Object> input, string url, string token)
    {
      try
      {

        var head = 0;
        foreach (var item in input)
        {
          var tv = new Thread(() => valid(item, head, url, token));
          tv.Start();
          head += 1;
        }

        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    private async Task<string> valid(object item, int head, string url, string token)
    {
      if (head != 0)
      {
        try
        {
          var result = new ViewPersonImport();

          result.Name = ((Newtonsoft.Json.Linq.JContainer)item)[0].ToString();
          if (result.Name == string.Empty)
            SaveLogs("Name is empty - line error: + " + head);

          result.Document = ((Newtonsoft.Json.Linq.JContainer)item)[1].ToString();
          if (result.Mail == string.Empty)
            SaveLogs("Mail is empty - line error: + " + head);

          result.Mail = ((Newtonsoft.Json.Linq.JContainer)item)[2].ToString();
          if (result.Mail == string.Empty)
            SaveLogs("Mail is empty - line error: + " + head);

          result.Phone = ((Newtonsoft.Json.Linq.JContainer)item)[3].ToString();
          try
          {
            result.TypeUser = byte.Parse(((Newtonsoft.Json.Linq.JContainer)item)[4].ToString());
          }
          catch (Exception)
          {
            SaveLogs("Invalid Type User - " + result.TypeUser + " line error: + " + head);
          }

          try
          {
            result.StatusUser = byte.Parse(((Newtonsoft.Json.Linq.JContainer)item)[5].ToString());
          }
          catch (Exception)
          {
            SaveLogs("Invalid Status User - " + result.StatusUser + " line error: + " + head);
          }

          result.NameCompany = ((Newtonsoft.Json.Linq.JContainer)item)[6].ToString();
          if (result.NameCompany == string.Empty)
            SaveLogs("Name Company is empty - line error: + " + head);

          result.NameOccupation = ((Newtonsoft.Json.Linq.JContainer)item)[7].ToString();
          if (result.NameOccupation == string.Empty)
            SaveLogs("Name Occupation is empty - line error: + " + head);

          result.NameOccupationGroup = ((Newtonsoft.Json.Linq.JContainer)item)[8].ToString();
          if (result.NameOccupationGroup == string.Empty)
            SaveLogs("Name Occupation Group is empty - line error: + " + head);

          try
          {
            result.Registration = long.Parse(((Newtonsoft.Json.Linq.JContainer)item)[9].ToString());
          }
          catch (Exception)
          {
            SaveLogs("Invalid Registration - " + result.Registration + " line error: + " + head);
          }

          result.NameManager = ((Newtonsoft.Json.Linq.JContainer)item)[10].ToString();
          var dateadm = ((Newtonsoft.Json.Linq.JContainer)item)[11].ToString();
          var datebirth = ((Newtonsoft.Json.Linq.JContainer)item)[11].ToString();
          try
          {
            result.DateBirth = DateTime.Parse(dateadm);
            result.DateAdm = DateTime.Parse(datebirth);
          }
          catch (Exception)
          {
            try
            {
              result.DateBirth = DateTime.ParseExact(dateadm, "MM/dd/yyyy", CultureInfo.InvariantCulture);
              result.DateAdm = DateTime.ParseExact(datebirth, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
              try
              {
                result.DateBirth = DateTime.ParseExact(dateadm, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                result.DateAdm = DateTime.ParseExact(datebirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
              }
              catch (Exception)
              {
                try
                {
                  result.DateBirth = DateTime.ParseExact(dateadm, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                  result.DateAdm = DateTime.ParseExact(datebirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                  SaveLogs("Invalid DateTime - " + dateadm + " or " + datebirth);
                }
              }
            }

          }

          result.DocumentManager = ((Newtonsoft.Json.Linq.JContainer)item)[13].ToString();
          if (result.DocumentManager == string.Empty)
            SaveLogs("Document Manager- line error: + " + head);

          result.NameArea = ((Newtonsoft.Json.Linq.JContainer)item)[14].ToString();
          if (result.NameArea == string.Empty)
            SaveLogs("Name Area is empty - line error: + " + head);

          result.NameSchooling = ((Newtonsoft.Json.Linq.JContainer)item)[15].ToString();
          if (result.NameSchooling == string.Empty)
            SaveLogs("Name Schooling is empty - line error: + " + head);



          var sendList = new List<ViewPersonImport>();
          sendList.Add(result);
          var jsonCSV = JsonConvert.SerializeObject(sendList);
          var contentCSV = new StringContent(jsonCSV, Encoding.UTF8, "application/json");
          var client = new HttpClient
          {
            BaseAddress = new Uri(url + "/integrationperson/")
          };

          contentCSV.Headers.ContentType.MediaType = "application/json";
          client.DefaultRequestHeaders.Add("ContentType", "application/json");
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

          var t = new Thread(() => sendApi(contentCSV, client));
          t.Start();
        }
        catch (Exception e)
        {
          throw e;
        }
      }
      return "ok";
    }

    public async Task<string> SaveLogs(string message)
    {
      message += " error file import in date " + DateTime.Now;
      try
      {
        string[] text = { message };
        string[] lines = null;
        try
        {
          text = System.IO.File.ReadAllLines("log_integration.txt");
          lines = new string[text.Count() + 1];
          var count = 0;
          foreach (var item in text)
          {
            lines.SetValue(item, count);
            count += 1;
          }
          lines.SetValue(message, text.Count());
        }
        catch (Exception)
        {
          lines = new string[1];
          lines.SetValue(message, 0);
        }

        System.IO.File.WriteAllLines("log_integration.txt", lines);
        Console.WriteLine("Error file verify logs");
        Console.ReadKey();
        Environment.Exit(0);

        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
