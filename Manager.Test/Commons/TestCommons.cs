using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Data;
using Manager.Services.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manager.Test.Commons
{
  public abstract class TestCommons<TEntity> : IDisposable
  {
    public DataContext context;
    public BaseUser baseUser;

    public void Dispose()
    {
      GC.SuppressFinalize(this);
    }

    protected void InitAccount()
    {
      try
      {
        context = new DataContext("mongodb://analisa_test:bti9010@10.0.0.15:27017/analisa_test", "analisa_test");
        // Limpar todas as collections
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    protected void Init()
    {
      try
      {
        context = new DataContext("mongodb://analisa_test:bti9010@10.0.0.15:27017/analisa_test", "analisa_test");
        // Limpar todas as collections

        // Buscar a pessoa de teste
        ServiceGeneric<Person> service = new ServiceGeneric<Person>(context);
        Person user = service.GetFreeNewVersion(p => p.User.Mail == "suporte@jmsoft.com.br").Result;
        baseUser = new BaseUser()
        {
          NamePerson = user.User.Name,
          _idAccount = user._idAccount,
          _idUser = user._id,
          Mail = user.User.Mail,
          NameAccount = "Suport"
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public IList<ValidationResult> ValidateModel(object model)
    {
      List<ValidationResult> validationResults = new List<ValidationResult>();
      ValidationContext ctx = new ValidationContext(model, null, null);
      Validator.TryValidateObject(model, ctx, validationResults, true);
      return validationResults;
    }
  }
}
