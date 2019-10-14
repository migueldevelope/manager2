using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Business.Integration;
using Manager.Core.Interfaces;
using Manager.Data;
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
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<User> userService;
    private readonly ServiceGeneric<Schooling> schoolingService;
    private readonly ServiceGeneric<Company> companyService;
    private readonly ServiceGeneric<Establishment> establishmentService;
    private readonly ServiceGeneric<Account> accountService;
    private readonly ServiceGeneric<Occupation> occupationService;
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
    private readonly ServiceGeneric<Group> groupService;

    // Colaborador V2
    private readonly ServiceGeneric<PayrollEmployee> payrollEmployeeService;
    private ColaboradorV2Retorno resultV2;

    #region Constructor
    public ServiceIntegration(DataContext context, DataContext contextLog, DataContext contextIntegration) : base(context)
    {
      try
      {
        personService = new ServiceGeneric<Person>(context);
        userService = new ServiceGeneric<User>(context);
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
      }
      catch (Exception)
      {
        throw;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      personService._user = _user;
      userService._user = _user;
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
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      personService._user = user;
      schoolingService._user = user;
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
    }
    #endregion

    #region Commun Area
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
          var i = integrationCompanyService.InsertNewVersion(item);
        }
        if (item.IdCompany.Equals("000000000000000000000000"))
        {
          item.Name = name;
          List<Company> companies = companyService.GetAllNewVersion(p => p.Name.ToLower() == name.ToLower()).Result.ToList<Company>();
          if (companies.Count == 1)
          {
            item.IdCompany = companies[0]._id;
            item.NameCompany = companies[0].Name;
            var i = integrationCompanyService.Update(item, null);
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
          var i = integrationEstablishmentService.InsertNewVersion(item);
        }
        if (item.IdEstablishment.Equals("000000000000000000000000"))
        {
          item.Name = name;
          item._idCompany = idcompany;
          List<Establishment> establishments = establishmentService.GetAllNewVersion(p => p.Company._id == idcompany && p.Name.ToLower() == name.ToLower()).Result.ToList<Establishment>();
          if (establishments.Count == 1)
          {
            item.IdEstablishment = establishments[0]._id;
            item.NameEstablishment = establishments[0].Name;
            var i = integrationEstablishmentService.Update(item, null);
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
          var i = integrationOccupationService.InsertNewVersion(item);
        }
        if (item.IdOccupation.Equals("000000000000000000000000"))
        {
          item.Name = name;
          item._idCompany = idcompany;
          List<Occupation> occupations = occupationService.GetAllNewVersion(p => p.Group.Company._id == idcompany && p.Name.ToLower() == name.ToLower()).Result.ToList<Occupation>();
          if (occupations.Count == 1)
          {
            item.IdOccupation = occupations[0]._id;
            item.NameOccupation = occupations[0].Name;
            var i = integrationOccupationService.Update(item, null);
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
            NameOccupation = item.NameOccupation
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
          var i = integrationSchoolingService.InsertNewVersion(item).Result;
        }
        if (item.IdSchooling.Equals("000000000000000000000000"))
        {
          item.Name = name;
          List<Schooling> schoolings = schoolingService.GetAllNewVersion(p => p.Name.ToLower() == name.ToLower()).Result.ToList<Schooling>();
          if (schoolings.Count == 1)
          {
            item.IdSchooling = schoolings[0]._id;
            item.NameSchooling = schoolings[0].Name;
            var i = integrationSchoolingService.Update(item, null);
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
            Mode = EnumIntegrationMode.DataBaseV1,
            Type = EnumIntegrationType.Complete,
            Status = EnumStatus.Enabled
          }).Result;
        }
        return new ViewCrudIntegrationParameter()
        {
          ConnectionString = param.ConnectionString,
          CriticalError = param.CriticalError,
          CustomVersionExecution = param.CustomVersionExecution,
          FilePathLocal = param.FilePathLocal,
          SheetName = param.SheetName,
          LastExecution = param.LastExecution,
          LinkPackCustom = param.LinkPackCustom,
          LinkPackProgram = param.LinkPackProgram,
          MachineIdentity = param.MachineIdentity,
          MessageAtualization = param.MessageAtualization,
          Process = param.Process,
          Mode = param.Mode,
          ProgramVersionExecution = param.ProgramVersionExecution,
          SqlCommand = param.SqlCommand,
          StatusExecution = param.StatusExecution,
          Type = param.Type,
          UploadNextLog = param.UploadNextLog,
          VersionPackCustom = param.VersionPackCustom,
          VersionPackProgram = param.VersionPackProgram,
          ApiIdentification = param.ApiIdentification,
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
        param.Process = view.Process;
        param.Type = view.Type;
        param.ConnectionString = view.ConnectionString;
        param.SqlCommand = view.SqlCommand;
        param.FilePathLocal = view.FilePathLocal;
        param.SheetName = view.SheetName;
        param.VersionPackProgram = view.VersionPackProgram;
        param.LinkPackProgram = view.LinkPackProgram;
        param.VersionPackCustom = view.VersionPackCustom;
        param.LinkPackCustom = view.LinkPackCustom;
        param.MessageAtualization = view.MessageAtualization;
        param.StatusExecution = view.StatusExecution;
        param.ProgramVersionExecution = view.ProgramVersionExecution;
        param.CustomVersionExecution = view.CustomVersionExecution;
        param.CriticalError = view.CriticalError;
        param.MachineIdentity = view.MachineIdentity;
        param.UploadNextLog = view.UploadNextLog;
        param.LinkLogExecution = view.LinkLogExecution;
        param.ApiIdentification = view.ApiIdentification;
        parameterService.Update(param, null).Wait();
        return new ViewCrudIntegrationParameter()
        {
          ConnectionString = param.ConnectionString,
          CriticalError = param.CriticalError,
          CustomVersionExecution = param.CustomVersionExecution,
          FilePathLocal = param.FilePathLocal,
          SheetName = param.SheetName,
          LastExecution = param.LastExecution,
          LinkPackCustom = param.LinkPackCustom,
          LinkPackProgram = param.LinkPackProgram,
          MachineIdentity = param.MachineIdentity,
          MessageAtualization = param.MessageAtualization,
          Process = param.Process,
          Mode = param.Mode,
          ProgramVersionExecution = param.ProgramVersionExecution,
          SqlCommand = param.SqlCommand,
          StatusExecution = param.StatusExecution,
          Type = param.Type,
          UploadNextLog = param.UploadNextLog,
          VersionPackCustom = param.VersionPackCustom,
          VersionPackProgram = param.VersionPackProgram,
          ApiIdentification = param.ApiIdentification,
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
        Schooling result = schoolingService.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
        return result != null
          ? new ViewListSchooling()
          {
            Name = result.Name,
            Order = result.Order,
            _id = result._id
          }
          : null;
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
        var result = companyService.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
        return result != null
          ? new ViewListCompany()
          {
            Name = result.Name,
            _id = result._id
          }
          : null;
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
        var result = establishmentService.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
        return result != null
          ? new ViewListEstablishment()
          {
            Name = result.Name,
            _id = result._id
          }
          : null;
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
        IQueryable<Person> personsDocument = personService.GetAllNewVersion(p => p.User.Document == document).Result.AsQueryable();
        if (personsDocument.Count() == 0)
          return null;
        Person person = personsDocument.Where(p => p.Company._id == idcompany && p.Establishment._id == idestablishment && p.Registration == registration).FirstOrDefault();
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

    #region Occupation
    public ViewIntegrationProfileOccupation IntegrationProfile(ViewIntegrationProfileOccupation view)
    {
      Occupation occupation = occupationService.GetNewVersion(p => p.Name == view.Name).Result;
      Group group = groupService.GetNewVersion(p => p.Name == view.NameGroup).Result;

      if (occupation == null)
      {
        occupation = new Occupation()
        {
          Group = group.GetViewList(),
          Cbo = null,
          Name = Capitalization(view.Name),
          SpecificRequirements = view.SpecificRequirements,
          SalaryScales = null,
          Process = new List<ViewListProcessLevelTwo>(),
          Skills = new List<ViewListSkill>(),
          Activities = new List<ViewListActivitie>(),
          Schooling = new List<ViewCrudSchooling>(),
          Line = 0
        };
        occupation.Process.Add(processLevelTwoService.GetNewVersion(p => p._id == view.IdProcessLevelTwo).Result.GetViewList());
        Skill skill;
        foreach (string item in view.Skills)
        {
          skill = skillService.GetNewVersion(p => p.Name == item).Result;
          occupation.Skills.Add(skill.GetViewList());
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
        occupation.Schooling = group.Schooling;
        for (int i = 0; i < view.Schooling.Count; i++)
        {
          for (int lin = 0; lin < occupation.Schooling.Count; lin++)
          {
            if (occupation.Schooling[lin].Name.ToUpper().Equals(view.Schooling[i].ToUpper()))
            {
              occupation.Schooling[lin].Complement = view.SchoolingComplement[i];
              break;
            }
          }
        }
        occupation = occupationService.InsertNewVersion(occupation).Result;
      } else {
        occupation.Skills = new List<ViewListSkill>();
        occupation.SpecificRequirements = view.SpecificRequirements;
        occupation.Activities = new List<ViewListActivitie>();
        Skill skill;
        foreach (string item in view.Skills)
        {
          skill = skillService.GetNewVersion(p => p.Name == item).Result;
          occupation.Skills.Add(skill.GetViewList());
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
        for (int i = 0; i < view.Schooling.Count; i++)
        {
          for (int lin = 0; lin < occupation.Schooling.Count; lin++)
          {
            if (occupation.Schooling[lin].Name.ToUpper().Equals(view.Schooling[i].ToUpper()))
            {
              occupation.Schooling[lin].Complement = view.SchoolingComplement[i];
              break;
            }
          }
        }
        Task retorno = occupationService.Update(occupation, null);
      }
      return new ViewIntegrationProfileOccupation()
      {
        _id = occupation._id,
        IdCompany = occupation.Group.Company._id,
        IdProcessLevelTwo = occupation.Process[0]._id,
        Name = occupation.Name,
        NameGroup = occupation.Group.Name,
        Activities = occupation.Activities.OrderBy(o => o.Order).Select(x => x.Name).ToList(),
        Skills = occupation.Skills.OrderBy(o => o.Name).Select(x => x.Name).ToList(),
        SpecificRequirements = occupation.SpecificRequirements,
        Schooling = occupation.Schooling.OrderBy(o => o.Order).Select(x => x.Name).ToList(),
        SchoolingComplement = occupation.Schooling.OrderBy(o => o.Order).Select(x => x.Complement).ToList()
      };
    }
    private string Capitalization(string nome)
    {
      try
      {
        TextInfo myTI = new CultureInfo("pt-BR", false).TextInfo;
        nome = myTI.ToTitleCase(nome.Trim().ToLower()).Replace(" Por ", " por ").Replace(" Com ", " com ").Replace(" E ", " e ").Replace(" De ", " de ").Replace(" Da ", " da ").Replace(" Dos ", " dos ").Replace(" Do ", " do ");
        switch (nome.Substring(nome.Length - 2, 2))
        {
          case "Jr":
          case "Pl":
          case "Sr":
          case "II":
            nome = string.Concat(nome.Substring(0, nome.Length - 1), nome.Substring(nome.Length - 1).ToUpper());
            break;
          default:
            break;
        }
        nome = nome.Trim().Replace(" Jr", " JR").Replace(" Pl", " PL").Replace(" Sr", " SR");
        return nome;
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #endregion

    #region Colaborador V2
    public ColaboradorV2Retorno IntegrationV2(ColaboradorV2Completo view)
    {
      EnumSex sexo = EnumSex.Others;
      EnumStatusUser situacao = EnumStatusUser.Enabled;
      EnumActionIntegration acao = EnumActionIntegration.Change;
      try
      {
        resultV2 = new ColaboradorV2Retorno
        {
          Situacao = EnumSituacaoRetornoIntegracao.Erro,
          Mensagem = new List<string>()
        };
        // Verificação da ação
        switch (view.Acao)
        {
          case "ADMISSAO":
            acao = EnumActionIntegration.Admission;
            break;
          case "DEMISSAO":
            acao = EnumActionIntegration.Resignation;
            break;
          case "ATUALIZACAO":
            break;
          case "CARGA":
            acao = EnumActionIntegration.Load;
            break;
          default:
            resultV2.Mensagem.Add("Ação inválida");
            break;
        }
        // Validação de Campos chave
        if (string.IsNullOrEmpty(view.Estabelecimento))
          view.Estabelecimento = "1";
        if (string.IsNullOrEmpty(view.NomeEstabelecimento))
          view.NomeEstabelecimento = "Estabelecimento Padrão";
        ValidKeyEmployee(view);
        // Validação de outros campos
        if (string.IsNullOrEmpty(view.Nome))
          resultV2.Mensagem.Add("Nome não informado");
        if (string.IsNullOrEmpty(view.Email))
          view.Email = string.Format("{0}@{1}", view.Matricula.ToLower(),"mail.com.br");
        switch (view.Sexo)
        {
          case "F":
            sexo = EnumSex.Female;
            break;
          case "feminino":
            sexo = EnumSex.Female;
            view.Sexo = "F";
            break;
          case "M":
            sexo = EnumSex.Male;
            break;
          case "masculino":
            sexo = EnumSex.Male;
            view.Sexo = "M";
            break;
          default:
            sexo = EnumSex.Others;
            view.Sexo = "O";
            break;
        };
        if (string.IsNullOrEmpty(view.GrauInstrucao))
          view.GrauInstrucao = "0";
        if (string.IsNullOrEmpty(view.NomeGrauInstrucao))
          view.NomeGrauInstrucao = "Não definida";
        if (view.DataAdmissao == null)
            resultV2.Mensagem.Add("Data de admissão deve ser informada");
        if (view.DataAdmissao == DateTime.MinValue)
            resultV2.Mensagem.Add("Data de admissão deve ser informada");
        if (string.IsNullOrEmpty(view.Cargo))
          resultV2.Mensagem.Add("Cargo deve ser informado.");
        if (string.IsNullOrEmpty(view.NomeCargo))
          resultV2.Mensagem.Add("Nome do cargo deve ser informado.");
        // Verificar situação
        switch (view.Situacao)
        {
          case "ATIVO":
            situacao = EnumStatusUser.Enabled;
            break;
          case "FÉRIAS":
            situacao = EnumStatusUser.Vacation;
            break;
          case "AFASTADO":
            situacao = EnumStatusUser.Away;
            break;
          case "DEMITIDO":
            situacao = EnumStatusUser.Disabled;
            break;
          default:
            resultV2.Mensagem.Add("Situação invalida.");
            break;
        }
        // Campos de gestor
        if (!string.IsNullOrEmpty(view.MatriculaGestor))
        {
          if (string.IsNullOrEmpty(view.CpfGestor))
            resultV2.Mensagem.Add("Cpf do gestor deve ser informado.");
          else
            if (!IsValidCPF(view.CpfGestor))
              resultV2.Mensagem.Add("Cpf do gestor inválido.");
          if (string.IsNullOrEmpty(view.EmpresaGestor))
            resultV2.Mensagem.Add("Empresa do gestor deve ser informada.");
          if (string.IsNullOrEmpty(view.NomeEmpresaGestor))
            resultV2.Mensagem.Add("Nome da empresa do gestor deve ser informado.");
          if (string.IsNullOrEmpty(view.EstabelecimentoGestor))
            view.EstabelecimentoGestor = "1";
          if (string.IsNullOrEmpty(view.NomeEstabelecimentoGestor))
            view.NomeEstabelecimentoGestor = "Estabelecimento Padrão";
        }
        if (resultV2.Mensagem.Count == 0)
        {
          PayrollEmployee payrollEmployee = payrollEmployeeService.GetAllNewVersion(p => p.Key == view.ChaveColaborador).Result.LastOrDefault();
          if (payrollEmployee != null && (acao == EnumActionIntegration.Admission || acao == EnumActionIntegration.Load))
            resultV2.Mensagem.Add("Colaborador já está na base de integração.");
          if (payrollEmployee == null && (acao == EnumActionIntegration.Resignation || acao == EnumActionIntegration.Change))
            resultV2.Mensagem.Add("Colaborador não está na base de integração.");
        }
        // Gravar tabela
        if (resultV2.Mensagem.Count == 0)
        {
          PayrollEmployee payrollEmployee = new PayrollEmployee
          {
            // Identificação
            Key = view.ChaveColaborador,
            DateRegister = DateTime.Now,
            Action = acao,
            StatusIntegration = EnumStatusIntegration.Saved,
            // Campos
            AdmissionDate = view.DataAdmissao,
            BirthDate = view.DataNascimento,
            CellNumber = view.Celular,
            Company = view.Empresa,
            CompanyName = view.NomeEmpresa,
            CostCenter = view.CentroCusto,
            CostCenterChangeDate = view.DataTrocaCentroCusto,
            CostCenterName = view.NomeCentroCusto,
            Document = view.Cpf,
            Establishment = view.Estabelecimento,
            EstablishmentName = view.NomeEstabelecimento,
            HolidayReturn = view.DataRetornoFerias,
            MotiveAside = view.MotivoAfastamento,
            Mail = view.Email,
            ManagerCompany = view.EmpresaGestor,
            ManagerCompanyName = view.NomeEmpresaGestor,
            ManagerDocument = view.CpfGestor,
            ManagerEstablishment = view.EstabelecimentoGestor,
            ManagerEstablishmentName = view.NomeEstabelecimentoGestor,
            MatriculaGestor = view.MatriculaGestor,
            Name = view.Nome,
            Nickname = view.Apelido,
            Occupation = view.Cargo,
            OccupationChangeDate = view.DataTrocaCargo,
            OccupationName = view.NomeCargo,
            Registration = view.Matricula,
            ResignationDate = view.DataDemissao,
            Salary = view.SalarioNominal,
            SalaryChangeDate = view.DataUltimoReajuste,
            SalaryChangeReason = view.MotivoUltimoReajuste,
            Schooling = view.GrauInstrucao,
            SchoolingName = view.NomeGrauInstrucao,
            Sex = sexo,
            Situacao = situacao,
            Workload = view.CargaHoraria
          };
          payrollEmployee = payrollEmployeeService.InsertNewVersion(payrollEmployee).Result;
          if (payrollEmployee != null)
          {
            // Resultado Ok
            resultV2.IdPayrollEmployee = payrollEmployee._id;
            resultV2.IdUser = string.Empty;
            resultV2.IdContract = string.Empty;
            resultV2.Situacao = EnumSituacaoRetornoIntegracao.Ok;
          }
        }
        return resultV2;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ColaboradorV2Retorno IntegrationV2(ColaboradorV2Admissao view)
    {
      EnumSex sexo = EnumSex.Others;
      try
      {
        resultV2 = new ColaboradorV2Retorno
        {
          Situacao = EnumSituacaoRetornoIntegracao.Erro,
          Mensagem = new List<string>()
        };
        // Validação de Campos chave
        if (string.IsNullOrEmpty(view.Estabelecimento))
          view.Estabelecimento = "1";
        if (string.IsNullOrEmpty(view.NomeEstabelecimento))
          view.NomeEstabelecimento = "Estabelecimento Padrão";
        ValidKeyEmployee(view);
        // Validação de outros campos
        if (string.IsNullOrEmpty(view.Nome))
          resultV2.Mensagem.Add("Nome não informado");
        if (string.IsNullOrEmpty(view.Email))
          view.Email = string.Format("{0}@{1}", view.Matricula.ToLower(), "mail.com.br");
        switch (view.Sexo)
        {
          case "F":
            sexo = EnumSex.Female;
            break;
          case "feminino":
            sexo = EnumSex.Female;
            view.Sexo = "F";
            break;
          case "M":
            sexo = EnumSex.Male;
            break;
          case "masculino":
            sexo = EnumSex.Male;
            view.Sexo = "M";
            break;
          default:
            sexo = EnumSex.Others;
            view.Sexo = "O";
            break;
        };
        if (string.IsNullOrEmpty(view.GrauInstrucao))
          view.GrauInstrucao = "0";
        if (string.IsNullOrEmpty(view.NomeGrauInstrucao))
          view.NomeGrauInstrucao = "Não definida";
        if (view.DataAdmissao == null)
          resultV2.Mensagem.Add("Data de admissão deve ser informada");
        if (view.DataAdmissao == DateTime.MinValue)
          resultV2.Mensagem.Add("Data de admissão deve ser informada");
        if (string.IsNullOrEmpty(view.Cargo))
          resultV2.Mensagem.Add("Cargo deve ser informado.");
        if (string.IsNullOrEmpty(view.NomeCargo))
          resultV2.Mensagem.Add("Nome do cargo deve ser informado.");
        // Campos de gestor
        if (!string.IsNullOrEmpty(view.MatriculaGestor))
        {
          if (string.IsNullOrEmpty(view.CpfGestor))
            resultV2.Mensagem.Add("Cpf do gestor deve ser informado.");
          else
            if (!IsValidCPF(view.CpfGestor))
            resultV2.Mensagem.Add("Cpf do gestor inválido.");
          if (string.IsNullOrEmpty(view.EmpresaGestor))
            resultV2.Mensagem.Add("Empresa do gestor deve ser informada.");
          if (string.IsNullOrEmpty(view.NomeEmpresaGestor))
            resultV2.Mensagem.Add("Nome da empresa do gestor deve ser informado.");
          if (string.IsNullOrEmpty(view.EstabelecimentoGestor))
            view.EstabelecimentoGestor = "1";
          if (string.IsNullOrEmpty(view.NomeEstabelecimentoGestor))
            view.NomeEstabelecimentoGestor = "Estabelecimento Padrão";
        }
        if (resultV2.Mensagem.Count == 0)
        {
          PayrollEmployee payrollEmployee = payrollEmployeeService.GetAllNewVersion(p => p.Key == view.ChaveColaborador).Result.LastOrDefault();
          if (payrollEmployee != null)
            resultV2.Mensagem.Add("Colaborador já está na base de integração.");
        }
        // Gravar tabela
        if (resultV2.Mensagem.Count == 0)
        {
          PayrollEmployee payrollEmployee = new PayrollEmployee
          {
            // Identificação
            Key = view.ChaveColaborador,
            DateRegister = DateTime.Now,
            Action = EnumActionIntegration.Admission,
            StatusIntegration = EnumStatusIntegration.Saved,
            // Campos
            AdmissionDate = view.DataAdmissao,
            BirthDate = view.DataNascimento,
            CellNumber = view.Celular,
            Company = view.Empresa,
            CompanyName = view.NomeEmpresa,
            CostCenter = view.CentroCusto,
            CostCenterChangeDate = null,
            CostCenterName = view.NomeCentroCusto,
            Document = view.Cpf,
            Establishment = view.Estabelecimento,
            EstablishmentName = view.NomeEstabelecimento,
            Mail = view.Email,
            ManagerCompany = view.EmpresaGestor,
            ManagerCompanyName = view.NomeEmpresaGestor,
            ManagerDocument = view.CpfGestor,
            ManagerEstablishment = view.EstabelecimentoGestor,
            ManagerEstablishmentName = view.NomeEstabelecimentoGestor,
            MatriculaGestor = view.MatriculaGestor,
            Name = view.Nome,
            Nickname = view.Apelido,
            Occupation = view.Cargo,
            OccupationChangeDate = null,
            OccupationName = view.NomeCargo,
            Registration = view.Matricula,
            ResignationDate = null,
            Salary = view.SalarioNominal,
            SalaryChangeDate = null,
            SalaryChangeReason = null,
            Schooling = view.GrauInstrucao,
            SchoolingName = view.NomeGrauInstrucao,
            Sex = sexo,
            Situacao = EnumStatusUser.Enabled,
            Workload = view.CargaHoraria
          };
          payrollEmployee = payrollEmployeeService.InsertNewVersion(payrollEmployee).Result;
          if (payrollEmployee != null)
          {
            // Resultado Ok
            resultV2.IdPayrollEmployee = payrollEmployee._id;
            resultV2.IdUser = string.Empty;
            resultV2.IdContract = string.Empty;
            resultV2.Situacao = EnumSituacaoRetornoIntegracao.Ok;
          }
        }
        return resultV2;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ColaboradorV2Retorno IntegrationV2(ColaboradorV2Cargo view)
    {
      try
      {
        resultV2 = new ColaboradorV2Retorno
        {
          Situacao = EnumSituacaoRetornoIntegracao.Erro,
          Mensagem = new List<string>()
        };
        // Validação de Campos chave
        if (string.IsNullOrEmpty(view.Estabelecimento))
          view.Estabelecimento = "1";
        if (string.IsNullOrEmpty(view.NomeEstabelecimento))
          view.NomeEstabelecimento = "Estabelecimento Padrão";
        ValidKeyEmployee(view);
        // Validação de outros campos
        if (string.IsNullOrEmpty(view.Cargo))
          resultV2.Mensagem.Add("Cargo deve ser informado.");
        if (string.IsNullOrEmpty(view.NomeCargo))
          resultV2.Mensagem.Add("Nome do cargo deve ser informado.");
        if (view.DataTrocaCargo == null)
          view.DataTrocaCargo = DateTime.Now.Date;
        // Gravar tabela
        if (resultV2.Mensagem.Count == 0)
        {
          PayrollEmployee payrollEmployee = payrollEmployeeService.GetAllNewVersion(p => p.Key == view.ChaveColaborador).Result.LastOrDefault();
          if (payrollEmployee == null)
            resultV2.Mensagem.Add("Colaborador não está na base de integração.");
          if (payrollEmployee.StatusIntegration != EnumStatusIntegration.Atualized)
          {
            payrollEmployee.StatusIntegration = EnumStatusIntegration.Reject;
            Task task = payrollEmployeeService.Update(payrollEmployee, null);
          }
          // Identificação
          payrollEmployee._id = null;
          payrollEmployee.DateRegister = DateTime.Now;
          payrollEmployee.StatusIntegration = EnumStatusIntegration.Saved;
          // Campos
          payrollEmployee.Occupation = view.Cargo;
          payrollEmployee.OccupationChangeDate = view.DataTrocaCargo;
          payrollEmployee.OccupationName = view.NomeCargo;
          payrollEmployee = payrollEmployeeService.InsertNewVersion(payrollEmployee).Result;
          if (payrollEmployee != null)
          {
            // Resultado Ok
            resultV2.IdPayrollEmployee = payrollEmployee._id;
            resultV2.IdUser = string.Empty;
            resultV2.IdContract = string.Empty;
            resultV2.Situacao = EnumSituacaoRetornoIntegracao.Ok;
          }
        }
        return resultV2;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ColaboradorV2Retorno IntegrationV2(ColaboradorV2CentroCusto view)
    {
      try
      {
        resultV2 = new ColaboradorV2Retorno
        {
          Situacao = EnumSituacaoRetornoIntegracao.Erro,
          Mensagem = new List<string>()
        };
        // Validação de Campos chave
        if (string.IsNullOrEmpty(view.Estabelecimento))
          view.Estabelecimento = "1";
        if (string.IsNullOrEmpty(view.NomeEstabelecimento))
          view.NomeEstabelecimento = "Estabelecimento Padrão";
        ValidKeyEmployee(view);
        // Validação de outros campos
        if (string.IsNullOrEmpty(view.CentroCusto))
          resultV2.Mensagem.Add("Centro de custo deve ser informado.");
        if (string.IsNullOrEmpty(view.NomeCentroCusto))
          resultV2.Mensagem.Add("Centro de custo deve ser informado.");
        if (view.DataTrocaCentroCusto == null)
          view.DataTrocaCentroCusto = DateTime.Now.Date;
        // Gravar tabela
        if (resultV2.Mensagem.Count == 0)
        {
          PayrollEmployee payrollEmployee = payrollEmployeeService.GetAllNewVersion(p => p.Key == view.ChaveColaborador).Result.LastOrDefault();
          if (payrollEmployee == null)
            resultV2.Mensagem.Add("Colaborador não está na base de integração.");
          if (payrollEmployee.StatusIntegration != EnumStatusIntegration.Atualized)
          {
            payrollEmployee.StatusIntegration = EnumStatusIntegration.Reject;
            Task task = payrollEmployeeService.Update(payrollEmployee, null);
          }
          // Identificação
          payrollEmployee._id = null;
          payrollEmployee.DateRegister = DateTime.Now;
          payrollEmployee.StatusIntegration = EnumStatusIntegration.Saved;
          // Campos
          payrollEmployee.CostCenter = view.CentroCusto;
          payrollEmployee.CostCenterChangeDate = view.DataTrocaCentroCusto;
          payrollEmployee.CostCenterName = view.NomeCentroCusto;
          payrollEmployee = payrollEmployeeService.InsertNewVersion(payrollEmployee).Result;
          if (payrollEmployee != null)
          {
            // Resultado Ok
            resultV2.IdPayrollEmployee = payrollEmployee._id;
            resultV2.IdUser = string.Empty;
            resultV2.IdContract = string.Empty;
            resultV2.Situacao = EnumSituacaoRetornoIntegracao.Ok;
          }
        }
        return resultV2;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ColaboradorV2Retorno IntegrationV2(ColaboradorV2Gestor view)
    {
      try
      {
        resultV2 = new ColaboradorV2Retorno
        {
          Situacao = EnumSituacaoRetornoIntegracao.Erro,
          Mensagem = new List<string>()
        };
        // Validação de Campos chave
        if (string.IsNullOrEmpty(view.Estabelecimento))
          view.Estabelecimento = "1";
        if (string.IsNullOrEmpty(view.NomeEstabelecimento))
          view.NomeEstabelecimento = "Estabelecimento Padrão";
        ValidKeyEmployee(view);
        // Campos de gestor
        if (!string.IsNullOrEmpty(view.MatriculaGestor))
        {
          if (string.IsNullOrEmpty(view.CpfGestor))
            resultV2.Mensagem.Add("Cpf do gestor deve ser informado.");
          else
            if (!IsValidCPF(view.CpfGestor))
            resultV2.Mensagem.Add("Cpf do gestor inválido.");
          if (string.IsNullOrEmpty(view.EmpresaGestor))
            resultV2.Mensagem.Add("Empresa do gestor deve ser informada.");
          if (string.IsNullOrEmpty(view.NomeEmpresaGestor))
            resultV2.Mensagem.Add("Nome da empresa do gestor deve ser informado.");
          if (string.IsNullOrEmpty(view.EstabelecimentoGestor))
            view.EstabelecimentoGestor = "1";
          if (string.IsNullOrEmpty(view.NomeEstabelecimentoGestor))
            view.NomeEstabelecimentoGestor = "Estabelecimento Padrão";
        }
        // Gravar tabela
        if (resultV2.Mensagem.Count == 0)
        {
          PayrollEmployee payrollEmployee = payrollEmployeeService.GetAllNewVersion(p => p.Key == view.ChaveColaborador).Result.LastOrDefault();
          if (payrollEmployee == null)
            resultV2.Mensagem.Add("Colaborador não está na base de integração.");
          if (payrollEmployee.StatusIntegration != EnumStatusIntegration.Atualized)
          {
            payrollEmployee.StatusIntegration = EnumStatusIntegration.Reject;
            Task task = payrollEmployeeService.Update(payrollEmployee, null);
          }
          // Identificação
          payrollEmployee._id = null;
          payrollEmployee.DateRegister = DateTime.Now;
          payrollEmployee.StatusIntegration = EnumStatusIntegration.Saved;
          // Campos
          payrollEmployee.ManagerCompany = view.EmpresaGestor;
          payrollEmployee.ManagerCompanyName = view.NomeEmpresaGestor;
          payrollEmployee.ManagerDocument = view.CpfGestor;
          payrollEmployee.ManagerEstablishment = view.EstabelecimentoGestor;
          payrollEmployee.ManagerEstablishmentName = view.NomeEstabelecimentoGestor;
          payrollEmployee.MatriculaGestor = view.MatriculaGestor;
          payrollEmployee = payrollEmployeeService.InsertNewVersion(payrollEmployee).Result;
          if (payrollEmployee != null)
          {
            // Resultado Ok
            resultV2.IdPayrollEmployee = payrollEmployee._id;
            resultV2.IdUser = string.Empty;
            resultV2.IdContract = string.Empty;
            resultV2.Situacao = EnumSituacaoRetornoIntegracao.Ok;
          }
        }
        return resultV2;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ColaboradorV2Retorno IntegrationV2(ColaboradorV2Salario view)
    {
      try
      {
        resultV2 = new ColaboradorV2Retorno
        {
          Situacao = EnumSituacaoRetornoIntegracao.Erro,
          Mensagem = new List<string>()
        };
        // Validação de Campos chave
        if (string.IsNullOrEmpty(view.Estabelecimento))
          view.Estabelecimento = "1";
        if (string.IsNullOrEmpty(view.NomeEstabelecimento))
          view.NomeEstabelecimento = "Estabelecimento Padrão";
        ValidKeyEmployee(view);
        // Validação outros campos
        if (view.DataUltimoReajuste == null)
          view.DataUltimoReajuste = DateTime.Now.Date;
        // Gravar tabela
        if (resultV2.Mensagem.Count == 0)
        {
          PayrollEmployee payrollEmployee = payrollEmployeeService.GetAllNewVersion(p => p.Key == view.ChaveColaborador).Result.LastOrDefault();
          if (payrollEmployee == null)
            resultV2.Mensagem.Add("Colaborador não está na base de integração.");
          if (payrollEmployee.StatusIntegration != EnumStatusIntegration.Atualized)
          {
            payrollEmployee.StatusIntegration = EnumStatusIntegration.Reject;
            Task task = payrollEmployeeService.Update(payrollEmployee, null);
          }
          // Identificação
          payrollEmployee._id = null;
          payrollEmployee.DateRegister = DateTime.Now;
          payrollEmployee.StatusIntegration = EnumStatusIntegration.Saved;
          // Campos
          payrollEmployee.Salary = view.SalarioNominal;
          payrollEmployee.Workload = view.CargaHoraria;
          payrollEmployee.SalaryChangeDate = view.DataUltimoReajuste;
          payrollEmployee.SalaryChangeReason = view.MotivoUltimoReajuste;
          payrollEmployee = payrollEmployeeService.InsertNewVersion(payrollEmployee).Result;
          if (payrollEmployee != null)
          {
            // Resultado Ok
            resultV2.IdPayrollEmployee = payrollEmployee._id;
            resultV2.IdUser = string.Empty;
            resultV2.IdContract = string.Empty;
            resultV2.Situacao = EnumSituacaoRetornoIntegracao.Ok;
          }
        }
        return resultV2;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ColaboradorV2Retorno IntegrationV2(ColaboradorV2Situacao view)
    {
      EnumStatusUser situacao = EnumStatusUser.Enabled;
      try
      {
        resultV2 = new ColaboradorV2Retorno
        {
          Situacao = EnumSituacaoRetornoIntegracao.Erro,
          Mensagem = new List<string>()
        };
        // Validação de Campos chave
        if (string.IsNullOrEmpty(view.Estabelecimento))
          view.Estabelecimento = "1";
        if (string.IsNullOrEmpty(view.NomeEstabelecimento))
          view.NomeEstabelecimento = "Estabelecimento Padrão";
        ValidKeyEmployee(view);
        // Validação de outros campos
        // Verificar situação
        switch (view.Situacao)
        {
          case "ATIVO":
            situacao = EnumStatusUser.Enabled;
            break;
          case "FÉRIAS":
            if (view.DataRetornoFerias == null)
              view.DataRetornoFerias = DateTime.Now.Date.AddDays(30);
            situacao = EnumStatusUser.Vacation;
            break;
          case "AFASTADO":
            situacao = EnumStatusUser.Away;
            break;
          case "DEMITIDO":
            resultV2.Mensagem.Add("Não é permitida a demissão por este método.");
            break;
          default:
            resultV2.Mensagem.Add("Situação invalida.");
            break;
        }
        // Gravar tabela
        if (resultV2.Mensagem.Count == 0)
        {
          PayrollEmployee payrollEmployee = payrollEmployeeService.GetAllNewVersion(p => p.Key == view.ChaveColaborador).Result.LastOrDefault();
          if (payrollEmployee == null)
            resultV2.Mensagem.Add("Colaborador não está na base de integração.");
          if (payrollEmployee.StatusIntegration != EnumStatusIntegration.Atualized)
          {
            payrollEmployee.StatusIntegration = EnumStatusIntegration.Reject;
            Task task = payrollEmployeeService.Update(payrollEmployee, null);
          }
          // Identificação
          payrollEmployee._id = null;
          payrollEmployee.DateRegister = DateTime.Now;
          payrollEmployee.StatusIntegration = EnumStatusIntegration.Saved;
          // Campos
          payrollEmployee.Situacao = situacao;
          payrollEmployee.MotiveAside = string.Empty;
          payrollEmployee.HolidayReturn = null;
          if (situacao == EnumStatusUser.Vacation)
            payrollEmployee.HolidayReturn = view.DataRetornoFerias;
          if (situacao == EnumStatusUser.Away)
            payrollEmployee.MotiveAside = view.MotivoAfastamento;
          payrollEmployee = payrollEmployeeService.InsertNewVersion(payrollEmployee).Result;
          if (payrollEmployee != null)
          {
            // Resultado Ok
            resultV2.IdPayrollEmployee = payrollEmployee._id;
            resultV2.IdUser = string.Empty;
            resultV2.IdContract = string.Empty;
            resultV2.Situacao = EnumSituacaoRetornoIntegracao.Ok;
          }
        }
        return resultV2;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public ColaboradorV2Retorno IntegrationV2(ColaboradorV2Demissao view)
    {
      try
      {
        resultV2 = new ColaboradorV2Retorno
        {
          Situacao = EnumSituacaoRetornoIntegracao.Erro,
          Mensagem = new List<string>()
        };
        // Validação de Campos chave
        if (string.IsNullOrEmpty(view.Estabelecimento))
          view.Estabelecimento = "1";
        if (string.IsNullOrEmpty(view.NomeEstabelecimento))
          view.NomeEstabelecimento = "Estabelecimento Padrão";
        ValidKeyEmployee(view);
        // Validação de outros campos
        if (view.DataDemissao == null)
          view.DataDemissao = DateTime.Now.Date;
        // Gravar tabela
        if (resultV2.Mensagem.Count == 0)
        {
          PayrollEmployee payrollEmployee = payrollEmployeeService.GetAllNewVersion(p => p.Key == view.ChaveColaborador).Result.LastOrDefault();
          if (payrollEmployee == null)
            resultV2.Mensagem.Add("Colaborador não está na base de integração.");
          if (payrollEmployee.StatusIntegration != EnumStatusIntegration.Atualized)
          {
            payrollEmployee.StatusIntegration = EnumStatusIntegration.Reject;
            Task task = payrollEmployeeService.Update(payrollEmployee, null);
          }
          // Identificação
          payrollEmployee._id = null;
          payrollEmployee.Action = EnumActionIntegration.Resignation;
          payrollEmployee.DateRegister = DateTime.Now;
          payrollEmployee.StatusIntegration = EnumStatusIntegration.Saved;
          // Campos
          payrollEmployee.Situacao = EnumStatusUser.Disabled;
          payrollEmployee.ResignationDate = view.DataDemissao;
          payrollEmployee = payrollEmployeeService.InsertNewVersion(payrollEmployee).Result;
          if (payrollEmployee != null)
          {
            // Resultado Ok
            resultV2.IdPayrollEmployee = payrollEmployee._id;
            resultV2.IdUser = string.Empty;
            resultV2.IdContract = string.Empty;
            resultV2.Situacao = EnumSituacaoRetornoIntegracao.Ok;
          }
        }
        return resultV2;
      }
      catch (Exception)
      {
        throw;
      }
    }

    #region Private
    private void ValidKeyEmployee(IColaboradorV2 view)
    {
      try
      {
        if (string.IsNullOrEmpty(view.Cpf))
          resultV2.Mensagem.Add("CPF deve ser informado.");
        else
        {
          if (!IsValidCPF(view.Cpf))
            resultV2.Mensagem.Add("CPF informado inválido.");
        }
        if (string.IsNullOrEmpty(view.Empresa))
          resultV2.Mensagem.Add("Empresa deve ser informado.");
        if (string.IsNullOrEmpty(view.NomeEmpresa))
          resultV2.Mensagem.Add("Nome da empresa deve ser informado.");
        if (string.IsNullOrEmpty(view.Estabelecimento))
          resultV2.Mensagem.Add("Estabelecimento deve ser informado.");
        if (string.IsNullOrEmpty(view.NomeEstabelecimento))
          resultV2.Mensagem.Add("Nome do Estabelecimento deve ser informado.");
        if (string.IsNullOrEmpty(view.Matricula))
          resultV2.Mensagem.Add("Matricula deve ser informada.");
      }
      catch (Exception)
      {
        throw;
      }
    }
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
      cpf = cpf.Trim();
      cpf = cpf.Replace(".", "").Replace("-", "");

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

    #endregion

  }
}
