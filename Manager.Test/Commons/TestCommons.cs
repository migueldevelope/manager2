using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manager.Test.Commons
{
  public abstract class TestCommons<TEntity> : IDisposable
  {
    public DataContext context;
    readonly string databaseConnection = "mongodb://analisa_teste:bti9010@10.0.0.16:27017/analisa_teste";
    readonly string databaseName = "analisa_teste";

    public BaseUser baseUser;
    //public IServiceMaturity serviceMaturity;
    //public IServiceControlQueue serviceControlQueue;
    //string serviceBusConnectionString = "Endpoint=sb://analisa.servicebus.windows.net/;SharedAccessKeyName=analisahomologacao;SharedAccessKey=MS943jYNc9KmGP3HoIcL/eGhxhgIEAscB5R5as48Xik=;";
    //string queueName = "analisahomologacao";

    public void Dispose()
    {
      GC.SuppressFinalize(this);
    }

    #region Inicialização de base de dados para o TESTE
    protected void InitOffBase()
    {
      try
      {
        context = new DataContext(databaseConnection, databaseName);

        // Limpeza do banco
        string script = @"db.getCollectionNames().forEach(function(c) { if (c.indexOf(""system."") == -1) db[c].drop(); })";
        BsonDocument response = context._db.RunCommand(new BsonDocumentCommand<BsonDocument>(new BsonDocument() {{ "eval", script }}));

        // Criar conta principal (Account)
        ServiceGeneric<Account> serviceAccount = new ServiceGeneric<Account>(context);
        Account account = new Account()
        {
          _id = "5b6c4f47d9090156f08775aa",
          Name = "Analisa",
          Nickname = "Analisa",
          Status = EnumStatus.Enabled,
          _idAccount = "5b6c4f47d9090156f08775aa",
          InfoClient = string.Empty
        };
        account = serviceAccount.InsertFreeNewVersion(account).Result;

        // Criar Empresa (Company)
        ServiceGeneric<Company> serviceCompany = new ServiceGeneric<Company>(context);
        Company company = new Company()
        {
          _id = "5b6c4f47d9090156f08775ab",
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Analisa",
          Status = EnumStatus.Enabled,
          Logo = null,
          Skills = new List<ViewListSkill>(),
          Template = null
        };
        company = serviceCompany.InsertFreeNewVersion(company).Result;

        // Criar usuário administrador (User)
        ServiceGeneric<User> serviceUser = new ServiceGeneric<User>(context);
        User user = new User()
        {
          _id = "5c741b4c8ce3ae39f6945e20",
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Analisa",
          Status = EnumStatus.Enabled,
          ChangePassword = EnumChangePassword.No,
          Coins = 0,
          DateAdm = null,
          DateBirth = null,
          Document = null,
          DocumentCTPF = null,
          DocumentID = null,
          ForeignForgotPassword = null,
          Mail = "suporte@jmsoft.com.br",
          Nickname = null,
          Password = "DB64C0254298CED993D41EAB7BAD2037",
          Phone = null,
          PhoneFixed = null,
          PhotoUrl = null,
          Schooling = null,
          Sex = EnumSex.Others,
          UserAdmin = true,
          UserTermOfServices = null
        };
        user = serviceUser.InsertFreeNewVersion(user).Result;

        // Criar usuário administrador (Person)
        ServiceGeneric<Person> servicePerson = new ServiceGeneric<Person>(context);
        Person person = new Person()
        {
          _id = "5b6c4f56d9090156f08775ac",
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Company = company.GetViewList(),
          DateLastOccupation = null,
          DateLastReadjust = null,
          DateResignation = null,
          DocumentManager = null,
          Establishment = null,
          HolidayReturn = null,
          Manager = null,
          MotiveAside = null,
          Occupation = null,
          Registration = null,
          Salary = 0,
          SalaryScales = null,
          StatusUser = EnumStatusUser.Enabled,
          TypeJourney = EnumTypeJourney.OnBoarding,
          TypeUser = EnumTypeUser.Administrator,
          User = user.GetViewCrud()
        };
        person = servicePerson.InsertFreeNewVersion(person).Result;

        baseUser = new BaseUser()
        {
          NameAccount = "Analisa",
          Mail = "suporte@jmsoft.com.br",
          NamePerson = "Analisa",
          _idAccount = "5b6c4f47d9090156f08775aa",
          _idPerson = "5b6c4f56d9090156f08775ac",
          _idUser = "5c741b4c8ce3ae39f6945e20"
        };

        // Criar parâmetros (Parameter)
        CreateParameters();

        // Criar textos padrões (TextDefault)
        CreateTextDefault(company);

        // Criar modelos e-mails (MailModel)
        CreateMailModel();

        // Criar as perguntas padrões (Questions)
        CreateQuestions(company);

        // Criar algumas skills (Skill)
        CreateSkill();

        // Criar as escolaridades (Schooling)
        CreateSchooling();

        // Criar as esferas (Sphere)
        CreateSphere(company);

        // Criar os eixos (Axis)
        CreateAxis(company);

        // Criar alguns grupos de cargo (Group)
        CreateGroup(company);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void CreateParameters()
    {
      try
      {
        ServiceGeneric<Parameter> service = new ServiceGeneric<Parameter>(context)
        {
          _user = baseUser
        };

        Parameter parameter = new Parameter()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "1",
          Help = "Informar 0 para não mostrar e 1 para mostrar",
          Key = "viewlo",
          Name = "Mostrar linhas de oportunidade",
          Status = EnumStatus.Enabled
        };
        parameter = service.InsertNewVersion(parameter).Result;
        parameter = new Parameter()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "0",
          Help = "Informar 0 para cadastro normal de 1 para cadastro de multicontratos",
          Key = "typeregiterperson",
          Name = "Tipo do cadastro da pessoa",
          Status = EnumStatus.Enabled
        };
        parameter = service.InsertNewVersion(parameter).Result;
        parameter = new Parameter()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "90",
          Help = "Quantidade de dias",
          Key = "DeadlineAdm",
          Name = "Total de dias do contrato de experiência",
          Status = EnumStatus.Enabled
        };
        parameter = service.InsertNewVersion(parameter).Result;
        parameter = new Parameter()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "suporte@jmsoft.com.br",
          Help = "Informar um e-mail",
          Key = "mailcheckpoint",
          Name = "E-mail do RH para enviar aviso do Checkpoint",
          Status = EnumStatus.Enabled
        };
        parameter = service.InsertNewVersion(parameter).Result;
        parameter = new Parameter()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "0",
          Help = "Notificação por e-mail das pendências do sistema",
          Key = "servicemailmessage",
          Name = "Ativar monitoramento de e-mail",
          Status = EnumStatus.Enabled
        };
        parameter = service.InsertNewVersion(parameter).Result;
        parameter = new Parameter()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "0",
          Help = "Informar 0 se não utilizar o processo de lançamento de objetivos",
          Key = "goalProcess",
          Name = "Lançamento de Objetivos",
          Status = EnumStatus.Enabled
        };
        parameter = service.InsertNewVersion(parameter).Result;
        parameter = new Parameter()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "0",
          Help = "Informar 0 se não utilizar o processo de meritocracia",
          Key = "meritocracyProcess",
          Name = "Meritocracia",
          Status = EnumStatus.Enabled
        };
        parameter = service.InsertNewVersion(parameter).Result;
      }
      catch (Exception)
      {
        throw;
      }
    }

    private void CreateTextDefault(Company company)
    {
      ServiceGeneric<TextDefault> service = new ServiceGeneric<TextDefault>(context)
      {
        _user = baseUser
      };
      TextDefault textDefault = new TextDefault()
      {
        _idAccount = "5b6c4f47d9090156f08775aa",
        Company = company.GetViewList(),
        Content = "<div>Olá!</br>Este é um momento muito importante na jornada do(a) <b>{employee_name}</b> na <b>{company_name}</b>.</br>Como gestor, você terá que decidir sobre a sua efetivação.</br>Estamos aqui para ajudá-lo! Reflita... pondere... e decida!</br>Boa sorte, ai ;)</div>",
        Name = "Checkpoint",
        TypeText = EnumTypeText.Checkpoint,
        Template = null,
        Status = EnumStatus.Enabled
      };
      textDefault = service.InsertNewVersion(textDefault).Result;
      textDefault = new TextDefault()
      {
        _idAccount = "5b6c4f47d9090156f08775aa",
        Company = company.GetViewList(),
        Content = "Você está prestes a iniciar o processo de acreditação do(a) <b>{employee_name}</b>, reconhecê-lo(a) como referência e diferenciado(a).</br>Vamos lá, escolha um item e comece o processo de solicitação de acreditação:",
        Name = "Texto inicial da acreditação",
        TypeText = EnumTypeText.CertificationHead,
        Template = null,
        Status = EnumStatus.Enabled
      };
      textDefault = service.InsertNewVersion(textDefault).Result;
      textDefault = new TextDefault()
      {
        _idAccount = "5b6c4f47d9090156f08775aa",
        Company = company.GetViewList(),
        Content = "<div>Esta ACREDITAÇÃO destina-se ao colaborador cuja a skill <b>{item_name}</b> seja considerada como parâmetro de referência, está sendo utilizada em estado de excelência.<br>Se focaliza em atividades de \nCRIAÇÃO/INOVAÇÃO, TREINAMENTO/ORIENTAÇÃO e/ou LIDERANÇA DE EQUIPES, sendo capaz de solucionar questões de grande complexidade da atividade ou sendo o benchmark da skill <b>{item_name}</b>.<br>Para iniciar o processo de ACREDITAÇÃO da&nbsp;&nbsp;<b>{item_name}</b> do(a) <b>{employee_name}</b>, reflita e responda:</div>",
        Name = "Cabeçalho acreditação - Gestor",
        TypeText = EnumTypeText.Certification,
        Template = null,
        Status = EnumStatus.Enabled
      };
      textDefault = service.InsertNewVersion(textDefault).Result;
      textDefault = new TextDefault()
      {
        _idAccount = "5b6c4f47d9090156f08775aa",
        Company = company.GetViewList(),
        Content = "Temos uma solicitação importante para você: apoiar o(a) <b>{manager_name}</b> no processo de acreditação do(a) <b>{employee_name}</b>:</br>Primeiro, entenda o que é a {type} <b>{item_name}</b>, {concept}.</br>Segundo, esta acreditação destina-se ao colaborador cuja {type} <b>{item_name}</b> seja considerada como parâmetro de referência, ou seja, está sendo utilizado em estado de excelência.</br>Se focaliza em atividades de CRIAÇÃO/INOVAÇÃO, TREINAMENTO/ORIENTAÇÃO e/ou LIDERANÇA DE EQUIPES, sendo capaz de solucionar questões de grande complexidade de atividade ou sendo o benchmark da {type} <b>{item_name}</b>.</br>Veja a seguir, as justificativas do(a) <b>{manager_name}</b> para a solicitação desta acreditação.",
        Name = "Cabeçalho acreditação - Aprovador",
        TypeText = EnumTypeText.CertificationPerson,
        Template = null,
        Status = EnumStatus.Enabled
      };
      textDefault = service.InsertNewVersion(textDefault).Result;
      textDefault = new TextDefault()
      {
        _idAccount = "5b6c4f47d9090156f08775aa",
        Company = company.GetViewList(),
        Content = "Terceiro, você concorda com a acreditação da {type} <b>{item_name}</b> para o(a) colaborador(a) <b>{employee_name}</b>:",
        Name = "Texto para concordar com a acreditação - Aprovador",
        TypeText = EnumTypeText.CertificationPersonEnd,
        Template = null,
        Status = EnumStatus.Enabled
      };
      textDefault = service.InsertNewVersion(textDefault).Result;
    }

    private void CreateMailModel()
    {
      try
      {
        ServiceGeneric<MailModel> service = new ServiceGeneric<MailModel>(context)
        {
          _user = baseUser
        };
        MailModel mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>Recursos Humanos</strong>,</br></br>O resultado do check point para o(a) colaborador(a) <strong>{Person}</strong> foi APROVADO.</br>Procure o gestor <strong>{Manager}</strong> para realizar os procedimentos de efetivação de período de experiência.</br></br>#VamosSerMaisFluidos",
          Name = "checkpointresult",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Decisão de Efetivação | Check point - APROVADO"          
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>{Person}</strong>,</br></br>Parabéns! Você foi efetivado na <strong>{Company}</strong>. Isso é fruto do seu engajamento e alinhamento com nossa cultura e nossas competências essenciais.</br>Agora é seguir interessado em conhecer mais sobre as entregas do seu cargo e as competências que fazem parte do mesmo.</br>Fique atento às notificações do Analisa e aproveite o máximo de suas funcionalidades.</br>Estaremos na torcida para que sua carreira seja a mais fluida possível.</br></br>#VamosSerMaisFluidos",
          Name = "checkpointresultperson",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Decisão de Efetivação | Checkpoint"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>{Person}</strong>,<div><br>Seu gestor <strong>{Manager}</strong> acaba de registrar ações de monitoramento da sua performance.<br>Não perca tempo, acesse agora o Analisa e veja este conteúdo.<br>Lembre-se: quanto mais engajamento nas suas ações de desenvolvimento, mais fluida será a sua carreira!<br><br>Para acessar o Analisa <a href='https://analisa.solutions/'>clique aqui</a>.<br><br>#VamosSerMaisFluidos</div>",
          Name = "monitoringapproval",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Aprovação do Monitoramento | Monitoring"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>{Manager}</strong>,</br></br>Seu colaborador <strong>{Person}</strong> acaba de propor uma revisão nos registros de monitoramento.</br>Acesse agora o Analisa e veja este conteúdo.</br>Lembre-se: quanto mais engajado o colaborador estiver em suas ações de desenvolvimento, mais fluida será a carreira dele e melhores resultados você alcançará como gestor.</br></br>Para acessar o sistema Analisa <a href='https://analisa.solutions/'>clique aqui</a>.</br></br>#VamosSerMaisFluidos",
          Name = "monitoringdisapproval",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Revisão do Monitoramento | Monitoring"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>{Person}</strong>,</br></br>Bem - vindo à <strong>{Company}</strong>!</br>Estaremos aqui na torcida para que sua trajetória de carreira seja a melhor possível.</br>Seu gestor <strong>{Manager}</strong> acaba de fazer um registro do seu embarque no Analisa.</br></br>Não perca tempo, <a href='https://analisa.solutions/'>clique aqui e confira</a>.</br></br>#VamosSerMaisFluidos",
          Name = "onboardingapproval",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação para Continuar Embarque | OnBoarding"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>{Manager}</strong>,</br></br>Seu colaborador <strong>{Person}</strong> acaba de propor novos registros de monitoramento.</br>Acesse agora o Analisa e veja este conteúdo.</br>Lembre-se: quanto mais engajado o colaborador estiver em suas ações de desenvolvimento, mais fluida será a carreira dele e melhores resultados você alcançará como gestor.</br></br>Para acessar o sistema Analisa <a href='https://analisa.solutions/'>clique aqui</a>.</br></br>#VamosSerMaisFluidos",
          Name = "monitoringapprovalmanager",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Continuar o Monitoramento | Monitoring"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>{Guest}</strong>,</br></br>O(a) gestor(a) <strong>{Manager}</strong> enviou uma solicitação para aprovação de acreditação do(a) colaborador(a) <strong>{Person}</strong>.</br>Ajude a encontrar as pessoas que são referências na nossa empresa.</br></br>Para acessar o sistema <a href='https://analisa.solutions/'>clique aqui</a>.</br></br>#VamosSerMaisFluidos",
          Name = "certification",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação para aprovação de Acreditação | Accretitation Skill"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>{Manager}</strong>,<br><br>A acreditação da competência <strong>{Skill}</strong> do(a) colaborador(a) <strong>{Person}</strong> foi APROVADA.<br>Enviamos um e-mail parabenizando o(a) colaborador(a) mas é muito importante que você comemore pessoalmente com ele(a).<br><br>#VamosSerMaisFluidos",
          Name = "certificationapproval",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Acreditação | Accreditation Skill - APROVADA"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>{Person}</strong>,<br><br>Foi concedido a você a acreditação da competência <strong>{Skill}!</strong><br>Parabéns por este feito, ele é fruto de seu engajamento e vontade de se desenvolver!<div>Aproveite para comemorar, celebre,... publique o selo de acreditação nas suas redes sociais!</div><div><br></div><div>Ficaremos aqui na torcida para que você siga se desenvolvendo!<br><br><div>#VamosSerMaisFluidos</div></div>",
          Name = "certificationapprovalperson",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Acreditação | Accreditation Skill - APROVADA COLABORADOR(A)"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"<div>Olá <b>{Manager}</b>,</div><div><br></div><div>A acreditação da competência <b>{Skill}</b> do(a) colaborador(a) <b>{Person}</b> foi REPROVADA.</div><div>Não desanime, siga incentivando o seu colaborador a se aperfeiçoar ainda mais!</div><div>Ainda não foi desta vez que houve consenso para a acreditação; todavia, daqui algum tempo você pode solicitá-la novamente, assim que perceber que houve evolução no desenvolvimento&nbsp;<span style=""background - color: transparent; font - size: 0.875rem; "">daquela competência.</span></div><div><br></div><div>#VamosSerMaisFluidos</div>",
          Name = "certificationdisapproval",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Acreditação | Accretitation Skill - REPROVADA"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>Recursos Humanos</strong>,</br></br>O resultado do check point para <strong>{Person}</strong> foi REPROVADO.</br>Procure o gestor <strong>{Manager}</strong> para realizar os procedimentos de desligamento.</br></br>#VamosSerMaisFluidos",
          Name = "checkpointresultdisapproved",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Decisão de Efetivação | Check point - REPROVADO"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>{Manager}</strong>,<br><br>O colaborador <strong>{Person}</strong> foi admitido na <strong>{Company}</strong> e iniciou os registros de embarque na sua carreira.<br>Dê continuidade a este processo para gerar mais engajamento de sua equipe e melhorar ainda mais os nossos processos de gestão de pessoas.<br><br>Não perca tempo, <a href=""https://analisa.solutions/""> clique aqui e confira </a>.<br><br>#VamosSerMaisFluidos",
          Name = "onboardingapprovalmanager",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação da Aprovação de Embarque | OnBoarding"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>{Manager}</strong>,</br></br>O seu colaborador <strong>{Person}</strong> fez alguns comentários no encerramento do embarque.</br></br>Não deixe para mais tarde, <a href='https://analisa.solutions/'> clique aqui </a> e olhe os comentários que foram redigidos.</br></br>#VamosSerMaisFluidos",
          Name = "onboardingdisapproval",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Revisão de Embarque | OnBoarding"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>{Person}</strong>,<br><br>Bem-vindo ao seu novo cargo <strong>{Occupation}</strong>!<br>Estaremos aqui na torcida para que sua trajetória de carreira seja a melhor possível.<br>Seu gestor <b>{Manager}</b><strong style=""font - weight: bold; ""> </strong>acaba de fazer um registro do seu embarque no Analisa.<br><br><b>Não perca tempo, </b><a href=""https://analisa.solutions/"" style=""font-weight: bold;"">clique aqui e confira</a><b>.</b><br><br><b>#VamosSerMaisFluidos</b>",
          Name = "onboardingapprovaloccupation",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação da Aprovação de OnBoarding Troca de Cargo"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>{Manager}</strong>,</br></br>O colaborador <strong>{Person}</strong> foi promovido para o cargo <strong>{Occupation}</strong>, e iniciou os registros de embarque.</br>De continuidade a este processo para gerar mais engajamento de sua equipe, e melhorar nossos processos de gestão.</br></br>Para acessar o sistema <a href='https://analisa.solutions/'>clique aqui</a>.</br></br>#VamosSerMaisFluidos",
          Name = "onboardingapprovalmanageroccupation",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação para Continuar Embarque no Cargo | OnBoarding Occupation"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <strong>{Manager}</strong>,</br></br>O seu colaborador <strong>{Person}</strong> fez alguns comentários no encerramento do embarque para troca de cargo.</br></br>Não deixe para mais tarde, <a href='https://analisa.solutions/'>clique aqui</a> e olhe os comentários que foram redigidos.</br></br>#VamosSerMaisFluidos",
          Name = "onboardingdisapprovaloccupation",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Revisão de Embarque | OnBoarding"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <b>{Manager}</b>,<br><br>O colaborador <b>{Person}</b> acaba de ser admitido. Recomendamos que, após o seu período inicial de integração, você apresente a ele as funcionalidades de nossa plataforma e promova uma conversa sobre o ""Mapa de Competências"".<br><br>Esta conversa é fundamental para que você faça um acordo de expectativas em relação ao que dele é esperado no exercício do cargo.<br><br>O ideal é que você possa realizar este acordo antes dos primeiros 30 (trinta) dias de empresa.<br><br>#VamosSerMaisFluidos<br><br>",
          Name = "onboardingadmission",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Admissão de Novo Colaborador | Jornada de Embarque | Onboarding"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <b>{Manager}</b>,<br><br>Identificamos que alguns colaboradores da sua equipe estão na jornada Embarque | Onboarding, mas não tiveram ainda o registro de sua conversa de apresentação do Mapa de Competências.<br><br>Vai lá,... faça acontecer este acordo de expectativas, que será fundamental para um ótimo desenvolvimento de carreira do seu novo colaborador.<br><div><br></div><div>{LIST1}{LIST2}{LIST3}{LIST4}</div>Para acessar a plataforma <a href=""{Link}"">clique aqui</a>.<br><br>#VamosSerMaisFluidos<br><br>",
          Name = "onboardingadmission",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Vencimento da Jornada de Embarque | Onboarding"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <b>{Manager}</b>,<br><br>Identificamos que alguns colaboradores da sua equipe estão na jornada de Definição de Efetivação | Check-Point. Você precisa ficar atento para não perder os prazos da sua decisão de efetivação (ou não) deste novo colaborador.<br><br>Coloque na sua agenda, faça de acordo com sua disponibilidade, mas lembre que esta ação é obrigatória dentro desta jornada!<br><br>{LIST1}{LIST2}{LIST3}{LIST4}{LIST5}<div>Para acessar a plataforma <a href=""{Link}"">clique aqui</a>.</div><br>#VamosSerMaisFluidos<br><br>",
          Name = "onboardingadmission",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Vencimentos da Jornada de Definição de Efetivação | Check-Point"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <b>{Manager}</b>,<br><br>Identificamos que alguns colaboradores da sua equipe estão na Jornada de Monitoramento | Monitoring, porém não tiveram interação com você há mais de 90 dias.<br><br>Para que os movimentos de carreira sejam mais fluidos e o engajamento da sua equipe seja cada vez maior, não perca tempo,... dá uma passada lá na plataforma e retome suas conversas e combinações!<br><br>{LIST1}<br>Para acessar a plataforma <a href=""{Link}"" target=""_blank"">clique aqui</a>.<br><br>#VamosSerMaisFluidos<br><br>",
          Name = "monitoringmanagerdeadline",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Monitoramento | Monitoring"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <b>{Person}</b>,<br><br>Identificamos que você está na Jordana de Monitoramento | Monitoring, mas não teve nenhuma interação com seu gestor há mais de 90 dias.<br><br>Seja protagonista de sua carreira e retome agora mesmo o seu monitoramento! <br><br>Faça isso agora! Acesse a plataforma <a href=""{Link}"" target=""_blank"">clique aqui</a>.<br><br>#VamosSerMaisFluidos<br><br>",
          Name = "monitoringdeadline",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Monitoramento | Monitoring do colaborador"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <b>{Manager}</b>,<br><br>Identificamos que existem ações de desenvolvimento da sua equipe com situações que requerem sua atenção!<br><br>Verifique o status destas ações, pois isto fomenta o círculo virtuoso das carreiras fluidas.<br><br>{LIST1}{LIST2}{LIST3}{LIST4}{LIST5}<br>Para acessar a plataforma <a href=""{Link}"" target=""_blank"">clique aqui</a>.<br><br>#VamosSerMaisFluidos<br><br>",
          Name = "actionplanmanagerdeadline",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Ação de Desenvolvimento | Action Plans"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <b>{Person}</b>,<br><br>Identificamos que existem ações de desenvolvimento com situações que requerem a sua atenção.<br><br>Verifique o status destas ações de desenvolvimento, pois ela são fundamentais para o seu desenvolvimento e para a fluidez de sua carreira!<br><div><br></div><div>{LIST1}{LIST2}{LIST3}{LIST4}{LIST5}</div>Para acessar a plataforma <a href=""{Link}"" target=""_blank"">clique aqui</a>.<br><br>#VamosSerMaisFluidos<br><br>",
          Name = "actionplandeadline",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Ação de Desenvolvimento | Action Plans do colaborador"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
        mailModel = new MailModel()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Status = EnumStatus.Enabled,
          Link = @"https://analisa.solutions/",
          Message = @"Olá <b>{Manager}</b>,<br><br>O gestor <b>{Requestor}</b>, está solicitando que o(a) colaborador(a) <b>{Employee}</b> faça parte da equipe dele.<br><br>Você <a href=""{Approved}"" target=""_blank"">aprova</a> ou <a href=""{Disapproved}"" target=""_blank"">reprova</a> esta solicitação?<br><br><div>#VamosSerMaisFluidos</div>",
          Name = "automanager",
          StatusMail = EnumStatus.Enabled,
          Subject = "Notificação de Auto Gestão - Pedido de Autorização"
        };
        mailModel = service.InsertNewVersion(mailModel).Result;
      }
      catch (Exception)
      {
        throw;
      }
    }

    private void CreateQuestions(Company company)
    {
      try
      {
        ServiceGeneric<Questions> service = new ServiceGeneric<Questions>(context)
        {
          _user = baseUser
        };
        Questions questions = new Questions()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "O(a) colaborador(a) {employee_name} está alinhado às skills essenciais da {company_name}?",
          Name = "ESSENCIAIS",
          Status = EnumStatus.Enabled,
          Company = company.GetViewList(),
          Order = 1,
          Template = null,
          TypeQuestion = EnumTypeQuestion.Skill,
          TypeRotine = EnumTypeRotine.Checkpoint

        };
        questions = service.InsertNewVersion(questions).Result;
        questions = new Questions()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "O(a) colaborador(a) {employee_name} mostra envolvimento com os produtos da {company_name}, curiosidade e principalmente empenho?",
          Name = "DEDICAÇÃO",
          Status = EnumStatus.Enabled,
          Company = company.GetViewList(),
          Order = 2,
          Template = null,
          TypeQuestion = EnumTypeQuestion.Default,
          TypeRotine = EnumTypeRotine.Checkpoint
        };
        questions = service.InsertNewVersion(questions).Result;
        questions = new Questions()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "O(a) colaborador(a) {employee_name} identificou-se rapidamente com o grupo de colegas, tratando-os cordialmente e trazendo uma visão positiva no enfrentamento de desafios?",
          Name = "RELACIONAMENTO",
          Status = EnumStatus.Enabled,
          Company = company.GetViewList(),
          Order = 3,
          Template = null,
          TypeQuestion = EnumTypeQuestion.Default,
          TypeRotine = EnumTypeRotine.Checkpoint
        };
        questions = service.InsertNewVersion(questions).Result;
        questions = new Questions()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "Por fim, o(a) colaborador(a) {employee_name} demonstra iniciativa para encontrar soluções criativas?",
          Name = "INICIATIVA",
          Status = EnumStatus.Enabled,
          Company = company.GetViewList(),
          Order = 4,
          Template = null,
          TypeQuestion = EnumTypeQuestion.Default,
          TypeRotine = EnumTypeRotine.Checkpoint
        };
        questions = service.InsertNewVersion(questions).Result;
        questions = new Questions()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "1. Em que situação você verificou que esta competência {item_name} estava enquadrada nas situações acima?",
          Name = "pergunta 1",
          Status = EnumStatus.Enabled,
          Company = company.GetViewList(),
          Order = 1,
          Template = null,
          TypeQuestion = EnumTypeQuestion.Text,
          TypeRotine = EnumTypeRotine.Certification
        };
        questions = service.InsertNewVersion(questions).Result;
        questions = new Questions()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "2. Tens como comprovar? Relatando ou anexando documentos.",
          Name = "pergunta 2",
          Status = EnumStatus.Enabled,
          Company = company.GetViewList(),
          Order = 2,
          Template = null,
          TypeQuestion = EnumTypeQuestion.Text,
          TypeRotine = EnumTypeRotine.Certification
        };
        questions = service.InsertNewVersion(questions).Result;
        questions = new Questions()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "Anexe ou visualize evidências (opcional)",
          Name = "pergunta 2 - anexo",
          Status = EnumStatus.Enabled,
          Company = company.GetViewList(),
          Order = 3,
          Template = null,
          TypeQuestion = EnumTypeQuestion.Attachment,
          TypeRotine = EnumTypeRotine.Certification
        };
        questions = service.InsertNewVersion(questions).Result;
        questions = new Questions()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "3. Convide 3 pessoas para ajudarem neste processo - faça as suas escolhas, pensando em situações em que os escolhidos tenham vivenciado a utilização desta competência {item_name} em conjunto com o(a) {employee_name}.</br>A partir de suas informações, irão apoiar no processo de decisão sobre a acreditação.",
          Name = "pergunta 3",
          Status = EnumStatus.Enabled,
          Company = company.GetViewList(),
          Order = 4,
          Template = null,
          TypeQuestion = EnumTypeQuestion.Default,
          TypeRotine = EnumTypeRotine.Certification
        };
        questions = service.InsertNewVersion(questions).Result;
        questions = new Questions()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Content = "Visualize evidências",
          Name = "visualizar anexo",
          Status = EnumStatus.Enabled,
          Company = company.GetViewList(),
          Order = 5,
          Template = null,
          TypeQuestion = EnumTypeQuestion.Default,
          TypeRotine = EnumTypeRotine.Certification
        };
        questions = service.InsertNewVersion(questions).Result;
      }
      catch (Exception)
      {
        throw;
      }
    }

    private void CreateSkill()
    {
      try
      {
        ServiceGeneric<Skill> service = new ServiceGeneric<Skill>(context)
        {
          _user = baseUser
        };
        Skill skill;
        for (int i = 1; i < 71; i++)
        {
          skill = new Skill()
          {
            _idAccount = "5b6c4f47d9090156f08775aa",
            Name = string.Format("Skill {0} {1}", i <= 15 ? "Soft" : "Hard",i),
            Concept = string.Format("Conceito da Skill {0} {1}", i <= 25 ? "Soft" : "Hard", i),
            Template = null,
            TypeSkill = EnumTypeSkill.Soft,
            Status = EnumStatus.Enabled
          };
          skill = service.InsertNewVersion(skill).Result;
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    private void CreateSchooling()
    {
      try
      {
        ServiceGeneric<Schooling> service = new ServiceGeneric<Schooling>(context)
        {
          _user = baseUser
        };
        Schooling schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Analfabeto",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = -1,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
        schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Ensino Fundamental Incompleto",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = 1,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
        schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Ensino Fundamental Completo",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = 2,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
        schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Ensino Médio Incompleto",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = 3,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
        schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Ensino Médio Completo",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = 4,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
        schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Técnico Incompleto",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = 5,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
        schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Técnico Completo",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = 6,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
        schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Ensino Superior Incompleto",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = 7,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
        schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Ensino Superior Completo",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = 8,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
        schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Pós Graduação Incompleto",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = 9,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
        schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Pós Graduação Completo",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = 10,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
        schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Mestrado Incompleto",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = 11,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
        schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Mestrado Completo",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = 12,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
        schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Doutorato Incompleto",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = 13,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
        schooling = new Schooling()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Doutorato Completo",
          Template = null,
          Status = EnumStatus.Enabled,
          Complement = null,
          Order = 14,
          Type = EnumTypeSchooling.Basic
        };
        schooling = service.InsertNewVersion(schooling).Result;
      }
      catch (Exception)
      {
        throw;
      }
    }

    private void CreateSphere(Company company)
    {
      try
      {
        ServiceGeneric<Sphere> service = new ServiceGeneric<Sphere>(context)
        {
          _user = baseUser
        };
        Sphere sphere = new Sphere()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Operacional",
          Company = company.GetViewList(),
          TypeSphere = EnumTypeSphere.Strategic,
          Template = null,
          Status = EnumStatus.Enabled,
        };
        sphere = service.InsertNewVersion(sphere).Result;
        sphere = new Sphere()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Tático",
          Company = company.GetViewList(),
          TypeSphere = EnumTypeSphere.Tactical,
          Template = null,
          Status = EnumStatus.Enabled,
        };
        sphere = service.InsertNewVersion(sphere).Result;
        sphere = new Sphere()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Estratégico",
          Company = company.GetViewList(),
          TypeSphere = EnumTypeSphere.Operational,
          Template = null,
          Status = EnumStatus.Enabled,
        };
        sphere = service.InsertNewVersion(sphere).Result;
      }
      catch (Exception)
      {
        throw;
      }
    }

    private void CreateAxis(Company company)
    {
      try
      {
        ServiceGeneric<Axis> service = new ServiceGeneric<Axis>(context)
        {
          _user = baseUser
        };
        Axis axis = new Axis()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Administrativo",
          Company = company.GetViewList(),
          Template = null,
          TypeAxis = EnumTypeAxis.Administrator,
          Status = EnumStatus.Enabled,
        };
        axis = service.InsertNewVersion(axis).Result;
        axis = new Axis()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Operação",
          Company = company.GetViewList(),
          Template = null,
          TypeAxis = EnumTypeAxis.Operational,
          Status = EnumStatus.Enabled,
        };
        axis = service.InsertNewVersion(axis).Result;
        axis = new Axis()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Técnico",
          Company = company.GetViewList(),
          Template = null,
          TypeAxis = EnumTypeAxis.Techinque,
          Status = EnumStatus.Enabled,
        };
        axis = service.InsertNewVersion(axis).Result;
        axis = new Axis()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Gestão",
          Company = company.GetViewList(),
          Template = null,
          TypeAxis = EnumTypeAxis.Manager,
          Status = EnumStatus.Enabled,
        };
        axis = service.InsertNewVersion(axis).Result;
      }
      catch (Exception)
      {
        throw;
      }
    }

    private void CreateGroup(Company company)
    {
      try
      {
        // Eixo
        ServiceGeneric<Axis> serviceAxis = new ServiceGeneric<Axis>(context)
        {
          _user = baseUser
        };
        Axis axis = serviceAxis.GetNewVersion(p => p.Name == "Gestão").Result;
        if (axis == null)
          throw new Exception("Erro no grupo buscando o eixo gestão");
        // Esfera
        ServiceGeneric<Sphere> serviceSphere = new ServiceGeneric<Sphere>(context)
        {
          _user = baseUser
        };
        Sphere sphere = serviceSphere.GetNewVersion(p => p.Name == "Estratégico").Result;
        if (axis == null)
          throw new Exception("Erro no grupo buscando a esfere estratégico");
        // Escopo do grupo
        List<ViewListScope> scopeList = new List<ViewListScope>();
        ViewListScope scope;
        for (int i = 1; i < 5; i++)
        {
          scope = new ViewListScope()
          {
            Name = string.Format("Scopo do diretor {0}", i),
            _id = ObjectId.GenerateNewId().ToString(),
            Order = i
          };
          scopeList.Add(scope);
        }
        // Escolaridades
        ServiceGeneric<Schooling> serviceSchooling = new ServiceGeneric<Schooling>(context)
        {
          _user = baseUser
        };
        List<ViewCrudSchooling> schoolingList = new List<ViewCrudSchooling>();
        Schooling schooling = serviceSchooling.GetNewVersion(p => p.Name == "Pós Graduação Completo").Result;
        if (schooling == null)
          throw new Exception("Erro no grupo buscando a escolaridade Pós Graduação Completo");
        schoolingList.Add(schooling.GetViewCrud());
        // Competências
        ServiceGeneric<Skill> serviceSkill = new ServiceGeneric<Skill>(context)
        {
          _user = baseUser
        };
        List<ViewListSkill> skillList = new List<ViewListSkill>();
        Skill skill;
        for (int i = 1; i < 4; i++)
        {
          skill = serviceSkill.GetNewVersion(p => p.Name == string.Format("Skill Soft {0}",i)).Result;
          if (skill == null)
            throw new Exception(string.Format("Erro no grupo buscando a skill soft {0}",i));
          skillList.Add(skill.GetViewList());
        }
        ServiceGeneric<Group> service = new ServiceGeneric<Group>(context)
        {
          _user = baseUser
        };
        Group group = new Group()
        {
          _idAccount = "5b6c4f47d9090156f08775aa",
          Name = "Diretor",
          Company = company.GetViewList(),
          Template = null,
          Status = EnumStatus.Enabled,
          Axis = axis.GetViewList(),
          Sphere = sphere.GetViewList(),
          Line = 1,
          Occupations = null,
          Scope = scopeList,
          Schooling = schoolingList,
          Skills = skillList
        };
        group = service.InsertNewVersion(group).Result;
      }
      catch (Exception)
      {
        throw;
      }
    }

    #endregion

    protected void Init()
    {
      try
      {
        context = new DataContext(databaseConnection, databaseName);
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
