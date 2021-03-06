﻿using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
#pragma warning disable 1998
  public class ServicePlan : Repository<Plan>, IServicePlan
  {
    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceGeneric<Course> serviceCourse;
    private readonly ServiceLog serviceLog;
    private readonly ServiceLogMessages serviceLogMessages;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceMandatoryTraining serviceMandatoryTraining;
    private readonly ServiceGeneric<Monitoring> serviceMonitoring;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Plan> servicePlan;
    private readonly ServiceGeneric<PlanActivity> servicePlanActivity;
    private readonly ServiceGeneric<StructPlan> serviceStructPlan;
    private readonly ServiceGeneric<Skill> serviceSkill;
    private readonly ServiceGeneric<TrainingPlan> serviceTrainingPlans;
    private readonly ServiceGeneric<Attachments> serviceAttachment;
    private readonly ServiceGeneric<FluidCareers> serviceFluidCareers;
    private readonly IServiceControlQueue serviceControlQueue;

    public string path;

    #region Constructor
    public ServicePlan(DataContext context, DataContext contextLog, string pathToken, IServiceControlQueue _serviceControlQueue) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context, contextLog, _serviceControlQueue, pathToken);
        serviceCourse = new ServiceGeneric<Course>(context);
        serviceLog = new ServiceLog(context, contextLog);
        serviceLogMessages = new ServiceLogMessages(context);
        serviceMail = new ServiceGeneric<MailLog>(context);
        serviceMailModel = new ServiceMailModel(context);
        serviceMandatoryTraining = new ServiceMandatoryTraining(context);
        serviceMonitoring = new ServiceGeneric<Monitoring>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        servicePlan = new ServiceGeneric<Plan>(context);
        servicePlanActivity = new ServiceGeneric<PlanActivity>(context);
        serviceStructPlan = new ServiceGeneric<StructPlan>(context);
        serviceTrainingPlans = new ServiceGeneric<TrainingPlan>(context);
        serviceSkill = new ServiceGeneric<Skill>(context);
        serviceAttachment = new ServiceGeneric<Attachments>(context);
        serviceControlQueue = _serviceControlQueue;
        serviceFluidCareers = new ServiceGeneric<FluidCareers>(context);
        path = pathToken;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      try
      {
        User(contextAccessor);
        serviceCourse._user = _user;
        serviceLog.SetUser(_user);
        serviceLogMessages.SetUser(_user);
        serviceMail._user = _user;
        serviceMailModel.SetUser(_user);
        serviceMandatoryTraining.SetUser(_user);
        serviceMonitoring._user = _user;
        servicePerson._user = _user;
        serviceFluidCareers._user = _user;
        servicePlan._user = _user;
        servicePlanActivity._user = _user;
        serviceStructPlan._user = _user;
        serviceSkill._user = _user;
        serviceTrainingPlans._user = _user;
        serviceAttachment._user = _user;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(BaseUser user)
    {
      try
      {
        _user = user;
        serviceCourse._user = user;
        serviceLog.SetUser(user);
        serviceLogMessages.SetUser(user);
        serviceMail._user = user;
        serviceMailModel.SetUser(user);
        serviceMandatoryTraining.SetUser(user);
        serviceMonitoring._user = user;
        servicePerson._user = user;
        servicePlan._user = user;
        serviceSkill._user = _user;
        servicePlanActivity._user = user;
        serviceStructPlan._user = user;
        serviceTrainingPlans._user = user;
        serviceAttachment._user = _user;
        serviceFluidCareers._user = _user;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region Private

    private void SendQueue(string idplan, string idperson, int evaluation)
    {
      try
      {

        var data = new ViewCrudMaturityRegister
        {
          _idPerson = idperson,
          TypeMaturity = EnumTypeMaturity.Plan,
          _idRegister = idplan,
          Date = DateTime.Now,
          Evaluation = evaluation,
          _idAccount = _user._idAccount
        };
        serviceControlQueue.SendMessageAsync(JsonConvert.SerializeObject(data));

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void UpdatePlan(string idmonitoring, Plan viewPlan)
    {
      try
      {
        var monitoring = serviceMonitoring.GetNewVersion(p => p._id == idmonitoring).Result;
        var person = servicePerson.GetNewVersion(p => p._id == monitoring.Person._id).Result;

        if (viewPlan.StatusPlanApproved == EnumStatusPlanApproved.Approved)
          Task.Run(() => serviceLogMessages.NewLogMessage("Plano", " Ação de desenvolvimento dentro do prazo do colaborador " + person.User.Name, person));

        UpdatePlan(viewPlan, person);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private string NewPlanView(string idmonitoring, string iditem, Plan planOld, Plan viewPlan)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();
        var person = servicePerson.GetNewVersion(p => p._id == monitoring.Person._id).Result;

        AddPlan(viewPlan, person, monitoring._id, iditem);
        planOld._idMonitoring = idmonitoring;
        planOld._idItem = iditem;
        planOld.Person = person.GetViewListBaseManager();
        UpdatePlan(planOld, person);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
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

    private Plan AddPlan(Plan plan, Person person, string _idmonitoring, string _iditem)
    {
      try
      {
        plan.DateInclude = DateTime.Now;
        plan._idMonitoring = _idmonitoring;
        plan._idItem = _iditem;
        plan.Person = person.GetViewListBaseManager();
        var verify = servicePlan.GetNewVersion(p => p._id == plan._id).Result;

        if (verify == null)
          servicePlan.InsertNewVersion(plan);
        else
          servicePlan.Update(plan, null);

        return plan;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private ViewCrudStructPlan AddStructPlan(StructPlan structPlan)
    {
      try
      {
        return serviceStructPlan.InsertNewVersion(structPlan).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void UpdatePlan(Plan plan, Person person)
    {
      try
      {

        Task.Run(() => SendQueue(plan._id, plan.Person._id, plan.Evaluation));

        Task.Run(() => LogSave(_user._idPerson, "Plan Process Update"));
        if (plan.StatusPlanApproved == EnumStatusPlanApproved.Wait)
        {
          if (plan.Person._id == _user._idPerson)
            Task.Run(() => Mail(person, person.Manager.Name, person.Manager.Mail));
          else
            Task.Run(() => Mail(person, person.User.Name, person.User.Mail));
        }


        servicePlan.Update(plan, null).Wait();
        //return "Plan altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void LogSave(string iduser, string local)
    {
      try
      {
        var user = servicePerson.GetAllNewVersion(p => p._id == iduser).Result.FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "Access Plan ",
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
    // send mail
    private void Mail(Person person, string namereceived, string mailreceived)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.PlanApproval(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;
        //string managername = person.Manager?.Name;
        var body = model.Message.Replace("{Person}", person.User.Name)
                                .Replace("{Link}", model.Link)
                                .Replace("{Manager}", namereceived)
                                .Replace("{Company}", person.Company.Name)
                                .Replace("{Occupation}", person.Occupation.Name);
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(mailreceived, namereceived)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
          Body = body,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = model.Subject
        };
        var mailObj = serviceMail.InsertNewVersion(sendMail).Result;
        var token = SendMail(path, person, mailObj._id.ToString());
      }
      catch (Exception)
      {
        //throw e;
      }
    }
    private string SendMail(string link, Person person, string idmail)
    {
      try
      {
        string token = serviceAuthentication.AuthenticationMail(person);
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link.Substring(0, link.Length - 1) + ":5201/");
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
          var resultMail = client.PostAsync("sendmail/" + idmail, null).Result;
          return token;
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }



    #endregion

    #region Plan

    public List<ViewGetPersonPlan> GetPersonPlan(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var monitorings = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End).Result.Select(p => p._id).ToList();
        var ids = servicePlan.GetAllNewVersion(p => monitorings.Contains(p._idMonitoring)
        && p.StatusPlanApproved != EnumStatusPlanApproved.Invisible).Result
          .Select(p => p.Person?._id).ToList();

        total = servicePerson.CountNewVersion(p => ids.Contains(p._id)
        && p.TypeJourney != EnumTypeJourney.OutOfJourney
        && p.Manager._id == idmanager
        && p.StatusUser != EnumStatusUser.Disabled).Result;

        return servicePerson.GetAllNewVersion(p => ids.Contains(p._id)
        && p.TypeJourney != EnumTypeJourney.OutOfJourney
        && p.StatusUser != EnumStatusUser.Disabled
        && p.Manager._id == idmanager
        && p.User.Name.Contains(filter)
        ).Result.
          Select(p => new ViewGetPersonPlan()
          {
            _id = p._id,
            Name = p.User?.Name
          }).Skip(skip).Take(count).OrderBy(p => p.Name).ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdatePlan(string idmonitoring, ViewCrudPlan viewPlan)
    {
      try
      {
        var plan = servicePlan.GetNewVersion(p => p._id == viewPlan._id).Result;
        plan.Deadline = viewPlan.Deadline;
        plan.Description = viewPlan.Description;
        plan.Name = viewPlan.Name;
        plan.DateEnd = DateTime.Now;
        plan.TypeAction = viewPlan.TypeAction;
        plan.Attachments = viewPlan.Attachments;
        plan.StatusPlan = viewPlan.StatusPlan;
        plan.NewAction = viewPlan.NewAction;
        plan.TextEnd = viewPlan.TextEnd;
        plan.TextEndManager = viewPlan.TextEndManager;
        plan.Evaluation = viewPlan.Evaluation;
        plan.StatusPlanApproved = viewPlan.StatusPlanApproved;
        plan.Skills = viewPlan.Skills;
        plan.TypePlan = viewPlan.TypePlan;

        Task.Run(() => UpdatePlan(idmonitoring, plan));
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemoveStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan)
    {
      try
      {
        var plan = servicePlan.GetAllNewVersion(p => p._id == idplan).Result.FirstOrDefault();

        foreach (var structplan in plan.StructPlans)
        {
          if (structplan._id == idstructplan)
          {
            plan.StructPlans.Remove(structplan);
            servicePlan.Update(plan, null).Wait();
            return "update";
          }
        }

        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string RemovePlanActivity(string id)
    {
      try
      {
        var model = servicePlanActivity.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
        model.Status = EnumStatus.Disabled;
        servicePlanActivity.Update(model, null).Wait();
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetAttachment(string idplan, string idmonitoring, string url, string fileName, string attachmentid)
    {
      try
      {

        var monitoring = serviceMonitoring.GetNewVersion(p => p._id == idmonitoring).Result;
        var res = servicePlan.GetNewVersion(p => p._id == idplan).Result;

        if (res.Attachments == null)
          res.Attachments = new List<ViewCrudAttachmentField>();

        res.Attachments.Add(new ViewCrudAttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });

        UpdatePlan(idmonitoring, res);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewPlanActivity> ListPlanActivity(ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewPlan> result = new List<ViewPlan>();

        var detail = servicePlanActivity.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result.Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
        total = servicePlanActivity.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewPlanActivity()
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

    public ViewPlanActivity GetPlanActivity(string id)
    {
      try
      {
        return servicePlanActivity.GetAllNewVersion(p => p._id == id)
          .Result.Select(p => new ViewPlanActivity()
          {
            _id = p._id,
            Name = p.Name
          }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewPlanActivity(ViewPlanActivity model)
    {
      try
      {
        servicePlanActivity.InsertNewVersion(new PlanActivity() { Name = model.Name, Status = EnumStatus.Enabled }).Wait();
        return "add plan activity";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdatePlanActivity(ViewPlanActivity model)
    {
      try
      {
        servicePlanActivity.Update(new PlanActivity()
        {
          _id = model._id,
          Name = model.Name,
          Status = EnumStatus.Enabled,
          _idAccount = _user._idAccount
        }, null).Wait();
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPlanStruct> ListPlansStruct(ref long total, string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte structplan)
    {
      try
      {
        int skip = (count * (page - 1));


        List<ViewListPlanStruct> result = new List<ViewListPlanStruct>();

        var plan = servicePlan.GetAllNewVersion().ToList();

        if (activities == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Activite).ToList();

        if (skillcompany == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Skill).ToList();

        if (schooling == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Schooling).ToList();

        foreach (var res in plan)
        {
          var view = new ViewListPlanStruct
          {
            _id = res._id,
            Name = res.Name,
            DateInclude = res.DateInclude,
            Deadline = res.Deadline,
            Description = res.Description,
            Skills = res.Skills?.OrderBy(p => p.Name)
          .Select(p => new ViewListSkill()
          {
            _id = p._id,
            Name = p.Name,
            Concept = p.Concept,
            TypeSkill = p.TypeSkill
          })
          .ToList(),
            UserInclude = res.Person == null ? null : servicePerson.GetAllNewVersion(p => p._id == res.Person._id).Result.FirstOrDefault()?._id,
            TypePlan = res.TypePlan,
            IdPerson = res.Person?._id,
            NamePerson = res.Person?.Name,
            SourcePlan = res.SourcePlan,
            _idMonitoring = res._idMonitoring,
            Evaluation = res.Evaluation,
            StatusPlan = res.StatusPlan,
            TypeAction = res.TypeAction,
            StatusPlanApproved = res.StatusPlanApproved,
            TextEnd = res.TextEnd,
            TextEndManager = res.TextEndManager,
            Status = res.Status,
            DateEnd = res.DateEnd,
            NewAction = res.NewAction
          };
          if (res.StructPlans != null)
          {
            if (res.StructPlans.Count() == 0)
              view.StructPlans = null;
            else
              view.StructPlans = res.StructPlans?.Select(p => new ViewCrudStructPlan()
              {
                _id = p._id,
                TypeResponsible = p.TypeResponsible,
                PlanActivity = p.PlanActivity,
                TypeAction = p.TypeAction,
                Course = (p.Course == null) ? null : new ViewListCourse() { _id = p.Course._id, Name = p.Course.Name }

              }).ToList();
          }
          result.Add(view);
        }

        if (structplan == 0)
        {
          try
          {
            result = result.Where(p => p.StructPlans.Count() == 0).ToList();
          }
          catch (Exception)
          {
            result = result.Where(p => p.StructPlans == null).ToList();
          }
        }


        result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Invisible).ToList();

        total = result.Count();

        return result.Skip(skip).Take(count).OrderBy(p => p.SourcePlan).ThenBy(p => p.Deadline).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewGetPlan> ListPlans(string id, string idperson, ref long total, string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewGetPlan> result = new List<ViewGetPlan>();

        var plan = servicePlan.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        var persons = servicePerson.GetAllNewVersion(p => p.Manager._id == id
        && p.StatusUser != EnumStatusUser.Disabled
        && p.User.Name.ToUpper().Contains(filter.ToUpper())).Result.Select(p => p._id).ToList();
        if (idperson != "")
        {
          persons = new List<string>();
          persons.Add(idperson);
        }


        foreach (var item in plan)
        {
          if (persons.Where(p => p == item.Person?._id).Count() == 0)
            plan.Where(p => p._id == item._id).FirstOrDefault().Status = EnumStatus.Disabled;
        }

        if (activities == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Activite).ToList();

        if (skillcompany == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Skill).ToList();

        if (schooling == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Schooling).ToList();

        foreach (var res in plan)
        {
          var monitoring = serviceMonitoring.GetNewVersion(p => p._id == res._idMonitoring).Result;
          if (monitoring != null)
          {
            if (monitoring.StatusMonitoring == EnumStatusMonitoring.End)
              result.Add(new ViewGetPlan()
              {
                _id = res._id,
                Name = res.Name,
                DateInclude = res.DateInclude,
                Deadline = res.Deadline,
                Description = res.Description,
                Skills = res.Skills?.Select(p => new ViewListSkill()
                {
                  _id = p._id,
                  TypeSkill = p.TypeSkill,
                  Concept = p.Concept,
                  Name = p.Name
                }).ToList(),
                UserInclude = res.Person?._id,
                TypePlan = res.TypePlan,
                _idPerson = res.Person?._id,
                NamePerson = res.Person?.Name,
                SourcePlan = res.SourcePlan,
                IdMonitoring = res._idMonitoring,
                Evaluation = res.Evaluation,
                StatusPlan = res.StatusPlan,
                TypeAction = res.TypeAction,
                StatusPlanApproved = res.StatusPlanApproved,
                TextEnd = res.TextEnd,
                TextEndManager = res.TextEndManager,
                Status = res.Status,
                DateEnd = res.DateEnd,
                NewAction = res.NewAction,
                Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
              });
          }
        }


        result = result.Where(p => p.Status != EnumStatus.Disabled && p.StatusPlanApproved != EnumStatusPlanApproved.Invisible).ToList();

        if (open == 0)
          result = result.Where(p => !(p.StatusPlanApproved == EnumStatusPlanApproved.Open & p.Deadline > DateTime.Now)).ToList();

        if (expired == 0)
          result = result.Where(p => !(p.StatusPlanApproved == EnumStatusPlanApproved.Open & p.Deadline < DateTime.Now)).ToList();

        if (end == 0)
          result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Approved).ToList();

        if (wait == 0)
          result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Wait).ToList();


        total = result.Count();

        return result.OrderBy(p => p.StatusPlanApproved).ThenBy(p => p.StatusPlan).ThenBy(p => p.Deadline).Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewGetPlan> ListPlansPerson(string id, ref long total, string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewGetPlan> result = new List<ViewGetPlan>();

        var plan = servicePlan.GetAllNewVersion(p => p.Person._id == id).Result.ToList();

        if (activities == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Activite).ToList();

        if (skillcompany == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Skill).ToList();

        if (schooling == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Schooling).ToList();

        foreach (var res in plan)
        {
          var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == res._idMonitoring).Result.FirstOrDefault();
          if (monitoring != null)
          {
            if (monitoring.StatusMonitoring == EnumStatusMonitoring.End)
              result.Add(new ViewGetPlan()
              {
                _id = res._id,
                Name = res.Name,
                DateInclude = res.DateInclude,
                Deadline = res.Deadline,
                Description = res.Description,
                Skills = res.Skills?.Select(p => new ViewListSkill()
                {
                  _id = p._id,
                  TypeSkill = p.TypeSkill,
                  Concept = p.Concept,
                  Name = p.Name
                }).ToList(),
                UserInclude = res.Person?._id,
                TypePlan = res.TypePlan,
                _idPerson = res.Person._id,
                NamePerson = res.Person.Name,
                SourcePlan = res.SourcePlan,
                IdMonitoring = res._idMonitoring,
                Evaluation = res.Evaluation,
                StatusPlan = res.StatusPlan,
                TypeAction = res.TypeAction,
                StatusPlanApproved = res.StatusPlanApproved,
                TextEnd = res.TextEnd,
                TextEndManager = res.TextEndManager,
                Status = res.Status,
                DateEnd = res.DateEnd,
                NewAction = res.NewAction,
                Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days),
                OriginPlan = EnumOriginPlan.Monitoring
              });
          }

        }



        result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Invisible).ToList();

        var planFluidCarrers = serviceFluidCareers.GetAllNewVersion(p => p.Person._id == id).Result;
        foreach (var item in planFluidCarrers)
        {
          if (item.Plan != null)
          {
            var view = new ViewGetPlan()
            {
              _id = item._id,
              Name = item.Plan.What,
              NamePerson = item.Person.Name,
              Deadline = item.Plan.Date,
              Description = item.Plan.What,
              Skills = item.Plan.Skills,
              OriginPlan = EnumOriginPlan.FluidCareers,
              _idPerson = item.Person._id,
              UserInclude = item.Person._id,
              TextEnd = item.Plan.Observation,
              StatusPlanApproved = item.Plan.StatusFluidCareerPlan == EnumStatusFluidCareerPlan.Open ?
            EnumStatusPlanApproved.Open : item.Plan.StatusFluidCareerPlan == EnumStatusFluidCareerPlan.Realized ?
            EnumStatusPlanApproved.Approved : EnumStatusPlanApproved.Invisible,
              StatusPlan = item.Plan.StatusFluidCareerPlan == EnumStatusFluidCareerPlan.Open ?
            EnumStatusPlan.Open : item.Plan.StatusFluidCareerPlan == EnumStatusFluidCareerPlan.Realized ?
            EnumStatusPlan.Realized : EnumStatusPlan.NoRealized
            };
            result.Add(view);
          }
        }

        if (open == 0)
          result = result.Where(p => !(p.StatusPlanApproved == EnumStatusPlanApproved.Open & p.Deadline > DateTime.Now)).ToList();

        if (expired == 0)
          result = result.Where(p => !(p.StatusPlanApproved == EnumStatusPlanApproved.Open & p.Deadline < DateTime.Now)).ToList();

        if (end == 0)
          result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Approved).ToList();

        if (wait == 0)
          result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Wait).ToList();


        total = result.Count();

        //return result.Skip(skip).Take(count).OrderBy(p => p.SourcePlan).ThenBy(p => p.Deadline).ToList();
        return result.OrderBy(p => p.StatusPlan).ThenBy(p => p.Deadline).Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }

    }

    public ViewListPlanStruct GetPlanStruct(string idmonitoring, string idplan)
    {
      try
      {
        var plan = servicePlan.GetNewVersion(p => p._id == idplan).Result;

        if (plan == null)
          return null;

        var view = new ViewListPlanStruct()
        {
          _id = plan._id,
          Description = plan.Description,
          Deadline = plan.Deadline,
          Skills = plan.Skills,
          UserInclude = plan.Person?._id,
          DateInclude = plan.DateInclude,
          TypePlan = plan.TypePlan,
          IdPerson = plan.Person?._id,
          NamePerson = plan.Person?.Name,
          SourcePlan = plan.SourcePlan,
          _idMonitoring = plan._idMonitoring,
          Evaluation = plan.Evaluation,
          StatusPlan = plan.StatusPlan,
          TypeAction = plan.TypeAction,
          StatusPlanApproved = plan.StatusPlanApproved,
          TextEnd = plan.TextEnd,
          TextEndManager = plan.TextEndManager,
          DateEnd = plan.DateEnd,
          Status = plan.Status,
          Attachments = plan.Attachments,
          NewAction = plan.NewAction,
          StructPlans = plan.StructPlans,
          Name = plan.Name
        };

        view.PlanNew = servicePlan.GetNewVersion(p => p._id == idmonitoring && p._id == plan._idItem && p.Name == plan.Name &
        p._id != plan._id).Result.GetViewCrud();

        return view;

      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewPlanShort> ListPlansPerson(string id, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewGetPlan> result = new List<ViewGetPlan>();

        List<Plan> plan = servicePlan.GetAllNewVersion(p => p.Person._id == id).Result.ToList();

        foreach (Plan res in plan)
        {
          result.Add(new ViewGetPlan()
          {
            _id = res._id,
            Name = res.Name,
            DateInclude = res.DateInclude,
            Deadline = res.Deadline,
            Description = res.Description,
            Skills = res.Skills?.Select(p => new ViewListSkill()
            {
              _id = p._id,
              TypeSkill = p.TypeSkill,
              Concept = p.Concept,
              Name = p.Name
            }).ToList(),
            UserInclude = res.Person?._id,
            TypePlan = res.TypePlan,
            _idPerson = res.Person._id,
            NamePerson = res.Person.Name,
            SourcePlan = res.SourcePlan,
            IdMonitoring = res._idMonitoring,
            Evaluation = res.Evaluation,
            StatusPlan = res.StatusPlan,
            TypeAction = res.TypeAction,
            StatusPlanApproved = res.StatusPlanApproved,
            TextEnd = res.TextEnd,
            TextEndManager = res.TextEndManager,
            Status = res.Status,
            DateEnd = res.DateEnd,
            NewAction = res.NewAction,
            Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
          });
        }


        result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Invisible).ToList();

        total = result.Count();

        result.Skip(skip).Take(count).OrderBy(p => p.StatusPlan).ThenBy(p => p.Deadline).ToList();

        result = result.Skip(skip).Take(count).OrderBy(p => p.SourcePlan).ThenBy(p => p.Deadline).ToList();
        var viewReturn = result.GroupBy(i => i.Name).Select(g => new ViewPlanShort
        {
          Name = g.Key,
          LastAction = g.Max(row => row.Deadline)
        }).ToList();

        return viewReturn;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewPlanShort> ListPlans(string id, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewGetPlan> result = new List<ViewGetPlan>();

        List<Plan> plan = servicePlan.GetAllNewVersion(p => p.Person._idManager == id).Result.ToList();

        foreach (var res in plan)
        {
          result.Add(new ViewGetPlan()
          {
            _id = res._id,
            Name = res.Name,
            DateInclude = res.DateInclude,
            Deadline = res.Deadline,
            Description = res.Description,
            Skills = res.Skills?.Select(p => new ViewListSkill()
            {
              _id = p._id,
              TypeSkill = p.TypeSkill,
              Concept = p.Concept,
              Name = p.Name
            }).ToList(),
            UserInclude = res.Person?._id,
            TypePlan = res.TypePlan,
            _idPerson = res.Person._id,
            NamePerson = res.Person.Name,
            SourcePlan = res.SourcePlan,
            IdMonitoring = res._idMonitoring,
            Evaluation = res.Evaluation,
            StatusPlan = res.StatusPlan,
            TypeAction = res.TypeAction,
            StatusPlanApproved = res.StatusPlanApproved,
            TextEnd = res.TextEnd,
            TextEndManager = res.TextEndManager,
            Status = res.Status,
            DateEnd = res.DateEnd,
            NewAction = res.NewAction,
            Bomb = GetBomb((DateTime.Parse(res.Deadline.ToString()) - DateTime.Now).Days)
          });
        }

        result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Invisible).ToList();

        total = result.Count();
        result = result.Skip(skip).Take(count).OrderBy(p => p.StatusPlanApproved).ThenBy(p => p.StatusPlan).ThenBy(p => p.Deadline).ToList();
        var viewReturn = result.GroupBy(i => i.Name).Select(g => new ViewPlanShort
        {
          Name = g.Key,
          LastAction = g.Max(row => row.Deadline)
        }).ToList();

        return viewReturn;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewPlan(string idmonitoring, string idplanold, ViewCrudPlan view)
    {
      try
      {
        var monitoring = serviceMonitoring.GetNewVersion(p => p._id == idmonitoring).Result;
        var person = servicePerson.GetNewVersion(p => p._id == monitoring.Person._id).Result;
        var plan = servicePlan.GetNewVersion(p => p._id == view._id).Result;


        var viewPlan = new Plan()
        {
          _id = view._id,
          Name = view.Name,
          Description = view.Description,
          Deadline = view.Deadline,
          Skills = view.Skills,
          TypePlan = view.TypePlan,
          SourcePlan = view.SourcePlan
        };


        AddPlan(viewPlan, person, monitoring._id, plan._idItem);

        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, ViewCrudStructPlan structplan)
    {
      try
      {
        var plan = servicePlan.GetNewVersion(p => p._id == idplan).Result;
        DateTime? deadline = DateTime.Now;

        plan.StructPlans.Add(AddStructPlan(new StructPlan()
        {
          _id = structplan._id,
          Course = structplan.Course,
          PlanActivity = structplan.PlanActivity,
          TypeAction = structplan.TypeAction,
          TypeResponsible = structplan.TypeResponsible
        }));

        if (structplan.Course != null)
        {
          var trainingPlan = new TrainingPlan
          {
            Course = structplan.Course,
            Deadline = deadline,
            Origin = EnumOrigin.Monitoring,
            Person = (plan.Person == null) ? null : servicePerson.GetAllNewVersion(p => p._id == plan.Person._id).Result.FirstOrDefault().GetViewListManager(),
            Include = DateTime.Now,
            StatusTrainingPlan = EnumStatusTrainingPlan.Open
          };
          serviceMandatoryTraining.NewTrainingPlanInternal(trainingPlan);
        }

        servicePlan.Update(plan, null).Wait();
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudStructPlan GetStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan)
    {
      try
      {
        var structplan = servicePlan.GetAllNewVersion(p => p._id == idplan).Result.FirstOrDefault().StructPlans.Where(p => p._id == idstructplan).FirstOrDefault();

        if (structplan == null)
          return null;

        return new ViewCrudStructPlan()
        {
          _id = structplan._id,
          Course = (structplan.Course == null) ? null : new ViewListCourse()
          {
            _id = structplan.Course._id,
            Name = structplan.Course.Name
          },
          PlanActivity = (structplan.PlanActivity == null) ? null : new ViewPlanActivity() { _id = structplan._id, Name = structplan.PlanActivity.Name },
          TypeAction = structplan.TypeAction,
          TypeResponsible = structplan.TypeResponsible
        };

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateStructPlan(string idmonitoring, string idplan, EnumSourcePlan sourceplan, ViewCrudStructPlan structplanedit)
    {
      try
      {
        var plan = servicePlan.GetAllNewVersion(p => p._id == idplan).Result.FirstOrDefault();

        foreach (var structplan in plan.StructPlans)
        {
          if (structplan._id == structplanedit._id)
          {

            if ((structplan.Course == null) & (structplanedit.Course != null))
            {
              var trainingPlan = new TrainingPlan
              {
                Course = new ViewListCourse()
                {
                  _id = structplan.Course?._id,
                  Name = structplan.Course?.Name
                } ?? null,
                Deadline = plan.Deadline,
                Origin = EnumOrigin.Monitoring,
                Person = servicePerson.GetAllNewVersion(p => p._id == plan.Person._id).Result.FirstOrDefault().GetViewListManager(),
                Include = DateTime.Now,
                StatusTrainingPlan = EnumStatusTrainingPlan.Open
              };
              if (serviceTrainingPlans.GetAllNewVersion(p => p.Person == trainingPlan.Person
               & p.Course == trainingPlan.Course & p.Origin == EnumOrigin.Monitoring & p.Deadline == trainingPlan.Deadline).Result.Count() == 0)
              {
                serviceMandatoryTraining.NewTrainingPlanInternal(trainingPlan);
              }
            }

            plan.StructPlans.Remove(structplan);
            plan.StructPlans.Add(new ViewCrudStructPlan()
            {
              _id = structplanedit._id,
              Course = structplanedit.Course,
              PlanActivity = structplanedit.PlanActivity,
              TypeAction = structplanedit.TypeAction,
              TypeResponsible = structplanedit.TypeResponsible
            });
            servicePlan.Update(plan, null).Wait();
            return "update";
          }
        }

        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    //public class ObjTest { public int campo { get; set; } }

    //public void test()
    //{
    //  var list = new List<ObjTest>();
    //  list.Add(new ObjTest() { campo = 1 });
    //  var itens = new int[3] { 1, 2, 3 };
    //  //in
    //  list.Where(p => !itens.Contains(p.campo));
    //  //not in
    //  list.Where(p => itens.Contains(p.campo));
    //}

    public string NewUpdatePlan(string idmonitoring, List<ViewCrudNewPlanUp> viewPlan)
    {
      try
      {

        Plan planNew = new Plan()
        {
          _id = viewPlan[1]._id,
          _idAccount = _user._idAccount,
          Name = viewPlan[1].Name,
          Description = viewPlan[1].Description,
          Deadline = viewPlan[1].Deadline,
          Skills = viewPlan[1].Skills,
          DateInclude = viewPlan[1].DateInclude,
          TypePlan = viewPlan[1].TypePlan,
          SourcePlan = viewPlan[1].SourcePlan,
          TypeAction = viewPlan[1].TypeAction,
          StatusPlan = viewPlan[1].StatusPlan,
          TextEnd = viewPlan[1].TextEnd,
          TextEndManager = viewPlan[1].TextEndManager,
          DateEnd = viewPlan[1].DateEnd,
          Evaluation = viewPlan[1].Evaluation,
          Result = viewPlan[1].Result,
          StatusPlanApproved = EnumStatusPlanApproved.Open,
          Status = viewPlan[1].Status,
          NewAction = viewPlan[1].NewAction,
          Attachments = viewPlan[1].Attachments
        };
        Plan planUpdate = new Plan()
        {
          _id = viewPlan[0]._id,
          _idAccount = _user._idAccount,
          Name = viewPlan[0].Name,
          Description = viewPlan[0].Description,
          Deadline = viewPlan[0].Deadline,
          Skills = viewPlan[0].Skills,
          DateInclude = viewPlan[0].DateInclude,
          TypePlan = viewPlan[0].TypePlan,
          SourcePlan = viewPlan[0].SourcePlan,
          TypeAction = viewPlan[0].TypeAction,
          StatusPlan = viewPlan[0].StatusPlan,
          TextEnd = viewPlan[0].TextEnd,
          TextEndManager = viewPlan[0].TextEndManager,
          DateEnd = viewPlan[0].DateEnd,
          Evaluation = viewPlan[0].Evaluation,
          Result = viewPlan[0].Result,
          StatusPlanApproved = viewPlan[0].StatusPlanApproved,
          Status = viewPlan[0].Status,
          NewAction = viewPlan[0].NewAction,
          Attachments = viewPlan[0].Attachments
        };

        var monitoring = serviceMonitoring.GetNewVersion(p => p._id == idmonitoring).Result;
        var person = servicePerson.GetNewVersion(p => p._id == monitoring.Person._id).Result;
        var plan = servicePlan.GetNewVersion(p => p._id == planUpdate._id).Result;


        if (_user._idUser == person.User._id)
        {
          planNew.StatusPlanApproved = EnumStatusPlanApproved.Invisible;
          NewPlanView(idmonitoring, plan._idItem, planUpdate, planNew);
        }
        else
        {
          if (viewPlan[0].NewAction == EnumNewAction.Yes)
          {
            NewPlanView(idmonitoring, plan._idItem, planUpdate, planNew);
          }
          else
            Task.Run(() => UpdatePlan(idmonitoring, planUpdate));
        }
        return "newupdate";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewGetPlan GetPlan(string idmonitoring, string idplan)
    {
      try
      {
        var plan = servicePlan.GetNewVersion(p => p._id == idplan).Result;
        var resultPlan = new ViewGetPlan()
        {
          DateInclude = plan.DateInclude,
          Deadline = plan.Deadline,
          Name = plan.Name,
          Description = plan.Description,
          SourcePlan = plan.SourcePlan,
          StatusPlan = plan.StatusPlan,
          StatusPlanApproved = plan.StatusPlanApproved,
          UserInclude = plan.Person?._id,
          TypeAction = plan.TypeAction,
          TypePlan = plan.TypePlan,
          Evaluation = plan.Evaluation,
          Skills = plan.Skills?.OrderBy(x => x.Name).Select(x =>
          new ViewListSkill()
          {
            _id = x._id,
            Name = x.Name,
            Concept = x.Concept,
            TypeSkill = x.TypeSkill
          }).ToList(),
          _id = plan._id,
          IdMonitoring = plan._idMonitoring,
          TextEnd = plan.TextEnd,
          TextEndManager = plan.TextEndManager,
          Status = plan.Status,
          DateEnd = plan.DateEnd,
          Attachments = plan.Attachments?.Select(x => new ViewCrudAttachmentField()
          {
            Name = x.Name,
            Url = x.Url,
            _idAttachment = x._idAttachment
          }).ToList(),
          NewAction = plan.NewAction,
          _idPerson = plan.Person._id,
          NamePerson = plan.Person.Name,
          Bomb = GetBomb((DateTime.Parse(plan.Deadline.ToString()) - DateTime.Now).Days)
        };

        resultPlan.PlanNew = servicePlan.GetAllNewVersion(p => p._idMonitoring == idmonitoring
        && p._idItem == plan._idItem && p._id != idplan).Result.Select(p => new ViewCrudPlan()
        {
          _id = p._id,
          Name = p.Name,
          Description = p.Description,
          Deadline = p.Deadline,
          Skills = p.Skills,
          TypePlan = p.TypePlan,
          SourcePlan = p.SourcePlan,
          StatusPlan = p.StatusPlan,
          StatusPlanApproved = p.StatusPlanApproved,
          TypeAction = p.TypeAction,
          Attachments = p.Attachments,
          NewAction = p.NewAction,
          TextEnd = p.TextEnd,
          TextEndManager = p.TextEndManager,
          Evaluation = byte.Parse(p.Evaluation.ToString())
        })
        .FirstOrDefault();

        return resultPlan;
      }
      catch (Exception e)
      {
        throw e;
      }
    }





    public List<ViewExportStatusPlan> ExportStatusPlan(ViewFilterIdAndDate filter)
    {
      try
      {
        var monitorings = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
         && p.DateEndEnd >= filter.Date.Begin && p.DateEndEnd <= filter.Date.End).Result;

        var list = new List<ViewExportStatusPlan>();
        foreach (var item in filter.Persons)
        {
          var result = servicePlan.GetAllNewVersion(p => p.Person._id == item._id).Result;
          foreach (var p in result)
          {
            var count = monitorings.Where(x => x._id == p._idMonitoring).Count();
            if (count > 0)
            {
              list.Add(new ViewExportStatusPlan()
              {
                NameManager = p.Person.NameManager ?? "Sem Gestor",
                NamePerson = p.Person.Name,
                What = p.Name,
                Description = p.Description,
                Deadline = p.Deadline,
                Status = p.StatusPlan == EnumStatusPlan.Realized ? "Realizado" :
                        p.StatusPlan == EnumStatusPlan.NoRealized ? "Não Realizado" : "Em aberto",
                Obs = p.TextEnd,
                DateEnd = p.DateEnd
              });
            }
          }

        };


        return list;
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
