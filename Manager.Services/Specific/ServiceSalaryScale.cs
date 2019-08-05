using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
      _user = user;
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
    public string New(ViewCrudSalaryScale view)
    {
      try
      {
        SalaryScale salaryScale = new SalaryScale()
        {
          Name = view.Name,
          Company = view.Company,
          Grades = new List<Grade>()
        };
        serviceSalaryScale.InsertNewVersion(salaryScale).Wait();
        return "Salary scale added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudSalaryScale view)
    {
      try
      {
        SalaryScale salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == view._id).Result;
        salaryScale.Name = view.Name;
        serviceSalaryScale.Update(salaryScale, null).Wait();
        return "Salary scale altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Delete(string id)
    {
      try
      {
        SalaryScale salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == id).Result;
        salaryScale.Status = EnumStatus.Disabled;
        serviceSalaryScale.Update(salaryScale, null).Wait();
        return "Salary scale deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Grades

    public string AddOccupationSalaryScale(ViewCrudOccupationSalaryScale view)
    {
      try
      {
        var occupation = serviceOccupation.GetNewVersion(p => p._id == view._idOccupation).Result;
        if (occupation.SalaryScales == null)
          occupation.SalaryScales = new List<SalaryScaleGrade>();

        occupation.SalaryScales.Add(new SalaryScaleGrade()
        {
          _idGrade = view._idGrade,
          _idSalaryScale = view._idSalaryScale,
          Workload = view.Workload,
          _id = ObjectId.GenerateNewId().ToString(),
          NameGrade = view.NameGrade,
          NameSalaryScale = view.NameSalaryScale
        });
        var result = serviceOccupation.Update(occupation, null);
        return "add";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveOccupationSalaryScale(string idoccupation, string idgrade)
    {
      try
      {
        var occupation = serviceOccupation.GetNewVersion(p => p._id == idoccupation).Result;

        foreach (var item in occupation.SalaryScales)
        {
          if (item._idGrade == idgrade)
          {
            occupation.SalaryScales.Remove(item);
            var result = serviceOccupation.Update(occupation, null);
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

    public List<ViewListGrade> ListGrade(string idsalaryscale, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        SalaryScale item = serviceSalaryScale.GetNewVersion(p => p._id == idsalaryscale).Result;
        var occupations = serviceOccupation.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;

        if (item == null)
          throw new Exception("Salary scale not found!");

        var detail = new List<ViewListGrade>();
        foreach (var grade in item.Grades)
        {
          var occupation = new List<ViewListOccupationSalaryScale>();
          foreach (var occ in occupations)
          {
            if (occ.SalaryScales != null)
            {
              if (occ.SalaryScales.Where(p => p._idGrade == grade._id).Count() > 0)
              {
                var occupationStep = new ViewListOccupationSalaryScale() { _id = occ._id, Name = occ.Name, Wordload = occ.SalaryScales.FirstOrDefault().Workload };
                foreach (var step in grade.ListSteps)
                {
                  var newStep = new ViewListStep()
                  {
                    Step = step.Step,
                    Salary = step.Salary
                  };
                  if (occupationStep.Wordload != grade.Workload)
                  {
                    newStep.Salary = Math.Round((step.Salary * occupationStep.Wordload) / grade.Workload, 2);
                  }
                  occupationStep.Steps.Add(newStep);
                }
                occupation.Add(occupationStep);
              }
            }
          }
          var view = new ViewListGrade
          {
            _id = grade._id,
            Name = grade.Name,
            StepMedium = grade.StepMedium,
            Order = grade.Order,
            Wordload = grade.Workload,
            Steps = new List<ViewListStep>(),
            Occupation = occupation,
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
        total = detail.Count();
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
          Workload = view.Workload,
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
        serviceSalaryScale.Update(salaryScale, null).Wait();
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
            grade.Workload = view.Workload;
          }
          list.Add(grade);
        }
        salaryScale.Grades = list;
        serviceSalaryScale.Update(salaryScale, null).Wait();
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
        serviceSalaryScale.Update(salaryScale, null).Wait();
        return "Grade altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteGrade(string idsalaryscale, string id)
    {
      try
      {
        SalaryScale salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == idsalaryscale).Result;
        var list = new List<Grade>();
        foreach (var grade in salaryScale.Grades)
          if (grade._id != id)
            list.Add(grade);
        salaryScale.Grades = list;
        serviceSalaryScale.Update(salaryScale, null).Wait();
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
        if (salaryScale == null)
          throw new Exception("Salary scale not available!");
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
              Workload = grade.Workload,
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
        serviceSalaryScale.Update(salaryScale, null).Wait();
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
        var list = serviceSalaryScale.GetAllNewVersion(p => p.Company._id == idcompany).Result;
        var result = new List<ViewListGradeFilter>();
        if (list == null)
          return new List<ViewListGradeFilter>();

        foreach (var salaryScale in list)
        {
          if (salaryScale.Grades != null)
          {
            foreach (var grade in salaryScale.Grades)
            {
              result.Add(new ViewListGradeFilter()
              {
                idGrade = grade._id,
                NameGrade = grade.Name,
                idSalaryScale = salaryScale._id,
                NameSalaryScale = salaryScale.Name
              });
            }
          }
        }

        total = result.Count();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ImportSalaryScale(string idsalaryscale, Stream stream)
    {
      try
      {
        var serviceExcel = new ServiceExcel();
        var tuple = serviceExcel.ImportSalaryScale(stream);
        var import = tuple.Item1;
        var gradename = tuple.Item2;
        var workload = tuple.Item4;
        var salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == idsalaryscale).Result;

        salaryScale.Grades = new List<Grade>();

        for (int row = 0; row < tuple.Item3; row++)
        {
          if (import[row][0].ToString() != string.Empty)
          {
            var grade = new Grade();
            grade.Order = row + 1;
            grade._id = ObjectId.GenerateNewId().ToString();
            grade.Name = gradename[row].ToString();
            grade.ListSteps = new List<ListSteps>();
            grade.Workload = workload[row];

            for (int col = 0; col < 8; col++)
            {
              if (import[row][col].ToString() != string.Empty)
              {
                var step = new ListSteps();
                step.Step = (EnumSteps)col;
                step.Salary = Math.Round(decimal.Parse(import[row][col].ToString()), 2);
                grade.ListSteps.Add(step);
              }
            }

            salaryScale.Grades.Add(grade);
          }
        }
        var scale = serviceSalaryScale.Update(salaryScale, null);

        return "import_ok";
      }
      catch (Exception e)
      {
        if (e.Message == "not_numeric")
          return e.Message;
        else if (e.Message == "not_numeric_workload")
          return e.Message;
        else
          throw e;
      }
    }

    #endregion

  }
}
