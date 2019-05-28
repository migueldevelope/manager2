using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Core.Views;
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
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
#pragma warning disable 1998
  public class ServiceEvent : Repository<Event>, IServiceEvent
  {
    private readonly ServiceGeneric<Course> serviceCourse;
    private readonly ServiceGeneric<CourseESocial> serviceCourseESocial;
    private readonly ServiceGeneric<Entity> serviceEntity;
    private readonly ServiceGeneric<Event> serviceEvent;
    private readonly ServiceGeneric<EventHistoric> serviceEventHistoric;
    private readonly ServiceLog serviceLog;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<TrainingPlan> serviceTrainingPlan;

    private readonly string Path;

    #region Constructor
    public ServiceEvent(DataContext context, DataContext contextLog, string pathToken) : base(context)
    {
      try
      {
        serviceCourse = new ServiceGeneric<Course>(context);
        serviceCourseESocial = new ServiceGeneric<CourseESocial>(context);
        serviceEntity = new ServiceGeneric<Entity>(context);
        serviceEvent = new ServiceGeneric<Event>(context);
        serviceEventHistoric = new ServiceGeneric<EventHistoric>(context);
        serviceLog = new ServiceLog(context, contextLog);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceTrainingPlan = new ServiceGeneric<TrainingPlan>(context);
        Path = pathToken;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceCourse._user = _user;
      serviceCourseESocial._user = _user;
      serviceEntity._user = _user;
      serviceEvent._user = _user;
      serviceEventHistoric._user = _user;
      serviceLog.SetUser(_user);
      servicePerson._user = _user;
      serviceTrainingPlan._user = _user;
    }

    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceCourse._user = user;
      serviceCourseESocial._user = user;
      serviceEntity._user = user;
      serviceEvent._user = user;
      serviceEventHistoric._user = user;
      serviceLog.SetUser(user);
      servicePerson._user = user;
      serviceTrainingPlan._user = user;
    }
    #endregion

    #region Event
    public string RemoveDays(string idevent, string iddays)
    {
      try
      {

        Task.Run(() => LogSave(_user._idPerson, "Remove Days Event: " + idevent + " | day: " + iddays));

        var events = serviceEvent.GetAll(p => p._id == idevent).FirstOrDefault();
        foreach (var item in events.Days)
        {
          if (item._id == iddays)
          {
            events.Days.Remove(item);
            UpdateAddDaysParticipant(ref events, item);
            MathWorkload(ref events);
            serviceEvent.Update(events, null);
            return "remove success";
          }
        }
        return "remove success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveParticipant(string idevent, string idperson)
    {
      try
      {
        var events = serviceEvent.GetAll(p => p._id == idevent).FirstOrDefault();
        foreach (var item in events.Participants)
        {
          if (item._id == idperson)
          {
            events.Participants.Remove(item);
            serviceEvent.Update(events, null);
            return "remove success";
          }

        }

        return "remove success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveInstructor(string idevent, string id)
    {
      try
      {
        var events = serviceEvent.GetAll(p => p._id == idevent).FirstOrDefault();
        foreach (var item in events.Instructors)
        {
          if (item._id == id)
          {
            events.Instructors.Remove(item);
            serviceEvent.Update(events, null);
            return "remove success";
          }

        }

        return "remove success";
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
        Task.Run(() => LogSave(_user._idPerson, "Delete Event " + id));

        var item = serviceEvent.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        serviceEvent.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveEventHistoric(string id)
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, "Delete Event Historic " + id));


        var item = serviceEventHistoric.GetAll(p => p._id == id).FirstOrDefault();
        var obs = "Realized Event: " + item.Name + ", ID_Historic: " + item._id;
        var trainingplan = serviceTrainingPlan.GetAll(p => p.Person._id == item.Person._id
        & p.Course._id == item.Course._id & p.StatusTrainingPlan == EnumStatusTrainingPlan.Realized
        & p.Observartion == obs).FirstOrDefault();
        if (trainingplan != null)
        {
          trainingplan.StatusTrainingPlan = EnumStatusTrainingPlan.Open;
          serviceTrainingPlan.Update(trainingplan, null);
        }
        item.Status = EnumStatus.Disabled;
        serviceEventHistoric.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveCourse(string id)
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, "Delete Course " + id));

        var item = serviceCourse.GetAll(p => p._id == id).FirstOrDefault();
        var exists = serviceEvent.GetAll(p => p.Course._id == item._id & p.StatusEvent == EnumStatusEvent.Open);
        if (exists.Count() > 0)
          return "error_exists";

        item.Status = EnumStatus.Disabled;
        serviceCourse.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveCourseESocial(string id)
    {
      try
      {
        var item = serviceCourseESocial.GetAuthentication(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        serviceCourseESocial.UpdateAccount(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
      throw new NotImplementedException();
    }

    public string ReopeningEvent(string idevent)
    {
      try
      {
        var events = serviceEvent.GetAll(p => p._id == idevent).FirstOrDefault();
        foreach (var item in serviceEventHistoric.GetAll(p => p.Event._id == events._id).ToList())
        {
          serviceEventHistoric.Delete(item._id);

        }

        var plans = serviceTrainingPlan.GetAll(p => p.Event._id == events._id & p.StatusTrainingPlan == EnumStatusTrainingPlan.Realized).ToList();
        foreach (var traningplan in plans)
        {
          traningplan.StatusTrainingPlan = EnumStatusTrainingPlan.Open;
          serviceTrainingPlan.Update(traningplan, null);
        }

        events.StatusEvent = EnumStatusEvent.Open;
        serviceEvent.Update(events, null);

        return "reopening";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetAttachment(string idevent, string url, string fileName, string attachmentid)
    {
      try
      {
        var events = serviceEvent.GetAll(p => p._id == idevent).FirstOrDefault();

        if (events.Attachments == null)
        {
          events.Attachments = new List<ViewCrudAttachmentField>();
        }
        events.Attachments.Add(new ViewCrudAttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
        serviceEvent.Update(events, null);

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetAttachmentHistoric(string idevent, string url, string fileName, string attachmentid)
    {
      try
      {
        var eventsHistoric = serviceEventHistoric.GetAll(p => p._id == idevent).FirstOrDefault();

        if (eventsHistoric.Attachments == null)
        {
          eventsHistoric.Attachments = new List<ViewCrudAttachmentField>();
        }
        eventsHistoric.Attachments.Add(new ViewCrudAttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
        serviceEventHistoric.Update(eventsHistoric, null);

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string SetGrade(string idevent, string idparticipant, decimal grade)
    {
      try
      {
        var events = serviceEvent.GetAll(p => p._id == idevent).FirstOrDefault();

        foreach (var participant in events.Participants)
        {
          if (participant._id == idparticipant)
          {
            participant.Grade = grade;
            if (participant.Grade < events.Grade)
              participant.ApprovedGrade = false;
            else
              participant.ApprovedGrade = true;

            serviceEvent.Update(events, null);
          }
        }

        return "success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Present(string idevent, string idparticipant, string idday, bool present)
    {
      try
      {
        var events = serviceEvent.GetAll(p => p._id == idevent).FirstOrDefault();
        decimal total = 0;
        decimal count = 0;

        foreach (var participant in events.Participants)
        {
          if (participant._id == idparticipant)
          {
            foreach (var freq in participant.FrequencyEvent)
            {
              if (freq._id == idday)
              {
                freq.Present = present;
              }
              if (freq.Present)
                count += 1;

              total += 1;
            }

            if (((count * 100) / total) > events.MinimumFrequency)
              participant.Approved = true;
            else
              participant.Approved = false;

            serviceEvent.Update(events, null);
          }
        }

        return "success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }




    public ViewCrudEvent Get(string id)
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, "Get Event by ID"));
        var events = serviceEvent.GetAll(p => p._id == id).FirstOrDefault();

        return new ViewCrudEvent()
        {
          _id = events._id,
          Course = events.Course,
          Name = events.Name,
          Content = events.Content,
          Entity = events.Entity,
          MinimumFrequency = events.MinimumFrequency,
          LimitParticipants = events.LimitParticipants,
          Grade = events.Grade,
          OpenSubscription = events.OpenSubscription,
          DaysSubscription = events.DaysSubscription,
          Workload = events.Workload,
          Begin = events.Begin,
          End = events.End,
          Instructors = events.Instructors,
          Days = events.Days,
          Participants = events.Participants,
          StatusEvent = events.StatusEvent,
          Observation = events.Observation,
          Evalution = events.Evalution,
          Attachments = events.Attachments,
          DateEnd = events.DateEnd,
          Modality = events.Modality,
          TypeESocial = events.TypeESocial
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudCourse GetCourse(string id)
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, "Get Course by ID"));
        var course = serviceCourse.GetAll(p => p._id == id).FirstOrDefault();

        return new ViewCrudCourse()
        {
          _id = course._id,
          Name = course.Name,
          Content = course.Content,
          CourseESocial = (course.CourseESocial == null) ? null : new ViewCrudCourseESocial() { _id = course.CourseESocial._id, Name = course.CourseESocial.Name, Code = course.CourseESocial.Code },
          Deadline = course.Deadline,
          Equivalents = course.Equivalents?.Select(p => new ViewListCourse()
          {
            _id = p._id,
            Name = p.Name
          }).ToList(),
          Prerequisites = course.Prerequisites?.Select(p => new ViewListCourse()
          {
            _id = p._id,
            Name = p.Name
          }).ToList(),
          Periodicity = course.Periodicity,
          Wordkload = course.Wordkload
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudCourseESocial GetCourseESocial(string id)
    {
      try
      {
        var course = serviceCourseESocial.GetAuthentication(p => p._id == id).FirstOrDefault();
        return new ViewCrudCourseESocial()
        {
          _id = course._idAccount,
          Name = course.Name,
          Code = course.Code
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudEventHistoric GetEventHistoric(string id)
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, "Get Historic by ID"));
        var eventhistoric = serviceEventHistoric.GetAll(p => p._id == id).FirstOrDefault();

        return new ViewCrudEventHistoric()
        {
          _id = eventhistoric._id,
          Begin = eventhistoric.Begin,
          Course = new ViewListCourse() { _id = eventhistoric.Course._id, Name = eventhistoric.Course.Name },
          Name = eventhistoric.Name,
          End = eventhistoric.End,
          Workload = eventhistoric.Workload,
          _idPerson = eventhistoric.Person._id,
          NamePerson = eventhistoric.Person.User.Name,
          Entity = new ViewCrudEntity() { _id = eventhistoric.Entity._id, Name = eventhistoric.Entity.Name },
          Event = new ViewListEvent() { _id = eventhistoric.Event._id, Name = eventhistoric.Event.Name },
          Attachments = eventhistoric.Attachments?.Select(p => new ViewCrudAttachmentField()
          {
            Url = p.Url,
            _idAttachment = p._idAttachment,
            Name = p.Name
          }).ToList()
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewCrudEntity> ListEntity(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceEntity.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = serviceEntity.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewCrudEntity()
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

    public List<ViewListPersonResume> ListPersonParticipants(string idevent, string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = new List<ViewListPersonResume>();
        var participants = serviceEvent.GetAll(p => p._id == idevent).FirstOrDefault().Participants.Select(p => p.Person).ToList();
        var list = servicePerson.GetAll(p => p.Company._id == idcompany & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        foreach (var item in list)
        {
          if (!participants.Contains(item.GetViewList()))
            detail.Add(
              new ViewListPersonResume()
              {
                _id = item._id,
                Name = item.User.Name,
                Document = item.User.Document,
                Cbo = (item.Occupation.CBO == null) ? null : new ViewListCbo()
                {
                  _id = item.Occupation.CBO._id,
                  Name = item.Occupation.CBO.Name,
                  Code = item.Occupation.CBO.Code
                }
              });
        }

        total = detail.Count();

        return detail.Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPersonResume> ListPersonInstructor(string idevent, string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = new List<ViewListPersonResume>();
        var instructors = serviceEvent.GetAll(p => p._id == idevent).FirstOrDefault().Instructors.Select(p => p.Person).ToList();
        var list = servicePerson.GetAll(p => p.Company._id == idcompany & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper())).ToList();

        foreach (var item in list)
        {
          if (!instructors.Contains(item.GetViewList()))
            detail.Add(new ViewListPersonResume()
            {
              _id = item._id,
              Name = item.User.Name,
              Document = item.User.Document,
              Cbo = (item.Occupation.CBO == null) ? null : new ViewListCbo()
              {
                _id = item.Occupation.CBO._id,
                Name = item.Occupation.CBO.Name,
                Code = item.Occupation.CBO.Code
              }
            });
        }

        total = detail.Count();

        return detail.ToList().Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListEvent> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, "List Event"));
        int skip = (count * (page - 1));
        var detail = serviceEvent.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.StatusEvent).ThenBy(p => p.Begin).Skip(skip).Take(count).ToList();
        total = serviceEvent.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.Select(p => new ViewListEvent()
        {
          _id = p._id,
          Name = p.Name,
          _idCourse = p.Course._id,
          NameCourse = p.Course.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListEvent> ListEventOpen(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, "List Open Events"));
        int skip = (count * (page - 1));
        var detail = serviceEvent.GetAll(p => p.StatusEvent == EnumStatusEvent.Open & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = serviceEvent.CountNewVersion(p => p.StatusEvent == EnumStatusEvent.Open & p.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewListEvent()
        {
          _id = p._id,
          Name = p.Name,
          _idCourse = p.Course._id,
          NameCourse = p.Course.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListEvent> ListEventOpenSubscription(string idperson, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        //LogSave(_user._idPerson, "List Open Events subscrive");
        DateTime? date = DateTime.Now;
        int skip = (count * (page - 1));
        var detail = serviceEvent.GetAll(p => p.OpenSubscription == true &
        p.StatusEvent == EnumStatusEvent.Open & p.Name.ToUpper().Contains(filter.ToUpper())).ToList();

        var result = new List<Event>();
        foreach (var item in detail)
        {
          if (item.Begin != null)
          {
            if (date.Value.Date < item.Begin.Value.AddDays(item.DaysSubscription * -1).Date)
            {
              var participants = item.Participants.Where(p => p.Person != null).ToList();
              if (participants.Where(p => p.Person._id == idperson).Count() == 0)
                result.Add(item);
            }
          }
        }
        total = result.Count();

        return result.OrderBy(p => p.Name).Skip(skip).Take(count).Select(p => new ViewListEvent()
        {
          _id = p._id,
          Name = p.Name,
          _idCourse = p.Course._id,
          NameCourse = p.Course.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListEvent> ListEventSubscription(string idperson, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        //LogSave(_user._idPerson, "List Open Events subscrive");
        DateTime? date = DateTime.Now;
        int skip = (count * (page - 1));
        var detail = serviceEvent.GetAll(p => p.StatusEvent == EnumStatusEvent.Open & p.Name.ToUpper().Contains(filter.ToUpper())).ToList();

        var result = new List<Event>();
        foreach (var item in detail)
        {
          if (item.Participants != null)
          {
            try
            {
              var participants = item.Participants.Where(p => p.Person != null).ToList();
              if (participants.Where(p => p.Person._id == idperson).Count() > 0)
                result.Add(item);
            }
            catch (Exception)
            {
              //person null
            }
          }
        }
        total = result.Count();

        return result.OrderBy(p => p.Name).Skip(skip).Take(count).Select(p => new ViewListEvent()
        {
          _id = p._id,
          Name = p.Name,
          _idCourse = p.Course._id,
          NameCourse = p.Course.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListEvent> ListEventEnd(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, "List Realized Events"));
        int skip = (count * (page - 1));
        var detail = serviceEvent.GetAll(p => p.StatusEvent == EnumStatusEvent.Realized & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = serviceEvent.GetAll(p => p.StatusEvent == EnumStatusEvent.Realized & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.Select(p => new ViewListEvent()
        {
          _id = p._id,
          Name = p.Name,
          _idCourse = p.Course._id,
          NameCourse = p.Course.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListEventHistoric> ListEventHistoric(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, "List Historic Events"));

        int skip = (count * (page - 1));
        var detail = serviceEventHistoric.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = serviceEventHistoric.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.Select(p => new ViewListEventHistoric()
        {
          _id = p._id,
          Name = p.Name,
          _idPerson = p.Person._id,
          NamePerson = p.Person.User.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListEventHistoric> ListEventHistoricPerson(string id, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, "List Historic Person"));
        int skip = (count * (page - 1));
        var detail = serviceEventHistoric.GetAll(p => p.Person.User._id == id & p.Course.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = serviceEventHistoric.CountNewVersion(p => p.Person.User._id == id & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewListEventHistoric()
        {
          _id = p._id,
          Name = p.Course.Name,
          _idPerson = p.Person._id,
          NamePerson = p.Person.User.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListCourse> ListCourse(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, "List Course"));

        int skip = (count * (page - 1));
        var detail = serviceCourse.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = serviceCourse.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewListCourse()
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

    public List<ViewCrudCourseESocial> ListCourseESocial(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceCourseESocial.GetAuthentication(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = serviceCourseESocial.CountFreeNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewCrudCourseESocial()
        {
          _id = p._id,
          Name = p.Name,
          Code = p.Code
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListEvent New(ViewCrudEvent view)
    {
      try
      {
        var events = new Event()
        {
          Participants = new List<ViewCrudParticipant>(),
          Instructors = new List<ViewCrudInstructor>(),
          Attachments = new List<ViewCrudAttachmentField>(),
          UserInclude = servicePerson.GetAll(p => p._id == _user._idPerson).FirstOrDefault().GetViewList(),
          DateInclude = DateTime.Now,
          Days = new List<ViewCrudDaysEvent>(),
          Entity = AddEntity(view.Entity.Name),
          Course = serviceCourse.GetAll(p => p._id == view.Course._id).Select(p => new ViewListCourse() { _id = p._id, Name = p.Name }).FirstOrDefault(),
          Name = view.Name,
          Begin = view.Begin,
          Content = view.Content,
          DateEnd = view.DateEnd,
          DaysSubscription = view.DaysSubscription,
          End = view.End,
          Evalution = view.Evalution,
          Grade = view.Grade,
          LimitParticipants = view.LimitParticipants,
          MinimumFrequency = view.MinimumFrequency,
          Modality = view.Modality,
          Observation = view.Observation,
          OpenSubscription = view.OpenSubscription,
          Status = EnumStatus.Enabled,
          StatusEvent = view.StatusEvent,
          TypeESocial = view.TypeESocial,
          Workload = view.Workload
        };

        serviceEvent.InsertNewVersion(events);
        Task.Run(() => LogSave(_user._idPerson, "Insert Event" + events._id));
        return new ViewListEvent()
        {
          _id = events._id,
          Name = events.Name,
          NameCourse = events.Course.Name,
          _idCourse = events.Course._id
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddDays(string idevent, ViewCrudDaysEvent view)
    {
      try
      {

        var events = serviceEvent.GetAll(p => p._id == idevent).FirstOrDefault();
        var days = new ViewCrudDaysEvent()
        {
          Begin = view.Begin,
          End = view.End,
          _id = ObjectId.GenerateNewId().ToString()
        };

        if (events.Days == null)
          events.Days = new List<ViewCrudDaysEvent>();

        events.Days.Add(days);
        MathWorkload(ref events);
        UpdateAddDaysParticipant(ref events, days);
        serviceEvent.Update(events, null);

        Task.Run(() => LogSave(_user._idPerson, "Insert Days Event: " + " | day :" + days._id));
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }

    }

    public async void UpdateRemoveDaysParticipant(string idevent, ViewCrudDaysEvent days)
    {
      try
      {
        var events = serviceEvent.GetAll(p => p._id == idevent).FirstOrDefault();
        foreach (var item in events.Participants)
        {

        }
        serviceEvent.Update(events, null);

      }
      catch (Exception e)
      {
        throw e;
      }

    }


    public string AddInstructor(string idevent, ViewCrudInstructor view)
    {
      try
      {
        var events = serviceEvent.GetAll(p => p._id == idevent).FirstOrDefault();
        var instructor = new ViewCrudInstructor()
        {
          _id = ObjectId.GenerateNewId().ToString(),
          Name = view.Name,
          Content = view.Content,
          Document = view.Document,
          TypeInstructor = view.TypeInstructor,
          Schooling = view.Schooling,
          Person = servicePerson.GetAll(p => p._id == view.Person._id).FirstOrDefault().GetViewList(),
          Cbo = (view.Cbo == null) ? null : new ViewCrudCbo() { _id = view.Cbo._id, Name = view.Cbo.Name, Code = view.Cbo.Code },
        };

        events.Instructors.Add(instructor);
        serviceEvent.Update(events, null);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddParticipant(string idevent, ViewCrudParticipant view)
    {
      try
      {

        var events = serviceEvent.GetAll(p => p._id == idevent).FirstOrDefault();
        var participant = new ViewCrudParticipant()
        {
          _id = ObjectId.GenerateNewId().ToString(),
          FrequencyEvent = new List<ViewCrudFrequencyEvent>(),
          Approved = true
        };

        Task.Run(() => LogSave(_user._idPerson, "Add participant Event: " + idevent + " | participant: " + participant._id));

        foreach (var days in events.Days)
        {
          participant.FrequencyEvent.Add(new ViewCrudFrequencyEvent()
          {
            _id = ObjectId.GenerateNewId().ToString(),
            DaysEvent = new ViewCrudDaysEvent()
            {
              Begin = days.Begin,
              End = days.End,
              _id = ObjectId.GenerateNewId().ToString()
            },
            Present = true
          });
        }

        if (events.Grade > 0)
          participant.ApprovedGrade = false;
        else
          participant.ApprovedGrade = true;

        events.Participants.Add(participant);
        serviceEvent.Update(events, null);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewCrudParticipant> ListParticipants(string idevent, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceEvent.GetAll(p => p._id == idevent).FirstOrDefault().Participants.Where(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        //total = serviceEvent.CountNewVersion(p => p._id == idevent).Result.FirstOrDefault().Participants.Where(p => p.Name.ToUpper().Contains(filter.ToUpper()));

        return detail.Select(p => new ViewCrudParticipant()
        {
          _id = p._id,
          Person = p.Person,
          FrequencyEvent = p.FrequencyEvent?.OrderBy(k => k.DaysEvent.Begin).Select
            (y => new ViewCrudFrequencyEvent()
            {
              _id = y._id,
              Present = y.Present,
              DaysEvent = (y.DaysEvent == null) ? null : new ViewCrudDaysEvent() { _id = y.DaysEvent._id, Begin = y.DaysEvent.Begin, End = y.DaysEvent.End }
            }).ToList(),
          Approved = p.Approved,
          Grade = p.Grade,
          Name = p.Name,
          TypeParticipant = p.TypeParticipant
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewEventHistoricFrontEnd(ViewCrudEventHistoric view)
    {
      try
      {
        var eventhistoric = new EventHistoric()
        {
          Entity = AddEntity(view.Entity.Name),
          Person = servicePerson.GetAll(p => p._id == view._idPerson).FirstOrDefault().GetViewList(),
          Course = (view.Course == null) ? null : serviceCourse.GetAll(p => p._id == view.Course._id)
          .Select(p => new ViewListCourse()
          {
            _id = p._id,
            Name = p.Name
          }).FirstOrDefault(),
          Event = (view.Event == null) ? null : serviceEvent.GetAll(p => p._id == view.Event._id).Select(p => new ViewListEvent()
          {
            _id = p._id,
            Name = p.Name,
            NameCourse = p.Course.Name,
            _idCourse = p.Course._id
          }).FirstOrDefault(),
          Workload = view.Workload,
          Begin = view.Begin,
          End = view.End,
          Attachments = view.Attachments
        };


        if (eventhistoric.Workload.ToString().Contains(","))
          eventhistoric.Workload = decimal.Parse(TimeSpan.Parse(view.Workload.ToString().Split(",")[0].PadLeft(2, '0') + ":" + view.Workload.ToString().Split(",")[1].PadRight(2, '0')).TotalMinutes.ToString());
        else
          eventhistoric.Workload = view.Workload * 60;

        //TimeSpan span = TimeSpan.FromHours(double.Parse(view.Workload.ToString()));
        //view.Workload = decimal.Parse(span.TotalMinutes.ToString());
        //string time = view.Workload.ToString().Replace(",",":");
        //string[] pieces = time.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
        //TimeSpan difference2 = new TimeSpan(Convert.ToInt32(pieces[0]), Convert.ToInt32(pieces[1]), 0);
        //double minutes2 = difference2.TotalMinutes; 
        //view.Workload = decimal.Parse(minutes2.ToString());

        var events = serviceEventHistoric.InsertNewVersion(eventhistoric).Result;
        var plan = serviceTrainingPlan.GetAll(p => p.Person._id == eventhistoric.Person._id & p.Course._id == view.Course._id
        & p.StatusTrainingPlan == EnumStatusTrainingPlan.Open).FirstOrDefault();
        if (plan != null)
        {
          plan.StatusTrainingPlan = EnumStatusTrainingPlan.Realized;
          plan.Observartion = "Realized Event: " + events.Name + ", ID_Historic: " + events._id;
          serviceTrainingPlan.Update(plan, null);
        }
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewEventHistoric(EventHistoric view)
    {
      try
      {
        view.Entity = AddEntity(view.Entity.Name);
        var events = serviceEventHistoric.InsertNewVersion(view).Result;
        var plan = serviceTrainingPlan.GetAll(p => p.Person._id == view.Person._id & p.Course._id == view.Course._id
        & p.StatusTrainingPlan == EnumStatusTrainingPlan.Open).FirstOrDefault();
        if (plan != null)
        {
          plan.StatusTrainingPlan = EnumStatusTrainingPlan.Realized;
          plan.Observartion = "Realized Event: " + events.Name + ", ID_Historic: " + events._id;
          plan.Event = view.Event;
          serviceTrainingPlan.Update(plan, null);
        }
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewCourse(ViewCrudCourse view)
    {
      try
      {
        var course = serviceCourse.InsertNewVersion(new Course()
        {
          Name = view.Name,
          Wordkload = view.Wordkload,
          Content = view.Content,
          Deadline = view.Deadline,
          Periodicity = view.Periodicity,
          CourseESocial = (view.CourseESocial == null) ? null :
          serviceCourseESocial.GetAll(p => p._id == view.CourseESocial._id).FirstOrDefault(),
          Status = EnumStatus.Enabled,
          Prerequisites = view.Prerequisites?.Select(p => new Course()
          {
            _id = p._id,
            Name = p.Name,
            Status = EnumStatus.Enabled,
            _idAccount = _user._idAccount
          }).ToList(),
          Equivalents = view.Equivalents.Select(p => new Course()
          {
            _id = p._id,
            Name = p.Name,
            Status = EnumStatus.Enabled,
            _idAccount = _user._idAccount
          }).ToList()

        }).Result;
        Task.Run(() => LogSave(_user._idPerson, "New Course " + course._id));

        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewCourseESocial(ViewCrudCourseESocial view)
    {
      try
      {
        serviceCourseESocial.InsertFreeNewVersion(new CourseESocial()
        {
          Name = view.Name,
          Code = view.Code,
          Status = EnumStatus.Enabled
        });
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListEvent Update(ViewCrudEvent view)
    {
      try
      {
        var events = serviceEvent.GetAll(p => p._id == view._id).FirstOrDefault();
        events.Course = serviceCourse.GetAll(p => p._id == view.Course._id).Select(p => new ViewListCourse()
        {
          _id = p._id,
          Name = p.Name
        }).FirstOrDefault();
        events.Name = view.Name;
        events.Begin = view.Begin;
        events.Content = view.Content;
        events.DateEnd = view.DateEnd;
        events.DaysSubscription = view.DaysSubscription;
        events.End = view.End;
        events.Evalution = view.Evalution;
        events.Grade = view.Grade;
        events.LimitParticipants = view.LimitParticipants;
        events.MinimumFrequency = view.MinimumFrequency;
        events.Modality = view.Modality;
        events.Observation = view.Observation;
        events.OpenSubscription = view.OpenSubscription;
        events.StatusEvent = view.StatusEvent;
        events.TypeESocial = view.TypeESocial;
        events.Workload = view.Workload;
        Task.Run(() => LogSave(_user._idPerson, "Update Event " + view._id));

        events.UserEdit = servicePerson.GetAll(p => p._id == _user._idPerson).FirstOrDefault().GetViewList();
        events.Entity = AddEntity(view.Entity.Name);
        if (view.StatusEvent == EnumStatusEvent.Realized)
        {
          view.DateEnd = DateTime.Now;
          GenerateHistoric(events);
        }
        serviceEvent.Update(events, null);
        return new ViewListEvent()
        {
          _id = view._id,
          Name = view.Name
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateEventHistoricFrontEnd(ViewCrudEventHistoric view)
    {
      try
      {
        var eventHistoric = serviceEventHistoric.GetAll(p => p._id == view._id).FirstOrDefault();
        eventHistoric.Name = view.Name;
        eventHistoric.Person = servicePerson.GetAll(p => p._id == view._idPerson).FirstOrDefault().GetViewList();
        eventHistoric.Course = (view.Course == null) ? null : serviceCourse.GetAll(p => p._id == view.Course._id).Select(p => new ViewListCourse()
        {
          _id = p._id,
          Name = p.Name
        }).FirstOrDefault();
        eventHistoric.Event = (view.Event == null) ? null : serviceEvent.GetAll(p => p._id == view.Event._id).Select(p => new ViewListEvent()
        {
          _id = p._id,
          Name = p.Name
        }).FirstOrDefault();
        eventHistoric.Workload = view.Workload;
        eventHistoric.Begin = view.Begin;
        eventHistoric.End = view.End;
        eventHistoric.Attachments = view.Attachments;
        Task.Run(() => LogSave(_user._idPerson, "Update Event Historic " + view._id));

        eventHistoric.Entity = AddEntity(view.Entity.Name);
        serviceEventHistoric.Update(eventHistoric, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateEventHistoric(EventHistoric view)
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, "Update Event Historic " + view._id));

        if (view.Workload.ToString().Contains(","))
          view.Workload = decimal.Parse(TimeSpan.Parse(view.Workload.ToString().Split(",")[0].PadLeft(2, '0') + ":" + view.Workload.ToString().Split(",")[1].PadRight(2, '0')).TotalMinutes.ToString());
        else
          view.Workload = view.Workload * 60;

        view.Entity = AddEntity(view.Entity.Name);
        serviceEventHistoric.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateCourse(ViewCrudCourse view)
    {
      try
      {
        var course = serviceCourse.GetAll(p => p._id == view._id).FirstOrDefault();
        course.Content = view.Content;
        course.Periodicity = view.Periodicity;
        course.Deadline = view.Deadline;
        course.Wordkload = view.Wordkload;
        course.CourseESocial = (view.CourseESocial == null) ? null : new CourseESocial() { _id = view.CourseESocial._id, Name = view.CourseESocial.Name, Code = view.CourseESocial.Code };
        course.Equivalents = view.Equivalents?.Select(p => new Course()
        {
          _id = p._id,
          Name = p.Name
        }).ToList();
        course.Prerequisites = view.Prerequisites?.Select(p => new Course()
        {
          _id = p._id,
          Name = p.Name
        }).ToList();

        Task.Run(() => LogSave(_user._idPerson, "Update Course " + view._id));

        serviceCourse.Update(course, null);

        VerifyEquivalent(course);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateCourseESocial(ViewCrudCourseESocial view)
    {
      try
      {
        var esocial = serviceCourseESocial.GetAuthentication(p => p._id == view._id).FirstOrDefault();
        esocial.Name = view.Name;
        esocial.Code = view.Code;

        serviceCourseESocial.UpdateAccount(esocial, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region private

    private void UpdateAddDaysParticipant(ref Event events, ViewCrudDaysEvent days)
    {
      try
      {
        foreach (var item in events.Participants)
        {
          item.FrequencyEvent.Add(new ViewCrudFrequencyEvent()
          {
            DaysEvent = days,
            Present = true,
            _id = ObjectId.GenerateNewId().ToString()
          });
        }
        //serviceEvent.Update(events, null);

      }
      catch (Exception e)
      {
        throw e;
      }

    }

    private async void GenerateHistoric(Event view)
    {
      try
      {
        foreach (var item in view.Participants)
        {
          if (item.Approved & (item.Grade > view.Grade))
          {
            //NewEventHistoric(new EventHistoric()
            //{
            //  Name = view.Name,
            //  Event = view,
            //  Course = view.Course,
            //  Entity = view.Entity,
            //  Workload = view.Workload,
            //  Person = item.Person,
            //  Status = EnumStatus.Enabled,
            //  Begin = DateTime.Parse(view.Begin.ToString()),
            //  End = DateTime.Parse(view.End.ToString()),
            //  Attachments = view.Attachments
            //});
          }

        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void MathWorkload(ref Event events)
    {
      try
      {
        if (events.Days.Count() > 0)
        {
          events.Begin = events.Days.Min(p => p.Begin);
          events.End = events.Days.Max(p => p.End);
        }
        else
        {
          events.Begin = null;
          events.End = null;
        }
        decimal workload = 0;
        foreach (var item in events.Days)
        {
          workload += decimal.Parse((item.End - item.Begin).TotalMinutes.ToString());
        }
        events.Workload = workload;
        //serviceEvent.Update(events, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private ViewCrudEntity AddEntity(string name)
    {
      try
      {
        var entity = serviceEntity.GetAuthentication(p => p.Name.ToUpper().Contains(name.ToUpper())).FirstOrDefault();
        if (entity == null)
          entity = serviceEntity.InsertNewVersion(new Entity()
          {
            Status = EnumStatus.Enabled,
            Name = name
          }).Result;

        return new ViewCrudEntity()
        {
          _id = entity._id,
          Name = entity.Name
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async void VerifyEquivalent(Course course)
    {
      try
      {
        var list = serviceTrainingPlan.GetAll(p => p.Course._id == course._id & p.StatusTrainingPlan == EnumStatusTrainingPlan.Open).ToList();
        if (course.Equivalents != null)
        {
          foreach (var item in course.Equivalents)
          {
            foreach (var plan in list)
            {
              var eventsHis = serviceEventHistoric.GetAll(p => p.Course._id == item._id & p.Person._id == plan.Person._id);
              if (eventsHis.Count() > 0)
              {
                plan.StatusTrainingPlan = EnumStatusTrainingPlan.Realized;
                plan.Observartion = "Realized Event: " + eventsHis.LastOrDefault().Name + ", ID_Historic: " + eventsHis.LastOrDefault()._id;
                serviceTrainingPlan.Update(plan, null);
              }

            }
          }
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task LogSave(string iduser, string local)
    {
      try
      {
        var user = servicePerson.GetAll(p => p._id == iduser).FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "Access Event ",
          Local = local,
          _idPerson = user._id
        };
        serviceLog.NewLog(log);
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
