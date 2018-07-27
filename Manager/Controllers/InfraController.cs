using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("infra")]
  public class InfraController : Controller
  {
    private readonly IServiceInfra service;

    public InfraController(IServiceInfra _service, IHttpContextAccessor contextAccessor)
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

    [Authorize]
    [HttpPost]
    [Route("addarea")]
    public string AddArea([FromBody]Area view)
    {
      return service.AddArea(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addaxis")]
    public string AddAxis([FromBody]Axis view)
    {
      return service.AddAxis(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addessential")]
    public string AddEssential([FromBody]ViewAddEssential view)
    {
      return service.AddEssential(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addgroup")]
    public string AddGroup([FromBody]ViewAddGroup view)
    {
      return service.AddGroup(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addmapgroupschooling")]
    public string AddMapGroupSchooling([FromBody]ViewAddMapGroupSchooling view)
    {
      return service.AddMapGroupSchooling(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addmapgroupscope")]
    public string AddMapGroupScope([FromBody]ViewAddMapGroupScope view)
    {
      return service.AddMapGroupScope(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addmapgroupskill")]
    public string AddMapGroupSkill([FromBody]ViewAddMapGroupSkill view)
    {
      return service.AddMapGroupSkill(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addoccupation")]
    public string AddOccupation([FromBody]ViewAddOccupation view)
    {
      return service.AddOccupation(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addoccupationactivities")]
    public string AddOccupationActivities([FromBody]ViewAddOccupationActivities view)
    {
      return service.AddOccupationActivities(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addoccupationskill")]
    public string AddOccupationSkill([FromBody]ViewAddOccupationSkill view)
    {
      return service.AddOccupationSkill(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addskill")]
    public Skill AddSkill([FromBody]ViewAddSkill view)
    {
      return service.AddSkill(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addsphere")]
    public string AddSphere([FromBody]Sphere view)
    {
      return service.AddSphere(view);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletearea/{id}")]
    public string DeleteArea(string id)
    {
      return service.DeleteArea(id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteaxis/{idaxis}")]
    public string DeleteAxis(string idaxis)
    {
      return service.DeleteAxis(idaxis);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteessential/{idcompany}/{id}")]
    public string DeleteEssential(string idcompany, string id)
    {
      return service.DeleteEssential(idcompany, id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletegroup/{id}")]
    public string DeleteGroup(string id)
    {
      return service.DeleteGroup(id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletemapgroupschooling/{idgroup}/{id}")]
    public string DeleteMapGroupSchooling(string idgroup, string id)
    {
      return service.DeleteMapGroupSchooling(idgroup, id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletemapgroupskill/{idgroup}/{id}")]
    public string DeleteMapGroupSkill(string idgroup, string id)
    {
      return service.DeleteMapGroupSkill(idgroup, id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletemapgroupscope/{idgroup}/{scope}")]
    public string DeleteMapGroupScope(string idgroup, string scope)
    {
      return service.DeleteMapGroupScope(idgroup, scope);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteoccupation/{id}")]
    public string DeleteOccupation(string id)
    {
      return service.DeleteOccupation(id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteoccupationactivities/{idoccupation}/{activitie}")]
    public string DeleteOccupationActivities(string idoccupation, string activitie)
    {
      return service.DeleteOccupationActivities(idoccupation, activitie);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteoccupationskill/{idoccupation}/{id}")]
    public string DeleteOccupationSkill(string idoccupation, string id)
    {
      return service.DeleteOccupationSkill(idoccupation, id);
    }


    [Authorize]
    [HttpDelete]
    [Route("deleteskill/{idskill}")]
    public string DeleteSkill(string idskill)
    {
      return service.DeleteSkill(idskill);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletesphere/{idsphere}")]
    public string DeleteSphere(string idsphere)
    {
      return service.DeleteSphere(idsphere);
    }

    [Authorize]
    [HttpGet]
    [Route("getareas")]
    public List<Area> GetAreas()
    {
      return service.GetAreas();
    }

    [Authorize]
    [HttpGet]
    [Route("getaxis")]
    public List<Axis> GetAxis()
    {
      return service.GetAxis();
    }

    [Authorize]
    [HttpGet]
    [Route("getaxis/{idcompany}")]
    public List<Axis> GetAxis(string idcompany)
    {
      return service.GetAxis(idcompany);
    }

    [Authorize]
    [HttpGet]
    [Route("getcompanies")]
    public List<Company> GetCompanies()
    {
      return service.GetCompanies();
    }

    [Authorize]
    [HttpGet]
    [Route("getgroup/{id}")]
    public Group GetGroup(string id)
    {
      return service.GetGroup(id);
    }

    [Authorize]
    [HttpGet]
    [Route("getgroups")]
    public List<Group> GetGroups()
    {
      return service.GetGroups();
    }

    [Authorize]
    [HttpGet]
    [Route("getgroups/{idcompany}")]
    public List<Group> GetGroups(string idcompany)
    {
      return service.GetGroups(idcompany);
    }


    [Authorize]
    [HttpGet]
    [Route("getoccupation/{id}")]
    public Occupation GetOccupation(string id)
    {
      return service.GetOccupation(id);
    }

    [Authorize]
    [HttpGet]
    [Route("getoccupations")]
    public List<Occupation> GetOccupations()
    {
      return service.GetOccupations();
    }

    [Authorize]
    [HttpGet]
    [Route("getoccupations/{idcompany}")]
    public List<Occupation> GetOccupations(string idcompany)
    {
      return service.GetOccupations(idcompany);
    }

    [Authorize]
    [HttpGet]
    [Route("getschooling")]
    public List<Schooling> GetSchooling()
    {
      return service.GetSchooling();
    }

    [Authorize]
    [HttpGet]
    [Route("getskills")]
    public List<Skill> GetSkills(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetSkills(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("getskills/{company}")]
    public List<ViewSkills> GetSkills(string company, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetSkills(company, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("getskills/{idcompany}/{idgroup}")]
    public List<ViewSkills> GetSkills(string idcompany, string idgroup, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetSkillsGroup(idgroup, idcompany, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("getspheres")]
    public List<Sphere> GetSpheres()
    {
      return service.GetSpheres();
    }

    [Authorize]
    [HttpGet]
    [Route("getspheres/{idcompany}")]
    public List<Sphere> GetSpheres(string idcompany)
    {
      return service.GetSpheres(idcompany);
    }

    [Authorize]
    [HttpPut]
    [Route("updatearea")]
    public string UpdateArea([FromBody]Area area)
    {
      return service.UpdateArea(area);
    }

    [Authorize]
    [HttpPut]
    [Route("updateaxis")]
    public string UpdateAxis([FromBody]Axis axis)
    {
      return service.UpdateAxis(axis);
    }

    [Authorize]
    [HttpPut]
    [Route("updategroup")]
    public string UpdateGroup([FromBody]Group group)
    {
      return service.UpdateGroup(group);
    }

    [Authorize]
    [HttpPut]
    [Route("updateoccupation")]
    public string UpdateOccupation([FromBody]Occupation occupation)
    {
      return service.UpdateOccupation(occupation);
    }

    [Authorize]
    [HttpPut]
    [Route("updateskill")]
    public string UpdateSkill([FromBody]Skill skill)
    {
      return service.UpdateSkill(skill);
    }

    [Authorize]
    [HttpPut]
    [Route("updatesphere")]
    public string UpdateSphere([FromBody]Sphere sphere)
    {
      return service.UpdateSphere(sphere);
    }

    [Authorize]
    [HttpPut]
    [Route("updatemapgroupschooling/{idgroup}")]
    public string UpdateMapGroupSchooling([FromBody]Schooling schooling, string idgroup)
    {
      return service.UpdateMapGroupSchooling(idgroup, schooling);
    }

    [Authorize]
    [HttpPut]
    [Route("updatemapoccupationschooling/{idoccupation}")]
    public string UpdateMapOccupationSchooling([FromBody]Schooling schooling, string idoccupation)
    {
      return service.UpdateMapOccupationSchooling(idoccupation, schooling);
    }
  }
}