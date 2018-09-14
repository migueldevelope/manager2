using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("event")]
  public class EventController : Controller
  {
    private readonly IServiceEvent service;

    public EventController(IServiceEvent _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [HttpPost]
    [Route("new")]
    public string Post([FromBody]Event view)
    {
      return service.New(view);
    }

    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<Event> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("get")]
    public Event List(string id)
    {
      return service.Get(id);
    }

    [Authorize]
    [HttpPut]
    [Route("update")]
    public string Update([FromBody]Event view)
    {
      return service.Update(view);
    }

    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public string Delete(string id)
    {
      return service.Remove(id);
    }


    [HttpPost]
    [Route("newcourse")]
    public string PostCourse([FromBody]Course view)
    {
      return service.NewCourse(view);
    }

    [Authorize]
    [HttpGet]
    [Route("listcourse")]
    public List<Course> ListCourse(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCourse(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("getcourse/{id}")]
    public Course ListCourse(string id)
    {
      return service.GetCourse(id);
    }

    [Authorize]
    [HttpPut]
    [Route("updatecourse")]
    public string UpdateCourse([FromBody]Course view)
    {
      return service.UpdateCourse(view);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletecourse/{id}")]
    public string DeleteCourse(string id)
    {
      return service.RemoveCourse(id);
    }

    [HttpPost]
    [Route("newcourseesocial")]
    public string PostCourseESocial([FromBody]CourseESocial view)
    {
      return service.NewCourseESocial(view);
    }

    [Authorize]
    [HttpGet]
    [Route("listcourseesocial")]
    public List<CourseESocial> ListCourseESocial(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCourseESocial(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("getcourseesocial/{id}")]
    public CourseESocial ListCourseESocial(string id)
    {
      return service.GetCourseESocial(id);
    }

    [Authorize]
    [HttpPut]
    [Route("updatecourseesocial")]
    public string UpdateCourseESocial([FromBody]CourseESocial view)
    {
      return service.UpdateCourseESocial(view);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletecourseesocial/{id}")]
    public string DeleteCourseEsocial(string id)
    {
      return service.RemoveCourseESocial(id);
    }
  }
}