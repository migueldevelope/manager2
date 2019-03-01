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
          Message = "Olá <strong>{Person}</strong>,</br></br>O gestor <strong>{Requestor}</strong>, está solicitando que o(a) colaborador(a) <strong>{Employee}</strong> faça parte da equipe dele. </br></br>Você <a href='{Approved}'> aprova </a> ou <a href='{Disapproved}'> reprova </a> esta solicitação?",
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
      mailModelService._user = _user;

    }

    public void SetUser(BaseUser user)
    {
      _user = user;
      mailModelService._user = _user;

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

    public MailModel MonitoringSeq1_Person(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "monitoringseq1_person");
        if (model.Count() == 0)
          return DefaultMonitoringSeq1_Person(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel PlanSeq1(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "planseq1");
        if (model.Count() == 0)
          return DefaultPlanSeq1(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel PlanSeq1_Person(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "planseq1_person");
        if (model.Count() == 0)
          return DefaultPlanSeq1_Person(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel PlanSeq2(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "planseq2");
        if (model.Count() == 0)
          return DefaultPlanSeq2(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel PlanSeq2_Person(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "planseq2_person");
        if (model.Count() == 0)
          return DefaultPlanSeq2_Person(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel PlanSeq3(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "planseq3");
        if (model.Count() == 0)
          return DefaultPlanSeq3(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel PlanSeq3_Person(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "planseq3_person");
        if (model.Count() == 0)
          return DefaultPlanSeq3_Person(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel OnBoardingDisapproval(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "onboardingdisapproval");
        if (model.Count() == 0)
          return DefaultOnBoardingDisapproval(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel Certification(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "certification");
        if (model.Count() == 0)
          return DefaultCertification(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel CertificationDisapproval(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "certificationdisapproval");
        if (model.Count() == 0)
          return DefaultCertificationDisapproval(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel CertificationApproval(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "certificationapproval");
        if (model.Count() == 0)
          return DefaultCertificationApproval(path);
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

    public MailModel OnBoardingApprovalManager(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "onboardingapprovalmanager");
        if (model.Count() == 0)
          return DefaultOnBoardingApprovalManager(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel OnBoardingApprovalOccupation(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "onboardingapprovaloccupation");
        if (model.Count() == 0)
          return DefaultOnBoardingApprovalOccupation(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel OnBoardingApprovalManagerOccupation(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "onboardingapprovalmanageroccupation");
        if (model.Count() == 0)
          return DefaultOnBoardingApprovalManagerOccupation(path);
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
          return DefaultOnBoardingSeq5(path);
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


    public MailModel CheckpointResultDisapproved(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "checkpointresultdisapproved");
        if (model.Count() == 0)
          return DefaultCheckpointResultDisapproved(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel CheckpointResultPerson(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "checkpointresultperson");
        if (model.Count() == 0)
          return DefaultCheckpointResultPerson(path);
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

    public MailModel MonitoringApprovalManager(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "monitoringapprovalmanager");
        if (model.Count() == 0)
          return DefaultMonitoringApprovalManager(path);
        else
          return model.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MailModel MonitoringDisApproval(string path)
    {
      try
      {
        var model = mailModelService.GetAll(p => p.Name == "monitoringdisapproval");
        if (model.Count() == 0)
          return DefaultMonitoringDisapproval(path);
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
          Message = "Ola Recursos Humanos, <br>O resultado do check point para {Person} foi APROVADO.<br>Procure o gestor {Manager} para realizar os procedimentos de efetivação de período de experiência.<br>#VamosSerMaisFluidos",
          Subject = "Notificação de Decisão de Efetivação | Check point - Aprovado",
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

    public MailModel DefaultCheckpointResultDisapproved(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola Recursos Humanos,<br>O resultado do check point para {Person} foi REPROVADO.<br>Procure o gestor {Manager} para realizar os procedimentos de desligamento.<br>#VamosSerMaisFluidos",
          Subject = "Notificação de Decisão de Efetivação | Check point - Reprovado",
          Name = "checkpointresultdisapproved",
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

    public MailModel DefaultCheckpointResultPerson(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá {Person},<br>Parabéns!Você foi efetivado na {Company}. Isso é fruto do seu engajamento e alinhamento com nossa cultura e nossas competências essenciais.<br>Agora é seguir interessado em conhecer mais sobre as entregas do seu cargo e as competências que fazem parte do mesmo.<br>Fique atento às notificações do Analisa e aproveite o máximo de suas funcionalidades.<br>Estaremos na torcida para que sua carreira seja a mais fluida possível.<br>#VamosSerMaisFluidos",
          Subject = "Notificação de Decisão de Efetivação | Check point",
          Name = "checkpointresultperson",
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
          Message = "Ola <strong>{Person}</strong>,</br></br>É necessário que você acesse o sistema e realize uma aprovação do checkpointapproval.</br></br>Para acessar o sistema <a href='https://analisa.solutions/'>clique aqui</a>.",
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
          Message = "Ola <strong>{Person}</strong>,</br></br>É necessário que você acesse o sistema e realize uma aprovação do Pdi.</br></br>Para acessar o sistema <a href='https://analisa.solutions/'>clique aqui</a>.",
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

    public MailModel DefaultOnBoardingApprovalManager(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá {Manager},<br> O colaborador {Person} foi admitido na {Company} e inicio os registros de embarque na sua carreira.<br> De continuidade a este processo para gerar mais engajamento de sua equipe,e melhorar nossos processos de gestão.<br>Não perca tempo,  <a href ='https://analisa.solutions/' > clique aqui e confira </a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação da Aprovação de Embarque | OnBoarding",
          Name = "onboardingapprovalmanager",
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
          Message = "Olá {Person},<br>Bem - vindo à {Company}!<br>Estaremos aqui na torcida para que sua trajetória de carreira seja a melhor possível.<br>Seu gestor {Manager} acaba de fazer um registro do seu embarque no Analisa.<br>Não perca tempo, <a href='https://analisa.solutions/'>clique aqui e confira</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação para Continuar Embarque | OnBoarding",
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

    public MailModel DefaultOnBoardingApprovalManagerOccupation(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá {Manager}, <br>O colaborador {Person} foi promovido para o cargo {Occupation}, e iniciou os registros de embarque. <br> De continuidade a este processo para gerar mais engajamento de sua equipe, e melhorar nossos processos de gestão.<br><br>Para acessar o sistema <a href='https://analisa.solutions/'>clique aqui</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação para Continuar Embarque no Cargo | OnBoarding Occupation",
          Name = "onboardingapprovalmanageroccupation",
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

    public MailModel DefaultOnBoardingApprovalOccupation(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá {Person},<br>Bem - vindo ao seu novo cargo {Occupation}!<br>Estaremos aqui na torcida para que sua trajetória de carreira seja a melhor possível.<br>Seu gestor {Manager} acaba de fazer um registro do seu embarque no Analisa.<br>Não perca tempo, <a href='https://analisa.solutions/'>clique aqui e confira</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação da Aprovação de OnBoarding Troca de Cargo",
          Name = "onboardingapprovaloccupation",
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

    public MailModel DefaultOnBoardingDisapproval(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>O seu subordinado {Person} não concordou com o OnBoarding.</br></br>Para acessar o sistema <a href='https://analisa.solutions/'>clique aqui</a>.",
          Subject = "Aprovação de OnBoarding",
          Name = "onboardingdisapproval",
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

    public MailModel DefaultCertificationDisapproval(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>O {Person} enviou a acreditação.</br></br>Para acessar o sistema <a href='https://analisa.solutions/'>clique aqui</a>.",
          Subject = "Acreditação",
          Name = "certificationdisapproval",
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

    public MailModel DefaultCertification(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Guest}</strong>,</br></br>{Manager} enviou a acreditação do colaborador {Person}.</br></br>Para acessar o sistema <a href='https://analisa.solutions/'>clique aqui</a>.",
          Subject = "Acreditação",
          Name = "certification",
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

    public MailModel DefaultCertificationApproval(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>O {Person} enviou a acreditação.</br></br>Para acessar o sistema <a href='https://analisa.solutions/'>clique aqui</a>.",
          Subject = "Acreditação",
          Name = "certificationapproval",
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
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
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
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
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
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
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
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
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

    public MailModel DefaultPlanSeq1(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>Você tem um plano que irá vencer" +
          " em {Days} dias do seu subordinao {Person}.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
          Subject = "Plano à vencer",
          Name = "planseq1",
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

    public MailModel DefaultPlanSeq1_Person(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>Você tem um plano que irá vencer" +
          " em {Days} dias.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
          Subject = "Plano à vencer",
          Name = "planseq1_person",
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

    public MailModel DefaultPlanSeq2(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>Você tem um plano que irá vencer" +
          " hoje do seu subordinao {Person}.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
          Subject = "Plano vencendo",
          Name = "planseq2",
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

    public MailModel DefaultPlanSeq2_Person(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>Você tem um plano que irá vencer" +
          " hoje.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
          Subject = "Plano vencendo",
          Name = "planseq2_person",
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

    public MailModel DefaultPlanSeq3(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>Você tem um plano vencido" +
          " à {Days} dias do seu subordinao {Person}.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
          Subject = "Plano",
          Name = "planseq3",
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

    public MailModel DefaultPlanSeq3_Person(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,</br></br>Você tem um plano vencido" +
          " à {Days}.</br></br>Para acessar o sistema " +
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
          Subject = "Plano",
          Name = "planseq3_person",
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
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
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
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
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
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
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
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
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
          "<a href='https://analisa.solutions/'>clique aqui</a>.",
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



    public MailModel DefaultMonitoringDisapproval(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá {Manager},<br>Seu colaborador {Person} acaba de propor uma revisão nos registros de monitoramento.<br>Acesse agora o Analisa e veja este conteúdo.<br>Lembre - se: quanto mais engajado o colaborador estiver em suas ações de desenvolvimento, mais fluida será a carreira dele e melhores resultados você alcançará como gestor.<br><br>Para acessar o sistema Analisa <a href='https://analisa.solutions/'>clique aqui</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Revisão do Monitoramento | Monitoring",
          Name = "monitoringdisapproval",
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
          Message = "Olá {Person},<br>Seu gestor {Manager} acaba de registrar ações de monitoramento da sua performance.<br>Não perca tempo,<br>acesse agora o Analisa e veja este conteúdo.<br>Lembre - se: quanto mais engajamento nas suas ações de desenvolvimento, mais fluida será a sua carreira!<br>Para acessar o Analisa <a href='https://analisa.solutions/'>clique aqui</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Aprovação do Monitoramento | Monitoring",
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

    public MailModel DefaultMonitoringApprovalManager(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá {Manager},<br>Seu colaborador {Person} acaba de propor novos registros de monitoramento.<br>Acesse agora o Analisa e veja este conteúdo.<br>Lembre - se: quanto mais engajado o colaborador estiver em suas ações de desenvolvimento, mais fluida será a carreira dele e melhores resultados você alcançará como gestor.<br><br>Para acessar o sistema Analisa <a href='https://analisa.solutions/'>clique aqui</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Continuar o Monitoramento | Monitoring",
          Name = "monitoringapprovalmanager",
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

