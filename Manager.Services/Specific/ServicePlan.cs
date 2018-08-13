using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tools;

namespace Manager.Services.Specific
{
  public class ServicePlan : Repository<Plan>, IServicePlan
  {
    private ServiceGeneric<Person> personService;
    private ServiceGeneric<Monitoring> monitoringService;
    private ServiceGeneric<Plan> planService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServicePlan(DataContext context)
      : base(context)
    {
      try
      {
        monitoringService = new ServiceGeneric<Monitoring>(context);
        personService = new ServiceGeneric<Person>(context);
        planService = new ServiceGeneric<Plan>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }


    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      try
      {
        User(contextAccessor);
        personService._user = _user;
        monitoringService._user = _user;
        planService._user = _user;

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }



    public List<ViewPlan> ListPlans(ref long total, string id, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var detail = monitoringService.GetAll(p => p.Person.Manager._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), Person = p.Person }).ToList();

        var detailSchool = monitoringService.GetAll(p => p.Person.Manager._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), Person = p.Person }).ToList();

        var detailSkills = monitoringService.GetAll(p => p.Person.Manager._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), Person = p.Person }).ToList();

        List<ViewPlan> result = new List<ViewPlan>();

        foreach (var item in detail)
        {
          foreach (var plan in item.Plans)
          {
            foreach (var res in plan)
            {
              result.Add(new ViewPlan()
              {
                Name = res.Name,
                DateInclude = res.DateInclude,
                Deadline = res.Deadline,
                Description = res.Description,
                Skills = res.Skills,
                UserInclude = res.UserInclude,
                TypePlan = res.TypePlan,
                IdPerson = item.Person._id,
                NamePerson = item.Person.Name
              });
            }
          }
        }

        foreach (var item in detailSchool)
        {
          foreach (var plan in item.Plans)
          {
            foreach (var res in plan)
            {
              result.Add(new ViewPlan()
              {
                Name = res.Name,
                DateInclude = res.DateInclude,
                Deadline = res.Deadline,
                Description = res.Description,
                Skills = res.Skills,
                UserInclude = res.UserInclude,
                TypePlan = res.TypePlan,
                IdPerson = item.Person._id,
                NamePerson = item.Person.Name
              });
            }
          }
        }

        foreach (var item in detailSkills)
        {
          foreach (var plan in item.Plans)
          {
            foreach (var res in plan)
            {
              result.Add(new ViewPlan()
              {
                Name = res.Name,
                DateInclude = res.DateInclude,
                Deadline = res.Deadline,
                Description = res.Description,
                Skills = res.Skills,
                UserInclude = res.UserInclude,
                TypePlan = res.TypePlan,
                IdPerson = item.Person._id,
                NamePerson = item.Person.Name
              });
            }
          }
        }

        total = result.Count();

        return result.Skip(skip).Take(count).OrderBy(p => p.Deadline).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewPlan> ListPlansPerson(ref long total, string id, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var detail = monitoringService.GetAll(p => p.Person._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), Person = p.Person }).ToList();

        var detailSchool = monitoringService.GetAll(p => p.Person._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), Person = p.Person }).ToList();

        var detailSkills = monitoringService.GetAll(p => p.Person._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), Person = p.Person }).ToList();


        List<ViewPlan> result = new List<ViewPlan>();

        foreach (var item in detail)
        {
          foreach (var plan in item.Plans)
          {
            foreach (var res in plan)
            {
              result.Add(new ViewPlan()
              {
                Name = res.Name,
                DateInclude = res.DateInclude,
                Deadline = res.Deadline,
                Description = res.Description,
                Skills = res.Skills,
                UserInclude = res.UserInclude,
                TypePlan = res.TypePlan,
                IdPerson = item.Person._id,
                NamePerson = item.Person.Name
              });
            }
          }
        }

        foreach (var item in detailSchool)
        {
          foreach (var plan in item.Plans)
          {
            foreach (var res in plan)
            {
              result.Add(new ViewPlan()
              {
                Name = res.Name,
                DateInclude = res.DateInclude,
                Deadline = res.Deadline,
                Description = res.Description,
                Skills = res.Skills,
                UserInclude = res.UserInclude,
                TypePlan = res.TypePlan,
                IdPerson = item.Person._id,
                NamePerson = item.Person.Name
              });
            }
          }
        }

        foreach (var item in detailSkills)
        {
          foreach (var plan in item.Plans)
          {
            foreach (var res in plan)
            {
              result.Add(new ViewPlan()
              {
                Name = res.Name,
                DateInclude = res.DateInclude,
                Deadline = res.Deadline,
                Description = res.Description,
                Skills = res.Skills,
                UserInclude = res.UserInclude,
                TypePlan = res.TypePlan,
                IdPerson = item.Person._id,
                NamePerson = item.Person.Name
              });
            }
          }
        }

        total = result.Count();

        return result.Skip(skip).Take(count).OrderBy(p => p.Deadline).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }

    }

  }
}
