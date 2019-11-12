using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
  public class InfraController : DefaultController
  {
    private readonly IServiceInfra service;

    #region Constructor
    /// <summary>
    /// Construtor da infra estrutura
    /// </summary>
    /// <param name="_service">Servico associado</param>
    /// <param name="contextAccessor">Token de autenticação</param>
    public InfraController(IServiceInfra _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
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

    #region Area
    /// <summary>
    /// Busca informações da area para editar
    /// </summary>
    /// <param name="idarea">Indetificador da area</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getareasbyid/{idarea}")]
    public async Task<ViewCrudArea> GetAreasById(string idarea)
    {
      return await Task.Run(() => service.GetAreasById(idarea));
    }
    /// <summary>
    /// Lista areas
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getareas")]
    public async Task<List<ViewListArea>> GetAreas()
    {
      return await Task.Run(() => service.GetAreas());
    }
    /// <summary>
    /// Lista areas filtrando pela empresa
    /// </summary>
    /// <param name="idcompany">Indetificador da empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getareas/{idcompany}")]
    public async Task<List<ViewListArea>> GetAreas(string idcompany)
    {
      return await Task.Run(() => service.GetAreas(idcompany));
    }
    /// <summary>
    /// Inclusão area
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addarea")]
    public async Task<string> AddArea([FromBody]ViewCrudArea view)
    {
      return await Task.Run(() => service.AddArea(view));
    }
    /// <summary>
    /// Alterar a ordem da área
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="idarea">Identificador da Área</param>
    /// <param name="order">Ordem da área</param>
    /// <param name="sum">Sentido da ordem p/cima ou p/baixo</param>
    /// <returns>Mensagem de Sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("areaorder/{idcompany}/{idarea}/{order}/{sum}")]
    public async Task<string> AreaOrder(string idcompany, string idarea, long order, bool sum)
    {
      return await Task.Run(() => service.AreaOrder(idcompany, idarea, order, sum));
    }
    /// <summary>
    /// Exclusão da area
    /// </summary>
    /// <param name="id">Identificador da área</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletearea/{id}")]
    public async Task<string> DeleteArea(string id)
    {
      return await Task.Run(() => service.DeleteArea(id));
    }
    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatearea")]
    public async Task<string> UpdateArea([FromBody]ViewCrudArea view)
    {
      return await Task.Run(() => service.UpdateArea(view));
    }
    #endregion

    #region Axis
    /// <summary>
    /// Lista Eixos
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getaxis")]
    public async Task<List<ViewListAxis>> GetAxis()
    {
      return await Task.Run(() => service.GetAxis());
    }
    /// <summary>
    /// Lista eixos filtrando pela empresa
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getaxis/{idcompany}")]
    public async Task<List<ViewListAxis>> GetAxis(string idcompany)
    {
      return await Task.Run(() => service.GetAxis(idcompany));
    }
    /// <summary>
    /// Busca informações para editar Area
    /// </summary>
    /// <param name="id">Identificador da area</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getaxisbyid/{id}")]
    public async Task<ViewCrudAxis> GetAxisById(string id)
    {
      return await Task.Run(() => service.GetAxisById(id));
    }
    /// <summary> eixo
    /// Inclusão
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addaxis")]
    public async Task<string> AddAxis([FromBody]ViewCrudAxis view)
    {
      return await Task.Run(() => service.AddAxis(view));
    }
    /// <summary>
    /// Exclusão de eixo
    /// </summary>
    /// <param name="idaxis">Identificador do eixo</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteaxis/{idaxis}")]
    public async Task<string> DeleteAxis(string idaxis)
    {
      return await Task.Run(() => service.DeleteAxis(idaxis));
    }
    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateaxis")]
    public async Task<string> UpdateAxis([FromBody]ViewCrudAxis view)
    {
      return await Task.Run(() => service.UpdateAxis(view));
    }
    #endregion

    #region Cbo
    /// <summary>
    /// Lista Cbo's
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listCbo")]
    public async Task<List<ViewListCbo>> ListCbo()
    {
      return await Task.Run(() => service.ListCbo());
    }
    /// <summary>
    /// Busca informações de Cbo para editar
    /// </summary>
    /// <param name="id">Identificador do Cbo</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getCbo/{id}")]
    public async Task<ViewCrudCbo> GetCbo(string id)
    {
      return await Task.Run(() => service.GetCbo(id));
    }
    /// <summary>
    /// Inclusão Cbo
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addCbo")]
    public async Task<string> AddCbo([FromBody]ViewCrudCbo view)
    {
      return await Task.Run(() => service.AddCbo(view));
    }
    /// <summary>
    /// Exclusão de Cbo
    /// </summary>
    /// <param name="id">Identificador do Cbo</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteCbo/{id}")]
    public async Task<string> DeleteCbo(string id)
    {
      return await Task.Run(() => service.DeleteCbo(id));
    }
    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateCbo")]
    public async Task<string> UpdateCbo([FromBody]ViewCrudCbo view)
    {
      return await Task.Run(() => service.UpdateCbo(view));
    }
    #endregion

    #region Company
    /// <summary>
    /// Lista as empresas
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getcompanies")]
    public async Task<List<ViewListCompany>> GetCompanies()
    {
      return await Task.Run(() => service.GetCompanies());
    }
    #endregion

    #region Course
    /// <summary>
    /// Lista cursos por cargos
    /// </summary>
    /// <param name="idoccupation">Identificador cargo</param>
    /// <param name="type">Tipo de treinamento obrigatório</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getcourseoccupation/{idoccupation}/{type}")]
    public async Task<List<ViewListCourse>> GetCourseOccupation(string idoccupation, EnumTypeMandatoryTraining type)
    {
      return await Task.Run(() => service.GetCourseOccupation(idoccupation, type));
    }
    #endregion

    #region Essencials
    /// <summary>
    /// Lista as skills da empresa
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getessential/{idcompany}")]
    public async Task<List<ViewListSkill>> GetEssential(string idcompany)
    {
      return await Task.Run(() => service.GetEssential(idcompany));
    }
    /// <summary>
    /// Inclusao skill essencial
    /// </summary>
    /// <param name="view">Objeto CRUD</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addessential")]
    public async Task<string> AddEssential([FromBody]ViewCrudEssential view)
    {
      return await Task.Run(() => service.AddEssential(view));
    }
    /// <summary>
    /// Exclusão de competência essencial
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="id">Identificador da competÊncia</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteessential/{idcompany}/{id}")]
    public async Task<string> DeleteEssential(string idcompany, string id)
    {
      return await Task.Run(() => service.DeleteEssential(idcompany, id));
    }
    #endregion

    #region Group
    /// <summary>
    /// Lista escopos do grupo
    /// </summary>
    /// <param name="idgroup">Identificador grupo</param>
    /// /// <param name="idscope">Identificador escopo</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getmapgroupscope/{idgroup}/{idscope}")]
    public async Task<ViewCrudMapGroupScope> GetMapGroupScopeById(string idgroup, string idscope)
    {
      return await Task.Run(() => service.GetMapGroupScopeById(idgroup, idscope));
    }
    /// <summary>
    /// Busca informações grupo para editar
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getgroup/{id}")]
    public async Task<ViewCrudGroup> GetGroup(string id)
    {
      return await Task.Run(() => service.GetGroup(id));
    }
    /// <summary>
    /// Lista grupos
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getgroups")]
    public async Task<List<ViewListGroup>> GetGroups()
    {
      return await Task.Run(() => service.GetGroups());
    }
    /// <summary>
    /// Busca informações para editar mapa do grupo
    /// </summary>
    /// <param name="id">Identificador grupo</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getmapgroup/{id}")]
    public async Task<ViewMapGroup> GetMapGroup(string id)
    {
      return await Task.Run(() => service.GetMapGroup(id));
    }
    /// <summary>
    /// Lista grupos filtrando pela empresa
    /// </summary>
    /// <param name="idcompany">Identificador empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getgroups/{idcompany}")]
    public async Task<List<ViewGroupListLO>> GetGroups(string idcompany)
    {
      return await Task.Run(() => service.GetGroups(idcompany));
    }
    /// <summary>
    /// Lista cargos para visualização impressão
    /// </summary>
    /// <param name="idcompany">Identificador empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getgroupsprint/{idcompany}")]
    public async Task<List<ViewListGroup>> GetGroupsPrint(string idcompany)
    {
      return await Task.Run(() => service.GetGroupsPrint(idcompany));
    }
    /// <summary>
    /// Reordenar o Scopo do grupo
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="idgroup">Identificador do grupo</param>
    /// <param name="idscope">Identificador do Scopo</param>
    /// <param name="sum">Sentido da ordem p/cima ou p/baixo</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("reordergroupscope/{idcompany}/{idgroup}/{idscope}/{sum}")]
    public async Task<string> ReorderGroupScope(string idcompany, string idgroup, string idscope, bool sum)
    {
      return await Task.Run(() => service.ReorderGroupScope(idcompany, idgroup, idscope, sum));
    }
    /// <summary>
    /// Reorganizar o escopo do grupo de maneira manual
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="idgroup">Identificador do grupo</param>
    /// <param name="idscope">Identificador do escopo</param>
    /// <param name="order">Ordem do escopo</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("reordergroupscopemanual/{idcompany}/{idgroup}/{idscope}/{order}")]
    public async Task<string> ReorderGroupScopeManual(string idcompany, string idgroup, string idscope, long order)
    {
      return await Task.Run(() => service.ReorderGroupScopeManual(idcompany, idgroup, idscope, order));
    }
    /// <summary>
    /// Inclusão
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addmapgroupschooling")]
    public async Task<string> AddMapGroupSchooling([FromBody]ViewCrudMapGroupSchooling view)
    {
      return await Task.Run(() => service.AddMapGroupSchooling(view));
    }
    /// <summary>
    /// Inclusao grupo
    /// </summary>
    /// <param name="view">Objeto CRUD</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addgroup")]
    public async Task<string> AddGroup([FromBody]ViewCrudGroup view)
    {
      service.AddGroup(view);
      return await Task.Run(() => "OK");
    }
    /// <summary>
    /// Incluir escopo no mapa do grupo
    /// </summary>
    /// <param name="view">Objeto de manutenção do grupo</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addmapgroupscope")]
    public async Task<string> AddMapGroupScope([FromBody]ViewCrudMapGroupScope view)
    {
      return await Task.Run(() => service.AddMapGroupScope(view));
    }
    /// <summary>
    /// Inclusao skill no grupo
    /// </summary>
    /// <param name="view">Objeto CRUD</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addmapgroupskill")]
    public async Task<string> AddMapGroupSkill([FromBody]ViewCrudMapGroupSkill view)
    {
      return await Task.Run(() => service.AddMapGroupSkill(view));
    }
    /// <summary>
    /// Exclusão de grupo
    /// </summary>
    /// <param name="id">Identificador do grupo</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletegroup/{id}")]
    public async Task<string> DeleteGroup(string id)
    {
      return await Task.Run(() => service.DeleteGroup(id));
    }
    /// <summary>
    /// Exclusão de escolaridade do mapa do grupo
    /// </summary>
    /// <param name="idgroup">Identificador do grupo</param>
    /// <param name="id">Identificador da escolaridade</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletemapgroupschooling/{idgroup}/{id}")]
    public async Task<string> DeleteMapGroupSchooling(string idgroup, string id)
    {
      return await Task.Run(() => service.DeleteMapGroupSchooling(idgroup, id));
    }
    /// <summary>
    /// Exclusão de competência do mapa do grupo
    /// </summary>
    /// <param name="idgroup">Identificador do grupo</param>
    /// <param name="id">Identificador da competência</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletemapgroupskill/{idgroup}/{id}")]
    public async Task<string> DeleteMapGroupSkill(string idgroup, string id)
    {
      return await Task.Run(() => service.DeleteMapGroupSkill(idgroup, id));
    }
    /// <summary>
    /// Exclusão de escopo do mapa do grupo
    /// </summary>
    /// <param name="idgroup">Identificador do grupo</param>
    /// <param name="scope">Identificador do escopo</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletemapgroupscope/{idgroup}/{scope}")]
    public async Task<string> DeleteMapGroupScope(string idgroup, string scope)
    {
      return await Task.Run(() => service.DeleteMapGroupScope(idgroup, scope));
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
    public async Task<string> UpdateMapGroupScope([FromBody]ViewCrudScope scope, string idgroup)
    {
      return await Task.Run(() => service.UpdateMapGroupScope(idgroup, scope));
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
    public async Task<string> UpdateMapGroupSchooling([FromBody]ViewCrudSchooling schooling, string idgroup)
    {
      return await Task.Run(() => service.UpdateMapGroupSchooling(idgroup, schooling));
    }
    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updategroup")]
    public async Task<string> UpdateGroup([FromBody]ViewCrudGroup view)
    {
      return await Task.Run(() => service.UpdateGroup(view));
    }
    /// <summary>
    /// Exportar o mapa de grupos de cargo em arquivo CSV
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getcsvcomparegroup/{idcompany}")]
    public async Task<string> GetCSVCompareGroup(string idcompany)
    {
      return await Task.Run(() => service.GetCSVCompareGroup(idcompany, XmlConnection.ReadVariablesSystem().BlobKey));
    }
    #endregion

    #region Occupation

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idarea"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getlineopportunity/{idarea}")]
    public async Task<List<ViewListInfraSphere>> GetLineOpportunity(string idarea,int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetLineOpportunity(idarea, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Retorna o mapa do cargo
    /// </summary>
    /// <param name="id">Identificador do mapa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getmapoccupation/{id}")]
    public async Task<ViewMapOccupation> GetMapOccupation(string id)
    {
      return await Task.Run(() => service.GetMapOccupation(id));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idcompany"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listopportunityline/{idcompany}")]
    public async Task<List<ViewListOpportunityLine>> ListOpportunityLine(string idcompany)
    {
      return await Task.Run(() => service.ListOpportunityLine(idcompany));
    }

    /// <summary>
    /// Busca informações de cargos para editar
    /// </summary>
    /// <param name="id">Identificador cargo</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getoccupation/{id}")]
    public async Task<ViewCrudOccupation> GetOccupation(string id)
    {
      return await Task.Run(() => service.GetOccupation(id));
    }
    /// <summary>
    /// Lista cargos
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getoccupations")]
    public async Task<List<ViewListOccupationView>> GetOccupations()
    {
      return await Task.Run(() => service.GetOccupations());
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
    public async Task<List<ViewOccupationListEdit>> ListOccupationEdit(string idcompany, string idarea, int count = 10, int page = 1, string filter = "", string filterGroup = "")
    {
      long total = 0;
      var result = service.ListOccupationsEdit(idcompany, idarea, ref total, filter, count, page, filterGroup);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Lista cargos com filtro de empresa e a area para editar
    /// </summary>
    /// <param name="idcompany">Identificador empresa</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <param name="filterGroup"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listoccupationedit/{idcompany}")]
    public async Task<List<ViewOccupationListEdit>> ListOccupationEdit(string idcompany, int count = 10, int page = 1, string filter = "", string filterGroup = "")
    {
      long total = 0;
      var result = service.ListOccupationsEdit(idcompany, ref total, filter, count, page, filterGroup);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
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
    public async Task<List<ViewGetOccupation>> GetOccupations(string idcompany, string idarea)
    {
      return await Task.Run(() => service.GetOccupations(idcompany, idarea));
    }
    /// <summary>
    /// Reorganizar as atividades do cargo
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="idoccupation">Identificador do cargo</param>
    /// <param name="idactivitie">Identificador da atividade</param>
    /// <param name="sum">Sentido da ordem p/cima ou p/baixo</param>
    /// <returns>Mensagem de Sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("reorderoccupationactivitie/{idcompany}/{idoccupation}/{idactivitie}/{sum}")]
    public async Task<string> ReorderOccupationActivitie(string idcompany, string idoccupation, string idactivitie, bool sum)
    {
      return await Task.Run(() => service.ReorderOccupationActivitie(idcompany, idoccupation, idactivitie, sum));
    }
    /// <summary>
    /// Reorganizar as atividades do cargo manualmente
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="idoccupation">Identificador do cargo</param>
    /// <param name="idactivitie">Identificador da atividade</param>
    /// <param name="order">Ordem da atividade</param>
    /// <returns>Mensagem de Sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("reorderoccupationactivitiemanual/{idcompany}/{idoccupation}/{idactivitie}/{order}")]
    public async Task<string> ReorderOccupationActivitieManual(string idcompany, string idoccupation, string idactivitie, long order)
    {
      return await Task.Run(() => service.ReorderOccupationActivitieManual(idcompany, idoccupation, idactivitie, order));
    }
    /// <summary>
    /// Inclui os requisitos no cargo
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <param name="idoccupation">Identificador do cargo</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addspecificrequirements/{idoccupation}")]
    public async Task<string> AddSpecificRequirements([FromBody]ViewCrudSpecificRequirements view, string idoccupation)
    {
      return await Task.Run(() => service.AddSpecificRequirements(idoccupation, view));
    }
    /// <summary>
    /// Inclusão
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addoccupationactivities")]
    public async Task<string> AddOccupationActivities([FromBody]ViewCrudOccupationActivities view)
    {
      return await Task.Run(() => service.AddOccupationActivities(view));
    }
    /// <summary>
    /// Inclusao cargo
    /// </summary>
    /// <param name="view">Objeto CRUD</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addoccupation")]
    public async Task<string> AddOccupation([FromBody]ViewCrudOccupation view)
    {
      return await Task.Run(() => service.AddOccupation(view));
    }
    /// <summary>
    /// Inclusão de entregas em lote
    /// </summary>
    /// <param name="list">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addoccupationactivitieslist")]
    public async Task<string> AddOccupationActivitiesList([FromBody]List<ViewCrudOccupationActivities> list)
    {
      return await Task.Run(() => service.AddOccupationActivitiesList(list));
    }
    /// <summary>
    /// Inclusão de skill no occupation
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addoccupationskill")]
    public async Task<string> AddOccupationSkill([FromBody]ViewCrudOccupationSkill view)
    {
      return await Task.Run(() => service.AddOccupationSkill(view));
    }
    /// <summary>
    /// Exclusão de cargo
    /// </summary>
    /// <param name="id">Identificador do cargo</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteoccupation/{id}")]
    public async Task<string> DeleteOccupation(string id)
    {
      return await Task.Run(() => service.DeleteOccupation(id));
    }
    /// <summary>
    /// Exclusão de atividade do mapa de cargo
    /// </summary>
    /// <param name="idoccupation">Identificador do cargo</param>
    /// <param name="idactivitie">Identificador da atividade</param>
    /// <returns>Mensagem de retorno</returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteoccupationactivities/{idoccupation}/{idactivitie}")]
    public async Task<string> DeleteOccupationActivities(string idoccupation, string idactivitie)
    {
      return await Task.Run(() => service.DeleteOccupationActivities(idoccupation, idactivitie));
    }
    /// <summary>
    /// Exclusão de competência do mapa do cargo
    /// </summary>
    /// <param name="idoccupation">Identificador do cargo</param>
    /// <param name="id">Identificador da competência</param>
    /// <returns>Mensagem de retorno</returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteoccupationskill/{idoccupation}/{id}")]
    public async Task<string> DeleteOccupationSkill(string idoccupation, string id)
    {
      return await Task.Run(() => service.DeleteOccupationSkill(idoccupation, id));
    }
    /// <summary>
    /// Alteração da escolaridade do mapa do cargo
    /// </summary>
    /// <param name="schooling">Objeto de manutenção da escolaridade</param>
    /// <param name="idoccupation">Identificador do cargo</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updatemapoccupationschooling/{idoccupation}")]
    public async Task<string> UpdateMapOccupationSchooling([FromBody]ViewCrudSchooling schooling, string idoccupation)
    {
      return await Task.Run(() => service.UpdateMapOccupationSchooling(idoccupation, schooling));
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
    public async Task<string> UpdateMapOccupationActivities([FromBody]ViewCrudActivities activitie, string idoccupation)
    {
      return await Task.Run(() => service.UpdateMapOccupationActivities(idoccupation, activitie));
    }
    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateoccupation")]
    public async Task<string> UpdateOccupation([FromBody]ViewCrudOccupation view)
    {
      return await Task.Run(() => service.UpdateOccupation(view));
    }
    #endregion

    #region Process Level One
    /// <summary>
    /// Lista os processos nivel 1 filtrando pela area
    /// </summary>
    /// <param name="idarea">Identificador da area</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getlistprocessleveonebyarea/{idarea}")]
    public async Task<List<ViewListProcessLevelOneByArea>> GetListProcessLevelOneByArea(string idarea)
    {
      return await Task.Run(() => service.GetListProcessLevelOneByArea(idarea));
    }
    /// <summary>
    /// Inclusão processo nivel 1
    /// </summary>
    /// <param name="processLevelOne">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addprocesslevelone")]
    public async Task<string> AddProcessLevelOne([FromBody]ViewCrudProcessLevelOne processLevelOne)
    {
      service.AddProcessLevelOne(processLevelOne);
      return await Task.Run(() => "ok");
    }
    /// <summary>
    /// Exclusão de processo nivel um
    /// </summary>
    /// <param name="id">Identificador do processo</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteprocesslevelone/{id}")]
    public async Task<string> DeleteProcessLevelOne(string id)
    {
      return await Task.Run(() => service.DeleteProcessLevelOne(id));
    }
    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateprocesslevelone")]
    public async Task<string> UpdateProcessLevelOne([FromBody]ViewCrudProcessLevelOne view)
    {
      return await Task.Run(() => service.UpdateProcessLevelOne(view));
    }

    /// <summary>
    /// Retorna o processo nivel 2
    /// </summary>
    /// <param name="id">Identificador do processo nivel 1</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getlistprocesslevelonebyid/{id}")]
    public async Task<ViewCrudProcessLevelOne> GetListProcessLevelOneById(string id)
    {
      return await Task.Run(() => service.GetListProcessLevelOneById(id));
    }

    #endregion

    #region Process Level Two
    /// <summary>
    /// Retorna o processo nivel 2
    /// </summary>
    /// <param name="id">Identificador do processo nivel 2</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getlistprocessleveltwobyid/{id}")]
    public async Task<ViewCrudProcessLevelTwo> GetListProcessLevelTwoById(string id)
    {
      return await Task.Run(() => service.GetListProcessLevelTwoById(id));
    }

    /// <summary>
    /// Listas os processos nivel 2 filtrando pela area
    /// </summary>
    /// <param name="idarea">Identificador da Area</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getprocessleveltwo/{idarea}")]
    public async Task<List<ViewListProcessLevelTwo>> GetProcessLevelTwo(string idarea)
    {
      return await Task.Run(() => service.GetProcessLevelTwoFilter(idarea));
    }
    /// <summary>
    /// Lista os processos nivel 2
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getprocessleveltwo")]
    public async Task<List<ViewListProcessLevelTwo>> GetProcessLevelTwo()
    {
      return await Task.Run(() => service.GetProcessLevelTwo());
    }
    /// <summary>
    /// Inclusão processo nivel 2
    /// </summary>
    /// <param name="processLevelTwo">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addprocessleveltwo")]
    public async Task<string> AddProcessLevelTwo([FromBody]ViewCrudProcessLevelTwo processLevelTwo)
    {
      service.AddProcessLevelTwo(processLevelTwo);
      return await Task.Run(() => "ok");
    }
    /// <summary>
    /// Exclusão de processo nivel dois
    /// </summary>
    /// <param name="id">Identificador do processo nivel dois</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteprocessleveltwo/{id}")]
    public async Task<string> DeleteProcessLevelTwo(string id)
    {
      return await Task.Run(() => service.DeleteProcessLevelTwo(id));
    }
    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateprocessleveltwo")]
    public async Task<string> UpdateProcessLevelTwo([FromBody]ViewCrudProcessLevelTwo view)
    {
      return await Task.Run(() => service.UpdateProcessLevelTwo(view));
    }
    #endregion

    #region Questions
    /// <summary>
    /// Lista perguntas filtrando pela empresa
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listquestions/{idcompany}")]
    public async Task<List<ViewListQuestions>> GetQuestions(string idcompany)
    {
      long total = 0;
      var list = await Task.Run(() => service.ListQuestions(ref total, idcompany));
      Response.Headers.Add("x-total-count", total.ToString());
      return list;
    }
    /// <summary>
    /// Busca informações da pergunta para editar
    /// </summary>
    /// <param name="id">Identificador da pergunta</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getquestions/{id}")]
    public async Task<ViewCrudQuestions> GetQuestionsId(string id)
    {
      return await Task.Run(() => service.GetQuestions(id));
    }
    /// <summary>
    /// Inclusão
    /// </summary>
    /// <param name="idcompany">id da empresa</param>
    /// <param name="questions">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addquestions/{idcompany}")]
    public async Task<string> AddQuestions([FromBody]ViewCrudQuestions questions, string idcompany)
    {
      service.AddQuestions(questions, idcompany);
      return await Task.Run(() => "ok");
    }
    /// <summary>
    /// Exclusão de pergunta
    /// </summary>
    /// <param name="id">Identificador da pergunta</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletequestion/{id}")]
    public async Task<string> DeleteQuestion(string id)
    {
      return await Task.Run(() => service.DeleteQuestion(id));
    }
    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatequestions")]
    public async Task<string> UpdateQuestions([FromBody]ViewCrudQuestions view)
    {
      return await Task.Run(() => service.UpdateQuestions(view));
    }
    #endregion

    #region Schooling
    /// <summary>
    /// Lista escolaridades
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getschooling")]
    public async Task<List<ViewListSchooling>> GetSchooling()
    {
      return await Task.Run(() => service.GetSchooling());
    }
    /// <summary>
    /// Busca informações para editar Escolaridade
    /// </summary>
    /// <param name="id">Identificador da escolaridade</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getschoolingbyid/{id}")]
    public async Task<ViewCrudSchooling> GetSchoolingById(string id)
    {
      return await Task.Run(() => service.GetSchoolingById(id));
    }
    /// <summary>
    /// Inclusão escolaridade
    /// </summary>
    /// <param name="schooling">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addschooling")]
    public async Task<string> AddSchooling([FromBody]ViewCrudSchooling schooling)
    {
      service.AddSchooling(schooling);
      return await Task.Run(() => "ok");
    }
    /// <summary>
    /// Exclusão de escolaridade
    /// </summary>
    /// <param name="idschooling">Identificador da escolaridade</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteschooling/{idschooling}")]
    public async Task<string> DeleteSchooling(string idschooling)
    {
      return await Task.Run(() => service.DeleteSchooling(idschooling));
    }
    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateschooling")]
    public async Task<string> UpdateSchooling([FromBody]ViewCrudSchooling view)
    {
      return await Task.Run(() => service.UpdateSchooling(view));
    }
    #endregion

    #region Skill
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
    public async Task<List<ViewListSkill>> GetSkills(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetSkills(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Inclusao de nova skill
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addskill")]
    public async Task<ViewCrudSkill> AddSkill([FromBody]ViewCrudSkill view)
    {
      return await Task.Run(() => service.AddSkill(view));
    }
    /// <summary>
    /// Busca informações para editar Skill
    /// </summary>
    /// <param name="id">Identificador</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getskillbyid/{id}")]
    public async Task<ViewCrudSkill> GetSkillById(string id)
    {
      return await Task.Run(() => service.GetSkillById(id));
    }
    /// <summary>
    /// Listar as competências da empresa
    /// </summary>
    /// <param name="company">Identificador da empresa</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getskills/{company}")]
    public async Task<List<ViewSkills>> GetSkills(string company, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetSkills(company, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Listar as competências do grupo de cargo
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="idgroup">Identificador do grupo</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getskills/{idcompany}/{idgroup}")]
    public async Task<List<ViewSkills>> GetSkills(string idcompany, string idgroup, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetSkillsGroup(idgroup, idcompany, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Listar as competências por escolha de filtros
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="idgroup">Identificador do grupo</param>
    /// <param name="idoccupation">Identificador do cargo</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getskills/{idcompany}/{idgroup}/{idoccupation}")]
    public async Task<List<ViewSkills>> GetSkills(string idcompany, string idgroup, string idoccupation, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetSkillsOccupation(idgroup, idcompany, idoccupation, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Exclusão de competência
    /// </summary>
    /// <param name="idskill">Identificador da competência</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteskill/{idskill}")]
    public async Task<string> DeleteSkill(string idskill)
    {
      return await Task.Run(() => service.DeleteSkill(idskill));
    }
    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateskill")]
    public async Task<string> UpdateSkill([FromBody]ViewCrudSkill view)
    {
      return await Task.Run(() => service.UpdateSkill(view));
    }
    /// <summary>
    /// Adicionar uma competência
    /// </summary>
    /// <param name="view">Objeto de manutenção</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addskills")]
    public async Task<string> AddSkills([FromBody]List<ViewCrudSkill> view)
    {
      service.AddSkills(view);
      return await Task.Run(() => "ok");
    }
    #endregion

    #region Sphere
    /// <summary>
    /// Lista de esferas
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getspheres")]
    public async Task<List<ViewListSphere>> GetSpheres()
    {
      return await Task.Run(() => service.GetSpheres());
    }
    /// <summary>
    /// Lista de esferas filtrando pela empresa
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getspheres/{idcompany}")]
    public async Task<List<ViewListSphere>> GetSpheres(string idcompany)
    {
      return await Task.Run(() => service.GetSpheres(idcompany));
    }
    /// <summary>
    /// Busca informações para editar Sphere
    /// </summary>
    /// <param name="id">Identificador</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getspherebyid/{id}")]
    public async Task<ViewCrudSphere> GetSphereById(string id)
    {
      return await Task.Run(() => service.GetSphereById(id));
    }
    /// <summary>
    /// Inclusão esfera
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addsphere")]
    public async Task<string> AddSphere([FromBody]ViewCrudSphere view)
    {
      return await Task.Run(() => service.AddSphere(view));
    }
    /// <summary>
    /// Exclusão de esfera
    /// </summary>
    /// <param name="idsphere">Identificador da esfera</param>
    /// <returns>Mensagem de retorno</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletesphere/{idsphere}")]
    public async Task<string> DeleteSphere(string idsphere)
    {
      return await Task.Run(() => service.DeleteSphere(idsphere));
    }
    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatesphere")]
    public async Task<string> UpdateSphere([FromBody]ViewCrudSphere view)
    {
      return await Task.Run(() => service.UpdateSphere(view));
    }
    #endregion

    #region TextDefault
    /// <summary>
    /// Busca informações
    /// </summary>
    /// <param name="idcompany">Identificador empresa</param>
    /// <param name="name">Nome texto</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("gettextdefault/{idcompany}/{name}")]
    public async Task<ViewCrudTextDefault> GetTextDefault(string idcompany, string name)
    {
      return await Task.Run(() => service.GetTextDefault(idcompany, name));
    }
    /// <summary>
    /// Busca informações para editar
    /// </summary>
    /// <param name="id">Identificador</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("gettextdefault/{id}")]
    public async Task<ViewCrudTextDefault> GetTextDefault(string id)
    {
      return await Task.Run(() => service.GetTextDefault(id));
    }
    /// <summary>
    /// Lista textos
    /// </summary>
    /// <param name="idcompany">Identificador empresa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listtextdefault/{idcompany}")]
    public async Task<List<ViewListTextDefault>> ListTextDefault(string idcompany)
    {
      return await Task.Run(() => service.ListTextDefault(idcompany));
    }
    /// <summary>
    /// Inclusao textos
    /// </summary>
    /// <param name="textDefault">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addtextdefault")]
    public async Task<string> AddTextDefault([FromBody]ViewCrudTextDefault textDefault)
    {
      service.AddTextDefault(textDefault);
      return await Task.Run(() => "ok");
    }
    /// <summary>
    /// Exclusão de texto padrão
    /// </summary>
    /// <param name="id">Identificador do texto padrão</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletetextdefault/{id}")]
    public async Task<string> DeleteTextDefault(string id)
    {
      return await Task.Run(() => service.DeleteTextDefault(id));
    }
    /// <summary>
    /// Atualização de dados 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatetextdefault")]
    public async Task<string> UpdateTextDefault([FromBody]ViewCrudTextDefault view)
    {
      return await Task.Run(() => service.UpdateTextDefault(view));
    }
    #endregion

    #endregion

  }
}