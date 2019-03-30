using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Services.Specific
{
#pragma warning disable 1998
  public class ServiceMandatoryTraining : Repository<MandatoryTraining>, IServiceMandatoryTraining
  {
    private readonly ServiceGeneric<Company> serviceCompany;
    private readonly ServiceGeneric<Course> serviceCourse;
    private readonly ServiceGeneric<CompanyMandatory> serviceCompanyMandatory;
    private readonly ServiceGeneric<EventHistoric> serviceEventHistoric;
    private readonly ServiceGeneric<MandatoryTraining> serviceMandatoryTraining;
    private readonly ServiceGeneric<Occupation> serviceOccupation;
    private readonly ServiceGeneric<OccupationMandatory> serviceOccupationMandatory;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<PersonMandatory> servicePersonMandatory;
    private readonly ServiceGeneric<TrainingPlan> serviceTrainingPlan;


    public ServiceMandatoryTraining(DataContext context) : base(context)
    {
      try
      {
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceCompanyMandatory = new ServiceGeneric<CompanyMandatory>(context);
        serviceCourse = new ServiceGeneric<Course>(context);
        serviceEventHistoric = new ServiceGeneric<EventHistoric>(context);
        serviceMandatoryTraining = new ServiceGeneric<MandatoryTraining>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        serviceOccupationMandatory = new ServiceGeneric<OccupationMandatory>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        servicePersonMandatory = new ServiceGeneric<PersonMandatory>(context);
        serviceTrainingPlan = new ServiceGeneric<TrainingPlan>(context);
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
      serviceCompanyMandatory._user = _user;
      serviceCourse._user = _user;
      serviceEventHistoric._user = _user;
      serviceMandatoryTraining._user = _user;
      serviceOccupation._user = _user;
      serviceOccupationMandatory._user = _user;
      servicePerson._user = _user;
      servicePersonMandatory._user = _user;
      serviceTrainingPlan._user = _user;
    }
    public string AddOccupation(ViewCrudOccupationMandatory view)
    {
      try
      {
        Course course = serviceCourse.GetNewVersion(p => p._id == view._id).Result;
        Occupation occupation = serviceOccupation.GetNewVersion(p => p._id == view._id).Result;
        var list = new List<OccupationMandatory>
        {
          AddOccupationMandatory(new OccupationMandatory()
          {
            Course = course,
            Occupation = occupation,
            BeginDate = view.BeginDate,
            TypeMandatoryTraining = view.TypeMandatoryTraining
          })
        };
        var mandatory = serviceMandatoryTraining.GetAll(p => p.Course._id == view.Course._id).FirstOrDefault();
        if (mandatory == null)
        {
          serviceMandatoryTraining.Insert(new MandatoryTraining()
          {
            Course = course,
            Occupations = list,
            Status = EnumStatus.Enabled,
            Companys = new List<CompanyMandatory>(),
            Persons = new List<PersonMandatory>()
          });
        }
        else
        {
          mandatory.Occupations.Add(list.FirstOrDefault());
          serviceMandatoryTraining.Update(mandatory, null);
        }
        UpdateTrainingPlanOccupation(course, occupation, view.BeginDate, view.TypeMandatoryTraining);
        return "add occupation";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void NewOnZero(Course course)
    {
      try
      {
        var on = serviceTrainingPlan.GetAuthentication(p => p.Status == EnumStatus.Disabled).Count();
        if (on == 0)
        {
          var person = servicePerson.GetAll().FirstOrDefault();
          var zero = serviceTrainingPlan.Insert(new TrainingPlan() { Person = person, Status = EnumStatus.Disabled, Course = course });
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

        NewOnZero(course);
        var listPlans = serviceTrainingPlan.GetAll(p => p.Course._id == course._id & p.Person._id == person._id).ToList();

        // VERITY DATE LAST COURSE REALIZED
        var realized = serviceEventHistoric.GetAll(p => p.Course._id == course._id & p.Person._id == person._id).ToList();
        var equivalents = Equivalents(course._id, person._id);

        DateTime? dateMax = null;
        if (realized.Count() > 0)
          dateMax = realized.Max(p => p.End);

        if (equivalents.Count() > 0)
        {
          var dateequ = equivalents.Max(p => p.End);
          if (dateequ > dateMax)
            dateMax = dateequ;
        }



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
          if (person.User.DateAdm > maxHis)
            maxHis = person.User.DateAdm;

          if (person.DateLastOccupation > maxHis)
            maxHis = person.DateLastOccupation;

          proxDate = maxHis.Value.AddMonths(course.Periodicity);
        }

        if (listPlans.Where(p => p.Deadline == proxDate).Count() == 0)
        {
          serviceTrainingPlan.Insert(new TrainingPlan()
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

    public List<EventHistoric> Equivalents(string idcourse, string idperson)
    {
      try
      {
        var list = serviceEventHistoric.GetAll(p => p.Person._id == idperson).ToList();
        List<EventHistoric> result = new List<EventHistoric>();
        foreach (var item in list)
        {
          if (item.Course.Equivalents.Where(p => p._id == idcourse).Count() > 0)
            result.Add(item);
        }

        return result;
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
        foreach (var item in serviceCourse.GetAll().ToList())
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
          if (serviceEventHistoric.GetAll(p => p.Course._id == course._id & p.Person._id == idperson).Count() == 0)
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

        var list = servicePerson.GetAll(p => p.Occupation._id == occupation._id).ToList();
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

        var list = servicePerson.GetAll(p => p.Company._id == company._id).ToList();
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

    public string AddPerson(ViewCrudPersonMandatory view)
    {
      try
      {
        Course course = serviceCourse.GetNewVersion(p => p._id == view._id).Result;
        Person person = servicePerson.GetNewVersion(p => p._id == view._id).Result;
        var list = new List<PersonMandatory>
        {
          AddPersonMandatory(new PersonMandatory()
          {
            Course = course,
            Person = person,
            BeginDate = view.BeginDate,
            TypeMandatoryTraining = view.TypeMandatoryTraining
          })
        };
        var mandatory = serviceMandatoryTraining.GetAll(p => p.Course._id == view.Course._id).FirstOrDefault();
        if (mandatory == null)
        {
          serviceMandatoryTraining.Insert(new MandatoryTraining()
          {
            Course = course,
            Occupations = new List<OccupationMandatory>(),
            Status = EnumStatus.Enabled,
            Companys = new List<CompanyMandatory>(),
            Persons = list
          });
        }
        else
        {
          mandatory.Persons.Add(list.FirstOrDefault());
          serviceMandatoryTraining.Update(mandatory, null);
        }
        UpdateTrainingPlanPerson(course, person, view.BeginDate, view.TypeMandatoryTraining);

        return "add occupation";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddCompany(ViewCrudCompanyMandatory view)
    {
      try
      {
        Course course = serviceCourse.GetNewVersion(p => p._id == view._id).Result;
        Company company = serviceCompany.GetNewVersion(p => p._id == view._id).Result;
        var list = new List<CompanyMandatory>
        {
          AddCompanyMandatory(new CompanyMandatory()
          {
            Course = course,
            Company = company,
            BeginDate = view.BeginDate,
            TypeMandatoryTraining = view.TypeMandatoryTraining
          })
        };
        var mandatory = serviceMandatoryTraining.GetAll(p => p.Course._id == view.Course._id).FirstOrDefault();
        if (mandatory == null)
        {
          serviceMandatoryTraining.Insert(new MandatoryTraining()
          {
            Course = course,
            Occupations = new List<OccupationMandatory>(),
            Status = EnumStatus.Enabled,
            Companys = list,
            Persons = new List<PersonMandatory>()
          });
        }
        else
        {
          mandatory.Companys.Add(list.FirstOrDefault());
          serviceMandatoryTraining.Update(mandatory, null);
        }
        UpdateTrainingPlanCompany(course, company, view.BeginDate, view.TypeMandatoryTraining);
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
        var mandatory = serviceMandatoryTraining.GetAll(p => p.Course._id == idcourse).FirstOrDefault();
        foreach (var item in mandatory.Occupations)
        {
          if (item.Occupation._id == idoccupation)
          {
            mandatory.Occupations.Remove(item);
            serviceMandatoryTraining.Update(mandatory, null);
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
        var mandatory = serviceMandatoryTraining.GetAll(p => p.Course._id == idcourse).FirstOrDefault();
        foreach (var item in mandatory.Persons)
        {
          if (item.Person._id == idperson)
          {
            mandatory.Persons.Remove(item);
            serviceMandatoryTraining.Update(mandatory, null);
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
        var plan = serviceTrainingPlan.GetAll(p => p.Person._id == idperson & p.Course._id == idcourse
        & p.StatusTrainingPlan == EnumStatusTrainingPlan.Open).FirstOrDefault();
        plan.Status = EnumStatus.Disabled;
        serviceTrainingPlan.Update(plan, null);
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
        foreach (var item in servicePerson.GetAll(p => p.Occupation._id == idoccoupation).ToList())
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
        foreach (var item in servicePerson.GetAll(p => p.Company._id == idcompany).ToList())
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
        var mandatory = serviceMandatoryTraining.GetAll(p => p.Course._id == idcourse).FirstOrDefault();
        foreach (var item in mandatory.Companys)
        {
          if (item.Company._id == idcompany)
          {
            mandatory.Companys.Remove(item);
            serviceMandatoryTraining.Update(mandatory, null);
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
        var detail = serviceMandatoryTraining.GetAll(p => p.Course.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Course.Name).Skip(skip).Take(count).ToList();
        total = serviceMandatoryTraining.GetAll(p => p.Course.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public MandatoryTraining GetMandatoryTraining(string idcourse)
    {
      try
      {
        var list = serviceMandatoryTraining.GetAll(p => p.Course._id == idcourse).ToList();

        var model = list.Select(p => new MandatoryTraining()
        {
          Occupations = p.Occupations.OrderBy(x => x.Occupation.Name).ToList(),
          Companys = p.Companys.OrderBy(x => x.Company).ToList(),
          Course = p.Course,
          Persons = p.Persons.OrderBy(x => x.Person.User.Name).ToList(),
          Status = p.Status,
          _id = p._id,
          _idAccount = p._idAccount
        })
        .FirstOrDefault();

        return model;
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

        if (view.Include == null)
          view.Include = DateTime.Now;

        serviceTrainingPlan.Insert(view);

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
        serviceTrainingPlan.Update(view, null);
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
        var item = serviceTrainingPlan.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        serviceTrainingPlan.Update(item, null);
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
        return serviceTrainingPlan.GetAll(p => p._id == id).FirstOrDefault();
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
        var detail = serviceTrainingPlan.GetAll(p => p.Person.Company._id == idcompany & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceTrainingPlan.GetAll(p => p.Person.Company._id == idcompany & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<TrainingPlan> ListTrainingPlan(string idcompany, string idperson, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceTrainingPlan.GetAll(p => p.Person._id == idperson & p.Person.Company._id == idcompany & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceTrainingPlan.GetAll(p => p.Person._id == idperson & p.Person.Company._id == idcompany & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTrainingPlan> ListTrainingPlanPerson(string idperson, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceTrainingPlan.GetAll(p => p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Person._id == idperson & p.Course.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceTrainingPlan.GetAll(p => p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Person._id == idperson & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Count();
        var list = new List<ViewTrainingPlan>();
        var countRealized = 0;
        var countNo = 0;

        foreach (var item in detail)
        {
          var view = new ViewTrainingPlan
          {
            Person = item.Person.User.Name,
            Course = item.Course.Name
          };
          if (item.StatusTrainingPlan == EnumStatusTrainingPlan.Realized)
            countRealized += 1;
          else
            countNo += 1;
          view.Origin = item.Origin;
          view.StatusTrainingPlan = item.StatusTrainingPlan;
          list.Add(view);
        }

        if (total > 0)
        {
          list.FirstOrDefault().PercentRealized = ((countRealized * 100) / total);
          list.FirstOrDefault().PercentNo = ((countNo * 100) / total);
        }


        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTrainingPlanList> ListTrainingPlanPersonList(string idmanager, EnumTypeUser typeUser, EnumOrigin origin, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = new List<TrainingPlan>();
        if (typeUser == EnumTypeUser.Manager)
        {
          if (origin == EnumOrigin.Full)
          {
            detail = serviceTrainingPlan.GetAll(p => p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Person.Manager._id == idmanager & p.Course.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
            total = serviceTrainingPlan.GetAll(p => p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Person.Manager._id == idmanager & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Count();
          }
          else
          {
            detail = serviceTrainingPlan.GetAll(p => p.Origin == origin & p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Person.Manager._id == idmanager & p.Course.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
            total = serviceTrainingPlan.GetAll(p => p.Origin == origin & p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Person.Manager._id == idmanager & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Count();
          }
        }
        else
        {
          if (origin == EnumOrigin.Full)
          {
            detail = serviceTrainingPlan.GetAll(p => p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Course.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
            total = serviceTrainingPlan.GetAll(p => p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Count();
          }
          else
          {
            detail = serviceTrainingPlan.GetAll(p => p.Origin == origin & p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Course.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
            total = serviceTrainingPlan.GetAll(p => p.Origin == origin & p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Count();
          }
        }

        var list = new List<ViewTrainingPlan>();

        foreach (var item in detail)
        {
          var plan = new ViewTrainingPlan
          {
            Person = item.Person.User.Name,
            Course = item.Course.Name,
            Origin = item.Origin,
            StatusTrainingPlan = item.StatusTrainingPlan
          };
          list.Add(plan);
        }

       

        var result = new List<ViewTrainingPlanList>();
        var view = new ViewTrainingPlanList();
        var countRealized = 0;
        var countNo = 0;
        var totalGeral = 0;
        view.Person = list.FirstOrDefault().Person;
        view.TraningPlans = new List<ViewTrainingPlan>();
        foreach (var item in list)
        {
          if (item.Person != view.Person)
          {

            if (totalGeral > 0)
            {
              view.TraningPlans.FirstOrDefault().PercentRealized = ((countRealized * 100) / totalGeral);
              view.TraningPlans.FirstOrDefault().PercentNo = ((countNo * 100) / totalGeral);
            }
            result.Add(view);
            view = new ViewTrainingPlanList
            {
              Person = item.Person,
              TraningPlans = new List<ViewTrainingPlan>()
            };
            totalGeral = 0;
            countRealized = 0;
            countNo = 0;
          }
          var training = new ViewTrainingPlan
          {
            Person = view.Person,
            Course = item.Course,
            StatusTrainingPlan = item.StatusTrainingPlan,
            PercentNo = item.PercentNo,
            PercentRealized = item.PercentRealized
          };
          totalGeral += 1;
          if (item.StatusTrainingPlan == EnumStatusTrainingPlan.Realized)
            countRealized += 1;
          else
            countNo += 1;
          view.TraningPlans.Add(training);
          if(item == list.Last())
          {
            if (totalGeral > 0)
            {
              view.TraningPlans.FirstOrDefault().PercentRealized = ((countRealized * 100) / totalGeral);
              view.TraningPlans.FirstOrDefault().PercentNo = ((countNo * 100) / totalGeral);
            }
            view.TraningPlans.Add(training);
          }
            
        }

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public CompanyMandatory AddCompanyMandatory(CompanyMandatory model)
    {
      try
      {
        return serviceCompanyMandatory.Insert(model);
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
        return serviceOccupationMandatory.Insert(model);
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
        return servicePersonMandatory.Insert(model);
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
        var model = serviceCompanyMandatory.GetAll(p => p._id == id).FirstOrDefault();
        model.Status = EnumStatus.Disabled;
        serviceCompanyMandatory.Update(model, null);
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
        var model = serviceOccupationMandatory.GetAll(p => p._id == id).FirstOrDefault();
        model.Status = EnumStatus.Disabled;
        serviceOccupationMandatory.Update(model, null);
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
        var model = servicePersonMandatory.GetAll(p => p._id == id).FirstOrDefault();
        model.Status = EnumStatus.Disabled;
        servicePersonMandatory.Update(model, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListOccupation> ListOccupation(string idcourse, string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        List<string> filters = new List<string>();

        var mandatory = serviceMandatoryTraining.GetNewVersion(p => p.Course._id == idcourse).Result;
        if (mandatory != null)
        {
          foreach (var item in mandatory.Occupations)
          {
            filters.Add(item.Occupation._id);
          }
        }

        var detail = serviceOccupation.GetAllNewVersion(p => p.Group.Company._id == idcompany && p.Name.ToUpper().Contains(filter.ToUpper())).Result
          .Where(x => !filters.Contains(x._id))
          .Select(x => new ViewListOccupation()
          {
            _id = x._id,
            Name = x.Name            
          }).ToList();
        total = detail.Count();
        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPerson> ListPerson(string idcourse, string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        List<string> filters = new List<string>();

        var mandatory = serviceMandatoryTraining.GetNewVersion(p => p.Course._id == idcourse).Result;
        if (mandatory != null)
        {
          foreach (var item in mandatory.Persons)
          {
            filters.Add(item.Person._id);
          }
        }

        var detail = servicePerson.GetAll(p => p.Company._id == idcompany & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name)
          .Where(x => !filters.Contains(x._id))
          .Select(x => new ViewListPerson()
          {
            _id = x._id,
            Company = new ViewListCompany(){ _id = x.Company._id, Name = x.Company.Name },
            Establishment = new ViewListEstablishment() { _id = x.Establishment._id, Name = x.Establishment.Name },
            Registration = x.Registration,
            User = new ViewListUser() { _id = x._id, Name = x.User.Name, Document = x.User.Document, Mail = x.User.Mail, Phone = x.User.Phone }
          }).ToList();

        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListCompany> ListCompany(string idcourse, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        List<string> filters = new List<string>();

        var mandatory = serviceMandatoryTraining.GetNewVersion(p => p.Course._id == idcourse).Result;
        if (mandatory != null)
        {
          foreach (var item in mandatory.Companys)
          {
            filters.Add(item.Company._id);
          }
        }

        var detail = serviceCompany.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result
          .Where(x => !filters.Contains(x._id))
          .Select(x => new ViewListCompany()
          {
            _id = x._id,
            Name = x.Name
          }).ToList();
        ;

        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
#pragma warning restore 1998
}
