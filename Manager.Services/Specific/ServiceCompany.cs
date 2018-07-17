using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Linq;

namespace Manager.Services.Specific
{
  public class ServiceCompany : Repository<Company>, IServiceCompany
  {
    private readonly ServiceGeneric<Company> companyService;
    private readonly ServiceGeneric<Person> personService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceCompany(DataContext context)
      : base(context)
    {
      try
      {
        companyService = new ServiceGeneric<Company>(context);
        personService = new ServiceGeneric<Person>(context);
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
    }

    public void SetUser(BaseUser baseUser)
    {
      companyService._user = baseUser;
      personService._user = baseUser;
    }
  }
}
