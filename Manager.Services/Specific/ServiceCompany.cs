using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
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
      serviceCompany._user = user;
      serviceEstablishment._user = user;
      servicePerson._user = user;
      serviceSalaryScale._user = user;
    }
    #endregion

    #region Company
    public string Remove(string id)
    {
      try
      {
        Company item = serviceCompany.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        item = serviceCompany.UpdateNewVersion(item).Result;
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
        company = serviceCompany.UpdateNewVersion(company).Result;
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
        company = serviceCompany.UpdateNewVersion(company).Result;

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
    public ViewCrudCompany Get(string id)
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
    public ViewCrudCompany GetByName(string name)
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

    private async Task PropagInfoCompany()
    {

    }
    #endregion

    #region Establishment
    public string RemoveEstablishment(string id)
    {
      try
      {
        Establishment item = serviceEstablishment.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        item = serviceEstablishment.UpdateNewVersion(item).Result;
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
            Company = company
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
        establishment.Company = company;
        establishment = serviceEstablishment.UpdateNewVersion(establishment).Result;
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
    public ViewCrudEstablishment GetEstablishmentByName(string idCompany, string name)
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

    #region Company Old
    public string NewOld(Company view)
    {
      try
      {
        serviceCompany.Insert(view);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateOld(Company view)
    {
      try
      {
        serviceCompany.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Company GetOld(string id)
    {
      try
      {
        return serviceCompany.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public Company GetByNameOld(string name)
    {
      try
      {
        return serviceCompany.GetAll(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<Company> ListOld(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceCompany.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = serviceCompany.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewEstablishmentOld(Establishment view)
    {
      try
      {
        serviceEstablishment.Insert(view);

        //var grades = gradeService.GetAll(p => p.Company._id == view.Company._id).ToList();
        //foreach (var grade in grades)
        //{
        //  var list = new List<ListSteps>();
        //  for (var step = 0; step <= 7; step++)
        //  {
        //    list.Add(new ListSteps()
        //    {
        //      _id = ObjectId.GenerateNewId().ToString(),
        //      _idAccount = _user._idAccount,
        //      Status = EnumStatus.Enabled,
        //      Salary = 0,
        //      Step = (EnumSteps)step,
        //    });
        //  }

        //  var item = new SalaryScale()
        //  {
        //    Grade = grade,
        //    Establishment = view,
        //    ListSteps = list
        //  };
        //  salaryScaleService.Insert(item);

        //}

        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateEstablishmentOld(Establishment view)
    {
      try
      {
        serviceEstablishment.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Establishment GetEstablishmentOld(string id)
    {
      try
      {
        return serviceEstablishment.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Establishment GetEstablishmentByNameOld(string idCompany, string name)
    {
      try
      {
        return serviceEstablishment.GetAll(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Establishment> ListEstablishmentOld(string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        try
        {
          int skip = (count * (page - 1));
          var detail = serviceEstablishment.GetAll(p => p.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = serviceEstablishment.GetAll(p => p.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

          return detail.ToList();
        }
        catch (Exception e)
        {
          throw e;
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Establishment> ListEstablishmentOld(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        try
        {
          int skip = (count * (page - 1));
          var detail = serviceEstablishment.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = serviceEstablishment.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

          return detail.ToList();
        }
        catch (Exception e)
        {
          throw e;
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

  }
}
