using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Manager.Core.Business;
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
      //CallAPIColdStart();
      return new string[] { "version", "0.000000038" };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Route("onboarding/load")]
    [HttpGet]
    public List<OnBoarding> OnboardingLoad()
    {
      return serviceOnBoarding.Load();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Route("person/load/{id}")]
    [HttpGet]
    public List<Person> PersonLoad(string id)
    {
      try
      {
        if (id != "d41d8cd98f00b204e9800998ecf8427e")
          return null;// new Exception("token_invalid");
        else
        {
          var result = servicePerson.Load();
          return result;
        }
      }
      catch (Exception e)
      {
        throw e;
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
