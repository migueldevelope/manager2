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

    private byte GetBomb(int days)
    {
      try
      {
        if (days > 30)
          return 0;
        else if (days > 10)
          return 1;
        else if (days > 5)
          return 2;
        else if (days >= 0)
          return 3;
        else
          return 4;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewPlan> ListPlans(ref long total, string id, string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewPlan> result = new List<ViewPlan>();

        if (activities == 1)
        {
          var detail = monitoringService.GetAll(p => p.Person.Manager._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), Person = p.Person, _id = p._id }).ToList();


          foreach (var item in detail)
          {
            foreach (var plan in item.Plans)
            {
              foreach (var res in plan)
              {
                result.Add(new ViewPlan()
                {
                  _id = res._id,
                  _idAccount = res._idAccount,
                  Name = res.Name,
                  DateInclude = res.DateInclude,
                  Deadline = res.Deadline,
                  Description = res.Description,
                  Skills = res.Skills,
                  UserInclude = res.UserInclude,
                  TypePlan = res.TypePlan,
                  IdPerson = item.Person._id,
                  NamePerson = item.Person.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
                  Evaluation = res.Evaluation,
                  StatusPlan = res.StatusPlan,
                  TypeAction = res.TypeAction,
                  Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
                });
              }
            }
          }
        }

        if (schooling == 1)
        {
          var detailSchool = monitoringService.GetAll(p => p.Person.Manager._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
            .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), Person = p.Person, _id = p._id }).ToList();
          foreach (var item in detailSchool)
          {
            foreach (var plan in item.Plans)
            {
              foreach (var res in plan)
              {
                result.Add(new ViewPlan()
                {
                  _id = res._id,
                  _idAccount = res._idAccount,
                  Name = res.Name,
                  DateInclude = res.DateInclude,
                  Deadline = res.Deadline,
                  Description = res.Description,
                  Skills = res.Skills,
                  UserInclude = res.UserInclude,
                  TypePlan = res.TypePlan,
                  IdPerson = item.Person._id,
                  NamePerson = item.Person.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
                  Evaluation = res.Evaluation,
                  StatusPlan = res.StatusPlan,
                  TypeAction = res.TypeAction,
                  Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
                });
              }
            }
          }
        }

        if (skillcompany == 1)
        {
          var detailSkills = monitoringService.GetAll(p => p.Person.Manager._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
            .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), Person = p.Person, _id = p._id }).ToList();

          foreach (var item in detailSkills)
          {
            foreach (var plan in item.Plans)
            {
              foreach (var res in plan)
              {
                result.Add(new ViewPlan()
                {
                  _id = res._id,
                  _idAccount = res._idAccount,
                  Name = res.Name,
                  DateInclude = res.DateInclude,
                  Deadline = res.Deadline,
                  Description = res.Description,
                  Skills = res.Skills,
                  UserInclude = res.UserInclude,
                  TypePlan = res.TypePlan,
                  IdPerson = item.Person._id,
                  NamePerson = item.Person.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
                  Evaluation = res.Evaluation,
                  StatusPlan = res.StatusPlan,
                  TypeAction = res.TypeAction,
                  Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
                });
              }
            }
          }
        }

        if (open == 0)
          result = result.Where(p => !(p.StatusPlan == EnumStatusPlan.Open & p.Deadline >= DateTime.Now)).ToList();

        if (expired == 0)
          result = result.Where(p => !(p.StatusPlan == EnumStatusPlan.Open & p.Deadline < DateTime.Now)).ToList();

        if (end == 0)
          result = result.Where(p => p.StatusPlan != EnumStatusPlan.Realized & p.StatusPlan != EnumStatusPlan.NoRealized).ToList();


        total = result.Count();

        return result.Skip(skip).Take(count).OrderBy(p => p.SourcePlan).ThenBy(p => p.Deadline).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewPlan> ListPlansPerson(ref long total, string id, string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewPlan> result = new List<ViewPlan>();

        if (activities == 1)
        {
          var detail = monitoringService.GetAll(p => p.Person._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), Person = p.Person, _id = p._id }).ToList();


          foreach (var item in detail)
          {
            foreach (var plan in item.Plans)
            {
              foreach (var res in plan)
              {
                result.Add(new ViewPlan()
                {
                  _id = res._id,
                  _idAccount = res._idAccount,
                  Name = res.Name,
                  DateInclude = res.DateInclude,
                  Deadline = res.Deadline,
                  Description = res.Description,
                  Skills = res.Skills,
                  UserInclude = res.UserInclude,
                  TypePlan = res.TypePlan,
                  IdPerson = item.Person._id,
                  NamePerson = item.Person.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
                  Evaluation = res.Evaluation,
                  StatusPlan = res.StatusPlan,
                  TypeAction = res.TypeAction,
                  Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
                });
              }
            }
          }
        }

        if (schooling == 1)
        {
          var detailSchool = monitoringService.GetAll(p => p.Person._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
            .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), Person = p.Person, _id = p._id }).ToList();
          foreach (var item in detailSchool)
          {
            foreach (var plan in item.Plans)
            {
              foreach (var res in plan)
              {
                result.Add(new ViewPlan()
                {
                  _id = res._id,
                  _idAccount = res._idAccount,
                  Name = res.Name,
                  DateInclude = res.DateInclude,
                  Deadline = res.Deadline,
                  Description = res.Description,
                  Skills = res.Skills,
                  UserInclude = res.UserInclude,
                  TypePlan = res.TypePlan,
                  IdPerson = item.Person._id,
                  NamePerson = item.Person.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
                  Evaluation = res.Evaluation,
                  StatusPlan = res.StatusPlan,
                  TypeAction = res.TypeAction,
                  Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
                });
              }
            }
          }
        }

        if (skillcompany == 1)
        {
          var detailSkills = monitoringService.GetAll(p => p.Person._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
            .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), Person = p.Person, _id = p._id }).ToList();

          foreach (var item in detailSkills)
          {
            foreach (var plan in item.Plans)
            {
              foreach (var res in plan)
              {
                result.Add(new ViewPlan()
                {
                  _id = res._id,
                  _idAccount = res._idAccount,
                  Name = res.Name,
                  DateInclude = res.DateInclude,
                  Deadline = res.Deadline,
                  Description = res.Description,
                  Skills = res.Skills,
                  UserInclude = res.UserInclude,
                  TypePlan = res.TypePlan,
                  IdPerson = item.Person._id,
                  NamePerson = item.Person.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
                  Evaluation = res.Evaluation,
                  StatusPlan = res.StatusPlan,
                  TypeAction = res.TypeAction,
                  Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
                });
              }
            }
          }
        }

        if (open == 0)
          result = result.Where(p => !(p.StatusPlan == EnumStatusPlan.Open & p.Deadline >= DateTime.Now)).ToList();

        if (expired == 0)
          result = result.Where(p => !(p.StatusPlan == EnumStatusPlan.Open & p.Deadline < DateTime.Now) ).ToList();

        if (end == 0)
          result = result.Where(p => p.StatusPlan != EnumStatusPlan.Realized & p.StatusPlan != EnumStatusPlan.NoRealized).ToList();

        total = result.Count();

        return result.Skip(skip).Take(count).OrderBy(p => p.SourcePlan).ThenBy(p => p.Deadline).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }

    }

    public string UpdatePlan(string idmonitoring, Plan viewPlan)
    {
      try
      {
        var monitoring = monitoringService.GetAll(p => p._id == idmonitoring).FirstOrDefault();

        //verify plan;
        if (viewPlan.SourcePlan == EnumSourcePlan.Activite)
        {
          foreach (var item in monitoring.Activities)
          {
            var listActivities = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == viewPlan._id)
              {
                UpdatePlan(viewPlan);
                listActivities.Add(viewPlan);
              }
              listActivities.Add(plan);
            }
            item.Plans = listActivities;
          }
        }
        else if (viewPlan.SourcePlan == EnumSourcePlan.Schooling)
        {
          foreach (var item in monitoring.Schoolings)
          {
            var listSchoolings = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == viewPlan._id)
              {
                UpdatePlan(viewPlan);
                listSchoolings.Add(viewPlan);
              }
              listSchoolings.Add(plan);
            }
            item.Plans = listSchoolings;
          }
        }
        else
        {
          foreach (var item in monitoring.SkillsCompany)
          {
            var listSkillsCompany = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == viewPlan._id)
              {
                UpdatePlan(viewPlan);
                listSkillsCompany.Add(viewPlan);
              }
              else
                listSkillsCompany.Add(plan);
            }
            item.Plans = listSkillsCompany;
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

    private string UpdatePlan(Plan plan)
    {
      try
      {
        planService.Update(plan, null);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Plan GetPlan(string idmonitoring, string idplan)
    {
      try
      {
        var detail = monitoringService.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), Person = p.Person, _id = p._id }).FirstOrDefault();

        var detailSchoolings = monitoringService.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), Person = p.Person, _id = p._id }).FirstOrDefault();

        var detailSkillsCompany = monitoringService.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), Person = p.Person, _id = p._id }).FirstOrDefault();


        List<ViewPlan> result = new List<ViewPlan>();

        foreach (var plan in detail.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
              return res;
          }
        }

        foreach (var plan in detailSchoolings.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
              return res;
          }
        }

        foreach (var plan in detailSkillsCompany.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
              return res;
          }
        }

        return null;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

  }
}
