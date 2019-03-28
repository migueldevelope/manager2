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
  /// Controlador de tabela salarial
  /// </summary>
  [Produces("application/json")]
  [Route("salaryscale")]
  public class SalaryScaleController : Controller
  {
    private readonly IServiceSalaryScale service;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Serviço da tabela salarial</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public SalaryScaleController(IServiceSalaryScale _service, IHttpContextAccessor contextAccessor)
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
    public List<ViewListSalaryScale> List(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Buscar objeto de manutenção da tabela salarial
    /// </summary>
    /// <param name="id">Identificador da tabela salarial</param>
    /// <returns>Objeto de menutenção da tabela salarial</returns>
    [Authorize]
    [HttpGet]
    [Route("edit/{id}")]
    public ViewCrudSalaryScale List(string id)
    {
      return service.Get(id);
    }
    /// <summary>
    /// Incluir nova tabela salarial
    /// </summary>
    /// <param name="view">Objeto de manutenção da tabela salarial</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPost]
    [Route("new")]
    public IActionResult PostSalary([FromBody]ViewCrudSalaryScale view)
    {
      return Ok(service.NewSalaryScale(view));
    }
    /// <summary>
    /// Atualizar uma tabela salarial
    /// </summary>
    /// <param name="view">Objeto de manutenção da tabela salarial</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public IActionResult UpdateSalary([FromBody]ViewCrudSalaryScale view)
    {
      return Ok(service.UpdateSalaryScale(view));
    }
    /// <summary>
    /// Excluir uma tabela salarial
    /// </summary>
    /// <param name="id">Identificador da tabela salarial</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public IActionResult Delete(string id)
    {
      return Ok(service.Remove(id));
    }
    #endregion

    #region Grade
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
    public List<ViewListGrade> ListGrade(string idsalaryscale, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGrade(idsalaryscale, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
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
    public List<ViewListGradeFilter> ListGrades(string idcompany, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListGrades(idcompany, ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Buscar grade para alteração
    /// </summary>
    /// <param name="idsalaryscale">Identificador da tabela salarial</param>
    /// <param name="id">Identificador do Grade</param>
    /// <returns>Objeto de manutenção do grade</returns>
    [Authorize]
    [HttpGet]
    [Route("editgrade/{idsalaryscale}/{id}")]
    public ViewCrudGrade GetGrade(string idsalaryscale, string id)
    {
      return service.GetGrade(idsalaryscale, id);
    }
    /// <summary>
    /// Incluir um novo grade na tabela salarial
    /// </summary>
    /// <param name="view">Informações do novo grade</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPost]
    [Route("addgrade")]
    public IActionResult PostGrade([FromBody]ViewCrudGrade view)
    {
      return Ok(service.AddGrade(view));
    }
    /// <summary>
    /// Alterar um grade da tabela salarial
    /// </summary>
    /// <param name="view">Grade para alterar</param>
    /// <returns>Mensagem de sucesso</returns>
    [Authorize]
    [HttpPut]
    [Route("updategrade")]
    public IActionResult UpdateGrade([FromBody]ViewCrudGrade view)
    {
      return Ok(service.UpdateGrade(view));
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
    [Route("updategradeposition")]
    public IActionResult UpdateGradePosition(string idsalaryscale, string idgrade, int position)
    {
      return Ok(service.UpdateGradePosition(idsalaryscale, idgrade, position));
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
    public string DeleteGrade(string idsalaryscale, string id)
    {
      return service.RemoveGrade(idsalaryscale, id);
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
    public IActionResult UpdateGrade([FromBody]ViewCrudStep view)
    {
      return Ok(service.UpdateStep(view));
    }
    #endregion
  }
}