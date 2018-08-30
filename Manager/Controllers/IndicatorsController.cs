using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
    [Produces("application/json")]
    [Route("indicators")]
    public class IndicatorsController : Controller
    {
    private readonly IServiceIndicators service;

    public IndicatorsController(IServiceIndicators _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [Authorize]
    [HttpGet]
    [Route("getnotes/{idperson}")]
    public List<ViewIndicatorsNotes> GetNotes(string idperson)
    {
      return service.GetNotes(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("getnotesperson/{idperson}")]
    public List<ViewIndicatorsNotes> GetNotesPerson(string idperson)
    {
      return service.GetNotesPerson(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("listtagscloud/{idmanager}")]
    public List<ViewTagsCloud> ListTagsCloud(string idmanager)
    {
      return service.ListTagsCloud(idmanager);
    }

    [Authorize]
    [HttpGet]
    [Route("listtagscloudcompany/{idmanager}")]
    public List<ViewTagsCloud> ListTagsCloudCompany(string idmanager)
    {
      return service.ListTagsCloudCompany(idmanager);
    }

  }
}