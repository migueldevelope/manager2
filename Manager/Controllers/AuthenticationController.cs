using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Manager.Controllers
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

      ViewPerson person;
      var authMaristas = user.Mail == "@redemaristas.org.br" ? true : false;

      if (authMaristas)
        person = this.service.AuthenticationMaristas(user.Mail, user.Password);
      else
        person = this.service.Authentication(user.Mail, user.Password);



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
      var authMaristas = user.Mail == "@redemaristas.org.br" ? true : false;

      if (authMaristas)
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