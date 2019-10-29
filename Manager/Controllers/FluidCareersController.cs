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
  [Route("fluidcareers")]
  public class FluidCareersController : DefaultController
  {
    private readonly IServiceFluidCareers service;

    #region Constructor
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="_service">Serviço da empresa</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public FluidCareersController(IServiceFluidCareers _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }
    #endregion

    #region FluidCareers
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
    public async Task<List<ViewListFluidCareers>> List(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListFluidCareers> result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Cadastrar uma nova empresa
    /// </summary>
    /// <param name="view">Objeto de cadastro da empresa</param>
    /// <returns></returns>
    [HttpPost]
    [Route("new")]
    public async Task<IActionResult> Post([FromBody]ViewCrudFluidCareers view)
    {
      return await Task.Run(() => Ok(service.New(view)));
    }
    /// <summary>
    /// Retorar a empresa para manutenção
    /// </summary>
    /// <param name="id">Identificador da empresa</param>
    /// <returns>Objeto de manutenção da empresa</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudFluidCareers> Get(string id)
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
    public async Task<IActionResult> Update([FromBody]ViewCrudFluidCareers view)
    {
      return await Task.Run(() => Ok(service.Update(view)));
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <param name="skills"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("calc/{idperson}")]
    public  ViewFluidCareers Calc([FromBody]List<ViewCrudSkillsCareers> skills,string idperson)
    {
      return service.Calc(idperson, skills);
    }

    /// <summary>
    /// Lista as skill's
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getskills")]
    public List<ViewCrudSkillsCareers> GetSkills(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetSkills(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    #endregion


  }
}