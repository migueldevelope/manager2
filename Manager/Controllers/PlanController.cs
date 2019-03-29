using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Tools;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("plan")]
  public class PlanController : Controller
  {
    private readonly IServicePlan service;

    #region Constructor
    public PlanController(IServicePlan _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion


    //#region Plan
    //[Authorize]
    //[HttpDelete]
    //[Route("removestructplan/{idmonitoring}/{idplan}/{sourceplan}/{idstructplan}")]
    //public string RemoveStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan)
    //{
    //  return service.RemoveStructPlan(idmonitoring, idplan, sourceplan, idstructplan);
    //}

    //[Authorize]
    //[HttpDelete]
    //[Route("removeplanactivity/{id}")]
    //public string RemovePlanActivity(string id)
    //{
    //  return service.RemovePlanActivity(id);
    //}


    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="id"></param>
    ///// <param name="activities"></param>
    ///// <param name="skillcompany"></param>
    ///// <param name="schooling"></param>
    ///// <param name="open"></param>
    ///// <param name="expired"></param>
    ///// <param name="end"></param>
    ///// <param name="wait"></param>
    ///// <param name="count"></param>
    ///// <param name="page"></param>
    ///// <param name="filter"></param>
    ///// <returns></returns>
    //[Authorize]
    //[HttpGet]
    //[Route("listplans/{id}/{activities}/{skillcompany}/{schooling}/{open}/{expired}/{end}/{wait}")]
    //public List<ViewPlan> ListPlans(string id, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait, int count = 10, int page = 1, string filter = "")
    //{
    //  long total = 0;
    //  var result = service.ListPlans(ref total, id, filter, count, page, activities, skillcompany, schooling, open, expired, end, wait);
    //  Response.Headers.Add("x-total-count", total.ToString());
    //  return result;
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("listplans/{id}")]
    //public List<ViewPlanShort> ListPlans(string id, int count = 10, int page = 1, string filter = "")
    //{
    //  long total = 0;
    //  var result = service.ListPlans(ref total, id, filter, count, page);
    //  Response.Headers.Add("x-total-count", total.ToString());
    //  return result;
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("listplansperson/{id}")]
    //public List<ViewPlanShort> ListPlansPerson(string id, int count = 10, int page = 1, string filter = "")
    //{
    //  long total = 0;
    //  var result = service.ListPlansPerson(ref total, id, filter, count, page);
    //  Response.Headers.Add("x-total-count", total.ToString());
    //  return result;
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("listplansperson/{id}/{activities}/{skillcompany}/{schooling}/{open}/{expired}/{end}/{wait}")]
    //public List<ViewPlan> ListPlansPerson(string id, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait, int count = 10, int page = 1, string filter = "")
    //{
    //  long total = 0;
    //  var result = service.ListPlansPerson(ref total, id, filter, count, page, activities, skillcompany, schooling, open, expired, end, wait);
    //  Response.Headers.Add("x-total-count", total.ToString());
    //  return result;
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("getplan/{idmonitoring}/{idplan}")]
    //public ViewPlan GetPlan(string idmonitoring, string idplan)
    //{
    //  return service.GetPlan(idmonitoring, idplan);
    //}

    //[Authorize]
    //[HttpPut]
    //[Route("updateplan/{idmonitoring}")]
    //public string UpdatePlan([FromBody]Plan plan, string idmonitoring)
    //{
    //  return service.UpdatePlan(idmonitoring, plan);
    //}

    //[Authorize]
    //[HttpPost]
    //[Route("newplan/{idmonitoring}/{idplanold}")]
    //public string NewPlan([FromBody]Plan plan, string idmonitoring, string idplanold)
    //{
    //  return service.NewPlan(idmonitoring, idplanold, plan);
    //}

    //[Authorize]
    //[HttpPut]
    //[Route("newupdateplan/{idmonitoring}")]
    //public string NewUpdatePlan([FromBody]List<ViewPlanNewUp> plan, string idmonitoring)
    //{
    //  return service.NewUpdatePlan(idmonitoring, plan);
    //}


    //[Authorize]
    //[HttpGet]
    //[Route("listplansstruct/{activities}/{skillcompany}/{schooling}/{structplan}")]
    //public List<ViewPlanStruct> ListPlansStruct(byte activities, byte skillcompany, byte schooling, byte structplan, int count = 10, int page = 1)
    //{
    //  long total = 0;
    //  var result = service.ListPlansStruct(ref total, "", count, page, activities, skillcompany, schooling, structplan);
    //  Response.Headers.Add("x-total-count", total.ToString());
    //  return result;
    //}

    //[Authorize]
    //[HttpPost]
    //[Route("newstructplan/{idmonitoring}/{idplan}/{sourceplan}")]
    //public string NewStructPlan([FromBody] StructPlan structplan, string idmonitoring, string idplan, EnumSourcePlan sourceplan)
    //{
    //  return service.NewStructPlan(idmonitoring, idplan, sourceplan, structplan);
    //}



    //[Authorize]
    //[HttpGet]
    //[Route("getstructplan/{idmonitoring}/{idplan}/{sourceplan}/{idstructplan}")]
    //public StructPlan GetStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan)
    //{
    //  return service.GetStructPlan(idmonitoring, idplan, sourceplan, idstructplan);
    //}


    //[Authorize]
    //[HttpGet]
    //[Route("getplanstruct/{idmonitoring}/{idplan}")]
    //public ViewPlanStruct GetPlanStruct(string idmonitoring, string idplan)
    //{
    //  return service.GetPlanStruct(idmonitoring, idplan);
    //}

    //[Authorize]
    //[HttpPut]
    //[Route("updatestructplan/{idmonitoring}/{idplan}/{sourceplan}")]
    //public string UpdateStructPlan([FromBody]StructPlan structplanedit, string idmonitoring, string idplan, EnumSourcePlan sourceplan)
    //{
    //  return service.UpdateStructPlan(idmonitoring, idplan, sourceplan, structplanedit);
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("listplanactivity")]
    //public List<PlanActivity> ListPlanActivity(string filter = "", int count = 10, int page = 1)
    //{
    //  long total = 0;
    //  var result = service.ListPlanActivity(ref total, filter, count, page);
    //  Response.Headers.Add("x-total-count", total.ToString());
    //  return result;
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("getplanactivity/{id}")]
    //public PlanActivity GetPlanActivity(string id)
    //{
    //  return service.GetPlanActivity(id);
    //}

    //[Authorize]
    //[HttpPost]
    //[Route("newplanactivity")]
    //public string NewPlanActivity([FromBody]PlanActivity model)
    //{
    //  return service.NewPlanActivity(model);
    //}

    //[Authorize]
    //[HttpPut]
    //[Route("updateplanactivity")]
    //public string UpdatePlanActivity([FromBody]PlanActivity model)
    //{
    //  return service.UpdatePlanActivity(model);
    //}
    //#endregion

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