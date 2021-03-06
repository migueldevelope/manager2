﻿using System.Collections.Generic;
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
        /// 
        /// </summary>
        /// <param name="idperson"></param>
        /// <param name="count"></param>
        /// <param name="page"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("listperson/{idperson}")]
        public async Task<List<ViewListFluidCareers>> ListPerson(string idperson, int count = 10, int page = 1, string filter = "")
        {
            long total = 0;
            List<ViewListFluidCareers> result = service.ListPerson(idperson, ref total, count, page, filter);
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
        /// <param name="filterCalcFluidCareers"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("calc/{idperson}/{filterCalcFluidCareers}")]
        public ViewFluidCareerPerson Calc([FromBody]List<ViewCrudSkillsCareers> skills, string idperson, EnumFilterCalcFluidCareers filterCalcFluidCareers)
        {
            return service.Calc(idperson, skills, filterCalcFluidCareers);
        }

        /// <summary>
        /// Lista as skill's
        /// </summary>
        /// <param name="count"></param>
        /// <param name="page"></param>
        /// <param name="filter"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getskills")]
        public List<ViewCrudSkillsCareers> GetSkills(byte type, int count = 10, int page = 1, string filter = "")
        {
            long total = 0;
            var result = service.GetSkills(type, ref total, filter, count, page);
            Response.Headers.Add("x-total-count", total.ToString());
            return result;
        }

        /// <summary>
        /// Lista as skill's
        /// </summary>
        /// <param name="idperson"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getperson/{idperson}")]
        public ViewFluidCareersPerson GetPerson(string idperson)
        {
            var result = service.GetPerson(idperson);
            return result;
        }

        #endregion

        #region Plan

        /// <summary>
        /// Cadastrar uma nova empresa
        /// </summary>
        /// <param name="view">Objeto de cadastro da empresa</param>
        /// <param name="idfluidcareer">Identificação da carreira fluida</param>
        /// <returns></returns>
        [HttpPost]
        [Route("newplan/{idfluidcareer}")]
        public async Task<IActionResult> PostPlan([FromBody]ViewCrudFluidCareerPlan view, string idfluidcareer)
        {
            return await Task.Run(() => Ok(service.NewPlan(idfluidcareer, view)));
        }
        /// <summary>
        /// Retorar a empresa para manutenção
        /// </summary>
        /// <param name="idfluidcareer">Identificador da empresa</param>
        /// <returns>Objeto de manutenção da empresa</returns>
        [Authorize]
        [HttpGet]
        [Route("getplan/{idfluidcareer}")]
        public async Task<ViewCrudFluidCareerPlan> GetPlan(string idfluidcareer)
        {
            return await Task.Run(() => service.GetPlan(idfluidcareer));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idperson"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getplanperson/{idperson}")]
        public async Task<List<ViewCrudFluidCareerPlan>> GetPlanPerson(string idperson)
        {
            return await Task.Run(() => service.GetPlanPerson(idperson));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getskillsplan/{id}")]
        public async Task<List<ViewListSkill>> GetSkillsPlan(string id)
        {
            return await Task.Run(() => service.GetSkillsPlan(id));
        }

        /// <summary>
        /// Alterar a empresa
        /// </summary>
        /// <param name="view">Objeto de manutenção da empresa</param>
        /// <param name="idfluidcareer">Identificação da carreira fluida</param>
        /// <returns>Mensagem de sucesso</returns>
        [Authorize]
        [HttpPut]
        [Route("updateplan/{idfluidcareer}")]
        public async Task<IActionResult> UpdatePlan([FromBody]ViewCrudFluidCareerPlan view, string idfluidcareer)
        {
            return await Task.Run(() => Ok(service.UpdatePlan(idfluidcareer, view)));
        }
        /// <summary>
        /// Excluir uma empresa
        /// </summary>
        /// <param name="idfluidcareer">Identificação da empresa</param>
        /// <returns>Mensagem de sucesso</returns>
        [Authorize]
        [HttpDelete]
        [Route("deleteplan/{idfluidcareer}")]
        public async Task<IActionResult> DeletePlan(string idfluidcareer)
        {
            return await Task.Run(() => Ok(service.Delete(idfluidcareer)));
        }

        #endregion


    }
}