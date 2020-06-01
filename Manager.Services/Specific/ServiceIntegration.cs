﻿using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Business.Integration;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Manager.Views.Integration;
using Manager.Views.Integration.V2;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceIntegration : Repository<Person>, IServiceIntegration
  {
    private readonly ServicePerson personService;
    private readonly ServiceUser userService;
    private readonly ServiceInfra serviceInfra;
    private readonly ServiceGeneric<Schooling> schoolingService;
    private readonly ServiceGeneric<Company> companyService;
    private readonly ServiceGeneric<Establishment> establishmentService;
    private readonly ServiceGeneric<Account> accountService;
    private readonly ServiceGeneric<Occupation> occupationService;
    private readonly ServiceGeneric<Group> groupService;
    private readonly ServiceGeneric<IntegrationSchooling> integrationSchoolingService;
    private readonly ServiceGeneric<IntegrationCompany> integrationCompanyService;
    private readonly ServiceGeneric<IntegrationEstablishment> integrationEstablishmentService;
    private readonly ServiceGeneric<IntegrationOccupation> integrationOccupationService;
    private readonly ServiceGeneric<IntegrationParameter> parameterService;
    private readonly ServiceGeneric<IntegrationPerson> integrationPersonService;
    private readonly IServiceLog logService;
    // Integração de Skills, Cargos e Mapas do ANALISA
    private readonly ServiceGeneric<ProcessLevelTwo> processLevelTwoService;
    private readonly ServiceGeneric<Skill> skillService;

    // Colaborador V2
    private readonly ServiceGeneric<PayrollEmployee> payrollEmployeeService;
    private readonly ServiceGeneric<PayrollOccupation> payrollOccupationService;
    private ColaboradorV2Retorno resultV2;

    #region Constructor
    public ServiceIntegration(DataContext context, DataContext contextLog, DataContext contextIntegration, IServiceControlQueue _seviceControlQueue, string _pathSignalr) : base(context)
    {
      try
      {
        personService = new ServicePerson(context, contextLog, _seviceControlQueue, _pathSignalr);
        userService = new ServiceUser(context, contextLog);
        serviceInfra = new ServiceInfra(context);
        schoolingService = new ServiceGeneric<Schooling>(context);
        companyService = new ServiceGeneric<Company>(context);
        establishmentService = new ServiceGeneric<Establishment>(context);
        accountService = new ServiceGeneric<Account>(context);
        occupationService = new ServiceGeneric<Occupation>(context);
        integrationSchoolingService = new ServiceGeneric<IntegrationSchooling>(contextIntegration);
        integrationCompanyService = new ServiceGeneric<IntegrationCompany>(contextIntegration);
        integrationEstablishmentService = new ServiceGeneric<IntegrationEstablishment>(contextIntegration);
        integrationOccupationService = new ServiceGeneric<IntegrationOccupation>(contextIntegration);
        parameterService = new ServiceGeneric<IntegrationParameter>(contextIntegration);
        integrationPersonService = new ServiceGeneric<IntegrationPerson>(contextIntegration);
        logService = new ServiceLog(context, contextLog);
        processLevelTwoService = new ServiceGeneric<ProcessLevelTwo>(context);
        skillService = new ServiceGeneric<Skill>(context);
        groupService = new ServiceGeneric<Group>(context);
        payrollEmployeeService = new ServiceGeneric<PayrollEmployee>(contextIntegration);
        payrollOccupationService = new ServiceGeneric<PayrollOccupation>(contextIntegration);
      }
      catch (Exception)
      {
        throw;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      personService.SetUser(_user);
      userService.SetUser(_user);
      serviceInfra.SetUser(_user);
      schoolingService._user = _user;
      companyService._user = _user;
      accountService._user = _user;
      occupationService._user = _user;
      establishmentService._user = _user;
      integrationSchoolingService._user = _user;
      integrationCompanyService._user = _user;
      integrationEstablishmentService._user = _user;
      integrationOccupationService._user = _user;
      integrationPersonService._user = _user;
      parameterService._user = _user;
      processLevelTwoService._user = _user;
      skillService._user = _user;
      groupService._user = _user;
      logService.SetUser(contextAccessor);
      payrollEmployeeService._user = _user;
      payrollOccupationService._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      personService.SetUser(user);
      userService.SetUser(user);
      serviceInfra.SetUser(user);
      companyService._user = user;
      accountService._user = user;
      occupationService._user = user;
      establishmentService._user = user;
      integrationSchoolingService._user = user;
      integrationCompanyService._user = user;
      integrationEstablishmentService._user = user;
      integrationOccupationService._user = user;
      integrationPersonService._user = user;
      parameterService._user = user;
      processLevelTwoService._user = user;
      skillService._user = user;
      groupService._user = user;
      payrollEmployeeService._user = user;
      payrollOccupationService._user = user;
    }
    #endregion

    #region Commun Area
    private PayrollEmployee UserUpdate(PayrollEmployee payrollEmployee)
    {
      try
      {
        IntegrationSchooling integrationSchooling = GetIntegrationSchooling(payrollEmployee.Schooling, payrollEmployee.SchoolingName);
        Schooling schooling = schoolingService.GetNewVersion(p => p._id == integrationSchooling.IdSchooling).Result;
        if (schooling == null)
        {
          payrollEmployee.Messages.Add("Falta integração de Escolaridade");
          return payrollEmployee;
        }
        ViewCrudUser user = userService.GetNewVersion(p => p.Document == payrollEmployee.Document).Result?.GetViewCrud();
        if (user == null)
        {
          user = new ViewCrudUser()
          {
            Name = payrollEmployee.Name,
            Document = payrollEmployee.Document,
            Mail = payrollEmployee.Mail,
            Phone = payrollEmployee.CellNumber,
            DateAdm = payrollEmployee.AdmissionDate,
            DateBirth = payrollEmployee.BirthDate,
            Schooling = schooling.GetViewList(),
            PhoneFixed = null,
            DocumentID = null,
            DocumentCTPF = null,
            Sex = payrollEmployee.Sex,
            Nickname = payrollEmployee.Nickname,
            Password = payrollEmployee.Document
          };
          user = userService.New(user);
        }
        else
        {
          user.Name = payrollEmployee.Name;
          user.Document = payrollEmployee.Document;
          user.Mail = payrollEmployee.Mail;
          user.Phone = payrollEmployee.CellNumber;
          user.DateAdm = payrollEmployee.AdmissionDate;
          user.DateBirth = payrollEmployee.BirthDate;
          user.Schooling = schooling.GetViewList();
          user.PhoneFixed = null;
          user.DocumentID = null;
          user.DocumentCTPF = null;
          user.Sex = payrollEmployee.Sex;
          user.Nickname = payrollEmployee.Nickname;
          user = userService.Update(user);
        }
        payrollEmployee._idUser = user._id;
        payrollEmployee._idSchooling = schooling._id;
        return payrollEmployee;
      }
      catch (Exception ex)
      {
        payrollEmployee.Messages.Add(ex.ToString());
        return payrollEmployee;
      }
    }
    private PayrollEmployee PersonUpdate(PayrollEmployee payrollEmployee, bool changeOcuppation)
    {
      try
      {
        // Company
        IntegrationCompany integrationCompany = GetIntegrationCompany(payrollEmployee.Company, payrollEmployee.CompanyName);
        Company company = companyService.GetNewVersion(p => p._id == integrationCompany.IdCompany).Result;
        if (company == null)
        {
          payrollEmployee.Messages.Add("Falta integração da empresa");
          payrollEmployee.Messages.Add("Falta integração de estabelecimento");
          payrollEmployee.Messages.Add("Falta integração de cargo");
          return payrollEmployee;
        }
        payrollEmployee._idCompany = company._id;
        // Establishment
        IntegrationEstablishment  integrationEstablishment = GetIntegrationEstablishment(payrollEmployee.EstablishmentKey(), payrollEmployee.EstablishmentName, company._id);
        Establishment establishment = establishmentService.GetNewVersion(p => p._id == integrationEstablishment.IdEstablishment).Result;
        if (establishment == null)
        {
          payrollEmployee.Messages.Add("Falta integração de estabelecimento");
          return payrollEmployee;
        }
        payrollEmployee._idEstablishment = establishment._id;
        // Occupation
        IntegrationOccupation integrationOccupation = GetIntegrationOccupation(payrollEmployee.OccupationKey(), payrollEmployee.OccupationName, company._id, payrollEmployee.CostCenterKey(), payrollEmployee.CostCenterName);
        Occupation occupation = occupationService.GetNewVersion(p => p._id == integrationOccupation.IdOccupation).Result;
        if (occupation == null)
        {
          payrollEmployee.Messages.Add("Falta integração de cargo");
          return payrollEmployee;
        }
        payrollEmployee._idOccupation = occupation._id;
        payrollEmployee._idIntegrationOccupation = integrationOccupation._id;

        // Type key person contract
        ViewCrudIntegrationParameter viewCrudIntegrationParameter = GetIntegrationParameter();

        // Search manager contract
        ViewCrudPerson personManager = null;
        if (!string.IsNullOrEmpty(payrollEmployee.ManagerCompanyName) && !string.IsNullOrEmpty(payrollEmployee.ManagerEstablishmentName) &&
            !string.IsNullOrEmpty(payrollEmployee.ManagerDocument) && !string.IsNullOrEmpty(payrollEmployee.ManagerRegistration))
        {
          // Company Manager
          IntegrationCompany integrationCompanyManager = GetIntegrationCompany(payrollEmployee.ManagerCompany, payrollEmployee.ManagerCompanyName);
          Company companyManager = companyService.GetNewVersion(p => p._id == integrationCompanyManager.IdCompany).Result;
          if (companyManager == null)
          {
            payrollEmployee.Messages.Add("Falta integração da empresa do gestor");
            return payrollEmployee;
          }
          // Establishment Manager
          IntegrationEstablishment integrationEstablishmentManager = GetIntegrationEstablishment(payrollEmployee.ManagerEstablishment, payrollEmployee.ManagerEstablishmentName, companyManager._id);
          Establishment establishmentManager = establishmentService.GetNewVersion(p => p._id == integrationEstablishmentManager.IdEstablishment).Result;
          if (establishmentManager == null)
          {
            payrollEmployee.Messages.Add("Falta integração do estabelecimento do gestor");
            return payrollEmployee;
          }
          switch (viewCrudIntegrationParameter.IntegrationKey)
          {
            case EnumIntegrationKey.CompanyEstablishment:
              personManager = personService.GetNewVersion(p => p.User.Document == payrollEmployee.ManagerDocument &&
                                                          p.Company._id == companyManager._id &&
                                                          p.Establishment._id == establishmentManager._id &&
                                                          p.Registration == payrollEmployee.ManagerRegistration).Result?.GetViewCrud();
              break;
            case EnumIntegrationKey.Company:
              personManager = personService.GetNewVersion(p => p.User.Document == payrollEmployee.ManagerDocument &&
                                                          p.Company._id == companyManager._id &&
                                                          p.Registration == payrollEmployee.ManagerRegistration).Result?.GetViewCrud();
              break;
            case EnumIntegrationKey.Document:
              personManager = personService.GetNewVersion(p => p.User.Document == payrollEmployee.ManagerDocument).Result?.GetViewCrud();
              break;
            default:
              payrollEmployee.Messages.Add("Tipo de integração de chaves inválida");
              return payrollEmployee;
          }
          // Testar se o gestor existe mas não é do tipo gestor
          if (personManager != null && (personManager.TypeUser != EnumTypeUser.Manager || personManager.TypeUser != EnumTypeUser.ManagerHR))
          {
            personManager.TypeUser = personManager.TypeUser == EnumTypeUser.HR ? EnumTypeUser.ManagerHR : EnumTypeUser.Manager;
            string updatePerson = personService.Update(personManager);
          }

        }

        // Person contract
        ViewCrudPerson person;
        switch (viewCrudIntegrationParameter.IntegrationKey)
        {
          case EnumIntegrationKey.CompanyEstablishment:
            person = personService.GetNewVersion(p => p.User.Document == payrollEmployee.Document &&
                                                 p.Company._id == company._id &&
                                                 p.Establishment._id == establishment._id &&
                                                 p.Registration == payrollEmployee.Registration).Result?.GetViewCrud();
            break;
          case EnumIntegrationKey.Company:
            person = personService.GetNewVersion(p => p.User.Document == payrollEmployee.Document &&
                                                 p.Company._id == company._id &&
                                                 p.Registration == payrollEmployee.Registration).Result?.GetViewCrud();
            break;
          case EnumIntegrationKey.Document:
            person = personService.GetNewVersion(p => p.User.Document == payrollEmployee.Document).Result?.GetViewCrud();
            break;
          default:
            payrollEmployee.Messages.Add("Tipo de integração de chaves inválida");
            return payrollEmployee;
        }
        if (person == null)
        {
          person = new ViewCrudPerson
          {
            TypeUser = EnumTypeUser.Employee,
            Company = company.GetViewList(),
            Establishment = establishment.GetViewList(),
            Occupation = occupation.GetViewListResume(),
            Registration = payrollEmployee.Registration,
            HolidayReturn = null,
            MotiveAside = null,
            DateLastOccupation = payrollEmployee.OccupationChangeDate,
            Salary = payrollEmployee.Salary,
            DateLastReadjust = payrollEmployee.SalaryChangeDate,
            DateResignation = payrollEmployee.DemissionDate,
            TypeJourney = DateTime.Now.Subtract(payrollEmployee.AdmissionDate).Days > 90 ? EnumTypeJourney.Monitoring : EnumTypeJourney.OnBoarding,
            Workload = payrollEmployee.Workload,
            StatusUser = payrollEmployee.StatusUser
          };
          if (_user._idAccount.Equals("5b91299a17858f95ffdb79f6"))
          // Ajuste de jornada para temporários e estagiários da UNIMED NORDESTE RS
          {
            if (person.Establishment.Name.StartsWith("Estagiários") || person.Establishment.Name.StartsWith("Multy Pessoal"))
            {
              person.TypeJourney = EnumTypeJourney.OutOfJourney;
            }
          }
          // Afastados aparecem fora de jornada
          person.TypeJourney = person.StatusUser == EnumStatusUser.Away ? EnumTypeJourney.OutOfJourney : person.TypeJourney;
          // Manager of person contract
          if (personManager != null)
          {
            person.Manager = new ViewBaseFields() { Mail = personManager.User.Mail, Name = personManager.User.Name, _id = personManager._id };
          }
          // User of person contract
          person.User = userService.GetNewVersion(p => p._id == payrollEmployee._idUser).Result.GetViewCrud();
          // Insert
          person = personService.New(person);
        }
        else
        {
          person.Company = company.GetViewList();
          person.Establishment = establishment.GetViewList();
          // Apenas mudar de cargo se mudou na folha de pagamento em relação a última carga
          person.Occupation = changeOcuppation ? occupation.GetViewListResume() : person.Occupation;
          person.Registration = payrollEmployee.Registration;
          person.HolidayReturn = null;
          person.MotiveAside = null;
          person.DateLastOccupation = payrollEmployee.OccupationChangeDate;
          person.Salary = payrollEmployee.Salary;
          person.DateLastReadjust = payrollEmployee.SalaryChangeDate;
          person.DateResignation = payrollEmployee.DemissionDate;
          person.Workload = payrollEmployee.Workload;
          person.StatusUser = payrollEmployee.StatusUser;
          // Ajuste de jornada para temporários e estagiários da UNIMED NORDESTE RS
          if (_user._idAccount.Equals("5b91299a17858f95ffdb79f6"))
          {
            if (person.Establishment.Name.StartsWith("Estagiários") || person.Establishment.Name.StartsWith("Multy Pessoal"))
            {
              person.TypeJourney = EnumTypeJourney.OutOfJourney;
            }
          }
          // Afastados aparecem fora de jornada
          person.TypeJourney = person.StatusUser == EnumStatusUser.Away ? EnumTypeJourney.OutOfJourney : person.TypeJourney;
          // Manager of person contract
          if (personManager != null)
          {
            person.Manager = new ViewBaseFields() { Mail = personManager.User.Mail, Name = personManager.User.Name, _id = personManager._id };
          }
          // User of person contract
          person.User = userService.GetNewVersion(p => p._id == payrollEmployee._idUser).Result?.GetViewCrud();
          // Update
          string updatePerson = personService.Update(person);
        }
        payrollEmployee._idContract = person._id;
        return payrollEmployee;
      }
      catch (Exception ex)
      {
        payrollEmployee.Messages.Add(ex.ToString());
        return payrollEmployee;
      }
    }
    private PayrollEmployee PersonDemission(PayrollEmployee payrollEmployee, PayrollEmployee payrollEmployeePrevious, EnumIntegrationKey integrationKey)
    {
      try
      {
        IntegrationCompany integrationCompany = GetIntegrationCompany(payrollEmployee.Company, payrollEmployee.CompanyName);
        Company company = companyService.GetNewVersion(p => p._id == integrationCompany.IdCompany).Result;
        IntegrationEstablishment integrationEstablishment = null;
        Establishment establishment = null;
        if (company == null)
        {
          payrollEmployee.Messages.Add("Falta integração da empresa");
          payrollEmployee.Messages.Add("Falta integração de estabelecimento");
        }
        else
        {
          // Estabelecimento
          payrollEmployee._idCompany = company._id;
          integrationEstablishment = GetIntegrationEstablishment(payrollEmployee.EstablishmentKey(), payrollEmployee.EstablishmentName, company._id);
          establishment = establishmentService.GetNewVersion(p => p._id == integrationEstablishment.IdEstablishment).Result;
          if (establishment == null)
          {
            payrollEmployee.Messages.Add("Falta integração de estabelecimento");
          }
          else
          {
            payrollEmployee._idEstablishment = establishment._id;
          }
        }
        if (payrollEmployee.Messages.Count() > 0)
          return payrollEmployee;
        ViewCrudPerson person;
        switch (integrationKey)
        {
          case EnumIntegrationKey.CompanyEstablishment:
            person = personService.GetNewVersion(p => p.User.Document == payrollEmployee.Document && p.Company._id == company._id
                && p.Establishment._id == establishment._id && p.Registration == payrollEmployee.Registration).Result?.GetViewCrud();
            break;
          case EnumIntegrationKey.Company:
            person = personService.GetNewVersion(p => p.User.Document == payrollEmployee.Document && p.Company._id == company._id
                && p.Registration == payrollEmployee.Registration).Result?.GetViewCrud();
            break;
          case EnumIntegrationKey.Document:
            person = personService.GetNewVersion(p => p.User.Document == payrollEmployee.Document).Result?.GetViewCrud();
            break;
          default:
            throw new Exception("Tipo de integração de chaves inválida");
        }
        if (person != null)
        {
          // Apenas mudar de cargo se mudou na folha de pagamento em relação a última carga
          person.DateResignation = payrollEmployee.DemissionDate;
          person.StatusUser = payrollEmployee.StatusUser;
          person.User = userService.GetNewVersion(p => p._id == payrollEmployee._idUser).Result?.GetViewCrud();
          string updatePerson = personService.Update(person);
          payrollEmployee._idContract = person._id;
          payrollEmployee._idUser = person.User._id;
          payrollEmployee.StatusIntegration = EnumStatusIntegration.Atualized;
          resultV2.IdContract = person._id;
          resultV2.IdUser = person.User._id;
        }
        else
        {
          payrollEmployee.Messages.Add("Colaborador não localizado.");
        }
        return payrollEmployee;
      }
      catch (Exception ex)
      {
        payrollEmployee.Messages.Add(ex.ToString());
        return payrollEmployee;
      }
    }
    private void PersonDemission(ColaboradorV2Demissao view, EnumIntegrationKey integrationKey)
    {
      try
      {
        ViewCrudPerson person;
        if (view._id == null)
        {
          IntegrationCompany integrationCompany = GetIntegrationCompany(view.Colaborador.Empresa, view.Colaborador.NomeEmpresa);
          Company company = companyService.GetNewVersion(p => p._id == integrationCompany.IdCompany).Result;
          Establishment establishment = null;
          if (company == null)
          {
            resultV2.Mensagem.Add("Falta integração da empresa");
            resultV2.Mensagem.Add("Falta integração de estabelecimento");
          }
          else
          {
            // Estabelecimento
            IntegrationEstablishment integrationEstablishment = GetIntegrationEstablishment(view.Colaborador.ChaveEstabelecimento(), view.Colaborador.NomeEstabelecimento, company._id);
            establishment = establishmentService.GetNewVersion(p => p._id == integrationEstablishment.IdEstablishment).Result;
            if (establishment == null)
            {
              resultV2.Mensagem.Add("Falta integração de estabelecimento");
            }
          }
          if (resultV2.Mensagem.Count() > 0)
            return;
          switch (integrationKey)
          {
            case EnumIntegrationKey.CompanyEstablishment:
              person = personService.GetNewVersion(p => p.User.Document == view.Colaborador.Cpf && p.Company._id == company._id
                  && p.Establishment._id == establishment._id && p.Registration == view.Colaborador.Matricula).Result?.GetViewCrud();
              break;
            case EnumIntegrationKey.Company:
              person = personService.GetNewVersion(p => p.User.Document == view.Colaborador.Cpf && p.Company._id == company._id
                  && p.Registration == view.Colaborador.Matricula).Result?.GetViewCrud();
              break;
            case EnumIntegrationKey.Document:
              person = personService.GetNewVersion(p => p.User.Document == view.Colaborador.Cpf).Result?.GetViewCrud();
              break;
            default:
              throw new Exception("Tipo de integração de chaves inválida");
          }
        }
        else
        {
          person = personService.GetNewVersion(p => p._id == view._id).Result?.GetViewCrud();
        }
        if (person != null)
        {
          // Apenas mudar de cargo se mudou na folha de pagamento em relação a última carga
          person.DateResignation = view.DataDemissao;
          person.StatusUser = EnumStatusUser.Disabled;
          string updatePerson = personService.Update(person);
          resultV2.IdContract = person._id;
          resultV2.IdUser = person.User._id;
        }
        else
        {
          resultV2.Mensagem.Add("Colaborador não localizado.");
        }
      }
      catch (Exception ex)
      {
        resultV2.Mensagem.Add(ex.ToString());
      }
    }
    private string PersonUpdateTypeUser(ColaboradorV2Base view)
    {
      try
      {
        IntegrationCompany integrationCompany = GetIntegrationCompany(view.Empresa, view.NomeEmpresa);
        Company company = companyService.GetNewVersion(p => p._id == integrationCompany.IdCompany).Result;
        IntegrationEstablishment integrationEstablishment = null;
        Establishment establishment = null;
        if (company == null)
        {
          return "Falta integração da empresa";
        }
        else
        {
          // Estabelecimento
          integrationEstablishment = GetIntegrationEstablishment(view.Estabelecimento, view.NomeEstabelecimento, company._id);
          establishment = establishmentService.GetNewVersion(p => p._id == integrationEstablishment.IdEstablishment).Result;
          if (establishment == null)
          {
            return "Falta integração de estabelecimento";
          }
        }
        ViewCrudIntegrationParameter viewCrudIntegrationParameter = GetIntegrationParameter();
        ViewCrudPerson person;

        switch (viewCrudIntegrationParameter.IntegrationKey)
        {
          case EnumIntegrationKey.CompanyEstablishment:
            person = personService.GetNewVersion(p => p.User.Document == view.Cpf && p.Company._id == company._id
                && p.Establishment._id == establishment._id && p.Registration == view.Matricula).Result?.GetViewCrud();
            break;
          case EnumIntegrationKey.Company:
            person = personService.GetNewVersion(p => p.User.Document == view.Cpf && p.Company._id == company._id
                && p.Registration == view.Matricula).Result?.GetViewCrud();
            break;
          case EnumIntegrationKey.Document:
            person = personService.GetNewVersion(p => p.User.Document == view.Cpf).Result?.GetViewCrud();
            break;
          default:
            throw new Exception("Tipo de integração de chaves inválida");
        }
        if (person == null)
        {
          return "Gestor não encontrado";
        }
        switch (person.TypeUser)
        {
          case EnumTypeUser.Support:
            return "Colaborador de suporte";
          case EnumTypeUser.Administrator:
            return "Colaborador é administrador";
          case EnumTypeUser.Manager:
            return "Colaborador já é gestor";
          case EnumTypeUser.Employee:
            person.TypeUser = EnumTypeUser.Manager;
            string updatePerson = personService.Update(person);
            return "Gestor atualizado";
          case EnumTypeUser.Anonymous:
            return "Colaborador anônimo";
          case EnumTypeUser.HR:
            return "Colaborador é de RH";
          case EnumTypeUser.ManagerHR:
            return "Colaborador é gestor de RH";
          default:
            return "Tipo de colaborador não encontrado";
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    #endregion

    #region Colaborador V1
    // Ok
    #region IntegrationStatus
    /// <summary>
    /// Retornar a situação da integração na tela de Dashboard de integração
    /// </summary>
    /// <returns></returns>
    public ViewIntegrationDashboard GetStatusDashboard()
    {
      try
      {
        ViewCrudIntegrationParameter param = GetIntegrationParameter();
        ViewIntegrationDashboard view = new ViewIntegrationDashboard
        {
          CompanyError = integrationCompanyService.GetAllNewVersion(p => p.IdCompany == "000000000000000000000000").Result.Count(),
          EstablishmentError = integrationEstablishmentService.GetAllNewVersion(p => p.IdEstablishment == "000000000000000000000000").Result.Count(),
          OccupationError = integrationOccupationService.GetAllNewVersion(p => p.IdOccupation == "000000000000000000000000").Result.Count(),
          SchoolingError = integrationSchoolingService.GetAllNewVersion(p => p.IdSchooling == "000000000000000000000000").Result.Count(),
          CriticalError = param.CriticalError,
          ProgramVersionExecution = param.ProgramVersionExecution,
          StatusExecution = param.StatusExecution
        };
        return view;
      }
      catch (Exception)
      {
        throw;
      }
    }
    /// <summary>
    /// Validar se existe informação pendente de parametrização da importação
    /// </summary>
    /// <returns></returns>
    public string GetStatusIntegration()
    {
      try
      {
        IntegrationCompany integrationCompany = integrationCompanyService.GetAllNewVersion(p => p.IdCompany == "000000000000000000000000").Result.FirstOrDefault();
        if (integrationCompany != null)
          throw new Exception("Verificar integração de empresas!");

        IntegrationSchooling integrationSchooling = integrationSchoolingService.GetAllNewVersion(p => p.IdSchooling == "000000000000000000000000").Result.FirstOrDefault();
        if (integrationSchooling != null)
          throw new Exception("Verificar integração de escolaridades!");

        IntegrationEstablishment integrationEstablishment = integrationEstablishmentService.GetAllNewVersion(p => p.IdEstablishment == "000000000000000000000000").Result.FirstOrDefault();
        if (integrationEstablishment != null)
          throw new Exception("Verificar integração de estabelecimentos!");

        IntegrationOccupation integrationOccupation = integrationOccupationService.GetAllNewVersion(p => p.IdOccupation == "000000000000000000000000").Result.FirstOrDefault();
        if (integrationOccupation != null)
          throw new Exception("Verificar integração de cargos!");

        return string.Empty;
      }
      catch (Exception e)
      {
        return e.Message;
      }
    }
    #endregion

    // Ok
    #region IntegrationCompany
    public IntegrationCompany GetIntegrationCompany(string key, string name)
    {
      try
      {
        IntegrationCompany item = integrationCompanyService.GetAllNewVersion(p => p.Key == key).Result.FirstOrDefault();
        if (item == null)
        {
          item = new IntegrationCompany()
          {
            Key = key,
            Name = name,
            _idCompany = "000000000000000000000000",
            IdCompany = "000000000000000000000000",
            NameCompany = string.Empty,
            Status = EnumStatus.Enabled
          };
          item = integrationCompanyService.InsertNewVersion(item).Result;
        }
        if (item.IdCompany.Equals("000000000000000000000000"))
        {
          item.Name = name;
          List<Company> companies = companyService.GetAllNewVersion(p => p.Name == name).Result.ToList();
          if (companies.Count == 1)
          {
            item.IdCompany = companies[0]._id;
            item.NameCompany = companies[0].Name;
            Task i = integrationCompanyService.Update(item, null);
          }
        }
        return item;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public List<ViewListIntegrationCompany> CompanyList(ref long total, int count = 10, int page = 1, string filter = "", bool all = false)
    {
      try
      {
        int skip = (count * (page - 1));
        IQueryable<IntegrationCompany> detail;
        if (all)
        {
          detail = integrationCompanyService.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Name).Skip(skip).Take(count).AsQueryable();
          total = integrationCompanyService.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        }
        else
        {
          detail = integrationCompanyService.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdCompany == "000000000000000000000000").Result.OrderBy(p => p.Name).Skip(skip).Take(count).AsQueryable();
          total = integrationCompanyService.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdCompany == "000000000000000000000000").Result;
        }
        List<ViewListIntegrationCompany> result = new List<ViewListIntegrationCompany>();
        foreach (var item in detail)
        {
          result.Add(new ViewListIntegrationCompany()
          {
            _id = item._id,
            Name = item.Name,
            Key = item.Key,
            IdCompany = item.IdCompany.Equals("000000000000000000000000") ? string.Empty : item.IdCompany,
            NameCompany = item.NameCompany
          });
        }
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewListIntegrationCompany CompanyUpdate(string idIntegration, string idCompany)
    {
      try
      {
        IntegrationCompany item = integrationCompanyService.GetAllNewVersion(p => p._id == idIntegration).Result.FirstOrDefault();
        if (item == null)
          throw new Exception("Id integration not found!");
        item.IdCompany = idCompany;
        item.NameCompany = companyService.GetAllNewVersion(p => p._id == idCompany).Result.FirstOrDefault().Name;
        var i = integrationCompanyService.Update(item, null);

        // Validar PayrollEmployee
        Task.Run(() => ValidPayrollEmployee(item));

        return new ViewListIntegrationCompany()
        {
          _id = item._id,
          Name = item.Name,
          Key = item.Key,
          IdCompany = item.IdCompany.Equals("000000000000000000000000") ? string.Empty : item.IdCompany,
          NameCompany = item.NameCompany
        };
      }
      catch (Exception)
      {
        throw;
      }
    }
    public string CompanyDelete(string idIntegration)
    {
      try
      {
        IntegrationCompany item = integrationCompanyService.GetAllNewVersion(p => p._id == idIntegration).Result.FirstOrDefault();
        if (item == null)
          throw new Exception("Id integration not found!");
        integrationCompanyService.Delete(idIntegration, false);
        return "Ok";
      }
      catch (Exception)
      {
        throw;
      }
    }
    public List<ViewListCompany> CompanyRootList(ref long total)
    {
      try
      {
        List<ViewListCompany> result = companyService.GetAllNewVersion()
          .Select(x => new ViewListCompany()
          {
            _id = x._id,
            Name = x.Name
          }).ToList();
        total = processLevelTwoService.CountNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    // Ok
    #region IntegrationEstablishment
    public IntegrationEstablishment GetIntegrationEstablishment(string key, string name, string idcompany)
    {
      try
      {
        IntegrationEstablishment item = integrationEstablishmentService.GetAllNewVersion(p => p.Key == key).Result.FirstOrDefault();
        if (item == null)
        {
          item = new IntegrationEstablishment()
          {
            Key = key,
            Name = name,
            _idCompany = idcompany,
            IdEstablishment = "000000000000000000000000",
            NameEstablishment = string.Empty,
            Status = EnumStatus.Enabled
          };
          item = integrationEstablishmentService.InsertNewVersion(item).Result;
        }
        if (item.IdEstablishment.Equals("000000000000000000000000"))
        {
          item.Name = name;
          item._idCompany = idcompany;
          List<Establishment> establishments = establishmentService.GetAllNewVersion(p => p.Company._id == idcompany && p.Name == name).Result.ToList();
          if (establishments.Count == 1)
          {
            item.IdEstablishment = establishments[0]._id;
            item.NameEstablishment = establishments[0].Name;
            Task i = integrationEstablishmentService.Update(item, null);
          }
        }
        return item;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public List<ViewListIntegrationEstablishment> EstablishmentList(ref long total, int count = 10, int page = 1, string filter = "", bool all = false)
    {
      try
      {
        int skip = (count * (page - 1));
        IQueryable<IntegrationEstablishment> detail;
        if (all)
        {
          detail = integrationEstablishmentService.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Name).Skip(skip).Take(count).AsQueryable();
          total = integrationEstablishmentService.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        }
        else
        {
          detail = integrationEstablishmentService.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdEstablishment == "000000000000000000000000").Result.OrderBy(p => p.Name).Skip(skip).Take(count).AsQueryable();
          total = integrationEstablishmentService.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdEstablishment == "000000000000000000000000").Result;
        }
        List<ViewListIntegrationEstablishment> result = new List<ViewListIntegrationEstablishment>();
        foreach (var item in detail)
        {
          result.Add(new ViewListIntegrationEstablishment()
          {
            _id = item._id,
            Name = item.Name,
            Key = item.Key,
            IdCompany = item._idCompany,
            NameCompany = companyService.GetAllNewVersion(p => p._id == item._idCompany).Result.FirstOrDefault().Name,
            IdEstablishment = item.IdEstablishment.Equals("000000000000000000000000") ? string.Empty : item.IdEstablishment,
            NameEstablishment = item.NameEstablishment
          });
        }
        return result;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ViewListIntegrationEstablishment EstablishmentUpdate(string idIntegration, string idEstablishment)
    {
      try
      {
        IntegrationEstablishment item = integrationEstablishmentService.GetAllNewVersion(p => p._id == idIntegration).Result.FirstOrDefault();
        if (item == null)
          throw new Exception("Id integration not found!");
        item.IdEstablishment = idEstablishment;
        Establishment establishment = establishmentService.GetAllNewVersion(p => p._id == idEstablishment).Result.FirstOrDefault();
        item.NameEstablishment = establishment.Name;
        item._idCompany = establishment.Company._id;
        var i = integrationEstablishmentService.Update(item, null);
        // Validar PayrollEmployee
        Task.Run(() => ValidPayrollEmployee(item));

        return new ViewListIntegrationEstablishment()
        {
          _id = item._id,
          Name = item.Name,
          Key = item.Key,
          IdCompany = item._idCompany,
          NameCompany = companyService.GetAllNewVersion(p => p._id == item._idCompany).Result.FirstOrDefault().Name,
          IdEstablishment = item.IdEstablishment.Equals("000000000000000000000000") ? string.Empty : item.IdEstablishment,
          NameEstablishment = item.NameEstablishment
        };
      }
      catch (Exception)
      {
        throw;
      }
    }
    public string EstablishmentDelete(string idIntegration)
    {
      try
      {
        IntegrationEstablishment item = integrationEstablishmentService.GetAllNewVersion(p => p._id == idIntegration).Result.FirstOrDefault();
        if (item == null)
          throw new Exception("Id integration not found!");
        integrationEstablishmentService.Delete(idIntegration, false);
        return "Ok";
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    // Ok
    #region IntegrationOccupation
    public IntegrationOccupation GetIntegrationOccupation(string key, string name, string idcompany)
    {
      try
      {
        IntegrationOccupation item = integrationOccupationService.GetAllNewVersion(p => p.Key == key).Result.FirstOrDefault();
        if (item == null)
        {
          item = new IntegrationOccupation()
          {
            Key = key,
            Name = name,
            _idCompany = idcompany,
            IdOccupation = "000000000000000000000000",
            NameOccupation = string.Empty,
            Status = EnumStatus.Enabled
          };
          item = integrationOccupationService.InsertNewVersion(item).Result;
        }
        if (item.IdOccupation.Equals("000000000000000000000000"))
        {
          item.Name = name;
          item._idCompany = idcompany;
          // Ajuste para cargos com centro de custo
          List<Occupation> occupations = occupationService.GetAllNewVersion(p => p.Group.Company._id == idcompany && p.Name == name).Result.ToList();
          if (occupations.Count == 1)
          {
            item.IdOccupation = occupations[0]._id;
            item.NameOccupation = occupations[0].Name;
            Task i = integrationOccupationService.Update(item, null);
          }
          else
          {
            occupations = occupationService.GetAllNewVersion(p => p.Group.Company._id == idcompany && p.Name == name && p.Description == null).Result.ToList();
            if (occupations.Count == 1)
            {
              item.IdOccupation = occupations[0]._id;
              item.NameOccupation = occupations[0].Name;
              Task i = integrationOccupationService.Update(item, null);
            }
          }
        }
        return item;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public List<ViewListIntegrationOccupation> OccupationList(ref long total, int count = 10, int page = 1, string filter = "", bool all = false)
    {
      try
      {
        int skip = (count * (page - 1));
        IQueryable<IntegrationOccupation> detail;
        if (all)
        {
          detail = integrationOccupationService.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Name).Skip(skip).Take(count).AsQueryable();
          total = integrationOccupationService.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        }
        else
        {
          detail = integrationOccupationService.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdOccupation == "000000000000000000000000").Result.OrderBy(p => p.Name).Skip(skip).Take(count).AsQueryable();
          total = integrationOccupationService.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdOccupation == "000000000000000000000000").Result;
        }
        List<ViewListIntegrationOccupation> result = new List<ViewListIntegrationOccupation>();
        foreach (var item in detail)
        {
          result.Add(new ViewListIntegrationOccupation()
          {
            _id = item._id,
            Name = item.Name,
            Key = item.Key,
            IdCompany = item._idCompany,
            NameCompany = companyService.GetAllNewVersion(p => p._id == item._idCompany).Result.FirstOrDefault().Name,
            IdOccupation = item.IdOccupation.Equals("000000000000000000000000") ? string.Empty : item.IdOccupation,
            NameOccupation = item.NameOccupation,
            Split = item._idPayrollOccupation == null ? false : !item._idPayrollOccupation.Equals("000000000000000000000000")
          });
        }
        return result;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ViewListIntegrationOccupation OccupationUpdate(string idIntegration, string idOccupation)
    {
      try
      {
        IntegrationOccupation item = integrationOccupationService.GetAllNewVersion(p => p._id == idIntegration).Result.FirstOrDefault();
        if (item == null)
          throw new Exception("Id integration not found!");
        item.IdOccupation = idOccupation;
        Occupation occupation = occupationService.GetAllNewVersion(p => p._id == idOccupation).Result.FirstOrDefault();
        item.NameOccupation = occupation.Name;
        item._idCompany = occupation.Group.Company._id;
        var i = integrationOccupationService.Update(item, null);
        // Validar PayrollEmployee
        Task.Run(() => ValidPayrollEmployee(item));
        return new ViewListIntegrationOccupation()
        {
          _id = item._id,
          Name = item.Name,
          Key = item.Key,
          IdCompany = item._idCompany,
          NameCompany = companyService.GetAllNewVersion(p => p._id == item._idCompany).Result.FirstOrDefault().Name,
          IdOccupation = item.IdOccupation.Equals("000000000000000000000000") ? string.Empty : item.IdOccupation,
          NameOccupation = item.NameOccupation
        };
      }
      catch (Exception)
      {
        throw;
      }
    }
    public string OccupationSplit(string idIntegration)
    {
      try
      {
        IntegrationOccupation item = integrationOccupationService.GetAllNewVersion(p => p._id == idIntegration).Result.FirstOrDefault();
        if (item == null)
          throw new Exception("Id integration not found!");
        PayrollOccupation payrollOccupation = payrollOccupationService.GetNewVersion(p => p.Key == item.Key).Result;
        if (payrollOccupation == null)
        {
          payrollOccupation = new PayrollOccupation()
          {
            Key = item.Key,
            Name = item.Name,
            Split = true
          };
          payrollOccupation = payrollOccupationService.InsertNewVersion(payrollOccupation).Result;
        }
        else
        {
          payrollOccupation.Split = true;
          Task i = payrollOccupationService.Update(payrollOccupation, null);
        }
        integrationOccupationService.Delete(item._id, false);
        return "Ok";
      }
      catch (Exception)
      {
        throw;
      }
    }
    public string OccupationJoin(string idIntegration)
    {
      try
      {
        IntegrationOccupation item = integrationOccupationService.GetAllNewVersion(p => p._id == idIntegration).Result.FirstOrDefault();
        if (item == null)
        {
          throw new Exception("Id integration not found!");
        }
        PayrollOccupation payrollOccupation = payrollOccupationService.GetNewVersion(p => p._id == item._idPayrollOccupation).Result;
        if (payrollOccupation == null)
        {
          throw new Exception("Id integration not split!");
        }
        List<IntegrationOccupation> integrationOccupations = integrationOccupationService.GetAllNewVersion(p => p._idPayrollOccupation == payrollOccupation._id).Result;
        foreach (IntegrationOccupation integrationOccupation in integrationOccupations)
        {
          integrationOccupationService.Delete(integrationOccupation._id, false);
        }
        payrollOccupationService.Delete(payrollOccupation._id, false);
        return "Ok";
      }
      catch (Exception)
      {
        throw;
      }
    }
    public string OccupationDelete(string idIntegration)
    {
      try
      {
        IntegrationOccupation item = integrationOccupationService.GetAllNewVersion(p => p._id == idIntegration).Result.FirstOrDefault();
        if (item == null)
          throw new Exception("Id integration not found!");
        integrationOccupationService.Delete(idIntegration, false);
        return "Ok";
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    // Ok
    #region IntegrationSchooling
    public IntegrationSchooling GetIntegrationSchooling(string key, string name)
    {
      try
      {
        IntegrationSchooling item = integrationSchoolingService.GetAllNewVersion(p => p.Key == key).Result.FirstOrDefault();
        if (item == null)
        {
          item = new IntegrationSchooling()
          {
            Key = key,
            Name = name,
            _idCompany = "000000000000000000000000",
            IdSchooling = "000000000000000000000000",
            NameSchooling = string.Empty,
            Status = EnumStatus.Enabled
          };
          item = integrationSchoolingService.InsertNewVersion(item).Result;
        }
        if (item.IdSchooling.Equals("000000000000000000000000"))
        {
          item.Name = name;
          List<Schooling> schoolings = schoolingService.GetAllNewVersion(p => p.Name == name).Result.ToList();
          if (schoolings.Count == 1)
          {
            item.IdSchooling = schoolings[0]._id;
            item.NameSchooling = schoolings[0].Name;
            Task i = integrationSchoolingService.Update(item, null);
          }
        }
        return item;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public List<ViewListIntegrationSchooling> SchoolingList(ref long total, int count = 10, int page = 1, string filter = "", bool all = false)
    {
      try
      {
        int skip = (count * (page - 1));
        IQueryable<IntegrationSchooling> detail;
        if (all)
        {
          detail = integrationSchoolingService.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Name).Skip(skip).Take(count).AsQueryable();
          total = integrationSchoolingService.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        }
        else
        {
          detail = integrationSchoolingService.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdSchooling == "000000000000000000000000").Result.OrderBy(p => p.Name).Skip(skip).Take(count).AsQueryable();
          total = integrationSchoolingService.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdSchooling == "000000000000000000000000").Result;
        }
        List<ViewListIntegrationSchooling> result = new List<ViewListIntegrationSchooling>();
        foreach (var item in detail)
        {
          result.Add(new ViewListIntegrationSchooling()
          {
            _id = item._id,
            Name = item.Name,
            Key = item.Key,
            IdSchooling = item.IdSchooling.Equals("000000000000000000000000") ? string.Empty : item.IdSchooling,
            NameSchooling = item.NameSchooling
          });
        }
        return result;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ViewListIntegrationSchooling SchoolingUpdate(string idIntegration, string idSchooling)
    {
      try
      {
        IntegrationSchooling item = integrationSchoolingService.GetAllNewVersion(p => p._id == idIntegration).Result.FirstOrDefault();
        if (item == null)
          throw new Exception("Id integration not found!");
        item.IdSchooling = idSchooling;
        item.NameSchooling = schoolingService.GetAllNewVersion(p => p._id == idSchooling).Result.FirstOrDefault().Name;
        var i = integrationSchoolingService.Update(item, null);
        // Validar PayrollEmployee
        Task.Run(() => ValidPayrollEmployee(item));
        return new ViewListIntegrationSchooling()
        {
          _id = item._id,
          Name = item.Name,
          Key = item.Key,
          IdSchooling = item.IdSchooling.Equals("000000000000000000000000") ? string.Empty : item.IdSchooling,
          NameSchooling = item.NameSchooling
        };
      }
      catch (Exception)
      {
        throw;
      }
    }
    public string SchoolingDelete(string idIntegration)
    {
      try
      {
        IntegrationSchooling item = integrationSchoolingService.GetAllNewVersion(p => p._id == idIntegration).Result.FirstOrDefault();
        if (item == null)
          throw new Exception("Id integration not found!");
        integrationSchoolingService.Delete(idIntegration, false);
        return "Ok";
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    // Ok
    #region IntegrationParameter
    /// <summary>
    /// Localizar os parâmetros de integração
    /// </summary>
    /// <returns></returns>
    public ViewCrudIntegrationParameter GetIntegrationParameter()
    {
      try
      {
        IntegrationParameter param = parameterService.GetAllNewVersion().FirstOrDefault();
        if (param == null)
        {
          param = parameterService.InsertNewVersion(new IntegrationParameter()
          {
            Mode = EnumIntegrationMode.DataBase,
            Version = EnumIntegrationVersion.V1,
            IntegrationKey = EnumIntegrationKey.Company,
            CultureDate = "pt-BR",
            Status = EnumStatus.Enabled
          }).Result;
        }
        return new ViewCrudIntegrationParameter()
        {
          ConnectionString = param.ConnectionString,
          CriticalError = param.CriticalError,
          FilePathLocal = param.FilePathLocal,
          SheetName = param.SheetName,
          LastExecution = param.LastExecution,
          MachineIdentity = param.MachineIdentity,
          Mode = param.Mode,
          ProgramVersionExecution = param.ProgramVersionExecution,
          SqlCommand = param.SqlCommand,
          StatusExecution = param.StatusExecution,
          Version = param.Version,
          ApiIdentification = param.ApiIdentification,
          IntegrationKey = param.IntegrationKey,
          CultureDate = param.CultureDate,
          ApiToken = param.ApiToken,
          _id = param._id
        };
      }
      catch (Exception)
      {
        throw;
      }
    }
    /// <summary>
    /// Atualizar os parâmetros de integração
    /// </summary>
    /// <param name="view">View de manutenção dos parâmetros de integração</param>
    /// <returns>View de manutenção dos parâmetros de integração</returns>
    public ViewCrudIntegrationParameter SetIntegrationParameter(ViewCrudIntegrationParameter view)
    {
      try
      {
        IntegrationParameter param = parameterService.GetAllNewVersion().FirstOrDefault();
        if (param == null)
          throw new Exception("Parameter Integration not found!");
        param.Mode = view.Mode;
        param.Version = view.Version;
        param.IntegrationKey = view.IntegrationKey;
        param.CultureDate = view.CultureDate;

        param.ConnectionString = view.ConnectionString;
        param.SqlCommand = view.SqlCommand;
        param.FilePathLocal = view.FilePathLocal;
        param.SheetName = view.SheetName;
        param.StatusExecution = view.StatusExecution;
        param.ProgramVersionExecution = view.ProgramVersionExecution;
        param.CriticalError = view.CriticalError;
        param.MachineIdentity = view.MachineIdentity;
        param.ApiIdentification = view.ApiIdentification;
        param.ApiToken = view.ApiToken;
        parameterService.Update(param, null).Wait();
        return new ViewCrudIntegrationParameter()
        {
          Mode = param.Mode,
          Version = param.Version,
          IntegrationKey = param.IntegrationKey,
          CultureDate = param.CultureDate,
          ConnectionString = param.ConnectionString,
          CriticalError = param.CriticalError,
          FilePathLocal = param.FilePathLocal,
          SheetName = param.SheetName,
          LastExecution = param.LastExecution,
          MachineIdentity = param.MachineIdentity,
          ProgramVersionExecution = param.ProgramVersionExecution,
          SqlCommand = param.SqlCommand,
          StatusExecution = param.StatusExecution,
          ApiIdentification = param.ApiIdentification,
          ApiToken = param.ApiToken,
          _id = param._id
        };
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    // Ok
    #region IntegrationPerson
    public IntegrationPerson GetIntegrationPerson(string key)
    {
      try
      {
        return integrationPersonService.GetAllNewVersion(p => p.Key == key).Result.FirstOrDefault();
      }
      catch (Exception)
      {

        throw;
      }
    }
    public void PostIntegrationPerson(IntegrationPerson integrationPerson)
    {
      try
      {
        integrationPersonService.InsertNewVersion(integrationPerson).Wait();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public void PutIntegrationPerson(IntegrationPerson integrationPerson)
    {
      try
      {
        integrationPersonService.Update(integrationPerson, null).Wait();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public List<string> EmployeeChange(ViewColaborador oldEmployee, ViewColaborador newEmployee)
    {
      try
      {
        var type = typeof(ViewColaborador);
        List<string> fields = new List<string>();
        var unequalProperties =
            from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            let selfValue = type.GetProperty(pi.Name).GetValue(oldEmployee, null)
            let toValue = type.GetProperty(pi.Name).GetValue(newEmployee, null)
            where selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue))
            select new ViewColaboradorMudanca() { Campo = pi.Name, ValorAntigo = selfValue.ToString(), ValorNovo = toValue.ToString() };
        if (unequalProperties.Count() != 0)
        {
          foreach (var item in unequalProperties.ToList<ViewColaboradorMudanca>())
            fields.Add(item.Campo);
        }
        return fields;
      }
      catch (Exception)
      {
        return new List<string>();
      }
    }
    #endregion

    // Ok
    #region Gets isolados por id
    public ViewListSchooling GetSchooling(string id)
    {
      try
      {
        return schoolingService.GetNewVersion(p => p._id == id).Result?.GetViewList();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ViewListCompany GetCompany(string id)
    {
      try
      {
        return companyService.GetNewVersion(p => p._id == id).Result?.GetViewList();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ViewListEstablishment GetEstablishment(string id)
    {
      try
      {
        return establishmentService.GetNewVersion(p => p._id == id).Result?.GetViewList();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ViewListOccupationResume GetOccupation(string id)
    {
      try
      {
        return occupationService.GetNewVersion(p => p._id == id).Result?.GetViewListResume();
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    // Ok
    #region Gets isolados por Key
    public ViewCrudUser GetUserByKey(string document)
    {
      try
      {
        return userService.GetNewVersion(p => p.Document == document).Result?.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudPerson GetPersonByKey(string idcompany, string idestablishment, string document, string registration)
    {
      try
      {
        ViewCrudIntegrationParameter viewCrudIntegrationParameter = GetIntegrationParameter();
        IQueryable<Person> personsDocument = personService.GetAllNewVersion(p => p.User.Document == document).Result.AsQueryable();
        if (personsDocument.Count() == 0)
          return null;
        Person person = null;
        switch (viewCrudIntegrationParameter.IntegrationKey)
        {
          case EnumIntegrationKey.CompanyEstablishment:
            person = personsDocument.Where(p => p.Company._id == idcompany && p.Establishment._id == idestablishment && p.Registration == registration).FirstOrDefault();
            break;
          case EnumIntegrationKey.Company:
            person = personsDocument.Where(p => p.Company._id == idcompany && p.Registration == registration).FirstOrDefault();
            break;
          case EnumIntegrationKey.Document:
            person = personsDocument.Where(p => p.User.Document == document).FirstOrDefault();
            break;
          default:
            throw new Exception("Tipo de integração de chaves inválida");
        }
        return person == null
          ? null
          : new ViewCrudPerson()
          {
            _id = person._id,
            Company = new ViewListCompany() { _id = person.Company._id, Name = person.Company.Name },
            DateLastOccupation = person.DateLastOccupation,
            DateLastReadjust = person.DateLastReadjust,
            DateResignation = person.DateResignation,
            Establishment = person.Establishment == null ? null : new ViewListEstablishment() { _id = person.Establishment._id, Name = person.Establishment.Name },
            HolidayReturn = person.HolidayReturn,
            Manager = person.Manager == null ? null : new ViewBaseFields()
            {
              _id = person.Manager._id,
              Name = person.Manager.Name,
              Mail = person.Manager.Mail
            },
            MotiveAside = person.MotiveAside,
            Occupation = person.Occupation,
            Registration = person.Registration,
            Salary = person.Salary,
            StatusUser = person.StatusUser,
            TypeJourney = person.TypeJourney,
            TypeUser = person.TypeUser,
            User = person.User
          };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region ProcessLevelTwo
    public List<ViewListProcessLevelTwo> ProcessLevelTwoList(ref long total)
    {
      try
      {
        List<ViewListProcessLevelTwo> result = processLevelTwoService.GetAllNewVersion().OrderBy(o => o.Name)
          .Select(x => new ViewListProcessLevelTwo()
          {
            _id = x._id,
            Name = x.Name,
            Order = x.Order,
            ProcessLevelOne = new
            ViewListProcessLevelOne()
            {
              _id = x.ProcessLevelOne._id,
              Name = x.ProcessLevelOne.Name,
              Order = x.ProcessLevelOne.Order,
              Area = x.ProcessLevelOne.Area
            }
          }).ToList();
        total = processLevelTwoService.CountNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        return result;
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #region Skill
    public ViewCrudSkill IntegrationSkill(ViewCrudSkill view)
    {
      var skill = skillService.GetNewVersion(p => p.Name == view.Name).Result;
      if (skill == null)
      {
        skill = new Skill()
        {
          Name = view.Name,
          Concept = view.Concept,
          TypeSkill = view.TypeSkill,
          Template = null
        };
        skill = skillService.InsertNewVersion(skill).Result;
      }
      return new ViewCrudSkill()
      {
        _id = skill._id,
        Name = skill.Name,
        Concept = skill.Concept,
        TypeSkill = skill.TypeSkill
      };
    }
    #endregion

    #region Occupation Profile
    public ViewIntegrationProfileOccupation IntegrationProfile(ViewIntegrationProfileOccupation view)
    {
      Group group = groupService.GetNewVersion(p => p.Name == view.NameGroup).Result;
      if (group == null)
      {
        view.Messages.Add("Grupo de cargo não encontrado!!!");
        return view;
      }
      ProcessLevelTwo subProcess = null;
      if (!string.IsNullOrEmpty(view.Area) && !string.IsNullOrEmpty(view.Process) && !string.IsNullOrEmpty(view.SubProcess))
      {
        subProcess = processLevelTwoService.GetNewVersion(p => p.Name == view.SubProcess && p.ProcessLevelOne.Name == view.Process && p.ProcessLevelOne.Area.Name == view.Area).Result;
      }
      else
      {
        subProcess = processLevelTwoService.GetNewVersion(p => p.Name == "Integração folha de pagamento").Result;
      }
      if (subProcess == null)
      {
        view.Messages.Add("Subprocesso de integração não localizado.");
        return view;
      }
      Occupation occupation = string.IsNullOrEmpty(view.Description)
        ? occupationService.GetNewVersion(p => p.Name == view.Name && (p.Description == null || p.Description == string.Empty)).Result
        : occupationService.GetNewVersion(p => p.Name == view.Name && p.Description == view.Description).Result;
      if (occupation == null)
      {
        // GOTO: talvez aqui
        occupation = new Occupation()
        {
          Group = group.GetViewList(),
          Cbo = null,
          Name = CapitalizeOccupation(view.Name),
          Description = string.IsNullOrEmpty(view.Description) ? null : view.Description,
          SpecificRequirements = view.SpecificRequirements,
          SalaryScales = null,
          Process = new List<ViewListProcessLevelTwo>(),
          Skills = new List<ViewListSkill>(),
          Activities = new List<ViewListActivitie>(),
          Schooling = group.Schooling?.Select(p => p.GetViewCrud()).ToList(),
          Line = 0
        };
        occupation.Process.Add(subProcess.GetViewList());
      }
      else
      {
        occupation.Skills = new List<ViewListSkill>();
        occupation.SpecificRequirements = view.SpecificRequirements;
        occupation.Activities = new List<ViewListActivitie>();
        occupation.Schooling = group.Schooling?.Select(p => p.GetViewCrud()).ToList();
      }
      string itemAux;
      Skill skill;
      foreach (string item in view.Skills)
      {
        itemAux = item;
        skill = skillService.GetNewVersion(p => p.Name == itemAux).Result;
        if (skill == null)
        {
          if (view.UpdateSkill)
          {
            skill = new Skill()
            {
              Name = itemAux,
              TypeSkill = EnumTypeSkill.Hard,
              Concept = string.Empty
            };
            skill = skillService.InsertNewVersion(skill).Result;
            occupation.Skills.Add(skill.GetViewList());
          }
          else
          {
            view.Messages.Add(string.Format("{0}@ competência não cadastrada", itemAux));
          }
        }
        else
        {
          occupation.Skills.Add(skill.GetViewList());
        }
      }
      int order = 0;
      foreach (string item in view.Activities)
      {
        order++;
        occupation.Activities.Add(new ViewListActivitie()
        {
          _id = ObjectId.GenerateNewId().ToString(),
          Name = item,
          Order = order
        });
      }
      Schooling schooling;
      for (int i = 0; i < view.Schooling.Count; i++)
      {
        itemAux = view.Schooling[i];
        schooling = schoolingService.GetNewVersion(p => p.Name == itemAux).Result;
        if (schooling == null)
        {
          view.Messages.Add(string.Format("{0}@ escolaridade não cadastrada", itemAux));
        }
        else
        {
          bool achou = false;
          for (int lin = 0; lin < occupation.Schooling.Count; lin++)
          {
            if (occupation.Schooling[lin].Name.ToUpper().Equals(itemAux))
            {
              occupation.Schooling[lin].Complement = view.SchoolingComplement[i];
              achou = true;
              break;
            }
          }
          if (!achou)
          {
            view.Messages.Add(string.Format("{0}@ escolaridade não localizada", itemAux));
          }
        }
      }
      if (view.Messages.Count == 0 && view.Update)
      {
        if (occupation._id == null)
        {
          occupation = occupationService.InsertNewVersion(occupation).Result;
        }
        else
        {
          Task retorno = occupationService.Update(occupation, null);
        }
      }
      return new ViewIntegrationProfileOccupation()
      {
        _id = occupation._id,
        Name = occupation.Name,
        Description = occupation.Description,
        NameGroup = occupation.Group.Name,
        Activities = occupation.Activities.OrderBy(o => o.Order).Select(x => x.Name).ToList(),
        Skills = occupation.Skills.OrderBy(o => o.Name).Select(x => x.Name).ToList(),
        SpecificRequirements = occupation.SpecificRequirements,
        Schooling = occupation.Schooling.OrderBy(o => o.Order).Select(x => x.Name).ToList(),
        SchoolingComplement = occupation.Schooling.OrderBy(o => o.Order).Select(x => x.Complement).ToList(),
        Messages = view.Messages,
        Update = view.Update,
        Area = occupation.Process[0].ProcessLevelOne.Area.Name,
        Process = occupation.Process[0].ProcessLevelOne.Name,
        SubProcess = occupation.Process[0].Name,
        UpdateSkill = view.UpdateSkill
      };
    }

    public List<ViewListOccupation> GetExportOccupations()
    {
      try
      {
        return occupationService.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result.OrderBy(p => p.Name).ThenBy(o => o.Description)
          .Select(p => p.GetViewList()).ToList();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public ViewMapOccupation GetExportOccupation(string id)
    {
      try
      {
        return serviceInfra.GetMapOccupation(id);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public List<ViewListGroup> GetExportGroups()
    {
      try
      {
        return groupService.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result.OrderBy(o => o.Line)
          .Select(x => x.GetViewList()).ToList();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    #endregion

    #endregion

    #region Colaborador V2

    #region IntegrationOccupation
    public IntegrationOccupation GetIntegrationOccupation(string key, string name, string idcompany, string costCenterKey, string costCenterName)
    {
      try
      {
        PayrollOccupation payrollOccupation = payrollOccupationService.GetNewVersion(p => p.Key == key).Result;
        string localKey = key;
        string localName = name;
        bool split = false;
        if (payrollOccupation != null && payrollOccupation.Split)
        {
          localKey = string.Format("{0};{1}", key, costCenterKey);
          localName = string.Format("{0} - {1}", name, costCenterName);
          split = true;
        }
        IntegrationOccupation item = integrationOccupationService.GetAllNewVersion(p => p.Key == localKey).Result.FirstOrDefault();
        if (item == null)
        {
          item = new IntegrationOccupation()
          {
            Key = localKey,
            Name = localName,
            _idCompany = idcompany,
            IdOccupation = "000000000000000000000000",
            NameOccupation = string.Empty,
            Status = EnumStatus.Enabled,
            _idPayrollOccupation = payrollOccupation == null ? "000000000000000000000000" : payrollOccupation._id
          };
          Task<IntegrationOccupation> i = integrationOccupationService.InsertNewVersion(item);
        }
        if (item.IdOccupation.Equals("000000000000000000000000"))
        {
          item.Name = localName;
          item._idCompany = idcompany;
          if (split)
          {
            item._idPayrollOccupation = payrollOccupation?._id;
            List<Occupation> occupations = occupationService.GetAllNewVersion(p => p.Group.Company._id == idcompany && p.Name == name && p.Description == costCenterName).Result.ToList();
            if (occupations.Count == 1)
            {
              item.IdOccupation = occupations[0]._id;
              item.NameOccupation = occupations[0].Name;
              Task i = integrationOccupationService.Update(item, null);
            }
          }
          else
          {
            item._idPayrollOccupation = "000000000000000000000000";
            List<Occupation> occupations = occupationService.GetAllNewVersion(p => p.Group.Company._id == idcompany && p.Name == name && p.Description == null).Result.ToList();
            if (occupations.Count == 1)
            {
              item.IdOccupation = occupations[0]._id;
              item.NameOccupation = occupations[0].Name;
              Task i = integrationOccupationService.Update(item, null);
            }
          }
        }
        return item;
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    public ColaboradorV2Retorno IntegrationV2(ColaboradorV2Completo view)
    {
      EnumSex sexo = EnumSex.Others;
      EnumStatusUser situacao = EnumStatusUser.Enabled;
      EnumActionIntegration acao = EnumActionIntegration.Change;
      try
      {
        resultV2 = new ColaboradorV2Retorno
        {
          Situacao = "Ok",
          Mensagem = new List<string>()
        };
        // Validação de campos especiais
        view.Colaborador = ValidKeyEmployee(view.Colaborador, " ");
        acao = ValidAcction(view.Acao);
        sexo = ValidSex(view.Sexo);
        situacao = ValidSituation(view.Situacao);
        // Validação de campos vazios obrigatórios
        ValidEmptyString(view.Nome, "Nome deve ser informado");
        view.Nome = CapitalizeName(view.Nome);
        ValidEmptyString(view.NomeCargo, "Nome do cargo deve ser informado");
        view.NomeCargo = CapitalizeOccupation(view.NomeCargo);
        view.Cargo = EmptyStringDefault(view.Cargo, view.NomeCargo);
        ValidEmptyDate(view.DataAdmissao, "Data de admissão deve ser informada");
        // Preenchimento de campos opcionais, caso vazios
        view.Email = view.Email?.ToLower();
        view.NomeGrauInstrucao = EmptyStringDefault(view.NomeGrauInstrucao, "Não definida");
        view.GrauInstrucao = EmptyStringDefault(view.GrauInstrucao, view.NomeGrauInstrucao);
        view.NomeGrauInstrucao = CapitalizeName(view.NomeGrauInstrucao);
        view.CentroCusto = EmptyStringDefault(view.CentroCusto, view.NomeCentroCusto);
        // Validação do Gestor
        if (view.Gestor != null && !string.IsNullOrEmpty(view.Gestor.Matricula))
        {
          view.Gestor = ValidKeyEmployee(view.Gestor, " do gestor ");
        }
        else
        {
          view.Gestor = null;
        }
        IntegrationParameter param = parameterService.GetAllNewVersion().FirstOrDefault();
        if (param == null)
        {
          resultV2.Mensagem.Add("Não existe parâmetro de integração.");
        }
        // Fim da validação de dados
        if (resultV2.Mensagem.Count != 0)
        {
          return resultV2;
        }
        // Atualização da base de dados
        List<PayrollEmployee> payrollEmployees = new List<PayrollEmployee>();
        switch (param.IntegrationKey)
        {
          case EnumIntegrationKey.CompanyEstablishment:
            payrollEmployees = payrollEmployeeService.GetAllNewVersion(p => p.Key1 == view.Colaborador.Chave1() && p.StatusIntegration != EnumStatusIntegration.Reject).Result.OrderByDescending(o => o.DateRegister).ToList();
            break;
          case EnumIntegrationKey.Company:
            payrollEmployees = payrollEmployeeService.GetAllNewVersion(p => p.Key2 == view.Colaborador.Chave2() && p.StatusIntegration != EnumStatusIntegration.Reject).Result.OrderByDescending(o => o.DateRegister).ToList();
            break;
          case EnumIntegrationKey.Document:
            payrollEmployees = payrollEmployeeService.GetAllNewVersion(p => p.Document == view.Colaborador.Cpf && p.StatusIntegration != EnumStatusIntegration.Reject).Result.OrderByDescending(o => o.DateRegister).ToList();
            break;
          default:
            throw new Exception("Tipo de integração de chaves inválida");
        }
        if (payrollEmployees.Count() == 0 && (acao == EnumActionIntegration.Demission || acao == EnumActionIntegration.Change))
        {
          resultV2.Mensagem.Add("Colaborador não está na base de integração.");
          return resultV2;
        }
        PayrollEmployee payrollEmployee = new PayrollEmployee
        {
          // Identificação
          Key1 = view.Colaborador.Chave1(),
          Key2 = view.Colaborador.Chave2(),
          DateRegister = DateTime.Now,
          Action = acao,
          StatusIntegration = EnumStatusIntegration.Saved,
          // Campos
          Document = view.Colaborador.Cpf,
          Company = view.Colaborador.Empresa,
          CompanyName = view.Colaborador.NomeEmpresa,
          Establishment = view.Colaborador.Estabelecimento,
          EstablishmentName = view.Colaborador.NomeEstabelecimento,
          Registration = view.Colaborador.Matricula,
          Name = view.Nome,
          Mail = view.Email,
          Sex = sexo,
          BirthDate = view.DataNascimento,
          CellNumber = view.Celular,
          Schooling = view.GrauInstrucao,
          SchoolingName = view.NomeGrauInstrucao,
          Nickname = view.Apelido,
          AdmissionDate = view.DataAdmissao,
          DemissionDate = view.DataDemissao,
          Occupation = view.Cargo,
          OccupationName = view.NomeCargo,
          OccupationChangeDate = view.DataTrocaCargo,
          CostCenter = view.CentroCusto,
          CostCenterName = view.NomeCentroCusto,
          CostCenterChangeDate = view.DataTrocaCentroCusto,
          Salary = view.SalarioNominal,
          Workload = Convert.ToInt32(view.CargaHoraria),
          SalaryChangeDate = view.DataUltimoReajuste,
          SalaryChangeReason = view.MotivoUltimoReajuste,
          StatusUser = situacao,
          ManagerDocument = view.Gestor?.Cpf,
          ManagerCompany = view.Gestor?.Empresa,
          ManagerCompanyName = view.Gestor?.NomeEmpresa,
          ManagerEstablishment = view.Gestor?.Estabelecimento,
          ManagerEstablishmentName = view.Gestor?.NomeEstabelecimento,
          ManagerRegistration = view.Gestor?.Matricula,
          Messages = new List<string>()
        };
        // Não tem registros anteriores (VAZIO)
        if (payrollEmployees.Count() == 0)
        {
          InsertHistory(payrollEmployee, true);
          return resultV2;
        };
        // Comparar com o registro anterior
        PayrollEmployee payrollEmployeePrevious = payrollEmployees.FirstOrDefault();
        if (payrollEmployee.Equal(payrollEmployeePrevious))
        {
          // Se for igual
          resultV2.IdUser = payrollEmployeePrevious._idUser;
          resultV2.IdContract = payrollEmployeePrevious._idContract;
          resultV2.IdPayrollEmployee = payrollEmployeePrevious._id;
          resultV2.Mensagem.Add("Colaborador sem alterações");
          return resultV2;
        }
        // Se for diferente
        if (payrollEmployeePrevious.StatusIntegration == EnumStatusIntegration.Atualized)
        {
          payrollEmployee._idPrevious = payrollEmployeePrevious._id;
          InsertHistory(payrollEmployee, payrollEmployee.OccupationName != payrollEmployeePrevious.OccupationName);
          return resultV2;
        }
        // Anterior e novo são iguais, não gravar o novo e retornar mensagens do antigo.
        payrollEmployeePrevious.StatusIntegration = EnumStatusIntegration.Reject;
        Task taskPrevious = payrollEmployeeService.Update(payrollEmployeePrevious, null);
        payrollEmployee._idPrevious = payrollEmployeePrevious._idPrevious;
        InsertHistory(payrollEmployee, payrollEmployee.OccupationName != payrollEmployeePrevious.OccupationName);
        return resultV2;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public ColaboradorV2Retorno IntegrationV2(ColaboradorV2Demissao view)
    {
      try
      {
        resultV2 = new ColaboradorV2Retorno
        {
          Situacao = "Ok",
          Mensagem = new List<string>()
        };
        List<PayrollEmployee> payrollEmployees = new List<PayrollEmployee>();
        IntegrationParameter param = parameterService.GetAllNewVersion().FirstOrDefault();
        if (param == null)
        {
          resultV2.Mensagem.Add("Não existe parâmetro de integração.");
          return resultV2;
        }
        if (view._id == null)
        {
          // Validação de campos especiais
          view.Colaborador = ValidKeyEmployee(view.Colaborador, " ");
          view.DataDemissao = EmptyDateDefault(view.DataDemissao, DateTime.Now.Date);
          // Atualização da base de dados
          switch (param.IntegrationKey)
          {
            case EnumIntegrationKey.CompanyEstablishment:
              payrollEmployees = payrollEmployeeService.GetAllNewVersion(p => p.Key1 == view.Colaborador.Chave1() && p.StatusIntegration != EnumStatusIntegration.Reject).Result.OrderByDescending(o => o.DateRegister).ToList();
              break;
            case EnumIntegrationKey.Company:
              payrollEmployees = payrollEmployeeService.GetAllNewVersion(p => p.Key2 == view.Colaborador.Chave2() && p.StatusIntegration != EnumStatusIntegration.Reject).Result.OrderByDescending(o => o.DateRegister).ToList();
              break;
            case EnumIntegrationKey.Document:
              payrollEmployees = payrollEmployeeService.GetAllNewVersion(p => p.Document == view.Colaborador.Cpf && p.StatusIntegration != EnumStatusIntegration.Reject).Result.OrderByDescending(o => o.DateRegister).ToList();
              break;
            default:
              throw new Exception("Tipo de integração de chaves inválida");
          }
        } else {
          payrollEmployees = payrollEmployeeService.GetAllNewVersion(p => p._idContract == view._id && p.StatusIntegration != EnumStatusIntegration.Reject).Result.OrderByDescending(o => o.DateRegister).ToList();
        }
        if (payrollEmployees.Count == 0)
        {
          // Sem histórico na PayrollEmployee
          PersonDemission(view, param.IntegrationKey);
          if (resultV2.Mensagem.Count == 0)
          {
            resultV2.Situacao = "Ok";
            resultV2.Mensagem.Add("Colaborador demitido");
          }
        }
        else
        {
          // Com histórico na PayrollEmployee
          PayrollEmployee payrollEmployeePrevious = payrollEmployees.FirstOrDefault();
          PayrollEmployee payrollEmployee = null;
          if (payrollEmployeePrevious.StatusIntegration == EnumStatusIntegration.Saved)
          {
            payrollEmployeePrevious = null;
            payrollEmployee = payrollEmployees.FirstOrDefault();
            payrollEmployee.Action = EnumActionIntegration.Demission;
            payrollEmployee.DateRegister = DateTime.Now.Date;
            payrollEmployee.StatusUser = EnumStatusUser.Disabled;
            payrollEmployee.DemissionDate = view.DataDemissao;
          }
          else
          {
            payrollEmployee = payrollEmployees.FirstOrDefault();
            payrollEmployee.Action = EnumActionIntegration.Demission;
            payrollEmployee.DateRegister = DateTime.Now.Date;
            payrollEmployee._idPrevious = payrollEmployeePrevious._id;
            payrollEmployee._id = null;
            payrollEmployee.StatusUser = EnumStatusUser.Disabled;
            payrollEmployee.DemissionDate = view.DataDemissao;
            payrollEmployee = payrollEmployeeService.InsertNewVersion(payrollEmployee).Result;
          }
          payrollEmployee = PersonDemission(payrollEmployee, payrollEmployeePrevious, param.IntegrationKey);
          Task task = payrollEmployeeService.Update(payrollEmployee, null);
          resultV2.IdPayrollEmployee = payrollEmployee._id;
          for (int i = 0; i < payrollEmployee.Messages.Count; i++)
          {
            resultV2.Mensagem.Add(payrollEmployee.Messages[i]);
          }
          if (resultV2.Mensagem.Count == 0)
          {
            resultV2.Situacao = "Ok";
            resultV2.Mensagem.Add("Colaborador demitido");
          }
        }
        return resultV2;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ColaboradorV2 GetV2(ColaboradorV2Base view)
    {
      try
      {
        // TODO: validar pesquisa por duas chaves
        PayrollEmployee payrollEmployee = payrollEmployeeService.GetAllNewVersion(p => p.Key1 == view.Chave1()).Result.OrderBy(o => o.DateRegister).LastOrDefault();
        if (payrollEmployee == null)
          payrollEmployee = payrollEmployeeService.GetAllNewVersion(p => p.Key2 == view.Chave2()).Result.OrderBy(o => o.DateRegister).LastOrDefault();
        if (payrollEmployee == null)
          throw new Exception("Colaborador não encontrado na integração");
        return payrollEmployee.GetColaboradorV2();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ColaboradorV2 GetV2(string id)
    {
      try
      {
        PayrollEmployee payrollEmployee = payrollEmployeeService.GetNewVersion(p => p._id == id).Result;
        if (payrollEmployee == null)
          throw new Exception("Colaborador não encontrado na integração");
        return payrollEmployee.GetColaboradorV2();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public List<ColaboradorV2Ativo> GetActiveV2()
    {
      try
      {
        List<IntegrationEstablishment> integrationEstablishments = integrationEstablishmentService.GetAllNewVersion().ToList();
        List<Person> persons = personService.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled &&
          p.TypeUser > EnumTypeUser.Administrator && p.Company != null && p.Establishment != null).Result;
        return persons.Select(x => new ColaboradorV2Ativo()
        {
          _id = x._id,
          Cpf = x.User.Document,
          Chaves = integrationEstablishments.FindAll(e => e._idCompany == x.Company._id && e.IdEstablishment == x.Establishment._id).ToList()
              .Select(y => y.Key).ToList(),
          Matricula = x.Registration
        }).ToList();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public string PerfilGestorV2(ColaboradorV2Base view)
    {
      try
      {
        return PersonUpdateTypeUser(view);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    #region Valid CPF
    private bool VerficarSeTodosOsDigitosSaoIdenticos(string cpf)
    {
      var previous = -1;
      for (var i = 0; i < cpf.Length; i++)
      {
        if (char.IsDigit(cpf[i]))
        {
          var digito = cpf[i] - '0';
          if (previous == -1)
            previous = digito;
          else
            if (previous != digito)
            return false;
        }
      }
      return true;
    }
    private bool IsValidCPF(string cpf)
    {
      if (cpf.Length != 11)
        return false;
      if (VerficarSeTodosOsDigitosSaoIdenticos(cpf))
        return false;

      int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
      int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
      string tempCpf;
      string digito;
      int soma;
      int resto;

      tempCpf = cpf.Substring(0, 9);
      soma = 0;
      for (int i = 0; i < 9; i++)
        soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
      resto = soma % 11;
      if (resto < 2)
        resto = 0;
      else
        resto = 11 - resto;
      digito = resto.ToString();
      tempCpf = tempCpf + digito;
      soma = 0;
      for (int i = 0; i < 10; i++)
        soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
      resto = soma % 11;
      if (resto < 2)
        resto = 0;
      else
        resto = 11 - resto;
      digito = digito + resto.ToString();
      return cpf.EndsWith(digito);
    }
    #endregion

    #region Valid Fields
    private ColaboradorV2Base ValidKeyEmployee(ColaboradorV2Base view, string messageComplement)
    {
      try
      {
        ValidEmptyString(view.Cpf, string.Format("CPF{0}deve ser informado", messageComplement));
        if (resultV2.Mensagem.Count == 0)
        {
          view.Cpf = view.Cpf.Trim().Replace(".", string.Empty).Replace("-", string.Empty).PadLeft(11, '0');
          if (!IsValidCPF(view.Cpf))
            resultV2.Mensagem.Add(string.Format("CPF{0}inválido", messageComplement));
        }
        ValidEmptyString(view.NomeEmpresa, string.Format("Nome da Empresa{0}deve ser informada", messageComplement));
        view.Empresa = EmptyStringDefault(view.Empresa, view.NomeEmpresa);
        view.NomeEstabelecimento = EmptyStringDefault(view.NomeEstabelecimento, "Estabelecimento Padrão");
        view.Estabelecimento = EmptyStringDefault(view.Estabelecimento, view.NomeEstabelecimento);
        ValidEmptyString(view.Matricula, string.Format("Matrícula{0}deve ser informada", messageComplement));
        return view;
      }
      catch (Exception)
      {
        throw;
      }
    }
    private EnumActionIntegration ValidAcction(string acao)
    {
      switch (acao.ToUpper().Trim())
      {
        case "ADMISSAO":
          return EnumActionIntegration.Admission;
        case "DEMISSAO":
          return EnumActionIntegration.Demission;
        case "ATUALIZACAO":
          return EnumActionIntegration.Change;
        case "CARGA":
          return EnumActionIntegration.Load;
        default:
          resultV2.Mensagem.Add("Ação inválida");
          return EnumActionIntegration.Load;
      }
    }
    private EnumSex ValidSex(string sex)
    {
      switch (sex.ToLower())
      {
        case "f":
        case "feminino":
          return EnumSex.Female;
        case "m":
        case "masculino":
          return EnumSex.Male;
        default:
          return EnumSex.Others;
      };
    }
    private EnumStatusUser ValidSituation(string situacao)
    {
      // Verificar situação
      switch (situacao.Trim().ToUpper())
      {
        case "ATIVO":
          return EnumStatusUser.Enabled;
        case "FERIAS":
        case "FÉRIAS":
          return EnumStatusUser.Vacation;
        case "AFASTADO":
          return EnumStatusUser.Away;
        case "DEMITIDO":
          return EnumStatusUser.Disabled;
        default:
          resultV2.Mensagem.Add("Situação invalida.");
          return EnumStatusUser.Disabled;
      }
    }
    private void ValidEmptyString(string field, string message)
    {
      if (string.IsNullOrEmpty(field))
        resultV2.Mensagem.Add(message);
    }
    private string EmptyStringDefault(string field, string defaultValue)
    {
      if (string.IsNullOrEmpty(field))
        return defaultValue;
      else
        return field;
    }
    private void ValidEmptyDate(DateTime date, string message)
    {
      if (date == null || date == DateTime.MinValue)
        resultV2.Mensagem.Add(message);
    }
    private DateTime EmptyDateDefault(DateTime? field, DateTime defaultValue)
    {
      if (field == null || field == DateTime.MinValue)
        return defaultValue;
      else
        return (DateTime)field;
    }
    #endregion

    #region FormatField
    private string CapitalizeName(string param)
    {
      try
      {
        string result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(param.Trim().ToLower());
        result = result.Replace(" Da ", " da ").Replace(" De ", " de ").Replace(" Do ", " do ").Replace(" Dos ", " dos ");
        return result;
      }
      catch (Exception)
      {

        throw;
      }
    }
    private string CapitalizeOccupation(string param)
    {
      try
      {
        string result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(param.Trim().ToLower());
        result = result.Replace(" Da ", " da ").Replace(" De ", " de ").Replace(" Do ", " do ").Replace(" Dos ", " dos ")
          .Replace(" Iii", " III").Replace(" Ii", " II").Replace(" Em ", " em ").Replace(" A ", " a ").Replace(" À ", " à ")
          .Replace(" Ao ", " ao ").Replace(" E ", " e ").Replace(" Com ", " com ").Replace(" Por ", " por ");
        string wordRight = result.Substring(result.Length - 3, 3);
        if (wordRight.Equals(" Ti"))
        {
          result = string.Concat(result[0..^2], "TI");
        }
        if (wordRight.Equals(" Jr"))
        {
          result = string.Concat(result[0..^2], "JR");
        }
        if (wordRight.Equals(" Pl"))
        {
          result = string.Concat(result[0..^2], "PL");
        }
        if (wordRight.Equals(" Sr"))
        {
          result = string.Concat(result[0..^2], "SR");
        }
        if (wordRight.Equals(" Ii"))
        {
          result = string.Concat(result[0..^2], "II");
        }
        if (result.Length > 3)
        {
          wordRight = result.Substring(result.Length - 4, 4);
          if (wordRight.Equals(" Iii"))
          {
            result = string.Concat(result[0..^3], "III");
          }
        }
        return result;
      }
      catch (Exception)
      {

        throw;
      }
    }
    #endregion

    #region Conclusion PayrollEmployee
    private void InsertHistory(PayrollEmployee payrollEmployee, bool changeOccupation)
    {
      try
      {
        // Atualização de usuário e devolve o id do usuário e mensagens
        payrollEmployee._idUser = string.Empty;
        payrollEmployee._idSchooling = string.Empty;
        payrollEmployee = UserUpdate(payrollEmployee);
        if ( payrollEmployee.Messages.Count ==0 )
        {
          // Apenas continuar se não tiver mensagens de aviso
          resultV2.IdUser = payrollEmployee._idUser;
          // Atualização de contratos do usuário
          payrollEmployee._idContract = string.Empty;
          payrollEmployee._idCompany = string.Empty;
          payrollEmployee._idEstablishment = string.Empty;
          payrollEmployee._idOccupation = string.Empty;
          payrollEmployee._idIntegrationOccupation = string.Empty;
          payrollEmployee = PersonUpdate(payrollEmployee, changeOccupation);
          if (payrollEmployee.Messages.Count == 0)
          {
            payrollEmployee.StatusIntegration = EnumStatusIntegration.Atualized;
            resultV2.IdContract = payrollEmployee._idContract;
          }
        }
        // Incluir novo histórico
        payrollEmployee = payrollEmployeeService.InsertNewVersion(payrollEmployee).Result;
        resultV2.IdPayrollEmployee = payrollEmployee._id;

        for (int i = 0; i < payrollEmployee.Messages.Count; i++)
        {
          resultV2.Mensagem.Add(payrollEmployee.Messages[i]);
        }
        if (resultV2.Mensagem.Count == 0)
        {
          resultV2.Situacao = "Ok";
          if (payrollEmployee.StatusIntegration == EnumStatusIntegration.Atualized)
          {
            resultV2.Mensagem.Add("Colaborador atualizado");
          }
          else
          {
            resultV2.Mensagem.Add("Apenas histórico incluído");
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    private void ConclusionV2(PayrollEmployee payrollEmployee, PayrollEmployee payrollEmployeePrevious, bool equals)
    {
      try
      {
        if (!equals)
        {
          if (payrollEmployeePrevious != null)
          {
            if (payrollEmployeePrevious.StatusIntegration == EnumStatusIntegration.Saved)
            {
              payrollEmployeePrevious.StatusIntegration = EnumStatusIntegration.Reject;
              payrollEmployee._idPrevious = payrollEmployeePrevious._idPrevious;
            }
            else
            {
              payrollEmployee._idPrevious = payrollEmployeePrevious._id;
            }
          }
        }
        else
        {
          payrollEmployee = payrollEmployeePrevious;
          payrollEmployee.Messages = new List<string>();
          if (payrollEmployeePrevious.StatusIntegration == EnumStatusIntegration.Atualized)
          {
            resultV2.IdUser = payrollEmployee._idUser;
            resultV2.IdContract = payrollEmployee._idContract;
            resultV2.IdPayrollEmployee = payrollEmployee._id;
            resultV2.Mensagem.Add("Colaborador sem alterações");
            return;
          }
        }
        // Atualização de usuário e devolve o id do usuário e mensagens
        payrollEmployee = UserUpdate(payrollEmployee);
        resultV2.IdUser = payrollEmployee._idUser;
        // Atualização de contratos do usuário
        payrollEmployee = PersonUpdate(payrollEmployee, payrollEmployeePrevious?.OccupationName == payrollEmployee.OccupationName);
        resultV2.IdContract = payrollEmployee._idContract;
        // Atualizar registro anterior
        if (!equals)
        {
          Task taskPrevious = payrollEmployeeService.Update(payrollEmployeePrevious, null);
          // Gravar novo registro
          payrollEmployee = payrollEmployeeService.InsertNewVersion(payrollEmployee).Result;
        }
        else
        {
          Task task = payrollEmployeeService.Update(payrollEmployee, null);
        }
        resultV2.IdPayrollEmployee = payrollEmployee._id;
        for (int i = 0; i < payrollEmployee.Messages.Count; i++)
        {
          resultV2.Mensagem.Add(payrollEmployee.Messages[i]);
        }
        if (resultV2.Mensagem.Count == 0)
        {
          resultV2.Situacao = "Ok";
          resultV2.Mensagem.Add("Colaborador atualizado");
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    #endregion

    #endregion

    #region Atualização de Integração
    // Validar PayrollEmployee
    private void ValidPayrollEmployee(IntegrationCompany item)
    {
      List<PayrollEmployee> payrollEmployees = payrollEmployeeService.GetAllNewVersion(p => p.StatusIntegration == EnumStatusIntegration.Saved && p._idCompany == null && p.CompanyName == item.Name).Result;
      PayrollEmployee payrollEmployee = null;
      foreach (var payrollEmployeeItem in payrollEmployees)
      {
        payrollEmployeeItem.Messages = new List<string>();
        // Atualização de usuário e devolve o id do usuário e mensagens
        payrollEmployee = UserUpdate(payrollEmployeeItem);
        // Atualização da pessoa e devolve o id da pessoa e mensagens
        if (payrollEmployee._idPrevious == null)
          payrollEmployee = PersonUpdate(payrollEmployee, false);
        else
        {
          if (payrollEmployee.OccupationName == payrollEmployeeService.GetNewVersion(p => p._id == payrollEmployee._idPrevious).Result.OccupationName)
            payrollEmployee = PersonUpdate(payrollEmployee, true);
          else
            payrollEmployee = PersonUpdate(payrollEmployee, false);
        }
        Task task = payrollEmployeeService.Update(payrollEmployee, null);
      }
    }
    private void ValidPayrollEmployee(IntegrationEstablishment item)
    {
      List<PayrollEmployee> payrollEmployees = payrollEmployeeService.GetAllNewVersion(p => p.StatusIntegration == EnumStatusIntegration.Saved && p._idEstablishment == null && p.EstablishmentName == item.Name).Result;
      PayrollEmployee payrollEmployee = null;
      foreach (var payrollEmployeeItem in payrollEmployees)
      {
        payrollEmployeeItem.Messages = new List<string>();
        // Atualização de usuário e devolve o id do usuário e mensagens
        payrollEmployee = UserUpdate(payrollEmployeeItem);
        // Atualização da pessoa e devolve o id da pessoa e mensagens
        if (payrollEmployee._idPrevious == null)
          payrollEmployee = PersonUpdate(payrollEmployee, false);
        else
        {
          if (payrollEmployee.OccupationName == payrollEmployeeService.GetNewVersion(p => p._id == payrollEmployee._idPrevious).Result.OccupationName)
            payrollEmployee = PersonUpdate(payrollEmployee, true);
          else
            payrollEmployee = PersonUpdate(payrollEmployee, false);
        }
        Task task = payrollEmployeeService.Update(payrollEmployee, null);
      }
    }
    private void ValidPayrollEmployee(IntegrationSchooling item)
    {
      List<PayrollEmployee> payrollEmployees = payrollEmployeeService.GetAllNewVersion(p => p.StatusIntegration == EnumStatusIntegration.Saved && p._idSchooling == null && p.SchoolingName == item.Name).Result;
      PayrollEmployee payrollEmployee = null;
      foreach (var payrollEmployeeItem in payrollEmployees)
      {
        payrollEmployeeItem.Messages = new List<string>();
        // Atualização de usuário e devolve o id do usuário e mensagens
        payrollEmployee = UserUpdate(payrollEmployeeItem);
        // Atualização da pessoa e devolve o id da pessoa e mensagens
        if (payrollEmployee._idPrevious == null)
          payrollEmployee = PersonUpdate(payrollEmployee, false);
        else
        {
          if (payrollEmployee.OccupationName == payrollEmployeeService.GetNewVersion(p => p._id == payrollEmployee._idPrevious).Result.OccupationName)
            payrollEmployee = PersonUpdate(payrollEmployee, true);
          else
            payrollEmployee = PersonUpdate(payrollEmployee, false);
        }
        Task task = payrollEmployeeService.Update(payrollEmployee, null);
      }
    }
    private void ValidPayrollEmployee(IntegrationOccupation item)
    {
      List<PayrollEmployee> payrollEmployees = payrollEmployeeService.GetAllNewVersion(p => p.StatusIntegration == EnumStatusIntegration.Saved && p._idIntegrationOccupation == item._id).Result;
      PayrollEmployee payrollEmployee = null;
      foreach (var payrollEmployeeItem in payrollEmployees)
      {
        payrollEmployeeItem.Messages = new List<string>();
        // Atualização de usuário e devolve o id do usuário e mensagens
        payrollEmployee = UserUpdate(payrollEmployeeItem);
        // Atualização da pessoa e devolve o id da pessoa e mensagens
        if (payrollEmployee._idPrevious == null)
          payrollEmployee = PersonUpdate(payrollEmployee, false);
        else
        {
          if (payrollEmployee.OccupationName == payrollEmployeeService.GetNewVersion(p => p._id == payrollEmployee._idPrevious).Result.OccupationName)
            payrollEmployee = PersonUpdate(payrollEmployee, true);
          else
            payrollEmployee = PersonUpdate(payrollEmployee, false);
        }
        Task task = payrollEmployeeService.Update(payrollEmployee, null);
      }
    }
    public ColaboradorV2Retorno IntegrationPayroll(string id)
    {
      try
      {
        resultV2 = new ColaboradorV2Retorno
        {
          Situacao = "Ok",
          Mensagem = new List<string>()
        };
        PayrollEmployee payrollEmployee = payrollEmployeeService.GetNewVersion(p => p._id == id).Result;
        if (payrollEmployee.StatusIntegration != EnumStatusIntegration.Saved)
          throw new Exception("Situação da integração inválida");
        PayrollEmployee payrollEmployeePrevious = payrollEmployeeService.GetNewVersion(p => p._id == payrollEmployee._idPrevious).Result;
        // TODO: falta testar aqui
        ConclusionV2(payrollEmployee, payrollEmployeePrevious, false);
        return resultV2;

      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

  }
}
