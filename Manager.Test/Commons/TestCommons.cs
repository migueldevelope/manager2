using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;

namespace Manager.Test.Commons
{
  public abstract class TestCommons<TEntity> : IDisposable
  {
    public DataContext context;
    public ServiceGeneric<Person> service;
    public BaseUser baseUser;
    public IHttpContextAccessor contextAccessor;

    public void Dispose()
    {

      GC.SuppressFinalize(this);
    }

    protected void Init()
    {
      try
      {
        //this.context = new DataContext("mongodb://test:bti9010@10.0.0.14:27017/evaluations_test", "evaluations_test");
        //this.context = new DataContext("mongodb://homologacao:bti9010@10.0.0.15:27017/evaluations_homologacao", "evaluations_homologacao");
        this.context = new DataContext("mongodb://jmsoft:x14r53p5!a@10.0.0.14:27017/evaluations", "evaluations");
        this.service = new ServiceGeneric<Person>(context);

        var user = this.service.GetAuthentication(p => p.Mail == "suporte@jmsoft.com.br").FirstOrDefault();
        baseUser = new BaseUser()
        {
          NamePerson = user.Name,
          _idAccount = user._idAccount,
          _idPerson = user._id,
          Mail = user.Mail,
          NameAccount = "Suportt"
        };

        this.contextAccessor = new HttpContextAccessor();


        var users = contextAccessor.HttpContext;
        var claims = new[]
        {
          new Claim(ClaimTypes.Name, this.baseUser.NamePerson),
          new Claim(ClaimTypes.Hash, this.baseUser._idAccount.ToString()),
          new Claim(ClaimTypes.Email, this.baseUser.Mail),
          new Claim(ClaimTypes.NameIdentifier, this.baseUser.NameAccount),
          new Claim(ClaimTypes.UserData, this.baseUser._idPerson.ToString())
        };

        var claim = new ClaimsIdentity(claims);
        var item = new ClaimsPrincipal(claim);

        var http = new GenericHttpContext()
        {
          User = item
        }; ;
        contextAccessor.HttpContext = http;

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    protected void InitOffAccount()
    {
      this.context = new DataContext("mongodb://test:bti9010@10.0.0.14:27017/evaluations_test", "evaluations_test");
      //this.context = new DataContext("mongodb://homologacao:bti9010@10.0.0.15:27017/evaluations_homologacao", "evaluations_homologacao");
      //this.context = new DataContext("mongodb://jmsoft:x14r53p5!a@10.0.0.14:27017/evaluations", "evaluations");
    }

    public IList<ValidationResult> ValidateModel(object model)
    {
      var validationResults = new List<ValidationResult>();
      var ctx = new ValidationContext(model, null, null);
      Validator.TryValidateObject(model, ctx, validationResults, true);
      return validationResults;
    }

    public class GenericHttpContext : HttpContext
    {
      public GenericHttpContext()
        : base()
      {
      }

      public override IFeatureCollection Features => throw new NotImplementedException();

      public override HttpRequest Request => throw new NotImplementedException();

      public override HttpResponse Response => throw new NotImplementedException();

      public override ConnectionInfo Connection => throw new NotImplementedException();

      public override WebSocketManager WebSockets => throw new NotImplementedException();

      public override ClaimsPrincipal User { get; set; }
      public override IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public override IServiceProvider RequestServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public override CancellationToken RequestAborted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public override string TraceIdentifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public override ISession Session { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public override Microsoft.AspNetCore.Http.Authentication.AuthenticationManager Authentication => throw new NotImplementedException();
      public override void Abort()
      {
        throw new NotImplementedException();
      }
    }

  }
}
