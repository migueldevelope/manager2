
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
    private readonly ServiceGeneric<SalaryScaleLog> serviceSalaryScaleLog;

    #region Constructor
    public ServiceSalaryScale(DataContext context) : base(context)
    {
      try
      {
        serviceCompany = new ServiceGeneric<Company>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        serviceSalaryScale = new ServiceGeneric<SalaryScale>(context);
        serviceSalaryScaleLog = new ServiceGeneric<SalaryScaleLog>(context);
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
      serviceSalaryScaleLog._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceCompany._user = user;
      servicePerson._user = user;
      serviceOccupation._user = user;
      serviceSalaryScale._user = user;
      serviceSalaryScaleLog._user = user;
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

    public string NewVersion(string idsalaryscale)
    {
      try
      {
        var old = serviceSalaryScale.GetNewVersion(p => p._id == idsalaryscale).Result;

        var salaryScale = new SalaryScaleLog()
        {
          Name = old.Name,
          Company = old.Company,
          Grades = null,
          _idSalaryScalePrevious = old._id,
          Date = DateTime.Now
        };

        var occupations = serviceOccupation.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        var detail = new List<ViewListGrade>();
        foreach (var grade in old.Grades)
        {
          var occupation = new List<ViewListOccupationSalaryScale>();
          foreach (var occ in occupations)
          {
            if (occ.SalaryScales != null)
            {
              if (occ.SalaryScales.Where(p => p._idGrade == grade._id).Count() > 0)
              {
                var occupationStep = new ViewListOccupationSalaryScale()
                {
                  _id = occ._id,
                  Name = occ.Name,
                  Description = occ.Description,
                  Wordload = occ.SalaryScales.FirstOrDefault().Workload,
                  Process = occ.Process == null ? null : occ.Process.Select(
                  x => new ViewListProcessLevelTwo()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Order = x.Order,
                    ProcessLevelOne = x.ProcessLevelOne
                  }).ToList()
                };
                occupationStep.Steps = new List<ViewListStep>();
                foreach (var step in grade.ListSteps)
                {
                  var newStep = new ViewListStep()
                  {
                    Step = step.Step,
                    Salary = step.Salary
                  };
                  if (occupationStep.Wordload != grade.Workload)
                  {
                    newStep.Salary = Math.Round((step.Salary * occupationStep.Wordload) / (grade.Workload == 0 ? 1 : grade.Workload), 2);
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
        salaryScale.Grades = detail;
        serviceSalaryScaleLog.InsertNewVersion(salaryScale).Wait();
        return "Salary scale added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RestoreVersion(string idsalaryscale)
    {
      try
      {
        var salaryscale = serviceSalaryScale.GetNewVersion(p => p._id == idsalaryscale).Result;
        var salaryscaleLog = serviceSalaryScaleLog.GetAllNewVersion(p => p._idSalaryScalePrevious == idsalaryscale).Result.LastOrDefault();


        foreach (var item in salaryscale.Grades)
        {
          foreach (var step in item.ListSteps)
          {
            var grade = salaryscaleLog.Grades.Where(p => p._id == item._id).FirstOrDefault();
            if (grade != null)
            {
              decimal salary = grade.Steps.Where(p => p.Step == step.Step).FirstOrDefault().Salary;
              step.Salary = salary;
            }

          }
        }
        var i = serviceSalaryScale.Update(salaryscale, null);

        return "Salary scale restore!";
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

    public List<ViewListSalaryScaleLog> ListSalaryScaleLog(string idsalaryscale, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListSalaryScaleLog> detail = serviceSalaryScaleLog.GetAllNewVersion(p => p._idSalaryScalePrevious == idsalaryscale & p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(x => new ViewListSalaryScaleLog()
          {
            _id = x._id,
            Name = x.Name,
            Company = new ViewListCompany() { _id = x.Company._id, Name = x.Company.Name },
            Date = x.Date
          }).ToList();
        total = serviceSalaryScaleLog.CountNewVersion(p => p._idSalaryScalePrevious == idsalaryscale && p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail.OrderByDescending(p => p.Date).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudSalaryScaleLog GetSalaryScaleLog(string id)
    {
      try
      {
        SalaryScaleLog item = serviceSalaryScaleLog.GetNewVersion(p => p._id == id).Result;
        var view = new ViewCrudSalaryScaleLog()
        {
          Company = new ViewListCompany() { _id = item.Company._id, Name = item.Company.Name },
          Name = item.Name,
          _id = item._id,
          Date = item.Date,
          Grades = item.Grades,
          _idSalaryScalePrevious = item._idSalaryScalePrevious
        };

        foreach (var grade in view.Grades)
        {
          foreach (var occ in grade.Occupation)
          {
            foreach (var step in occ.Steps)
            {
              if ((occ.StepLimit != EnumSteps.Default) && (step.Step > occ.StepLimit))
                step.Salary = 0;
            }
          }

        }
        return view;
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
          NameSalaryScale = view.NameSalaryScale,
          StepLimit = view.StepLimit
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
                var occupationStep = new ViewListOccupationSalaryScale()
                {
                  _id = occ._id,
                  Name = occ.Name,
                  Description = occ.Description,
                  Wordload = occ.SalaryScales.Where(p => p._idSalaryScale == idsalaryscale).FirstOrDefault().Workload,
                  StepLimit = occ.SalaryScales.Where(p => p._idSalaryScale == idsalaryscale).FirstOrDefault().StepLimit,
                  Process = occ.Process == null ? null : occ.Process.Select(
                  x => new ViewListProcessLevelTwo()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Order = x.Order,
                    ProcessLevelOne = x.ProcessLevelOne
                  }).ToList()
                };
                occupationStep.Steps = new List<ViewListStep>();

                foreach (var step in grade.ListSteps)
                {
                  var newStep = new ViewListStep()
                  {
                    Step = step.Step,
                    Salary = Math.Round(step.Salary, 2)
                  };
                  if (occupationStep.Wordload != grade.Workload)
                  {
                    newStep.Salary = Math.Round((step.Salary * occupationStep.Wordload) / (grade.Workload == 0 ? 1 : grade.Workload), 2);
                  }
                  if ((occupationStep.StepLimit != EnumSteps.E) && (step.Step > occupationStep.StepLimit))
                    newStep.Salary = 0;

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
              Salary = Math.Round(step.Salary, 2)
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

    public List<ViewListGrade> ListGradeManager(string idmanager, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        var listidocc = servicePerson.GetAllNewVersion(p => p.Manager._id == idmanager).Result.Select(p =>
        p.Occupation?._id).ToList();

        var occupations = serviceOccupation.GetAllNewVersion(p => listidocc.Contains(p._id)).Result;

        var detail = new List<ViewListGrade>();
        var occupation = new List<ViewListOccupationSalaryScale>();
        foreach (var occ in occupations)
        {

          if (occ.SalaryScales != null)
          {
            var occOri = serviceOccupation.GetNewVersion(p => p._id == occ._id).Result.SalaryScales?.Select(p => p._idGrade).ToList();

            foreach (var sal in occ.SalaryScales)
            {
              SalaryScale item = serviceSalaryScale.GetNewVersion(p => p._id == sal._idSalaryScale).Result;
              if (item == null)
                throw new Exception("Salary scale not found!");

              var occupationStep = new ViewListOccupationSalaryScale()
              {
                _id = occ._id,
                Name = occ.Name,
                Description = occ.Description,
                Wordload = occ.SalaryScales.FirstOrDefault().Workload,
                Process = occ.Process == null ? null : occ.Process.Select(
                x => new ViewListProcessLevelTwo()
                {
                  _id = x._id,
                  Name = x.Name,
                  Order = x.Order,
                  ProcessLevelOne = x.ProcessLevelOne
                }).ToList()
              };

              occupationStep.Steps = new List<ViewListStep>();

              foreach (var grade in item.Grades)
              {
                if (occOri.Contains(grade._id) == true)
                {
                  if (occupationStep.Steps.Count == 0)
                  {
                    foreach (var step in grade.ListSteps)
                    {
                      var newStep = new ViewListStep()
                      {
                        Step = step.Step,
                        Salary = step.Salary
                      };
                      if (occupationStep.Wordload != grade.Workload)
                      {
                        newStep.Salary = Math.Round((step.Salary * occupationStep.Wordload) / (grade.Workload == 0 ? 1 : grade.Workload), 2);
                      }
                      occupationStep.Steps.Add(newStep);
                    }

                  }

                  if (occupation.Count == 0)
                  {
                    occupation.Add(occupationStep);
                  }
                  else
                  {
                    occupation = new List<ViewListOccupationSalaryScale>();
                    occupation.Add(occupationStep);
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

                  detail.Add(view);
                }


              }

            }
          }

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

    public string UpdateSteps(string id, decimal percent)
    {
      try
      {
        Task.Run(() => NewVersion(id));
        SalaryScale salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == id).Result;
        foreach (var item in salaryScale.Grades)
        {
          foreach (var step in item.ListSteps)
          {
            step.Salary = step.Salary + (step.Salary * (percent / 100));
          }
        }

        serviceSalaryScale.Update(salaryScale, null).Wait();
        return "Salary scale altered!";
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

    public string ImportUpdateSalaryScale(string idsalaryscale, Stream stream)
    {
      try
      {
        var serviceExcel = new ServiceExcel();
        var tuple = serviceExcel.ImportSalaryScale(stream);
        var import = tuple.Item1;
        var gradename = tuple.Item2;
        var workload = tuple.Item4;
        var salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == idsalaryscale).Result;

        var gradesnew = new List<Grade>();

        //salaryScale.Grades = new List<Grade>();

        for (int row = 0; row < tuple.Item3; row++)
        {
          if (import[row][0].ToString() != string.Empty)
          {
            var grade = salaryScale.Grades.Where(p => p.Name == gradename[row].ToString()).FirstOrDefault();
            //grade.Order = row + 1;
            //grade._id = ObjectId.GenerateNewId().ToString();
            //grade.Name = gradename[row].ToString();
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

            //salaryScale.Grades.Add(grade);
            gradesnew.Add(grade);
          }
        }
        salaryScale.Grades = gradesnew;

        var scale = serviceSalaryScale.Update(salaryScale, null);
        Task.Run(() => NewVersion(idsalaryscale));

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

    public string ExportSalaryScaleLog(string idsalaryscale)
    {
      try
      {
        var serviceExcel = new ServiceExcel();
        var occupations = serviceOccupation.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;

        var salaryScale = serviceSalaryScaleLog.GetNewVersion(p => p._id == idsalaryscale).Result;
        if (salaryScale.Grades.Count > 0)
        {
          //long count = salaryScale.Grades.Count;
          long count = (occupations.Count + salaryScale.Grades.Count);
          string[] occupationsname = new string[count];
          string[] descriptionname = new string[count];
          string[] grades = new string[count];
          string[] groups = new string[count];
          string[] spheres = new string[count];
          int[] workloads = new int[count];
          double[][] matriz = new double[count][];
          for (int i = 0; i < count; i++)
            matriz[i] = new double[9];

          int row = 0;

          //com grades vazios
          //foreach (var item in salaryScale.Grades)
          //{
          //    grades[row] = item.Name;
          //    long countgrade = 0;

          //    foreach (var step in item.ListSteps)
          //    {
          //        matriz[row][(byte)step.Step] = double.Parse(step.Salary.ToString());
          //    }

          //    workloads[row] = item.Workload;
          //    descriptionname[row] = "";
          //    groups[row] = "";
          //    spheres[row] = "";

          //    foreach (var occ in occupations)
          //    {
          //        if (occ.SalaryScales != null)
          //        {
          //            if (occ.SalaryScales.Where(p => p._idGrade == item._id).Count() > 0)
          //            {
          //                grades[row] = item.Name;
          //                occupationsname[row] = occ.Name;
          //                descriptionname[row] = occ.Description;
          //                groups[row] = occ.Group?.Name;
          //                spheres[row] = occ.Group.Sphere.Name;
          //                var salaryscale = occ.SalaryScales.Where(p => p._idSalaryScale == idsalaryscale).FirstOrDefault();
          //                if (salaryscale != null)
          //                    workloads[row] = salaryscale.Workload;

          //                foreach (var step in item.ListSteps)
          //                {

          //                    if (salaryscale.Workload != item.Workload)
          //                        matriz[row][(byte)step.Step] = double.Parse(Math.Round((step.Salary * occ.SalaryScales.FirstOrDefault().Workload) / (item.Workload == 0 ? 1 : item.Workload), 2).ToString());
          //                    else
          //                        matriz[row][(byte)step.Step] = double.Parse(step.Salary.ToString());
          //                }
          //                row += 1;
          //                countgrade += 1;
          //            }
          //        }

          //    }
          //    if (countgrade == 0)
          //        row += 1;
          //}



          foreach (var item in salaryScale.Grades)
          {
            long countgrade = 0;

            //grades[row] = item.Name;
            //foreach (var step in item.ListSteps)
            //{
            //    matriz[row][(byte)step.Step] = double.Parse(step.Salary.ToString());
            //}

            //workloads[row] = item.Workload;
            //descriptionname[row] = "";
            //groups[row] = "";
            //spheres[row] = "";

            foreach (var occ in occupations)
            {
              if (occ.SalaryScales != null)
              {
                if (occ.SalaryScales.Where(p => p._idGrade == item._id).Count() > 0)
                {
                  grades[row] = item.Name;
                  occupationsname[row] = occ.Name;
                  descriptionname[row] = occ.Description;
                  groups[row] = occ.Group?.Name;
                  spheres[row] = occ.Group.Sphere.Name;
                  var salaryscale = occ.SalaryScales.Where(p => p._idSalaryScale == salaryScale._idSalaryScalePrevious).FirstOrDefault();
                  if (salaryscale != null)
                    workloads[row] = salaryscale.Workload;

                  foreach (var step in item.Steps)
                  {

                    if (salaryscale.Workload != item.Wordload)
                      matriz[row][(byte)step.Step] = double.Parse(Math.Round((step.Salary * occ.SalaryScales.Where(p => p._idSalaryScale == salaryScale._idSalaryScalePrevious).FirstOrDefault().Workload) / (item.Wordload == 0 ? 1 : item.Wordload), 2).ToString());
                    else
                      matriz[row][(byte)step.Step] = double.Parse(step.Salary.ToString());

                    if ((occ.SalaryScales.FirstOrDefault().StepLimit != EnumSteps.Default) && (step.Step > occ.SalaryScales.Where(p => p._idSalaryScale == salaryScale._idSalaryScalePrevious).FirstOrDefault().StepLimit))
                      matriz[row][(byte)step.Step] = 0;
                  }
                  row += 1;
                  countgrade += 1;
                }
              }

            }

            //if (countgrade == 0)
            //    row += 1;
          }
          var salaryScaleLog = serviceSalaryScaleLog.GetAllNewVersion(p => p._id == idsalaryscale).Result.LastOrDefault();

          var view = new
          {
            Company = salaryScale.Company.Name,
            Name = salaryScale.Name,
            Version = salaryScaleLog == null ? DateTime.Now.ToString("dd/MM/yyyy HH:mm") : salaryScaleLog.Date.Value.AddHours(-3).ToString("dd/MM/yyyy HH:mm"),
            Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
          };

          var export = serviceExcel.ExportSalaryScale(new Tuple<double[][], string[], string[], string[], string[], int[], long>(matriz, occupationsname, grades, groups, spheres, workloads, row + 1), descriptionname, view);
          return export;
        }

        return "";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ExportSalaryScale(string idsalaryscale)
    {
      try
      {
        var serviceExcel = new ServiceExcel();
        var occupations = serviceOccupation.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;

        var salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == idsalaryscale).Result;
        if (salaryScale.Grades.Count > 0)
        {
          //long count = salaryScale.Grades.Count;
          long count = (occupations.Count + salaryScale.Grades.Count);
          string[] occupationsname = new string[count];
          string[] descriptionname = new string[count];
          string[] grades = new string[count];
          string[] groups = new string[count];
          string[] spheres = new string[count];
          int[] workloads = new int[count];
          double[][] matriz = new double[count][];
          for (int i = 0; i < count; i++)
            matriz[i] = new double[9];

          int row = 0;

          //com grades vazios
          //foreach (var item in salaryScale.Grades)
          //{
          //    grades[row] = item.Name;
          //    long countgrade = 0;

          //    foreach (var step in item.ListSteps)
          //    {
          //        matriz[row][(byte)step.Step] = double.Parse(step.Salary.ToString());
          //    }

          //    workloads[row] = item.Workload;
          //    descriptionname[row] = "";
          //    groups[row] = "";
          //    spheres[row] = "";

          //    foreach (var occ in occupations)
          //    {
          //        if (occ.SalaryScales != null)
          //        {
          //            if (occ.SalaryScales.Where(p => p._idGrade == item._id).Count() > 0)
          //            {
          //                grades[row] = item.Name;
          //                occupationsname[row] = occ.Name;
          //                descriptionname[row] = occ.Description;
          //                groups[row] = occ.Group?.Name;
          //                spheres[row] = occ.Group.Sphere.Name;
          //                var salaryscale = occ.SalaryScales.Where(p => p._idSalaryScale == idsalaryscale).FirstOrDefault();
          //                if (salaryscale != null)
          //                    workloads[row] = salaryscale.Workload;

          //                foreach (var step in item.ListSteps)
          //                {

          //                    if (salaryscale.Workload != item.Workload)
          //                        matriz[row][(byte)step.Step] = double.Parse(Math.Round((step.Salary * occ.SalaryScales.FirstOrDefault().Workload) / (item.Workload == 0 ? 1 : item.Workload), 2).ToString());
          //                    else
          //                        matriz[row][(byte)step.Step] = double.Parse(step.Salary.ToString());
          //                }
          //                row += 1;
          //                countgrade += 1;
          //            }
          //        }

          //    }
          //    if (countgrade == 0)
          //        row += 1;
          //}



          foreach (var item in salaryScale.Grades)
          {
            long countgrade = 0;

            //grades[row] = item.Name;
            //foreach (var step in item.ListSteps)
            //{
            //    matriz[row][(byte)step.Step] = double.Parse(step.Salary.ToString());
            //}

            //workloads[row] = item.Workload;
            //descriptionname[row] = "";
            //groups[row] = "";
            //spheres[row] = "";

            foreach (var occ in occupations)
            {
              if (occ.SalaryScales != null)
              {
                if (occ.SalaryScales.Where(p => p._idGrade == item._id).Count() > 0)
                {
                  grades[row] = item.Name;
                  occupationsname[row] = occ.Name;
                  descriptionname[row] = occ.Description;
                  groups[row] = occ.Group?.Name;
                  spheres[row] = occ.Group.Sphere.Name;
                  var salaryscale = occ.SalaryScales.Where(p => p._idSalaryScale == idsalaryscale).FirstOrDefault();
                  if (salaryscale != null)
                    workloads[row] = salaryscale.Workload;

                  foreach (var step in item.ListSteps)
                  {

                    if (salaryscale.Workload != item.Workload)
                      matriz[row][(byte)step.Step] = double.Parse(Math.Round((step.Salary * occ.SalaryScales.Where(p => p._idSalaryScale == idsalaryscale).FirstOrDefault().Workload) / (item.Workload == 0 ? 1 : item.Workload), 2).ToString());
                    else
                      matriz[row][(byte)step.Step] = double.Parse(step.Salary.ToString());

                    if ((occ.SalaryScales.Where(p => p._idSalaryScale == idsalaryscale).FirstOrDefault().StepLimit != EnumSteps.Default) && (step.Step > occ.SalaryScales.Where(p => p._idSalaryScale == idsalaryscale).FirstOrDefault().StepLimit))
                      matriz[row][(byte)step.Step] = 0;
                  }
                  row += 1;
                  countgrade += 1;
                }
              }

            }

            //if (countgrade == 0)
            //    row += 1;
          }
          var salaryScaleLog = serviceSalaryScaleLog.GetAllNewVersion(p => p._idSalaryScalePrevious == idsalaryscale).Result.LastOrDefault();

          var view = new
          {
            Company = salaryScale.Company.Name,
            Name = salaryScale.Name,
            Version = salaryScaleLog == null ? DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") : salaryScaleLog.Date.Value.ToString("dd/MM/yyyy hh:mm:ss"),
            Date = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")
          };

          var export = serviceExcel.ExportSalaryScale(new Tuple<double[][], string[], string[], string[], string[], int[], long>(matriz, occupationsname, grades, groups, spheres, workloads, row + 1), descriptionname, view);
          return export;
        }

        return "";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ExportUpdateSalaryScale(string idsalaryscale)
    {
      try
      {
        var serviceExcel = new ServiceExcel();


        var salaryScale = serviceSalaryScale.GetNewVersion(p => p._id == idsalaryscale).Result;
        if (salaryScale.Grades.Count > 0)
        {
          //long count = salaryScale.Grades.Count;
          long count = (salaryScale.Grades.Count);
          string[] grades = new string[count];
          int[] workloads = new int[count];
          double[][] matriz = new double[count][];
          for (int i = 0; i < count; i++)
            matriz[i] = new double[9];

          int row = 0;


          foreach (var item in salaryScale.Grades)
          {
            grades[row] = item.Name;
            workloads[row] = item.Workload;
            grades[row] = item.Name;
            foreach (var step in item.ListSteps)
            {
              matriz[row][(byte)step.Step] = double.Parse(step.Salary.ToString());
            }

            row += 1;
          }

          var export = serviceExcel.ExportUpdateSalaryScale(new Tuple<double[][], string[], int[], long>(matriz, grades, workloads, row + 1));
          return export;
        }

        return "";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

  }
}
