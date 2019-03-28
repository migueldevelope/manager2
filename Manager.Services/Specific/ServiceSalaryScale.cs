using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
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
  public class ServiceSalaryScale : Repository<SalaryScale>, IServiceSalaryScale
  {
    private readonly ServiceGeneric<Company> serviceCompany;
    private readonly ServiceGeneric<Occupation> serviceOccupation;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<SalaryScale> serviceSalaryScale;

    #region Constructor
    public ServiceSalaryScale(DataContext context) : base(context)
    {
      try
      {
        serviceCompany = new ServiceGeneric<Company>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
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
      serviceOccupation._user = _user;
      servicePerson._user = _user;
      serviceSalaryScale._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      serviceCompany._user = user;
      servicePerson._user = user;
      serviceOccupation._user = user;
      serviceSalaryScale._user = user;
    }
    #endregion

    #region Salary Scale
    public List<ViewListSalaryScale> List(string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListSalaryScale> detail = serviceSalaryScale.GetAllNewVersion(p => p.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(x => new ViewListSalaryScale()
          {
            _id = x._id,
            Name = x.Name,
            Company = new ViewListCompany() { _id = x.Company._id, Name = x.Company.Name }
          }).ToList();
        total = serviceSalaryScale.CountNewVersion(p => p.Company._id == idcompany && p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudSalaryScale Get(string id)
    {
      try
      {
        SalaryScale item = serviceSalaryScale.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudSalaryScale()
        {
          Company = new ViewListCompany() { _id = item.Company._id, Name = item.Company.Name },
          Name = item.Name,
          _id = item._id
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string NewSalaryScale(ViewCrudSalaryScale view)
    {
      try
      {
        SalaryScale salaryScale = new SalaryScale()
        {
          Name = view.Name,
          Company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result,
          Grades = new List<Grade>()
        };
        salaryScale = serviceSalaryScale.InsertNewVersion(salaryScale).Result;
        return "Salary scale added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateSalaryScale(ViewCrudSalaryScale view)
    {
      try
      {
        SalaryScale salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == view._id).Result;
        salaryScale.Name = view.Name;
        serviceSalaryScale.Update(salaryScale, null);
        return "Salary scale altered!";
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
        SalaryScale salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == id).Result;
        salaryScale.Status = EnumStatus.Disabled;
        serviceSalaryScale.Update(salaryScale, null);
        return "Salary scale deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Grades
    public List<ViewListGrade> ListGrade(string idsalaryscale, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        SalaryScale item = serviceSalaryScale.GetNewVersion(p => p._id == idsalaryscale).Result;
        if (item == null)
          throw new Exception("Salary scale not found!");

        var detail = new List<ViewListGrade>();
        foreach (var grade in item.Grades)
        {
          var view = new ViewListGrade
          {
            _id = grade._id,
            Name = grade.Name,
            StepMedium = grade.StepMedium,
            Order = grade.Order,
            Steps = new List<ViewListStep>()
          };
          foreach (var step in grade.ListSteps)
          {
            var newStep = new ViewListStep()
            {
              Step = step.Step,
              Salary = step.Salary
            };
            view.Steps.Add(newStep);
          }
          detail.Add(view);
        }
        total = 1;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string AddGrade(ViewCrudGrade view)
    {
      try
      {
        SalaryScale salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == view.SalaryScale._id).Result;
        view.Order = 1;
        if (salaryScale.Grades.Count != 0)
          view.Order = salaryScale.Grades.Max(p => p.Order) + 1;

        Grade grade = new Grade
        {
          _id = ObjectId.GenerateNewId().ToString(),
          Name = view.Name,
          Order = view.Order,
          StepMedium = view.StepMedium,
          ListSteps = new List<ListSteps>()
        };
        for (var step = 0; step <= 7; step++)
        {
          grade.ListSteps.Add(new ListSteps()
          {
            Salary = 0,
            Step = (EnumSteps)step,
          });
        }
        salaryScale.Grades.Add(grade);
        // TODO: problema de persistência para array vazio
        serviceSalaryScale.Update(salaryScale, null);
        return "Grade added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateGrade(ViewCrudGrade view)
    {
      try
      {
        SalaryScale salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == view.SalaryScale._id).Result;
        var list = new List<Grade>();
        foreach (var grade in salaryScale.Grades)
        {
          if (grade._id == view._id)
          {
            grade.Name = view.Name;
            grade.Order = view.Order;
            grade.StepMedium = view.StepMedium;
          }
          list.Add(grade);
        }
        salaryScale.Grades = list;
        serviceSalaryScale.Update(salaryScale, null);
        return "Grade altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateGradePosition(string idsalaryscale, string idgrade, int position)
    {
      try
      {
        SalaryScale salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == idsalaryscale).Result;
        var list = new List<Grade>();
        foreach (var grade in salaryScale.Grades)
        {
          if (grade._id == idgrade)
          {
            grade.Order = position;
          }
          list.Add(grade);
        }
        salaryScale.Grades = list;
        serviceSalaryScale.Update(salaryScale, null);
        return "Grade altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveGrade(string idsalaryscale, string id)
    {
      try
      {
        SalaryScale salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == idsalaryscale).Result;
        var list = new List<Grade>();
        foreach (var grade in salaryScale.Grades)
          if (grade._id != id)
            list.Add(grade);
        salaryScale.Grades = list;
        serviceSalaryScale.Update(salaryScale, null);
        return "Grade deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudGrade GetGrade(string idsalaryscale, string id)
    {
      try
      {
        SalaryScale salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == idsalaryscale).Result;
        var result = new ViewCrudGrade();
        foreach (var grade in salaryScale.Grades)
          if (grade._id == id)
          {
            result = new ViewCrudGrade()
            {
              _id = grade._id,
              Name = grade.Name,
              Order = grade.Order,
              StepMedium = grade.StepMedium,
              SalaryScale = new ViewListSalaryScale() { _id = salaryScale._id, Name = salaryScale.Name, Company = new ViewListCompany() { _id = salaryScale.Company._id, Name = salaryScale.Company.Name } }
            };
            break;
          }
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateStep(ViewCrudStep view)
    {
      try
      {
        SalaryScale salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == view._idSalaryScale).Result;
        var list = new List<Grade>();
        foreach (var grade in salaryScale.Grades)
        {
          if (grade._id == view._idGrade)
          {
            var listStep = new List<ListSteps>();
            foreach (var item in grade.ListSteps)
            {
              if (item.Step == view.Step)
              {
                item.Salary = view.Salary;
              }
              listStep.Add(item);
            }
            grade.ListSteps = listStep;
          }
          list.Add(grade);
        }
        salaryScale.Grades = list;
        serviceSalaryScale.Update(salaryScale, null);
        return "Step altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListGradeFilter> ListGrades(string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        var salaryScale = serviceSalaryScale.GetAll(p => p.Company._id == idcompany).FirstOrDefault();
        if (salaryScale.Grades == null)
          return null;


        return salaryScale.Grades.Select(p => new ViewListGradeFilter()
        {
          idGrade = p._id,
          NameGrade = p.Name,
          idSalaryScale = salaryScale._id,
          NameSalaryScale = salaryScale.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion
  }
}
