using Manager.Core.Base;
using Manager.Core.Business;
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
using System.Threading.Tasks;

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


    #region Constructor
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
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceCompany._user = user;
      serviceCompanyMandatory._user = user;
      serviceCourse._user = user;
      serviceEventHistoric._user = user;
      serviceMandatoryTraining._user = user;
      serviceOccupation._user = user;
      serviceOccupationMandatory._user = user;
      servicePerson._user = user;
      servicePersonMandatory._user = user;
      serviceTrainingPlan._user = user;
    }

    #endregion

    #region private

    private void NewOnZero(Course course)
    {
      try
      {
        var on = serviceTrainingPlan.GetAuthentication(p => p.Status == EnumStatus.Disabled).Count();
        if (on == 0)
        {
          var person = servicePerson.GetAllNewVersion().FirstOrDefault();
          var zero = serviceTrainingPlan.InsertNewVersion(new TrainingPlan() { Person = null, Status = EnumStatus.Disabled, Course = null });
        }
      }
      catch (Exception)
      {
        throw;
      }
    }
    private void UpdateTrainingPlanPerson(Course course, Person person, DateTime? beginDate, EnumTypeMandatoryTraining typeMandatoryTraining)
    {
      try
      {
        if (beginDate == null)
          beginDate = DateTime.Now;

        var viewPerson = servicePerson.GetAllNewVersion(p => p._id == person._id).Result.
          Select(p => new ViewListPerson()
          {
            _id = p._id,
            Company = p.Company.GetViewList(),
            Establishment = p.Establishment?.GetViewList(),
            Registration = p.Registration,
            User = p.User.GetViewList()

          }).FirstOrDefault();

        NewOnZero(course);
        var listPlans = serviceTrainingPlan.GetAllNewVersion(p => p.Course._id == course._id & p.Person == viewPerson).Result.ToList();

        // VERITY DATE LAST COURSE REALIZED
        var realized = serviceEventHistoric.GetAllNewVersion(p => p.Course._id == course._id & p.Person == viewPerson).Result.ToList();
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
          serviceTrainingPlan.InsertNewVersion(new TrainingPlan()
          {
            Person = person.Manager == null ? null : person.GetViewListManager(),
            Course = new ViewListCourse() { _id = course._id, Name = course.Name },
            Include = DateTime.Now,
            Observartion = string.Empty,
            Status = EnumStatus.Enabled,
            Origin = (typeMandatoryTraining == EnumTypeMandatoryTraining.Mandatory) ? EnumOrigin.Mandatory : EnumOrigin.Optional,
            StatusTrainingPlan = status,
            Deadline = proxDate.Value.AddMonths(course.Deadline)
          }).Wait();
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private List<EventHistoric> Equivalents(string idcourse, string idperson)
    {
      try
      {
        var list = serviceEventHistoric.GetAllNewVersion(p => p.Person._id == idperson).Result.ToList();
        List<EventHistoric> result = new List<EventHistoric>();
        foreach (var item in list)
        {
          var course = serviceCourse.GetAllNewVersion(p => p._id == item._id).Result.FirstOrDefault();
          if (course.Equivalents.Where(p => p._id == idcourse).Count() > 0)
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
        foreach (var item in serviceCourse.GetAllNewVersion().ToList())
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
          if (serviceEventHistoric.CountNewVersion(p => p.Course._id == course._id & p.Person._id == idperson).Result == 0)
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
    private void UpdateTrainingPlanOccupation(Course course, Occupation occupation, DateTime? beginDate, EnumTypeMandatoryTraining typeMandatoryTraining)
    {
      try
      {
        if (beginDate == null)
          beginDate = DateTime.Now;

        var list = servicePerson.GetAllNewVersion(p => p.Occupation == occupation).Result.ToList();
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
    private void UpdateTrainingPlanCompany(Course course, Company company, DateTime? beginDate, EnumTypeMandatoryTraining typeMandatoryTraining)
    {
      try
      {
        if (beginDate == null)
          beginDate = DateTime.Now;

        var list = servicePerson.GetAllNewVersion(p => p.Company._id == company._id).Result.ToList();
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

    private CompanyMandatory AddCompanyMandatory(CompanyMandatory model)
    {
      try
      {
        return serviceCompanyMandatory.InsertNewVersion(model).Result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private OccupationMandatory AddOccupationMandatory(OccupationMandatory model)
    {
      try
      {
        return serviceOccupationMandatory.InsertNewVersion(model).Result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private PersonMandatory AddPersonMandatory(PersonMandatory model)
    {
      try
      {
        return servicePersonMandatory.InsertNewVersion(model).Result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region mandatorytraining

    public string NewTrainingPlanInternal(TrainingPlan view)
    {
      try
      {

        if (view.Include == null)
          view.Include = DateTime.Now;

        serviceTrainingPlan.InsertNewVersion(view).Wait();

        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateTrainingPlanInternal(TrainingPlan view)
    {
      try
      {
        serviceTrainingPlan.Update(view, null).Wait();
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddOccupation(ViewCrudOccupationMandatory view)
    {
      try
      {
        Course course = serviceCourse.GetNewVersion(p => p._id == view.Course._id).Result;
        Occupation occupation = serviceOccupation.GetNewVersion(p => p._id == view.Occupation._id).Result;
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
        var mandatory = serviceMandatoryTraining.GetAllNewVersion(p => p.Course._id == view.Course._id).Result.FirstOrDefault();
        if (mandatory == null)
        {
          serviceMandatoryTraining.InsertNewVersion(new MandatoryTraining()
          {
            Course = course,
            Occupations = list,
            Status = EnumStatus.Enabled,
            Companys = new List<CompanyMandatory>(),
            Persons = new List<PersonMandatory>()
          }).Wait();
        }
        else
        {
          mandatory.Occupations.Add(list.FirstOrDefault());
          serviceMandatoryTraining.Update(mandatory, null).Wait();
        }
        Task.Run(() => UpdateTrainingPlanOccupation(course, occupation, view.BeginDate, view.TypeMandatoryTraining));
        return "add occupation";
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
        Course course = serviceCourse.GetNewVersion(p => p._id == view.Course._id).Result;
        Person person = servicePerson.GetNewVersion(p => p._id == view.Person._id).Result;
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
        var mandatory = serviceMandatoryTraining.GetAllNewVersion(p => p.Course._id == view.Course._id).Result.FirstOrDefault();
        if (mandatory == null)
        {
          serviceMandatoryTraining.InsertNewVersion(new MandatoryTraining()
          {
            Course = course,
            Occupations = new List<OccupationMandatory>(),
            Status = EnumStatus.Enabled,
            Companys = new List<CompanyMandatory>(),
            Persons = list
          }).Wait();
        }
        else
        {
          mandatory.Persons.Add(list.FirstOrDefault());
          serviceMandatoryTraining.Update(mandatory, null).Wait();
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
        Course course = serviceCourse.GetNewVersion(p => p._id == view.Course._id).Result;
        Company company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result;
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
        var mandatory = serviceMandatoryTraining.GetAllNewVersion(p => p.Course._id == view.Course._id).Result.FirstOrDefault();
        if (mandatory == null)
        {
          serviceMandatoryTraining.InsertNewVersion(new MandatoryTraining()
          {
            Course = course,
            Occupations = new List<OccupationMandatory>(),
            Status = EnumStatus.Enabled,
            Companys = list,
            Persons = new List<PersonMandatory>()
          }).Wait();
        }
        else
        {
          mandatory.Companys.Add(list.FirstOrDefault());
          serviceMandatoryTraining.Update(mandatory, null).Wait();
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
        var mandatory = serviceMandatoryTraining.GetAllNewVersion(p => p.Course._id == idcourse).Result.FirstOrDefault();
        foreach (var item in mandatory.Occupations)
        {
          if (item.Occupation._id == idoccupation)
          {
            mandatory.Occupations.Remove(item);
            serviceMandatoryTraining.Update(mandatory, null).Wait();
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
        var mandatory = serviceMandatoryTraining.GetAllNewVersion(p => p.Course._id == idcourse).Result.FirstOrDefault();
        foreach (var item in mandatory.Persons)
        {
          if (item.Person._id == idperson)
          {
            mandatory.Persons.Remove(item);
            serviceMandatoryTraining.Update(mandatory, null).Wait();
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
    public void RemovePersonPlan(string idperson, string idcourse)
    {
      try
      {
        var plan = serviceTrainingPlan.GetAllNewVersion(p => p.Person._id == idperson & p.Course._id == idcourse
        & p.StatusTrainingPlan == EnumStatusTrainingPlan.Open).Result.FirstOrDefault();
        if(plan != null)
        {
          plan.Status = EnumStatus.Disabled;
          serviceTrainingPlan.Update(plan, null).Wait();
        }
        
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void RemoveOccupationPlan(string idoccoupation, string idcourse)
    {
      try
      {
        foreach (var item in servicePerson.GetAllNewVersion(p => p.Occupation._id == idoccoupation).Result.ToList())
        {
          RemovePersonPlan(item._id, idcourse);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void RemoveCompanyPlan(string idcompany, string idcourse)
    {
      try
      {
        foreach (var item in servicePerson.GetAllNewVersion(p => p.Company._id == idcompany).Result.ToList())
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
        var mandatory = serviceMandatoryTraining.GetAllNewVersion(p => p.Course._id == idcourse).Result.FirstOrDefault();
        foreach (var item in mandatory.Companys)
        {
          if (item.Company._id == idcompany)
          {
            mandatory.Companys.Remove(item);
            serviceMandatoryTraining.Update(mandatory, null).Wait();
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

    public void RemoveCompanyMandatory(string id)
    {
      try
      {
        var model = serviceCompanyMandatory.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
        model.Status = EnumStatus.Disabled;
        serviceCompanyMandatory.Update(model, null).Wait();
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
        var model = serviceOccupationMandatory.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
        model.Status = EnumStatus.Disabled;
        serviceOccupationMandatory.Update(model, null).Wait();
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
        var model = servicePersonMandatory.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
        model.Status = EnumStatus.Disabled;
        servicePersonMandatory.Update(model, null).Wait();
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
            filters.Add(item.Occupation?._id);
          }
        }

        var detail = serviceOccupation.GetAllNewVersion(p => p.Group.Company._id == idcompany && p.Name.ToUpper().Contains(filter.ToUpper())).Result
          .Where(x => !filters.Contains(x._id))
          .Select(x => x.GetViewList()).ToList();
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
            filters.Add(item.Person?._id);
          }
        }

        var detail = servicePerson.GetAllNewVersion(p => p.Company._id == idcompany & p.User.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.User.Name)
          .Where(x => !filters.Contains(x._id))
          .Select(x => x.GetViewList()).ToList();

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
            filters.Add(item.Company?._id);
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

    public List<ViewTrainingPlan> ListTrainingPlanPerson(string iduser, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceTrainingPlan.GetAllNewVersion(p => p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Person.User._id == iduser & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceTrainingPlan.CountNewVersion(p => p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Person.User._id == iduser & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result;
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
            detail = serviceTrainingPlan.GetAllNewVersion(p => p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Person._idManager == idmanager & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
            total = serviceTrainingPlan.CountNewVersion(p => p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Person._idManager == idmanager & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result;
          }
          else
          {
            detail = serviceTrainingPlan.GetAllNewVersion(p => p.Origin == origin & p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Person._idManager == idmanager & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
            total = serviceTrainingPlan.CountNewVersion(p => p.Origin == origin & p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Person._idManager == idmanager & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result;
          }
        }
        else
        {
          if (origin == EnumOrigin.Full)
          {
            detail = serviceTrainingPlan.GetAllNewVersion(p => p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
            total = serviceTrainingPlan.CountNewVersion(p => p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result;
          }
          else
          {
            detail = serviceTrainingPlan.GetAllNewVersion(p => p.Origin == origin & p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
            total = serviceTrainingPlan.CountNewVersion(p => p.Origin == origin & p.StatusTrainingPlan != EnumStatusTrainingPlan.Canceled & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result;
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
          if (item == list.Last())
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

    public string RemoveTrainingPlan(string id)
    {
      try
      {
        var item = serviceTrainingPlan.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        serviceTrainingPlan.Update(item, null).Wait();
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }



    public List<ViewCrudMandatoryTraining> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceMandatoryTraining.GetAllNewVersion(p => p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Course.Name).Skip(skip).Take(count).ToList();
        total = serviceMandatoryTraining.CountNewVersion(p => p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewCrudMandatoryTraining()
        {
          _id = p._id,
          Persons = (p.Persons == null) ? null : p.Persons.Select(x => new ViewCrudPersonMandatory()
          {
            _id = x._id,
            Name = x.Person.User.Name,
            BeginDate = x.BeginDate,
            TypeMandatoryTraining = x.TypeMandatoryTraining,
            Course = new ViewListCourse() { _id = x.Course._id, Name = x.Course.Name },
            Person = x.Person.GetViewList()
          }).ToList(),
          Course = new ViewListCourse() { _id = p.Course._id, Name = p.Course.Name },
          Companys = (p.Companys == null) ? null : p.Companys.Select(x => new ViewCrudCompanyMandatory()
          {
            _id = x._id,
            Name = x.Company.Name,
            BeginDate = x.BeginDate,
            TypeMandatoryTraining = x.TypeMandatoryTraining,
            Course = new ViewListCourse() { _id = x.Course._id, Name = x.Course.Name },
            Company = new ViewListCompany()
            {
              _id = x.Company._id,
              Name = x.Company.Name,
            }
          }).ToList(),
          Occupations = (p.Occupations == null) ? null : p.Occupations.Select(x => new ViewCrudOccupationMandatory()
          {
            _id = x._id,
            BeginDate = x.BeginDate,
            TypeMandatoryTraining = x.TypeMandatoryTraining,
            Course = new ViewListCourse() { _id = x.Course._id, Name = x.Course.Name },
            Occupation = x.Occupation.GetViewList()
          }).ToList()

        }).ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudMandatoryTraining GetMandatoryTraining(string idcourse)
    {
      try
      {
        var list = serviceMandatoryTraining.GetAllNewVersion(p => p.Course._id == idcourse).Result.ToList();

        var model = list.Select(p => new MandatoryTraining()
        {
          Occupations = p.Occupations ?? p.Occupations.OrderBy(x => x.Occupation.Name).ToList(),
          Companys = p.Companys ?? p.Companys.OrderBy(x => x.Company).ToList(),
          Course = p.Course,
          Persons = p.Persons ?? p.Persons.OrderBy(x => x.Person.User.Name).ToList(),
          Status = p.Status,
          _id = p._id,
          _idAccount = p._idAccount
        })
        .ToList();

        return model.Select(p => new ViewCrudMandatoryTraining()
        {
          _id = p._id,
          Persons = (p.Persons == null) ? null : p.Persons.Select(x => new ViewCrudPersonMandatory()
          {
            _id = x._id,
            Name = x.Person == null ? null : x.Person.User.Name,
            BeginDate = x.BeginDate,
            TypeMandatoryTraining = x.TypeMandatoryTraining,
            Course = x.Course == null ? null : new ViewListCourse() { _id = x.Course._id, Name = x.Course.Name },
            Person = x.Person == null ? null : x.Person.GetViewList()
          }).ToList(),
          Course = new ViewListCourse() { _id = p.Course._id, Name = p.Course.Name },
          Companys = (p.Companys == null) ? null : p.Companys.Select(x => new ViewCrudCompanyMandatory()
          {
            _id = x._id,
            Name = x.Company == null ? null : x.Company.Name,
            BeginDate = x.BeginDate,
            TypeMandatoryTraining = x.TypeMandatoryTraining,
            Course = x.Course == null ? null : new ViewListCourse() { _id = x.Course._id, Name = x.Course.Name },
            Company = x.Company == null ? null : new ViewListCompany() { _id = x.Company._id, Name = x.Company.Name }
          }).ToList(),
          Occupations = (p.Occupations == null) ? null : p.Occupations.Select(x => new ViewCrudOccupationMandatory()
          {
            _id = x._id,
            BeginDate = x.BeginDate,
            TypeMandatoryTraining = x.TypeMandatoryTraining,
            Course = x.Course == null ? null : new ViewListCourse() { _id = x.Course._id, Name = x.Course.Name },
            Occupation = x.Occupation == null ? null : x.Occupation.GetViewList()
          }).ToList()

        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewTrainingPlan(ViewCrudTrainingPlan view)
    {
      try
      {
        var trainingplan = new TrainingPlan()
        {
          Include = (view.Include == null) ? null : view.Include = DateTime.Now,
          Status = EnumStatus.Enabled,
          _idAccount = _user._idAccount,
          Observartion = view.Observartion,
          Deadline = view.Deadline,
          Origin = view.Origin,
          StatusTrainingPlan = view.StatusTrainingPlan,
          Person = (view.Person == null) ? null : servicePerson.GetAllNewVersion(p => p._id == view.Person._id).Result.FirstOrDefault().GetViewListManager(),
          Course = (view.Course == null) ? null : new ViewListCourse() { _id = view.Course._id, Name = view.Course.Name },
          Event = (view.Event == null) ? null : new ViewListEvent() { _id = view.Event._id, Name = view.Event.Name }
        };

        serviceTrainingPlan.InsertNewVersion(trainingplan).Wait();

        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateTrainingPlan(ViewCrudTrainingPlan view)
    {
      try
      {
        var trainingplan = serviceTrainingPlan.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
        trainingplan.Include = (view.Include == null) ? null : view.Include = DateTime.Now;
        trainingplan.Observartion = view.Observartion;
        trainingplan.Deadline = view.Deadline;
        trainingplan.Origin = view.Origin;
        trainingplan.StatusTrainingPlan = view.StatusTrainingPlan;
        trainingplan.Person = (view.Person == null) ? null : servicePerson.GetAllNewVersion(p => p._id == view.Person._id).Result.FirstOrDefault().GetViewListManager();
        trainingplan.Course = (view.Course == null) ? null : new ViewListCourse() { _id = view.Course._id, Name = view.Course.Name };
        trainingplan.Event = (view.Event == null) ? null : new ViewListEvent() { _id = view.Event._id, Name = view.Event.Name };

        serviceTrainingPlan.Update(trainingplan, null).Wait();
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudTrainingPlan GetTrainingPlan(string id)
    {
      try
      {
        return serviceTrainingPlan.GetAllNewVersion(p => p._id == id).Result
          .Select(p => new ViewCrudTrainingPlan()
          {
            _id = p._id,
            Deadline = p.Deadline,
            Include = p.Include,
            Origin = p.Origin,
            Observartion = p.Observartion,
            StatusTrainingPlan = p.StatusTrainingPlan,
            Person = (p.Person == null) ? null : new ViewListPersonResume() { _id = p.Person._id, Name = p.Person.User.Name },
            Course = (p.Course == null) ? null : new ViewListCourse() { _id = p.Course._id, Name = p.Course.Name },
            Event = (p.Event == null) ? null : new ViewListEvent() { _id = p.Event._id, Name = p.Event.Name }
          })
          .FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewCrudTrainingPlan> ListTrainingPlan(string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceTrainingPlan.GetAllNewVersion(p => p.Person.Company._id == idcompany & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceTrainingPlan.CountNewVersion(p => p.Person.Company._id == idcompany & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewCrudTrainingPlan()
        {
          _id = p._id,
          Deadline = p.Deadline,
          Include = p.Include,
          Origin = p.Origin,
          Observartion = p.Observartion,
          StatusTrainingPlan = p.StatusTrainingPlan,
          Person = (p.Person == null) ? null : new ViewListPersonResume() { _id = p.Person._id, Name = p.Person.User.Name },
          Course = (p.Course == null) ? null : new ViewListCourse() { _id = p.Course._id, Name = p.Course.Name },
          Event = (p.Event == null) ? null : new ViewListEvent() { _id = p.Event._id, Name = p.Event.Name }
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewCrudTrainingPlan> ListTrainingPlan(string idcompany, string idperson, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceTrainingPlan.GetAllNewVersion(p => p.Person._id == idperson & p.Person.Company._id == idcompany & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceTrainingPlan.CountNewVersion(p => p.Person._id == idperson & p.Person.Company._id == idcompany & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewCrudTrainingPlan()
        {
          _id = p._id,
          Deadline = p.Deadline,
          Include = p.Include,
          Origin = p.Origin,
          Observartion = p.Observartion,
          StatusTrainingPlan = p.StatusTrainingPlan,
          Person = (p.Person == null) ? null : new ViewListPersonResume() { _id = p.Person._id, Name = p.Person.User.Name },
          Course = (p.Course == null) ? null : new ViewListCourse() { _id = p.Course._id, Name = p.Course.Name },
          Event = (p.Event == null) ? null : new ViewListEvent() { _id = p.Event._id, Name = p.Event.Name }
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

  }
#pragma warning restore 1998
}
