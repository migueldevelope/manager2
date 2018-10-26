using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Business.Integration;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.Enumns;
using Manager.Views.Integration;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tools;

namespace Manager.Services.Specific
{
  public class ServiceIntegration : Repository<Person>, IServiceIntegration
  {
    private readonly ServiceGeneric<Person> personService;
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
    private readonly IServiceLog logService;

    public ServiceIntegration(DataContext context)
      : base(context)
    {
      try
      {
        personService = new ServiceGeneric<Person>(context);
        schoolingService = new ServiceGeneric<Schooling>(context);
        companyService = new ServiceGeneric<Company>(context);
        establishmentService = new ServiceGeneric<Establishment>(context);
        accountService = new ServiceGeneric<Account>(context);
        occupationService = new ServiceGeneric<Occupation>(context);
        integrationSchoolingService = new ServiceGeneric<IntegrationSchooling>(context);
        integrationCompanyService = new ServiceGeneric<IntegrationCompany>(context);
        integrationEstablishmentService = new ServiceGeneric<IntegrationEstablishment>(context);
        integrationOccupationService = new ServiceGeneric<IntegrationOccupation>(context);
        parameterService= new ServiceGeneric<IntegrationParameter>(context);
        logService = new ServiceLog(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public List<Schooling> GetIntegrationSchooling(string code, string name)
    {
      try
      {
        IntegrationSchooling item = integrationSchoolingService.GetAll(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
        if (item != null)
          if (item.Schooling == null)
            return new List<Schooling>();
          else
            return new List<Schooling>() { item.Schooling };
        List<Schooling> list = schoolingService.GetAll(p => p.Name.ToLower() == name.ToLower()).ToList();
        item = new IntegrationSchooling()
        {
          Code = code,
          Name = name,
          Schooling = null,
          Status = EnumStatus.Enabled
        };

        if (list.Count == 1)
          item.Schooling = list[0];

        integrationSchoolingService.Insert(item);
        return list;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Company> GetIntegrationCompany(string code, string name)
    {
      try
      {
        IntegrationCompany item = integrationCompanyService.GetAll(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
        if (item != null)
          if (item.Company == null)
            return new List<Company>();
          else
            return new List<Company>() { item.Company };
        List<Company> list = companyService.GetAll(p => p.Name.ToLower() == name.ToLower()).ToList();
        item = new IntegrationCompany()
        {
          Code = code,
          Name = name,
          Company = null,
          Status = EnumStatus.Enabled
        };

        if (list.Count == 1)
          item.Company = list[0];

        integrationCompanyService.Insert(item);
        return list;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Establishment> GetIntegrationEstablishment(string idcompany, string code, string name)
    {
      try
      {
        IntegrationEstablishment item = integrationEstablishmentService.GetAll(p => p._idCompany == idcompany && p.Name.ToLower() == name.ToLower()).FirstOrDefault();
        if (item != null)
          if (item.Establishment == null)
            return new List<Establishment>();
          else
            return new List<Establishment>() { item.Establishment };
        List<Establishment> list = establishmentService.GetAll(p => p.Name.ToLower() == name.ToLower()).ToList();
        item = new IntegrationEstablishment()
        {
          Code = code,
          Name = name,
          _idCompany = idcompany,
          Establishment = null,
          Status = EnumStatus.Enabled
        };

        if (list.Count == 1)
          item.Establishment = list[0];

        integrationEstablishmentService.Insert(item);
        return list;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Occupation> GetIntegrationOccupation(string idcompany, string code, string name)
    {
      try
      {
        IntegrationOccupation item = integrationOccupationService.GetAll(p => p._idCompany == idcompany && p.Name.ToLower() == name.ToLower()).FirstOrDefault();
        if (item != null)
          if (item.Occupation == null)
            return new List<Occupation>();
          else
            return new List<Occupation>() { item.Occupation };
        List<Occupation> list = occupationService.GetAll(p => p.ProcessLevelTwo.ProcessLevelOne.Area.Company._id == idcompany && p.Name.ToLower() == name.ToLower()).ToList();
        item = new IntegrationOccupation()
        {
          Code = code,
          Name = name,
          _idCompany = idcompany,
          Occupation = null,
          Status = EnumStatus.Enabled
        };

        if (list.Count == 1)
          item.Occupation = list[0];

        integrationOccupationService.Insert(item);
        return list;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public Person GetPersonByKey(string document, string idcompany, long registration)
    {
      try
      {
        return personService.GetAll(p => p.Document == document && p.Company._id == idcompany && p.Registration == registration).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

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

    public IntegrationParameter GetIntegrationParameter()
    {
      try
      {
        var param = parameterService.GetAll().FirstOrDefault();
        if (param == null)
        {
          param = parameterService.Insert(new IntegrationParameter()
          {
            Mode = EnumIntegrationMode.DataBaseV1,
            Type = EnumIntegrationType.Complete,
            Status = EnumStatus.Enabled
          });
        }
        return param;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public IntegrationParameter SetIntegrationParameter(ViewIntegrationParameterMode view)
    {
      try
      {
        IntegrationParameter param = parameterService.GetAll().FirstOrDefault();
        param.Mode = view.Mode;
        param.Process = view.Process;
        param.Type = view.Type;
        param.ConnectionString = view.ConnectionString;
        param.SqlCommand = view.SqlCommand;
        param.FilePathLocal = view.FilePathLocal;
        parameterService.Update(param, null);
        return param;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public IntegrationParameter SetIntegrationParameter(ViewIntegrationParameterPack view)
    {
      try
      {
        IntegrationParameter param = parameterService.GetAll().FirstOrDefault();
        param.VersionPackProgram = view.VersionPackProgram;
        param.LinkPackProgram = view.LinkPackProgram;
        param.VersionPackCustom = view.VersionPackCustom;
        param.LinkPackCustom = view.LinkPackCustom;
        param.MessageAtualization = view.MessageAtualization;
        parameterService.Update(param, null);
        return param;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public IntegrationParameter SetIntegrationParameter(ViewIntegrationParameterExecution view)
    {
      try
      {
        IntegrationParameter param = parameterService.GetAll().FirstOrDefault();
        param.StatusExecution = view.StatusExecution;
        param.ProgramVersionExecution = view.ProgramVersionExecution;
        param.CustomVersionExecution = view.CustomVersionExecution;
        param.CriticalError = view.CriticalError;
        param.MachineIdentity = view.MachineIdentity;
        param.UploadNextLog = view.UploadNextLog;
        param.LinkLogExecution = view.LinkLogExecution;
        parameterService.Update(param, null);
        return param;
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
      schoolingService._user = _user;
      companyService._user = _user;
      accountService._user = _user;
      occupationService._user = _user;
      establishmentService._user = _user;
      integrationSchoolingService._user = _user;
      integrationCompanyService._user = _user;
      integrationEstablishmentService._user = _user;
      integrationOccupationService._user = _user;
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
      parameterService._user = _user;
    }
  }
}
