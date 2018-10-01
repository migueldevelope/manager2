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
    [Route("findskill")]
    public IActionResult FindSkill([FromBody]string name)
    {
      try
      {
        Skill skill = service.GetSkill(name);
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
    [Route("addskill")]
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
    [Route("findoccupation")]
    public IActionResult FindOccupation([FromBody]ViewIntegrationFindOccupation view)
    {
      try
      {
        Occupation occupation = service.GetOccupation(view.IdCompany, view.Name);
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
    [Route("addoccupation")]
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
        List<Schooling> schoolings = new List<Schooling>();
        foreach (var item in group.Schooling)
        {
          if (item.Name == view.Schooling[0])
            item.Complement = view.SchoolingComplement[0];
          if (item.Name == view.Schooling[1])
            item.Complement = view.SchoolingComplement[1];
          schoolings.Add(item);
        }
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
        return Ok(service.InsertOccupation(newOccupation));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.ToString());
      }
    }
  }
}