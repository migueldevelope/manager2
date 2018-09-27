﻿using System;
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
    public Event Post([FromBody]Event view)
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
    [Route("listeventopen")]
    public List<Event> ListEventOpen(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventOpen(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listeventend")]
    public List<Event> ListEventEnd(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventEnd(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listpersoninstructor/{idevent}/{idcompany}")]
    public List<Person> ListPersonInstructor(string idevent, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPersonInstructor(idevent, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listpersonparticipants/{idevent}/{idcompany}")]
    public List<Person> ListPersonParticipants(string idevent, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPersonParticipants(idevent, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listentity")]
    public List<Entity> ListEntity(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEntity(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public Event List(string id)
    {
      return service.Get(id);
    }

    [Authorize]
    [HttpPut]
    [Route("update")]
    public Event Update([FromBody]Event view)
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
    [Route("neweventhistoric")]
    public string PostEventHistoric([FromBody]EventHistoric view)
    {
      return service.NewEventHistoric(view);
    }

    [Authorize]
    [HttpGet]
    [Route("listeventhistoric")]
    public List<EventHistoric> ListEventHistoric(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventHistoric(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [HttpGet]
    [Route("listeventhistoricperson/{id}")]
    public List<EventHistoric> ListEventHistoricPerson(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventHistoricPerson(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("geteventhistoric/{id}")]
    public EventHistoric ListEventHistoric(string id)
    {
      return service.GetEventHistoric(id);
    }

    [Authorize]
    [HttpPut]
    [Route("updateeventhistoric")]
    public string UpdateEventHistoric([FromBody]EventHistoric view)
    {
      return service.UpdateEventHistoric(view);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteeventhistoric/{id}")]
    public string DeleteEventHistoric(string id)
    {
      return service.RemoveEventHistoric(id);
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

    [HttpPost]
    [Route("addparticipant/{idevent}")]
    public string AddParticipant([FromBody]Participant participant, string idevent)
    {
      return service.AddParticipant(idevent, participant);
    }

    [HttpPost]
    [Route("adddays/{idevent}")]
    public string AddDays([FromBody]DaysEvent days, string idevent)
    {
      return service.AddDays(idevent, days);
    }

    [Authorize]
    [HttpDelete]
    [Route("removeparticipant/{idevent}/{idperson}")]
    public string RemoveParticipant(string idevent, string idperson)
    {
      return service.RemoveParticipant(idevent, idperson);
    }

    [Authorize]
    [HttpDelete]
    [Route("removedays/{idevent}/{idday}")]
    public string RemoveDays(string idevent, string begin, string end, string idday)
    {
      return service.RemoveDays(idevent, idday);
    }

    [HttpPost]
    [Route("addinstructor/{idevent}")]
    public string AddDays([FromBody]Instructor view, string idevent)
    {
      return service.AddInstructor(idevent, view);
    }

    [HttpDelete]
    [Route("removeinstructor/{idevent}/{id}")]
    public string RemoveInstructor(string idevent, string id)
    {
      return service.RemoveInstructor(idevent, id);
    }

    [Authorize]
    [HttpPut]
    [Route("present/{idevent}/{idparticipant}/{idday}/{present}")]
    public string Present(string idevent, string idparticipant, string idday, bool present)
    {
      return service.Present(idevent, idparticipant, idday, present);
    }

    [Authorize]
    [HttpPut]
    [Route("setgrade/{idevent}/{idparticipant}/{grade}")]
    public string SetGrade(string idevent, string idparticipant, decimal grade)
    {
      return service.SetGrade(idevent, idparticipant, grade);
    }

    [Authorize]
    [HttpGet]
    [Route("listparticipants/{idevent}")]
    public List<Participant> ListParticipants(string idevent, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListParticipants(idevent, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }


  }
}