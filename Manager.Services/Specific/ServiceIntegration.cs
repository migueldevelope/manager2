using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Business.Integration;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Manager.Views.Integration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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

    #region Constructor
    public ServiceIntegration(DataContext context)
      : base(context)
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
        integrationSchoolingService = new ServiceGeneric<IntegrationSchooling>(context);
        integrationCompanyService = new ServiceGeneric<IntegrationCompany>(context);
        integrationEstablishmentService = new ServiceGeneric<IntegrationEstablishment>(context);
        integrationOccupationService = new ServiceGeneric<IntegrationOccupation>(context);
        parameterService = new ServiceGeneric<IntegrationParameter>(context);
        integrationPersonService = new ServiceGeneric<IntegrationPerson>(context);
        logService = new ServiceLog(context);
      }
      catch (Exception)
      {
        throw;
      }
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

    // Parcial
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
          integrationCompanyService.Insert(item);
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
      catch (Exception)
      {
        throw;
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
        integrationCompanyService.Delete(idIntegration,false);
        return "Ok";
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    // Parcial
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
          integrationEstablishmentService.Insert(item);
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

    // Parcial
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
          integrationOccupationService.Insert(item);
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
        item._idCompany = occupation.ProcessLevelTwo.ProcessLevelOne.Area.Company._id;
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

    // Parcial
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
          integrationSchoolingService.Insert(item);
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
        item.NameSchooling = item.Name;
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

    // ok
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
          param = parameterService.Insert(new IntegrationParameter()
          {
            Mode = EnumIntegrationMode.DataBaseV1,
            Type = EnumIntegrationType.Complete,
            Status = EnumStatus.Enabled
          });
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
          _id = param._id
        };
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

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
        integrationPersonService.Insert(integrationPerson);
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

    #region Gets isolados por id
    public Schooling GetSchooling(string id)
    {
      try
      {
        return schoolingService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public Company GetCompany(string id)
    {
      try
      {
        return companyService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public Establishment GetEstablishment(string id)
    {
      try
      {
        return establishmentService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception)
      {
        throw;
      }
    }
    public Occupation GetOccupation(string id)
    {
      try
      {
        return occupationService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #region Configuração de contexto
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
      logService.SetUser(contextAccessor);
    }

    public void SetUser(BaseUser user)
    {
      _user = user;
      personService._user = user;
      schoolingService._user = user;
      companyService._user = user;
      accountService._user = user;
      occupationService._user = _user;
      establishmentService._user = _user;
      integrationSchoolingService._user = _user;
      integrationCompanyService._user = _user;
      integrationEstablishmentService._user = _user;
      integrationOccupationService._user = _user;
      integrationPersonService._user = _user;
      parameterService._user = _user;
    }
    #endregion

    public User GetUserByKey(string document)
    {
      try
      {
        return userService.GetAll(p => p.Document == document).SingleOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Person GetPersonByKey(string idcompany, string idestablishment, string document, long registration)
    {
      try
      {
        IQueryable<Person> personsDocument = personService.GetAll(p => p.User.Document == document);
        if (personsDocument.Count() == 0)
          return null;
        return personsDocument.Where(p => p.Company._id == idcompany && p.Establishment._id == idestablishment && p.Registration == registration.ToString()).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
  }
}
