using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [HttpGet]
    [Route("{idcompany}/career/{typeax}")]
    public List<ViewOccupationGroupCareer> GetCareer(string idcompany, EnumTypeAxis typeax, EnumTypeCareer type = EnumTypeCareer.X)
    {
      return service.GetViewOccupationGroupCareers(idcompany, typeax, type);
    }

    [Authorize]
    [HttpGet]
    [Route("{idcompany}/career")]
    public List<ViewCareer> GetCareerType(string idcompany)
    {
      return service.GetCareer(idcompany);
    }

    [Authorize]
    [HttpGet]
    [Route("listcareer")]
    public List<ViewCareer> GetCareerType()
    {
      return service.ListGetCareer();
    }

    [Authorize]
    [HttpGet]
    [Route("{idcompany}/occupationgroupcareer/{id}")]
    public List<ViewOccupationGroupCareer> GetOccupationGroupCareer(string idcompany, string id, EnumTypeCareer type = EnumTypeCareer.X)
    {
      return service.GetOccupationGroupCareersEdit(idcompany, id, type);
    }

    [Authorize]
    [HttpGet]
    [Route("{idcompany}/occupationgroupcareer")]
    public List<ViewOccupationGroupCareer> GetOccupationGroupCareerList(string idcompany, EnumTypeCareer type = EnumTypeCareer.X)
    {
      return service.GetOccupationGroupCareersList(idcompany, type);
    }

    [Authorize]
    [HttpGet]
    [Route("{idcompany}/occupationgroup")]
    public List<ViewOccupationGroupCareer> GetOccupationGroupList(string idcompany)
    {
      return service.GetOccupationGroupList(idcompany);
    }

    [Authorize]
    [HttpGet]
    [Route("occupationgroup/{id}")]
    public ViewOccupationGroupCareer GetOccupationGroupEdit(string id)
    {
      return service.GetOccupationGroupEdit(id);
    }

    [Authorize]
    [HttpGet]
    [Route("{idcompany}/occupation")]
    public List<ViewOccupationLine> GetOccupationList(string idcompany)
    {
      return service.GetOccupationList(idcompany);
    }

    [Authorize]
    [HttpGet]
    [Route("occupation/{id}")]
    public ViewOccupationLine GetOccupationEdit(string id)
    {
      return service.GetOccupationEdit(id);
    }

    [Authorize]
    [HttpGet]
    [Route("{idcompany}/maxposition")]
    public long GetMaxPosition(string idcompany, EnumTypeCareer type = EnumTypeCareer.X)
    {
      return service.GetMaxPosition(idcompany, type);
    }

    [Authorize]
    [HttpGet]
    [Route("{idcompany}/headsphere")]
    public List<ViewHeadInfraSphere> GetSphere(string idcompany, EnumTypeCareer type = EnumTypeCareer.X)
    {
      return service.GetHeadSphere(idcompany, type);
    }

    [Authorize]
    [HttpGet]
    [Route("{idcompany}/headaxis")]
    public List<ViewHeadInfraAxis> GetAxis(string idcompany, EnumTypeCareer type = EnumTypeCareer.X)
    {
      return service.GetHeadAxis(idcompany, type);
    }

    [Authorize]
    [HttpGet]
    [Route("{idcompany}/occupationline/{id}")]
    public ViewOccupationLine GetOccupationLine(string idcompany, string id)
    {
      return service.GetLinesOccuationEdit(idcompany, id);
    }

    [Authorize]
    [HttpGet]
    [Route("{idcompany}/occupationline")]
    public List<ViewOccupationLine> GetOccupationLine(string idcompany)
    {
      return service.GetLinesOccuation(idcompany);
    }

    [Authorize]
    [HttpGet]
    [Route("{idcompany}/listarea")]
    public List<ViewArea> GetArea(string idcompany)
    {
      return service.GetArea(idcompany);
    }


    [Authorize]
    [HttpGet]
    [Route("{idcompany}/listaxis")]
    public List<ViewAxis> GetAxis(string idcompany)
    {
      return service.GetAxis(idcompany);
    }

    [Authorize]
    [HttpGet]
    [Route("{idcompany}/listsphere")]
    public List<ViewSphere> GetSphere(string idcompany)
    {
      return service.GetSphere(idcompany);
    }

    [Authorize]
    [HttpGet]
    [Route("listdictionary")]
    public List<ViewLists> GetDictionary(EnumTypeSphere type)
    {
      return service.GetDictionary(type);
    }
    [Authorize]
    [HttpDelete]
    [Route("occupationgroupcareer/{id}")]
    public string DeleteOccupationGroupCareer(string id, EnumTypeCareer type = EnumTypeCareer.X)
    {
      return service.DeleteOccupationGroupCareer(id);
    }

    [Authorize]
    [HttpDelete]
    [Route("occupationgroupcareer/{id}/disconnect")]
    public string DisconnectOccupationGroupCareer(string id, EnumTypeCareer type = EnumTypeCareer.X)
    {
      return service.DisconnectOccupationGroupCareer(id, type);
    }

    [Authorize]
    [HttpDelete]
    [Route("occupation/{id}/disconnect")]
    public string DisconnectOccupation(string id)
    {
      return service.DisconnectOccupationLine(id);
    }


    [Authorize]
    [HttpPost]
    [Route("axis")]
    public string Post([FromBody]ViewAxisNew view)
    {
      service.NewAxis(view);
      return "ok";
    }

    [Authorize]
    [HttpPost]
    [Route("sphere")]
    public string Post([FromBody]ViewSphereNew view)
    {
      service.NewSphere(view);
      return "ok";
    }

    [Authorize]
    [HttpPost]
    [Route("dictionary")]
    public string Post([FromBody]ViewDictionarySphereNew view)
    {
      service.NewDictionary(view);
      return "ok";
    }

    [Authorize]
    [HttpPost]
    [Route("career")]
    public string Post([FromBody]ViewCareerNew view)
    {
      service.NewCareer(view);
      return "ok";
    }

    [Authorize]
    [HttpPost]
    [Route("occupationgroupcareer")]
    public string Post([FromBody]ViewOccupationGroupCareerNew view, EnumTypeCareer type = EnumTypeCareer.X)
    {
      service.NewOccupationGroupCareer(view, type);
      return "ok";
    }

    [Authorize]
    [HttpPost]
    [Route("area")]
    public string Post([FromBody]ViewAreaNew view)
    {
      service.NewArea(view);
      return "ok";
    }


    [Authorize]
    [HttpPut]
    [Route("axis/{id}")]
    public string Put([FromBody]ViewAxisNew view, string id)
    {
      service.UpdateAxis(view, id);
      return "ok";
    }

    [Authorize]
    [HttpPut]
    [Route("sphere/{id}")]
    public string Put([FromBody]ViewSphereNew view, string id)
    {
      service.UpdateSphere(view, id);
      return "ok";
    }

    [Authorize]
    [HttpPut]
    [Route("dictionary/{id}")]
    public string Put([FromBody]ViewDictionarySphereNew view, string id)
    {
      service.UpdateDictionary(view, id);
      return "ok";
    }

    [Authorize]
    [HttpPut]
    [Route("career/{id}")]
    public string Put([FromBody]ViewCareerNew view, string id)
    {
      service.UpdateCareer(view, id);
      return "ok";
    }


    [Authorize]
    [HttpPut]
    [Route("area/{id}")]
    public string Put([FromBody]ViewAreaNew view, string id)
    {
      service.UpdateArea(view, id);
      return "ok";
    }

    [Authorize]
    [HttpPut]
    [Route("occupationgroupcareer/{id}")]
    public string Put([FromBody]ViewOccupationGroupCareerNew view, string id, EnumTypeCareer type = EnumTypeCareer.X)
    {
      service.UpdateOccupationGroupCareer(view, type, id);
      return "ok";
    }

    [Authorize]
    [HttpPut]
    [Route("occupationgroupcareer/{id}/modify")]
    public string Put([FromBody]ViewCareerPosition view, string id, EnumTypeCareer type = EnumTypeCareer.X)
    {
      service.UpdatePosition(view, type, id);
      return "ok";
    }

    [Authorize]
    [HttpPut]
    [Route("occupationline/{id}/modify")]
    public string Put([FromBody]ViewCareerPosition view, string id)
    {
      service.UpdatePositionLine(view, id);
      return "ok";
    }

    [Authorize]
    [HttpPost]
    [Route("occupationline")]
    public string Post([FromBody]ViewOccupationLineNew view)
    {
      service.NewOccupationLine(view);
      return "ok";
    }

    [Authorize]
    [HttpPut]
    [Route("occupationline/{id}")]
    public string Put([FromBody]ViewOccupationLineNew view, string id)
    {
      service.UpdateOccupationLine(view, id);
      return "ok";
    }

    [Authorize]
    [HttpDelete]
    [Route("occupationline/{id}")]
    public string DeleteOccupationLine(string id)
    {
      return service.DeleteOccupationLine(id);
    }

    [Authorize]
    [HttpDelete]
    [Route("area/{id}")]
    public string DeleteArea(string id)
    {
      return service.DeleteArea(id);
    }

  }
}