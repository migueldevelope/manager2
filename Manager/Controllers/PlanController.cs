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

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador de plano de desenvolvimento
  /// </summary>
  [Produces("application/json")]
  [Route("plan")]
  public class PlanController : Controller
  {
    private readonly IServicePlan service;

    #region Constructor
    /// <summary>
    /// Contrutor do plano de desenvolvimento
    /// </summary>
    /// <param name="_service">Serviço do plano de desenvolvimento</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public PlanController(IServicePlan _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Plan
    [Authorize]
    [HttpDelete]
    [Route("removestructplan/{idmonitoring}/{idplan}/{sourceplan}/{idstructplan}")]
    public string RemoveStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan)
    {
      return service.RemoveStructPlan(idmonitoring, idplan, sourceplan, idstructplan);
    }
    [Authorize]
    [HttpDelete]
    [Route("removeplanactivity/{id}")]
    public string RemovePlanActivity(string id)
    {
      return service.RemovePlanActivity(id);
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
    public List<ViewGetPlan> ListPlans(string id, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlans(ref total, id, filter, count, page, activities, skillcompany, schooling, open, expired, end, wait);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
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
    public List<ViewPlanShort> ListPlans(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlans(ref total, id, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
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
    public List<ViewPlanShort> ListPlansPerson(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlansPerson(ref total, id, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
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
    public List<ViewGetPlan> ListPlansPerson(string id, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlansPerson(ref total, id, filter, count, page, activities, skillcompany, schooling, open, expired, end, wait);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
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
    public ViewGetPlan GetPlan(string idmonitoring, string idplan)
    {
      return service.GetPlan(idmonitoring, idplan);
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
    public string UpdatePlan([FromBody]ViewCrudPlan plan, string idmonitoring)
    {
      return service.UpdatePlan(idmonitoring, plan);
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
    public string NewPlan([FromBody]ViewCrudPlan plan, string idmonitoring, string idplanold)
    {
      return service.NewPlan(idmonitoring, idplanold, plan);
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
    public string NewUpdatePlan([FromBody]List<ViewCrudNewPlanUp> plan, string idmonitoring)
    {
      return service.NewUpdatePlan(idmonitoring, plan);
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
    public List<ViewListPlanStruct> ListPlansStruct(byte activities, byte skillcompany, byte schooling, byte structplan, int count = 10, int page = 1)
    {
      long total = 0;
      var result = service.ListPlansStruct(ref total, "", count, page, activities, skillcompany, schooling, structplan);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
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
    public string NewStructPlan([FromBody] ViewCrudStructPlan structplan, string idmonitoring, string idplan, EnumSourcePlan sourceplan)
    {
      return service.NewStructPlan(idmonitoring, idplan, sourceplan, structplan);
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
    public ViewCrudStructPlan GetStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan)
    {
      return service.GetStructPlan(idmonitoring, idplan, sourceplan, idstructplan);
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
    public ViewListPlanStruct GetPlanStruct(string idmonitoring, string idplan)
    {
      return service.GetPlanStruct(idmonitoring, idplan);
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
    public string UpdateStructPlan([FromBody]ViewCrudStructPlan structplanedit, string idmonitoring, string idplan, EnumSourcePlan sourceplan)
    {
      return service.UpdateStructPlan(idmonitoring, idplan, sourceplan, structplanedit);
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
    public List<ViewPlanActivity> ListPlanActivity(string filter = "", int count = 10, int page = 1)
    {
      long total = 0;
      var result = service.ListPlanActivity(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Busca informações para editar planos de entrega
    /// </summary>
    /// <param name="id">Identificador plano</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getplanactivity/{id}")]
    public ViewPlanActivity GetPlanActivity(string id)
    {
      return service.GetPlanActivity(id);
    }
    /// <summary>
    /// Adiciona entrega
    /// </summary>
    /// <param name="model">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("newplanactivity")]
    public string NewPlanActivity([FromBody]ViewPlanActivity model)
    {
      return service.NewPlanActivity(model);
    }
    /// <summary>
    /// Atualiza informações de entrega
    /// </summary>
    /// <param name="model">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateplanactivity")]
    public string UpdatePlanActivity([FromBody]ViewPlanActivity model)
    {
      return service.UpdatePlanActivity(model);
    }
    #endregion

    #region Old

    [Authorize]
    [HttpGet]
    [Route("old/listplans/{id}/{activities}/{skillcompany}/{schooling}/{open}/{expired}/{end}/{wait}")]
    public List<ViewPlan> ListPlansOld(string id, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlansOld(ref total, id, filter, count, page, activities, skillcompany, schooling, open, expired, end, wait);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    //[Authorize]
    //[HttpGet]
    //[Route("old/listplans/{id}")]
    //public List<ViewPlanShort> ListPlansOld(string id, int count = 10, int page = 1, string filter = "")
    //{
    //  long total = 0;
    //  var result = service.ListPlansOld(ref total, id, filter, count, page);
    //  Response.Headers.Add("x-total-count", total.ToString());
    //  return result;
    //}

    [Authorize]
    [HttpGet]
    [Route("old/listplansperson/{id}")]
    public List<ViewPlanShort> ListPlansPersonOld(string id, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlansPersonOld(ref total, id, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/listplansperson/{id}/{activities}/{skillcompany}/{schooling}/{open}/{expired}/{end}/{wait}")]
    public List<ViewPlan> ListPlansPersonOld(string id, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPlansPersonOld(ref total, id, filter, count, page, activities, skillcompany, schooling, open, expired, end, wait);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/getplan/{idmonitoring}/{idplan}")]
    public ViewPlan GetPlanOld(string idmonitoring, string idplan)
    {
      return service.GetPlanOld(idmonitoring, idplan);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updateplan/{idmonitoring}")]
    public string UpdatePlanOld([FromBody]Plan plan, string idmonitoring)
    {
      return service.UpdatePlanOld(idmonitoring, plan);
    }

    [Authorize]
    [HttpPost]
    [Route("old/newplan/{idmonitoring}/{idplanold}")]
    public string NewPlanOld([FromBody]Plan plan, string idmonitoring, string idplanold)
    {
      return service.NewPlanOld(idmonitoring, idplanold, plan);
    }

    [Authorize]
    [HttpPut]
    [Route("old/newupdateplan/{idmonitoring}")]
    public string NewUpdatePlanOld([FromBody]List<ViewPlanNewUp> plan, string idmonitoring)
    {
      return service.NewUpdatePlanOld(idmonitoring, plan);
    }


    [Authorize]
    [HttpGet]
    [Route("old/listplansstruct/{activities}/{skillcompany}/{schooling}/{structplan}")]
    public List<ViewPlanStruct> ListPlansStructOld(byte activities, byte skillcompany, byte schooling, byte structplan, int count = 10, int page = 1)
    {
      long total = 0;
      var result = service.ListPlansStructOld(ref total, "", count, page, activities, skillcompany, schooling, structplan);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpPost]
    [Route("old/newstructplan/{idmonitoring}/{idplan}/{sourceplan}")]
    public string NewStructPlanOld([FromBody] StructPlan structplan, string idmonitoring, string idplan, EnumSourcePlan sourceplan)
    {
      return service.NewStructPlanOld(idmonitoring, idplan, sourceplan, structplan);
    }



    [Authorize]
    [HttpGet]
    [Route("old/getstructplan/{idmonitoring}/{idplan}/{sourceplan}/{idstructplan}")]
    public StructPlan GetStructPlanOld(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan)
    {
      return service.GetStructPlanOld(idmonitoring, idplan, sourceplan, idstructplan);
    }


    [Authorize]
    [HttpGet]
    [Route("old/getplanstruct/{idmonitoring}/{idplan}")]
    public ViewPlanStruct GetPlanStructOld(string idmonitoring, string idplan)
    {
      return service.GetPlanStructOld(idmonitoring, idplan);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updatestructplan/{idmonitoring}/{idplan}/{sourceplan}")]
    public string UpdateStructPlanOld([FromBody]StructPlan structplanedit, string idmonitoring, string idplan, EnumSourcePlan sourceplan)
    {
      return service.UpdateStructPlanOld(idmonitoring, idplan, sourceplan, structplanedit);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listplanactivity")]
    public List<PlanActivity> ListPlanActivityOld(string filter = "", int count = 10, int page = 1)
    {
      long total = 0;
      var result = service.ListPlanActivityOld(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpPost]
    [Route("old/newplanactivity")]
    public string NewPlanActivityOld([FromBody]PlanActivity model)
    {
      return service.NewPlanActivityOld(model);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updateplanactivity")]
    public string UpdatePlanActivityOld([FromBody]PlanActivity model)
    {
      return service.UpdatePlanActivityOld(model);
    }

    #endregion

  }
}