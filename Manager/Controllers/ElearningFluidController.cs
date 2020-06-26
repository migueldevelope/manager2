using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador da empresa
  /// </summary>
  [Produces("application/json")]
  [Route("elearningfluid")]
  public class ElearningFluidController : DefaultController
  {
    private readonly IServiceElearningFluid service;

    #region Constructor
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">Serviço da empresa</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public ElearningFluidController(IServiceElearningFluid _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region ElearningFluid
    /// <summary>
    /// Listar as empresas
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome da empresa</param>
    /// <returns>Lista de empresas cadastradas</returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public async Task<List<ViewListElearningFluid>> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListElearningFluid> result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Cadastrar uma nova empresa
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("new")]
    public async Task<IActionResult> Post()
    {
      return await Task.Run(() => Ok(service.New()));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("elearningcertificate")]
    public async Task<IActionResult> ElearningCertificate()
    {
      return await Task.Run(() => Ok(service.ElearningCertificate()));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("elearningvideo")]
    public async Task<IActionResult> ElearningVideo()
    {
      return await Task.Run(() => Ok(service.ElearningVideo()));
    }

    /// <summary>
    /// Retorar a empresa para manutenção
    /// </summary>
    /// <param name="id">Identificador da empresa</param>
    /// <returns>Objeto de manutenção da empresa</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudElearningFluid> Get(string id)
    {
      return await Task.Run(() => service.Get(id));
    }
    /// <summary>
    /// Alterar a empresa
    /// </summary>
    /// <param name="view">Objeto de manutenção da empresa</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update([FromBody]ViewCrudElearningFluid view)
    {
      return await Task.Run(() => Ok(service.Update(view)));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idquestion"></param>
    /// <param name="idelearning"></param>
    /// <param name="answer"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatequestion/{idquestion}/{idelearning}/{answer}")]
    public async Task<IActionResult> UpdateQuestion(string idquestion, string idelearning, string answer)
    {
      return await Task.Run(() => Ok(service.UpdateQuestion(idquestion, idelearning, answer)));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("endelearning/{id}")]
    public async Task<IActionResult> EndElearning(string id)
    {
      return await Task.Run(() => Ok(service.EndElearning(id)));
    }

    /// <summary>
    /// Excluir uma empresa
    /// </summary>
    /// <param name="id">Identificação da empresa</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      return await Task.Run(() => Ok(service.Delete(id)));
    }
    #endregion

    #region ElearningFluidQuestions
    /// <summary>
    /// Listar estabelecimentos
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do estabelecimento</param>
    /// <returns>Lista de estabelecimentos</returns>
    [Authorize]
    [HttpGet]
    [Route("listelearningfluidquestions")]
    public async Task<List<ViewListElearningFluidQuestions>> ListElearningFluidQuestions(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListElearningFluidQuestions> result = service.ListElearningFluidQuestions(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Novo estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("newelearningfluidquestions")]
    public async Task<IActionResult> PostElearningFluidQuestions([FromBody]ViewCrudElearningFluidQuestions view)
    {
      return await Task.Run(() => Ok(service.NewElearningFluidQuestions(view)));
    }
    /// <summary>
    /// Buscar estabelecimento para manutenção
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Objeto de manutenção do estabelecimento</returns>
    [Authorize]
    [HttpGet]
    [Route("getelearningfluidquestions/{id}")]
    public async Task<ViewCrudElearningFluidQuestions> ListElearningFluidQuestions(string id)
    {
      return await Task.Run(() => service.GetElearningFluidQuestions(id));
    }
    /// <summary>
    /// Alterar o estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updateelearningfluidquestions")]
    public async Task<IActionResult> UpdateElearningFluidQuestions([FromBody]ViewCrudElearningFluidQuestions view)
    {
      return await Task.Run(() => Ok(service.UpdateElearningFluidQuestions(view)));
    }
    /// <summary>
    /// Deletar um estabelecimento
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteelearningfluidquestions/{id}")]
    public async Task<IActionResult> DeleteElearningFluidQuestions(string id)
    {
      return await Task.Run(() => Ok(service.DeleteElearningFluidQuestions(id)));
    }
    #endregion

  }
}