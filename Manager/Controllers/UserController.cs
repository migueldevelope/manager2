using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
  public class UserController : DefaultController
  {
    private readonly IServiceUser service;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Serviço de usuário</param>
    /// <param name="contextAccessor">Autorização</param>
    public UserController(IServiceUser _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
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
    public async Task<List<ViewListUser>> List(EnumTypeUser type,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(count, page, filter, type);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }



    /// <summary>
    /// Aceite do Termo de Serviço
    /// </summary>
    /// <param name="iduser">Identificador do usuário</param>
    /// <returns>Informações de login e token de segurança, caso haja problema retorna a mensagem com o problema</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route("checktermofservice/{iduser}")]
    public async Task<string> CheckTermOfService(string iduser)
    {
      await service.CheckTermOfService(iduser);
      return "ok";
    }


    /// <summary>
    /// Buscar informações para edição do usuário
    /// </summary>
    /// <param name="iduser">Identificador do usuário</param>
    /// <returns>Objeto CRUD do usuário</returns>
    [Authorize]
    [HttpGet]
    [Route("get/{iduser}")]
    public async Task<ViewCrudUser> Get(string iduser)
    {
      return await service.Get(iduser);
    }
    /// <summary>
    /// Inclusão de novo usuário
    /// </summary>
    /// <param name="view">Objeto CRUD do usuário</param>
    /// <returns>Objeto CRUD incluído do usuário</returns>
    [Authorize]
    [HttpPost]
    [Route("new")]
    public async Task<ViewCrudUser> New([FromBody] ViewCrudUser view)
    {
      return await service.New(view);
    }
    /// <summary>
    /// Alteração de usuário
    /// </summary>
    /// <param name="view">Objeto CRUD do usuário para alterar</param>
    /// <returns>Objeto CRUD atualizado do usuário</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<ViewCrudUser> Update([FromBody] ViewCrudUser view)
    {
      return await service.Update(view);
    }
    /// <summary>
    /// Foto do perfil do usuário
    /// </summary>
    /// <param name="iduser">Identificador do usuário</param>
    /// <returns>URL da imagem da foto do perfil</returns>
    [Authorize]
    [HttpGet]
    [Route("photo/{iduser}")]
    public async Task<string> GetPhoto(string iduser)
    {
      return await service.GetPhoto(iduser);
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
    public async Task<List<ViewListPersonInfo>> ListPerson(string iduser,  int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListPerson(iduser, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
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
    public async Task<string> AlterPassword([FromBody]ViewAlterPass view, string idUser)
    {
      return await service.AlterPassword(view, idUser);
    }
    /// <summary>
    /// Alterar o password do usuário pelo esqueci minha senha
    /// </summary>
    /// <param name="view">Objeto com senhas novas</param>
    /// <param name="foreign">Identificador do esquecer senha</param>
    /// <returns>Mensagem de sucesso, ou error_valid</returns>
    [HttpPut]
    [Route("forgotpassword/{foreign}/alter")]
    public async Task<string> AlterPasswordForgot([FromBody]ViewAlterPass view, string foreign)
    {
      return await service.AlterPasswordForgot(view, foreign);
    }
    /// <summary>
    /// Enviar e-mail de esqueci minha senha
    /// </summary>
    /// <param name="view">Objeto com mensagens para o usuário</param>
    /// <param name="mail">E-mail para onde enviar a mensagem</param>
    /// <returns>Mensagem de sucesso!</returns>
    [HttpPut]
    [Route("forgotpassword/{mail}")]
    public async Task<string> ForgotPassword([FromBody]ViewForgotPassword view, string mail)
    {
      Config conn = XmlConnection.ReadConfig();
      return await service.ForgotPassword(mail, view, conn.SendGridKey);
    }
    #endregion

  }
}