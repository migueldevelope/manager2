using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
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

    #region constructor
    public CertificationController(IServiceCertification _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion


    #region certification

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
    [HttpGet]
    [Route("getprofile/{idperson}")]
    public ViewCertificationProfile GetProfile(string idperson)
    {
      return service.GetProfile(idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("listcertificationswaitperson/{idperson}")]
    public List<ViewCertification> ListCertificationsWaitPerson(string idperson, string filter = "", int count = 10, int page = 1)
    {
      long total = 0;
      var result = service.ListCertificationsWaitPerson(idperson, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("listcertificationperson/{idperson}")]
    public List<ViewCertificationItem> ListCertificationPerson(string idperson, string filter = "", int count = 10, int page = 1)
    {
      long total = 0;
      var result = service.ListCertificationPerson(idperson, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpPut]
    [Route("updatestatuscertification/{idperson}")]
    public string UpdateStatusCertification([FromBody]ViewCertificationStatus certification, string idperson)
    {
      return service.UpdateStatusCertification(certification, idperson);
    }


    [Authorize]
    [HttpPost]
    [Route("addperson/{idcertification}")]
    public string AddPerson([FromBody]ViewListBasePerson person, string idcertification)
    {
      return service.AddPerson(idcertification, person);
    }

    [Authorize]
    [HttpPut]
    [Route("approvedcertification/{idcertificationperson}")]
    public string ApprovedCertification([FromBody]ViewCrudCertificationPerson view, string idcertificationperson)
    {
      return service.ApprovedCertification(idcertificationperson, view);
    }

    [Authorize]
    [HttpGet]
    [Route("getlistexclud")]
    public List<ViewListCertification> GetListExclud(string filter = "", int count = 999999999, int page = 1)
    {
      long total = 0;
      return service.GetListExclud(ref total, filter, count, page);
    }

    [Authorize]
    [HttpGet]
    [Route("certificationswaitperson/{idcertification}")]
    public ViewListCertification CertificationsWaitPerson(string idcertification)
    {
      return service.CertificationsWaitPerson(idcertification);
    }

    [Authorize]
    [HttpGet]
    [Route("listpersons/{idcertification}")]
    public List<ViewListBasePerson> ListPersons(string idcertification, string filter = "", int count = 999999999, int page = 1)
    {
      long total = 0;
      return service.ListPersons(idcertification, ref total, filter, count, page);
    }

    [Authorize]
    [HttpPost]
    [Route("newcertification/{idperson}")]
    public ViewCrudCertification NewCertification([FromBody]CertificationItem item, string idperson)
    {
      return service.NewCertification(item, idperson);
    }



    [Authorize]
    [HttpPut]
    [Route("updatecertification/{idperson}/{idmonitoring}")]
    public string UpdateCertification([FromBody]ViewCrudCertification certification, string idperson, string idmonitoring)
    {
      return service.UpdateCertification(certification, idperson, idmonitoring);
    }


    #endregion

    #region old
    [Authorize]
    [HttpPost]
    [Route("old/addperson/{idcertification}")]
    public string AddPersonOld([FromBody]BaseFields person, string idcertification)
    {
      return service.AddPersonOld(idcertification, person);
    }

    [Authorize]
    [HttpPut]
    [Route("old/approvedcertification/{idcertificationperson}")]
    public string ApprovedCertificationOld([FromBody]CertificationPerson view, string idcertificationperson)
    {
      return service.ApprovedCertificationOld(idcertificationperson, view);
    }

    [Authorize]
    [HttpGet]
    [Route("old/getlistexclud")]
    public List<Certification> GetListExcludOld(string filter = "", int count = 999999999, int page = 1)
    {
      long total = 0;
      return service.GetListExcludOld(ref total, filter, count, page);
    }

    [Authorize]
    [HttpGet]
    [Route("old/certificationswaitperson/{idcertification}")]
    public Certification CertificationsWaitPersonOld(string idcertification)
    {
      return service.CertificationsWaitPersonOld(idcertification);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listpersons/{idcertification}")]
    public List<BaseFields> ListPersonsOld(string idcertification, string filter = "", int count = 999999999, int page = 1)
    {
      long total = 0;
      return service.ListPersonsOld(idcertification, ref total, filter, count, page);
    }

    [Authorize]
    [HttpPost]
    [Route("old/newcertification/{idperson}")]
    public Certification NewCertificationOld([FromBody]CertificationItem item, string idperson)
    {
      return service.NewCertificationOld(item, idperson);
    }



    [Authorize]
    [HttpPut]
    [Route("old/updatecertification/{idperson}/{idmonitoring}")]
    public string UpdateCertificationOld([FromBody]Certification certification, string idperson, string idmonitoring)
    {
      return service.UpdateCertificationOld(certification, idperson, idmonitoring);
    }


    #endregion


  }
}