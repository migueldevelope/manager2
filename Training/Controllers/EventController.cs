using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Training.Controllers
{
  /// <summary>
  /// Controlador de Registro de Turmas de Treinamento
  /// </summary>
  [Produces("application/json")]
  [Route("event")]
  public class EventController : DefaultController
  {
    private readonly IServiceEvent service;

    #region Constructor
    /// <summary>
    /// Construtor do controle
    /// </summary>
    /// <param name="_service">Serviço específico</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public EventController(IServiceEvent _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region event
    /// <summary>
    /// Excluir um evento
    /// </summary>
    /// <param name="id">Identificador do evento</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<string> Delete(string id)
    {
      return await Task.Run(() =>service.Remove(id));
    }
    /// <summary>
    /// Excluir um evento histórico
    /// </summary>
    /// <param name="id">Identificador do evento histórico</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteeventhistoric/{id}")]
    public async Task<string> DeleteEventHistoric(string id)
    {
      return await Task.Run(() =>service.RemoveEventHistoric(id));
    }
    /// <summary>
    /// Excluir um curso
    /// </summary>
    /// <param name="id">Identificador do curso</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deletecourse/{id}")]
    public async Task<string> DeleteCourse(string id)
    {
      return await Task.Run(() =>service.RemoveCourse(id));
    }
    /// <summary>
    /// Excluir um curso do e-Social
    /// </summary>
    /// <param name="id">Identificador do curso do e-Social</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deletecourseesocial/{id}")]
    public async Task<string> DeleteCourseESocial(string id)
    {
      return await Task.Run(() =>service.RemoveCourseESocial(id));
    }
    /// <summary>
    /// Excluir um participante do evento
    /// </summary>
    /// <param name="idevent">Identificador do evento</param>
    /// <param name="idperson">Identificador da pessoa</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("removeparticipant/{idevent}/{idperson}")]
    public async Task<string> RemoveParticipant(string idevent, string idperson)
    {
      return await Task.Run(() =>service.RemoveParticipant(idevent, idperson));
    }
    /// <summary>
    /// Excluir um dia do evento
    /// </summary>
    /// <param name="idevent">Identificador do evento</param>
    /// <param name="begin">Inicio</param>
    /// <param name="end">Fim</param>
    /// <param name="idday">Identificador do dia</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("removedays/{idevent}/{idday}")]
    public async Task<string> RemoveDays(string idevent, string begin, string end, string idday)
    {
      return await Task.Run(() =>service.RemoveDays(idevent, idday));
    }
    /// <summary>
    /// Excluir instrutor do evento
    /// </summary>
    /// <param name="idevent">Identificador do evento</param>
    /// <param name="id">Identificador do instrutor</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("removeinstructor/{idevent}/{id}")]
    public async Task<string> RemoveInstructor(string idevent, string id)
    {
      return await Task.Run(() =>service.RemoveInstructor(idevent, id));
    }
    /// <summary>
    /// Marcar a presença do aluno
    /// </summary>
    /// <param name="idevent">Identificador do evento</param>
    /// <param name="idparticipant">Identificador do participante</param>
    /// <param name="idday">Identificador do dia</param>
    /// <param name="present">Marcar presença o retirar presença</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("present/{idevent}/{idparticipant}/{idday}/{present}")]
    public async Task<string> Present(string idevent, string idparticipant, string idday, bool present)
    {
      return await Task.Run(() =>service.Present(idevent, idparticipant, idday, present));
    }
    /// <summary>
    /// Setar a grade de participantes
    /// </summary>
    /// <param name="idevent">Identificador do evento</param>
    /// <param name="idparticipant">Identificador do perticipante</param>
    /// <param name="grade"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("setgrade/{idevent}/{idparticipant}/{grade}")]
    public async Task<string> SetGrade(string idevent, string idparticipant, decimal grade)
    {
      return await Task.Run(() =>service.SetGrade(idevent, idparticipant, grade));
    }
    /// <summary>
    /// Reabrir evento
    /// </summary>
    /// <param name="idevent">Identificador do evento</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("reopeningevent/{idevent}")]
    public async Task<string> ReopeningEvent(string idevent)
    {
      return await Task.Run(() =>service.ReopeningEvent(idevent));
    }
    /// <summary>
    /// Inclusão de um evento
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [HttpPost]
    [Route("new")]
    public async Task<ViewListEvent> New([FromBody]ViewCrudEvent view)
    {
      return await Task.Run(() =>service.New(view));
    }
    /// <summary>
    /// Lista eventos
    /// </summary>
    /// <param name="count"></param>
    /// <param name="type"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("list/{type}")]
    public async Task<List<ViewListEventDetail>> List(EnumTypeEvent type, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(type,ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
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
    public async Task<List<ViewListEventSubscription>> ListEventOpenSubscription(string idperson,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventOpenSubscription(idperson, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
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
    public async Task<List<ViewListEventSubscription>> ListEventSubscription(string idperson,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventSubscription(idperson, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }

    /// <summary>
    /// Lista para instrutor marcar presença
    /// </summary>
    /// <param name="idperson"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listeventinstructor/{idperson}")]
    public async Task<List<ViewListEventSubscription>> ListEventInstructor(string idperson, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventInstructor(idperson, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="idperson"></param>
    /// <param name="idcourse"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("listhistoric")]
    public async Task<List<ViewListHistoric>> ListHistoric([FromBody]ViewFilterDate date, string idperson = "", string idcourse = "")
    {
      var result = service.ListHistoric(idperson,idcourse,date);
      return await Task.Run(() => result);
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
    public async Task<List<ViewListEvent>> ListEventOpen( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventOpen(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
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
    public async Task<List<ViewListEvent>> ListEventEnd( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventEnd(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
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
    public async Task<List<ViewListPersonResume>> ListPersonInstructor(string idevent, string idcompany,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPersonInstructor(idevent, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
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
    public async Task<List<ViewListPersonResume>> ListPersonParticipants(string idevent, string idcompany,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPersonParticipants(idevent, idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
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
    public async Task<List<ViewCrudEntity>> ListEntity( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEntity(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }
    /// <summary>
    /// Busca informações de evento para editar 
    /// </summary>
    /// <param name="id">Identificador evento</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudEvent> List(string id)
    {
      return await Task.Run(() =>service.Get(id));
    }
    /// <summary>
    /// Atualiza informações de um evento
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<ViewListEvent> Update([FromBody]ViewCrudEvent view)
    {
      return await Task.Run(() =>service.Update(view));
    }
    /// <summary>
    /// Inclusão histórico de evento
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("neweventhistoric")]
    public async Task<string> NewEventHistoricFrontEnd([FromBody]ViewCrudEventHistoric view)
    {
      return await Task.Run(() =>service.NewEventHistoricFrontEnd(view));
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
    public async Task<List<ViewListEventHistoric>> ListEventHistoric( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventHistoric(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }

    /// <summary>
    /// Lista os histórico de evento de um instrutor
    /// </summary>
    /// <param name="id">Identificador usuário</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("listeventhistoricinstructor/{id}")]
    public async Task<List<ViewCrudEventHistoric>> ListEventHistoricInstructor(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventHistoricInstructor(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
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
    public async Task<List<ViewCrudEventHistoric>> ListEventHistoricPerson(string id,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEventHistoricPerson(id, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }
    /// <summary>
    /// Busca informações para editar um histórico de evento
    /// </summary>
    /// <param name="id">Identificador histórico de evento</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("geteventhistoric/{id}")]
    public async Task<ViewCrudEventHistoric> GetEventHistoric(string id)
    {
      return await Task.Run(() =>service.GetEventHistoric(id));
    }
    /// <summary>
    /// Atualiza informações de um historico de eventos
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateeventhistoric")]
    public async Task<string> UpdateEventHistoric([FromBody]ViewCrudEventHistoric view)
    {
      return await Task.Run(() =>service.UpdateEventHistoricFrontEnd(view));
    }
    /// <summary>
    /// Inclusão de um novo curso
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [HttpPost]
    [Route("newcourse")]
    public async Task<string> NewCourse([FromBody]ViewCrudCourse view)
    {
      return await Task.Run(() =>service.NewCourse(view));
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
    public async Task<List<ViewListCourse>> ListCourse( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCourse(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }
    /// <summary>
    /// Busca informações para editar um curso
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getcourse/{id}")]
    public async Task<ViewCrudCourse> GetCourse(string id)
    {
      return await Task.Run(() =>service.GetCourse(id));
    }
    /// <summary>
    /// Atualizar informaçõe de um curso
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatecourse")]
    public async Task<string> UpdateCourse([FromBody]ViewCrudCourse view)
    {
      return await Task.Run(() =>service.UpdateCourse(view));
    }
    /// <summary>
    /// Inclusão curso esocial
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [HttpPost]
    [Route("newcourseesocial")]
    public async Task<string> NewCourseESocial([FromBody]ViewCrudCourseESocial view)
    {
      return await Task.Run(() =>service.NewCourseESocial(view));
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
    public async Task<List<ViewCrudCourseESocial>> ListCourseESocial( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListCourseESocial(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }
    /// <summary>
    /// Busca informações de um curso do esocial para editar
    /// </summary>
    /// <param name="id">Identificador do curso esocial</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getcourseesocial/{id}")]
    public async Task<ViewCrudCourseESocial> GetCourseESocial(string id)
    {
      return await Task.Run(() =>service.GetCourseESocial(id));
    }
    /// <summary>
    /// Atualiza informações de um curso do esocial
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatecourseesocial")]
    public async Task<string> UpdateCourseESocial([FromBody]ViewCrudCourseESocial view)
    {
      return await Task.Run(() =>service.UpdateCourseESocial(view));
    }
    /// <summary>
    /// Adiciona um participante a um evento
    /// </summary>
    /// <param name="participant">Objeto Crud</param>
    /// <param name="idevent">Identificador evento</param>
    /// <returns></returns>
    [HttpPost]
    [Route("addparticipant/{idevent}")]
    public async Task<string> AddParticipant([FromBody]ViewCrudParticipant participant, string idevent)
    {
      return await Task.Run(() =>service.AddParticipant(idevent, participant));
    }
    /// <summary>
    /// Adicionar dias a um evento
    /// </summary>
    /// <param name="days">Objeto Crud</param>
    /// <param name="idevent">Identificador evento</param>
    /// <returns></returns>
    [HttpPost]
    [Route("adddays/{idevent}")]
    public async Task<string> AddDays([FromBody]ViewCrudDaysEvent days, string idevent)
    {
      return await Task.Run(() =>service.AddDays(idevent, days));
    }
    /// <summary>
    /// Adiciona um instrutor em um evento
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <param name="idevent">Identificador evento</param>
    /// <returns></returns>
    [HttpPost]
    [Route("addinstructor/{idevent}")]
    public async Task<string> AddDays([FromBody]ViewCrudInstructor view, string idevent)
    {
      return await Task.Run(() =>service.AddInstructor(idevent, view));
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
    public async Task<List<ViewCrudParticipant>> ListParticipants(string idevent,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListParticipants(idevent, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }
    #endregion

  }
}