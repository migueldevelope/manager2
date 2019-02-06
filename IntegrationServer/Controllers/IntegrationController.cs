using System;
using System.Collections.Generic;
using Manager.Core.Business.Integration;
using Manager.Core.Interfaces;
using Manager.Views.Integration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationServer.InfraController
{
  [Produces("application/json")]
  [Route("integration")]
  public class IntegrationController : Controller
  {
    private readonly IServiceIntegration service;

    public IntegrationController(IServiceIntegration _service, IHttpContextAccessor contextAccessor)
    {
      try
      {
        service = _service;
        service.SetUser(contextAccessor);
      }
      catch (Exception)
      {
        throw;
      }
    }

    #region Dashboard
    [Authorize]
    [HttpGet]
    [Route("status")]
    public IActionResult GetStatusDashboard()
    {
      try
      {
        return Ok(service.GetStatusDashboard());
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    #endregion

    #region Company
    [Authorize]
    [HttpGet]
    [Route("company/list")]
    public List<ViewIntegrationCompany> GetCompanyList(int count = 10, int page = 1, string filter = "", bool all = false)
    {
      long total = 0;
      var result = service.CompanyList(ref total, count, page, filter, all);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpPost]
    [Route("company/update/{idintegration}/{idcompany}")]
    public IActionResult PostCompany(string idintegration, string idcompany)
    {
      try
      {
        return Ok(service.CompanyUpdate(idintegration, idcompany));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    #endregion

    #region Schooling
    [Authorize]
    [HttpGet]
    [Route("schooling/list")]
    public List<IntegrationSchooling> GetSchoolingList(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.SchoolingList(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpPost]
    [Route("schooling/update/{idintegration}/{id}")]
    public IActionResult PostSchooling(string idintegration, string id)
    {
      try
      {
        return Ok(service.SchoolingUpdate(idintegration, id));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    #endregion

    #region Establishment
    [Authorize]
    [HttpGet]
    [Route("establishment/list")]
    public List<IntegrationEstablishment> GetEstablishmentList(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.EstablishmentList(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpPost]
    [Route("establishment/update/{idintegration}/{id}")]
    public IActionResult PostEstablishment(string idintegration, string id)
    {
      try
      {
        return Ok(service.EstablishmentUpdate(idintegration, id));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    #endregion

    #region Occupation
    [Authorize]
    [HttpGet]
    [Route("occupation/list")]
    public List<IntegrationOccupation> GetOccupationList(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.OccupationList(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpPost]
    [Route("occupation/update/{idintegration}/{id}")]
    public IActionResult PostOccupation(string idintegration, string id)
    {
      try
      {
        return Ok(service.OccupationUpdate(idintegration, id));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    #endregion

  }
}
