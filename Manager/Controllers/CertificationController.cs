using System.Collections.Generic;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador para acreditação
  /// </summary>
  [Produces("application/json")]
  [Route("certification")]
  public class CertificationController : Controller
  {
    private readonly IServiceCertification service;

    #region constructor
    /// <summary>
    /// Construtor do controle
    /// </summary>
    /// <param name="_service">Serviço da acreditação</param>
    /// <param name="contextAccessor">Token de autenticação</param>
    public CertificationController(IServiceCertification _service, IHttpContextAccessor contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region certification
    /// <summary>
    /// Retornar a lista de certificações pendentes
    /// </summary>
    /// <param name="idperson">Identificador da pessoa</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do usuário</param>
    /// <returns>Retorna a lista de pendências da pessoa</returns>
    [Authorize]
    [HttpGet]
    [Route("listcertificationswaitperson/{idperson}")]
    public List<ViewListCertificationPerson> ListCertificationsWaitPerson(string idperson, string filter = "", int count = 10, int page = 1)
    {
      long total = 0;
      var result = service.ListCertificationsWaitPerson(idperson, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Apaga uma acreditação
    /// </summary>
    /// <param name="idcertification">Identificador da acreditação</param>
    /// <returns>Mensagem de Sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("removecertification/{idcertification}")]
    public IActionResult DeleteCertification(string idcertification)
    {
      return Ok(service.DeleteCertification(idcertification));
    }
    /// <summary>
    /// Retira uma pessoa da acreditãção
    /// </summary>
    /// <param name="idcertification">Identificador da acreditação</param>
    /// <param name="idperson">Identificador da pessoa</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("removeperson/{idcertification}/{idperson}")]
    public string DeletePerson(string idcertification, string idperson)
    {
      return service.DeletePerson(idcertification, idperson);
    }
    /// <summary>
    /// Retornar as competências que podem ser acreditadas para uma pessoa
    /// </summary>
    /// <param name="idperson">Identificador da pessoa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getprofile/{idperson}")]
    public ViewListCertificationProfile GetProfile(string idperson)
    {
      return service.GetProfile(idperson);
    }
    /// <summary>
    /// Lista as acreditações pendentes para uma pessoa
    /// </summary>
    /// <param name="idperson">Identificador da pessoa</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do usuário</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listcertificationperson/{idperson}")]
    public List<ViewListCertificationItem> ListCertificationPerson(string idperson, string filter = "", int count = 10, int page = 1)
    {
      long total = 0;
      var result = service.ListCertificationPerson(idperson, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Atualizar a situação 
    /// </summary>
    /// <param name="certification"></param>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatestatuscertification/{idperson}")]
    public string UpdateStatusCertification([FromBody]ViewCrudCertificationPersonStatus certification, string idperson)
    {
      return service.UpdateStatusCertification(certification, idperson);
    }
    /// <summary>
    /// Inclusão pessoa para acreditação
    /// </summary>
    /// <param name="person">Objetivo Crud</param>
    /// <param name="idcertification">Identificador acreditação</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addperson/{idcertification}")]
    public string AddPerson([FromBody]ViewListPerson person, string idcertification)
    {
      return service.AddPerson(idcertification, person);
    }
    /// <summary>
    /// Acreditação de contrato 
    /// </summary>
    /// <param name="view">Objeto Crud</param>
    /// <param name="idcertificationperson">Identificador de contrato da acreditação</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("approvedcertification/{idcertificationperson}")]
    public string ApprovedCertification([FromBody]ViewCrudCertificationPerson view, string idcertificationperson)
    {
      return service.ApprovedCertification(idcertificationperson, view);
    }
    /// <summary>
    /// Lista acreditação para excluir
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getlistexclud")]
    public List<ViewListCertification> ListEnded(string filter = "", int count = 999999999, int page = 1)
    {
      long total = 0;
      return service.ListEnded(ref total, filter, count, page);
    }
    /// <summary>
    /// Busca informação de acreditção
    /// </summary>
    /// <param name="idcertification">Identificador Acreditação</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("certificationswaitperson/{idcertification}")]
    public ViewCrudCertification CertificationsWaitPerson(string idcertification)
    {
      return service.CertificationsWaitPerson(idcertification);
    }
    /// <summary>
    /// Lista contratos para adicionar na acreditçaão
    /// </summary>
    /// <param name="idcertification">Identificador da acreditação</param>
    /// <param name="filter"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns>Lista de colaboradores</returns>
    [Authorize]
    [HttpGet]
    [Route("listpersons/{idcertification}")]
    public List<ViewListPerson> ListPersons(string idcertification, string filter = "", int count = 999999999, int page = 1)
    {
      long total = 0;
      return service.ListPersons(idcertification, ref total, filter, count, page);
    }
    /// <summary>
    /// Inclusão de nova acreditação
    /// </summary>
    /// <param name="item">Objeto Crud</param>
    /// <param name="idperson">Identificador do contrato</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("newcertification/{idperson}")]
    public ViewCrudCertification NewCertification([FromBody]ViewListCertificationItem item, string idperson)
    {
      return service.NewCertification(item, idperson);
    }
    /// <summary>
    /// Atualiza informações da acreidtação
    /// </summary>
    /// <param name="certification">Objeto Crud</param>
    /// <param name="idperson">Identificador contrato</param>
    /// <param name="idcertification">Identificador acreditação</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatecertification/{idperson}/{idcertification}")]
    public string UpdateCertification([FromBody]ViewCrudCertification certification, string idperson, string idcertification)
    {
      return service.UpdateCertification(certification, idperson, idcertification);
    }
    #endregion
  
  }
}