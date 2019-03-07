using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Manager.Controllers
{
  /// <summary>
  /// Controle de Autenticação
  /// </summary>
  [Produces("application/json")]
  [Route("authentication")]
  public class AuthenticationController : Controller
  {

    #region Constructor + Fields
    private readonly IServiceAuthentication service;
    private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";
    /// <summary>
    /// Contrutor do controle
    /// </summary>
    /// <param name="_service">Serviço de autenticação</param>
    public AuthenticationController(IServiceAuthentication _service)
    {
      service = _service;
    }
    #endregion

    #region Old Authentication
    /// <summary>
    /// Autenticação de usuário
    /// </summary>
    /// <param name="user">Objeto de autenticação de usuário</param>
    /// <returns>Informações de login e token de segurança, caso haja problema retorna a mensagem com o problema</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route("old")]
    public ObjectResult Post([FromBody]ViewAuthentication user)
    {
      try
      {
        if (string.IsNullOrEmpty(user.Mail))
          return BadRequest("MSG1");
        if (string.IsNullOrEmpty(user.Password))
          return BadRequest("MSG2");

        bool authMarista = false;
        if (user.Mail.IndexOf("@maristas.org.br") != -1 || user.Mail.IndexOf("@pucrs.br") != -1)
          authMarista = true;

        ViewPerson person;
        if (authMarista)
          person = service.AuthenticationMaristas(user.Mail, user.Password);
        else
          person = service.Authentication(user.Mail, user.Password);

        Claim[] claims = new[]
        {
        new Claim(ClaimTypes.Name, person.Name),
        new Claim(ClaimTypes.Hash, person.IdAccount),
        new Claim(ClaimTypes.Email, user.Mail),
        new Claim(ClaimTypes.NameIdentifier, person.NameAccount),
        new Claim(ClaimTypes.UserData, person.Contracts.FirstOrDefault().IdPerson)
      };
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "localhost",
            audience: "localhost",
            claims: claims,
            expires: DateTime.Now.AddYears(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)), SecurityAlgorithms.HmacSha256)
        );
        person.Token = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(person);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
    /// <summary>
    /// Autenticação com criptografia já ativada
    /// </summary>
    /// <param name="user">Objeto de autenticação de usuário</param>
    /// <returns>Informações de login e token de segurança, caso haja problema retorna a mensagem com o problema</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route("encrypt")]
    public ObjectResult PostEncripty([FromBody]ViewAuthentication user)
    {
      try
      {
        if (string.IsNullOrEmpty(user.Mail))
          return BadRequest("MSG1");
        if (string.IsNullOrEmpty(user.Password))
          return BadRequest("MSG2");

        bool authMarista = false;
        if (user.Mail.IndexOf("@maristas.org.br") != -1 || user.Mail.IndexOf("@pucrs.br") != -1)
          authMarista = true;

        ViewPerson person;
        if (authMarista)
          person = service.AuthenticationEncryptMaristas(user.Mail, user.Password);
        else
          person = service.AuthenticationEncrypt(user.Mail, user.Password);

        Claim[] claims = new[]
        {
        new Claim(ClaimTypes.Name, person.Name),
        new Claim(ClaimTypes.Hash, person.IdAccount),
        new Claim(ClaimTypes.Email, user.Mail),
        new Claim(ClaimTypes.NameIdentifier, person.NameAccount),
        new Claim(ClaimTypes.UserData, person.Contracts.FirstOrDefault().IdPerson)
      };

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "localhost",
            audience: "localhost",
            claims: claims,
            expires: DateTime.Now.AddYears(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)), SecurityAlgorithms.HmacSha256)
        );
        person.Token = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(person);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
    #endregion

    #region Authentication
    /// <summary>
    /// Autenticação de usuário
    /// </summary>
    /// <param name="userLogin">Objeto de autenticação de usuário</param>
    /// <returns>Informações de login e token de segurança, caso haja problema retorna a mensagem com o problema</returns>
    [AllowAnonymous]
    [HttpPost]
    public ObjectResult PostNewAuthentication([FromBody]ViewAuthentication userLogin)
    {
      try
      {
        return Ok(service.Authentication(userLogin));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    #endregion

  }
}
