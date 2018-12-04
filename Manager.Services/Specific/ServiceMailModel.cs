using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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
          Message = "<html><head><style>#customers {font-family: 'Segoe UI', Arial, Helvetica, sans-serif;border-collapse: collapse;width: 100%;}#customers td, #customers th{border: 1px solid #ddd;padding: 8px;}#customers tr:nth-child(even){background-color: #f2f2f2;}#customers th {padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #4598dc;color: white;}</style></head><body><table id='customers'><tr><td colspan='3' style='font-size: medium; font-weight: bold; text-align: center;'>Plano de Desenvolvimento Individual</td></tr><tr><td colspan='3'>Sr(a). <strong>{Person}</strong>, abaixo estão relacionados os planos de desenvolvimento dos seus funcionários que já venceram, ou que estão vencendo este mês, por favor acesse o sistema e atualize os registros.</tr><tr><th>Funcionário</th><th>Plano de Ação</th><th>Prazo</th></tr>{List}<tr align='center'><td colspan='3'><a href='https://analisa.solutions/'>Clique aqui para acessar o sistema</a></tr><tr align='center'><td colspan='3'>Obrigado por sua atenção.</tr></body></html>",
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

    public MailModel MonitoringSeq1(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "monitoringseq1");
        if (model.Count() == 0)
          return DefaultMonitoringSeq1(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
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


    public MailModel OnBoardingSeq1(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "onboardingseq1");
        if (model.Count() == 0)
          return DefaultOnBoardingSeq1(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel CheckpointSeq1(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "checkpointseq1");
        if (model.Count() == 0)
          return DefaultCheckpointSeq1(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel CheckpointSeq2(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "checkpointseq2");
        if (model.Count() == 0)
          return DefaultCheckpointSeq2(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel OnBoardingSeq2(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "onboardingseq2");
        if (model.Count() == 0)
          return DefaultOnBoardingSeq2(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel OnBoardingSeq3(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "onboardingseq3");
        if (model.Count() == 0)
          return DefaultOnBoardingSeq3(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel OnBoardingSeq4(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "onboardingseq4");
        if (model.Count() == 0)
          return DefaultOnBoardingSeq4(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel OnBoardingSeq5(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "onboardingseq5");
        if (model.Count() == 0)
          return DefaultOnBoardingSeq1(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel PlanApproval(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "planapproval");
        if (model.Count() == 0)
          return DefaultPlanApproval(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public MailModel CheckpointResult(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "checkpointresult");
        if (model.Count() == 0)
          return DefaultCheckpointResult(path);
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
    public MailModel DefaultCheckpointResult(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>Recursos Humanos</strong>,</br></br>O resultado do chekcpoint para <strong>{Person}</strong> foi {Result}.</br></br>Para acessar o sistema <a href='https://analisa.solutions/'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
          Subject = "Restulado do chekcpoint",
          Name = "checkpointresult",
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
    public MailModel DefaultCheckpointApproval(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Person}</strong>,</br></br>É necessário que você acesse o sistema e realize uma aprovação do checkpointapproval.</br></br>Para acessar o sistema <a href='https://analisa.solutions/'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
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
    public MailModel DefaultPlanApproval(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Person}</strong>,</br></br>É necessário que você acesse o sistema e realize uma aprovação do Pdi.</br></br>Para acessar o sistema <a href='https://analisa.solutions/'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
          Subject = "Aprovação de PDI",
          Name = "planapproval",
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
          Message = "Ola <strong>{Person}</strong>,</br></br>É necessário que você acesse o sistema e realize uma aprovação do OnBoarding.</br></br>Para acessar o sistema <a href='https://analisa.solutions/'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
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


    public MailModel DefaultCheckpointSeq1(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>É necessário que você acesse o sistema e realize o Checkpoint de " +
          "seu subordinado {Person} que irá vencer em {Days} dias.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
          Subject = "Checkpoint à vencer",
          Name = "checkpointseq1",
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

    public MailModel DefaultCheckpointSeq2(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>É necessário que você acesse o sistema e realize o Checkpoint de " +
          "seu subordinado {Person} que está atrasado em {Days} dias.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
          Subject = "Checkpoint atrasado",
          Name = "checkpointseq2",
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

    public MailModel DefaultMonitoringSeq1(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>Você está a {Days} dias sem " +
          "realizar um Monitoring do seu subordinao {Person}.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
          Subject = "Monitoring",
          Name = "monitoringseq1",
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

    public MailModel DefaultMonitoringSeq1_Person(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>Você está a {Days} dias sem " +
          "realizar um Monitoring.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
          Subject = "Monitoring",
          Name = "monitoringseq1_person",
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

    public MailModel DefaultOnBoardingSeq1(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>É necessário que você acesse o sistema e realize o OnBoarding de " +
          "seu subordinado {Person} que irá vencer em 30 dias.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
          Subject = "OnBoarding à vencer",
          Name = "onboardingseq1",
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

    public MailModel DefaultOnBoardingSeq2(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
            Message = "Ola <strong>{Manager}</strong>,</br></br>É necessário que você acesse o sistema e realize o OnBoarding de " +
          "seu subordinado {Person} que irá vencer em 5 dias.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
          Subject = "OnBoarding à vencer",
          Name = "onboardingseq2",
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

    public MailModel DefaultOnBoardingSeq3(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>É necessário que você acesse o sistema e realize o OnBoarding de " +
          "seu subordinado {Person} que irá vencer hoje.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
          Subject = "OnBoarding vencendo",
          Name = "onboardingseq3",
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

    public MailModel DefaultOnBoardingSeq4(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>É necessário que você acesse o sistema e realize o OnBoarding de " +
          "seu subordinado {Person} que está vencido a 5 dias.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
          Subject = "OnBoarding vencido",
          Name = "onboardingseq4",
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

    public MailModel DefaultOnBoardingSeq5(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>É necessário que você acesse o sistema e realize o OnBoarding de " +
          "seu subordinado {Person} que está vencido a 10 dias.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
          Subject = "OnBoarding vencido",
          Name = "onboardingseq5",
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
          Message = "Ola <strong>{Person}</strong>,</br></br>É necessário que você acesse o sistema e realize uma aprovação do Monitoring.</br></br>Para acessar o sistema <a href='https://analisa.solutions/'>clique aqui</a>.</br></br>Obrigado por sua atenção.",
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
          Message = "<html><head><style>#customers { font-family: 'Segoe UI', Arial, Helvetica, sans-serif; border-collapse: collapse; width: 100%; }#customers td, #customers th {border: 1px solid #ddd; padding: 8px; }#customers tr:nth-child(even){background-color: #f2f2f2;}#customers th{padding-top: 12px;padding-bottom: 12px; text-align: left; background-color: #4598dc; color: white;}</style></head><body><table id='customers'><tr><td colspan='2' style='font-size: medium; font-weight: bold; text-align: center;'>Acordos de Expectativas Pendentes</td></tr><tr><td colspan='2'>Sr(a). <strong>{Person}</strong>, abaixo estão relacionados os acordos de expectativas dos seus funcionários que ainda não tem registro de conclusão, por favor acesse o sistema e realize estes acordos. </tr><tr><th>Funcionário</th><th>Situação</th></tr>{List}<tr align='center'><td colspan='2'><a href='https://analisa.solutions/'>Clique aqui para acessar o sistema</a></tr><tr align='center'><td colspan='2'>Obrigado por sua atenção.</tr></body></html>",
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
          Message = "<html><head><style>body{font-family: 'Segoe UI', Arial, Helvetica, sans-serif; border-collapse: collapse; width: 100%;}</style></head><body>Olá <strong>{Person}</strong>,<br /><br />Existe um acordo de expectativa pendende de sua aprovação, por favor, <a href='https://analisa.solutions/'>clique aqui</a> para acessar o sistema.<br /><br />Obrigado por sua atenção.</body></html>",
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
    public string New(MailModel view)
    {
      try
      {
        mailModelService.Insert(view);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(MailModel view)
    {
      try
      {
        mailModelService.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Remove(string id)
    {
      try
      {
        var item = mailModelService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        mailModelService.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel Get(string id)
    {
      try
      {
        return mailModelService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<MailModel> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        try
        {
          int skip = (count * (page - 1));
          var detail = mailModelService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
          total = detail.Count();

          return detail.Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
        }
        catch (Exception e)
        {
          throw new ServiceException(_user, e, this._context);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
}

