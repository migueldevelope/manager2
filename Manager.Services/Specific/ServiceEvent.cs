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
        eventService.Insert(view);
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
      courseService._user = _user;
      courseESocialService._user = _user;
      personService._user = _user;
    }

    public void SetUser(BaseUser baseUser)
    {
      _user = baseUser;
      eventService._user = _user;
      courseService._user = _user;
      courseESocialService._user = _user;
      personService._user = _user;
    }

    public string Update(Event view)
    {
      try
      {
        eventService.Update(view, null);
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
