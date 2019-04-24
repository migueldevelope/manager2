using System;
using System.Collections.Generic;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tools;
using Tools.Data;

namespace Manager.Controllers
{
  /// <summary>
  /// Controlador de Usuários 
  /// </summary>
  [Produces("application/json")]
  [Route("user")]
  public class UserController : Controller
  {
    private readonly IServiceUser service;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Serviço de usuário</param>
    /// <param name="contextAccessor">Autorização</param>
    public UserController(IServiceUser _service, IHttpContextAccessor contextAccessor)
    {
      try
      {
        service = _service;
        service.SetUser(contextAccessor);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region User
    /// <summary>
    /// Listar usuários da base de dados
    /// </summary>
    /// <param name="type">Tipo do usuário que está fazendo a consulta</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do usuário</param>
    /// <returns>Lista de usuários da conta</returns>
    [Authorize]
    [HttpGet]
    [Route("list/{type}")]
    public List<ViewListUser> List(EnumTypeUser type, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter, type);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    /// <summary>
    /// Buscar informações para edição do usuário
    /// </summary>
    /// <param name="iduser">Identificador do usuário</param>
    /// <returns>Objeto CRUD do usuário</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{iduser}")]
    public ViewCrudUser Get(string iduser)
    {
      return service.Get(iduser);
    }
    /// <summary>
    /// Inclusão de novo usuário
    /// </summary>
    /// <param name="view">Objeto CRUD do usuário</param>
    /// <returns>Objeto CRUD incluído do usuário</returns>
    [Authorize]
    [HttpPost]
    [Route("new")]
    public ViewCrudUser New([FromBody] ViewCrudUser view)
    {
      return service.New(view);
    }
    /// <summary>
    /// Alteração de usuário
    /// </summary>
    /// <param name="view">Objeto CRUD do usuário para alterar</param>
    /// <returns>Objeto CRUD atualizado do usuário</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public ViewCrudUser Update([FromBody] ViewCrudUser view)
    {
      return service.Update(view);
    }
    /// <summary>
    /// Foto do perfil do usuário
    /// </summary>
    /// <param name="iduser">Identificador do usuário</param>
    /// <returns>URL da imagem da foto do perfil</returns>
    [Authorize]
    [HttpGet]
    [Route("photo/{iduser}")]
    public string GetPhoto(string iduser)
    {
      return service.GetPhoto(iduser);
    }
    #endregion

    #region Person
    /// <summary>
    /// Listar as pessoas de um usuário
    /// </summary>
    /// <param name="iduser">Identificador do usuário</param>
    /// <param name="count">Quantidade de registros</param>
    /// <param name="page">Página para mostrar</param>
    /// <param name="filter">Filtro para o nome do usuário</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listperson/{iduser}")]
    public List<ViewListPersonInfo> ListPerson(string iduser, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPerson(iduser, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return result;
    }
    #endregion

    #region Password
    /// <summary>
    /// Alterar o password do usuário
    /// </summary>
    /// <param name="view">Objeto com senhas novas</param>
    /// <param name="idUser">Identificador do usuário</param>
    /// <returns>Mensagem de sucesso, ou error_old_password</returns>
    [Authorize]
    [HttpPut]
    [Route("alterpass/{idUser}")]
    public string AlterPassword([FromBody]ViewAlterPass view, string idUser)
    {
      return service.AlterPassword(view, idUser);
    }
    /// <summary>
    /// Alterar o password do usuário pelo esqueci minha senha
    /// </summary>
    /// <param name="view">Objeto com senhas novas</param>
    /// <param name="foreign">Identificador do esquecer senha</param>
    /// <returns>Mensagem de sucesso, ou error_valid</returns>
    [HttpPut]
    [Route("forgotpassword/{foreign}/alter")]
    public string AlterPasswordForgot([FromBody]ViewAlterPass view, string foreign)
    {
      return service.AlterPasswordForgot(view, foreign);
    }
    /// <summary>
    /// Enviar e-mail de esqueci minha senha
    /// </summary>
    /// <param name="view">Objeto com mensagens para o usuário</param>
    /// <param name="mail">E-mail para onde enviar a mensagem</param>
    /// <returns>Mensagem de sucesso!</returns>
    [HttpPut]
    [Route("forgotpassword/{mail}")]
    public string ForgotPassword([FromBody]ViewForgotPassword view, string mail)
    {
      Config conn = XmlConnection.ReadConfig();
      return service.ForgotPassword(mail, view, conn.SendGridKey).Result;
    }
    #endregion

  }
}