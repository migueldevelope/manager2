using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("onboarding")]
  public class OnBoardingController : Controller
  {
    private readonly IServiceOnBoarding service;

    public OnBoardingController(IServiceOnBoarding _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [Authorize]
    [HttpPost]
    [Route("new/{idperson}")]
    public OnBoarding Post([FromBody]OnBoarding onboarding, string idperson)
    {
      return service.NewOnBoarding(onboarding, idperson);
    }

    [Authorize]
    [HttpPut]
    [Route("update/{idperson}")]
    public string Put([FromBody]OnBoarding onboarding, string idperson)
    {
      return service.UpdateOnBoarding(onboarding, idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("listend/{idmanager}")]
    public List<OnBoarding> ListEnd(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOnBoardingsEnd(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("list/{idmanager}")]
    public List<OnBoarding> List(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOnBoardingsWait(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Route("personend/{idmanager}")]
    public List<OnBoarding> PersonOnBoardingsEnd(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.PersonOnBoardingsEnd(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("personwait/{idmanager}")]
    public OnBoarding ListPerson(string idmanager)
    {
      return service.PersonOnBoardingsWait(idmanager);
    }

    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public OnBoarding GetOnBoarding(string id)
    {
      return service.GetOnBoardings(id);
    }

    [Authorize]
    [HttpGet]
    [Route("getlistexclud")]
    public List<OnBoarding> GetListExclud(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetListExclud(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpDelete]
    [Route("delete/{idperson}")]
    public string RemoveOnBoarding(string idperson)
    {
      return service.RemoveOnBoarding(idperson);
    }


    [Authorize]
    [HttpPut]
    [Route("updatecomments/{idonboarding}/{iditem}")]
    public string UpdateComments([FromBody]ListComments comments, string idonboarding, string iditem)
    {
      return service.UpdateComments(idonboarding, iditem, comments);
    }

    [Authorize]
    [HttpGet]
    [Route("listcomments/{idonboarding}/{iditem}")]
    public List<ListComments> GetListComments( string idonboarding, string iditem)
    {
      return service.GetListComments(idonboarding, iditem);
    }

    [Authorize]
    [HttpPost]
    [Route("addcomments/{idonboarding}/{iditem}")]
    public List<ListComments> AddComments([FromBody]ListComments comments, string idonboarding, string iditem)
    {
      return service.AddComments(idonboarding, iditem, comments);
    }


    [Authorize]
    [HttpDelete]
    [Route("deletecomments/{idonboarding}/{iditem}/{idcomments}")]
    public string DeleteComments(string idonboarding, string iditem, string idcomments)
    {
      return service.DeleteComments(idonboarding, iditem, idcomments);
    }

    [Authorize]
    [HttpPut]
    [Route("updatecommentsview/{idonboarding}/{iditem}/{usercomment}")]
    public string UpdateCommentsView(string idonboarding, string iditem, EnumUserComment usercomment)
    {
      return service.UpdateCommentsView(idonboarding, iditem, usercomment);
    }


  }
}