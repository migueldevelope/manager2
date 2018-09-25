using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
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
        return eventService.GetAll(p => p._id == id).FirstOrDefault();
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
        var detail = entityService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
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
        var list = personService.GetAll(p => p.Company._id == idcompany & p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator & p.Name.ToUpper().Contains(filter.ToUpper())
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
        var list = personService.GetAll(p => p.Company._id == idcompany & p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator & p.Name.ToUpper().Contains(filter.ToUpper())
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
        int skip = (count * (page - 1));
        var detail = eventService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
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
        int skip = (count * (page - 1));
        var detail = eventHistoricService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
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
        int skip = (count * (page - 1));
        var detail = courseService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
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
        var detail = courseESocialService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
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
        return eventService.Insert(view);
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
        events.Days.Add(days);
        eventService.Update(events, null);
        MathWorkload(idevent);
        UpdateAddDaysParticipant(idevent, days);
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

    public async void UpdateAddDaysParticipant(string idevent, DaysEvent days)
    {
      try
      {
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();
        foreach (var item in events.Participants)
        {
          item.FrequencyEvent.Add(new FrequencyEvent()
          {
            DaysEvent = new DaysEvent() { Begin = days.Begin, End = days.End, Status = EnumStatus.Enabled },
            Present = false,
            Status = EnumStatus.Enabled
          });
        }
        eventService.Update(events, null);

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
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();
        foreach (var item in events.Days)
        {
          if (item._id == iddays)
          {
            events.Days.Remove(item);
            eventService.Update(events, null);
            UpdateAddDaysParticipant(idevent, item);
            MathWorkload(idevent);
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

    private async void MathWorkload(string idevent)
    {
      try
      {
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();
        events.Begin = events.Days.Min(p => p.Begin);
        events.End = events.Days.Max(p => p.End);
        decimal workload = 0;
        foreach (var item in events.Days)
        {
          workload += decimal.Parse((item.End - item.Begin).TotalMinutes.ToString());
        }
        events.Workload = workload;
        eventService.Update(events, null);
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
            Present = false,
            Status = EnumStatus.Enabled
          });
        }
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
        var detail = eventService.GetAll(p => p._id == idevent).FirstOrDefault().Participants.Where(p => p.Name.ToUpper().Contains(filter.ToUpper()));
        total = detail.Count();

        return detail.Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
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

        foreach (var participant in events.Participants)
        {
          if (participant._id == idparticipant)
          {
            foreach (var freq in participant.FrequencyEvent)
            {
              if (freq.DaysEvent._id == idday)
              {
                freq.Present = present;
              }
            }
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

    public string SetGrade(string idevent, string idparticipant, string idday, decimal grade)
    {
      try
      {
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();

        foreach (var participant in events.Participants)
        {
          if (participant._id == idparticipant)
          {
            participant.Grade = grade;
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

    public string NewEventHistoric(EventHistoric view)
    {
      try
      {
        view.Entity = AddEntity(view.Entity.Name);
        eventHistoricService.Insert(view);
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
        courseService.Insert(view);
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
    }

    public Event Update(Event view)
    {
      try
      {
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
          if (item.Approved)
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
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateEventHistoric(EventHistoric view)
    {
      try
      {
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

  }
}
