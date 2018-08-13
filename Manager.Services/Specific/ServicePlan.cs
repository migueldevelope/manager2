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



    public List<Plan> ListPlans(ref long total, string id, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = monitoringService.GetAll(p => p.Person.Manager._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => p.Activities.Select(x => x.Plans)).ToList();

        var detailSchool = monitoringService.GetAll(p => p.Person._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => p.Schoolings.Select(x => x.Plans)).ToList();

        var detailSkills = monitoringService.GetAll(p => p.Person._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => p.SkillsCompany.Select(x => x.Plans)).ToList();

        List<Plan> result = new List<Plan>();

        foreach (var item in detail)
        {
          foreach (var plan in item)
          {
            foreach (var res in plan)
            {
              result.Add(res);
            }
          }

        }

        foreach (var item in detailSchool)
        {
          foreach (var plan in item)
          {
            foreach (var res in plan)
            {
              result.Add(res);
            }
          }

        }

        foreach (var item in detailSkills)
        {
          foreach (var plan in item)
          {
            foreach (var res in plan)
            {
              result.Add(res);
            }
          }

        }

        total = result.Count();

        return result;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Plan> ListPlansPerson(ref long total, string id, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = monitoringService.GetAll(p => p.Person._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => p.Activities.Select(x => x.Plans)).ToList();

        var detailSchool = monitoringService.GetAll(p => p.Person._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => p.Schoolings.Select(x => x.Plans)).ToList();

        var detailSkills = monitoringService.GetAll(p => p.Person._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => p.SkillsCompany.Select(x => x.Plans)).ToList();


        List<Plan> result = new List<Plan>();

        foreach (var item in detail)
        {
          foreach (var plan in item)
          {
            foreach (var res in plan)
            {
              result.Add(res);
            }
          }

        }

        foreach (var item in detailSchool)
        {
          foreach (var plan in item)
          {
            foreach (var res in plan)
            {
              result.Add(res);
            }
          }

        }

        foreach (var item in detailSkills)
        {
          foreach (var plan in item)
          {
            foreach (var res in plan)
            {
              result.Add(res);
            }
          }

        }

        total = result.Count();

        return result;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }

    }

  }
}
