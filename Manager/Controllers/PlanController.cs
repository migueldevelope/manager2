using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador de plano de desenvolvimento
  /// </summary>
  [Produces("application/json")]
  [Route("plan")]
  public class PlanController : DefaultController
  {
    private readonly IServicePlan service;

    #region Constructor
    /// <summary>
    /// Contrutor do plano de desenvolvimento
    /// </summary>
    /// <param name="_service">Serviço do plano de desenvolvimento</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public PlanController(IServicePlan _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Plan
    [Authorize]
    [HttpDelete]
    [Route("removestructplan/{idmonitoring}/{idplan}/{sourceplan}/{idstructplan}")]
    public async Task<string> RemoveStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan)
    {
      return await service.RemoveStructPlan(idmonitoring, idplan, sourceplan, idstructplan);
    }
    [Authorize]
    [HttpDelete]
    [Route("removeplanactivity/{id}")]
    public async Task<string> RemovePlanActivity(string id)
    {
      return await service.RemovePlanActivity(id);
    }
    /// <summary>
    /// Lista os planos para tela principal
    /// </summary>
    /// <param name="id"></param>
    /// <param name="activities"></param>
    /// <param name="skillcompany"></param>
    /// <param name="schooling"></param>
    /// <param name="open"></param>
    /// <param name="expired"></param>
    /// <param name="end"></param>
    /// <param name="wait"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listplans/{id}/{activities}/{skillcompany}/{schooling}/{open}/{expired}/{end}/{wait}")]
    public async Task<List<ViewGetPlan>> ListPlans(string id, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlans(id, ref total, filter, count, page, activities, skillcompany, schooling, open, expired, end, wait);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }
    /// <summary>
    /// Lista os planos filtrando pelo gestor
    /// </summary>
    /// <param name="id">Identificador do gestor</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listplans/{id}")]
    public async Task<List<ViewPlanShort>> ListPlans(string id,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlans(id, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }
    /// <summary>
    /// Lista os planos filtrando pelo contrato
    /// </summary>
    /// <param name="id">Identificador do contrato</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listplansperson/{id}")]
    public async Task<List<ViewPlanShort>> ListPlansPerson(string id,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlansPerson(id, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }
    /// <summary>
    /// Lista o monitoring para tela principal filtrando pelo contrato
    /// </summary>
    /// <param name="id">Identificador do contrato</param>
    /// <param name="activities"></param>
    /// <param name="skillcompany"></param>
    /// <param name="schooling"></param>
    /// <param name="open"></param>
    /// <param name="expired"></param>
    /// <param name="end"></param>
    /// <param name="wait"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listplansperson/{id}/{activities}/{skillcompany}/{schooling}/{open}/{expired}/{end}/{wait}")]
    public async Task<List<ViewGetPlan>> ListPlansPerson(string id, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlansPerson(id, ref total, filter,count, page, activities, skillcompany, schooling, open, expired, end, wait);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }
    /// <summary>
    /// Busca informações de plano para editar
    /// </summary>
    /// <param name="idmonitoring">Identificador do monitoring</param>
    /// <param name="idplan">Identificador do plano</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getplan/{idmonitoring}/{idplan}")]
    public async Task<ViewGetPlan> GetPlan(string idmonitoring, string idplan)
    {
      return await service.GetPlan(idmonitoring, idplan);
    }
    /// <summary>
    /// Atualiza informações do plano
    /// </summary>
    /// <param name="plan">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador do monitoring</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateplan/{idmonitoring}")]
    public async Task<string> UpdatePlan([FromBody]ViewCrudPlan plan, string idmonitoring)
    {
      return await service.UpdatePlan(idmonitoring, plan);
    }
    /// <summary>
    /// Inclusão de novo plano
    /// </summary>
    /// <param name="plan">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador Monitoring</param>
    /// <param name="idplanold">Identificador do plano</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("newplan/{idmonitoring}/{idplanold}")]
    public async Task<string> NewPlan([FromBody]ViewCrudPlan plan, string idmonitoring, string idplanold)
    {
      return await service.NewPlan(idmonitoring, idplanold, plan);
    }
    /// <summary>
    /// Inclusão de novo plano quando reprovado (não concorda) o plano anterior
    /// </summary>
    /// <param name="plan">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador do monitoring</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("newupdateplan/{idmonitoring}")]
    public async Task<string> NewUpdatePlan([FromBody]List<ViewCrudNewPlanUp> plan, string idmonitoring)
    {
      return await service.NewUpdatePlan(idmonitoring, plan);
    }
    /// <summary>
    /// Lista planos para curadoria
    /// </summary>
    /// <param name="activities"></param>
    /// <param name="skillcompany"></param>
    /// <param name="schooling"></param>
    /// <param name="structplan"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listplansstruct/{activities}/{skillcompany}/{schooling}/{structplan}")]
    public async Task<List<ViewListPlanStruct>> ListPlansStruct(byte activities, byte skillcompany, byte schooling, byte structplan, int count = 10, int page = 1)
    {
      long total = 0;
      var result = service.ListPlansStruct(ref total, "", count, page, activities, skillcompany, schooling, structplan);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }
    /// <summary>
    /// Inclusão da Curadoria
    /// </summary>
    /// <param name="structplan">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <param name="idplan">Identificador plano</param>
    /// <param name="sourceplan">Tipo de plano</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("newstructplan/{idmonitoring}/{idplan}/{sourceplan}")]
    public async Task<string> NewStructPlan([FromBody] ViewCrudStructPlan structplan, string idmonitoring, string idplan, EnumSourcePlan sourceplan)
    {
      return await service.NewStructPlan(idmonitoring, idplan, sourceplan, structplan);
    }
    /// <summary>
    /// Busca informações para editar curadoria
    /// </summary>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <param name="idplan">Identificador plano</param>
    /// <param name="sourceplan">Tipo de plano</param>
    /// <param name="idstructplan">Indetificador curadoria do plano</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getstructplan/{idmonitoring}/{idplan}/{sourceplan}/{idstructplan}")]
    public async Task<ViewCrudStructPlan> GetStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan)
    {
      return await service.GetStructPlan(idmonitoring, idplan, sourceplan, idstructplan);
    }
    /// <summary>
    /// Busca informações para editar plano para curadoria
    /// </summary>
    /// <param name="idmonitoring">Identificador do monitoring</param>
    /// <param name="idplan">Identificador do plano</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getplanstruct/{idmonitoring}/{idplan}")]
    public async Task<ViewListPlanStruct> GetPlanStruct(string idmonitoring, string idplan)
    {
      return await service.GetPlanStruct(idmonitoring, idplan);
    }
    /// <summary>
    /// Atualiza informações da curadoria
    /// </summary>
    /// <param name="structplanedit">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <param name="idplan">Identificador do Plano</param>
    /// <param name="sourceplan">Tipo do Plano</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatestructplan/{idmonitoring}/{idplan}/{sourceplan}")]
    public async Task<string> UpdateStructPlan([FromBody]ViewCrudStructPlan structplanedit, string idmonitoring, string idplan, EnumSourcePlan sourceplan)
    {
      return await service.UpdateStructPlan(idmonitoring, idplan, sourceplan, structplanedit);
    }
    /// <summary>
    /// Lista planos de entregas
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listplanactivity")]
    public async Task<List<ViewPlanActivity>> ListPlanActivity(string filter = "", int count = 10, int page = 1)
    {
      long total = 0;
      var result = service.ListPlanActivity(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }
    /// <summary>
    /// Busca informações para editar planos de entrega
    /// </summary>
    /// <param name="id">Identificador plano</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getplanactivity/{id}")]
    public async Task<ViewPlanActivity> GetPlanActivity(string id)
    {
      return await service.GetPlanActivity(id);
    }
    /// <summary>
    /// Adiciona entrega
    /// </summary>
    /// <param name="model">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("newplanactivity")]
    public async Task<string> NewPlanActivity([FromBody]ViewPlanActivity model)
    {
      return await service.NewPlanActivity(model);
    }
    /// <summary>
    /// Atualiza informações de entrega
    /// </summary>
    /// <param name="model">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateplanactivity")]
    public async Task<string> UpdatePlanActivity([FromBody]ViewPlanActivity model)
    {
      return await service.UpdatePlanActivity(model);
    }
    #endregion


  }
}