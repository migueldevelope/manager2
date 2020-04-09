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
  /// Controlador de tabela salarial
  /// </summary>
  [Produces("application/json")]
  [Route("salaryscale")]
  public class SalaryScaleController : DefaultController
  {
    private readonly IServiceSalaryScale service;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Serviço da tabela salarial</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public SalaryScaleController(IServiceSalaryScale _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region Salary Scale
    /// <summary>
    /// Listar todas as tabelas salariais da empresa
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome da tabela salarial</param>
    /// <returns>Lista de tabelas salariais</returns>
    [Authorize]
    [HttpGet]
    [Route("list/{idcompany}")]
    public async Task<List<ViewListSalaryScale>> List(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Buscar objeto de manutenção da tabela salarial
    /// </summary>
    /// <param name="id">Identificador da tabela salarial</param>
    /// <returns>Objeto de menutenção da tabela salarial</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudSalaryScale> Get(string id)
    {
      return await Task.Run(() => service.Get(id));
    }
    /// <summary>
    /// Incluir nova tabela salarial
    /// </summary>
    /// <param name="view">Objeto de manutenção da tabela salarial</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPost]
    [Route("new")]
    public async Task<IActionResult> New([FromBody]ViewCrudSalaryScale view)
    {
      return await Task.Run(() => Ok(service.New(view)));
    }
    /// <summary>
    /// Atualizar uma tabela salarial
    /// </summary>
    /// <param name="view">Objeto de manutenção da tabela salarial</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update([FromBody]ViewCrudSalaryScale view)
    {
      return await Task.Run(() => Ok(service.Update(view)));
    }
    /// <summary>
    /// Excluir uma tabela salarial
    /// </summary>
    /// <param name="id">Identificador da tabela salarial</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      return await Task.Run(() => Ok(service.Delete(id)));
    }
    #endregion

    #region Salary Scale


    /// <summary>
    /// Update salaryscale
    /// </summary>
    /// <param name="idsalaryscale"></param>
    /// <param name="view"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatesteps/{idsalaryscale}")]
    public async Task<string> UpdateSteps([FromBody]ViewPercent view, string idsalaryscale)
    {
      return await Task.Run(() => service.UpdateSteps(idsalaryscale, view.Percent));
    }

    /// <summary>
    /// Restore Version
    /// </summary>
    /// <param name="idsalaryscale"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("restoreversion/{idsalaryscale}")]
    public async Task<string> UpdateSteps(string idsalaryscale)
    {
      return await Task.Run(() => service.RestoreVersion(idsalaryscale));
    }

    /// <summary>
    /// Listar todas as tabelas salariais da empresa
    /// </summary>
    /// <param name="idsalaryscale">Identificador da tabela salarial</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome da tabela salarial</param>
    /// <returns>Lista de tabelas salariais</returns>
    [Authorize]
    [HttpGet]
    [Route("listsalaryscalelog/{idsalaryscale}")]
    public async Task<List<ViewListSalaryScaleLog>> ListSalaryScaleLog(string idsalaryscale, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListSalaryScaleLog(idsalaryscale, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="view"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatelog/{id}")]
    public async Task<IActionResult> UpdateLog([FromBody]ViewCrudDescription view, string id)
    {
      return await Task.Run(() => Ok(service.UpdateLog(id, view)));
    }

    /// <summary>
    /// Buscar objeto de manutenção da tabela salarial
    /// </summary>
    /// <param name="id">Identificador da tabela salarial</param>
    /// <returns>Objeto de menutenção da tabela salarial</returns>
    [Authorize]
    [HttpGet]
    [Route("getsalaryscalelog/{id}")]
    public async Task<ViewCrudSalaryScaleLog> GetSalaryScaleLog(string id)
    {
      return await Task.Run(() => service.GetSalaryScaleLog(id));
    }
    /// <summary>
    /// Incluir nova tabela salarial
    /// </summary>
    /// <param name="idsalaryscale">identificador da tabela salarial</param>
    /// <param name="view">comentarios da alteração</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPost]
    [Route("newversion/{idsalaryscale}")]
    public async Task<IActionResult> NewVersion([FromBody] ViewCrudDescription view, string idsalaryscale)
    {
      return await Task.Run(() => Ok(service.NewVersion(idsalaryscale, view.Description)));
    }
    #endregion

    #region Grade


    /// <summary>
    /// 
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addoccupationsalaryscale")]
    public async Task<string> AddOccupationSalaryScale([FromBody]ViewCrudOccupationSalaryScale view)
    {
      var result = service.AddOccupationSalaryScale(view);
      return await Task.Run(() => result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idoccupation"></param>
    /// <param name="idgrade"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("removeoccupationsalaryscale/{idoccupation}/{idgrade}")]
    public async Task<string> RemoveOccupationSalaryScale(string idoccupation, string idgrade)
    {
      var result = service.RemoveOccupationSalaryScale(idoccupation, idgrade);
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Listar todos os grades de uma tabela salarial
    /// </summary>
    /// <param name="idsalaryscale">Identificador da tabela salarial</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome da tabela salarial</param>
    /// <returns>Matriz de grades de uma tabela salarial</returns>
    [Authorize]
    [HttpGet]
    [Route("listgrade/{idsalaryscale}")]
    public async Task<List<ViewListGrade>> ListGrade(string idsalaryscale, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGrade(idsalaryscale, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmanager"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listgrademanager/{idmanager}")]
    public async Task<List<ViewListGrade>> ListGradeManager(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGradeManager(idmanager, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Lista todos os grades para filtro
    /// </summary>
    /// <param name="idcompany">Identificador da empresa</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listgrades/{idcompany}")]
    public async Task<List<ViewListGradeFilter>> ListGrades(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGrades(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Buscar grade para alteração
    /// </summary>
    /// <param name="idsalaryscale">Identificador da tabela salarial</param>
    /// <param name="id">Identificador do Grade</param>
    /// <returns>Objeto de manutenção do grade</returns>
    [Authorize]
    [HttpGet]
    [Route("getgrade/{idsalaryscale}/{id}")]
    public async Task<ViewCrudGrade> GetGrade(string idsalaryscale, string id)
    {
      return await Task.Run(() => service.GetGrade(idsalaryscale, id));
    }
    /// <summary>
    /// Incluir um novo grade na tabela salarial
    /// </summary>
    /// <param name="view">Informações do novo grade</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPost]
    [Route("addgrade")]
    public async Task<IActionResult> AddGrade([FromBody]ViewCrudGrade view)
    {
      return await Task.Run(() => Ok(service.AddGrade(view)));
    }
    /// <summary>
    /// Alterar um grade da tabela salarial
    /// </summary>
    /// <param name="view">Grade para alterar</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updategrade")]
    public async Task<IActionResult> UpdateGrade([FromBody]ViewCrudGrade view)
    {
      return await Task.Run(() => Ok(service.UpdateGrade(view)));
    }
    /// <summary>
    /// Alterar a ordem do grade
    /// </summary>
    /// <param name="idsalaryscale">Identificador da tabela salarial</param>
    /// <param name="idgrade">Identificador do Grade</param>
    /// <param name="position">Nova posicao</param>
    /// <returns>Mensagem de Sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updategradeposition/{idsalaryscale}/{idgrade}")]
    public async Task<IActionResult> UpdateGradePosition(string idsalaryscale, string idgrade, int position)
    {
      return await Task.Run(() => Ok(service.UpdateGradePosition(idsalaryscale, idgrade, position)));
    }
    /// <summary>
    /// Remover um grade da tabela salarial
    /// </summary>
    /// <param name="idsalaryscale">Identificador da tabela salarial</param>
    /// <param name="id">Identificador do grade</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deletegrade/{idsalaryscale}/{id}")]
    public async Task<string> DeleteGrade(string idsalaryscale, string id)
    {
      return await Task.Run(() => service.DeleteGrade(idsalaryscale, id));
    }
    #endregion

    #region Step
    /// <summary>
    /// Alterar o salário de um step do grade da tabela salarial
    /// </summary>
    /// <param name="view">Objeto de alteração de salário</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updatestep")]
    public async Task<IActionResult> UpdateStep([FromBody]ViewCrudStep view)
    {
      return await Task.Run(() => Ok(service.UpdateStep(view)));
    }
    #endregion

  }
}