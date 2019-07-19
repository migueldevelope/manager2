using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
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
  public class CertificationController : DefaultController
  {
    private readonly IServiceCertification service;

    #region constructor
    /// <summary>
    /// Construtor do controle
    /// </summary>
    /// <param name="_service">Serviço da acreditação</param>
    /// <param name="contextAccessor">Token de autenticação</param>
    public CertificationController(IServiceCertification _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
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
    public async Task<List<ViewListCertificationPerson>> ListCertificationsWaitPerson(string idperson, string filter = "", int count = 10, int page = 1)
    {
      long total = 0;
      var result = service.ListCertificationsWaitPerson(idperson, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }
    /// <summary>
    /// Apaga uma acreditação
    /// </summary>
    /// <param name="idcertification">Identificador da acreditação</param>
    /// <returns>Mensagem de Sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("removecertification/{idcertification}")]
    public async Task<IActionResult> DeleteCertification(string idcertification)
    {
      return await Task.Run(() =>Ok( service.DeleteCertification(idcertification)));
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
    public async Task<string> DeletePerson(string idcertification, string idperson)
    {
      return await Task.Run(() =>service.DeletePerson(idcertification, idperson));
    }
    /// <summary>
    /// Retornar as competências que podem ser acreditadas para uma pessoa
    /// </summary>
    /// <param name="idperson">Identificador da pessoa</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getprofile/{idperson}")]
    public async Task<ViewListCertificationProfile> GetProfile(string idperson)
    {
      return await Task.Run(() =>service.GetProfile(idperson));
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
    public async Task<List<ViewListCertificationItem>> ListCertificationPerson(string idperson, string filter = "", int count = 10, int page = 1)
    {
      long total = 0;
      var result = service.ListCertificationPerson(idperson, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
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
    public async Task<string> UpdateStatusCertification([FromBody]ViewCrudCertificationPersonStatus certification, string idperson)
    {
      return await Task.Run(() =>service.UpdateStatusCertification(certification, idperson));
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
    public async Task<string> AddPerson([FromBody]ViewListPersonBase person, string idcertification)
    {
      return await Task.Run(() =>service.AddPerson(idcertification, person));
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
    public async Task<string> ApprovedCertification([FromBody]ViewCrudCertificationPerson view, string idcertificationperson)
    {
      return await Task.Run(() =>service.ApprovedCertification(idcertificationperson, view));
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
    public async Task<List<ViewListCertification>> ListEnded(string filter = "", int count = 999999999, int page = 1)
    {
      long total = 0;
      return await Task.Run(() =>service.ListEnded(ref total, filter, count, page));
    }
    /// <summary>
    /// Busca informação de acreditção
    /// </summary>
    /// <param name="idcertification">Identificador Acreditação</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("certificationswaitperson/{idcertification}")]
    public async Task<ViewCrudCertification> CertificationsWaitPerson(string idcertification)
    {
      return await Task.Run(() =>service.CertificationsWaitPerson(idcertification));
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
    public async Task<List<ViewListPersonBase>> ListPersons(string idcertification, string filter = "", int count = 999999999, int page = 1)
    {
      long total = 0;
      return await Task.Run(() =>service.ListPersons(idcertification, ref total, filter, count, page));
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
    public async Task<ViewCrudCertification> NewCertification([FromBody]ViewListCertificationItem item, string idperson)
    {
      return await Task.Run(() =>service.NewCertification(item, idperson));
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
    public async Task<string> UpdateCertification([FromBody]ViewCrudCertification certification, string idperson, string idcertification)
    {
      return await Task.Run(() =>service.UpdateCertification(certification, idperson, idcertification));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("exportstatuscertification")]
    public async Task<List<ViewExportStatusCertification>> ExportStatusCertification()
    {
      return await Task.Run(() => service.ExportStatusCertification());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("exportstatuscertification/{idperson}")]
    public async Task<List<ViewExportStatusCertificationPerson>> ExportStatusCertification(string idperson)
    {
      return await Task.Run(() => service.ExportStatusCertification(idperson));
    }


    #endregion

  }
}