using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Business.Integration;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Manager.Views.Integration;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

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
    }
    #endregion

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
          CompanyError = integrationCompanyService.GetAll(p => p.IdCompany == "000000000000000000000000").Count(),
          EstablishmentError = integrationEstablishmentService.GetAll(p => p.IdEstablishment == "000000000000000000000000").Count(),
          OccupationError = integrationOccupationService.GetAll(p => p.IdOccupation == "000000000000000000000000").Count(),
          SchoolingError = integrationSchoolingService.GetAll(p => p.IdSchooling == "000000000000000000000000").Count(),
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
        IntegrationCompany integrationCompany = integrationCompanyService.GetAll(p => p.IdCompany == "000000000000000000000000").FirstOrDefault();
        if (integrationCompany != null)
          throw new Exception("Verificar integração de empresas!");

        IntegrationSchooling integrationSchooling = integrationSchoolingService.GetAll(p => p.IdSchooling == "000000000000000000000000").FirstOrDefault();
        if (integrationSchooling != null)
          throw new Exception("Verificar integração de escolaridades!");

        IntegrationEstablishment integrationEstablishment = integrationEstablishmentService.GetAll(p => p.IdEstablishment == "000000000000000000000000").FirstOrDefault();
        if (integrationEstablishment != null)
          throw new Exception("Verificar integração de estabelecimentos!");

        IntegrationOccupation integrationOccupation = integrationOccupationService.GetAll(p => p.IdOccupation == "000000000000000000000000").FirstOrDefault();
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
        IntegrationCompany item = integrationCompanyService.GetAll(p => p.Key == key).FirstOrDefault();
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
          integrationCompanyService.InsertNewVersion(item);
        }
        if (item.IdCompany.Equals("000000000000000000000000"))
        {
          item.Name = name;
          List<Company> companies = companyService.GetAll(p => p.Name.ToLower() == name.ToLower()).ToList<Company>();
          if (companies.Count == 1)
          {
            item.IdCompany = companies[0]._id;
            item.NameCompany = companies[0].Name;
            integrationCompanyService.Update(item, null);
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
          detail = integrationCompanyService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count);
          total = integrationCompanyService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }
        else
        {
          detail = integrationCompanyService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdCompany == "000000000000000000000000").OrderBy(p => p.Name).Skip(skip).Take(count);
          total = integrationCompanyService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdCompany == "000000000000000000000000").Count();
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
        IntegrationCompany item = integrationCompanyService.GetAll(p => p._id == idIntegration).FirstOrDefault();
        if (item == null)
          throw new Exception("Id integration not found!");
        item.IdCompany = idCompany;
        item.NameCompany = companyService.GetAll(p => p._id == idCompany).FirstOrDefault().Name;
        integrationCompanyService.Update(item, null);
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
        IntegrationCompany item = integrationCompanyService.GetAll(p => p._id == idIntegration).FirstOrDefault();
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
        IntegrationEstablishment item = integrationEstablishmentService.GetAll(p => p.Key == key).FirstOrDefault();
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
          integrationEstablishmentService.InsertNewVersion(item);
        }
        if (item.IdEstablishment.Equals("000000000000000000000000"))
        {
          item.Name = name;
          item._idCompany = idcompany;
          List<Establishment> establishments = establishmentService.GetAll(p => p.Company._id == idcompany && p.Name.ToLower() == name.ToLower()).ToList<Establishment>();
          if (establishments.Count == 1)
          {
            item.IdEstablishment = establishments[0]._id;
            item.NameEstablishment = establishments[0].Name;
            integrationEstablishmentService.Update(item, null);
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
          detail = integrationEstablishmentService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count);
          total = integrationEstablishmentService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }
        else
        {
          detail = integrationEstablishmentService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdEstablishment == "000000000000000000000000").OrderBy(p => p.Name).Skip(skip).Take(count);
          total = integrationEstablishmentService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdEstablishment == "000000000000000000000000").Count();
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
            NameCompany = companyService.GetAll(p => p._id == item._idCompany).FirstOrDefault().Name,
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
        IntegrationEstablishment item = integrationEstablishmentService.GetAll(p => p._id == idIntegration).FirstOrDefault();
        if (item == null)
          throw new Exception("Id integration not found!");
        item.IdEstablishment = idEstablishment;
        Establishment establishment = establishmentService.GetAll(p => p._id == idEstablishment).FirstOrDefault();
        item.NameEstablishment = establishment.Name;
        item._idCompany = establishment.Company._id;
        integrationEstablishmentService.Update(item, null);
        return new ViewListIntegrationEstablishment()
        {
          _id = item._id,
          Name = item.Name,
          Key = item.Key,
          IdCompany = item._idCompany,
          NameCompany = companyService.GetAll(p => p._id == item._idCompany).FirstOrDefault().Name,
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
        IntegrationEstablishment item = integrationEstablishmentService.GetAll(p => p._id == idIntegration).FirstOrDefault();
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
        IntegrationOccupation item = integrationOccupationService.GetAll(p => p.Key == key).FirstOrDefault();
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
          integrationOccupationService.InsertNewVersion(item);
        }
        if (item.IdOccupation.Equals("000000000000000000000000"))
        {
          item.Name = name;
          item._idCompany = idcompany;
          List<Occupation> occupations = occupationService.GetAll(p => p.Group.Company._id == idcompany && p.Name.ToLower() == name.ToLower()).ToList<Occupation>();
          if (occupations.Count == 1)
          {
            item.IdOccupation = occupations[0]._id;
            item.NameOccupation = occupations[0].Name;
            integrationOccupationService.Update(item, null);
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
          detail = integrationOccupationService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count);
          total = integrationOccupationService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }
        else
        {
          detail = integrationOccupationService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdOccupation == "000000000000000000000000").OrderBy(p => p.Name).Skip(skip).Take(count);
          total = integrationOccupationService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdOccupation == "000000000000000000000000").Count();
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
            NameCompany = companyService.GetAll(p => p._id == item._idCompany).FirstOrDefault().Name,
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
        IntegrationOccupation item = integrationOccupationService.GetAll(p => p._id == idIntegration).FirstOrDefault();
        if (item == null)
          throw new Exception("Id integration not found!");
        item.IdOccupation = idOccupation;
        Occupation occupation = occupationService.GetAll(p => p._id == idOccupation).FirstOrDefault();
        item.NameOccupation = occupation.Name;
        item._idCompany = occupation.Group.Company._id;
        integrationOccupationService.Update(item, null);
        return new ViewListIntegrationOccupation()
        {
          _id = item._id,
          Name = item.Name,
          Key = item.Key,
          IdCompany = item._idCompany,
          NameCompany = companyService.GetAll(p => p._id == item._idCompany).FirstOrDefault().Name,
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
        IntegrationOccupation item = integrationOccupationService.GetAll(p => p._id == idIntegration).FirstOrDefault();
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
        IntegrationSchooling item = integrationSchoolingService.GetAll(p => p.Key == key).FirstOrDefault();
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
          integrationSchoolingService.InsertNewVersion(item);
        }
        if (item.IdSchooling.Equals("000000000000000000000000"))
        {
          item.Name = name;
          List<Schooling> schoolings = schoolingService.GetAll(p => p.Name.ToLower() == name.ToLower()).ToList<Schooling>();
          if (schoolings.Count == 1)
          {
            item.IdSchooling = schoolings[0]._id;
            item.NameSchooling = schoolings[0].Name;
            integrationSchoolingService.Update(item, null);
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
          detail = integrationSchoolingService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count);
          total = integrationSchoolingService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }
        else
        {
          detail = integrationSchoolingService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdSchooling == "000000000000000000000000").OrderBy(p => p.Name).Skip(skip).Take(count);
          total = integrationSchoolingService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper()) && p.IdSchooling == "000000000000000000000000").Count();
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
        IntegrationSchooling item = integrationSchoolingService.GetAll(p => p._id == idIntegration).FirstOrDefault();
        if (item == null)
          throw new Exception("Id integration not found!");
        item.IdSchooling = idSchooling;
        item.NameSchooling = schoolingService.GetAll(p => p._id == idSchooling).FirstOrDefault().Name;
        integrationSchoolingService.Update(item, null);
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
        IntegrationSchooling item = integrationSchoolingService.GetAll(p => p._id == idIntegration).FirstOrDefault();
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
        IntegrationParameter param = parameterService.GetAll().FirstOrDefault();
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
        IntegrationParameter param = parameterService.GetAll().FirstOrDefault();
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
        parameterService.Update(param, null);
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
        return integrationPersonService.GetAll(p => p.Key == key).FirstOrDefault();
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
        integrationPersonService.InsertNewVersion(integrationPerson);
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
        integrationPersonService.Update(integrationPerson, null);
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
        Schooling result = schoolingService.GetAll(p => p._id == id).FirstOrDefault();
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
        var result = companyService.GetAll(p => p._id == id).FirstOrDefault();
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
        var result = establishmentService.GetAll(p => p._id == id).FirstOrDefault();
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
    public ViewListOccupation GetOccupation(string id)
    {
      try
      {
        var result = occupationService.GetAll(p => p._id == id).FirstOrDefault();
        return result != null
          ? new ViewListOccupation()
          {
            _id = result._id,
            Name = result.Name,
            Line = result.Line,
            Company = new ViewListCompany() { _id = result.Group.Company._id, Name = result.Group.Company.Name },
            Group = new ViewListGroup()
            {
              _id = result.Group._id,
              Name = result.Group.Name,
              Line = result.Group.Line,
              Axis = new ViewListAxis() { _id = result.Group.Axis._id, Name = result.Group.Axis.Name, TypeAxis = result.Group.Axis.TypeAxis },
              Sphere = new ViewListSphere() { _id = result.Group.Sphere._id, Name = result.Group.Sphere.Name, TypeSphere = result.Group.Sphere.TypeSphere }
            },
            Process = result.Process?.OrderBy(x => x.ProcessLevelOne.Area.Name).ThenBy(x => x.ProcessLevelOne.Order).ThenBy(x => x.Order)
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
                Area = new ViewListArea() { _id = x.ProcessLevelOne.Area._id, Name = x.ProcessLevelOne.Area.Name }
              }
            })
            .ToList()
          }
          : null;
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
        User user = userService.GetAll(p => p.Document == document).SingleOrDefault();
        return user == null
          ? null
          : new ViewCrudUser()
          {
            DateAdm = user.DateAdm,
            DateBirth = user.DateBirth,
            Document = user.Document,
            DocumentCTPF = user.DocumentCTPF,
            DocumentID = user.DocumentID,
            Mail = user.Mail,
            Name = user.Mail,
            Nickname = user.Nickname,
            Password = string.Empty,
            Phone = user.Phone,
            PhoneFixed = user.PhoneFixed,
            PhotoUrl = user.PhotoUrl,
            Sex = user.Sex,
            Schooling = user.Schooling == null ? null : new ViewListSchooling() { _id = user.Schooling._id, Name = user.Schooling.Name, Order = user.Schooling.Order },
            _id = user._id
          };
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
        IQueryable<Person> personsDocument = personService.GetAll(p => p.User.Document == document);
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
            Occupation = person.Occupation == null ? null : new ViewListOccupation()
            {
              _id = person.Occupation._id,
              Name = person.Occupation.Name,
              Line = person.Occupation.Line,
              Company = new ViewListCompany() { _id = person.Occupation.Group.Company._id, Name = person.Occupation.Group.Company.Name },
              Group = new ViewListGroup()
              {
                _id = person.Occupation.Group._id,
                Name = person.Occupation.Group.Name,
                Line = person.Occupation.Group.Line,
                Axis = new ViewListAxis()
                {
                  _id = person.Occupation.Group.Axis._id,
                  Name = person.Occupation.Group.Axis.Name,
                  TypeAxis = person.Occupation.Group.Axis.TypeAxis
                },
                Sphere = new ViewListSphere()
                {
                  _id = person.Occupation.Group.Sphere._id,
                  Name = person.Occupation.Group.Sphere.Name,
                  TypeSphere = person.Occupation.Group.Sphere.TypeSphere
                }
              },
              Process = person.Occupation.Process.Select(p => new ViewListProcessLevelTwo()
              {
                _id = p._id,
                Name = p.Name,
                Order = p.Order,
                ProcessLevelOne = new ViewListProcessLevelOne()
                {
                  _id = p.ProcessLevelOne._id,
                  Name = p.ProcessLevelOne.Name,
                  Order = p.ProcessLevelOne.Order,
                  Area = new ViewListArea()
                  {
                    _id = p.ProcessLevelOne.Area._id,
                    Name = p.ProcessLevelOne.Area.Name
                  }
                }
              }).ToList()
            },
            Registration = person.Registration,
            Salary = person.Salary,
            StatusUser = person.StatusUser,
            TypeJourney = person.TypeJourney,
            TypeUser = person.TypeUser,
            User = new ViewCrudUser()
            {
              Name = person.User.Name,
              Nickname = person.User.Nickname,
              DateAdm = person.User.DateAdm,
              DateBirth = person.User.DateBirth,
              Document = person.User.Document,
              DocumentCTPF = person.User.DocumentCTPF,
              DocumentID = person.User.DocumentID,
              Mail = person.User.Mail,
              Password = string.Empty,
              Phone = person.User.Phone,
              PhoneFixed = person.User.PhoneFixed,
              PhotoUrl = person.User.PhotoUrl,
              Schooling = person.User.Schooling == null ? null : new ViewListSchooling() { _id = person.User.Schooling._id, Name = person.User.Schooling.Name, Order = person.User.Schooling.Order },
              Sex = person.User.Sex,
              _id = person.User._id
            }
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
              Area = new ViewListArea() { _id = x.ProcessLevelOne.Area._id, Name = x.ProcessLevelOne.Area.Name }
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
      if (occupation == null)
      {
        occupation = new Occupation()
        {
          Group = groupService.GetNewVersion(p => p.Name == view.NameGroup).Result,
          CBO = null,
          Name = Capitalization(view.Name),
          SpecificRequirements = view.SpecificRequirements,
          SalaryScales = null,
          Process = new List<ProcessLevelTwo>(),
          Skills = new List<Skill>(),
          Activities = new List<Activitie>(),
          Schooling = new List<Schooling>(),
          Line = 0
        };
        occupation.Process.Add(processLevelTwoService.GetNewVersion(p => p._id == view.IdProcessLevelTwo).Result);
        Skill skill;
        foreach (string item in view.Skills)
        {
          skill = skillService.GetNewVersion(p => p.Name == item).Result;
          occupation.Skills.Add(skill);
        }
        int order = 0;
        foreach (string item in view.Activities)
        {
          order++;
          occupation.Activities.Add(new Activitie()
          {
            _id = ObjectId.GenerateNewId().ToString(),
            _idAccount = _user._idAccount,
            Name = item,
            Order = order,
            Status = EnumStatus.Enabled
          });
        }
        occupation.Schooling = occupation.Group.Schooling;
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
        Schooling = occupation.Group.Schooling.OrderBy(o => o.Order).Select(x => x.Name).ToList(),
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

  }
}
