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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Tools;

namespace Manager.Services.Specific
{
  public class ServicePlan : Repository<Plan>, IServicePlan
  {
    private ServiceGeneric<Person> personService;
    private ServiceGeneric<Monitoring> monitoringService;
    private ServiceGeneric<Plan> planService;
    private readonly ServiceLog logService;
    private readonly ServiceMailModel mailModelService;
    private readonly ServiceGeneric<MailMessage> mailMessageService;
    private readonly ServiceGeneric<MailLog> mailService;
    public string path;

    public BaseUser user { get => _user; set => user = _user; }

    public ServicePlan(DataContext context, string pathToken)
      : base(context)
    {
      try
      {
        monitoringService = new ServiceGeneric<Monitoring>(context);
        personService = new ServiceGeneric<Person>(context);
        planService = new ServiceGeneric<Plan>(context);
        logService = new ServiceLog(_context);
        mailModelService = new ServiceMailModel(context);
        mailMessageService = new ServiceGeneric<MailMessage>(context);
        mailService = new ServiceGeneric<MailLog>(context);
        path = pathToken;
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
        logService._user = _user;
        mailModelService._user = _user;
        mailMessageService._user = _user;
        mailService._user = _user;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private long GetBomb(int days)
    {
      try
      {
        if (days < 0)
          return -1;
        else
          return days;
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
          var detail = monitoringService.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
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
                  StatusPlanApproved = res.StatusPlanApproved,
                  TextEnd = res.TextEnd,
                  Status = res.Status,
                  DateEnd = res.DateEnd,
                  Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
                });
              }
            }
          }
        }

        if (schooling == 1)
        {
          var detailSchool = monitoringService.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
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
                  StatusPlanApproved = res.StatusPlanApproved,
                  TextEnd = res.TextEnd,
                  Status = res.Status,
                  DateEnd = res.DateEnd,
                  Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
                });
              }
            }
          }
        }

        if (skillcompany == 1)
        {
          var detailSkills = monitoringService.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
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
                  StatusPlanApproved = res.StatusPlanApproved,
                  TextEnd = res.TextEnd,
                  Status = res.Status,
                  DateEnd = res.DateEnd,
                  Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
                });
              }
            }
          }
        }

        if (open == 0)
          result = result.Where(p => !(p.StatusPlanApproved != EnumStatusPlanApproved.Approved & p.Deadline >= DateTime.Now)).ToList();

        if (expired == 0)
          result = result.Where(p => !(p.StatusPlanApproved != EnumStatusPlanApproved.Approved & p.Deadline < DateTime.Now)).ToList();

        if (end == 0)
          result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Approved).ToList();

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
          var detail = monitoringService.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
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
                  StatusPlanApproved = res.StatusPlanApproved,
                  TextEnd = res.TextEnd,
                  Status = res.Status,
                  DateEnd = res.DateEnd,
                  Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
                });
              }
            }
          }
        }

        if (schooling == 1)
        {
          var detailSchool = monitoringService.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
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
                  StatusPlanApproved = res.StatusPlanApproved,
                  TextEnd = res.TextEnd,
                  Status = res.Status,
                  DateEnd = res.DateEnd,
                  Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
                });
              }
            }
          }
        }

        if (skillcompany == 1)
        {
          var detailSkills = monitoringService.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person._id == id & p.Person.Name.ToUpper().Contains(filter.ToUpper()))
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
                  StatusPlanApproved = res.StatusPlanApproved,
                  TextEnd = res.TextEnd,
                  Status = res.Status,
                  DateEnd = res.DateEnd,
                  Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
                });
              }
            }
          }
        }

        if (open == 0)
          result = result.Where(p => !(p.StatusPlanApproved != EnumStatusPlanApproved.Approved & p.Deadline >= DateTime.Now)).ToList();

        if (expired == 0)
          result = result.Where(p => !(p.StatusPlanApproved != EnumStatusPlanApproved.Approved & p.Deadline < DateTime.Now)).ToList();

        if (end == 0)
          result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Approved).ToList();

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
                UpdatePlan(viewPlan, monitoring.Person.Manager);
                listActivities.Add(viewPlan);
              }
              else
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
                UpdatePlan(viewPlan, monitoring.Person.Manager);
                listSchoolings.Add(viewPlan);
              }
              else
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
                UpdatePlan(viewPlan, monitoring.Person.Manager);
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

    public string NewPlan(string idmonitoring, string idplanold, Plan viewPlan)
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
              if (plan._id == idplanold)
              {
                AddPlan(viewPlan, monitoring.Person);
                listActivities.Add(plan);
                listActivities.Add(viewPlan);
              }
              else
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
              if (plan._id == idplanold)
              {
                AddPlan(viewPlan, monitoring.Person);
                listSchoolings.Add(plan);
                listSchoolings.Add(viewPlan);
              }
              else
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
              if (plan._id == idplanold)
              {
                AddPlan(viewPlan, monitoring.Person);
                listSkillsCompany.Add(plan);
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

    public string NewPlanView(string idmonitoring, Plan planOld, Plan viewPlan)
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
              if (plan._id == planOld._id)
              {
                AddPlan(viewPlan, monitoring.Person);
                listActivities.Add(planOld);
                listActivities.Add(viewPlan);
              }
              else
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
              if (plan._id == planOld._id)
              {
                AddPlan(viewPlan, monitoring.Person);
                listSchoolings.Add(planOld);
                listSchoolings.Add(viewPlan);
              }
              else
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
              if (plan._id == planOld._id)
              {
                AddPlan(viewPlan, monitoring.Person);
                listSkillsCompany.Add(planOld);
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

    public string NewUpdatePlan(string idmonitoring, List<ViewPlanNewUp> viewPlan)
    {
      try
      {
        Plan planNew = new Plan();
        Plan planUpdate = new Plan();

        foreach (var item in viewPlan)
        {
          if (item.TypeViewPlan == EnumTypeViewPlan.New)
            planNew = new Plan()
            {
              _id = item._id,
              _idAccount = item._idAccount,
              Name = item.Name,
              Description = item.Description,
              Deadline = item.Deadline,
              Skills = item.Skills,
              UserInclude = item.UserInclude,
              DateInclude = item.DateInclude,
              TypePlan = item.TypePlan,
              SourcePlan = item.SourcePlan,
              TypeAction = item.TypeAction,
              StatusPlan = item.StatusPlan,
              TextEnd = item.TextEnd,
              DateEnd = item.DateEnd,
              Evaluation = item.Evaluation,
              Result = item.Result,
              StatusPlanApproved = item.StatusPlanApproved,
              Status = item.Status
            };
          else
            planUpdate = new Plan()
            {
              _id = item._id,
              _idAccount = item._idAccount,
              Name = item.Name,
              Description = item.Description,
              Deadline = item.Deadline,
              Skills = item.Skills,
              UserInclude = item.UserInclude,
              DateInclude = item.DateInclude,
              TypePlan = item.TypePlan,
              SourcePlan = item.SourcePlan,
              TypeAction = item.TypeAction,
              StatusPlan = item.StatusPlan,
              TextEnd = item.TextEnd,
              DateEnd = item.DateEnd,
              Evaluation = item.Evaluation,
              Result = item.Result,
              StatusPlanApproved = item.StatusPlanApproved,
              Status = item.Status
            };
        }

        NewPlanView(idmonitoring, planUpdate, planNew);

        return "newupdate";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private Plan AddPlan(Plan plan, Person person)
    {
      try
      {
        plan.DateInclude = DateTime.Now;
        plan.UserInclude = person;
        return planService.Insert(plan);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private string UpdatePlan(Plan plan, Person manager)
    {
      try
      {
        LogSave(manager._id, "Plan Process Update");

        if (plan.StatusPlanApproved == EnumStatusPlanApproved.Wait)
          Mail(manager);

        planService.Update(plan, null);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public ViewPlan GetPlan(string idmonitoring, string idplan)
    {
      try
      {
        var detail = monitoringService.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), Person = p.Person, _id = p._id }).FirstOrDefault();

        var detailSchoolings = monitoringService.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), Person = p.Person, _id = p._id }).FirstOrDefault();

        var detailSkillsCompany = monitoringService.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), Person = p.Person, _id = p._id }).FirstOrDefault();



        foreach (var plan in detail.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
              return new ViewPlan()
              {
                DateInclude = res.DateInclude,
                Deadline = res.Deadline,
                Name = res.Name,
                Description = res.Description,
                SourcePlan = res.SourcePlan,
                StatusPlan = res.StatusPlan,
                StatusPlanApproved = res.StatusPlanApproved,
                UserInclude = res.UserInclude,
                TypeAction = res.TypeAction,
                TypePlan = res.TypePlan,
                Evaluation = res.Evaluation,
                Skills = res.Skills,
                _id = res._id,
                _idAccount = res._idAccount,
                IdMonitoring = detail._id,
                TextEnd = res.TextEnd,
                Status = res.Status,
                DateEnd = res.DateEnd,
                Attachments = res.Attachments
              };
          }
        }

        foreach (var plan in detailSchoolings.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
              return new ViewPlan()
              {
                DateInclude = res.DateInclude,
                Deadline = res.Deadline,
                Name = res.Name,
                Description = res.Description,
                SourcePlan = res.SourcePlan,
                StatusPlan = res.StatusPlan,
                StatusPlanApproved = res.StatusPlanApproved,
                UserInclude = res.UserInclude,
                TypeAction = res.TypeAction,
                TypePlan = res.TypePlan,
                Evaluation = res.Evaluation,
                Skills = res.Skills,
                _id = res._id,
                _idAccount = res._idAccount,
                IdMonitoring = detail._id,
                TextEnd = res.TextEnd,
                Status = res.Status,
                DateEnd = res.DateEnd,
                Attachments = res.Attachments
              };
          }
        }

        foreach (var plan in detailSkillsCompany.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
              return new ViewPlan()
              {
                DateInclude = res.DateInclude,
                Deadline = res.Deadline,
                Name = res.Name,
                Description = res.Description,
                SourcePlan = res.SourcePlan,
                StatusPlan = res.StatusPlan,
                StatusPlanApproved = res.StatusPlanApproved,
                UserInclude = res.UserInclude,
                TypeAction = res.TypeAction,
                TypePlan = res.TypePlan,
                Evaluation = res.Evaluation,
                Skills = res.Skills,
                _id = res._id,
                _idAccount = res._idAccount,
                IdMonitoring = detail._id,
                TextEnd = res.TextEnd,
                Status = res.Status,
                DateEnd = res.DateEnd,
                Attachments = res.Attachments
              };
          }
        }

        return null;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public async void LogSave(string iduser, string local)
    {
      try
      {
        var user = personService.GetAll(p => p._id == iduser).FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "Access Plan ",
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
    // send mail
    public void Mail(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.OnBoardingApproval(path);
        var url = "";
        var body = model.Message.Replace("{Person}", person.Name).Replace("{Link}", model.Link);
        var message = new MailMessage
        {
          Type = EnumTypeMailMessage.Put,
          Name = model.Name,
          Url = url,
          Body = body
        };
        var idMessage = mailMessageService.Insert(message)._id;
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Suporte"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Mail, person.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.Name,
          Body = body,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = model.Subject
        };
        var mailObj = mailService.Insert(sendMail);
        var token = SendMail(path, person, mailObj._id.ToString());
        var messageEnd = mailMessageService.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        mailMessageService.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public string SendMail(string link, Person person, string idmail)
    {
      try
      {
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link);
          var data = new
          {
            mail = person.Mail,
            password = person.Password
          };
          var json = JsonConvert.SerializeObject(data);
          var content = new StringContent(json);
          content.Headers.ContentType.MediaType = "application/json";
          client.DefaultRequestHeaders.Add("ContentType", "application/json");
          var result = client.PostAsync("manager/authentication/encrypt", content).Result;
          var resultContent = result.Content.ReadAsStringAsync().Result;
          var auth = JsonConvert.DeserializeObject<ViewPerson>(resultContent);
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth.Token);
          var resultMail = client.PostAsync("mail/sendmail/" + idmail, null).Result;
          return auth.Token;
        }
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetAttachment(string id, string url, string fileName, string attachmentid)
    {
      try
      {
        Plan plan = planService.GetAll(p => p._id == id).FirstOrDefault();
        if (plan.Attachments == null)
        {
          plan.Attachments = new List<AttachmentField>();
        }

        plan.Attachments.Add(new AttachmentField { Url = url, Name = fileName, _idAttachment = id });

        planService.Update(plan, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
  }
}
