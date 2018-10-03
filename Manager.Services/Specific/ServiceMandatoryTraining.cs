﻿using Manager.Core.Base;
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
    private readonly ServiceGeneric<CompanyMandatory> companyMandatoryService;
    private readonly ServiceGeneric<PersonMandatory> personMandatoryService;
    private readonly ServiceGeneric<OccupationMandatory> occupationMandatoryService;
    private readonly ServiceGeneric<MandatoryTraining> mandatoryTrainingService;
    private readonly ServiceGeneric<TrainingPlan> trainingPlanService;
    private readonly ServiceGeneric<EventHistoric> eventHistoricService;
    private readonly ServiceGeneric<Course> courseService;

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
        companyMandatoryService = new ServiceGeneric<CompanyMandatory>(context);
        personMandatoryService = new ServiceGeneric<PersonMandatory>(context);
        occupationMandatoryService = new ServiceGeneric<OccupationMandatory>(context);
        courseService = new ServiceGeneric<Course>(context);
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
      companyMandatoryService._user = _user;
      personMandatoryService._user = _user;
      occupationMandatoryService._user = _user;
      courseService._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      companyService._user = baseUser;
      personService._user = baseUser;
      occupationService._user = baseUser;
      mandatoryTrainingService._user = _user;
      trainingPlanService._user = _user;
      eventHistoricService._user = _user;
      companyMandatoryService._user = _user;
      personMandatoryService._user = _user;
      occupationMandatoryService._user = _user;
      courseService._user = _user;
    }

    public string AddOccuaption(ViewAddOccupationMandatory view)
    {
      try
      {
        var list = new List<OccupationMandatory>();
        list.Add(AddOccupationMandatory(new OccupationMandatory()
        {
          Occupation = view.Occupation,
          BeginDate = view.BeginDate,
          TypeMandatoryTraining = view.TypeMandatoryTraining
        }));
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

    private void newOnZero(Course course)
    {
      try
      {
        var on = trainingPlanService.GetAuthentication(p => p.Status == EnumStatus.Disabled).Count();
        if (on == 0)
        {
          var person = personService.GetAll().FirstOrDefault();
          var zero = trainingPlanService.Insert(new TrainingPlan() { Person = person, Status = EnumStatus.Disabled, Course = course });
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public async void UpdateTrainingPlanPerson(Course course, Person person, DateTime? beginDate, EnumTypeMandatoryTraining typeMandatoryTraining)
    {
      try
      {
        if (beginDate == null)
          beginDate = DateTime.Now;

        newOnZero(course);
        var listPlans = trainingPlanService.GetAll(p => p.Course._id == course._id & p.Person._id == person._id).ToList();

        // VERITY DATE LAST COURSE REALIZED
        var realized = eventHistoricService.GetAll(p => p.Course._id == course._id & p.Person._id == person._id).ToList();

        DateTime? dateMax = null;
        if (realized.Count() > 0)
          dateMax = realized.Max(p => p.End);

        if ((dateMax != null) & (Prerequeriments(course._id) == true))
          return;

        if ((dateMax != null) & (course.Periodicity == 0))
          return;

        if (PrerequerimentsInvert(course, person._id) == true)
          return;


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

        if (listPlans.Where(p => p.Deadline == proxDate).Count() == 0)
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
            Deadline = proxDate.Value.AddMonths(course.Deadline)
          });
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private bool Prerequeriments(string idcourse)
    {
      try
      {
        foreach (var item in courseService.GetAll().ToList())
        {
          if (item.Prerequisites.Where(x => x._id == idcourse).Count() > 0)
            return true;
        }

        return false;
      }
      catch (Exception e)
      {
        {
          throw e;
        }
      }
    }

    private bool PrerequerimentsInvert(Course course, string idperson)
    {
      try
      {
        foreach (var item in course.Prerequisites)
        {
          if (eventHistoricService.GetAll(p => p.Course._id == course._id & p.Person._id == idperson).Count() == 0)
            return true;
        }

        return false;
      }
      catch (Exception e)
      {
        {
          throw e;
        }
      }
    }

    public async void UpdateTrainingPlanOccupation(Course course, Occupation occupation, DateTime? beginDate, EnumTypeMandatoryTraining typeMandatoryTraining)
    {
      try
      {
        if (beginDate == null)
          beginDate = DateTime.Now;

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
        if (beginDate == null)
          beginDate = DateTime.Now;

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
        list.Add(AddPersonMandatory(new PersonMandatory()
        {
          Person = view.Person,
          BeginDate = view.BeginDate,
          TypeMandatoryTraining = view.TypeMandatoryTraining
        }));
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
        list.Add(AddCompanyMandatory(new CompanyMandatory()
        {
          Company = view.Company,
          BeginDate = view.BeginDate,
          TypeMandatoryTraining = view.TypeMandatoryTraining
        }));
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
      try
      {
        var mandatory = mandatoryTrainingService.GetAll(p => p.Course._id == idcourse).FirstOrDefault();
        foreach (var item in mandatory.Occupations)
        {
          if (item.Occupation._id == idoccupation)
          {
            mandatory.Occupations.Remove(item);
            mandatoryTrainingService.Update(mandatory, null);
            RemoveOccupationPlan(idoccupation, idcourse);
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

    public string RemovePerson(string idcourse, string idperson)
    {
      try
      {
        var mandatory = mandatoryTrainingService.GetAll(p => p.Course._id == idcourse).FirstOrDefault();
        foreach (var item in mandatory.Persons)
        {
          if (item.Person._id == idperson)
          {
            mandatory.Persons.Remove(item);
            mandatoryTrainingService.Update(mandatory, null);
            RemovePersonPlan(idperson, idcourse);
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

    public async void RemovePersonPlan(string idperson, string idcourse)
    {
      try
      {
        var plan = trainingPlanService.GetAll(p => p.Person._id == idperson & p.Course._id == idcourse
        & p.StatusTrainingPlan == EnumStatusTrainingPlan.Open).FirstOrDefault();
        plan.Status = EnumStatus.Disabled;
        trainingPlanService.Update(plan, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async void RemoveOccupationPlan(string idoccoupation, string idcourse)
    {
      try
      {
        foreach (var item in personService.GetAll(p => p.Occupation._id == idoccoupation).ToList())
        {
          RemovePersonPlan(item._id, idcourse);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async void RemoveCompanyPlan(string idcompany, string idcourse)
    {
      try
      {
        foreach (var item in personService.GetAll(p => p.Company._id == idcompany).ToList())
        {
          RemovePersonPlan(item._id, idcourse);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveCompany(string idcourse, string idcompany)
    {
      try
      {
        var mandatory = mandatoryTrainingService.GetAll(p => p.Course._id == idcourse).FirstOrDefault();
        foreach (var item in mandatory.Companys)
        {
          if (item.Company._id == idcompany)
          {
            mandatory.Companys.Remove(item);
            mandatoryTrainingService.Update(mandatory, null);
            RemoveCompanyPlan(idcompany, idcourse);
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

    public CompanyMandatory AddCompanyMandatory(CompanyMandatory model)
    {
      try
      {
        return companyMandatoryService.Insert(model);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public OccupationMandatory AddOccupationMandatory(OccupationMandatory model)
    {
      try
      {
        return occupationMandatoryService.Insert(model);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public PersonMandatory AddPersonMandatory(PersonMandatory model)
    {
      try
      {
        return personMandatoryService.Insert(model);
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public void RemoveCompanyMandatory(string id)
    {
      try
      {
        var model = companyMandatoryService.GetAll(p => p._id == id).FirstOrDefault();
        model.Status = EnumStatus.Disabled;
        companyMandatoryService.Update(model, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void RemoveOccupationMandatory(string id)
    {
      try
      {
        var model = occupationMandatoryService.GetAll(p => p._id == id).FirstOrDefault();
        model.Status = EnumStatus.Disabled;
        occupationMandatoryService.Update(model, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void RemovePersonMandatory(string id)
    {
      try
      {
        var model = personMandatoryService.GetAll(p => p._id == id).FirstOrDefault();
        model.Status = EnumStatus.Disabled;
        personMandatoryService.Update(model, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Occupation> ListOccupation(string idcourse, string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        List<string> filters = new List<string>();

        var mandatory = mandatoryTrainingService.GetAll(p => p.Course._id == idcourse).FirstOrDefault();
        if (mandatory != null)
        {
          foreach (var item in mandatory.Occupations)
          {
            filters.Add(item.Occupation._id);
          }
        }

        var detail = occupationService.GetAll(p => p.Group.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name)
          .Where(x => !filters.Contains(x._id)).ToList();

        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Person> ListPerson(string idcourse, string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        List<string> filters = new List<string>();

        var mandatory = mandatoryTrainingService.GetAll(p => p.Course._id == idcourse).FirstOrDefault();
        if (mandatory != null)
        {
          foreach (var item in mandatory.Persons)
          {
            filters.Add(item.Person._id);
          }
        }

        var detail = personService.GetAll(p => p.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name)
          .Where(x => !filters.Contains(x._id)).ToList();

        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Company> ListCompany(string idcourse, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        List<string> filters = new List<string>();

        var mandatory = mandatoryTrainingService.GetAll(p => p.Course._id == idcourse).FirstOrDefault();
        if (mandatory != null)
        {
          foreach (var item in mandatory.Companys)
          {
            filters.Add(item.Company._id);
          }
        }

        var detail = companyService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name)
          .Where(x => !filters.Contains(x._id)).ToList();

        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
  }
}
