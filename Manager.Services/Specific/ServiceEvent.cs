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
using System.Text;

namespace Manager.Services.Specific
{
  public class ServiceEvent : Repository<Event>, IServiceEvent
  {
    private readonly ServiceGeneric<Event> eventService;
    private readonly ServiceGeneric<Entity> entityService;
    private readonly ServiceGeneric<EventHistoric> eventHistoricService;
    private readonly ServiceGeneric<Course> courseService;
    private readonly ServiceGeneric<CourseESocial> courseESocialService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<TrainingPlan> trainingPlanService;
    private readonly ServiceLog logService;
    private string path;

    public ServiceEvent(DataContext context, string pathToken)
     : base(context)
    {
      try
      {
        eventService = new ServiceGeneric<Event>(context);
        courseService = new ServiceGeneric<Course>(context);
        courseESocialService = new ServiceGeneric<CourseESocial>(context);
        personService = new ServiceGeneric<Person>(context);
        entityService = new ServiceGeneric<Entity>(context);
        eventHistoricService = new ServiceGeneric<EventHistoric>(context);
        trainingPlanService = new ServiceGeneric<TrainingPlan>(context);
        logService = new ServiceLog(context);
        path = pathToken;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Event Get(string id)
    {
      try
      {
        LogSave(_user._idPerson, "Get Event by ID");
        var events = eventService.GetAll(p => p._id == id).FirstOrDefault();

        return new Event()
        {
          _id = events._id,
          _idAccount = events._idAccount,
          Status = events.Status,
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
          Days = events.Days.OrderBy(p => p.Begin).ToList(),
          Participants = events.Participants.Select(x => new Participant()
          {
            _id = x._id,
            _idAccount = x._idAccount,
            Status = x.Status,
            Person = x.Person,
            FrequencyEvent = x.FrequencyEvent.OrderBy(k => k.DaysEvent.Begin).ToList(),
            Approved = x.Approved,
            Grade = x.Grade,
            Name = x.Name,
            TypeParticipant = x.TypeParticipant
          }).ToList(),
          StatusEvent = events.StatusEvent,
          Observation = events.Observation,
          Evalution = events.Evalution,
          Attachments = events.Attachments,
          UserInclude = events.UserInclude,
          DateInclude = events.DateInclude,
          UserEdit = events.UserEdit,
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

    private Entity AddEntity(string name)
    {
      try
      {
        var entity = entityService.GetAuthentication(p => p.Name.ToUpper().Contains(name.ToUpper())).FirstOrDefault();
        if (entity == null)
          return entityService.Insert(new Entity()
          {
            Status = EnumStatus.Enabled,
            Name = name
          });
        else
          return entity;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Course GetCourse(string id)
    {
      try
      {
        LogSave(_user._idPerson, "Get Course by ID");
        return courseService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public CourseESocial GetCourseESocial(string id)
    {
      try
      {
        return courseESocialService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public EventHistoric GetEventHistoric(string id)
    {
      try
      {
        LogSave(_user._idPerson, "Get Historic by ID");
        return eventHistoricService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Entity> ListEntity(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = entityService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = entityService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Person> ListPersonParticipants(string idevent, string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = new List<Person>();
        var participants = eventService.GetAll(p => p._id == idevent).FirstOrDefault().Participants.Select(p => p.Person).ToList();
        var list = personService.GetAll(p => p.Company._id == idcompany & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Name.ToUpper().Contains(filter.ToUpper())
        ).ToList();
        foreach (var item in list)
        {
          if (!participants.Contains(item))
            detail.Add(item);
        }

        total = detail.Count();

        return detail.Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Person> ListPersonInstructor(string idevent, string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = new List<Person>();
        var instructors = eventService.GetAll(p => p._id == idevent).FirstOrDefault().Instructors.Select(p => p.Person).ToList();
        var list = personService.GetAll(p => p.Company._id == idcompany & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Name.ToUpper().Contains(filter.ToUpper())
        ).ToList();
        foreach (var item in list)
        {
          if (!instructors.Contains(item))
            detail.Add(item);
        }

        total = detail.Count();

        return detail.Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Event> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        LogSave(_user._idPerson, "List Event");
        int skip = (count * (page - 1));
        var detail = eventService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.StatusEvent).ThenBy(p => p.Begin).Skip(skip).Take(count).ToList();
        total = eventService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Event> ListEventOpen(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        LogSave(_user._idPerson, "List Open Events");
        int skip = (count * (page - 1));
        var detail = eventService.GetAll(p => p.StatusEvent == EnumStatusEvent.Open & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = eventService.GetAll(p => p.StatusEvent == EnumStatusEvent.Open & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Event> ListEventEnd(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        LogSave(_user._idPerson, "List Realized Events");
        int skip = (count * (page - 1));
        var detail = eventService.GetAll(p => p.StatusEvent == EnumStatusEvent.Realized & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = eventService.GetAll(p => p.StatusEvent == EnumStatusEvent.Realized & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<EventHistoric> ListEventHistoric(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        LogSave(_user._idPerson, "List Historic Events");

        int skip = (count * (page - 1));
        var detail = eventHistoricService.GetAll(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = eventHistoricService.GetAll(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<EventHistoric> ListEventHistoricPerson(string id, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        LogSave(_user._idPerson, "List Historic Person");
        int skip = (count * (page - 1));
        var detail = eventHistoricService.GetAll(p => p.Person._id == id & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = eventHistoricService.GetAll(p => p.Person._id == id & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Course> ListCourse(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        LogSave(_user._idPerson, "List Course");

        int skip = (count * (page - 1));
        var detail = courseService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = courseService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<CourseESocial> ListCourseESocial(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = courseESocialService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = courseESocialService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Event New(Event view)
    {
      try
      {

        view.Participants = new List<Participant>();
        view.Instructors = new List<Instructor>();
        view.Attachments = new List<AttachmentField>();
        view.UserInclude = personService.GetAll(p => p._id == _user._idPerson).FirstOrDefault();
        view.DateInclude = DateTime.Now;
        view.Days = new List<DaysEvent>();
        view.Entity = AddEntity(view.Entity.Name);

        var events = eventService.Insert(view);
        LogSave(_user._idPerson, "Insert Event" + events._id);
        return events;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddDays(string idevent, DaysEvent days)
    {
      try
      {

        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();
        days._idAccount = _user._idAccount;
        days._id = ObjectId.GenerateNewId().ToString();
        days.Status = EnumStatus.Enabled;
        if (events.Days == null)
          events.Days = new List<DaysEvent>();

        events.Days.Add(days);
        MathWorkload(ref events);
        UpdateAddDaysParticipant(ref events, days);
        eventService.Update(events, null);

        LogSave(_user._idPerson, "Insert Days Event: " + " | day :" + days._id);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }

    }

    public async void UpdateRemoveDaysParticipant(string idevent, DaysEvent days)
    {
      try
      {
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();
        foreach (var item in events.Participants)
        {

        }
        eventService.Update(events, null);

      }
      catch (Exception e)
      {
        throw e;
      }

    }

    public void UpdateAddDaysParticipant(ref Event events, DaysEvent days)
    {
      try
      {
        foreach (var item in events.Participants)
        {
          item.FrequencyEvent.Add(new FrequencyEvent()
          {
            DaysEvent = days,
            Present = true,
            Status = EnumStatus.Enabled,
            _id = ObjectId.GenerateNewId().ToString(),
            _idAccount = _user._idAccount
          });
        }
        //eventService.Update(events, null);

      }
      catch (Exception e)
      {
        throw e;
      }

    }

    public string RemoveDays(string idevent, string iddays)
    {
      try
      {

        LogSave(_user._idPerson, "Remove Days Event: " + idevent + " | day: " + iddays);

        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();
        foreach (var item in events.Days)
        {
          if (item._id == iddays)
          {
            events.Days.Remove(item);
            UpdateAddDaysParticipant(ref events, item);
            MathWorkload(ref events);
            eventService.Update(events, null);
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
        //eventService.Update(events, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddInstructor(string idevent, Instructor instructor)
    {
      try
      {
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();
        instructor._idAccount = _user._idAccount;
        instructor._id = ObjectId.GenerateNewId().ToString();
        instructor.Status = EnumStatus.Enabled;
        events.Instructors.Add(instructor);
        eventService.Update(events, null);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddParticipant(string idevent, Participant participant)
    {
      try
      {

        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();

        participant._id = ObjectId.GenerateNewId().ToString();

        LogSave(_user._idPerson, "Add participant Event: " + idevent + " | participant: " + participant._id);

        participant._idAccount = _user._idAccount;
        participant.FrequencyEvent = new List<FrequencyEvent>();

        foreach (var days in events.Days)
        {
          participant.FrequencyEvent.Add(new FrequencyEvent()
          {
            _id = ObjectId.GenerateNewId().ToString(),
            _idAccount = _user._idAccount,
            DaysEvent = new DaysEvent()
            {
              Begin = days.Begin,
              End = days.End,
              Status = EnumStatus.Enabled,
              _id = ObjectId.GenerateNewId().ToString(),
              _idAccount = _user._idAccount,
            },
            Present = true,
            Status = EnumStatus.Enabled
          });
        }
        participant.Approved = true;
        if (events.Grade > 0)
          participant.ApprovedGrade = false;
        else
          participant.ApprovedGrade = true;

        events.Participants.Add(participant);
        eventService.Update(events, null);
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Participant> ListParticipants(string idevent, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = eventService.GetAll(p => p._id == idevent).FirstOrDefault().Participants.Where(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = eventService.GetAll(p => p._id == idevent).FirstOrDefault().Participants.Where(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string Present(string idevent, string idparticipant, string idday, bool present)
    {
      try
      {
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();
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

            if (((count * 100) / total) >= events.MinimumFrequency)
              participant.Approved = true;
            else
              participant.Approved = false;

            eventService.Update(events, null);
          }
        }

        return "success";
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
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();

        foreach (var participant in events.Participants)
        {
          if (participant._id == idparticipant)
          {
            participant.Grade = grade;
            if (participant.Grade < events.Grade)
              participant.ApprovedGrade = false;
            else
              participant.ApprovedGrade = true;

            eventService.Update(events, null);
          }
        }

        return "success";
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
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();
        foreach (var item in events.Participants)
        {
          if (item._id == idperson)
          {
            events.Participants.Remove(item);
            eventService.Update(events, null);
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
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();
        foreach (var item in events.Instructors)
        {
          if (item._id == id)
          {
            events.Instructors.Remove(item);
            eventService.Update(events, null);
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

    public string NewEventHistoricFrontEnd(EventHistoric view)
    {
      try
      {
        view.Entity = AddEntity(view.Entity.Name);
        TimeSpan span = TimeSpan.FromHours(double.Parse(view.Workload.ToString()));
        view.Workload = decimal.Parse(span.TotalMinutes.ToString());

        var events = eventHistoricService.Insert(view);
        var plan = trainingPlanService.GetAll(p => p.Person._id == view.Person._id & p.Course._id == view.Course._id
        & p.StatusTrainingPlan == EnumStatusTrainingPlan.Open).FirstOrDefault();
        if (plan != null)
        {
          plan.StatusTrainingPlan = EnumStatusTrainingPlan.Realized;
          plan.Observartion = "Realized Event: " + view.Name + ", ID: " + events._id;
          trainingPlanService.Update(plan, null);
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
        var events = eventHistoricService.Insert(view);
        var plan = trainingPlanService.GetAll(p => p.Person._id == view.Person._id & p.Course._id == view.Course._id
        & p.StatusTrainingPlan == EnumStatusTrainingPlan.Open).FirstOrDefault();
        if(plan != null)
        {
          plan.StatusTrainingPlan = EnumStatusTrainingPlan.Realized;
          plan.Observartion = "Realized Event: " + view.Name + ", ID: " + events._id;
          trainingPlanService.Update(plan, null);
        }
        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewCourse(Course view)
    {
      try
      {
        var course = courseService.Insert(view);
        LogSave(_user._idPerson, "New Course " + course._id);

        return "add success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewCourseESocial(CourseESocial view)
    {
      try
      {
        courseESocialService.Insert(view);
        return "add success";
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
        LogSave(_user._idPerson, "Delete Event " + id);

        var item = eventService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        eventService.Update(item, null);
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
        LogSave(_user._idPerson, "Delete Event Historic " + id);

        var item = eventHistoricService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        eventHistoricService.Update(item, null);
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
        LogSave(_user._idPerson, "Delete Course " + id);

        var item = courseService.GetAll(p => p._id == id).FirstOrDefault();
        var exists = eventService.GetAll(p => p.Course == item & p.StatusEvent == EnumStatusEvent.Open);
        if (exists.Count() > 0)
          return "error_exists";

        item.Status = EnumStatus.Disabled;
        courseService.Update(item, null);
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
        var item = courseESocialService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        courseESocialService.Update(item, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
      throw new NotImplementedException();
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      eventService._user = _user;
      eventHistoricService._user = _user;
      courseService._user = _user;
      courseESocialService._user = _user;
      personService._user = _user;
      entityService._user = _user;
      logService._user = _user;
      trainingPlanService._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      _user = baseUser;
      eventService._user = _user;
      eventHistoricService._user = _user;
      courseService._user = _user;
      courseESocialService._user = _user;
      personService._user = _user;
      entityService._user = _user;
      logService._user = _user;
      trainingPlanService._user = _user;
    }

    public Event Update(Event view)
    {
      try
      {
        LogSave(_user._idPerson, "Update Event " + view._id);

        view.UserEdit = personService.GetAll(p => p._id == _user._idPerson).FirstOrDefault();
        view.Entity = AddEntity(view.Entity.Name);
        if (view.StatusEvent == EnumStatusEvent.Realized)
        {
          view.DateEnd = DateTime.Now;
          GenerateHistoric(view);
        }
        eventService.Update(view, null);
        return view;
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
          if (item.Approved & (item.Grade >= view.Grade))
          {
            NewEventHistoric(new EventHistoric()
            {
              Name = view.Name,
              Event = view,
              Course = view.Course,
              Entity = view.Entity,
              Workload = view.Workload,
              Person = item.Person,
              Status = EnumStatus.Enabled,
              Begin = DateTime.Parse(view.Begin.ToString()),
              End = DateTime.Parse(view.End.ToString())
            });
          }

        }
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
        LogSave(_user._idPerson, "Update Event Historic " + view._id);

        view.Entity = AddEntity(view.Entity.Name);
        eventHistoricService.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateEventHistoricFrontEnd(EventHistoric view)
    {
      try
      {
        LogSave(_user._idPerson, "Update Event Historic " + view._id);

        TimeSpan span = TimeSpan.FromHours(double.Parse(view.Workload.ToString()));
        view.Workload = decimal.Parse(span.TotalMinutes.ToString());

        view.Entity = AddEntity(view.Entity.Name);
        eventHistoricService.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateCourse(Course view)
    {
      try
      {
        LogSave(_user._idPerson, "Update Course " + view._id);

        courseService.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateCourseESocial(CourseESocial view)
    {
      try
      {
        courseESocialService.Update(view, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public async void LogSave(string iduser, string local)
    {
      try
      {
        var user = personService.GetAll(p => p._id == iduser).FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "Access Event ",
          Local = local,
          Person = user
        };
        logService.NewLog(log);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ReopeningEvent(string idevent)
    {
      try
      {
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();
        foreach (var item in eventHistoricService.GetAll(p => p.Event == events).ToList())
        {
          eventHistoricService.Delete(item._id);
        }
        events.StatusEvent = EnumStatusEvent.Open;
        eventService.Update(events, null);

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
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();

        if (events.Attachments == null)
        {
          events.Attachments = new List<AttachmentField>();
        }
        events.Attachments.Add(new AttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
        eventService.Update(events, null);

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetAttachmentHistoric(string idevent, string url, string fileName, string attachmentid)
    {
      try
      {
        var eventsHistoric = eventHistoricService.GetAll(p => p._id == idevent).FirstOrDefault();

        if (eventsHistoric.Attachments == null)
        {
          eventsHistoric.Attachments = new List<AttachmentField>();
        }
        eventsHistoric.Attachments.Add(new AttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
        eventHistoricService.Update(eventsHistoric, null);

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

  }
}
