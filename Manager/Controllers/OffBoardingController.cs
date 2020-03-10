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
    /// Controlador da empresa
    /// </summary>
    [Produces("application/json")]
    [Route("offboarding")]
    public class OffBoardingController : DefaultController
    {
        private readonly IServiceOffBoarding service;

        #region Constructor
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="_service">Serviço da empresa</param>
        /// <param name="contextAccessor">Token de segurança</param>
        public OffBoardingController(IServiceOffBoarding _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            service = _service;
            service.SetUser(contextAccessor);
        }
        #endregion

        #region OffBoarding
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
        public async Task<List<ViewListOffBoarding>> List(int count = 10, int page = 1, string filter = "")
        {
            long total = 0;
            List<ViewListOffBoarding> result = service.List(ref total, count, page, filter);
            Response.Headers.Add("x-total-count", total.ToString());
            return await Task.Run(() => result);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idperson"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("new/{idperson}/{step}")]
        public async Task<IActionResult> Post(string idperson, EnumStepOffBoarding step)
        {
            return await Task.Run(() => Ok(service.New(idperson, step)));
        }
        /// <summary>
        /// Retorar a empresa para manutenção
        /// </summary>
        /// <param name="id">Identificador da empresa</param>
        /// <returns>Objeto de manutenção da empresa</returns>
        [Authorize]
        [HttpGet]
        [Route("get/{id}")]
        public async Task<ViewCrudOffBoarding> Get(string id)
        {
            return await Task.Run(() => service.Get(id));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("update/{step}")]
        public async Task<IActionResult> Update([FromBody]ViewCrudOffBoarding view, EnumStepOffBoarding step)
        {
            return await Task.Run(() => Ok(service.Update(view, step)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="step"></param>
        /// <param name="idquestion"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("updatequestionsmark/{id}/{step}/{idquestion}/{mark}")]
        public async Task<IActionResult> UpdateQuestionsMark(string id, EnumStepOffBoarding step, string idquestion, byte mark)
        {
            return await Task.Run(() => Ok(service.UpdateQuestionsMark(id, step, idquestion, mark)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="id"></param>
        /// <param name="step"></param>
        /// <param name="idquestion"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("updatequestionstext/{id}/{step}/{idquestion}")]
        public async Task<IActionResult> UpdateQuestionsText([FromBody]ViewResponse response, string id, EnumStepOffBoarding step, string idquestion)
        {
            return await Task.Run(() => Ok(service.UpdateQuestionsText(id, step, idquestion, response)));
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

    }
}