﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
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

    #region Constructor
    public EventController(IServiceEvent _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    #endregion

    #region event

    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public string Remove(string id)
    {
      return service.Remove(id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteeventhistoric/{id}")]
    public string RemoveEventHistoric(string id)
    {
      return service.RemoveEventHistoric(id);
    }


    [Authorize]
    [HttpDelete]
    [Route("deletecourse/{id}")]
    public string RemoveCourse(string id)
    {
      return service.RemoveCourse(id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletecourseesocial/{id}")]
    public string RemoveCourseESocial(string id)
    {
      return service.RemoveCourseESocial(id);
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
    [HttpPut]
    [Route("reopeningevent/{idevent}")]
    public string ReopeningEvent(string idevent)
    {
      return service.ReopeningEvent(idevent);
    }


    /// <summary>
    /// Inclusão de um evento
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [HttpPost]
    [Route("new")]
    public ViewListEvent New([FromBody]ViewCrudEvent view)
    {
      return service.New(view);
    }

    /// <summary>
    /// Lista eventos
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public List<ViewListEvent> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Lista eventos para inscrição
    /// </summary>
    /// <param name="idperson">Identificador contrato</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listeventopensubscription/{idperson}")]
    public List<ViewListEvent> ListEventOpenSubscription(string idperson, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventOpenSubscription(idperson, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Lista eventos inscritos
    /// </summary>
    /// <param name="idperson">Identificador contrato</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listeventsubscription/{idperson}")]
    public List<ViewListEvent> ListEventSubscription(string idperson, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventSubscription(idperson, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Lista eventos abertos
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listeventopen")]
    public List<ViewListEvent> ListEventOpen(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventOpen(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Lista eventos encerrados
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listeventend")]
    public List<ViewListEvent> ListEventEnd(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventEnd(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Lista instrutores de um evento
    /// </summary>
    /// <param name="idevent">Identificador evento</param>
    /// <param name="idcompany">Identificador empresa</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listpersoninstructor/{idevent}/{idcompany}")]
    public List<ViewListPersonResume> ListPersonInstructor(string idevent, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPersonInstructor(idevent, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Lista participante do evento
    /// </summary>
    /// <param name="idevent">Identificador evento</param>
    /// <param name="idcompany">Identificador empresa</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listpersonparticipants/{idevent}/{idcompany}")]
    public List<ViewListPersonResume> ListPersonParticipants(string idevent, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPersonParticipants(idevent, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Lista entidades
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listentity")]
    public List<ViewCrudEntity> ListEntity(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEntity(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Busca informações de evento para editar 
    /// </summary>
    /// <param name="id">Identificador evento</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public ViewCrudEvent List(string id)
    {
      return service.Get(id);
    }

    /// <summary>
    /// Atualiza informações de um evento
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public ViewListEvent Update([FromBody]ViewCrudEvent view)
    {
      return service.Update(view);
    }


    /// <summary>
    /// Inclusão histórico de evento
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("neweventhistoric")]
    public string NewEventHistoricFrontEnd([FromBody]ViewCrudEventHistoric view)
    {
      return service.NewEventHistoricFrontEnd(view);
    }

    /// <summary>
    /// Lista histórico de eventos
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listeventhistoric")]
    public List<ViewListEventHistoric> ListEventHistoric(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventHistoric(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Lista os histórico de evento de um contrato
    /// </summary>
    /// <param name="id">Identificador usuário</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("listeventhistoricperson/{id}")]
    public List<ViewListEventHistoric> ListEventHistoricPerson(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventHistoricPerson(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Busca informações para editar um histórico de evento
    /// </summary>
    /// <param name="id">Identificador histórico de evento</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("geteventhistoric/{id}")]
    public ViewCrudEventHistoric GetEventHistoric(string id)
    {
      return service.GetEventHistoric(id);
    }

    /// <summary>
    /// Atualiza informações de um historico de eventos
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateeventhistoric")]
    public string UpdateEventHistoric([FromBody]ViewCrudEventHistoric view)
    {
      return service.UpdateEventHistoricFrontEnd(view);
    }


    /// <summary>
    /// Inclusão de um novo curso
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [HttpPost]
    [Route("newcourse")]
    public string NewCourse([FromBody]ViewCrudCourse view)
    {
      return service.NewCourse(view);
    }

    /// <summary>
    /// Lista os cursos
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listcourse")]
    public List<ViewListCourse> ListCourse(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCourse(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Busca informações para editar um curso
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getcourse/{id}")]
    public ViewCrudCourse GetCourse(string id)
    {
      return service.GetCourse(id);
    }

    /// <summary>
    /// Atualizar informaçõe de um curso
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatecourse")]
    public string UpdateCourse([FromBody]ViewCrudCourse view)
    {
      return service.UpdateCourse(view);
    }

    /// <summary>
    /// Inclusão curso esocial
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [HttpPost]
    [Route("newcourseesocial")]
    public string NewCourseESocial([FromBody]ViewCrudCourseESocial view)
    {
      return service.NewCourseESocial(view);
    }

    /// <summary>
    /// Lista cursos do esocial
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listcourseesocial")]
    public List<ViewCrudCourseESocial> ListCourseESocial(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCourseESocial(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Busca informações de um curso do esocial para editar
    /// </summary>
    /// <param name="id">Identificador do curso esocial</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getcourseesocial/{id}")]
    public ViewCrudCourseESocial GetCourseESocial(string id)
    {
      return service.GetCourseESocial(id);
    }

    /// <summary>
    /// Atualiza informações de um curso do esocial
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatecourseesocial")]
    public string UpdateCourseESocial([FromBody]ViewCrudCourseESocial view)
    {
      return service.UpdateCourseESocial(view);
    }

    /// <summary>
    /// Adiciona um participante a um evento
    /// </summary>
    /// <param name="participant">Objeto Crud</param>
    /// <param name="idevent">Identificador evento</param>
    /// <returns></returns>
    [HttpPost]
    [Route("addparticipant/{idevent}")]
    public string AddParticipant([FromBody]ViewCrudParticipant participant, string idevent)
    {
      return service.AddParticipant(idevent, participant);
    }

    /// <summary>
    /// Adicionar dias a um evento
    /// </summary>
    /// <param name="days">Objeto Crud</param>
    /// <param name="idevent">Identificador evento</param>
    /// <returns></returns>
    [HttpPost]
    [Route("adddays/{idevent}")]
    public string AddDays([FromBody]ViewCrudDaysEvent days, string idevent)
    {
      return service.AddDays(idevent, days);
    }

    /// <summary>
    /// Adiciona um instrutor em um evento
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <param name="idevent">Identificador evento</param>
    /// <returns></returns>
    [HttpPost]
    [Route("addinstructor/{idevent}")]
    public string AddDays([FromBody]ViewCrudInstructor view, string idevent)
    {
      return service.AddInstructor(idevent, view);
    }

    /// <summary>
    /// Lista os participante de um evento
    /// </summary>
    /// <param name="idevent">Identificador do evento</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listparticipants/{idevent}")]
    public List<ViewCrudParticipant> ListParticipants(string idevent, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListParticipants(idevent, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    #endregion

    #region old
    [HttpPost]
    [Route("old/new")]
    public Event NewOld([FromBody]Event view)
    {
      return service.NewOld(view);
    }

    [Authorize]
    [HttpGet]
    [Route("old/list")]
    public List<Event> ListOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOld(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/listeventopensubscription/{idperson}")]
    public List<Event> ListEventOpenSubscriptionOld(string idperson, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventOpenSubscriptionOld(idperson, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/listeventsubscription/{idperson}")]
    public List<Event> ListEventSubscriptionOld(string idperson, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventSubscriptionOld(idperson, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/listeventopen")]
    public List<Event> ListEventOpenOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventOpenOld(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/listeventend")]
    public List<Event> ListEventEndOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventEndOld(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/listpersoninstructor/{idevent}/{idcompany}")]
    public List<Person> ListPersonInstructorOld(string idevent, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPersonInstructorOld(idevent, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/listpersonparticipants/{idevent}/{idcompany}")]
    public List<Person> ListPersonParticipantsOld(string idevent, string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPersonParticipantsOld(idevent, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/listentity")]
    public List<Entity> ListEntityOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEntityOld(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/get/{id}")]
    public Event GetOld(string id)
    {
      return service.GetOld(id);
    }

    [Authorize]
    [HttpPut]
    [Route("old/update")]
    public Event UpdateOld([FromBody]Event view)
    {
      return service.UpdateOld(view);
    }


    [HttpPost]
    [Route("old/neweventhistoric")]
    public string NewEventHistoricFrontEndOld([FromBody]EventHistoric view)
    {
      return service.NewEventHistoricFrontEndOld(view);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listeventhistoric")]
    public List<EventHistoric> ListEventHistoricOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventHistoricOld(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [HttpGet]
    [Route("old/listeventhistoricperson/{id}")]
    public List<EventHistoric> ListEventHistoricPersonOld(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventHistoricPersonOld(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/geteventhistoric/{id}")]
    public EventHistoric GetEventHistoricOld(string id)
    {
      return service.GetEventHistoricOld(id);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updateeventhistoric")]
    public string UpdateEventHistoricFrontEndOld([FromBody]EventHistoric view)
    {
      return service.UpdateEventHistoricFrontEndOld(view);
    }

    [HttpPost]
    [Route("old/newcourse")]
    public string NewCourseOld([FromBody]Course view)
    {
      return service.NewCourseOld(view);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listcourse")]
    public List<Course> ListCourseOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCourseOld(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/getcourse/{id}")]
    public Course GetCourseOld(string id)
    {
      return service.GetCourseOld(id);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updatecourse")]
    public string UpdateCourseOld([FromBody]Course view)
    {
      return service.UpdateCourseOld(view);
    }

    [HttpPost]
    [Route("old/newcourseesocial")]
    public string NewCourseESocialOld([FromBody]CourseESocial view)
    {
      return service.NewCourseESocialOld(view);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listcourseesocial")]
    public List<CourseESocial> ListCourseESocialOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCourseESocialOld(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/getcourseesocial/{id}")]
    public CourseESocial ListCourseESocialOld(string id)
    {
      return service.GetCourseESocialOld(id);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updatecourseesocial")]
    public string UpdateCourseESocialOld([FromBody]CourseESocial view)
    {
      return service.UpdateCourseESocialOld(view);
    }


    [HttpPost]
    [Route("old/addparticipant/{idevent}")]
    public string AddParticipantOld([FromBody]Participant participant, string idevent)
    {
      return service.AddParticipantOld(idevent, participant);
    }

    [HttpPost]
    [Route("old/adddays/{idevent}")]
    public string AddDaysOld([FromBody]DaysEvent days, string idevent)
    {
      return service.AddDaysOld(idevent, days);
    }

    [HttpPost]
    [Route("old/addinstructor/{idevent}")]
    public string AddInstructorOld([FromBody]Instructor view, string idevent)
    {
      return service.AddInstructorOld(idevent, view);
    }


    [Authorize]
    [HttpGet]
    [Route("old/listparticipants/{idevent}")]
    public List<Participant> ListParticipantsOld(string idevent, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListParticipantsOld(idevent, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    #endregion




  }
}