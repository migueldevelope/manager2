using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{

  public class ServiceMailModel : Repository<MailModel>, IServiceMailModel
  {
    private readonly ServiceGeneric<MailModel> serviceMailModel;

    #region Constructor
    public ServiceMailModel(DataContext context) : base(context)
    {
      try
      {
        serviceMailModel = new ServiceGeneric<MailModel>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceMailModel._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceMailModel._user = _user;
    }
    #endregion

    #region MailModel
    public List<ViewListMailModel> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListMailModel> detail = serviceMailModel.GetAllNewVersion(p => p.Subject.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Subject").Result.Select(p => p.GetViewList()).ToList();

        total = serviceMailModel.CountNewVersion(p => p.Subject.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string New(ViewCrudMailModel view)
    {
      try
      {
        var mailModel = new MailModel()
        {
          Name = view.Name,
          Link = view.Link,
          Message = view.Message,
          StatusMail = view.StatusMail,
          Subject = view.Subject,
          TypeFrequence = view.TypeFrequence,
          Weekly = view.Weekly,
          Day = view.Day,
          TypeMailModel = view.TypeMailModel
        };
        serviceMailModel.InsertNewVersion(mailModel).Wait();
        return "Mail model added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudMailModel view)
    {
      try
      {
        var mailModel = serviceMailModel.GetNewVersion(p => p._id == view._id).Result;
        // Não atualizar o nome e o link
        //mailModel.Name = view.Name;
        //mailModel.Link = view.Link;
        mailModel.Message = view.Message;
        mailModel.StatusMail = view.StatusMail;
        mailModel.Subject = view.Subject;
        mailModel.Link = view.Link;
        mailModel.TypeFrequence = view.TypeFrequence;
        mailModel.Weekly = view.Weekly;
        mailModel.Day = view.Day;
        mailModel.TypeMailModel = view.TypeMailModel;

        serviceMailModel.Update(mailModel, null).Wait();
        return "Mail model altered!";
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
        var item = serviceMailModel.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceMailModel.Update(item, null).Wait();
        return "Mail model deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudMailModel Get(string id)
    {
      try
      {
        var view = serviceMailModel.GetNewVersion(p => p._id == id).Result.GetViewCrud();
        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Auto manager mail
    public MailModel AutoManager(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "automanager").Result;
        return model ?? AutoManagerDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel AutoManagerDefault(string path)
    {
      try
      {
        var model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Manager}</strong>,<br><br>O gestor <strong>{Requestor}</strong>, está solicitando que o(a) colaborador(a) <strong>{Employee}</strong> faça parte da equipe dele. <br><br>Você <a href='{Approved}'> aprova </a> ou <a href='{Disapproved}'> reprova </a> esta solicitação?",
          Subject = "Notificação de Auto Gestão - Pedido de Autorização",
          Name = "automanager",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Certification Mail
    public MailModel Certification(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "certification").Result;
        return model ?? CertificationDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel CertificationDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Guest}</strong>,<br><br>O(a) gestor(a) <strong>{Manager}</strong> enviou uma solicitação para aprovação de acreditação do(a) colaborador(a) <strong>{Person}</strong>.<br>Ajude a encontrar as pessoas que são referências na nossa empresa.<br><br>Para acessar o sistema <a href='{Link}'>clique aqui</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação para aprovação de Acreditação | Accretitation Skill",
          Name = "certification",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel CertificationApproval(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "certificationapproval").Result;
        return model ?? CertificationApprovalDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel CertificationApprovalDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Manager}</strong>,<br><br>A acreditação da competência <strong>{Skill}</strong> do(a) colaborador(a) <strong>{Person}</strong> foi APROVADA.<br>Enviamos um e-mail parabenizando o(a) colaborador(a) mas é muito importante que você comemore com ele(a).<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Acreditação | Accretitation Skill - APROVADA",
          Name = "certificationapproval",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel CertificationApprovalPerson(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "certificationapprovalperson").Result;
        return model ?? CertificationApprovalPersonDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel CertificationApprovalPersonDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Person}</strong>,<br><br>Foi concedido a você a acreditação da competência <strong>{Skill}</strong>.<br>Parabéns por este feito glorioso, e continue buscando outras acreditações.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Acreditação | Accretitation Skill - APROVADA",
          Name = "certificationapprovalperson",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel CertificationDisapproval(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "certificationdisapproval").Result;
        return model ?? CertificationDisapprovalDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel CertificationDisapprovalDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Manager}</strong>,<br><br>A acreditação da competência <strong>{Skill}</strong> do(a) colaborador(a) <strong>{Person}</strong> foi REPROVADA.<br>Não se aboreça, insentive seu colaborador a se aperfeiçoar mais e solicite novamente daqui algum tempo.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Acreditação | Accretitation Skill - REPROVADA",
          Name = "certificationdisapproval",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Checkpoint Mail
    public MailModel CheckpointResultApproved(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "checkpointresult").Result;
        return model ?? CheckpointResultApprovedDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel CheckpointResultApprovedDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>Recursos Humanos</strong>,<br><br>O resultado do check point para o(a) colaborador(a) <strong>{Person}</strong> foi APROVADO.<br>Procure o gestor <strong>{Manager}</strong> para realizar os procedimentos de efetivação de período de experiência.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Decisão de Efetivação | Check point - APROVADO",
          Name = "checkpointresult",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel CheckpointResultDisapproved(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "checkpointresultdisapproved").Result;
        return model ?? CheckpointResultDisapprovedDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel CheckpointResultDisapprovedDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>Recursos Humanos</strong>,<br><br>O resultado do check point para <strong>{Person}</strong> foi REPROVADO.<br>Procure o gestor <strong>{Manager}</strong> para realizar os procedimentos de desligamento.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Decisão de Efetivação | Check point - REPROVADO",
          Name = "checkpointresultdisapproved",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel CheckpointResultPerson(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "checkpointresultperson").Result;
        return model ?? CheckpointResultPersonDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel CheckpointResultPersonDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Person}</strong>,<br><br>Parabéns! Você foi efetivado na <strong>{Company}</strong>. Isso é fruto do seu engajamento e alinhamento com nossa cultura e nossas competências essenciais.<br>Agora é seguir interessado em conhecer mais sobre as entregas do seu cargo e as competências que fazem parte do mesmo.<br>Fique atento às notificações do Fluid e aproveite o máximo de suas funcionalidades.<br>Estaremos na torcida para que sua carreira seja a mais fluida possível.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Decisão de Efetivação | Checkpoint",
          Name = "checkpointresultperson",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    // Serviço de Notificação
    public MailModel CheckpointManagerDeadline(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "checkpointmanagerdeadline").Result;
        return model ?? CheckpointManagerDeadlineDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel CheckpointManagerDeadlineDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = string.Concat("Olá <strong>{Manager}</strong>,<br><br>",
                                  "Identificamos que alguns colaboradores da sua equipe estão na jornada de Definição de Efetivação | Check-Point. Você precisa ficar atento para não perder os prazos da sua decisão de efetivação (ou não) deste novo colaborador.<br><br>",
                                  "Coloque na sua agenda, faça de acordo com sua disponibilidade, mas lembre que esta ação é obrigatória dentro desta jornada!.<br><br>",
                                  "{LIST1}{LIST2}{LIST3}{LIST4}{LIST5}",
                                  "Para acessar o sistema <a href='{Link}'>clique aqui</a>.<br><br>",
                                  "#VamosSerMaisFluidos<br>"),
          Subject = "Notificação de Vencimentos da Jornada de Definição de Efetivação | Check-Point",
          Name = "checkpointmanagerdeadline",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Goals
    public MailModel GoalsApproval(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "goalsapproval").Result;
        return model ?? GoalsApprovalDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel GoalsApprovalDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Person}</strong>,<br>Seu gestor <strong>{Manager}</strong> acaba de registrar objetivos.<br>Não perca tempo, acesse agora o Fluid e veja este conteúdo.<br>Lembre-se: quanto mais engajamento nas suas ações de desenvolvimento, mais fluida será a sua carreira!<br><br>Para acessar o Fluid <a href='{Link}'>clique aqui</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Aprovação dos Objetivos | Objetivos",
          Name = "goalsapproval",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel GoalsApprovalManager(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "goalsapprovalmanager").Result;
        return model ?? GoalsApprovalManagerDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel GoalsApprovalManagerDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Manager}</strong>,<br><br>Seu colaborador <strong>{Person}</strong> acaba de propor novos objetivos.<br>Acesse agora o Fluid e veja este conteúdo.<br>Lembre-se: quanto mais engajado o colaborador estiver em suas ações de desenvolvimento, mais fluida será a carreira dele e melhores resultados você alcançará como gestor.<br><br>Para acessar o sistema Fluid <a href='{Link}'>clique aqui</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Continuar os Objetivos | Objetivos",
          Name = "goalsapprovalmanager",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel GoalsDisapproval(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "goalsdisapproval").Result;
        return model ?? GoalsDisapprovalDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel GoalsDisapprovalDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Manager}</strong>,<br><br>Seu colaborador <strong>{Person}</strong> acaba de propor uma revisão nos objetivos.<br>Acesse agora o Fluid e veja este conteúdo.<br>Lembre-se: quanto mais engajado o colaborador estiver em suas ações de desenvolvimento, mais fluida será a carreira dele e melhores resultados você alcançará como gestor.<br><br>Para acessar o sistema Fluid <a href='{Link}'>clique aqui</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Revisão dos Objetivos | Objetivos",
          Name = "goalsdisapproval",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Monitoring Mail

    public MailModel MonitoringApproval(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "monitoringapproval").Result;
        return model ?? MonitoringApprovalDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private MailModel RecommendationDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Person}</strong>,<br>Você acaba de receber o reconhecimento {Type} que foi enviado por {Manager}.<br>É muito bom ser reconhecido, né? Parabéns!<br>Não perca tempo, acesse agora o Fluid e veja este conteúdo!<br>Para acessar o Fluid <a href='{Link}'>clique aqui</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Reconhecimento | Recommendation",
          Name = "recommendation",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public MailModel Recommendation(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "recommendation").Result;
        return model ?? RecommendationDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel MonitoringApprovalDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Person}</strong>,<br>Seu gestor <strong>{Manager}</strong> acaba de registrar ações de monitoramento da sua performance.<br>Não perca tempo, acesse agora o Fluid e veja este conteúdo.<br>Lembre-se: quanto mais engajamento nas suas ações de desenvolvimento, mais fluida será a sua carreira!<br><br>Para acessar o Fluid <a href='{Link}'>clique aqui</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Aprovação do Monitoramento | Monitoring",
          Name = "monitoringapproval",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public MailModel MonitoringApprovalManager(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "monitoringapprovalmanager").Result;
        return model ?? MonitoringApprovalManagerDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel MonitoringApprovalManagerDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Manager}</strong>,<br><br>Seu colaborador <strong>{Person}</strong> acaba de propor novos registros de monitoramento.<br>Acesse agora o Fluid e veja este conteúdo.<br>Lembre-se: quanto mais engajado o colaborador estiver em suas ações de desenvolvimento, mais fluida será a carreira dele e melhores resultados você alcançará como gestor.<br><br>Para acessar o sistema Fluid <a href='{Link}'>clique aqui</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Continuar o Monitoramento | Monitoring",
          Name = "monitoringapprovalmanager",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel MonitoringDisapproval(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "monitoringdisapproval").Result;
        return model ?? MonitoringDisapprovalDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel MonitoringDisapprovalDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Manager}</strong>,<br><br>Seu colaborador <strong>{Person}</strong> acaba de propor uma revisão nos registros de monitoramento.<br>Acesse agora o Fluid e veja este conteúdo.<br>Lembre-se: quanto mais engajado o colaborador estiver em suas ações de desenvolvimento, mais fluida será a carreira dele e melhores resultados você alcançará como gestor.<br><br>Para acessar o sistema Fluid <a href='{Link}'>clique aqui</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Revisão do Monitoramento | Monitoring",
          Name = "monitoringdisapproval",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    // Serviço de Notificação
    public MailModel MonitoringManagerDeadline(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "monitoringmanagerdeadline").Result;
        return model ?? MonitoringManagerDeadlineDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private MailModel OnboardingEmployeeDeadlineDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = string.Concat("Olá <strong>{Person}</strong>,<br><br>",
                                  "Seu gestor <strong>{Manager}</strong> concluiu seu embarque.<br><br>Acesse a plataforma, para aceitar o processo de embarque, e para conhecer outras funcionalidades muito legais.<br><br>",
                                  "Para acessar a plataforma <a href='{Link}'>clique aqui</a>.<br><br>",
                                  "#VamosSerMaisFluidos<br>"),
          Subject = "Notificação de Embarque | Onboarding",
          Name = "onboardingemployeedeadline",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private MailModel MonitoringManagerDeadlineDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = string.Concat("Olá <strong>{Manager}</strong>,<br><br>",
                                  "Identificamos que alguns colaboradores da sua equipe estão na Jornada de Monitoramento | Monitoring, porém não tiveram interação com você há mais de {DAYS} dias.<br><br>",
                                  "Para que os movimentos de carreira sejam mais fluidos e o engajamento da sua equipe seja cada vez maior, não perca tempo,... dá uma passada lá na plataforma e retome suas conversas e combinações!<br><br>",
                                  "{LIST1}",
                                  "Para acessar a plataforma <a href='{Link}'>clique aqui</a>.<br><br>",
                                  "#VamosSerMaisFluidos<br>"),
          Subject = "Notificação de Monitoramento | Monitoring",
          Name = "monitoringmanagerdeadline",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel MonitoringDeadline(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "monitoringdeadline").Result;
        return model ?? MonitoringDeadlineDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel MonitoringDeadlineDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = string.Concat("Olá <strong>{Person}</strong>,<br><br>",
                                  "Identificamos que você está na Jordana de Monitoramento | Monitoring, mas não teve nenhuma interação com seu gestor há mais de {DAYS} dias.<br><br>",
                                  "Seja protagonista de sua carreira e retome agora mesmo o seu monitoramento! <br><br>",
                                  "Faça isso agora! Acesse a plataforma <a href='{Link}'>clique aqui</a>.<br><br>",
                                  "#VamosSerMaisFluidos<br>"),
          Subject = "Notificação de Monitoramento | Monitoring",
          Name = "monitoringdeadline",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region OnBoarding Mail
    public MailModel OnBoardingApproval(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "onboardingapproval").Result;
        return model ?? OnBoardingApprovalDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel OnBoardingApprovalDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Person}</strong>,<br><br>Bem - vindo à <strong>{Company}</strong>!<br>Estaremos aqui na torcida para que sua trajetória de carreira seja a melhor possível.<br>Seu gestor <strong>{Manager}</strong> acaba de fazer um registro do seu embarque no Fluid.<br><br>Não perca tempo, <a href='{Link}'>clique aqui e confira</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação para Continuar Embarque | OnBoarding",
          Name = "onboardingapproval",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel OnBoardingApprovalManager(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "onboardingapprovalmanager").Result;
        return model ?? OnBoardingApprovalManagerDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel OnBoardingApprovalManagerDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Manager}</strong>,<br><br>O colaborador <strong>{Person}</strong> foi admitido na <strong>{Company}</strong> e iniciou os registros de embarque na sua carreira.<br>De continuidade a este processo para gerar mais engajamento de sua equipe,e melhorar nossos processos de gestão.<br><br>Não perca tempo, <a href ='{Link}' > clique aqui e confira </a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação da Aprovação de Embarque | OnBoarding",
          Name = "onboardingapprovalmanager",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel OnBoardingDisapproval(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "onboardingdisapproval").Result;
        return model ?? OnBoardingDisapprovalDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel OnBoardingDisapprovalDefault(string path)
    {
      // Ok
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Manager}</strong>,<br><br>O seu colaborador <strong>{Person}</strong> fez alguns comentários no encerramento do embarque.<br><br>Não deixe para mais tarde, <a href='{Link}'> clique aqui </a> e olhe os comentários que foram redigidos.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Revisão de Embarque | OnBoarding",
          Name = "onboardingdisapproval",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel OnBoardingOccupationApproval(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "onboardingapprovaloccupation").Result;
        return model ?? OnBoardingOccupationApprovalDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel OnBoardingOccupationApprovalDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Person}</strong>,<br><br>Bem - vindo ao seu novo cargo <strong>{Occupation}</strong>!<br>Estaremos aqui na torcida para que sua trajetória de carreira seja a melhor possível.<br>Seu gestor <strong>{Manager}<strong> acaba de fazer um registro do seu embarque no Fluid.<br><br>Não perca tempo, <a href='{Link}'>clique aqui e confira</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação da Aprovação de OnBoarding Troca de Cargo",
          Name = "onboardingapprovaloccupation",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel OnBoardingOccupationApprovalManager(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "onboardingapprovalmanageroccupation").Result;
        return model ?? OnBoardingOccupationApprovalManagerDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel OnBoardingOccupationApprovalManagerDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Manager}</strong>,<br><br>O colaborador <strong>{Person}</strong> foi promovido para o cargo <strong>{Occupation}</strong>, e iniciou os registros de embarque.<br>De continuidade a este processo para gerar mais engajamento de sua equipe, e melhorar nossos processos de gestão.<br><br>Para acessar o sistema <a href='{Link}'>clique aqui</a>.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação para Continuar Embarque no Cargo | OnBoarding Occupation",
          Name = "onboardingapprovalmanageroccupation",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel OnBoardingOccupationDisapproval(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "onboardingdisapprovaloccupation").Result;
        return model ?? OnBoardingOccupationDisapprovalDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel OnBoardingOccupationDisapprovalDefault(string path)
    {
      // Ok
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Olá <strong>{Manager}</strong>,<br><br>O seu colaborador <strong>{Person}</strong> fez alguns comentários no encerramento do embarque para troca de cargo.<br><br>Não deixe para mais tarde, <a href='{Link}'>clique aqui</a> e olhe os comentários que foram redigidos.<br><br>#VamosSerMaisFluidos",
          Subject = "Notificação de Revisão de Embarque | OnBoarding",
          Name = "onboardingdisapprovaloccupation",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    // Serviço de Notificação
    public MailModel OnboardingAdmission(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "onboardingadmission").Result;
        return model ?? OnboardingAdmissionDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel OnboardingAdmissionDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = string.Concat("Olá <strong>{Manager}</strong>,<br><br>",
                                  "O colaborador <strong>{Person}</strong> acaba de ser admitido. Recomendamos que, após o seu período inicial de integração, você apresente a ele as funcionalidades de nossa plataforma e promova uma conversa sobre o <strong>Mapa de Competências</strong>.<br><br>",
                                  "Esta conversa é fundamental para que você faça um acordo de expectativas em relação ao que dele é esperado no exercício do cargo.<br><br>",
                                  "O ideal é que você possa realizar este acordo antes dos primeiros 30(trinta) dias de empresa.<br><br>",
                                  "#VamosSerMaisFluidos<br>"),
          Subject = "Notificação de Admissão de Novo Colaborador, Jornada de Embarque | Onboarding",
          Name = "onboardingadmission",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel OnboardingManagerDeadline(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "onboardingmanagerdeadline").Result;
        return model ?? OnboardingManagerDeadlineDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public MailModel OnboardingEmployeeDeadline(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "onboardingemployeedeadline").Result;
        return model ?? OnboardingEmployeeDeadlineDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private MailModel OnboardingManagerDeadlineDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = string.Concat("Olá <strong>{Manager}</strong>,<br><br>",
                                  "Identificamos que alguns colaboradores da sua equipe estão na jornada Embarque | Onboarding, mas não tiveram ainda o registro de sua conversa de apresentação do Mapa de Competências.<br><br>",
                                  "Vai lá,... faça acontecer este acordo de expectativas, que será fundamental para um ótimo desenvolvimento de carreira do seu novo colaborador.<br><br>",
                                  "{LIST1}{LIST2}{LIST3}{LIST4}",
                                  "Para acessar a plataforma <a href='{Link}'>clique aqui</a>.<br><br>",
                                  "#VamosSerMaisFluidos<br>"),
          Subject = "Notificação de Vencimento da Jornada de Embarque | Onboarding",
          Name = "onboardingmanagerdeadline",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Plan Mail
    public MailModel PlanApproval(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "planapproval").Result;
        return model ?? PlanApprovalDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel PlanApprovalDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = "Ola <strong>{Manager}</strong>,<br><br>É necessário que você acesse o sistema e realize uma aprovação do Plano de Ação do colaborador(a) {Person}.<br><br>Para acessar o sistema <a href='{Link}'>clique aqui</a>.",
          Subject = "Notificação de Plano de Ação | Action Plan",
          Name = "planapproval",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    // Serviço de Notificação
    public MailModel PlanManagerDeadline(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "actionplanmanagerdeadline").Result;
        return model ?? PlanManagerDeadlineDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private MailModel PlanManagerDeadlineDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = string.Concat("Olá <strong>{Manager}</strong>,<br><br>",
                                  "Identificamos que existem ações de desenvolvimento da sua equipe com situações que requerem sua atenção!<br><br>",
                                  "Verifique o status destas ações, pois isto fomenta o círculo virtuoso das carreiras fluidas.<br><br>",
                                  "{LIST1}{LIST2}{LIST3}{LIST4}{LIST5}",
                                  "Para acessar a plataforma <a href='{Link}'>clique aqui</a>.<br><br>",
                                  "#VamosSerMaisFluidos<br>"),
          Subject = "Notificação de Ação de Desenvolvimento | Action Plans",
          Name = "actionplanmanagerdeadline",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public MailModel PlanDeadline(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "actionplandeadline").Result;
        return model ?? PlanDeadlineDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public MailModel BirthCompany(string path)
    {
      try
      {
        MailModel model = serviceMailModel.GetNewVersion(p => p.Name == "congratulatecompanyanniversary").Result;
        return model ?? BirthCompanyDefault(path);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private MailModel PlanDeadlineDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = string.Concat("Olá <strong>{Person}</strong>,<br><br>",
                                  "Identificamos que existem ações de desenvolvimento com situações que requerem a sua atenção.<br><br>",
                                  "Verifique o status destas ações de desenvolvimento, pois ela são fundamentais para o seu desenvolvimento e para a fluidez de sua carreira!<br><br>",
                                  "{LIST1}{LIST2}{LIST3}{LIST4}{LIST5}",
                                  "Para acessar a plataforma <a href='{Link}'>clique aqui</a>.<br><br>",
                                  "#VamosSerMaisFluidos<br>"),
          Subject = "Notificação de Ação de Desenvolvimento | Action Plans do colaborador",
          Name = "actionplandeadline",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private MailModel BirthCompanyDefault(string path)
    {
      try
      {
        MailModel model = new MailModel
        {
          Status = EnumStatus.Enabled,
          Message = string.Concat(@"<p style=""text-align: center; margin: 10px 0px; padding: 0px; text-size-adjust: 100%; color: rgb(32, 32, 32); font-family: Helvetica; font-size: 16px; line-height: 24px;""><img src=""https://Fluidfluid.blob.core.windows.net/5b7b01c0d6869299519ddf13/5dd400e0a388c4000175f5b1.png""><br></p><p style=""text-align: center; margin: 10px 0px; padding: 0px; text-size-adjust: 100%; color: rgb(32, 32, 32); font-family: Helvetica; font-size: 16px; line-height: 24px;""><strong style=""background-color: transparent;""></strong></p><font face=""Helvetica""><div style=""color: rgb(32, 32, 32); text-align: center;""><span style=""font-size: 12px;""></span><span style=""font-size: 16px;""></span></div><div style=""color: rgb(32, 32, 32); text-align: center;""><span style=""font-size: 16px;""></span><span style=""font-size: 18px;"">Olá {Person}</span><span style=""font-size: 18px; background-color: transparent;"">!&nbsp;</span></div></font><font face=""Helvetica""><div style=""color: rgb(32, 32, 32); text-align: center;""><span style=""font-size: 18px;"">Parabéns pelo seu aniversário de empresa/instituição!&nbsp;</span></div><div style=""color: rgb(32, 32, 32); text-align: center;""><span style=""font-size: 18px;"">Foi um ano de muita dedicação, superação de desafios e novas conquistas :)</span></div><div style=""color: rgb(32, 32, 32); text-align: center;""><span style=""font-size: 18px;""><br></span></div><div style=""color: rgb(32, 32, 32); text-align: center;""><span style=""font-size: 18px;"">Desejamos ainda mais&nbsp;sucesso na sua jornada fluída de carreira.</span></div><div style=""color: rgb(32, 32, 32); text-align: center;""><span style=""font-size: 18px;""><br></span></div><div style=""color: rgb(32, 32, 32); text-align: center;""><span style=""font-size: 18px;"">Grande abraço,</span></div><div style=""color: rgb(32, 32, 32); text-align: center;""><span style=""font-size: 18px;"">Equipe de Sucesso do Cliente | Fluid</span><span style=""font-size: 16px; background-color: transparent;""><br></span></div><div style=""color: rgb(32, 32, 32); text-align: center;""><span style=""font-size: 16px;""><br></span></div><div style=""color: rgb(32, 32, 32); text-align: center;""><span style=""font-size: 16px;""><br></span></div><div style=""text-align: center;""><b style=""""><a href=""https://www.facebook.com/Fluid.fluid.careers/"" target=""_blank"" style=""""><font color=""#537bbe""></font></a><font color=""#137bb8"" style=""font-size: 0.875rem;""><a href=""https://www.linkedin.com/company/Fluid-fluid-careers/"" target=""_blank"" style=""font-size: 0.875rem;""></a></font></b></div></font><font face=""Helvetica""><div style=""text-align: center;""><a href=""https://www.facebook.com/Fluid.fluid.careers/"" target=""_blank"" style=""font-size: 0.875rem; font-family: Roboto, Helvetica, Arial, sans-serif; text-align: left;"">Facebook&nbsp;</a>|&nbsp;&nbsp;<a href=""https://Fluid.solutions/"" target=""_blank"" style=""font-size: 0.875rem; font-family: Roboto, Helvetica, Arial, sans-serif; text-align: left;"">Fluid</a>&nbsp;<span style=""background-color: transparent; font-size: 0.875rem;"">|&nbsp;&nbsp;</span><a href=""https://www.instagram.com/Fluid.fluidcareers/"" target=""_blank"" style=""font-size: 0.875rem; font-family: Roboto, Helvetica, Arial, sans-serif; text-align: left;"">Instagram</a>&nbsp;<span style=""background-color: transparent; font-size: 0.875rem;"">|&nbsp;&nbsp;</span><a href=""https://www.youtube.com/channel/UCekWe2ZrjMCC_JsvxAISRaA"" target=""_blank"" style=""font-size: 0.875rem; font-family: Roboto, Helvetica, Arial, sans-serif; text-align: left;"">YouTube</a>&nbsp;<span style=""background-color: transparent; font-size: 0.875rem;"">|&nbsp;</span><a href=""https://www.linkedin.com/company/Fluid-fluid-careers/"" target=""_blank"" style=""font-size: 0.875rem; font-family: Roboto, Helvetica, Arial, sans-serif; text-align: left;"">Linkedin</a></div></font><font face=""Helvetica""><div style=""text-align: center;""><br></div><div style=""text-align: center;""><br></div><div style=""text-align: center;""><span style=""font-size: 12px;"">Este e-mail foi enviado pela equipe de Sucesso do Cliente | Fluid</span></div><div style=""text-align: center;""><span style=""font-size: 12px;"">suporte@fluidstate.com.br</span></div><div style=""text-align: center;""><span style=""font-size: 12px;"">Rua 24 de Outubro,1299 - Cj 401 | Bairro Moinhos de Vento | Porto Alegre / RS | CEP: 90510-003&nbsp;</span></div><div style=""text-align: center;""><span style=""font-size: 12px;"">Copyright © 2019 Fluid. Todos os direitos reservados.</span><b style=""""><br></b></div></font><p></p><div style=""text-align: center;""></div><div style=""text-align: center;""><span style=""font-size: 16px; background-color: transparent;""><div></div></span></div>"),
          Subject = "Estamos juntos nessa conquista",
          Name = "congratulatecompanyanniversary",
          Link = path
        };
        // Insert
        model = serviceMailModel.InsertNewVersion(model).Result;
        return model;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

  }
}

