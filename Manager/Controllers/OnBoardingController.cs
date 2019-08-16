using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador do Onboarding
  /// </summary>
  [Produces("application/json")]
  [Route("onboarding")]
  public class OnBoardingController : DefaultController
  {
    private readonly IServiceOnBoarding service;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Serviço de Onboarding</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public OnBoardingController(IServiceOnBoarding _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Onboarding
    /// <summary>
    /// Listar as pendências de Onboarding para o gestor
    /// </summary>
    /// <param name="idmanager">Identificação do gestor</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do colaborador</param>
    /// <returns>Lista com pendências de Onboarding</returns>
    [Authorize]
    [HttpGet]
    [Route("list/{idmanager}")]
    public async Task<List<ViewListOnBoarding>> List(string idmanager,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListOnBoarding> result = service.List(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("v2/list")]
    public async Task<List<ViewListOnBoarding>> List_v2([FromBody] List<ViewListIdIndicators> persons, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListOnBoarding> result = service.List_v2(persons, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Consulta a situação do colaborador no Onboarding
    /// </summary>
    /// <param name="idperson">Identificador do colaborador</param>
    /// <returns>Situação do Onboarding do cloaborador</returns>
    [Authorize]
    [HttpGet]
    [Route("personwait/{idperson}")]
    public async Task<ViewListOnBoarding> PersonWait(string idperson)
    {
      return await Task.Run(() =>service.PersonWait(idperson));
    }
    /// <summary>
    /// Inclusão de novo OnBoarding
    /// </summary>
    /// <param name="idperson">Identificador da pessoa</param>
    /// <returns>Objeto de listagem do OnBoarding</returns>
    [Authorize]
    [HttpPost]
    [Route("new/{idperson}")]
    public async Task<ViewListOnBoarding> New(string idperson)
    {
      return await Task.Run(() =>service.New(idperson));
    }
    /// <summary>
    /// Iniciar o processo de onboarding do colaborador
    /// </summary>
    /// <param name="id">Identificador do colaborador</param>
    /// <returns>Objeto de listagem do OnBoarding</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudOnboarding> Get(string id)
    {
      return await Task.Run(() =>service.Get(id));
    }
    /// <summary>
    /// Apagar comentários
    /// </summary>
    /// <param name="idonboarding">Identificador do onboarding</param>
    /// <param name="iditem">Identificador do item</param>
    /// <param name="idcomment">Identificador do comentário</param>
    /// <returns>Mensagem de Sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletecomments/{idonboarding}/{iditem}/{idcomments}")]
    public async Task<IActionResult> DeleteComments(string idonboarding, string iditem, string idcomment)
    {
      return await Task.Run(() => Ok(service.DeleteComments(idonboarding, iditem, idcomment)));
    }
    /// <summary>
    /// Alteração de leitura de comentário
    /// </summary>
    /// <param name="idonboarding">Identificador do onboarding</param>
    /// <param name="iditem">Identificador do item</param>
    /// <param name="usercomment">Marcação de leitura</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updatecommentsview/{idonboarding}/{iditem}/{usercomment}")]
    public async Task<IActionResult> UpdateCommentsView(string idonboarding, string iditem, EnumUserComment usercomment)
    {
      return await Task.Run(() =>Ok( service.UpdateCommentsView(idonboarding, iditem, usercomment)));
    }
    /// <summary>
    /// Apagar onboarding
    /// </summary>
    /// <param name="id">Identificador do Onboarding</param>
    /// <returns>Mensagem de Sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      return await Task.Run(() =>Ok( service.Delete(id)));
    }
    /// <summary>
    /// Atualiza informações do onboarding
    /// </summary>
    /// <param name="onboarding">Objeto Crud</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update([FromBody]ViewCrudOnboarding onboarding)
    {
      return await Task.Run(() =>Ok( service.Update(onboarding)));
    }
    /// <summary>
    /// Lista onboarding finalizados
    /// </summary>
    /// <param name="idmanager">Identificador contrato gestor</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listend/{idmanager}")]
    public async Task<List<ViewListOnBoarding>> ListEnded(string idmanager,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListEnded(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }
    /// <summary>
    /// Lista onboarding finalizado
    /// </summary>
    /// <param name="idmanager">Identificador contrato</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("personend/{idmanager}")]
    public async Task<List<ViewListOnBoarding>> ListPersonEnd(string idmanager,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPersonEnd(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }
    /// <summary>
    /// List onboarding para exclusão
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getlistexclud")]
    public async Task<List<ViewListOnBoarding>> ListExcluded( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListExcluded(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }
    /// <summary>
    /// Atualização informações de comentarios
    /// </summary>
    /// <param name="comments">Objeto Crud</param>
    /// <param name="idonboarding">Identificador onboarding</param>
    /// <param name="iditem">Indetificador item do onboarding</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatecomments/{idonboarding}/{iditem}")]
    public async Task<string> UpdateComments([FromBody]ViewCrudComment comments, string idonboarding, string iditem)
    {
      return await Task.Run(() =>service.UpdateComments(idonboarding, iditem, comments));
    }
    /// <summary>
    /// Lista comentarios onboarding
    /// </summary>
    /// <param name="idonboarding">Identificador onboarding</param>
    /// <param name="iditem">Identificador item onboarding</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listcomments/{idonboarding}/{iditem}")]
    public async Task<List<ViewCrudComment>> ListComments(string idonboarding, string iditem)
    {
      return await Task.Run(() =>service.ListComments(idonboarding, iditem));
    }
    /// <summary>
    /// Inclusão comentario no item do onboarding
    /// </summary>
    /// <param name="comments">Objeto Crud</param>
    /// <param name="idonboarding">Identificador onboarding</param>
    /// <param name="iditem">Identificador item do onboarding</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addcomments/{idonboarding}/{iditem}")]
    public async Task<List<ViewCrudComment>> AddComments([FromBody]ViewCrudComment comments, string idonboarding, string iditem)
    {
      return await Task.Run(() =>service.AddComments(idonboarding, iditem, comments));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("exportonboarding/{idperson}")]
    public async Task<List<ViewExportStatusOnboarding>> ExportStatusOnboarding(string idperson)
    {
      return await Task.Run(() => service.ExportStatusOnboarding(idperson));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("exportonboardingcomments")]
    public async Task<List<ViewExportOnboardingComments>> ExportOnboardingComments([FromBody] List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.ExportOnboardingComments(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("exportonboarding")]
    public async Task<List<ViewExportStatusOnboardingGeral>> ExportStatusOnboarding([FromBody] List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.ExportStatusOnboarding(persons));
    }

    #endregion
  }
}