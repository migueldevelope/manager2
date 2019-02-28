using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("certification")]
  public class CertificationController : Controller
  {

    private readonly IServiceCertification service;

    public CertificationController(IServiceCertification _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [Authorize]
    [HttpPost]
    [Route("addperson/{idcertification}")]
    public string AddPerson([FromBody]BaseFields person, string idcertification)
    {
      return service.AddPerson(idcertification, person);
    }

    [Authorize]
    [HttpPut]
    [Route("approvedcertification/{idcertificationperson}")]
    public string ApprovedCertification([FromBody]CertificationPerson view, string idcertificationperson)
    {
      return service.ApprovedCertification(idcertificationperson, view);
    }

    [Authorize]
    [HttpGet]
    [Route("getlistexclud")]
    public List<Certification> GetListExclud(string filter = "", int count = 999999999, int page = 1)
    {
      long total = 0;
      return service.GetListExclud(ref total, filter, count, page);
    }

    [Authorize]
    [HttpGet]
    [Route("getprofile/{idperson}")]
    public ViewCertificationProfile GetProfile(string idperson)
    {
      return service.GetProfile(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("listcertificationswaitperson/{idperson}")]
    public List<ViewCertification> ListCertificationsWaitPerson(string idperson)
    {
      return service.ListCertificationsWaitPerson(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("certificationswaitperson/{idcertification}")]
    public Certification CertificationsWaitPerson(string idcertification)
    {
      return service.CertificationsWaitPerson(idcertification);
    }

    [Authorize]
    [HttpGet]
    [Route("listpersons/{idcertification}")]
    public List<BaseFields> ListPersons(string idcertification, string filter = "", int count = 999999999, int page = 1)
    {
      long total = 0;
      return service.ListPersons(idcertification, ref total, filter, count, page);
    }

    [Authorize]
    [HttpPost]
    [Route("newcertification/{idperson}")]
    public Certification NewCertification([FromBody]CertificationItem item, string idperson)
    {
      return service.NewCertification(item, idperson);
    }

    [Authorize]
    [HttpDelete]
    [Route("removecertification/{idcertification}")]
    public string RemoveCertification(string idcertification)
    {
      return service.RemoveCertification(idcertification);
    }

    [Authorize]
    [HttpDelete]
    [Route("removeperson/{idcertification}/{idcertificationperson}")]
    public string RemovePerson(string idcertification, string idcertificationperson)
    {
      return service.RemovePerson(idcertification, idcertificationperson);
    }


    [Authorize]
    [HttpPut]
    [Route("updatecertification/{idperson}")]
    public string UpdateCertification([FromBody]Certification certification, string idperson)
    {
      return service.UpdateCertification(certification, idperson);
    }
  }
}