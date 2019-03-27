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
using Manager.Views.Enumns;
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
    [Route("getaxisbyid/{id}")]
    public ViewCrudAxis GetAxisById(string id)
    {
      return service.GetAxisById(id);
    }

    /// <summary>
    /// Busca informações para editar Escolaridade
    /// </summary>
    /// <param name="id">Identificador da escolaridade</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getschoolingbyid/{id}")]
    public ViewCrudSchooling GetSchoolingById(string id)
    {
      return service.GetSchoolingById(id);
    }

    /// <summary>
    /// Busca informações para editar Skill
    /// </summary>
    /// <param name="id">Identificador</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getskillbyid/{id}")]
    public ViewCrudSkill GetSkillById(string id)
    {
      return service.GetSkillById(id);
    }

    /// <summary>
    /// Busca informações para editar Sphere
    /// </summary>
    /// <param name="id">Identificador</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getspherebyid/{id}")]
    public ViewCrudSphere GetSphereById(string id)
    {
      return service.GetSphereById(id);
    }

    /// <summary>
    /// Busca informações
    /// </summary>
    /// <param name="idcompany">Identificador empresa</param>
    /// <param name="name">Nome texto</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("gettextdefault/{idcompany}/{name}")]
    public ViewCrudTextDefault GetTextDefault(string idcompany, string name)
    {
      return service.GetTextDefault(idcompany, name);
    }

    /// <summary>
    /// Busca informações para editar
    /// </summary>
    /// <param name="id">Identificador</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("gettextdefault/{id}")]
    public ViewCrudTextDefault GetTextDefault(string id)
    {
      return service.GetTextDefault(id);
    }

    /// <summary>
    /// Lista textos
    /// </summary>
    /// <param name="idcompany">Identificador empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listtextdefault/{idcompany}")]
    public List<ViewListTextDefault> ListTextDefault(string idcompany)
    {
      return service.ListTextDefault(idcompany);
    }

    /// <summary>
    /// Lista cursos por cargos
    /// </summary>
    /// <param name="idoccupation">Identificador cargo</param>
    /// <param name="type">Tipo de treinamento obrigatório</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getcourseoccupation/{idoccupation}/{type}")]
    public List<ViewListCourse> GetCourseOccupation(string idoccupation, EnumTypeMandatoryTraining type)
    {
      return service.GetCourseOccupation(idoccupation, type);
    }

    /// <summary>
    /// Busca informações grupo para editar
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getgroup/{id}")]
    public ViewCrudGroup GetGroup(string id)
    {
      return service.GetGroup(id);
    }

    /// <summary>
    /// Lista grupos
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getgroups")]
    public List<ViewListGroup> GetGroups()
    {
      return service.GetGroups();
    }

    /// <summary>
    /// Lista grupos filtrando pela empresa
    /// </summary>
    /// <param name="idcompany">Identificador empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getgroups/{idcompany}")]
    public List<ViewGroupList> GetGroups(string idcompany)
    {
      return service.GetGroups(idcompany);
    }

    /// <summary>
    /// Lista cargos para visualização impressão
    /// </summary>
    /// <param name="idcompany">Identificador empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getgroupsprint/{idcompany}")]
    public List<ViewListGroup> GetGroupsPrint(string idcompany)
    {
      return service.GetGroupsPrint(idcompany);
    }


    /// <summary>
    /// Busca informações de cargos para editar
    /// </summary>
    /// <param name="id">Identificador cargo</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getoccupation/{id}")]
    public ViewCrudOccupation GetOccupation(string id)
    {
      return service.GetOccupation(id);
    }

    /// <summary>
    /// Lista cargos
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getoccupations")]
    public List<ViewListOccupation> GetOccupations()
    {
      return service.GetOccupations();
    }



    /// <summary>
    /// Lista cargos com filtro de empresa e a area para editar
    /// </summary>
    /// <param name="idcompany">Identificador empresa</param>
    /// <param name="idarea">Identificador Area</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <param name="filterGroup"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Lista cargos com filtro de empresa e area
    /// </summary>
    /// <param name="idcompany">Identificador empresa</param>
    /// <param name="idarea">Identificador Area</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getoccupations/{idcompany}/{idarea}")]
    public List<ViewGetOccupation> GetOccupations(string idcompany, string idarea)
    {
      return service.GetOccupations(idcompany, idarea);
    }

    /// <summary>
    /// Inclusão area
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addarea")]
    public string AddArea([FromBody]ViewCrudArea view)
    {
      return service.AddArea(view);
    }


    /// <summary>
    /// Inclusão cbo
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addcbo")]
    public string AddCbo([FromBody]ViewCrudCbo view)
    {
      return service.AddCBO(view);
    }

    /// <summary> eixo
    /// Inclusão
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addaxis")]
    public string AddAxis([FromBody]ViewCrudAxis view)
    {
      return service.AddAxis(view);
    }

    /// <summary>
    /// Inclusão esfera
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addsphere")]
    public string AddSphere([FromBody]ViewCrudSphere view)
    {
      return service.AddSphere(view);
    }

    /// <summary>
    /// Inclusão escolaridade
    /// </summary>
    /// <param name="schooling">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addschooling")]
    public string AddSchooling([FromBody]ViewCrudSchooling schooling)
    {
      service.AddSchooling(schooling);
      return "ok";
    }


    /// <summary>
    /// Inclusão processo nivel 1
    /// </summary>
    /// <param name="processLevelOne">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addprocesslevelone")]
    public string AddProcessLevelOne([FromBody]ViewCrudProcessLevelOne processLevelOne)
    {
      service.AddProcessLevelOne(processLevelOne);
      return "ok";
    }

    /// <summary>
    /// Inclusão processo nivel 2
    /// </summary>
    /// <param name="processLevelTwo">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addprocessleveltwo")]
    public string AddProcessLevelTwo([FromBody]ViewCrudProcessLevelTwo processLevelTwo)
    {
      service.AddProcessLevelTwo(processLevelTwo);
      return "ok";
    }

    /// <summary>
    /// Inclusão
    /// </summary>
    /// <param name="questions">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addquestions")]
    public string AddQuestions([FromBody]ViewCrudQuestions questions)
    {
      service.AddQuestions(questions);
      return "ok";
    }

    /// <summary>
    /// Inclusao textos
    /// </summary>
    /// <param name="textDefault">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addtextdefault")]
    public string AddTextDefault([FromBody]ViewCrudTextDefault textDefault)
    {
      service.AddTextDefault(textDefault);
      return "ok";
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
    [HttpPost]
    [Route("addspecificrequirements/{idoccupation}")]
    public string AddSpecificRequirements([FromBody]ViewAddSpecificRequirements view, string idoccupation)
    {
      return service.AddSpecificRequirements(idoccupation, view);
    }

    /// <summary>
    /// Inclusão
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addmapgroupschooling")]
    public string AddMapGroupSchooling([FromBody]ViewCrudMapGroupSchooling view)
    {
      return service.AddMapGroupSchooling(view);
    }

    /// <summary>
    /// Inclusão
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addoccupationactivities")]
    public string AddOccupationActivities([FromBody]ViewCrudOccupationActivities view)
    {
      return service.AddOccupationActivities(view);
    }

    /// <summary>
    /// Inclusao skill essencial
    /// </summary>
    /// <param name="view">Objeto CRUD</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addessential")]
    public string AddEssential([FromBody]ViewCrudEssential view)
    {
      return service.AddEssential(view);
    }

    /// <summary>
    /// Inclusao grupo
    /// </summary>
    /// <param name="view">Objeto CRUD</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addgroup")]
    public string AddGroup([FromBody]ViewCrudGroup view)
    {
      service.AddGroup(view);
      return "OK";
    }

    [Authorize]
    [HttpPost]
    [Route("addmapgroupscope")]
    public string AddMapGroupScope([FromBody]ViewCrudMapGroupScope view)
    {
      return service.AddMapGroupScope(view);
    }

    /// <summary>
    /// Inclusao skill no grupo
    /// </summary>
    /// <param name="view">Objeto CRUD</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addmapgroupskill")]
    public string AddMapGroupSkill([FromBody]ViewCrudMapGroupSkill view)
    {
      return service.AddMapGroupSkill(view);
    }

    /// <summary>
    /// Inclusao cargo
    /// </summary>
    /// <param name="view">Objeto CRUD</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addoccupation")]
    public string AddOccupation([FromBody]ViewCrudOccupation view)
    {
      return service.AddOccupation(view);
    }

    //[Authorize]
    //[HttpPost]
    //[Route("addoccupationactivitieslist")]
    //public string AddOccupationActivitiesList([FromBody]List<ViewCrudOccupationActivities> list)
    //{
    //  return service.AddOccupationActivitiesList(list);
    //}

    [Authorize]
    [HttpPost]
    [Route("addoccupationskill")]
    public string AddOccupationSkill([FromBody]ViewAddOccupationSkill view)
    {
      return service.AddOccupationSkill(view);
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

    /// <summary>
    /// Atualiza informações do scopo no grupo
    /// </summary>
    /// <param name="scope">Objeto Crud</param>
    /// <param name="idgroup">Identificador Grupo</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatemapgroupscope/{idgroup}")]
    public string UpdateMapGroupScope([FromBody]ViewCrudScope scope, string idgroup)
    {
      return service.UpdateMapGroupScope(idgroup, scope);
    }
    /// <summary>
    /// Atualiza inforamções de escolaridade no grupo
    /// </summary>
    /// <param name="schooling">Objeto do Crud</param>
    /// <param name="idgroup">Identificador do grupo</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatemapgroupschooling/{idgroup}")]
    public string UpdateMapGroupSchooling([FromBody]ViewCrudSchooling schooling, string idgroup)
    {
      return service.UpdateMapGroupSchooling(idgroup, schooling);
    }

    [Authorize]
    [HttpPut]
    [Route("updatemapoccupationschooling/{idoccupation}")]
    public string UpdateMapOccupationSchooling([FromBody]ViewCrudSchooling schooling, string idoccupation)
    {
      return service.UpdateMapOccupationSchooling(idoccupation, schooling);
    }

    /// <summary>
    /// Atualizar informações de entragas no cargo
    /// </summary>
    /// <param name="activitie">Objeto CRUD</param>
    /// <param name="idoccupation">Identificador do cargo</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatemapoccupationactivities/{idoccupation}")]
    public string UpdateMapOccupationActivities([FromBody]ViewCrudActivities activitie, string idoccupation)
    {
      return service.UpdateMapOccupationActivities(idoccupation, activitie);
    }

    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatearea")]
    public string UpdateArea([FromBody]ViewCrudArea view)
    {
      return service.UpdateArea(view);
    }

    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateaxis")]
    public string UpdateAxis([FromBody]ViewCrudAxis view)
    {
      return service.UpdateAxis(view);
    }

    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updategroup")]
    public string UpdateGroup([FromBody]ViewCrudGroup view)
    {
      return service.UpdateGroup(view);
    }


    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateoccupation")]
    public string UpdateOccupation([FromBody]ViewCrudOccupation view)
    {
      return service.UpdateOccupation(view);
    }

    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateskill")]
    public string UpdateSkill([FromBody]ViewCrudSkill view)
    {
      return service.UpdateSkill(view);
    }

    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatequestions")]
    public string UpdateQuestions([FromBody]ViewCrudQuestions view)
    {
      return service.UpdateQuestions(view);
    }

    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatetextdefault")]
    public string UpdateTextDefault([FromBody]ViewCrudTextDefault view)
    {
      return service.UpdateTextDefault(view);
    }

    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatesphere")]
    public string UpdateSphere([FromBody]ViewCrudSphere view)
    {
      return service.UpdateSphere(view);
    }

    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateschooling")]
    public string UpdateSchooling([FromBody]ViewCrudSchooling view)
    {
      return service.UpdateSchooling(view);
    }

    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateprocesslevelone")]
    public string UpdateProcessLevelOne([FromBody]ViewCrudProcessLevelOne view)
    {
      return service.UpdateProcessLevelOne(view);
    }

    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateprocessleveltwo")]
    public string UpdateProcessLevelTwo([FromBody]ViewCrudProcessLevelTwo view)
    {
      return service.UpdateProcessLevelTwo(view);
    }

    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatecbo")]
    public string UpdateCBO([FromBody]ViewCrudCbo view)
    {
      return service.UpdateCBO(view);
    }



    [Authorize]
    [HttpPost]
    [Route("addskills")]
    public string AddSkills([FromBody]List<ViewAddSkill> view)
    {
      service.AddSkills(view);
      return "ok";
    }

    #endregion



    //################################################# Old #################################################
    #region Old


    [Authorize]
    [HttpPost]
    [Route("old/addessential")]
    public string AddEssentialOld([FromBody]ViewAddEssential view)
    {
      return service.AddEssentialOld(view);
    }

    [Authorize]
    [HttpPost]
    [Route("old/addgroup")]
    public string AddGroupOld([FromBody]ViewAddGroup view)
    {
      service.AddGroupOld(view);
      return "OK";
    }

    [Authorize]
    [HttpPost]
    [Route("old/addmapgroupscope")]
    public string AddMapGroupScopeOld([FromBody]ViewAddMapGroupScope view)
    {
      return service.AddMapGroupScopeOld(view);
    }

    [Authorize]
    [HttpPost]
    [Route("old/addmapgroupskill")]
    public string AddMapGroupSkillOld([FromBody]ViewAddMapGroupSkill view)
    {
      return service.AddMapGroupSkillOld(view);
    }

    [Authorize]
    [HttpPost]
    [Route("old/addoccupation")]
    public string AddOccupationOld([FromBody]ViewAddOccupation view)
    {
      return service.AddOccupationOld(view);
    }

    [Authorize]
    [HttpPost]
    [Route("old/addoccupationactivitieslist")]
    public string AddOccupationActivitiesListOld([FromBody]List<ViewAddOccupationActivities> list)
    {
      return service.AddOccupationActivitiesListOld(list);
    }


    [Authorize]
    [HttpPut]
    [Route("old/updatemapgroupscope/{idgroup}")]
    public string UpdateMapGroupScopeOld([FromBody]Scope scope, string idgroup)
    {
      return service.UpdateMapGroupScopeOld(idgroup, scope);
    }
    [Authorize]
    [HttpPut]
    [Route("old/updatemapgroupschooling/{idgroup}")]
    public string UpdateMapGroupSchoolingOld([FromBody]Schooling schooling, string idgroup)
    {
      return service.UpdateMapGroupSchoolingOld(idgroup, schooling);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updatemapoccupationschooling/{idoccupation}")]
    public string UpdateMapOccupationSchoolingOld([FromBody]Schooling schooling, string idoccupation)
    {
      return service.UpdateMapOccupationSchoolingOld(idoccupation, schooling);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updatemapoccupationactivities/{idoccupation}")]
    public string UpdateMapOccupationActivitiesOld([FromBody]Activitie activitie, string idoccupation)
    {
      return service.UpdateMapOccupationActivitiesOld(idoccupation, activitie);
    }

    [Authorize]
    [HttpPost]
    [Route("old/addarea")]
    public string AddAreaOld([FromBody]Area view)
    {
      return service.AddAreaOld(view);
    }


    [Authorize]
    [HttpPost]
    [Route("old/addcbo")]
    public string AddCboOld([FromBody]Cbo view)
    {
      return service.AddCBOOld(view);
    }

    [Authorize]
    [HttpPost]
    [Route("old/addaxis")]
    public string AddAxisOld([FromBody]Axis view)
    {
      return service.AddAxisOld(view);
    }

    [Authorize]
    [HttpPost]
    [Route("old/addsphere")]
    public string AddSphereOld([FromBody]Sphere view)
    {
      return service.AddSphereOld(view);
    }

    [Authorize]
    [HttpPost]
    [Route("old/addschooling")]
    public string AddSchoolingOld([FromBody]Schooling schooling)
    {
      service.AddSchoolingOld(schooling);
      return "ok";
    }


    [Authorize]
    [HttpPost]
    [Route("old/addprocesslevelone")]
    public string AddProcessLevelOneOld([FromBody]ProcessLevelOne processLevelOne)
    {
      service.AddProcessLevelOneOld(processLevelOne);
      return "ok";
    }

    [Authorize]
    [HttpPost]
    [Route("old/addprocessleveltwo")]
    public string AddProcessLevelTwoOld([FromBody]ProcessLevelTwo processLevelTwo)
    {
      service.AddProcessLevelTwoOld(processLevelTwo);
      return "ok";
    }

    [Authorize]
    [HttpPost]
    [Route("old/addquestions")]
    public string AddQuestionsOld([FromBody]Questions questions)
    {
      service.AddQuestionsOld(questions);
      return "ok";
    }

    [Authorize]
    [HttpPost]
    [Route("old/addtextdefault")]
    public string AddTextDefaultOld([FromBody]TextDefault textDefault)
    {
      service.AddTextDefaultOld(textDefault);
      return "ok";
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
    [Route("old/gettextdefault/{idcompany}/{name}")]
    public TextDefault GetTextDefaultOld(string idcompany, string name)
    {
      return service.GetTextDefaultOld(idcompany, name);
    }

    [Authorize]
    [HttpGet]
    [Route("old/gettextdefault/{id}")]
    public TextDefault GetTextDefaultOld(string id)
    {
      return service.GetTextDefaultOld(id);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listtextdefault/{idcompany}")]
    public List<TextDefault> ListTextDefaultOld(string idcompany)
    {
      return service.ListTextDefaultOld(idcompany);
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
    [Route("old/getgroup/{id}")]
    public Group GetGroupOld(string id)
    {
      return service.GetGroupOld(id);
    }

    [Authorize]
    [HttpGet]
    [Route("old/getgroups")]
    public List<Group> GetGroupsOld()
    {
      return service.GetGroupsOld();
    }


    [Authorize]
    [HttpGet]
    [Route("old/getgroupsprint/{idcompany}")]
    public List<Group> GetGroupsPrintOld(string idcompany)
    {
      return service.GetGroupsPrintOld(idcompany);
    }


    [Authorize]
    [HttpGet]
    [Route("old/getoccupation/{id}")]
    public Occupation GetOccupationOld(string id)
    {
      return service.GetOccupationOld(id);
    }

    [Authorize]
    [HttpGet]
    [Route("old/getoccupations")]
    public List<Occupation> GetOccupationsOld()
    {
      return service.GetOccupationsOld();
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
    [Route("old/updatearea")]
    public string UpdateAreaOld([FromBody]Area area)
    {
      return service.UpdateAreaOld(area);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updateaxis")]
    public string UpdateAxisOld([FromBody]Axis axis)
    {
      return service.UpdateAxisOld(axis);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updategroup")]
    public string UpdateGroupOld([FromBody]Group group)
    {
      return service.UpdateGroupOld(group);
    }

   

    [Authorize]
    [HttpPut]
    [Route("old/updateoccupation")]
    public string UpdateOccupationOld([FromBody]Occupation occupation)
    {
      return service.UpdateOccupationOld(occupation);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updateskill")]
    public string UpdateSkillOld([FromBody]Skill skill)
    {
      return service.UpdateSkillOld(skill);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updatequestions")]
    public string UpdateQuestionsOld([FromBody]Questions questions)
    {
      return service.UpdateQuestionsOld(questions);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updatetextdefault")]
    public string UpdateTextDefaultOld([FromBody]TextDefault textDefault)
    {
      return service.UpdateTextDefaultOld(textDefault);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updatesphere")]
    public string UpdateSphereOld([FromBody]Sphere sphere)
    {
      return service.UpdateSphereOld(sphere);
    }

    
    [Authorize]
    [HttpPut]
    [Route("old/updateschooling")]
    public string UpdateSchoolingOld([FromBody]Schooling schooling)
    {
      return service.UpdateSchoolingOld(schooling);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updateprocesslevelone")]
    public string UpdateProcessLevelOneOld([FromBody]ProcessLevelOne processLevelOne)
    {
      return service.UpdateProcessLevelOneOld(processLevelOne);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updateprocessleveltwo")]
    public string UpdateProcessLevelTwoOld([FromBody]ProcessLevelTwo processLevelTwo)
    {
      return service.UpdateProcessLevelTwoOld(processLevelTwo);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updatecbo")]
    public string UpdateCBOOld([FromBody]Cbo model)
    {
      return service.UpdateCBOOld(model);
    }

    [Authorize]
    [HttpGet]
    [Route("old/getcourseoccupation/{idoccupation}/{type}")]
    public List<Course> GetCourseOccupationOld(string idoccupation, EnumTypeMandatoryTraining type)
    {
      return service.GetCourseOccupationOld(idoccupation, type);
    }
    #endregion

  }
}