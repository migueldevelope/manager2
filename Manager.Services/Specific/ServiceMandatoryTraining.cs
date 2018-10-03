using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Services.Specific
{
  public class ServiceMandatoryTraining : Repository<MandatoryTraining>, IServiceMandatoryTraining
  {
    private readonly ServiceGeneric<Company> companyService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<Occupation> occupationService;
    private readonly ServiceGeneric<MandatoryTraining> mandatoryTrainingService;
    private readonly ServiceGeneric<TrainingPlan> trainingPlanService;
    private readonly ServiceGeneric<EventHistoric> eventHistoricService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceMandatoryTraining(DataContext context)
      : base(context)
    {
      try
      {
        companyService = new ServiceGeneric<Company>(context);
        personService = new ServiceGeneric<Person>(context);
        occupationService = new ServiceGeneric<Occupation>(context);
        mandatoryTrainingService = new ServiceGeneric<MandatoryTraining>(context);
        trainingPlanService = new ServiceGeneric<TrainingPlan>(context);
        eventHistoricService = new ServiceGeneric<EventHistoric>(context);

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
      occupationService._user = _user;
      mandatoryTrainingService._user = _user;
      trainingPlanService._user = _user;
      eventHistoricService._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      companyService._user = baseUser;
      personService._user = baseUser;
      occupationService._user = baseUser;
      mandatoryTrainingService._user = _user;
      trainingPlanService._user = _user;
      eventHistoricService._user = _user;
    }

    public string AddOccuaption(ViewAddOccupationMandatory view)
    {
      try
      {
        var list = new List<OccupationMandatory>();
        list.Add(new OccupationMandatory()
        {
          Occupation = view.Occupation,
          BeginDate = view.BeginDate,
          TypeMandatoryTraining = view.TypeMandatoryTraining
        });
        var mandatory = mandatoryTrainingService.GetAll(p => p.Course == view.Course).FirstOrDefault();
        if (mandatory == null)
        {
          mandatoryTrainingService.Insert(new MandatoryTraining()
          {
            Course = view.Course,
            Occupations = list,
            Status = EnumStatus.Enabled,
            Companys = new List<CompanyMandatory>(),
            Persons = new List<PersonMandatory>()
          });
        }
        else
        {
          mandatory.Occupations.Add(list.FirstOrDefault());
          mandatoryTrainingService.Update(mandatory, null);
        }
        UpdateTrainingPlanOccupation(view.Course, view.Occupation, view.BeginDate, view.TypeMandatoryTraining);
        return "add occupation";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async void UpdateTrainingPlanPerson(Course course, Person person, DateTime? beginDate, EnumTypeMandatoryTraining typeMandatoryTraining)
    {
      try
      {
        var listPlans = trainingPlanService.GetAll(p => p.Course._id == course._id & p.Person._id == person._id).ToList();

        // VERITY DATE LAST COURSE REALIZED
        var realized = eventHistoricService.GetAll(p => p.Course._id == course._id & p.Person._id == person._id).ToList();
        DateTime? dateMax = null;
        if (realized.Count() > 0)
          dateMax = realized.Max(p => p.End);

        DateTime? proxDate = null;
        EnumStatusTrainingPlan status = EnumStatusTrainingPlan.Open;
        if (dateMax != null)
        {
          proxDate = dateMax.Value.AddMonths(course.Periodicity);
          status = EnumStatusTrainingPlan.Realized;
        }
        else
        {
          DateTime? maxHis = beginDate;
          if (person.DateAdm > maxHis)
            maxHis = person.DateAdm;

          if (person.DateLastOccupation > maxHis)
            maxHis = person.DateLastOccupation;

          proxDate = maxHis.Value.AddMonths(course.Periodicity);
        }

        if(listPlans.Where(p => p.Deadline == proxDate).Count() == 0)
        {
          trainingPlanService.Insert(new TrainingPlan()
          {
            Person = person,
            Course = course,
            Include = DateTime.Now,
            Observartion = string.Empty,
            Status = EnumStatus.Enabled,
            Origin = (typeMandatoryTraining == EnumTypeMandatoryTraining.Mandatory) ? EnumOrigin.Mandatory : EnumOrigin.Optional,
            StatusTrainingPlan = status,
            Deadline = proxDate.Value.AddMonths(course.Periodicity)
          });
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async void UpdateTrainingPlanOccupation(Course course, Occupation occupation, DateTime? beginDate, EnumTypeMandatoryTraining typeMandatoryTraining)
    {
      try
      {
        var list = personService.GetAll(p => p.Occupation._id == occupation._id).ToList();
        foreach (var item in list)
        {
          UpdateTrainingPlanPerson(course, item, beginDate, typeMandatoryTraining);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async void UpdateTrainingPlanCompany(Course course, Company company, DateTime? beginDate, EnumTypeMandatoryTraining typeMandatoryTraining)
    {
      try
      {
        var list = personService.GetAll(p => p.Company._id == company._id).ToList();
        foreach (var item in list)
        {
          UpdateTrainingPlanPerson(course, item, beginDate, typeMandatoryTraining);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddPerson(ViewAddPersonMandatory view)
    {
      try
      {
        var list = new List<PersonMandatory>();
        list.Add(new PersonMandatory()
        {
          Person = view.Person,
          BeginDate = view.BeginDate,
          TypeMandatoryTraining = view.TypeMandatoryTraining
        });
        var mandatory = mandatoryTrainingService.GetAll(p => p.Course == view.Course).FirstOrDefault();
        if (mandatory == null)
        {
          mandatoryTrainingService.Insert(new MandatoryTraining()
          {
            Course = view.Course,
            Occupations = new List<OccupationMandatory>(),
            Status = EnumStatus.Enabled,
            Companys = new List<CompanyMandatory>(),
            Persons = list
          });
        }
        else
        {
          mandatory.Persons.Add(list.FirstOrDefault());
          mandatoryTrainingService.Update(mandatory, null);
        }
        UpdateTrainingPlanPerson(view.Course, view.Person, view.BeginDate, view.TypeMandatoryTraining);

        return "add occupation";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddCompany(ViewAddCompanyMandatory view)
    {
      try
      {
        var list = new List<CompanyMandatory>();
        list.Add(new CompanyMandatory()
        {
          Company = view.Company,
          BeginDate = view.BeginDate,
          TypeMandatoryTraining = view.TypeMandatoryTraining
        });
        var mandatory = mandatoryTrainingService.GetAll(p => p.Course == view.Course).FirstOrDefault();
        if (mandatory == null)
        {
          mandatoryTrainingService.Insert(new MandatoryTraining()
          {
            Course = view.Course,
            Occupations = new List<OccupationMandatory>(),
            Status = EnumStatus.Enabled,
            Companys = list,
            Persons = new List<PersonMandatory>()
          });
        }
        else
        {
          mandatory.Companys.Add(list.FirstOrDefault());
          mandatoryTrainingService.Update(mandatory, null);
        }
        UpdateTrainingPlanCompany(view.Course, view.Company, view.BeginDate, view.TypeMandatoryTraining);
        return "add occupation";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveOccupation(string idcourse, string idoccupation)
    {
      throw new NotImplementedException();
    }

    public string RemovePerson(string idcourse, string idperson)
    {
      throw new NotImplementedException();
    }

    public string RemoveCompany(string idcourse, string idcompany)
    {
      throw new NotImplementedException();
    }

    public List<MandatoryTraining> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = mandatoryTrainingService.GetAll(p => p.Course.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Course.Name).Skip(skip).Take(count).ToList();
        total = mandatoryTrainingService.GetAll(p => p.Course.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewTrainingPlan(TrainingPlan view)
    {
      try
      {
        trainingPlanService.Insert(view);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateTrainingPlan(TrainingPlan view)
    {
      try
      {
        trainingPlanService.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveTrainingPlan(string id)
    {
      try
      {
        var item = trainingPlanService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        trainingPlanService.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public TrainingPlan GetTrainingPlan(string id)
    {
      try
      {
        return trainingPlanService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<TrainingPlan> ListTrainingPlan(string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        try
        {
          int skip = (count * (page - 1));
          var detail = trainingPlanService.GetAll(p => p.Person.Company._id == idcompany & p.Person.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.Name).Skip(skip).Take(count).ToList();
          total = trainingPlanService.GetAll(p => p.Person.Company._id == idcompany & p.Person.Name.ToUpper().Contains(filter.ToUpper())).Count();

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
