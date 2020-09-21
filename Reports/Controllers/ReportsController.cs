using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Reports.Controllers
{
  /// <summary>
  /// 
  /// </summary>
  [Produces("application/json")]
  [Route("")]
  public class ReportsController
  {
    private readonly IServiceReports service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_service"></param>
    /// <param name="contextAccessor"></param>
    public ReportsController(IServiceReports _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listelearningfluid")]
    public async Task<string> ListElearningFluid()
    {
      return await Task.Run(() => service.ListElearningFluid());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listsalaryscale/{id}")]
    public async Task<string> ListSalaryScale(string id)
    {
      return await Task.Run(() => service.ListSalaryScale(id));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listmyawareness/{idperson}")]
    public async Task<string> ListPersons(string idperson)
    {
      return await Task.Run(() => service.ListMyAwareness(idperson));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idcompany"></param>
    /// <param name="idarea"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listopportunityline/{idcompany}")]
    public async Task<string> ListOpportunityLine(string idcompany, string idarea = "")
    {
      return await Task.Run(() => service.ListOpportunityLine(idcompany, idarea));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="idevent"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listtraining/{idevent}")]
    public async Task<string> ListTraining(string idevent)
    {
      return await Task.Run(() => service.ListTraining(idevent));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idevent"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listtrainingdays/{idevent}")]
    public async Task<string> ListTrainingDays(string idevent)
    {
      return await Task.Run(() => service.ListTrainingDays(idevent));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idevent"></param>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listcertificate/{idevent}")]
    public async Task<string> ListCertificate(string idevent, string idperson = "")
    {
      return await Task.Run(() => service.ListCertificate(idevent, idperson));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("listhistorictraining")]
    public async Task<string> ListHistoricTraining([FromBody]ViewFilterDate date, string idperson = "")
    {
      return await Task.Run(() => service.ListHistoricTraining(date, idperson));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listonboarding/{id}")]
    public async Task<string> ListOnBoarding(string id)
    {
      return await Task.Run(() => service.ListOnBoarding(id));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listmonitoring/{id}")]
    public async Task<string> ListMonitoring(string id)
    {
      return await Task.Run(() => service.ListMonitoring(id));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listoffboarding/{id}")]
    public async Task<string> ListOffBoarding(string id)
    {
      return await Task.Run(() => service.ListOffBoarding(id));
    }

  }
}