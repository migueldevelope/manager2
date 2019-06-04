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
  public class ServiceCompany : Repository<Company>, IServiceCompany
  {
    private readonly ServiceGeneric<Company> serviceCompany;
    private readonly ServiceGeneric<Establishment> serviceEstablishment;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<SalaryScale> serviceSalaryScale;

    #region Constructor
    public ServiceCompany(DataContext context) : base(context)
    {
      try
      {
        serviceCompany = new ServiceGeneric<Company>(context);
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
      serviceEstablishment._user = _user;
      servicePerson._user = _user;
      serviceSalaryScale._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceCompany._user = user;
      serviceEstablishment._user = user;
      servicePerson._user = user;
      serviceSalaryScale._user = user;
    }
    #endregion

    #region Company
    public async Task<string> Delete(string id)
    {
      try
      {
        Company item = serviceCompany.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceCompany.Update(item, null);
        return "Company deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task SetLogo(string idCompany, string url)
    {
      try
      {
        Company company = serviceCompany.GetNewVersion(p => p._id == idCompany).Result;
        company.Logo = url;
        serviceCompany.Update(company, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<string> GetLogo(string idCompany)
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
    public async Task<string> New(ViewCrudCompany view)
    {
      try
      {
        Company company = serviceCompany.InsertNewVersion(new Company()
          {
            _id = view._id,
            Name = view.Name,
            Logo = view.Logo
          }).Result;
        return "Company added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<string> Update(ViewCrudCompany view)
    {
      try
      {
        bool propag = false;
        Company company = serviceCompany.GetNewVersion(p => p._id == view._id).Result;
        if (!company.Name.ToLower().Equals(view.Name.ToLower()))
          propag = true;
        company.Name = view.Name;
        company.Logo = view.Logo;
        serviceCompany.Update(company, null);

        if (propag)
        {
          Task.Run(PropagInfoCompany);
        }

        return "Company altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<ViewCrudCompany> Get(string id)
    {
      try
      {
        Company company = serviceCompany.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudCompany()
        {
          _id = company._id,
          Name = company.Name,
          Logo = company.Logo
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<ViewCrudCompany> GetByName(string name)
    {
      try
      {
        Company company = serviceCompany.GetNewVersion(p => p.Name.ToLower() == name.ToLower()).Result;
        return new ViewCrudCompany()
        {
          _id = company._id,
          Name = company.Name,
          Logo = company.Logo
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<List<ViewListCompany>> List( int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListCompany> detail = serviceCompany.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(x => new ViewListCompany()
          {
            _id = x._id,
            Name = x.Name            
          }).ToList();
        var total = serviceCompany.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async Task PropagInfoCompany()
    {

    }

    #endregion

    #region Establishment
    public async Task<string> RemoveEstablishment(string id)
    {
      try
      {
        Establishment item = serviceEstablishment.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceEstablishment.Update(item, null);
        return "Establishment deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<string> NewEstablishment(ViewCrudEstablishment view)
    {
      try
      {
        Company company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result;
        Establishment establishment = serviceEstablishment.InsertNewVersion(
          new Establishment()
          {
            _id = view._id,
            Name = view.Name,
            Company = company
          }).Result;
        return "Establishment added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<string> UpdateEstablishment(ViewCrudEstablishment view)
    {
      try
      {
        Company company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result;
        Establishment establishment = serviceEstablishment.GetNewVersion(p => p._id == view._id).Result;
        establishment.Name = view.Name;
        establishment.Company = company;
        serviceEstablishment.Update(establishment, null);
        return "Establishment altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<ViewCrudEstablishment> GetEstablishment(string id)
    {
      try
      {
        Establishment establishment = serviceEstablishment.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudEstablishment()
        {
          _id = establishment._id,
          Name = establishment.Name,
          Company = new ViewListCompany() { _id = establishment.Company._id, Name = establishment.Company.Name }
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<ViewCrudEstablishment >GetEstablishmentByName(string idCompany, string name)
    {
      try
      {
        Establishment establishment = serviceEstablishment.GetNewVersion(p => p.Name.ToLower() == name.ToLower() && p.Company._id == idCompany).Result;
        return new ViewCrudEstablishment()
        {
          _id = establishment._id,
          Name = establishment.Name,
          Company = new ViewListCompany() { _id = establishment.Company._id, Name = establishment.Company.Name }
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<List<ViewListEstablishment>> ListEstablishment(string idcompany,  int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListEstablishment> detail = serviceEstablishment.GetAllNewVersion(p => p.Company._id == idcompany && p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewListEstablishment
          {
            _id = p._id,
            Name = p.Name
          }).ToList();
        var total = serviceEstablishment.CountNewVersion(p => p.Company._id == idcompany && p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<List<ViewListEstablishment>> ListEstablishment( int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListEstablishment> detail = serviceEstablishment.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewListEstablishment
          {
            _id = p._id,
            Name = p.Name
          }).ToList();
        var total = serviceEstablishment.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
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
