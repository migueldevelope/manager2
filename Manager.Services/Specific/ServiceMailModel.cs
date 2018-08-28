using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Manager.Services.Specific
{

  public class ServiceMailModel : Repository<MailModel>, IServiceMailModel
  {
    private readonly ServiceGeneric<MailModel> mailModelService;


    public BaseUser user { get => _user; set => user = _user; }
    public ServiceMailModel(DataContext context)
      : base(context)
    {
      try
      {
        mailModelService = new ServiceGeneric<MailModel>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public MailModel AutoManager(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "automanager");
        if (model.Count() == 0)
          return DefaultAutoManager(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public MailModel DefaultAutoManager(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Person}</strong>,</br></br>O gestor <strong>{Requestor}</strong>, está solicitando que o(a) colaborador(a) <strong>{Employee}</strong> faça parte da equipe dele. </br></br>Você <a href='{Approved}'> aprova </a> ou <a href='{Disapproved}'> reprova </a> esta solicitação?</br></br>Obrigado por sua atenção.",
          Subject = "Solicitação de Aprovação de Auto Gestão",
          Name = "automanager",
          Link = string.Format("{0}evaluation_f/genericmessage", path)
        };
        // Insert
        mailModelService.Insert(model);
        return model;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public MailModel DevelopmentPlan(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "developmentplan");
        if (model.Count() == 0)
          return DefaultDevelopmentPlan(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public MailModel DefaultDevelopmentPlan(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "<html><head><style>#customers {font-family: 'Segoe UI', Arial, Helvetica, sans-serif;border-collapse: collapse;width: 100%;}#customers td, #customers th{border: 1px solid #ddd;padding: 8px;}#customers tr:nth-child(even){background-color: #f2f2f2;}#customers th {padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #4598dc;color: white;}</style></head><body><table id='customers'><tr><td colspan='3' style='font-size: medium; font-weight: bold; text-align: center;'>Plano de Desenvolvimento Individual</td></tr><tr><td colspan='3'>Sr(a). <strong>{Person}</strong>, abaixo estão relacionados os planos de desenvolvimento dos seus funcionários que já venceram, ou que estão vencendo este mês, por favor acesse o sistema e atualize os registros.</tr><tr><th>Funcionário</th><th>Plano de Ação</th><th>Prazo</th></tr>{List}<tr align='center'><td colspan='3'><a href='{Link}'>Clique aqui para acessar o sistema</a></tr><tr align='center'><td colspan='3'>Obrigado por sua atenção.</tr></body></html>",
          Subject = "Pendência do Plano de Desenvolvimento Individual (PDI)",
          Name = "developmentplan",
          Link = path
        };
        // Insert
        mailModelService.Insert(model);
        return model;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }   
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
    }
    public MailModel OnBoardingApproval(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "onboardingapproval");
        if (model.Count() == 0)
          return DefaultOnBoardingApproval(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public MailModel MonitoringApproval(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "monitoringapproval");
        if (model.Count() == 0)
          return DefaultMonitoringApproval(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public MailModel CheckpointApproval(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "checkpointapproval");
        if (model.Count() == 0)
          return DefaultCheckpointApproval(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel DefaultCheckpointApproval(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Person}</strong>,</br></br>É necessário que você acesse o sistema e realize uma aprovação do checkpointapproval.</br></br>Para acessar o sistema <a href='{Link}'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
          Subject = "Aprovação de checkpointapproval",
          Name = "checkpointapproval",
          Link = path
        };
        // Insert
        mailModelService.Insert(model);
        return model;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }


    public MailModel DefaultOnBoardingApproval(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Person}</strong>,</br></br>É necessário que você acesse o sistema e realize uma aprovação do OnBoarding.</br></br>Para acessar o sistema <a href='{Link}'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
          Subject = "Aprovação de OnBoarding",
          Name = "onboardingapproval",
          Link = path
        };
        // Insert
        mailModelService.Insert(model);
        return model;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public MailModel DefaultMonitoringApproval(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Person}</strong>,</br></br>É necessário que você acesse o sistema e realize uma aprovação do Monitoring.</br></br>Para acessar o sistema <a href='{Link}'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
          Subject = "Aprovação de Monitoring",
          Name = "monitoringapproval",
          Link = path
        };
        // Insert
        mailModelService.Insert(model);
        return model;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public MailModel OnBoardingPendingManager(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "expectationspendingmanager");
        if (model.Count() == 0)
          return DefaultExpectationsPendingManager(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public MailModel DefaultExpectationsPendingManager(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "<html><head><style>#customers { font-family: 'Segoe UI', Arial, Helvetica, sans-serif; border-collapse: collapse; width: 100%; }#customers td, #customers th {border: 1px solid #ddd; padding: 8px; }#customers tr:nth-child(even){background-color: #f2f2f2;}#customers th{padding-top: 12px;padding-bottom: 12px; text-align: left; background-color: #4598dc; color: white;}</style></head><body><table id='customers'><tr><td colspan='2' style='font-size: medium; font-weight: bold; text-align: center;'>Acordos de Expectativas Pendentes</td></tr><tr><td colspan='2'>Sr(a). <strong>{Person}</strong>, abaixo estão relacionados os acordos de expectativas dos seus funcionários que ainda não tem registro de conclusão, por favor acesse o sistema e realize estes acordos. </tr><tr><th>Funcionário</th><th>Situação</th></tr>{List}<tr align='center'><td colspan='2'><a href='{Link}'>Clique aqui para acessar o sistema</a></tr><tr align='center'><td colspan='2'>Obrigado por sua atenção.</tr></body></html>",
          Subject = "Pendência de Acordo de Expectativas",
          Name = "expectationspendingmanager",
          Link = path
        };
        // Insert
        mailModelService.Insert(model);
        return model;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public MailModel OnBoardingPendingEmployee(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "expectationspendingemployee");
        if (model.Count() == 0)
          return DefaultExpectationsPendingEmployee(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public MailModel DefaultExpectationsPendingEmployee(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "<html><head><style>body{font-family: 'Segoe UI', Arial, Helvetica, sans-serif; border-collapse: collapse; width: 100%;}</style></head><body>Olá <strong>{Person}</strong>,<br /><br />Existe um acordo de expectativa pendende de sua aprovação, por favor, <a href='{Link}'>clique aqui</a> para acessar o sistema.<br /><br />Obrigado por sua atenção.</body></html>",
          Subject = "Pendência de Acordo de Expectativas",
          Name = "expectationspendingemployee",
          Link = path
        };
        // Insert
        mailModelService.Insert(model);
        return model;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
  }
}
