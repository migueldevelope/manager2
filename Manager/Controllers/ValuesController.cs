using System.Collections.Generic;
using Manager.Core.Interfaces;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Manager.Controllers
{
  /// <summary>
  /// Controle para verficação de API ativa
  /// </summary>
  [Route("api/[controller]")]
  public class ValuesController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    public IServiceOnBoarding serviceOnBoarding;
    public IServiceParameters serviceParameters;
    public IServiceMonitoring serviceMonitoring;
    public IServiceCertification serviceCertification;
    public IServicePerson servicePerson;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_serviceOnboarding"></param>
    /// <param name="contextAccessor"></param>
    /// <param name="_serviceParameters"></param>
    /// <param name="_serviceMonitoring"></param>
    /// <param name="_serviceCertification"></param>
    /// <param name="_servicePerson"></param>
    public ValuesController(IServiceOnBoarding _serviceOnboarding, IHttpContextAccessor contextAccessor,IServiceParameters _serviceParameters,
     IServiceMonitoring _serviceMonitoring, IServiceCertification _serviceCertification, IServicePerson _servicePerson)
    {
      serviceOnBoarding = _serviceOnboarding;
      serviceParameters = _serviceParameters;
      serviceMonitoring = _serviceMonitoring;
      serviceCertification = _serviceCertification;
      servicePerson = _servicePerson;

      serviceOnBoarding.SetUser(contextAccessor);
    }

    /// <summary>
    /// Método único para demonstração de API REST ativa
    /// </summary>
    /// <returns>String com a versão da API REST</returns>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      long total = 0;
      //service.ListExcluded(ref total, "", 1, 1);
      serviceParameters.List(ref total);
      serviceOnBoarding.ListExcluded(ref total, "", 1, 1);
      serviceMonitoring.GetListExclud(ref total, "", 1, 1);
      serviceCertification.ListPersons(ObjectId.GenerateNewId().ToString(), ref total, "", 1, 1);
      servicePerson.ListCompany(ref total, "", 1, 1);
      return new string[] { "version", "0.000000036" };
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
