using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador da meritocracia
  /// </summary>
  [Produces("application/json")]
  [Route("meritocracy")]
  public class MeritocracyController : DefaultController
  {
    private readonly IServiceMeritocracy service;

    #region Constructor
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">Serviço da meritocracia</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public MeritocracyController(IServiceMeritocracy _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Meritocracy
    /// <summary>
    /// Listar as meritocracias
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome da meritocracia</param>
    /// <returns>Lista de meritocracias cadastradas</returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public async Task<List<ViewListMeritocracy>> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListMeritocracy> result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Listar as meritocracias
    /// </summary>
    /// <returns>Lista de meritocracias cadastradas</returns>
    [Authorize]
    [HttpGet]
    [Route("listmeritocracyactivitie/{idmeritocracy}")]
    public async Task<List<ViewListMeritocracyActivitie>> ListMeritocracyActivitie(string idmeritocracy)
    {
      List<ViewListMeritocracyActivitie> result = service.ListMeritocracyActivitie(idmeritocracy);
      return await Task.Run(() => result);
    }


    /// <summary>
    /// Listar ponutação de meritocracias
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    ///  <param name="idmanager">Identificador gestor</param>
    /// <param name="filter">Filtro para o nome do ponutação de meritocracia</param>
    /// <returns>Lista de ponutação de meritocracias</returns>
    [Authorize]
    [HttpGet]
    [Route("listwaitmanager/{idmanager}")]
    public async Task<List<ViewListMeritocracy>> ListWaitManager(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListMeritocracy> result = service.ListWaitManager(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }


    /// <summary>
    /// Atualiza informações de avaliação das entregas
    /// </summary>
    /// <param name="idmeritocracy"></param>
    /// <param name="idactivitie"></param>
    /// <param name="mark"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateactivitiemark/{idmeritocracy}/{idactivitie}/{mark}")]
    public async Task<IActionResult> UpdateActivitieMark(string idmeritocracy, string idactivitie, byte mark)
    {
      return await Task.Run(() => Ok(service.UpdateActivitieMark(idmeritocracy, idactivitie, mark)));
    }

    /// <summary>
    /// Atualiza informações de avaliação das entregas
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idschooling"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateschooling/{id}/{idschooling}")]
    public async Task<IActionResult> UpdateSchooling(string id, string idschooling)
    {
      return await Task.Run(() => Ok(service.UpdateSchooling(id, idschooling)));
    }


    /// <summary>
    /// Cadastrar uma nova meritocracia
    /// </summary>
    /// <param name="idperson">Identificador da pessoa</param>
    /// <returns></returns>
    [HttpPost]
    [Route("new/{idperson}")]
    public async Task<IActionResult> Post(string idperson)
    {
      return await Task.Run(() => Ok(service.New(idperson)));
    }
    /// <summary>
    /// Retorar a meritocracia para manutenção
    /// </summary>
    /// <param name="id">Identificador da meritocracia</param>
    /// <returns>Objeto de manutenção da meritocracia</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudMeritocracy> Get(string id)
    {
      return await Task.Run(() => service.Get(id));
    }
    /// <summary>
    /// Alterar a meritocracia
    /// </summary>
    /// <param name="view">Objeto de manutenção da meritocracia</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update([FromBody]ViewCrudMeritocracy view)
    {
      return await Task.Run(() => Ok(service.Update(view)));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmeritocracy"></param>
    /// <param name="showperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateshow/{idmeritocracy}/{showperson}")]
    public async Task<IActionResult> UpdateShow(string idmeritocracy, bool showperson)
    {
      return await Task.Run(() => Ok(service.UpdateShow(idmeritocracy,showperson)));
    }

    /// <summary>
    /// Alterar a meritocracia
    /// </summary>
    /// <param name="id">Identificador da meritocracia</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("end/{id}")]
    public async Task<IActionResult> Update(string id)
    {
      return await Task.Run(() => Ok(service.End(id)));
    }

    /// <summary>
    /// Alterar a meritocracia
    /// </summary>
    /// <param name="view">Objeto de manutenção da meritocracia</param>
    /// <param name="id">Identificador da meritocracia</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updatecompanydate/{id}")]
    public async Task<IActionResult> UpdateCompanyDate([FromBody]ViewCrudMeritocracyDate view, string id)
    {
      return await Task.Run(() => Ok(service.UpdateCompanyDate(view, id)));
    }

    /// <summary>
    /// Alterar a meritocracia
    /// </summary>
    /// <param name="view">Objeto de manutenção da meritocracia</param>
    /// <param name="id">Identificador da meritocracia</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updateoccupationdate/{id}")]
    public async Task<IActionResult> UpdateOccupationDate([FromBody]ViewCrudMeritocracyDate view, string id)
    {
      return await Task.Run(() => Ok(service.UpdateOccupationDate(view, id)));
    }


    /// <summary>
    /// Excluir uma meritocracia
    /// </summary>
    /// <param name="id">Identificação da meritocracia</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      return await Task.Run(() => Ok(service.Delete(id)));
    }
    #endregion

    #region MeritocracyScore


    /// <summary>
    /// Listar ponutação de meritocracias
    /// </summary>
    /// <returns>Lista de ponutação de meritocracias</returns>
    [Authorize]
    [HttpGet]
    [Route("listmeritocracyscore")]
    public async Task<ViewListMeritocracyScore> ListMeritocracyScore()
    {
      ViewListMeritocracyScore result = service.ListMeritocracyScore();
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Novo ponutação de meritocracia
    /// </summary>
    /// <param name="view">Objeto de manutenção do ponutação de meritocracia</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("newmeritocracyscore")]
    public async Task<IActionResult> PostMeritocracyScore([FromBody]ViewCrudMeritocracyScore view)
    {
      return await Task.Run(() => Ok(service.NewMeritocracyScore(view)));
    }
    /// <summary>
    /// Buscar ponutação de meritocracia para manutenção
    /// </summary>
    /// <param name="id">Identificador do ponutação de meritocracia</param>
    /// <returns>Objeto de manutenção do ponutação de meritocracia</returns>
    [Authorize]
    [HttpGet]
    [Route("getmeritocracyscore/{id}")]
    public async Task<ViewCrudMeritocracyScore> ListMeritocracyScore(string id)
    {
      return await Task.Run(() => service.GetMeritocracyScore(id));
    }
    /// <summary>
    /// Alterar o ponutação de meritocracia
    /// </summary>
    /// <param name="view">Objeto de manutenção do ponutação de meritocracia</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updatemeritocracyscore")]
    public async Task<IActionResult> UpdateMeritocracyScore([FromBody]ViewCrudMeritocracyScore view)
    {
      return await Task.Run(() => Ok(service.UpdateMeritocracyScore(view)));
    }
    /// <summary>
    /// Deletar um ponutação de meritocracia
    /// </summary>
    /// <param name="id">Identificador do ponutação de meritocracia</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletemeritocracyscore/{id}")]
    public async Task<IActionResult> DeleteMeritocracyScore(string id)
    {
      return await Task.Run(() => Ok(service.RemoveMeritocracyScore(id)));
    }

   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="occupations"></param>
    /// <param name="idmanager"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("listmeritocracy/{idmanager}")]
    public async Task<List<ViewListMeritocracyResume>> ListMeritocracy([FromBody]List<_ViewList> occupations, string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListMeritocracy(idmanager, occupations, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listmeritocracyperson/{idperson}")]
    public async Task<List<ViewListMeritocracyResume>> ListMeritocracyPerson(string iduser, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListMeritocracyPerson(iduser, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="view"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("listmeritocracyrh")]
    public async Task<List<ViewListMeritocracyResume>> ListMeritocracyRH([FromBody]ViewFilterOccupationsAndManagers view, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListMeritocracyRH(view, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    #endregion

    #region SalaryScaleScore
    /// <summary>
    /// Listar ponutação de meritocracias
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do ponutação de meritocracia</param>
    /// <returns>Lista de ponutação de meritocracias</returns>
    [Authorize]
    [HttpGet]
    [Route("listsalaryscalescore")]
    public async Task<List<ViewCrudSalaryScaleScore>> ListSalaryScaleScore(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewCrudSalaryScaleScore> result = service.ListSalaryScaleScore(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    ///// <summary>
    ///// Novo ponutação de meritocracia
    ///// </summary>
    ///// <param name="view">Objeto de manutenção do ponutação de meritocracia</param>
    ///// <returns>Mensagem de sucesso</returns>
    //[HttpPost]
    //[Route("newmsalaryscale")]
    //public async Task<IActionResult> PostSalaryScaleScore([FromBody]ViewCrudSalaryScaleScore view)
    //{
    //  return await Task.Run(() =>Ok( service.NewSalaryScaleScore(view));
    //}
    /// <summary>
    /// Buscar ponutação de meritocracia para manutenção
    /// </summary>
    /// <param name="id">Identificador do ponutação de meritocracia</param>
    /// <returns>Objeto de manutenção do ponutação de meritocracia</returns>
    [Authorize]
    [HttpGet]
    [Route("getsalaryscale/{id}")]
    public async Task<ViewCrudSalaryScaleScore> ListSalaryScaleScore(string id)
    {
      return await Task.Run(() => service.GetSalaryScaleScore(id));
    }
    /// <summary>
    /// Alterar o ponutação de meritocracia
    /// </summary>
    /// <param name="view">Objeto de manutenção do ponutação de meritocracia</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updatesalaryscale")]
    public async Task<IActionResult> UpdateSalaryScaleScore([FromBody]ViewCrudSalaryScaleScore view)
    {
      return await Task.Run(() => Ok(service.UpdateSalaryScaleScore(view)));
    }
    /// <summary>
    /// Deletar um ponutação de meritocracia
    /// </summary>
    /// <param name="id">Identificador do ponutação de meritocracia</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletesalaryscale/{id}")]
    public async Task<IActionResult> DeleteSalaryScaleScore(string id)
    {
      return await Task.Run(() => Ok(service.DeleteSalaryScaleScore(id)));
    }
    #endregion
  }
}