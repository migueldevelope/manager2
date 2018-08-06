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
  public class ServiceMonitoring : Repository<Monitoring>, IServiceMonitoring
  {
    private readonly ServiceGeneric<Monitoring> monitoringService;
    private readonly ServiceGeneric<Person> personService;

    public ServiceMonitoring(DataContext context)
      : base(context)
    {
      try
      {
        monitoringService = new ServiceGeneric<Monitoring>(context);
        personService = new ServiceGeneric<Person>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Monitoring> ListMonitoringsEnd(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = monitoringService.GetAll(p => p.Person.Manager._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private void newOnZero()
    {
      try
      {
        var on = monitoringService.GetAuthentication(p => p.Status == EnumStatus.Disabled).Count();
        if (on == 0)
        {
          var person = personService.GetAll().FirstOrDefault();
          var zero = monitoringService.Insert(new Monitoring() { Person = person, Status = EnumStatus.Disabled, StatusMonitoring = EnumStatusMonitoring.End });
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public List<Monitoring> ListMonitoringsWait(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        newOnZero();
        int skip = (count * (page - 1));
        var list = personService.GetAll(p => p.Manager._id == idmanager
        & p.Name.ToUpper().Contains(filter.ToUpper()))
        .ToList().Select(p => new { Person = p, Monitoring = monitoringService.GetAll(x => x.StatusMonitoring != EnumStatusMonitoring.End & x.Person._id == p._id).FirstOrDefault() })
        .ToList();

        var detail = new List<Monitoring>();
        foreach (var item in list)
        {
          if (item.Monitoring == null)
            detail.Add(new Monitoring
            {
              Person = item.Person,
              _id = null,
              StatusMonitoring = EnumStatusMonitoring.Open
            });
          else
            if (item.Monitoring.StatusMonitoring != EnumStatusMonitoring.End)
            detail.Add(item.Monitoring);
        }

        total = detail.Count();
        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Monitoring PersonMonitoringsEnd(string idmanager)
    {
      try
      {
        return monitoringService.GetAll(p => p.Person._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Monitoring PersonMonitoringsWait(string idmanager)
    {
      try
      {
        var item = personService.GetAll(p => p._id == idmanager)
        .ToList().Select(p => new { Person = p, Monitoring = monitoringService.GetAll(x => x.StatusMonitoring != EnumStatusMonitoring.End & x.Person._id == p._id).FirstOrDefault() })
        .FirstOrDefault();

        if (item.Monitoring == null)
          return new Monitoring
          {
            Person = item.Person,
            _id = null,
            StatusMonitoring = EnumStatusMonitoring.Open
          };
        else
         if (item.Monitoring.StatusMonitoring != EnumStatusMonitoring.End)
          return item.Monitoring;
        else
          return null;


      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Monitoring GetMonitorings(string id)
    {
      try
      {
        return monitoringService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private Monitoring loadMap(Monitoring monitoring)
    {
      try
      {
        monitoring.SkillsCompany = new List<MonitoringSkills>();
        foreach (var item in monitoring.Person.Company.Skills)
        {
          monitoring.SkillsCompany.Add(new MonitoringSkills() { Skill = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<Plan>() });
        }

        monitoring.Activities = new List<MonitoringActivities>();
        foreach (var item in monitoring.Person.Occupation.Activities)
        {
          monitoring.Activities.Add(new MonitoringActivities() { Activities = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<Plan>() });
        }

        monitoring.Schoolings = new List<MonitoringSchooling>();
        foreach (var item in monitoring.Person.Occupation.Schooling)
        {
          monitoring.Schoolings.Add(new MonitoringSchooling() { Schooling = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<Plan>() });
        }

        return monitoring;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Monitoring NewMonitoring(Monitoring monitoring, string idperson)
    {
      try
      {
        if (monitoring._id == null)
        {
          loadMap(monitoring);

          if (monitoring.Person._id == idperson)
          {
            monitoring.DateBeginPerson = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressPerson;
          }
          else
          {
            monitoring.DateBeginManager = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressManager;
          }

          monitoringService.Insert(monitoring);
        }
        else
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Wait)
          {
            monitoring.DateBeginEnd = DateTime.Now;
          }
          monitoringService.Update(monitoring, null);
        }

        return monitoring;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateMonitoring(Monitoring monitoring, string idperson)
    {
      try
      {
        if (monitoring.Person._id != idperson)
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Wait)
          {
            monitoring.DateEndManager = DateTime.Now;
          }
        }
        else
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.End)
          {
            monitoring.DateEndEnd = DateTime.Now;
          }
          else if (monitoring.StatusMonitoring == EnumStatusMonitoring.WaitManager)
          {
            monitoring.DateEndPerson = DateTime.Now;
          }

        }
        monitoringService.Update(monitoring, null);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      personService._user = _user;
      monitoringService._user = _user;
    }

    public List<Skill> GetSkills(string idperson)
    {
      try
      {
        var skills = personService.GetAll(p => p._id == idperson).FirstOrDefault().Occupation.Group.Skills;
        var skillsgroup = personService.GetAll(p => p._id == idperson).FirstOrDefault().Occupation.Skills;
        var list = skillsgroup;
        foreach(var item in skills)
        {
          list.Add(item);
        }
        return list;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
  }
}
