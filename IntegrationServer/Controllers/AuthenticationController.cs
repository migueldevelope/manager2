using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IntegrationServer.Controllers
{
  [Produces("application/json")]
  [Route("authentication")]
  public class AuthenticationController : Controller
  {
    private readonly IServiceAuthentication service;
    private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

    public AuthenticationController(IServiceAuthentication _service)
    {
      service = _service;
    }

    [AllowAnonymous]
    [HttpPost]
    public ObjectResult Post([FromBody]ViewAuthentication user)
    {
      if (String.IsNullOrEmpty(user.Mail))
        return BadRequest("MSG1");
      if (String.IsNullOrEmpty(user.Password))
        return BadRequest("MSG2");

      ViewPerson person = this.service.Authentication(user.Mail, user.Password);


      Claim[] claims = new[]
      {
        new Claim(ClaimTypes.Name, person.Name),
        new Claim(ClaimTypes.Hash, person.IdAccount),
        new Claim(ClaimTypes.Email, user.Mail),
        new Claim(ClaimTypes.NameIdentifier, person.NameAccount),
        new Claim(ClaimTypes.UserData, person.IdPerson)
      };

      SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
      SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      JwtSecurityToken token = new JwtSecurityToken(
          issuer: "localhost",
          audience: "localhost",
          claims: claims,
          expires: DateTime.Now.AddYears(10),
          signingCredentials: creds
      );

      string tokenId = new JwtSecurityTokenHandler().WriteToken(token);
      person.Token = tokenId;

      return Ok(person);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("encrypt")]
    public ObjectResult PostEncripty([FromBody]ViewAuthentication user)
    {
      if (String.IsNullOrEmpty(user.Mail))
        return BadRequest("MSG1");
      if (String.IsNullOrEmpty(user.Password))
        return BadRequest("MSG2");

      ViewPerson person;
      var authMaristas = false;
      var authPUC = false;
      try
      {
        authMaristas = user.Mail.Substring(user.Mail.IndexOf("@"), user.Mail.Length - user.Mail.IndexOf("@")) == "@maristas.org.br" ? true : false;
        authPUC = user.Mail.Substring(user.Mail.IndexOf("@"), user.Mail.Length - user.Mail.IndexOf("@")) == "@pucrs.br" ? true : false;
      }
      catch (Exception)
      {

      }

      if ((authMaristas) || (authPUC))
        person = this.service.AuthenticationEncryptMaristas(user.Mail, user.Password);
      else
        person = this.service.AuthenticationEncrypt(user.Mail, user.Password);

      var claims = new[]
      {
        new Claim(ClaimTypes.Name, person.Name),
        new Claim(ClaimTypes.Hash, person.IdAccount),
        new Claim(ClaimTypes.Email, user.Mail),
        new Claim(ClaimTypes.NameIdentifier, person.NameAccount),
        new Claim(ClaimTypes.UserData, person.IdPerson)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer: "localhost",
          audience: "localhost",
          claims: claims,
          expires: DateTime.Now.AddYears(1),
          signingCredentials: creds
      );

      var tokenId = new JwtSecurityTokenHandler().WriteToken(token);
      person.Token = tokenId;

      return Ok(person);
    }
  }
}