using System;
using System.Collections.Generic;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tools;

namespace Manager.Controllers
{
  /// <summary>
  /// Controle da Infra Estrutura
  /// </summary>
  [Produces("application/json")]
  [Route("infra")]
  public class InfraController : Controller
  {
    private readonly IServiceInfra service;

    #region Constructor
    /// <summary>
    /// Construtor da infra estrutura
    /// </summary>
    /// <param name="_service">Servico associado</param>
    /// <param name="contextAccessor">Token de autenticação</param>
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
    #endregion

    #region Infra 
    /// <summary>
    /// Lista de esferas
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getspheres")]
    public List<ViewListSphere> GetSpheres()
    {
      return service.GetSpheres();
    }

    /// <summary>
    /// Lista de esferas filtrando pela empresa
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getspheres/{idcompany}")]
    public List<ViewListSphere> GetSpheres(string idcompany)
    {
      return service.GetSpheres(idcompany);
    }

    /// <summary>
    /// Lista escolaridades
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getschooling")]
    public List<ViewListSchooling> GetSchooling()
    {
      return service.GetSchooling();
    }

    /// <summary>
    /// Lista perguntas filtrando pela empresa
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listquestions/{idcompany}")]
    public List<ViewListQuestions> GetQuestions(string idcompany)
    {
      return service.ListQuestions(idcompany);
    }

    /// <summary>
    /// Lista CBO's
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listcbo")]
    public List<ViewListCbo> ListCBO()
    {
      return service.ListCBO();
    }

    /// <summary>
    /// Busca informações de CBO para editar
    /// </summary>
    /// <param name="id">Identificador do CBO</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getcbo/{id}")]
    public ViewCrudCbo GetCBO(string id)
    {
      return service.GetCBO(id);
    }

    /// <summary>
    /// Lista areas
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getareas")]
    public List<ViewListArea> GetAreas()
    {
      return service.GetAreas();
    }

    /// <summary>
    /// Lista areas filtrando pela empresa
    /// </summary>
    /// <param name="idcompany">Indetificador da empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getareas/{idcompany}")]
    public List<ViewListArea> GetAreas(string idcompany)
    {
      return service.GetAreas(idcompany);
    }

    /// <summary>
    /// Lista Eixos
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getaxis")]
    public List<ViewListAxis> GetAxis()
    {
      return service.GetAxis();
    }

    /// <summary>
    /// Lista eixos filtrando pela empresa
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getaxis/{idcompany}")]
    public List<ViewListAxis> GetAxis(string idcompany)
    {
      return service.GetAxis(idcompany);
    }

    /// <summary>
    /// Busca informações da pergunta para editar
    /// </summary>
    /// <param name="id">Identificador da pergunta</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getquestions/{id}")]
    public ViewCrudQuestions GetQuestionsId(string id)
    {
      return service.GetQuestions(id);
    }

    /// <summary>
    /// Lista as empresas
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getcompanies")]
    public List<ViewListCompany> GetCompanies()
    {
      return service.GetCompanies();
    }

    /// <summary>
    /// Lista as skill's
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getskills")]
    public List<ViewListSkill> GetSkills(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetSkills(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    /// <summary>
    /// Listas os processos nivel 2 filtrando pela area
    /// </summary>
    /// <param name="idarea">Identificador da Area</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getprocessleveltwo/{idarea}")]
    public List<ViewListProcessLevelTwo> GetProcessLevelTwo(string idarea)
    {
      return service.GetProcessLevelTwoFilter(idarea);
    }

    /// <summary>
    /// Lista os processos nivel 2
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getprocessleveltwo")]
    public List<ViewListProcessLevelTwo> GetProcessLevelTwo()
    {
      return service.GetProcessLevelTwo();
    }

    /// <summary>
    /// Inclusao de nova skill
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addskill")]
    public ViewCrudSkill AddSkill([FromBody]ViewAddSkill view)
    {
      return service.AddSkill(view);
    }

    /// <summary>
    /// Busca informações para editar Area
    /// </summary>
    /// <param name="idarea">Identificador da area</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getareasbyid/{idarea}")]
    public ViewCrudArea GetAreasById(string idarea)
    {
      return service.GetAreasById(idarea);
    }


    #endregion




    #region Old

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
    public string AddCbo([FromBody]Cbo view)
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
    [Route("old/addskill")]
    public Skill AddSkillOld([FromBody]ViewAddSkill view)
    {
      return service.AddSkillOld(view);
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
    [Route("old/listquestions/{idcompany}")]
    public List<Questions> GetQuestionsOld(string idcompany)
    {
      return service.ListQuestionsOld(idcompany);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listcbo")]
    public List<Cbo> ListCBOOld()
    {
      return service.ListCBOOld();
    }

    [Authorize]
    [HttpGet]
    [Route("old/getcbo/{id}")]
    public Cbo GetCBOOld(string id)
    {
      return service.GetCBOOld(id);
    }

    [Authorize]
    [HttpGet]
    [Route("old/getquestions/{id}")]
    public Questions GetQuestionsIdOld(string id)
    {
      return service.GetQuestionsOld(id);
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
    [Route("old/getareas")]
    public List<Area> GetAreasOld()
    {
      return service.GetAreasOld();
    }

    [Authorize]
    [HttpGet]
    [Route("old/getareas/{idcompany}")]
    public List<Area> GetAreasOld(string idcompany)
    {
      return service.GetAreasOld(idcompany);
    }

    [Authorize]
    [HttpGet]
    [Route("old/getaxis")]
    public List<Axis> GetAxisOld()
    {
      return service.GetAxisOld();
    }

    [Authorize]
    [HttpGet]
    [Route("old/getaxis/{idcompany}")]
    public List<Axis> GetAxisOld(string idcompany)
    {
      return service.GetAxisOld(idcompany);
    }

    [Authorize]
    [HttpGet]
    [Route("old/getcompanies")]
    public List<Company> GetCompaniesOld()
    {
      return service.GetCompaniesOld();
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
    public List<ViewGroupList> GetGroups(string idcompany)
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

    //[Authorize]
    //[HttpGet]
    //[Route("getoccupationsinfra")]
    //public List<Occupation> GetOccupationsInfra(int count = 10, int page = 1, string filter = "")
    //{
    //  long total = 0;
    //  var result = service.GetOccupationsInfra(ref total, filter, count, page);
    //  Response.Headers.Add("x-total-count", total.ToString());
    //  return result;
    //}

    [Authorize]
    [HttpGet]
    [Route("listoccupationedit/{idcompany}/{idarea}")]
    public List<ViewOccupationListEdit> ListOccupationEdit(string idcompany, string idarea, int count = 10, int page = 1, string filter = "", string filterGroup = "")
    {
      long total = 0;
      var result = service.ListOccupationsEdit(idcompany, idarea, ref total, filter, count, page, filterGroup);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("getoccupations/{idcompany}/{idarea}")]
    public List<ViewGetOccupation> GetOccupations(string idcompany, string idarea)
    {
      return service.GetOccupations(idcompany, idarea);
    }

    [Authorize]
    [HttpGet]
    [Route("old/getschooling")]
    public List<Schooling> GetSchoolingOld()
    {
      return service.GetSchoolingOld();
    }

    [Authorize]
    [HttpGet]
    [Route("old/getskills")]
    public List<Skill> GetSkillsOld(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetSkillsOld(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    //[Authorize]
    //[HttpGet]
    //[Route("getskillsinfra")]
    //public List<Skill> GetSkillsinfra(int count = 10, int page = 1, string filter = "")
    //{
    //  long total = 0;
    //  var result = service.GetSkillsInfra(ref total, filter, count, page);
    //  Response.Headers.Add("x-total-count", total.ToString());
    //  return result;
    //}

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
    [Route("old/getspheres")]
    public List<Sphere> GetSpheresOld()
    {
      return service.GetSpheresOld();
    }

    [Authorize]
    [HttpGet]
    [Route("old/getprocessleveltwo/{idarea}")]
    public List<ProcessLevelTwo> GetProcessLevelTwoOld(string idarea)
    {
      return service.GetProcessLevelTwoFilterOld(idarea);
    }
    [Authorize]
    [HttpGet]
    [Route("old/getprocessleveltwo")]
    public List<ProcessLevelTwo> GetProcessLevelTwoOld()
    {
      return service.GetProcessLevelTwoOld();
    }

    [Authorize]
    [HttpGet]
    [Route("old/getspheres/{idcompany}")]
    public List<Sphere> GetSpheresOld(string idcompany)
    {
      return service.GetSpheresOld(idcompany);
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
    public string UpdateCBO([FromBody]Cbo model)
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
    public string GetCSVCompareGroup(string idcompany)
    {
      var conn = ConnectionNoSqlService.GetConnetionServer();
      return service.GetCSVCompareGroup(idcompany, conn.BlobKey);
    }

    [Authorize]
    [HttpGet]
    [Route("getcourseoccupation/{idoccupation}/{type}")]
    public List<Course> GetCourseOccupation(string idoccupation, EnumTypeMandatoryTraining type)
    {
      return service.GetCourseOccupation(idoccupation, type);
    }
    #endregion

  }
}