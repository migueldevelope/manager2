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
    [Route("")]
    public string AddArea(ViewAddArea view)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public string AddAxis(ViewAddAxis view)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public string AddEssential(ViewAddEssential view)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public string AddGroup(ViewAddGroup view)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public string AddMapGroupSchooling(ViewAddMapGroupSchooling view)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public string AddMapGroupScope(ViewAddMapGroupScope view)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public string AddMapGroupSkill(ViewAddMapGroupSkill view)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public string AddOccupation(ViewAddOccupation view)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public string AddOccupationActivities(ViewAddOccupationActivities view)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public string AddOccupationSkill(ViewAddOccupationSkill view)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public string AddSchooling(ViewAddOccupationSchooling view)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public string AddSkill(ViewAddSkill view)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public string AddSphere(ViewAddSphere view)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete]
    [Route("")]
    public string DeleteArea(Area area)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete]
    [Route("")]
    public string DeleteAxis(Axis axis)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete]
    [Route("")]
    public string DeleteEssential(Company company, string id)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete]
    [Route("")]
    public string DeleteGroup(Group group)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete]
    [Route("")]
    public string DeleteMapGroupSchooling(Group group, string id)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete]
    [Route("")]
    public string DeleteMapGroupSkill(Group group, string id)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete]
    [Route("")]
    public string DeleteOccupation(Occupation occupation)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete]
    [Route("")]
    public string DeleteOccupationActivities(Occupation occupation, string activitie)
    {
      throw new NotImplementedException();
    }

    public string DeleteOccupationSkill(Occupation occupation, string id)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete]
    [Route("")]
    public string DeleteSchooling(Occupation occupation, string id)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete]
    [Route("")]
    public string DeleteSkill(Skill skill)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete]
    [Route("")]
    public string DeleteSphere(Sphere sphere)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet]
    [Route("")]
    public List<Area> GetAreas()
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet]
    [Route("")]
    public List<Axis> GetAxis()
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet]
    [Route("")]
    public List<Company> GetCompanies()
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet]
    [Route("")]
    public Group GetGroup(string id)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet]
    [Route("")]
    public List<Group> GetGroups()
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet]
    [Route("")]
    public Occupation GetOccupation(string id)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet]
    [Route("")]
    public List<Occupation> GetOccupations()
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet]
    [Route("")]
    public List<Schooling> GetSchooling()
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet]
    [Route("")]
    public List<Skill> GetSkills(ref long total, string filter, int count, int page)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet]
    [Route("")]
    public List<Sphere> GetSpheres()
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut]
    [Route("")]
    public string UpdateArea(Area area)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut]
    [Route("")]
    public string UpdateAxis(Axis axis)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut]
    [Route("")]
    public string UpdateEssential(ViewAddEssential view)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut]
    [Route("")]
    public string UpdateGroup(Group group)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut]
    [Route("")]
    public string UpdateOccupation(Occupation occupation)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut]
    [Route("")]
    public string UpdateSkill(Skill skill)
    {
      throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut]
    [Route("")]
    public string UpdateSphere(Sphere sphere)
    {
      throw new NotImplementedException();
    }
  }
}