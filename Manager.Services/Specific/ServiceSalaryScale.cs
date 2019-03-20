using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
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
    public List<SalaryScale> List(string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = salaryScaleService.GetAll(p => p.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = salaryScaleService.GetAll(p => p.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<SalaryScaleGrade> ListGrades(string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var list = salaryScaleService.GetAll(p => p.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();

        var detail = new List<SalaryScaleGrade>();
        foreach (var item in list)
        {
          foreach (var grade in item.Grades)
          {
            var view = new SalaryScaleGrade();
            view._idSalaryScale = item._id;
            view.NameSalaryScale = item.Name;
            view._idGrade = grade._id;
            view.NameGrade = grade.Name;
            detail.Add(view);
          }
        }


        total = detail.Count();

        return detail.ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewSalaryScale(ViewNewSalaryScale view)
    {
      try
      {


        var item = new SalaryScale()
        {
          Name = view.Name,
          Company = view.Company,
          Grades = new List<Grade>()
        };
        salaryScaleService.Insert(item);

        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateSalaryScale(ViewUpdateSalaryScale view)
    {
      try
      {
        var item = salaryScaleService.GetAll(p => p._id == view._id).FirstOrDefault();
        item.Name = view.Name;
        salaryScaleService.Update(item, null);

        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddGrade(Grade view, string idsalaryscale)
    {
      try
      {
        var salaryScale = salaryScaleService.GetAll(p => p._id == idsalaryscale).FirstOrDefault();
        try
        {
          var order = salaryScale.Grades.Max(p => p.Order);
          view.Order = order + 1;
        }
        catch (Exception)
        {
          view.Order = 1;
        }


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

        view.ListSteps = list;
        var grade = gradeService.Insert(view);


        salaryScale.Grades.Add(grade);
        salaryScaleService.Update(salaryScale, null);

        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateGrade(Grade view, string idsalaryscale)
    {
      try
      {
        gradeService.Update(view, null);
        UpdateAllGrade(view, idsalaryscale);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async void UpdateAllGrade(Grade grade, string idsalaryscale)
    {
      try
      {
        var salaryScale = salaryScaleService.GetAll(p => p._id == idsalaryscale).FirstOrDefault();
        foreach (var item in salaryScale.Grades)
        {
          if (grade._id == item._id)
          {
            item.Name = grade.Name;
            item.Order = grade.Order;
            item.StepMedium = grade.StepMedium;
            item.ListSteps = grade.ListSteps;
            item.Status = grade.Status;
            salaryScaleService.Update(salaryScale, null);
          }

        }


        //foreach (var item in occupationService.GetAll(p => p.Grade._id == grade._id))
        //{
        //  item.Grade = grade;
        //  occupationService.Update(item, null);
        //  UpdateOccupationAll(item);
        //}

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveGrade(string id, string idsalaryscale)
    {
      try
      {
        var salaryscale = salaryScaleService.GetAll(p => p._id == idsalaryscale).FirstOrDefault();
        var item = gradeService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        gradeService.Update(item, null);

        foreach(var grad in salaryscale.Grades)
        {
          if(grad._id == item._id)
          {
            salaryscale.Grades.Remove(grad);
            salaryScaleService.Update(salaryscale, null);
            return "deleted";
          }
        }
        
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

    public List<Grade> ListGrade(string idsalaryscale, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        try
        {
          var salaryScale = salaryScaleService.GetAll(p => p._id == idsalaryscale).FirstOrDefault();
          return salaryScale.Grades.OrderBy(p => p.Order).ToList();
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

    public string UpdateStep(string idsalaryscale, string idgrade, EnumSteps step, decimal salary)
    {
      try
      {
        var view = salaryScaleService.GetAll(p => p._id == idsalaryscale).FirstOrDefault();
        var grade = view.Grades.Where(p => p._id == idgrade).FirstOrDefault();

        foreach (var item in grade.ListSteps)
        {
          if (item.Step == step)
          {
            item.Salary = salary;
            UpdateGrade(grade, idsalaryscale);
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
        foreach (var item in personService.GetAll(p => p.StatusUser != Manager.Views.Enumns.EnumStatusUser.Disabled & p.StatusUser != Manager.Views.Enumns.EnumStatusUser.ErrorIntegration & p.TypeUser != Manager.Views.Enumns.EnumTypeUser.Administrator & p.Occupation._id == occupation._id).ToList())
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
