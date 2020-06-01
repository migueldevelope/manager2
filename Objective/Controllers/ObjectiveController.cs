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
  [Route("objective")]
  public class ObjectiveController
  {
    private readonly IServiceObjective service;

    #region Constructor
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">Serviço da empresa</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public ObjectiveController(IServiceObjective _service, IHttpContextAccessor contextAccessor) 
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Objective
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
    public async Task<List<ViewListObjective>> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListObjective> result = service.List(ref total, count, page, filter);
      //Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Cadastrar uma nova empresa
    /// </summary>
    /// <param name="view">Objeto de cadastro da empresa</param>
    /// <returns></returns>
    [HttpPost]
    [Route("new")]
    public async Task<ViewCrudObjective> Post([FromBody]ViewCrudObjective view)
    {
      return await Task.Run(() => service.New(view));
    }
    /// <summary>
    /// Retorar a empresa para manutenção
    /// </summary>
    /// <param name="id">Identificador da empresa</param>
    /// <returns>Objeto de manutenção da empresa</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudObjective> Get(string id)
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
    public async Task<ViewCrudObjective> Update([FromBody]ViewCrudObjective view)
    {
      return await Task.Run(() => service.Update(view));
    }
    /// <summary>
    /// Excluir uma empresa
    /// </summary>
    /// <param name="id">Identificação da empresa</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<string> Delete(string id)
    {
      return await Task.Run(() => service.Delete(id));
    }
    #endregion

    #region KeyResult
    /// <summary>
    /// Listar estabelecimentos
    /// </summary>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do estabelecimento</param>
    /// <returns>Lista de estabelecimentos</returns>
    [Authorize]
    [HttpGet]
    [Route("listkeyresult")]
    public async Task<List<ViewListKeyResult>> ListKeyResult(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListKeyResult> result = service.ListKeyResult(ref total, count, page, filter);
      //Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Listar estabelecimento de uma empresa
    /// </summary>
    /// <param name="idobjective">Identificador da empresa</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do estabelecimento</param>
    /// <returns>Lista de estabelecimentos da empresa</returns>
    [Authorize]
    [HttpGet]
    [Route("listkeyresult/{idobjective}")]
    public async Task<List<ViewListKeyResult>> ListKeyResult(string idobjective, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListKeyResult> result = service.ListKeyResult(idobjective, ref total, count, page, filter);
      //Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Novo estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [HttpPost]
    [Route("newkeyresult")]
    public async Task<ViewCrudKeyResult> PostKeyResult([FromBody]ViewCrudKeyResult view)
    {
      return await Task.Run(() => service.NewKeyResult(view));
    }
    /// <summary>
    /// Buscar estabelecimento para manutenção
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Objeto de manutenção do estabelecimento</returns>
    [Authorize]
    [HttpGet]
    [Route("getkeyresult/{id}")]
    public async Task<ViewCrudKeyResult> ListKeyResult(string id)
    {
      return await Task.Run(() => service.GetKeyResult(id));
    }
    /// <summary>
    /// Alterar o estabelecimento
    /// </summary>
    /// <param name="view">Objeto de manutenção do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updatekeyresult")]
    public async Task<ViewCrudKeyResult> UpdateKeyResult([FromBody]ViewCrudKeyResult view)
    {
      return await Task.Run(() => service.UpdateKeyResult(view));
    }
    /// <summary>
    /// Deletar um estabelecimento
    /// </summary>
    /// <param name="id">Identificador do estabelecimento</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("deletekeyresult/{id}")]
    public async Task<string> DeleteKeyResult(string id)
    {
      return await Task.Run(() => service.DeleteKeyResult(id));
    }
    #endregion

  }
}