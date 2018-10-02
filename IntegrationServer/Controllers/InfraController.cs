using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegrationServer.Views;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace IntegrationServer.InfraController
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
    [Route("skill")]
    public IActionResult FindSkill([FromBody]ViewIntegrationFilterName filter)
    {
      try
      {
        Skill skill = service.GetSkill(filter.Name);
        if (skill == null)
          return NotFound("Skill não encontrada!");
        return Ok(new ViewIntegrationSkill()
        {
          IdSkill = skill._id,
          Name = skill.Name,
          Concept = skill.Concept,
          TypeSkill = (int)skill.TypeSkill
        });
      }
      catch (Exception ex)
      {
        return BadRequest(ex.ToString());
      }
    }
    [Authorize]
    [HttpPost]
    [Route("skill/new")]
    public Skill AddSkill([FromBody]ViewIntegrationSkill view)
    {

      ViewAddSkill newSkill = new ViewAddSkill()
      {
         Concept=view.Concept,
         Name = view.Name,
         TypeSkill = (EnumTypeSkill)view.TypeSkill
      };
      return service.AddSkill(newSkill);
    }
    [Authorize]
    [HttpPost]
    [Route("processleveltwo")]
    public IActionResult FindProcessLevelTwo([FromBody]ViewIntegrationFilterName filter)
    {
      try
      {
        ProcessLevelTwo process = service.GetProcessLevelTwo(filter.Id);
        if (process == null)
          return NotFound("Sub processo não encontrado!");
        return Ok(new ViewIntegrationProcessLevelTwo(){
          Id = process._id,
          Name = process.Name,
          IdProcessLevelOne = process.ProcessLevelOne._id,
          NameProcessLevelOne = process.ProcessLevelOne.Name,
          IdArea = process.ProcessLevelOne.Area._id,
          NameArea = process.ProcessLevelOne.Area.Name,
          IdCompany = process.ProcessLevelOne.Area.Company._id,
          NameCompany = process.ProcessLevelOne.Area.Company.Name 
        });
      }
      catch (Exception ex)
      {
        return BadRequest(ex.ToString());
      }
    }
    [Authorize]
    [HttpPost]
    [Route("group")]
    public IActionResult FindGroup([FromBody]ViewIntegrationFilterName filter)
    {
      try
      {
        Group group = service.GetGroup(filter.IdCompany,filter.Name);
        if (group == null)
          return NotFound("Grupo de cargo não encontrado!");
        ViewIntegrationGroup view =new ViewIntegrationGroup()
        {
          Id = group._id,
          Name = group.Name,
          Schooling = new List<string>()
        };
        foreach (var item in group.Schooling)
          view.Schooling.Add(item.Name);
        return Ok(view);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.ToString());
      }
    }
    [Authorize]
    [HttpPost]
    [Route("occupation")]
    public IActionResult FindOccupation([FromBody]ViewIntegrationFilterName filter)
    {
      try
      {
        Occupation occupation = service.GetOccupation(filter.IdCompany, filter.Name);
        if (occupation == null)
          return NotFound("Cargo não encontrado!");
        return Ok(new ViewIntegrationOccupation()
        {
          IdOccupation = occupation._id,
          Name = occupation.Name,
          NameGroup = occupation.Group.Name,
          IdProcessLevelTwo = occupation.Process[0]._id,
          NameProcessLevelTwo = occupation.Process[0].Name,
          IdProcessLevelOne = occupation.Process[0].ProcessLevelOne._id,
          NameProcessLevelOne = occupation.Process[0].ProcessLevelOne.Name,
          IdArea = occupation.Process[0].ProcessLevelOne.Area._id,
          NameArea = occupation.Process[0].ProcessLevelOne.Area.Name,
          IdCompany = occupation.Process[0].ProcessLevelOne.Area.Company._id,
          NameCompany = occupation.Process[0].ProcessLevelOne.Area.Company.Name
        });
      }
      catch (Exception ex)
      {
        return BadRequest(ex.ToString());
      }
    }
    [Authorize]
    [HttpPost]
    [Route("occupation/new")]
    public IActionResult AddOccupation([FromBody]ViewIntegrationOccupation view)
    {
      try
      {
        ProcessLevelTwo processLevelTwo = service.GetProcessLevelTwo(view.IdProcessLevelTwo);
        Group group = service.GetGroup(processLevelTwo.ProcessLevelOne.Area.Company._id, view.NameGroup);
        List<Area> areas = new List<Area> {
          processLevelTwo.ProcessLevelOne.Area
        };
        List<ProcessLevelTwo> process = new List<ProcessLevelTwo> {
          processLevelTwo
        };
        List<Activitie> activitie = new List<Activitie>();
        long order = 1;
        foreach (var item in view.Activities) {
          activitie.Add(new Activitie() {
            _id= ObjectId.GenerateNewId().ToString(),
            _idAccount = processLevelTwo._idAccount,
            Status = EnumStatus.Enabled,
            Name = item
          });
          order++;
        }
        List<Skill> skills = new List<Skill>();
        foreach (var item in view.Skills)
        {
          Skill skill = service.GetSkill(item);
          if (skill == null)
            return BadRequest("Skill não encontrada");
          skills.Add(skill);
        }
        List<Schooling> schoolings = group.Schooling;
        for (int i = 0; i < view.Schooling.Count; i++)
          schoolings[i].Complement = view.SchoolingComplement[i];

        Occupation newOccupation = new Occupation()
        {
          Name = view.Name,
          Group = group,
          Area = processLevelTwo.ProcessLevelOne.Area,
          Line = 0,
          ProcessLevelTwo = processLevelTwo,
          Areas = areas,
          Status = EnumStatus.Enabled,
          Process = process,
          Activities = activitie,
          Skills = skills,
          Schooling = schoolings 
        };
        return Ok(service.AddOccupation(newOccupation));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.ToString());
      }
    }
  }
}