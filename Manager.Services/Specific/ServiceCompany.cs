using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Services.Specific
{
  public class ServiceCompany : Repository<Company>, IServiceCompany
  {
    private readonly ServiceGeneric<Company> companyService;
    private readonly ServiceGeneric<Establishment> establishmentService;
    private readonly ServiceGeneric<Person> personService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceCompany(DataContext context)
      : base(context)
    {
      try
      {
        companyService = new ServiceGeneric<Company>(context);
        personService = new ServiceGeneric<Person>(context);
        establishmentService = new ServiceGeneric<Establishment>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetLogo(string idCompany, string url)
    {
      try
      {
        var company = companyService.GetAll(p => p._id == idCompany).SingleOrDefault();
        company.Logo = url;
        companyService.Update(company, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string GetLogo(string idCompany)
    {
      try
      {
        var company = companyService.GetAll(p => p._id == idCompany).SingleOrDefault();
        return company.Logo;
      }
      catch (Exception)
      {
        return "";
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      companyService._user = _user;
      personService._user = _user;
      establishmentService._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      companyService._user = baseUser;
      personService._user = baseUser;
      establishmentService._user = baseUser;
    }

    public string New(Company view)
    {
      try
      {
        companyService.Insert(view);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Update(Company view)
    {
      try
      {
        companyService.Update(view, null);
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
        var item = companyService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        companyService.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Company Get(string id)
    {
      try
      {
        return companyService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Company> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        try
        {
          int skip = (count * (page - 1));
          var detail = companyService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = companyService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

          return detail.ToList();
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

    public string NewEstablishment(Establishment view)
    {
      try
      {
        establishmentService.Insert(view);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateEstablishment(Establishment view)
    {
      try
      {
        establishmentService.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveEstablishment(string id)
    {
      try
      {
        var item = establishmentService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        establishmentService.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Establishment GetEstablishment(string id)
    {
      try
      {
        return establishmentService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Establishment> ListEstablishment(string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        try
        {
          int skip = (count * (page - 1));
          var detail = establishmentService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = establishmentService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

          return detail.ToList();
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
