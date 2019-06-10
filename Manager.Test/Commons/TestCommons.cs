using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Services.Specific;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manager.Test.Commons
{
  public abstract class TestCommons<TEntity> : IDisposable
  {
    public DataContext context;
    public BaseUser baseUser;
    public IServiceMaturity serviceMaturity;
    public IServiceControlQueue serviceControlQueue;
    string serviceBusConnectionString = "Endpoint=sb://analisa.servicebus.windows.net/;SharedAccessKeyName=analisahomologacao;SharedAccessKey=MS943jYNc9KmGP3HoIcL/eGhxhgIEAscB5R5as48Xik=;";
    string queueName = "analisahomologacao";

    public void Dispose()
    {
      GC.SuppressFinalize(this);
    }

    protected void InitOffAccount()
    {
      try
      {
        context = new DataContext("mongodb://test:bti9010@10.0.0.14:27017/evaluations_test", "evaluations_test");

        serviceMaturity = new ServiceMaturity(context);
        serviceControlQueue = new ServiceControlQueue(serviceBusConnectionString, queueName, serviceMaturity);

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
        context = new DataContext("mongodb://test:bti9010@10.0.0.14:27017/evaluations_test", "evaluations_test");

        serviceMaturity = new ServiceMaturity(context);
        serviceControlQueue = new ServiceControlQueue(serviceBusConnectionString, queueName, serviceMaturity);

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
