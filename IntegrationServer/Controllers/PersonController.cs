using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegrationServer.Views;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Core.Views.Integration;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace IntegrationServer.InfraController
{
  [Produces("application/json")]
  [Route("person")]
  public class PersonController : Controller
  {
    private readonly IServiceCompany service;
    private readonly IServiceInfra serviceInfra;

    public PersonController(IServiceCompany _service, IServiceInfra _serviceInfra, IHttpContextAccessor contextAccessor)
    {
      try
      {
        service = _service;
        serviceInfra = _serviceInfra;
        service.SetUser(contextAccessor);
      }
      catch (Exception)
      {
        throw;
      }
    }
    [Authorize]
    [HttpPost]
    [Route("schooling")]
    public IActionResult GetSchoolingByName([FromBody]ViewIntegrationMapOfV1 view)
    {
      try
      {
        Schooling schooling = service.GetByName(view.Name);
        if (schooling == null)
        {
          view.Id = string.Empty;
          view.Message = "Schooling not found!";
        }
        else
        {
          view.Id = schooling._id;
          view.Message = string.Empty;
        }
        return Ok(view);
      }
      catch (Exception ex)
      {
        view.Message = ex.Message;
        return Ok(view);
      }
    }

    [Authorize]
    [HttpPost]
    [Route("company")]
    public IActionResult GetCompanyByName([FromBody]ViewIntegrationMapOfV1 view)
    {
      try
      {
        Company company = service.GetByName(view.Name);
        if (company == null)
        {
          view.Id = string.Empty;
          view.Message = "Company not found!";
        }
        else
        {
          view.Id = company._id;
          view.Message = string.Empty;
        }
        return Ok(view);
      }
      catch (Exception ex)
      {
        view.Message = ex.Message;
        return Ok(view);
      }
    }

    [Authorize]
    [HttpPost]
    [Route("establishment")]
    public IActionResult GetEstablishmentByName([FromBody]ViewIntegrationMapOfV1 view)
    {
      try
      {
        Establishment establishment = service.GetEstablishmentByName(view.IdCompany, view.Name);
        if (establishment == null)
        {
          view.Id = string.Empty;
          view.Message = "Establishment not found!";
        }
        else
        {
          view.Id = establishment._id;
          view.Message = string.Empty;
        }
        return Ok(view);
      }
      catch (Exception ex)
      {
        view.Message = ex.Message;
        return Ok(view);
      }
    }

    [Authorize]
    [HttpPost]
    [Route("occupation")]
    public IActionResult GetOccupationByName([FromBody]ViewIntegrationMapOfV1 view)
    {
      try
      {
        Occupation occupation = serviceInfra.GetOccupation(view.IdCompany, view.Name);
        if (occupation == null)
        {
          view.Id = string.Empty;
          view.Message = "Occupation not found!";
        }
        else
        {
          view.Id = occupation._id;
          view.Message = string.Empty;
        }
        return Ok(view);
      }
      catch (Exception ex)
      {
        view.Message = ex.Message;
        return Ok(view);
      }
    }
  }
}
