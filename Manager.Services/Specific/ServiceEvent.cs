using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
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

    public string New(Event view)
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
        eventService.Insert(view);
        return "add success";
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
        events.Days.Add(days);
        eventService.Update(events, null);
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
        foreach(var item in events.Participants)
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

    public string RemoveDays(string idevent, DaysEvent days)
    {
      try
      {
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();
        events.Days.Remove(days);
        eventService.Update(events, null);
        UpdateAddDaysParticipant(idevent, days);
        return "remove success";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddParticipant(string idevent, Person person)
    {
      try
      {
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();
        var participant = new Participant()
        {
          Person = person,
          Approved = false,
          FrequencyEvent = new List<FrequencyEvent>(),
          Grade = 0,
          Status = EnumStatus.Enabled
        };

        foreach (var days in events.Days)
        {
          participant.FrequencyEvent.Add(new FrequencyEvent()
          {
            DaysEvent = new DaysEvent() { Begin = days.Begin, End = days.End, Status = EnumStatus.Enabled },
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

    public string RemoveParticipant(string idevent, string idperson)
    {
      try
      {
        var events = eventService.GetAll(p => p._id == idevent).FirstOrDefault();
        foreach (var item in events.Participants)
        {
          if (item.Person._id == idperson)
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

    public string Update(Event view)
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

        return "update";
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
