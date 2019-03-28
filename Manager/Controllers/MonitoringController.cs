using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("monitoring")]
  public class MonitoringController : Controller
  {
    private readonly IServiceMonitoring service;

    #region Constructor
    public MonitoringController(IServiceMonitoring _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    #endregion

    #region Monitoring

    [Authorize]
    [HttpDelete]
    [Route("deleteall/{idperson}")]
    public string RemoveAllMonitoring(string idperson)
    {
      return service.RemoveAllMonitoring(idperson);
    }

    [Authorize]
    [HttpDelete]
    [Route("delete/{idmonitoring}")]
    public string RemoveOnBoarding(string idmonitoring)
    {
      return service.RemoveMonitoring(idmonitoring);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletelast/{idperson}")]
    public string RemoveLastMonitoring(string idperson)
    {
      return service.RemoveLastMonitoring(idperson);
    }

    [Authorize]
    [HttpDelete]
    [Route("removemonitoringactivities/{idmonitoring}/{idactivitie}")]
    public string RemoveMonitoringActivities(string idmonitoring, string idactivitie)
    {
      return service.RemoveMonitoringActivities(idmonitoring, idactivitie);
    }
    [Authorize]
    [HttpDelete]
    [Route("deletecomments/{idmonitoring}/{iditem}/{idcomments}")]
    public string DeleteComments(string idmonitoring, string iditem, string idcomments)
    {
      return service.DeleteComments(idmonitoring, iditem, idcomments);
    }
    [Authorize]
    [HttpPut]
    [Route("updatecommentsview/{idmonitoring}/{iditem}/{usercomment}")]
    public string UpdateCommentsView(string idmonitoring, string iditem, EnumUserComment usercomment)
    {
      return service.UpdateCommentsView(idmonitoring, iditem, usercomment);
    }

    [Authorize]
    [HttpGet]
    [Route("validcomments/{idmonitoring}")]
    public bool UpdateCommentsView(string idmonitoring)
    {
      return service.ValidComments(idmonitoring);
    }



    //[Authorize]
    //[HttpPost]
    //[Route("new/{idperson}")]
    //public Monitoring NewMonitoring([FromBody]Monitoring monitoring, string idperson)
    //{
    //  return service.NewMonitoring(monitoring, idperson);
    //}

    //[Authorize]
    //[HttpPut]
    //[Route("update/{idperson}")]
    //public string UpdateMonitoring([FromBody]Monitoring monitoring, string idperson)
    //{
    //  return service.UpdateMonitoring(monitoring, idperson);
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("listend/{idmanager}")]
    //public List<Monitoring> ListMonitoringsEnd(string idmanager, int count = 10, int page = 1, string filter = "")
    //{
    //  long total = 0;
    //  var result = service.ListMonitoringsEnd(idmanager, ref total, filter, count, page);
    //  Response.Headers.Add("x-total-count", total.ToString());
    //  return result;
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("getlistexclud")]
    //public List<Monitoring> GetListExclud(int count = 10, int page = 1, string filter = "")
    //{
    //  long total = 0;
    //  var result = service.GetListExclud(ref total, filter, count, page);
    //  Response.Headers.Add("x-total-count", total.ToString());
    //  return result;
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("list/{idmanager}")]
    //public List<Monitoring> ListMonitoringsWait(string idmanager, int count = 10, int page = 1, string filter = "")
    //{
    //  long total = 0;
    //  var result = service.ListMonitoringsWait(idmanager, ref total, filter, count, page);
    //  Response.Headers.Add("x-total-count", total.ToString());
    //  return result;
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("personend/{idmanager}")]
    //public List<Monitoring> PersonMonitoringsEnd(string idmanager)
    //{
    //  return service.PersonMonitoringsEnd(idmanager);
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("personwait/{idmanager}")]
    //public Monitoring PersonMonitoringsWait(string idmanager)
    //{
    //  return service.PersonMonitoringsWait(idmanager);
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("get/{id}")]
    //public Monitoring GetMonitoring(string id)
    //{
    //  return service.GetMonitorings(id);
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("getskills/{idperson}")]
    //public List<Skill> GetSkills(string idperson)
    //{
    //  return service.GetSkills(idperson);
    //}


    //[Authorize]
    //[HttpGet]
    //[Route("getmonitoringactivities/{idmonitoring}/{idactivitie}")]
    //public MonitoringActivities GetMonitoringActivities(string idmonitoring, string idactivitie)
    //{
    //  return service.GetMonitoringActivities(idmonitoring, idactivitie);
    //}


    //[Authorize]
    //[HttpPut]
    //[Route("updatemonitoringactivities/{idmonitoring}")]
    //public string UpdateMonitoringActivities([FromBody]MonitoringActivities activitie, string idmonitoring)
    //{
    //  return service.UpdateMonitoringActivities(idmonitoring, activitie);
    //}

    //[Authorize]
    //[HttpPost]
    //[Route("addmonitoringactivities/{idmonitoring}")]
    //public string AddMonitoringActivities([FromBody] Activitie activitie, string idmonitoring)
    //{
    //  return service.AddMonitoringActivities(idmonitoring, activitie);
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("listcomments/{idmonitoring}/{iditem}")]
    //public List<ListComments> GetListComments(string idmonitoring, string iditem)
    //{
    //  return service.GetListCommentsOld(idmonitoring, iditem);
    //}

    //[Authorize]
    //[HttpPost]
    //[Route("addcomments/{idmonitoring}/{iditem}")]
    //public List<ListComments> AddComments([FromBody]ListComments comments, string idmonitoring, string iditem)
    //{
    //  return service.AddComments(idmonitoring, iditem, comments);
    //}


    //[Authorize]
    //[HttpPut]
    //[Route("updatecomments/{idmonitoring}/{iditem}")]
    //public string UpdateComments([FromBody]ListComments comments, string idmonitoring, string iditem)
    //{
    //  return service.UpdateComments(idmonitoring, iditem, comments);
    //}


    //[Authorize]
    //[HttpPost]
    //[Route("addplan/{idmonitoring}/{iditem}")]
    //public List<Plan> AddPlan([FromBody]Plan plan, string idmonitoring, string iditem)
    //{
    //  return service.AddPlan(idmonitoring, iditem, plan);
    //}

    //[Authorize]
    //[HttpPut]
    //[Route("updateplan/{idmonitoring}/{iditem}")]
    //public List<Plan> UpdatePlan([FromBody]Plan plan, string idmonitoring, string iditem)
    //{
    //  return service.UpdatePlan(idmonitoring, iditem, plan);
    //}

    #endregion

    #region Old

    [Authorize]
    [HttpPost]
    [Route("old/new/{idperson}")]
    public Monitoring NewMonitoringOld([FromBody]Monitoring monitoring, string idperson)
    {
      return service.NewMonitoringOld(monitoring, idperson);
    }

    [Authorize]
    [HttpPut]
    [Route("old/update/{idperson}")]
    public string UpdateMonitoringOld([FromBody]Monitoring monitoring, string idperson)
    {
      return service.UpdateMonitoringOld(monitoring, idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listend/{idmanager}")]
    public List<Monitoring> ListEndOld(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListMonitoringsEndOld(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/getlistexclud")]
    public List<Monitoring> GetListExcludOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetListExcludOld(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/list/{idmanager}")]
    public List<Monitoring> ListOld(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListMonitoringsWaitOld(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/personend/{idmanager}")]
    public List<Monitoring> ListEndPersonOld(string idmanager)
    {
      return service.PersonMonitoringsEndOld(idmanager);
    }

    [Authorize]
    [HttpGet]
    [Route("old/personwait/{idmanager}")]
    public Monitoring ListPersonOld(string idmanager)
    {
      return service.PersonMonitoringsWaitOld(idmanager);
    }

    [Authorize]
    [HttpGet]
    [Route("old/get/{id}")]
    public Monitoring GetMonitoringOld(string id)
    {
      return service.GetMonitoringsOld(id);
    }

    [Authorize]
    [HttpGet]
    [Route("old/getskills/{idperson}")]
    public List<Skill> GetSkillsOld(string idperson)
    {
      return service.GetSkillsOld(idperson);
    }


    [Authorize]
    [HttpGet]
    [Route("old/getmonitoringactivities/{idmonitoring}/{idactivitie}")]
    public MonitoringActivities GetMonitoringActivitiesOld(string idmonitoring, string idactivitie)
    {
      return service.GetMonitoringActivitiesOld(idmonitoring, idactivitie);
    }


    [Authorize]
    [HttpPut]
    [Route("old/updatemonitoringactivities/{idmonitoring}")]
    public string UpdateMonitoringActivitiesOld([FromBody]MonitoringActivities activitie, string idmonitoring)
    {
      return service.UpdateMonitoringActivitiesOld(idmonitoring, activitie);
    }

    [Authorize]
    [HttpPost]
    [Route("old/addmonitoringactivities/{idmonitoring}")]
    public string AddMonitoringActivitiesOld([FromBody] Activitie activitie, string idmonitoring)
    {
      return service.AddMonitoringActivitiesOld(idmonitoring, activitie);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listcomments/{idmonitoring}/{iditem}")]
    public List<ListComments> GetListCommentsOld(string idmonitoring, string iditem)
    {
      return service.GetListCommentsOld(idmonitoring, iditem);
    }

    [Authorize]
    [HttpPost]
    [Route("old/addcomments/{idmonitoring}/{iditem}")]
    public List<ListComments> AddCommentsOld([FromBody]ListComments comments, string idmonitoring, string iditem)
    {
      return service.AddCommentsOld(idmonitoring, iditem, comments);
    }


    [Authorize]
    [HttpPut]
    [Route("old/updatecomments/{idmonitoring}/{iditem}")]
    public string UpdateCommentsOld([FromBody]ListComments comments, string idmonitoring, string iditem)
    {
      return service.UpdateCommentsOld(idmonitoring, iditem, comments);
    }


    [Authorize]
    [HttpPost]
    [Route("old/addplan/{idmonitoring}/{iditem}")]
    public List<Plan> AddPlanOld([FromBody]Plan plan, string idmonitoring, string iditem)
    {
      return service.AddPlanOld(idmonitoring, iditem, plan);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updateplan/{idmonitoring}/{iditem}")]
    public List<Plan> UpdatePlanOld([FromBody]Plan plan, string idmonitoring, string iditem)
    {
      return service.UpdatePlanOld(idmonitoring, iditem, plan);
    }

    #endregion




  }
}