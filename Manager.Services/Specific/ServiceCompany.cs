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
    private readonly ServiceGeneric<SalaryScale> salaryScaleService;
    private readonly ServiceGeneric<Grade> gradeService;

    #region Constructor
    public BaseUser user { get => _user; set => user = _user; }


    public ServiceCompany(DataContext context)
      : base(context)
    {
      try
      {
        companyService = new ServiceGeneric<Company>(context);
        personService = new ServiceGeneric<Person>(context);
        establishmentService = new ServiceGeneric<Establishment>(context);
        salaryScaleService = new ServiceGeneric<SalaryScale>(context);
        gradeService = new ServiceGeneric<Grade>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      companyService._user = _user;
      personService._user = _user;
      establishmentService._user = _user;
      salaryScaleService._user = _user;
      gradeService._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      _user = baseUser;
      companyService._user = baseUser;
      personService._user = baseUser;
      establishmentService._user = baseUser;
      salaryScaleService._user = baseUser;
      gradeService._user = baseUser;
    }
    #endregion

    #region Company
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

    public string New(ViewCrudCompany view)
    {
      try
      {
        companyService.Insert(new Company() { _id = view._id, Name = view.Name, Logo = view.Logo });
        return "add success";
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
        var company = companyService.GetAll(p => p._id == view._id).FirstOrDefault();
        company.Name = view.Name;
        company.Logo = view.Logo;
        companyService.Update(company, null);
        return "update";
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
        return companyService.GetAll(p => p._id == id).Select(p => new ViewCrudCompany()
        {
          _id = p._id,
          Name = p.Name,
          Logo = p.Logo
        }).FirstOrDefault();
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
        return companyService.GetAll(p => p.Name.ToLower() == name.ToLower()).Select(p => new ViewCrudCompany()
        {
          _id = p._id,
          Name = p.Name,
          Logo = p.Logo
        }).FirstOrDefault();
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
        int skip = (count * (page - 1));
        var detail = companyService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = companyService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.Select(p => new ViewListCompany
        {
          _id = p._id,
          Name = p.Name
        }).ToList();

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
        var company = companyService.GetAll(p => p._id == view.Company._id).FirstOrDefault();
        establishmentService.Insert(new Establishment() { _id = view._id, Name = view.Name, Company = company });
        return "add success";
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
        var company = companyService.GetAll(p => p._id == view.Company._id).FirstOrDefault();
        var establishment = establishmentService.GetAll(p => p._id == view._id).FirstOrDefault();
        establishment.Name = view.Name;
        establishment.Company = company;
        establishmentService.Update(establishment, null);
        return "update";
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
        return establishmentService.GetAll(p => p._id == id).Select(p => new ViewCrudEstablishment()
        {
          _id = p._id,
          Name = p.Name,
          Company = new ViewListCompany() { _id = p.Company._id, Name = p.Company.Name }
        }).FirstOrDefault();
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
        return establishmentService.GetAll(p => p.Name.ToLower() == name.ToLower()).Select(p => new ViewCrudEstablishment()
        {
          _id = p._id,
          Name = p.Name,
          Company = new ViewListCompany() { _id = p.Company._id, Name = p.Company.Name }
        }).FirstOrDefault();
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
        try
        {
          int skip = (count * (page - 1));
          var detail = establishmentService.GetAll(p => p.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = establishmentService.GetAll(p => p.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

          return detail.Select(p => new ViewListEstablishment
          {
            _id = p._id,
            Name = p.Name
          }).ToList();
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

    public List<ViewListEstablishment> ListEstablishment(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        try
        {
          int skip = (count * (page - 1));
          var detail = establishmentService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = establishmentService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

          return detail.Select(p => new ViewListEstablishment
          {
            _id = p._id,
            Name = p.Name
          }).ToList();
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


    #endregion


    #region Company Old
    public string NewOld(Company view)
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

    public string UpdateOld(Company view)
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

    public Company GetOld(string id)
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
    public Company GetByNameOld(string name)
    {
      try
      {
        return companyService.GetAll(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
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
        var detail = companyService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = companyService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

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
        establishmentService.Insert(view);

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
        establishmentService.Update(view, null);
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
        return establishmentService.GetAll(p => p._id == id).FirstOrDefault();
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
        return establishmentService.GetAll(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
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
          var detail = establishmentService.GetAll(p => p.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = establishmentService.GetAll(p => p.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

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

    public List<Establishment> ListEstablishmentOld(ref long total, int count = 10, int page = 1, string filter = "")
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

    #endregion

  }
}
