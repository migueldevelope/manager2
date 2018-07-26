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
    public string AddArea([FromBody]ViewAddArea view)
    {
      return service.AddArea(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addaxis")]
    public string AddAxis([FromBody]ViewAddAxis view)
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
    [Route("addschooling")]
    public string AddSchooling([FromBody]ViewAddOccupationSchooling view)
    {
      return service.AddSchooling(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addskill")]
    public string AddSkill([FromBody]ViewAddSkill view)
    {
      return service.AddSkill(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addsphere")]
    public string AddSphere([FromBody]ViewAddSphere view)
    {
      return service.AddSphere(view);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletearea")]
    public string DeleteArea([FromBody]Area area)
    {
      return service.DeleteArea(area);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteaxis")]
    public string DeleteAxis([FromBody]Axis axis)
    {
      return service.DeleteAxis(axis);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteessential/{id}")]
    public string DeleteEssential([FromBody]Company company, string id)
    {
      return service.DeleteEssential(company, id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletegroup")]
    public string DeleteGroup([FromBody]Group group)
    {
      return service.DeleteGroup(group);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletemapgroupschooling/{id}")]
    public string DeleteMapGroupSchooling([FromBody]Group group, string id)
    {
      return service.DeleteMapGroupSchooling(group, id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletemapgroupskill/{id}")]
    public string DeleteMapGroupSkill([FromBody]Group group, string id)
    {
      return service.DeleteMapGroupSkill(group, id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteoccupation")]
    public string DeleteOccupation([FromBody]Occupation occupation)
    {
      return service.DeleteOccupation(occupation);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteoccupationactivities/{activitie}")]
    public string DeleteOccupationActivities([FromBody]Occupation occupation, string activitie)
    {
      return service.DeleteOccupationActivities(occupation, activitie);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteoccupationskill/{id}")]
    public string DeleteOccupationSkill([FromBody]Occupation occupation, string id)
    {
      return service.DeleteOccupationSkill(occupation, id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteschooling/{id}")]
    public string DeleteSchooling([FromBody]Occupation occupation, string id)
    {
      return service.DeleteSchooling(occupation, id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteskill")]
    public string DeleteSkill([FromBody]Skill skill)
    {
      return service.DeleteSkill(skill);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletesphere")]
    public string DeleteSphere([FromBody]Sphere sphere)
    {
      return service.DeleteSphere(sphere);
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
    [Route("getschooling")]
    public List<Schooling> GetSchooling()
    {
      return service.GetSchooling();
    }

    [Authorize]
    [HttpGet]
    [Route("getskills")]
    public List<Skill> GetSkills(ref long total, string filter, int count, int page)
    {
      return service.GetSkills(ref total, filter, count, page);
    }

    [Authorize]
    [HttpGet]
    [Route("getspheres")]
    public List<Sphere> GetSpheres()
    {
      return service.GetSpheres();
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
    [Route("updateessential")]
    public string UpdateEssential(ViewAddEssential view)
    {
      return service.UpdateEssential(view);
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
  }
}