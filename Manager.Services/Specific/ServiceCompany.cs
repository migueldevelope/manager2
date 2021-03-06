﻿using Manager.Core.Base;
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
  public class ServiceCompany : Repository<Company>, IServiceCompany
  {
    private readonly ServiceGeneric<Company> serviceCompany;
    private readonly ServiceGeneric<CompanyLog> serviceCompanyLog;
    private readonly ServiceGeneric<Establishment> serviceEstablishment;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<SalaryScale> serviceSalaryScale;

    #region Constructor
    public ServiceCompany(DataContext context) : base(context)
    {
      try
      {
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceCompanyLog = new ServiceGeneric<CompanyLog>(context);
        serviceEstablishment = new ServiceGeneric<Establishment>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceSalaryScale = new ServiceGeneric<SalaryScale>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceCompany._user = _user;
      serviceCompanyLog._user = _user;
      serviceEstablishment._user = _user;
      servicePerson._user = _user;
      serviceSalaryScale._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceCompany._user = user;
      serviceCompanyLog._user = user;
      serviceEstablishment._user = user;
      servicePerson._user = user;
      serviceSalaryScale._user = user;
    }
    #endregion

    #region Company
    public string Delete(string id)
    {
      try
      {
        Company item = serviceCompany.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceCompany.Update(item, null).Wait();
        return "Company deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetLogo(string idCompany, string url)
    {
      try
      {
        Company company = serviceCompany.GetNewVersion(p => p._id == idCompany).Result;
        company.Logo = url;
        serviceCompany.Update(company, null).Wait();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string GetLogo(string idCompany)
    {
      try
      {
        return serviceCompany.GetNewVersion(p => p._id == idCompany).Result.Logo;
      }
      catch (Exception)
      {
        return string.Empty;
      }
    }
    public string New(ViewCrudCompany view)
    {
      try
      {
        Company company = serviceCompany.InsertNewVersion(new Company()
        {
          _id = view._id,
          Name = view.Name,
          Logo = view.Logo
        }).Result;

        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");

        // Save company version
        var companyLog = new CompanyLog()
        {
          Name = company.Name,
          Skills = company.Skills,
          Logo = company.Logo,
          Template = company.Template,
          Status = company.Status,
          _idAccount = company._idAccount
        };
        companyLog._idCompanyPrevious = company._id;
        companyLog.Date = datenow;
        companyLog.DateLog = DateTime.Now;

        var i = serviceCompanyLog.InsertNewVersion(companyLog);

        return "Company added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudCompany view)
    {
      try
      {
        bool propag = false;
        Company company = serviceCompany.GetNewVersion(p => p._id == view._id).Result;
        if (!company.Name.ToLower().Equals(view.Name.ToLower()))
          propag = true;
        company.Name = view.Name;
        company.Logo = view.Logo;
        serviceCompany.Update(company, null).Wait();

        if (propag)
        {
          // Task.Run(PropagInfoCompany);
        }

        return "Company altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudCompany Get(string id)
    {
      try
      {
        return serviceCompany.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudCompany GetByName(string name)
    {
      try
      {
        return serviceCompany.GetNewVersion(p => p.Name.ToLower() == name.ToLower()).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListCompany> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListCompany> detail = serviceCompany.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(x => new ViewListCompany()
          {
            _id = x._id,
            Name = x.Name
          }).ToList();
        total = serviceCompany.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    //private async Task PropagInfoCompany()
    //{

    //}

    #endregion

    #region Establishment
    public string RemoveEstablishment(string id)
    {
      try
      {
        Establishment item = serviceEstablishment.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceEstablishment.Update(item, null).Wait();
        return "Establishment deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string NewEstablishment(ViewCrudEstablishment view)
    {
      try
      {
        Company company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result;
        Establishment establishment = serviceEstablishment.InsertNewVersion(
          new Establishment()
          {
            _id = view._id,
            Name = view.Name,
            Company = company.GetViewList()
          }).Result;
        return "Establishment added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateEstablishment(ViewCrudEstablishment view)
    {
      try
      {
        Company company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result;
        Establishment establishment = serviceEstablishment.GetNewVersion(p => p._id == view._id).Result;
        establishment.Name = view.Name;
        establishment.Company = company.GetViewList();
        serviceEstablishment.Update(establishment, null).Wait();
        return "Establishment altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudEstablishment GetEstablishment(string id)
    {
      try
      {
        return serviceEstablishment.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudEstablishment GetEstablishmentByName(string idCompany, string name)
    {
      try
      {
        return serviceEstablishment.GetNewVersion(p => p.Name.ToLower() == name.ToLower() && p.Company._id == idCompany).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListEstablishment> ListEstablishment(string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListEstablishment> detail = serviceEstablishment.GetAllNewVersion(p => p.Company._id == idcompany && p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewListEstablishment
          {
            _id = p._id,
            Name = p.Name
          }).ToList();
        total = serviceEstablishment.CountNewVersion(p => p.Company._id == idcompany && p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListEstablishment> ListEstablishment(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListEstablishment> detail = serviceEstablishment.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewListEstablishment
          {
            _id = p._id,
            Name = p.Name
          }).ToList();
        total = serviceEstablishment.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

  }
}
