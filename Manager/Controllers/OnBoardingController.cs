using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
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
  /// Controlador do Onboarding
  /// </summary>
  [Produces("application/json")]
  [Route("onboarding")]
  public class OnBoardingController : Controller
  {
    private readonly IServiceOnBoarding service;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Serviço de Onboarding</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public OnBoardingController(IServiceOnBoarding _service, IHttpContextAccessor contextAccessor)
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
    public List<ViewListOnBoarding> List(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      List<ViewListOnBoarding> result = service.ListOnBoarding(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Consulta a situação do colaborador no Onboarding
    /// </summary>
    /// <param name="idperson">Identificador do colaborador</param>
    /// <returns>Situação do Onboarding do cloaborador</returns>
    [Authorize]
    [HttpGet]
    [Route("personwait/{idmanager}")]
    public ViewListOnBoarding ListPerson(string idperson)
    {
      return service.PersonOnBoardingWait(idperson);
    }
    /// <summary>
    /// Inclusão de novo OnBoarding
    /// </summary>
    /// <param name="idperson">Identificador da pessoa</param>
    /// <returns>Objeto de listagem do OnBoarding</returns>
    [Authorize]
    [HttpPost]
    [Route("new/{idperson}")]
    public ViewListOnBoarding Post(string idperson)
    {
      return service.NewOnBoarding(idperson);
    }
    /// <summary>
    /// Iniciar o processo de onboarding do colaborador
    /// </summary>
    /// <param name="id">Identificador do colaborador</param>
    /// <returns>Objeto de listagem do OnBoarding</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public ViewCrudOnboarding GetOnBoarding(string id)
    {
      return service.GetOnBoarding(id);
    }

    #endregion

    #region Old
    [Authorize]
    [HttpPost]
    [Route("old/new/{idperson}")]
    public OnBoarding PostOld([FromBody]OnBoarding onboarding, string idperson)
    {
      return service.NewOnBoardingOld(onboarding, idperson);
    }

    [Authorize]
    [HttpPut]
    [Route("old/update/{idperson}")]
    public string Put([FromBody]OnBoarding onboarding, string idperson)
    {
      return service.UpdateOnBoarding(onboarding, idperson);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listend/{idmanager}")]
    public List<OnBoarding> ListEnd(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOnBoardingsEnd(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/list/{idmanager}")]
    public List<OnBoarding> ListOld(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListOnBoardingsWaitOld(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/personend/{idmanager}")]
    public List<OnBoarding> PersonOnBoardingsEnd(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.PersonOnBoardingsEnd(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpGet]
    [Route("old/personwait/{idmanager}")]
    public OnBoarding ListPersonOld(string idmanager)
    {
      return service.PersonOnBoardingsWaitOld(idmanager);
    }

    [Authorize]
    [HttpGet]
    [Route("old/get/{id}")]
    public OnBoarding GetOnBoardingOld(string id)
    {
      return service.GetOnBoardings(id);
    }

    [Authorize]
    [HttpGet]
    [Route("old/getlistexclud")]
    public List<OnBoarding> GetListExclud(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetListExclud(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }

    [Authorize]
    [HttpDelete]
    [Route("old/delete/{idperson}")]
    public string RemoveOnBoarding(string idperson)
    {
      return service.RemoveOnBoarding(idperson);
    }


    [Authorize]
    [HttpPut]
    [Route("old/updatecomments/{idonboarding}/{iditem}")]
    public string UpdateComments([FromBody]ListComments comments, string idonboarding, string iditem)
    {
      return service.UpdateComments(idonboarding, iditem, comments);
    }

    [Authorize]
    [HttpGet]
    [Route("old/listcomments/{idonboarding}/{iditem}")]
    public List<ListComments> GetListComments( string idonboarding, string iditem)
    {
      return service.GetListComments(idonboarding, iditem);
    }

    [Authorize]
    [HttpPost]
    [Route("old/addcomments/{idonboarding}/{iditem}")]
    public List<ListComments> AddComments([FromBody]ListComments comments, string idonboarding, string iditem)
    {
      return service.AddComments(idonboarding, iditem, comments);
    }


    [Authorize]
    [HttpDelete]
    [Route("old/deletecomments/{idonboarding}/{iditem}/{idcomments}")]
    public string DeleteComments(string idonboarding, string iditem, string idcomments)
    {
      return service.DeleteComments(idonboarding, iditem, idcomments);
    }

    [Authorize]
    [HttpPut]
    [Route("old/updatecommentsview/{idonboarding}/{iditem}/{usercomment}")]
    public string UpdateCommentsView(string idonboarding, string iditem, EnumUserComment usercomment)
    {
      return service.UpdateCommentsView(idonboarding, iditem, usercomment);
    }
  }
  #endregion

}