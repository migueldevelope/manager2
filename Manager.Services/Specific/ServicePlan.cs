using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
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
    private readonly IServiceControlQueue serviceControlQueue;

    public string path;

    #region Constructor
    public ServicePlan(DataContext context, DataContext contextLog, string pathToken, IServiceControlQueue _serviceControlQueue) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context, contextLog);
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
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region Private

    private async Task SendQueue(string idplan, string idperson, byte evaluation)
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

    private string UpdatePlan(string idmonitoring, Plan viewPlan)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        if (viewPlan.StatusPlanApproved == EnumStatusPlanApproved.Approved)
          serviceLogMessages.NewLogMessage("Plano", " Ação de desenvolvimento dentro do prazo do colaborador " + monitoring.Person.User.Name, monitoring.Person);


        //verify plan;
        if (viewPlan.SourcePlan == EnumSourcePlan.Activite)
        {
          foreach (var item in monitoring.Activities)
          {
            var listActivities = new List<ViewCrudPlan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == viewPlan._id)
              {
                Task.Run(() => UpdatePlan(viewPlan, monitoring.Person));
                listActivities.Add(new ViewCrudPlan()
                {
                  _id = viewPlan._id,
                  Name = viewPlan.Name,
                  Description = viewPlan.Description,
                  Deadline = viewPlan.Deadline,
                  Skills = viewPlan.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = viewPlan.SourcePlan,
                  TypePlan = viewPlan.TypePlan
                });
              }
              else if ((plan.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (viewPlan.NewAction == EnumNewAction.Yes))
              {
                if (viewPlan.NewAction == EnumNewAction.Yes)
                  plan.StatusPlanApproved = EnumStatusPlanApproved.Open;

                var upPlan = servicePlan.GetAll(p => p._id == plan._id).FirstOrDefault();
                upPlan.Description = plan.Description;
                upPlan.Deadline = plan.Deadline;
                upPlan.Skills = new List<Skill>();
                foreach (var skill in plan.Skills)
                  upPlan.Skills.Add(serviceSkill.GetAll(p => p._id == skill._id).FirstOrDefault());
                upPlan.TypePlan = plan.TypePlan;
                upPlan.SourcePlan = plan.SourcePlan;
                upPlan.StatusPlan = plan.StatusPlan;
                upPlan.StatusPlanApproved = plan.StatusPlanApproved;
                upPlan.TypeAction = plan.TypeAction;
                upPlan.Attachments = new List<AttachmentField>();
                foreach (var attachments in plan.Attachments)
                  upPlan.Attachments.Add(new AttachmentField()
                  {
                    _idAttachment = attachments._idAttachment,
                    Name = attachments.Name,
                    Url = attachments.Url
                  });
                upPlan.NewAction = plan.NewAction;
                upPlan.TextEnd = plan.TextEnd;
                upPlan.TextEndManager = plan.TextEndManager;
                upPlan.Evaluation = plan.Evaluation;


                Task.Run(() => UpdatePlan(upPlan, monitoring.Person));
                listActivities.Add(plan);
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
            var listSchoolings = new List<ViewCrudPlan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == viewPlan._id)
              {
                Task.Run(() => UpdatePlan(viewPlan, monitoring.Person));
                listSchoolings.Add(new ViewCrudPlan()
                {
                  _id = viewPlan._id,
                  Name = viewPlan.Name,
                  Description = viewPlan.Description,
                  Deadline = viewPlan.Deadline,
                  Skills = viewPlan.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = viewPlan.SourcePlan,
                  TypePlan = viewPlan.TypePlan
                });
              }
              else if ((plan.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (viewPlan.NewAction == EnumNewAction.Yes))
              {
                if (viewPlan.NewAction == EnumNewAction.Yes)
                  plan.StatusPlanApproved = EnumStatusPlanApproved.Open;

                var upPlan = servicePlan.GetAll(p => p._id == plan._id).FirstOrDefault();
                upPlan.Description = plan.Description;
                upPlan.Deadline = plan.Deadline;
                upPlan.Skills = new List<Skill>();
                foreach (var skill in plan.Skills)
                  upPlan.Skills.Add(serviceSkill.GetAll(p => p._id == skill._id).FirstOrDefault());
                upPlan.TypePlan = plan.TypePlan;
                upPlan.SourcePlan = plan.SourcePlan;
                upPlan.StatusPlan = plan.StatusPlan;
                upPlan.StatusPlanApproved = plan.StatusPlanApproved;
                upPlan.TypeAction = plan.TypeAction;
                upPlan.Attachments = new List<AttachmentField>();
                foreach (var attachments in plan.Attachments)
                  upPlan.Attachments.Add(new AttachmentField()
                  {
                    _idAttachment = attachments._idAttachment,
                    Name = attachments.Name,
                    Url = attachments.Url
                  });
                upPlan.NewAction = plan.NewAction;
                upPlan.TextEnd = plan.TextEnd;
                upPlan.TextEndManager = plan.TextEndManager;
                upPlan.Evaluation = plan.Evaluation;

                Task.Run(() => UpdatePlan(upPlan, monitoring.Person));
                listSchoolings.Add(plan);
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
            var listSkillsCompany = new List<ViewCrudPlan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == viewPlan._id)
              {
                Task.Run(() => UpdatePlan(viewPlan, monitoring.Person));
                listSkillsCompany.Add(new ViewCrudPlan()
                {
                  _id = viewPlan._id,
                  Name = viewPlan.Name,
                  Description = viewPlan.Description,
                  Deadline = viewPlan.Deadline,
                  Skills = viewPlan.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = viewPlan.SourcePlan,
                  TypePlan = viewPlan.TypePlan
                });
              }
              else if ((plan.StatusPlanApproved == EnumStatusPlanApproved.Invisible))
              {
                if (viewPlan.NewAction == EnumNewAction.Yes)
                  plan.StatusPlanApproved = EnumStatusPlanApproved.Open;

                var upPlan = servicePlan.GetAll(p => p._id == plan._id).FirstOrDefault();
                upPlan.Description = plan.Description;
                upPlan.Deadline = plan.Deadline;
                upPlan.Skills = new List<Skill>();
                foreach (var skill in plan.Skills)
                  upPlan.Skills.Add(serviceSkill.GetAll(p => p._id == skill._id).FirstOrDefault());
                upPlan.TypePlan = plan.TypePlan;
                upPlan.SourcePlan = plan.SourcePlan;
                upPlan.StatusPlan = plan.StatusPlan;
                upPlan.StatusPlanApproved = plan.StatusPlanApproved;
                upPlan.TypeAction = plan.TypeAction;
                upPlan.Attachments = new List<AttachmentField>();
                foreach (var attachments in plan.Attachments)
                  upPlan.Attachments.Add(new AttachmentField()
                  {
                    _idAttachment = attachments._idAttachment,
                    Name = attachments.Name,
                    Url = attachments.Url
                  });
                upPlan.NewAction = plan.NewAction;
                upPlan.TextEnd = plan.TextEnd;
                upPlan.TextEndManager = plan.TextEndManager;
                upPlan.Evaluation = plan.Evaluation;

                Task.Run(() => UpdatePlan(upPlan, monitoring.Person));
                listSkillsCompany.Add(plan);
              }
              else
                listSkillsCompany.Add(plan);
            }
            item.Plans = listSkillsCompany;
          }
        }

        serviceMonitoring.Update(monitoring, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private string NewPlanView(string idmonitoring, Plan planOld, Plan viewPlan)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

        //verify plan;
        if (viewPlan.SourcePlan == EnumSourcePlan.Activite)
        {
          foreach (var item in monitoring.Activities)
          {
            var listActivities = new List<ViewCrudPlan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == planOld._id)
              {
                AddPlan(viewPlan, monitoring.Person, idmonitoring, item._id);
                Task.Run(() => UpdatePlan(planOld, monitoring.Person));
                listActivities.Add(new ViewCrudPlan()
                {
                  _id = planOld._id,
                  Name = planOld.Name,
                  Description = planOld.Description,
                  Deadline = planOld.Deadline,
                  Skills = planOld.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = planOld.SourcePlan,
                  TypePlan = planOld.TypePlan
                });
                listActivities.Add(new ViewCrudPlan()
                {
                  _id = viewPlan._id,
                  Name = viewPlan.Name,
                  Description = viewPlan.Description,
                  Deadline = viewPlan.Deadline,
                  Skills = viewPlan.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = viewPlan.SourcePlan,
                  TypePlan = viewPlan.TypePlan
                });
              }
              else
                listActivities.Add(new ViewCrudPlan()
                {
                  _id = plan._id,
                  Name = plan.Name,
                  Description = plan.Description,
                  Deadline = plan.Deadline,
                  Skills = plan.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = plan.SourcePlan,
                  TypePlan = plan.TypePlan
                });
            }
            item.Plans = listActivities;
          }
        }
        else if (viewPlan.SourcePlan == EnumSourcePlan.Schooling)
        {
          foreach (var item in monitoring.Schoolings)
          {
            var listSchooling = new List<ViewCrudPlan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == planOld._id)
              {
                AddPlan(viewPlan, monitoring.Person, monitoring._id, item._id);
                Task.Run(() => UpdatePlan(planOld, monitoring.Person));
                listSchooling.Add(new ViewCrudPlan()
                {
                  _id = planOld._id,
                  Name = planOld.Name,
                  Description = planOld.Description,
                  Deadline = planOld.Deadline,
                  Skills = planOld.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = planOld.SourcePlan,
                  TypePlan = planOld.TypePlan
                });
                listSchooling.Add(new ViewCrudPlan()
                {
                  _id = viewPlan._id,
                  Name = viewPlan.Name,
                  Description = viewPlan.Description,
                  Deadline = viewPlan.Deadline,
                  Skills = viewPlan.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = viewPlan.SourcePlan,
                  TypePlan = viewPlan.TypePlan
                });
              }
              else
                listSchooling.Add(new ViewCrudPlan()
                {
                  _id = plan._id,
                  Name = plan.Name,
                  Description = plan.Description,
                  Deadline = plan.Deadline,
                  Skills = plan.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = plan.SourcePlan,
                  TypePlan = plan.TypePlan
                });
            }
          }
        }
        else
        {
          foreach (var item in monitoring.SkillsCompany)
          {
            var listSkillsCompany = new List<ViewCrudPlan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == planOld._id)
              {
                AddPlan(viewPlan, monitoring.Person, idmonitoring, item._id);
                Task.Run(() => UpdatePlan(planOld, monitoring.Person));
                listSkillsCompany.Add(new ViewCrudPlan()
                {
                  _id = planOld._id,
                  Name = planOld.Name,
                  Description = planOld.Description,
                  Deadline = planOld.Deadline,
                  Skills = planOld.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = planOld.SourcePlan,
                  TypePlan = planOld.TypePlan
                });
                listSkillsCompany.Add(new ViewCrudPlan()
                {
                  _id = viewPlan._id,
                  Name = viewPlan.Name,
                  Description = viewPlan.Description,
                  Deadline = viewPlan.Deadline,
                  Skills = viewPlan.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = viewPlan.SourcePlan,
                  TypePlan = viewPlan.TypePlan
                });
              }
              else
                listSkillsCompany.Add(new ViewCrudPlan()
                {
                  _id = plan._id,
                  Name = plan.Name,
                  Description = plan.Description,
                  Deadline = plan.Deadline,
                  Skills = plan.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = plan.SourcePlan,
                  TypePlan = plan.TypePlan
                });
            }
          }
        }

        serviceMonitoring.Update(monitoring, null);
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
        plan.Person = new ViewListPersonPlan()
        {
          _id = person._id,
          _idManager = person.Manager._id,
          NameManager = person.Manager.Name,
          User = new ViewListUser()
          {
            _id = person.User._id,
            Name = person.User.Name,
            Document = person.User.Document,
            Mail = person.User.Mail,
            Phone = person.User.Phone
          },
          Company = new ViewListCompany() { _id = person.Company._id, Name = person.Company.Name },
          Registration = person.Registration,
          Establishment = person.Establishment == null ? null : new ViewListEstablishment() { _id = person.Establishment._id, Name = person.Establishment.Name }
        };
        return servicePlan.InsertNewVersion(plan).Result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private StructPlan AddStructPlan(StructPlan structPlan)
    {
      try
      {
        return serviceStructPlan.InsertNewVersion(structPlan).Result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdatePlan(Plan plan, Person person)
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


        servicePlan.Update(plan, null);
        //return "Plan altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task LogSave(string iduser, string local)
    {
      try
      {
        var user = servicePerson.GetAll(p => p._id == iduser).FirstOrDefault();
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
    private async Task Mail(Person person, string namereceived, string mailreceived)
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
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
          client.BaseAddress = new Uri(link);
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
          var resultMail = client.PostAsync("mail/sendmail/" + idmail, null).Result;
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

    public string UpdatePlan(string idmonitoring, ViewCrudPlan viewPlan)
    {
      try
      {
        var plan = servicePlan.GetAll(p => p._id == viewPlan._id).FirstOrDefault();
        plan.Deadline = viewPlan.Deadline;
        plan.Description = viewPlan.Description;
        plan.Name = viewPlan.Name;
        plan.DateEnd = DateTime.Now;
        plan.TypeAction = viewPlan.TypeAction;
        plan.Attachments = viewPlan.Attachments?.Select(p => new AttachmentField()
        {
          _idAttachment = p._idAttachment,
          Name = p.Name,
          Url = p.Url
        }).ToList();
        plan.StatusPlan = viewPlan.StatusPlan;
        plan.NewAction = viewPlan.NewAction;
        plan.TextEnd = viewPlan.TextEnd;
        plan.TextEndManager = viewPlan.TextEndManager;
        plan.Evaluation = viewPlan.Evaluation;
        plan.StatusPlanApproved = viewPlan.StatusPlanApproved;
        plan.Skills = viewPlan.Skills?.Select(p => new Skill()
        {
          _id = p._id,
          Name = p.Name,
          Concept = p.Concept,
          Status = EnumStatus.Enabled,
          TypeSkill = p.TypeSkill,
          _idAccount = _user._idAccount
        }).ToList();
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
        var plan = servicePlan.GetAll(p => p._id == idplan).FirstOrDefault();

        foreach (var structplan in plan.StructPlans)
        {
          if (structplan._id == idstructplan)
          {
            plan.StructPlans.Remove(structplan);
            servicePlan.Update(plan, null);
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
        var model = servicePlanActivity.GetAll(p => p._id == id).FirstOrDefault();
        model.Status = EnumStatus.Disabled;
        servicePlanActivity.Update(model, null);
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

        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

        foreach (var plan in monitoring.Activities)
        {
          foreach (var res in plan.Plans)
          {
            if (res._id == idplan)
            {
              if (res.Attachments == null)
              {
                res.Attachments = new List<ViewCrudAttachmentField>();
              }
              res.Attachments.Add(new ViewCrudAttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });

              Task.Run(() => UpdatePlan(idmonitoring, res));
            }
          }
        }

        foreach (var plan in monitoring.SkillsCompany)
        {
          foreach (var res in plan.Plans)
          {
            if (res._id == idplan)
            {
              if (res.Attachments == null)
              {
                res.Attachments = new List<ViewCrudAttachmentField>();
              }
              res.Attachments.Add(new ViewCrudAttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
              Task.Run(() => UpdatePlan(idmonitoring, res));
            }
          }
        }

        foreach (var plan in monitoring.Schoolings)
        {
          foreach (var res in plan.Plans)
          {
            if (res._id == idplan)
            {
              if (res.Attachments == null)
              {
                res.Attachments = new List<ViewCrudAttachmentField>();
              }
              res.Attachments.Add(new ViewCrudAttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
              Task.Run(() => UpdatePlan(idmonitoring, res));
            }
          }
        }

        serviceMonitoring.Update(monitoring, null);
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

        var detail = servicePlanActivity.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
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
        return servicePlanActivity.GetAll(p => p._id == id)
          .Select(p => new ViewPlanActivity()
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
        servicePlanActivity.InsertNewVersion(new PlanActivity() { Name = model.Name, Status = EnumStatus.Enabled });
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
        }, null);
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

        var plan = servicePlan.GetAll().ToList();

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
            UserInclude = res.Person == null ? null : servicePerson.GetAll(p => p._id == res.Person._id).FirstOrDefault()?._id,
            TypePlan = res.TypePlan,
            IdPerson = res.Person?._id,
            NamePerson = res.Person?.User.Name,
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
                PlanActivity = (p.PlanActivity == null) ? null : new ViewPlanActivity() { Name = p.PlanActivity.Name, _id = p.PlanActivity._id },
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


    public List<ViewGetPlan> ListPlans(ref long total, string id, string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewGetPlan> result = new List<ViewGetPlan>();

        var plan = servicePlan.GetAll(p => p.Person._idManager == id).ToList();

        if (activities == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Activite).ToList();

        if (skillcompany == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Skill).ToList();

        if (schooling == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Schooling).ToList();

        foreach (var res in plan)
        {
          var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == res._idMonitoring).Result.FirstOrDefault().StatusMonitoring;
          if (monitoring == EnumStatusMonitoring.End)
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
              UserInclude = res.Person == null ? null : servicePerson.GetAll(p => p._id == res.Person._id).FirstOrDefault()?._id,
              TypePlan = res.TypePlan,
              _idPerson = res.Person._id,
              NamePerson = res.Person.User.Name,
              SourcePlan = res.SourcePlan,
              IdMonitoring = res._id,
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

        if (open == 0)
          result = result.Where(p => !(p.StatusPlanApproved == EnumStatusPlanApproved.Open & p.Deadline > DateTime.Now)).ToList();

        if (expired == 0)
          result = result.Where(p => !(p.StatusPlanApproved == EnumStatusPlanApproved.Open & p.Deadline < DateTime.Now)).ToList();

        if (end == 0)
          result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Approved).ToList();

        if (wait == 0)
          result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Wait).ToList();


        total = result.Count();

        return result.Skip(skip).Take(count).OrderBy(p => p.SourcePlan).ThenBy(p => p.Deadline).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewGetPlan> ListPlansPerson(ref long total, string id, string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewGetPlan> result = new List<ViewGetPlan>();

        var plan = servicePlan.GetAll(p => p.Person._id == id).ToList();

        if (activities == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Activite).ToList();

        if (skillcompany == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Skill).ToList();

        if (schooling == 0)
          plan = plan.Where(p => p.SourcePlan != EnumSourcePlan.Schooling).ToList();

        foreach (var res in plan)
        {
          var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == res._idMonitoring).Result.FirstOrDefault().StatusMonitoring;
          if (monitoring == EnumStatusMonitoring.End)
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
              UserInclude = res.Person == null ? null : servicePerson.GetAll(p => p._id == res.Person._id).FirstOrDefault()?._id,
              TypePlan = res.TypePlan,
              _idPerson = res.Person._id,
              NamePerson = res.Person.User.Name,
              SourcePlan = res.SourcePlan,
              IdMonitoring = res._id,
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

        if (open == 0)
          result = result.Where(p => !(p.StatusPlanApproved == EnumStatusPlanApproved.Open & p.Deadline > DateTime.Now)).ToList();

        if (expired == 0)
          result = result.Where(p => !(p.StatusPlanApproved == EnumStatusPlanApproved.Open & p.Deadline < DateTime.Now)).ToList();

        if (end == 0)
          result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Approved).ToList();

        if (wait == 0)
          result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Wait).ToList();


        total = result.Count();

        return result.Skip(skip).Take(count).OrderBy(p => p.SourcePlan).ThenBy(p => p.Deadline).ToList();
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
        var plan = servicePlan.GetAll(p => p._id == idplan).FirstOrDefault();

        if (plan == null)
          return null;

        var view = new ViewListPlanStruct()
        {
          _id = plan._id,
          Description = plan.Description,
          Deadline = plan.Deadline,
          Skills = plan.Skills == null ? null : plan.Skills.Select(p => new ViewListSkill()
          {
            _id = p._id,
            Concept = p.Concept,
            Name = p.Name,
            TypeSkill = p.TypeSkill
          }).ToList(),
          UserInclude = plan.Person?._id,
          DateInclude = plan.DateInclude,
          TypePlan = plan.TypePlan,
          IdPerson = plan.Person?._id,
          NamePerson = plan.Person?.User.Name,
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
          Attachments = plan.Attachments == null ? null :
          plan.Attachments.Select(p => new ViewCrudAttachmentField()
          {
            Name = p.Name,
            Url = p.Url,
            _idAttachment = p._idAttachment
          }).ToList(),
          NewAction = plan.NewAction,
          StructPlans = plan.StructPlans == null ? null : plan.StructPlans.Select(p => new ViewCrudStructPlan()
          {
            _id = p._id,
            TypeResponsible = p.TypeResponsible,
            TypeAction = p.TypeAction,
            PlanActivity = new ViewPlanActivity() { _id = p.PlanActivity._id, Name = p.PlanActivity.Name },
            Course = new ViewListCourse() { _id = p.Course._id, Name = p.Course.Name }
          }).ToList(),
          Name = plan.Name
        };

        if (plan.SourcePlan == EnumSourcePlan.Activite)
        {
          try
          {
            view.PlanNew = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault().Activities.Where(
            p => p._id == plan._idItem).FirstOrDefault().Plans.Where(p => p.Name == plan.Name &
            p._id != plan._id).FirstOrDefault();
          }
          catch (Exception)
          {

          }
        }

        if (plan.SourcePlan == EnumSourcePlan.Skill)
        {
          try
          {
            view.PlanNew = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault().SkillsCompany.Where(
           p => p._id == plan._idItem).FirstOrDefault().Plans.Where(p => p.Name == plan.Name &
           p._id != plan._id).FirstOrDefault();
          }
          catch (Exception)
          {

          }
        }


        if (plan.SourcePlan == EnumSourcePlan.Schooling)
        {
          try
          {
            view.PlanNew = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault().Schoolings.Where(
            p => p._id == plan._idItem).FirstOrDefault().Plans.Where(p => p.Name == plan.Name &
            p._id != plan._id).FirstOrDefault();
          }
          catch (Exception)
          {

          }
        }
        return view;

      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewPlanShort> ListPlansPerson(ref long total, string id, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewGetPlan> result = new List<ViewGetPlan>();

        var plan = servicePlan.GetAll(p => p.Person._id == id).ToList();

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
            UserInclude = res.Person == null ? null : servicePerson.GetAll(p => p._id == res.Person._id).FirstOrDefault()?._id,
            TypePlan = res.TypePlan,
            _idPerson = res.Person._id,
            NamePerson = res.Person.User.Name,
            SourcePlan = res.SourcePlan,
            IdMonitoring = res._id,
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

        result.Skip(skip).Take(count).OrderBy(p => p.SourcePlan).ThenBy(p => p.Deadline).ToList();

        total = result.Count();
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

    public List<ViewPlanShort> ListPlans(ref long total, string id, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewGetPlan> result = new List<ViewGetPlan>();

        var plan = servicePlan.GetAll(p => p.Person._idManager == id).ToList();

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
            UserInclude = res.Person == null ? null : servicePerson.GetAll(p => p._id == res.Person._id).FirstOrDefault()?._id,
            TypePlan = res.TypePlan,
            _idPerson = res.Person._id,
            NamePerson = res.Person.User.Name,
            SourcePlan = res.SourcePlan,
            IdMonitoring = res._id,
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

    public string NewPlan(string idmonitoring, string idplanold, ViewCrudPlan view)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        var viewPlan = new Plan()
        {
          _id = view._id,
          Name = view.Name,
          Description = view.Description,
          Deadline = view.Deadline,
          Skills = view.Skills?.Select(p => new Skill()
          {
            _id = p._id,
            Name = p.Name,
            Concept = p.Concept,
            Status = EnumStatus.Enabled,
            TypeSkill = p.TypeSkill,
            _idAccount = _user._idAccount
          }).ToList(),
          TypePlan = view.TypePlan,
          SourcePlan = view.SourcePlan
        };

        //verify plan;
        if (viewPlan.SourcePlan == EnumSourcePlan.Activite)
        {
          foreach (var item in monitoring.Activities)
          {
            var listActivities = new List<ViewCrudPlan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplanold)
              {
                AddPlan(viewPlan, monitoring.Person, monitoring._id, item._id);
                listActivities.Add(plan);
                listActivities.Add(new ViewCrudPlan()
                {
                  _id = viewPlan._id,
                  Name = viewPlan.Name,
                  Description = viewPlan.Description,
                  Deadline = viewPlan.Deadline,
                  Skills = viewPlan.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = viewPlan.SourcePlan,
                  TypePlan = viewPlan.TypePlan
                });
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
            var listSchoolings = new List<ViewCrudPlan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplanold)
              {
                AddPlan(viewPlan, monitoring.Person, monitoring._id, item._id);
                listSchoolings.Add(plan);
                listSchoolings.Add(new ViewCrudPlan()
                {
                  _id = viewPlan._id,
                  Name = viewPlan.Name,
                  Description = viewPlan.Description,
                  Deadline = viewPlan.Deadline,
                  Skills = viewPlan.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = viewPlan.SourcePlan,
                  TypePlan = viewPlan.TypePlan
                });
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
            var listSkillsCompany = new List<ViewCrudPlan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplanold)
              {
                AddPlan(viewPlan, monitoring.Person, monitoring._id, item._id);
                listSkillsCompany.Add(plan);
                listSkillsCompany.Add(new ViewCrudPlan()
                {
                  _id = viewPlan._id,
                  Name = viewPlan.Name,
                  Description = viewPlan.Description,
                  Deadline = viewPlan.Deadline,
                  Skills = viewPlan.Skills?.Select(x => new ViewListSkill()
                  {
                    _id = x._id,
                    Name = x.Name,
                    Concept = x.Concept,
                    TypeSkill = x.TypeSkill
                  }).ToList(),
                  SourcePlan = viewPlan.SourcePlan,
                  TypePlan = viewPlan.TypePlan
                });
              }
              else
                listSkillsCompany.Add(plan);
            }
            item.Plans = listSkillsCompany;
          }
        }


        serviceMonitoring.Update(monitoring, null);
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
        var plan = servicePlan.GetAll(p => p._id == idplan).FirstOrDefault();
        DateTime? deadline = DateTime.Now;

        plan.StructPlans.Add(AddStructPlan(new StructPlan()
        {
          _id = structplan._id,
          Course = (structplan.Course == null) ? null : serviceCourse.GetAll(p => p._id == structplan.Course._id).FirstOrDefault(),
          PlanActivity = (structplan.PlanActivity == null) ? null : new PlanActivity() { _id = structplan.PlanActivity._id, _idAccount = _user._idAccount, Status = EnumStatus.Enabled, Name = structplan.PlanActivity.Name },
          TypeAction = structplan.TypeAction,
          TypeResponsible = structplan.TypeResponsible
        }));

        if (structplan.Course != null)
        {
          var trainingPlan = new TrainingPlan
          {
            Course = (structplan.Course == null) ? null : serviceCourse.GetAll(p => p._id == structplan.Course._id)
            .Select(p => new ViewListCourse()
            {
              _id = p._id,
              Name = p.Name
            }).FirstOrDefault(),
            Deadline = deadline,
            Origin = EnumOrigin.Monitoring,
            Person = servicePerson.GetAll(p => p._id == plan.Person._id).FirstOrDefault().GetViewListManager(),
            Include = DateTime.Now,
            StatusTrainingPlan = EnumStatusTrainingPlan.Open
          };
          serviceMandatoryTraining.NewTrainingPlanInternal(trainingPlan);
        }

        servicePlan.Update(plan, null);
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
        var structplan = servicePlan.GetAll(p => p._id == idplan).FirstOrDefault().StructPlans.Where(p => p._id == idstructplan).FirstOrDefault();

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
        var plan = servicePlan.GetAll(p => p._id == idplan).FirstOrDefault();

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
                Person = servicePerson.GetAll(p => p._id == plan.Person._id).FirstOrDefault().GetViewListManager(),
                Include = DateTime.Now,
                StatusTrainingPlan = EnumStatusTrainingPlan.Open
              };
              if (serviceTrainingPlans.GetAll(p => p.Person == trainingPlan.Person
               & p.Course == trainingPlan.Course & p.Origin == EnumOrigin.Monitoring & p.Deadline == trainingPlan.Deadline).Count() == 0)
              {
                serviceMandatoryTraining.NewTrainingPlanInternal(trainingPlan);
              }
            }

            plan.StructPlans.Remove(structplan);
            plan.StructPlans.Add(new StructPlan()
            {
              _id = structplanedit._id,
              Course = (structplanedit.Course == null) ? null : serviceCourse.GetAll(p => p._id == structplanedit.Course._id).FirstOrDefault(),
              PlanActivity = (structplanedit.PlanActivity == null) ? null : new PlanActivity() { _id = structplanedit.PlanActivity._id, Name = structplanedit.PlanActivity.Name },
              TypeAction = structplanedit.TypeAction,
              TypeResponsible = structplanedit.TypeResponsible
            });
            servicePlan.Update(plan, null);
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

    public string NewUpdatePlan(string idmonitoring, List<ViewCrudNewPlanUp> viewPlan)
    {
      try
      {

        Plan planNew = new Plan();
        Plan planUpdate = new Plan();

        var person = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault().Person;

        foreach (var item in viewPlan)
        {
          if (item.TypeViewPlan == EnumTypeViewPlan.New)
            planNew = new Plan()
            {
              _id = item._id,
              _idAccount = _user._idAccount,
              Name = item.Name,
              Description = item.Description,
              Deadline = item.Deadline,
              Skills = item.Skills?.Select(p => new Skill()
              {
                _id = p._id,
                Name = p.Name,
                Concept = p.Name,
                TypeSkill = p.TypeSkill,
                _idAccount = _user._idAccount,
                Status = EnumStatus.Enabled
              }).ToList(),
              DateInclude = item.DateInclude,
              TypePlan = item.TypePlan,
              SourcePlan = item.SourcePlan,
              TypeAction = item.TypeAction,
              StatusPlan = item.StatusPlan,
              TextEnd = item.TextEnd,
              TextEndManager = item.TextEndManager,
              DateEnd = item.DateEnd,
              Evaluation = item.Evaluation,
              Result = item.Result,
              StatusPlanApproved = EnumStatusPlanApproved.Open,
              Status = item.Status,
              NewAction = item.NewAction,
              Attachments = item.Attachments?.Select(p => new AttachmentField()
              {
                _idAttachment = p._idAttachment,
                Name = p.Name,
                Url = p.Url
              }).ToList()
            };
          else
            planUpdate = new Plan()
            {
              _id = item._id,
              _idAccount = _user._idAccount,
              Name = item.Name,
              Description = item.Description,
              Deadline = item.Deadline,
              Skills = item.Skills?.Select(p => new Skill()
              {
                _id = p._id,
                Name = p.Name,
                Concept = p.Name,
                TypeSkill = p.TypeSkill,
                _idAccount = _user._idAccount,
                Status = EnumStatus.Enabled
              }).ToList(),
              DateInclude = item.DateInclude,
              TypePlan = item.TypePlan,
              SourcePlan = item.SourcePlan,
              TypeAction = item.TypeAction,
              StatusPlan = item.StatusPlan,
              TextEnd = item.TextEnd,
              TextEndManager = item.TextEndManager,
              DateEnd = item.DateEnd,
              Evaluation = item.Evaluation,
              Result = item.Result,
              StatusPlanApproved = item.StatusPlanApproved,
              Status = item.Status,
              NewAction = item.NewAction,
              Attachments = item.Attachments?.Select(p => new AttachmentField()
              {
                _idAttachment = p._idAttachment,
                Name = p.Name,
                Url = p.Url
              }).ToList()
            };
        }

        if (_user._idUser == person.User._id)
        {
          planNew.StatusPlanApproved = EnumStatusPlanApproved.Invisible;
          NewPlanView(idmonitoring, planUpdate, planNew);
        }
        else
        {
          if (planUpdate.NewAction == EnumNewAction.Yes)
          {
            NewPlanView(idmonitoring, planUpdate, planNew);
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
        var plan = servicePlan.GetAll(p => p._id == idplan).FirstOrDefault();
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
          Skills = plan.Skills == null ? null : plan.Skills.OrderBy(x => x.Name).Select(x =>
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
          Attachments = plan.Attachments == null ? null : plan.Attachments.Select(x => new ViewCrudAttachmentField()
          {
            Name = x.Name,
            Url = x.Url,
            _idAttachment = x._idAttachment
          }).ToList(),
          NewAction = plan.NewAction,
          _idPerson = plan.Person._id,
          NamePerson = plan.Person.User.Name,
          Bomb = GetBomb((DateTime.Parse(plan.Deadline.ToString()) - DateTime.Now).Days)
        };
        if (plan.SourcePlan == EnumSourcePlan.Activite)
        {
          try
          {
            resultPlan.PlanNew = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault().Activities.Where(
            p => p._id == plan._idItem).FirstOrDefault().Plans.Where(p => p.Name == plan.Name &
            p._id != plan._id).FirstOrDefault();
          }
          catch (Exception)
          {

          }
        }

        if (plan.SourcePlan == EnumSourcePlan.Skill)
        {
          try
          {
            resultPlan.PlanNew = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault().SkillsCompany.Where(
           p => p._id == plan._idItem).FirstOrDefault().Plans.Where(p => p.Name == plan.Name &
           p._id != plan._id).FirstOrDefault();
          }
          catch (Exception)
          {

          }
        }


        if (plan.SourcePlan == EnumSourcePlan.Schooling)
        {
          try
          {
            resultPlan.PlanNew = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault().Schoolings.Where(
            p => p._id == plan._idItem).FirstOrDefault().Plans.Where(p => p.Name == plan.Name &
            p._id != plan._id).FirstOrDefault();
          }
          catch (Exception)
          {

          }
        }


        return resultPlan;
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
