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
    [Route("addcbo")]
    public string AddCbo([FromBody]CBO view)
    {
      return service.AddCBO(view);
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
    [Route("addspecificrequirements/{idoccupation}")]
    public string AddSpecificRequirements([FromBody]ViewAddSpecificRequirements view, string idoccupation)
    {
      return service.AddSpecificRequirements(idoccupation, view);
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
      service.AddGroup(view);
      return "OK";
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
    [Route("addoccupationactivitieslist")]
    public string AddOccupationActivitiesList([FromBody]List<ViewAddOccupationActivities> list)
    {
      return service.AddOccupationActivitiesList(list);
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
    [Route("addskills")]
    public string AddSkills([FromBody]List<ViewAddSkill> view)
    {
      service.AddSkills(view);
      return "ok";
    }

    [Authorize]
    [HttpPost]
    [Route("addsphere")]
    public string AddSphere([FromBody]Sphere view)
    {
      return service.AddSphere(view);
    }

    [Authorize]
    [HttpPost]
    [Route("addschooling")]
    public string AddSchooling([FromBody]Schooling schooling)
    {
      service.AddSchooling(schooling);
      return "ok";
    }


    [Authorize]
    [HttpPost]
    [Route("addprocesslevelone")]
    public string AddProcessLevelOne([FromBody]ProcessLevelOne processLevelOne)
    {
      service.AddProcessLevelOne(processLevelOne);
      return "ok";
    }

    [Authorize]
    [HttpPost]
    [Route("addprocessleveltwo")]
    public string AddProcessLevelTwo([FromBody]ProcessLevelTwo processLevelTwo)
    {
      service.AddProcessLevelTwo(processLevelTwo);
      return "ok";
    }

    [Authorize]
    [HttpPost]
    [Route("addquestions")]
    public string AddQuestions([FromBody]Questions questions)
    {
      service.AddQuestions(questions);
      return "ok";
    }

    [Authorize]
    [HttpPost]
    [Route("addtextdefault")]
    public string AddTextDefault([FromBody]TextDefault textDefault)
    {
      service.AddTextDefault(textDefault);
      return "ok";
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
    [Route("deletequestion/{id}")]
    public string DeleteQuestion(string id)
    {
      return service.DeleteQuestion(id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deletetextdefault/{id}")]
    public string DeleteTextDefault(string id)
    {
      return service.DeleteTextDefault(id);
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
    [Route("deletecbo/{id}")]
    public string DeleteCBO(string id)
    {
      return service.DeleteCBO(id);
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
    [HttpDelete]
    [Route("deleteschooling/{idschooling}")]
    public string DeleteSchooling(string idschooling)
    {
      return service.DeleteSchooling(idschooling);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteprocesslevelone/{id}")]
    public string DeleteProcessLevelOne(string id)
    {
      return service.DeleteProcessLevelOne(id);
    }

    [Authorize]
    [HttpDelete]
    [Route("deleteprocessleveltwo/{id}")]
    public string DeleteProcessLevelTwo(string id)
    {
      return service.DeleteProcessLevelTwo(id);
    }

    [Authorize]
    [HttpGet]
    [Route("listquestions/{idcompany}")]
    public List<Questions> GetQuestions(string idcompany)
    {
      return service.ListQuestions(idcompany);
    }

    [Authorize]
    [HttpGet]
    [Route("listcbo")]
    public List<CBO> ListCBO()
    {
      return service.ListCBO();
    }

    [Authorize]
    [HttpGet]
    [Route("getcbo/{id}")]
    public CBO GetCBO(string id)
    {
      return service.GetCBO(id);
    }

    [Authorize]
    [HttpGet]
    [Route("getquestions/{id}")]
    public Questions GetQuestionsId(string id)
    {
      return service.GetQuestions(id);
    }


    [Authorize]
    [HttpGet]
    [Route("gettextdefault/{idcompany}/{name}")]
    public TextDefault GetTextDefault(string idcompany, string name)
    {
      return service.GetTextDefault(idcompany, name);
    }

    [Authorize]
    [HttpGet]
    [Route("gettextdefault/{id}")]
    public TextDefault GetTextDefault(string id)
    {
      return service.GetTextDefault(id);
    }

    [Authorize]
    [HttpGet]
    [Route("listtextdefault/{idcompany}")]
    public List<TextDefault> ListTextDefault(string idcompany)
    {
      return service.ListTextDefault(idcompany);
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
    [Route("getareas/{idcompany}")]
    public List<Area> GetAreas(string idcompany)
    {
      return service.GetAreas(idcompany);
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
    [Route("getgroupsprint/{idcompany}")]
    public List<Group> GetGroupsPrint(string idcompany)
    {
      return service.GetGroupsPrint(idcompany);
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
    [Route("getoccupationsinfra")]
    public List<Occupation> GetOccupationsInfra(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetOccupationsInfra(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listoccupationedit/{idcompany}")]
    public List<ViewOccupationListEdit> ListOccupationEdit(string idcompany, int count = 10, int page = 1, string filter = "", string filterGroup = "")
    {
      long total = 0;
      var result = service.ListOccupationsEdit(idcompany, ref total, filter, count, page, filterGroup);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("getoccupations/{idcompany}/{idarea}")]
    public List<Occupation> GetOccupations(string idcompany, string idarea)
    {
      return service.GetOccupations(idcompany, idarea);
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
    [Route("getskillsinfra")]
    public List<Skill> GetSkillsinfra(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetSkillsInfra(ref total, filter, count, page);
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
    [Route("getskills/{idcompany}/{idgroup}/{idoccupation}")]
    public List<ViewSkills> GetSkills(string idcompany, string idgroup, string idoccupation, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetSkillsOccupation(idgroup, idcompany, idoccupation, ref total, filter, count, page);
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
    [Route("getprocessleveltwo")]
    public List<ProcessLevelTwo> GetProcessLevelTwo()
    {
      return service.GetProcessLevelTwo();
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
    [Route("updatemapgroupscope/{idgroup}")]
    public string UpdateMapGroupScope([FromBody]Scope scope, string idgroup)
    {
      return service.UpdateMapGroupScope(idgroup, scope);
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
    [Route("updatequestions")]
    public string UpdateQuestions([FromBody]Questions questions)
    {
      return service.UpdateQuestions(questions);
    }

    [Authorize]
    [HttpPut]
    [Route("updatetextdefault")]
    public string UpdateTextDefault([FromBody]TextDefault textDefault)
    {
      return service.UpdateTextDefault(textDefault);
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

    [Authorize]
    [HttpPut]
    [Route("updatemapoccupationactivities/{idoccupation}")]
    public string UpdateMapOccupationActivities([FromBody]Activitie activitie, string idoccupation)
    {
      return service.UpdateMapOccupationActivities(idoccupation, activitie);
    }

    [Authorize]
    [HttpPut]
    [Route("updateschooling")]
    public string UpdateSchooling([FromBody]Schooling schooling)
    {
      return service.UpdateSchooling(schooling);
    }

    [Authorize]
    [HttpPut]
    [Route("updateprocesslevelone")]
    public string UpdateProcessLevelOne([FromBody]ProcessLevelOne processLevelOne)
    {
      return service.UpdateProcessLevelOne(processLevelOne);
    }

    [Authorize]
    [HttpPut]
    [Route("updateprocessleveltwo")]
    public string UpdateProcessLevelTwo([FromBody]ProcessLevelTwo processLevelTwo)
    {
      return service.UpdateProcessLevelTwo(processLevelTwo);
    }

    [Authorize]
    [HttpPut]
    [Route("updatecbo")]
    public string UpdateCBO([FromBody]CBO model)
    {
      return service.UpdateCBO(model);
    }

    [Authorize]
    [HttpPut]
    [Route("areaorder/{idcompany}/{idarea}/{order}/{sum}")]
    public string AreaOrder(string idcompany, string idarea, long order, bool sum)
    {
      return service.AreaOrder(idcompany, idarea, order, sum);
    }

    [Authorize]
    [HttpPut]
    [Route("reordergroupscope/{idcompany}/{idgroup}/{idscope}/{sum}")]
    public string ReorderGroupScope(string idcompany, string idgroup, string idscope, bool sum)
    {
      return service.ReorderGroupScope(idcompany, idgroup, idscope, sum);
    }

    [Authorize]
    [HttpPut]
    [Route("reorderoccupationactivitie/{idcompany}/{idoccupation}/{idactivitie}/{sum}")]
    public string ReorderOccupationActivitie(string idcompany, string idoccupation, string idactivitie, bool sum)
    {
      return service.ReorderOccupationActivitie(idcompany, idoccupation, idactivitie, sum);
    }

    [Authorize]
    [HttpPut]
    [Route("reordergroupscopemanual/{idcompany}/{idgroup}/{idscope}/{order}")]
    public string ReorderGroupScopeManual(string idcompany, string idgroup, string idscope, long order)
    {
      return service.ReorderGroupScopeManual(idcompany, idgroup, idscope, order);
    }

    [Authorize]
    [HttpPut]
    [Route("reorderoccupationactivitiemanual/{idcompany}/{idoccupation}/{idactivitie}/{order}")]
    public string ReorderOccupationActivitieManual(string idcompany, string idoccupation, string idactivitie, long order)
    {
      return service.ReorderOccupationActivitieManual(idcompany, idoccupation, idactivitie, order);
    }

    [Authorize]
    [HttpGet]
    [Route("getcsvcomparegroup/{idcompany}")]
    public string[] GetCSVCompareGroup(string idcompany)
    {
      return service.GetCSVCompareGroup(idcompany);
    }

  }
}