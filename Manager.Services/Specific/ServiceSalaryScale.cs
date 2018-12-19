using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceSalaryScale : Repository<SalaryScale>, IServiceSalaryScale
  {
    private readonly ServiceGeneric<SalaryScale> salaryScaleService;
    private readonly ServiceGeneric<Grade> gradeService;
    private readonly ServiceGeneric<Occupation> occupationService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<Establishment> establishmentService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceSalaryScale(DataContext context)
      : base(context)
    {
      try
      {
        salaryScaleService = new ServiceGeneric<SalaryScale>(context);
        personService = new ServiceGeneric<Person>(context);
        gradeService = new ServiceGeneric<Grade>(context);
        establishmentService = new ServiceGeneric<Establishment>(context);
        occupationService = new ServiceGeneric<Occupation>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }


    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      salaryScaleService._user = _user;
      personService._user = _user;
      gradeService._user = _user;
      establishmentService._user = _user;
      occupationService._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      _user = baseUser;
      salaryScaleService._user = baseUser;
      personService._user = baseUser;
      gradeService._user = baseUser;
      establishmentService._user = baseUser;
      occupationService._user = baseUser;
    }



    public string Remove(string id)
    {
      try
      {
        var item = salaryScaleService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        salaryScaleService.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public SalaryScale Get(string id)
    {
      try
      {
        return salaryScaleService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<SalaryScale> List(string idcompany, string idestablishment, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = salaryScaleService.GetAll(p => p.Grade.Company._id == idcompany & p.Establishment._id == idestablishment & p.Grade.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Grade.Order).Skip(skip).Take(count).ToList();
        total = salaryScaleService.GetAll(p => p.Grade.Company._id == idcompany & p.Establishment._id == idestablishment & p.Grade.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewGrade(Grade view)
    {
      try
      {
        gradeService.Insert(view);
        var establisments = establishmentService.GetAll(p => p.Company._id == view.Company._id).ToList();
        foreach (var est in establisments)
        {
          var list = new List<ListSteps>();
          for (var step = 0; step <= 7; step++)
          {
            list.Add(new ListSteps()
            {
              _id = ObjectId.GenerateNewId().ToString(),
              _idAccount = _user._idAccount,
              Status = EnumStatus.Enabled,
              Salary = 0,
              Step = (EnumSteps)step,
            });
          }

          var item = new SalaryScale()
          {
            Grade = view,
            Establishment = est,
            ListSteps = list
          };
          salaryScaleService.Insert(item);

        }
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateGrade(Grade view)
    {
      try
      {
        gradeService.Update(view, null);
        UpdateAllGrade(view);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async void UpdateAllGrade(Grade grade)
    {
      try
      {
        foreach (var item in salaryScaleService.GetAll(p => p.Grade._id == grade._id))
        {
          item.Grade = grade;
          salaryScaleService.Update(item, null);
        }

        foreach (var item in occupationService.GetAll(p => p.Grade._id == grade._id))
        {
          item.Grade = grade;
          occupationService.Update(item, null);
          UpdateOccupationAll(item);
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveGrade(string id)
    {
      try
      {
        var item = gradeService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        gradeService.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Grade GetGrade(string id)
    {
      try
      {
        return gradeService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Grade> ListGrade(string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        try
        {
          int skip = (count * (page - 1));
          var detail = gradeService.GetAll(p => p.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = gradeService.GetAll(p => p.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

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

    public string UpdateStep(string idestablishment, string idgrade, EnumSteps step, decimal salary)
    {
      try
      {
        var view = salaryScaleService.GetAll(p => p.Grade._id == idgrade & p.Establishment._id == idestablishment).FirstOrDefault();
        foreach (var item in view.ListSteps)
        {
          if (item.Step == step)
          {
            item.Salary = salary;
            salaryScaleService.Update(view, null);
            return "update";
          }
        }

        return "step not found";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateOccupationAll(Occupation occupation)
    {
      try
      {
        foreach (var item in personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Occupation._id == occupation._id).ToList())
        {
          item.Occupation = occupation;
          personService.Update(item, null);
        }

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
  }
}
