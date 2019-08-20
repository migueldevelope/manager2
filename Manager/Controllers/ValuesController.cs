using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Services.Commons;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using Tools;

namespace Manager.Controllers
{
  /// <summary>
  /// Controle para verficação de API ativa
  /// </summary>
  [Route("api/[controller]")]
  public class ValuesController : Controller
  {
    private IServiceOnBoarding serviceOnBoarding;
    private IServiceParameters serviceParameters;
    private IServiceMonitoring serviceMonitoring;
    private IServiceCertification serviceCertification;
    private IServicePerson servicePerson;
    private IServiceAuthentication serviceAuthentication;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_serviceOnboarding"></param>
    /// <param name="contextAccessor"></param>
    /// <param name="_serviceParameters"></param>
    /// <param name="_serviceMonitoring"></param>
    /// <param name="_serviceCertification"></param>
    /// <param name="_serviceAuthentication"></param>
    /// <param name="_servicePerson"></param>
    public ValuesController(IServiceOnBoarding _serviceOnboarding, IHttpContextAccessor contextAccessor,IServiceParameters _serviceParameters,
     IServiceMonitoring _serviceMonitoring, IServiceCertification _serviceCertification, IServicePerson _servicePerson, IServiceAuthentication _serviceAuthentication)
    {
      serviceOnBoarding = _serviceOnboarding;
      serviceParameters = _serviceParameters;
      serviceMonitoring = _serviceMonitoring;
      serviceCertification = _serviceCertification;
      servicePerson = _servicePerson;
      serviceAuthentication= _serviceAuthentication;

      serviceOnBoarding.SetUser(contextAccessor);
    }

    /// <summary>
    /// Método único para demonstração de API REST ativa
    /// </summary>
    /// <returns>String com a versão da API REST</returns>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      //long total = 0;
      //service.ListExcluded(ref total, "", 1, 1);
      //serviceParameters.List(ref total);
      //serviceOnBoarding.ListExcluded(ref total, "", 1, 1);
      //serviceMonitoring.GetListExclud(ref total, "", 1, 1);
      //serviceCertification.ListPersons(ObjectId.GenerateNewId().ToString(), ref total, "", 1, 1);
      //servicePerson.ListCompany(ref total, "", 1, 1);
      CallAPIColdStart();
      return new string[] { "version", "0.000000036" };
    }

    private void CallAPIColdStart()
    {
      try
      {
        var conn = XmlConnection.ReadVariablesSystem();
        var user = new ViewAuthentication()
        {
          Mail = "suporte@jmsoft.com.br",
          Password = "x14r53p5!a"
        };
        var body = new ViewListIdIndicators
        {
          DateAdm = null,
          Name = "Test",
          OccupationName = "Test",
          TypeJourney = EnumTypeJourney.OnBoarding,
          _id = "5d52b89acf99e80001cae10c"
        };
        var list = new List<ViewListIdIndicators>();
        list.Add(body);

        string token = serviceAuthentication.Authentication(user).Token;
        using (var client = new HttpClient())
        {
          var json = JsonConvert.SerializeObject(list);
          var content = new StringContent(json, Encoding.UTF8, "application/json");
          client.BaseAddress = new Uri(conn.TokenServer.Substring(0, conn.TokenServer.Length - 1) + ":5200/");
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
          var resultMail = client.PostAsync("onboarding/v2/list", content).Result;
          resultMail = client.PostAsync("monitoring/v2/list", content).Result;
          resultMail = client.PostAsync("checkpotin/v2/listwaitmanager", content).Result;
        }
      }
      catch (Exception e)
      {
        var message = e;
      }
    }

    /// <summary>
    /// Test mail
    /// </summary>
    /// <returns></returns>
    [Route("mail")]
    [HttpGet]
    public IEnumerable<string> Mail()
    {
      serviceOnBoarding.MailTest();
      return new string[] { "test", "0.000000035" };
    }

  }
}
