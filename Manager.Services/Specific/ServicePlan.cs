using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Manager.Services.Specific
{
#pragma warning disable 1998
  public class ServicePlan : Repository<Plan>, IServicePlan
  {
    private readonly ServiceAuthentication serviceAuthentication;
    private ServiceGeneric<Person> servicePerson;
    private ServiceGeneric<Course> serviceCourse;
    private ServiceGeneric<Monitoring> serviceMonitoring;
    private ServiceGeneric<Plan> servicePlan;
    private ServiceGeneric<StructPlan> serviceStructPlan;
    private ServiceGeneric<PlanActivity> servicePlanActivity;
    private readonly ServiceLog serviceLog;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<MailMessage> serviceMailMessage;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceGeneric<TrainingPlan> serviceTrainingPlans;
    private readonly ServiceLogMessages serviceLogMessages;
    IServiceMandatoryTraining serviceMandatoryTraining;
    public string path;


    #region Constructor
    public BaseUser user { get => _user; set => user = _user; }

    public ServicePlan(DataContext context, string pathToken, IServiceMandatoryTraining _serviceMandatoryTraining)
      : base(context)
    {
      try
      {
        serviceMandatoryTraining = _serviceMandatoryTraining;
        serviceMonitoring = new ServiceGeneric<Monitoring>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceCourse = new ServiceGeneric<Course>(context);
        servicePlan = new ServiceGeneric<Plan>(context);
        serviceLog = new ServiceLog(_context);
        serviceTrainingPlans = new ServiceGeneric<TrainingPlan>(context);
        serviceMailModel = new ServiceMailModel(context);
        serviceMailMessage = new ServiceGeneric<MailMessage>(context);
        serviceMail = new ServiceGeneric<MailLog>(context);
        serviceStructPlan = new ServiceGeneric<StructPlan>(context);
        servicePlanActivity = new ServiceGeneric<PlanActivity>(context);
        serviceLogMessages = new ServiceLogMessages(context);
        serviceAuthentication = new ServiceAuthentication(context);
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
        servicePerson._user = _user;
        serviceCourse._user = _user;
        serviceMonitoring._user = _user;
        servicePlan._user = _user;
        serviceLog._user = _user;
        serviceMailModel._user = _user;
        serviceMailMessage._user = _user;
        serviceMail._user = _user;
        servicePlan._user = _user;
        serviceStructPlan._user = _user;
        servicePlanActivity._user = _user;
        serviceTrainingPlans._user = _user;
        serviceLogMessages._user = _user;
        serviceMandatoryTraining.SetUser(contextAccessor);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region Private
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
            var listActivities = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == viewPlan._id)
              {
                UpdatePlan(viewPlan, monitoring.Person);
                listActivities.Add(viewPlan);
              }
              else if ((plan.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (viewPlan.NewAction == EnumNewAction.Yes))
              {
                if (viewPlan.NewAction == EnumNewAction.Yes)
                  plan.StatusPlanApproved = EnumStatusPlanApproved.Open;

                UpdatePlan(plan, monitoring.Person);
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
            var listSchoolings = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == viewPlan._id)
              {
                UpdatePlan(viewPlan, monitoring.Person);
                listSchoolings.Add(viewPlan);
              }
              else if ((plan.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (viewPlan.NewAction == EnumNewAction.Yes))
              {
                if (viewPlan.NewAction == EnumNewAction.Yes)
                  plan.StatusPlanApproved = EnumStatusPlanApproved.Open;

                UpdatePlan(plan, monitoring.Person);
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
            var listSkillsCompany = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == viewPlan._id)
              {
                UpdatePlan(viewPlan, monitoring.Person);
                listSkillsCompany.Add(viewPlan);
              }
              else if ((plan.StatusPlanApproved == EnumStatusPlanApproved.Invisible))
              {
                if (viewPlan.NewAction == EnumNewAction.Yes)
                  plan.StatusPlanApproved = EnumStatusPlanApproved.Open;

                UpdatePlan(plan, monitoring.Person);
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
            var listActivities = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == planOld._id)
              {
                AddPlan(viewPlan, monitoring.Person);
                UpdatePlan(planOld, monitoring.Person);
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
                UpdatePlan(planOld, monitoring.Person);
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
                UpdatePlan(planOld, monitoring.Person);
                listSkillsCompany.Add(planOld);
                listSkillsCompany.Add(viewPlan);
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

    private Plan AddPlan(Plan plan, Person person)
    {
      try
      {
        plan.DateInclude = DateTime.Now;
        plan.UserInclude = person;
        return servicePlan.Insert(plan);
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
        return serviceStructPlan.Insert(structPlan);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private string UpdatePlan(Plan plan, Person person)
    {
      try
      {
        LogSave(person._id, "Plan Process Update");
        if (plan.StatusPlanApproved == EnumStatusPlanApproved.Wait)
        {
          if (user._idPerson == person._id)
          {
            var manager = servicePerson.GetAll(p => p._id == person.Manager._id).FirstOrDefault();
            Mail(manager);
          }
          else
          {
            Mail(person);
          }
        }
        servicePlan.Update(plan, null);
        return "Plan altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async void LogSave(string iduser, string local)
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
    private void Mail(Person person)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.PlanApproval(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        string managername = "";
        try
        {
          managername = servicePerson.GetAll(p => p._id == person.Manager._id).FirstOrDefault().User.Name;
        }
        catch (Exception)
        {

        }

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", managername).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
        var message = new MailMessage
        {
          Type = EnumTypeMailMessage.Put,
          Name = model.Name,
          Url = url,
          Body = body
        };
        var idMessage = serviceMailMessage.Insert(message)._id;
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.User.Mail, person.User.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
          Body = body,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = model.Subject
        };
        var mailObj = serviceMail.Insert(sendMail);
        var token = SendMail(path, person, mailObj._id.ToString());
        var messageEnd = serviceMailMessage.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        serviceMailMessage.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        //throw e;
      }
    }
    private string SendMail(string link, Person person, string idmail)
    {
      try
      {
        ViewPerson view = serviceAuthentication.AuthenticationMail(person);
        using (var client = new HttpClient())
        {
          //client.BaseAddress = new Uri(link);
          //var data = new
          //{
          //  mail = person.User.Mail,
          //  password = person.User.Password
          //};
          //var json = JsonConvert.SerializeObject(data);
          //var content = new StringContent(json);
          //content.Headers.ContentType.MediaType = "application/json";
          //client.DefaultRequestHeaders.Add("ContentType", "application/json");
          //var result = client.PostAsync("manager/authentication/encrypt", content).Result;
          //var resultContent = result.Content.ReadAsStringAsync().Result;
          //var auth = JsonConvert.DeserializeObject<ViewPerson>(resultContent);
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + view.Token);
          var resultMail = client.PostAsync("mail/sendmail/" + idmail, null).Result;
          return view.Token;
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

        UpdatePlan(idmonitoring, plan);
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
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

        //verify plan;
        if (sourceplan == EnumSourcePlan.Activite)
        {
          foreach (var item in monitoring.Activities)
          {
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == idstructplan)
                  {
                    plan.StructPlans.Remove(structplan);
                    serviceMonitoring.Update(monitoring, null);
                    return "update";
                  }
                }
              }
            }
          }
        }
        else if (sourceplan == EnumSourcePlan.Schooling)
        {
          foreach (var item in monitoring.Schoolings)
          {
            var listSchoolings = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == idstructplan)
                  {
                    plan.StructPlans.Remove(structplan);
                    serviceMonitoring.Update(monitoring, null);
                    return "update";
                  }
                }
              }
            }
          }
        }
        else
        {
          foreach (var item in monitoring.SkillsCompany)
          {
            var listSkillsCompany = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == idstructplan)
                  {
                    plan.StructPlans.Remove(structplan);
                    serviceMonitoring.Update(monitoring, null);
                    return "update";
                  }
                }
              }
            }
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
                res.Attachments = new List<AttachmentField>();
              }
              res.Attachments.Add(new AttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
              servicePlan.Update(res, null);
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
                res.Attachments = new List<AttachmentField>();
              }
              res.Attachments.Add(new AttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
              servicePlan.Update(res, null);
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
                res.Attachments = new List<AttachmentField>();
              }
              res.Attachments.Add(new AttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
              servicePlan.Update(res, null);
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
        total = servicePlanActivity.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

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
        servicePlanActivity.Insert(new PlanActivity() { Name = model.Name, Status = EnumStatus.Enabled });
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

        if (activities == 1)
        {
          var detail = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End)
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), p.Person, p._id }).ToList();


          foreach (var item in detail)
          {
            foreach (var plan in item.Plans)
            {
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
                  UserInclude = res.UserInclude?._id,
                  TypePlan = res.TypePlan,
                  IdPerson = item.Person._id,
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  _idMonitoring = item._id,
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
            }
          }
        }

        if (schooling == 1)
        {
          var detailSchool = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End)
            .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), p.Person, p._id }).ToList();
          foreach (var item in detailSchool)
          {
            foreach (var plan in item.Plans)
            {
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
                  UserInclude = res.UserInclude?._id,
                  TypePlan = res.TypePlan,
                  IdPerson = item.Person._id,
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  _idMonitoring = item._id,
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
            }
          }
        }

        if (skillcompany == 1)
        {
          var detailSkills = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End)
            .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), p.Person, p._id }).ToList();

          foreach (var item in detailSkills)
          {
            foreach (var plan in item.Plans)
            {
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
                  UserInclude = res.UserInclude?._id,
                  TypePlan = res.TypePlan,
                  IdPerson = item.Person._id,
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  _idMonitoring = item._id,
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
            }
          }
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

        if (activities == 1)
        {
          var detail = (from monitoring in
         serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
       .Select(p => new { Plans = p.Activities.Select(x => x.Plans), p.Person, p._id }).ToList()
                        join person in servicePerson.GetAll() on monitoring.Person._id equals person._id
                        where person.Manager._id == id
                        select monitoring
       ).ToList();

          /*var detail = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), Person = p.Person, _id = p._id }).ToList();*/


          foreach (var item in detail)
          {
            foreach (var plan in item.Plans)
            {
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
                  UserInclude = res.UserInclude?._id,
                  TypePlan = res.TypePlan,
                  _idPerson = item.Person._id,
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
          }
        }

        if (schooling == 1)
        {
          //var detailSchool = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          //  .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), Person = p.Person, _id = p._id }).ToList();

          var detailSchool = (from monitoring in
         serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
       .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), p.Person, p._id }).ToList()
                              join person in servicePerson.GetAll() on monitoring.Person._id equals person._id
                              where person.Manager._id == id
                              select monitoring
       ).ToList();

          foreach (var item in detailSchool)
          {
            foreach (var plan in item.Plans)
            {
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
                  UserInclude = res.UserInclude?._id,
                  TypePlan = res.TypePlan,
                  _idPerson = item.Person._id,
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
          }
        }

        if (skillcompany == 1)
        {
          //var detailSkills = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          //  .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), Person = p.Person, _id = p._id }).ToList();

          var detailSkills = (from monitoring in
         serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
       .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), p.Person, p._id }).ToList()
                              join person in servicePerson.GetAll() on monitoring.Person._id equals person._id
                              where person.Manager._id == id
                              select monitoring
       ).ToList();

          foreach (var item in detailSkills)
          {
            foreach (var plan in item.Plans)
            {
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
                  UserInclude = res.UserInclude?._id,
                  TypePlan = res.TypePlan,
                  _idPerson = item.Person._id,
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
          }
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

        if (activities == 1)
        {
          var detail = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), p.Person, p._id }).ToList();


          foreach (var item in detail)
          {
            foreach (var plan in item.Plans)
            {
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
                  UserInclude = res.UserInclude?._id,
                  TypePlan = res.TypePlan,
                  _idPerson = item.Person._id,
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
          }
        }

        if (schooling == 1)
        {
          var detailSchool = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
            .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), p.Person, p._id }).ToList();
          foreach (var item in detailSchool)
          {
            foreach (var plan in item.Plans)
            {
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
                  UserInclude = res.UserInclude?._id,
                  TypePlan = res.TypePlan,
                  _idPerson = item.Person._id,
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
          }
        }

        if (skillcompany == 1)
        {
          var detailSkills = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
            .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), p.Person, p._id }).ToList();

          foreach (var item in detailSkills)
          {
            foreach (var plan in item.Plans)
            {
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
                  UserInclude = res.UserInclude?._id,
                  TypePlan = res.TypePlan,
                  _idPerson = item.Person._id,
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
          }
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
        var detail = serviceMonitoring.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), p.Person, p._id }).FirstOrDefault();

        var detailSchoolings = serviceMonitoring.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), p.Person, p._id }).FirstOrDefault();

        var detailSkillsCompany = serviceMonitoring.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), p.Person, p._id }).FirstOrDefault();

        if (detail == null)
          return new ViewListPlanStruct() { _id = idmonitoring };

        ViewListPlanStruct view = new ViewListPlanStruct();
        bool exists = false;

        foreach (var plan in detail.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
            {
              exists = true;
              view.DateInclude = res.DateInclude;
              view.Deadline = res.Deadline;
              view.Name = res.Name;
              view.Description = res.Description;
              view.SourcePlan = res.SourcePlan;
              view.StatusPlan = res.StatusPlan;
              view.StatusPlanApproved = res.StatusPlanApproved;
              view.UserInclude = res.UserInclude?._id;
              view.TypeAction = res.TypeAction;
              view.TypePlan = res.TypePlan;
              view.Evaluation = res.Evaluation;
              view.Skills = res.Skills?.OrderBy(p => p.Name)
                .Select(p => new ViewListSkill()
                {
                  _id = p._id,
                  Name = p.Name,
                  Concept = p.Concept,
                  TypeSkill = p.TypeSkill
                })
                .ToList();
              view._id = res._id;
              view._idMonitoring = detail._id;
              view.TextEnd = res.TextEnd;
              view.TextEndManager = res.TextEndManager;
              view.Status = res.Status;
              view.DateEnd = res.DateEnd;
              view.Attachments = res.Attachments?.Select(p => new ViewCrudAttachmentField()
              {
                Name = p.Name,
                Url = p.Url,
                _idAttachment = p._idAttachment
              }).ToList();
              view.NewAction = res.NewAction;
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
            }
            else if ((res.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (res.Name == view.Name))
              view.PlanNew = new ViewCrudPlan()
              {
                _id = res._id,
                TypePlan = res.TypePlan,
                SourcePlan = res.SourcePlan,
                Name = res.Name,
                Deadline = res.Deadline,
                Description = res.Description,
                Skills = res.Skills?.Select(p => new ViewListSkill()
                {
                  _id = p._id,
                  Name = p.Name,
                  Concept = p.Name,
                  TypeSkill = p.TypeSkill
                }).ToList()
              };
          }
        }

        if (exists)
          return view;

        foreach (var plan in detailSchoolings.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
            {
              exists = true;
              view.DateInclude = res.DateInclude;
              view.Deadline = res.Deadline;
              view.Name = res.Name;
              view.Description = res.Description;
              view.SourcePlan = res.SourcePlan;
              view.StatusPlan = res.StatusPlan;
              view.StatusPlanApproved = res.StatusPlanApproved;
              view.UserInclude = res.UserInclude?._id;
              view.TypeAction = res.TypeAction;
              view.TypePlan = res.TypePlan;
              view.Evaluation = res.Evaluation;
              view.Skills = res.Skills?.OrderBy(p => p.Name)
                .Select(p => new ViewListSkill()
                {
                  _id = p._id,
                  Name = p.Name,
                  Concept = p.Concept,
                  TypeSkill = p.TypeSkill
                })
                .ToList();
              view._id = res._id;
              view._idMonitoring = detail._id;
              view.TextEnd = res.TextEnd;
              view.TextEndManager = res.TextEndManager;
              view.Status = res.Status;
              view.DateEnd = res.DateEnd;
              view.Attachments = res.Attachments?.Select(p => new ViewCrudAttachmentField()
              {
                Name = p.Name,
                Url = p.Url,
                _idAttachment = p._idAttachment
              }).ToList();
              view.NewAction = res.NewAction;
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
            }
            else if ((res.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (res.Name == view.Name))
              view.PlanNew = new ViewCrudPlan()
              {
                _id = res._id,
                TypePlan = res.TypePlan,
                SourcePlan = res.SourcePlan,
                Name = res.Name,
                Deadline = res.Deadline,
                Description = res.Description,
                Skills = res.Skills?.Select(p => new ViewListSkill()
                {
                  _id = p._id,
                  Name = p.Name,
                  Concept = p.Name,
                  TypeSkill = p.TypeSkill
                }).ToList()
              };
          }
        }

        if (exists)
          return view;

        foreach (var plan in detailSkillsCompany.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
            {
              exists = true;
              view.DateInclude = res.DateInclude;
              view.Deadline = res.Deadline;
              view.Name = res.Name;
              view.Description = res.Description;
              view.SourcePlan = res.SourcePlan;
              view.StatusPlan = res.StatusPlan;
              view.StatusPlanApproved = res.StatusPlanApproved;
              view.UserInclude = res.UserInclude?._id;
              view.TypeAction = res.TypeAction;
              view.TypePlan = res.TypePlan;
              view.Evaluation = res.Evaluation;
              view.Skills = res.Skills?.OrderBy(p => p.Name)
                .Select(p => new ViewListSkill()
                {
                  _id = p._id,
                  Name = p.Name,
                  Concept = p.Concept,
                  TypeSkill = p.TypeSkill
                })
                .ToList();
              view._id = res._id;
              view._idMonitoring = detail._id;
              view.TextEnd = res.TextEnd;
              view.TextEndManager = res.TextEndManager;
              view.Status = res.Status;
              view.DateEnd = res.DateEnd;
              view.Attachments = res.Attachments?.Select(p => new ViewCrudAttachmentField()
              {
                Name = p.Name,
                Url = p.Url,
                _idAttachment = p._idAttachment
              }).ToList();
              view.NewAction = res.NewAction;
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
            }
            else if ((res.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (res.Name == view.Name))
              view.PlanNew = new ViewCrudPlan()
              {
                _id = res._id,
                TypePlan = res.TypePlan,
                SourcePlan = res.SourcePlan,
                Name = res.Name,
                Deadline = res.Deadline,
                Description = res.Description,
                Skills = res.Skills?.Select(p => new ViewListSkill()
                {
                  _id = p._id,
                  Name = p.Name,
                  Concept = p.Name,
                  TypeSkill = p.TypeSkill
                }).ToList()
              };
          }
        }

        if (exists)
          return view;

        return null;
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
        List<ViewPlan> result = new List<ViewPlan>();

        var detail = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
        .Select(p => new { Plans = p.Activities.Select(x => x.Plans), p.Person, p._id }).ToList();


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
                NamePerson = item.Person.User.Name,
                SourcePlan = res.SourcePlan,
                IdMonitoring = item._id,
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
        }


        var detailSchool = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), p.Person, p._id }).ToList();
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
                NamePerson = item.Person.User.Name,
                SourcePlan = res.SourcePlan,
                IdMonitoring = item._id,
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
        }


        var detailSkills = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), p.Person, p._id }).ToList();

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
                NamePerson = item.Person.User.Name,
                SourcePlan = res.SourcePlan,
                IdMonitoring = item._id,
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

    public List<ViewPlanShort> ListPlans(ref long total, string id, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewGetPlan> result = new List<ViewGetPlan>();

        var detail = (from monitoring in
          serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
        .Select(p => new { Plans = p.Activities.Select(x => x.Plans), p.Person, p._id }).ToList()
                      join person in servicePerson.GetAll() on monitoring.Person._id equals person._id
                      where person.Manager._id == id
                      select monitoring
        ).ToList();


        foreach (var item in detail)
        {
          foreach (var plan in item.Plans)
          {
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
                UserInclude = res.UserInclude?._id,
                TypePlan = res.TypePlan,
                _idPerson = item.Person._id,
                NamePerson = item.Person.User.Name,
                SourcePlan = res.SourcePlan,
                IdMonitoring = item._id,
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
        }


        var detailSchool = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), p.Person, p._id }).ToList();
        foreach (var item in detailSchool)
        {
          foreach (var plan in item.Plans)
          {
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
                UserInclude = res.UserInclude?._id,
                TypePlan = res.TypePlan,
                _idPerson = item.Person._id,
                NamePerson = item.Person.User.Name,
                SourcePlan = res.SourcePlan,
                IdMonitoring = item._id,
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
        }


        var detailSkills = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), p.Person, p._id }).ToList();

        foreach (var item in detailSkills)
        {
          foreach (var plan in item.Plans)
          {
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
                UserInclude = res.UserInclude?._id,
                TypePlan = res.TypePlan,
                _idPerson = item.Person._id,
                NamePerson = item.Person.User.Name,
                SourcePlan = res.SourcePlan,
                IdMonitoring = item._id,
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
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        DateTime? deadline = DateTime.Now;

        //verify plan;
        if (sourceplan == EnumSourcePlan.Activite)
        {
          foreach (var item in monitoring.Activities)
          {
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                deadline = plan.Deadline;

                if (plan.StructPlans == null)
                  plan.StructPlans = new List<StructPlan>();

                plan.StructPlans.Add(AddStructPlan(new StructPlan()
                {
                  _id = structplan._id,
                  Course = (structplan.Course == null) ? null : serviceCourse.GetAll(p => p._id == structplan.Course._id).FirstOrDefault(),
                  PlanActivity = (structplan.PlanActivity == null) ? null : new PlanActivity() { _id = structplan.PlanActivity._id, _idAccount = _user._idAccount, Status = EnumStatus.Enabled, Name = structplan.PlanActivity.Name },
                  TypeAction = structplan.TypeAction,
                  TypeResponsible = structplan.TypeResponsible
                }));
              }
            }
          }
        }
        else if (sourceplan == EnumSourcePlan.Schooling)
        {
          foreach (var item in monitoring.Schoolings)
          {
            var listSchoolings = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                deadline = plan.Deadline;

                if (plan.StructPlans == null)
                  plan.StructPlans = new List<StructPlan>();

                plan.StructPlans.Add(AddStructPlan(new StructPlan()
                {
                  _id = structplan._id,
                  Course = (structplan.Course == null) ? null : serviceCourse.GetAll(p => p._id == structplan.Course._id).FirstOrDefault(),
                  PlanActivity = (structplan.PlanActivity == null) ? null : new PlanActivity() { _id = structplan.PlanActivity._id, _idAccount = _user._idAccount, Status = EnumStatus.Enabled, Name = structplan.PlanActivity.Name },
                  TypeAction = structplan.TypeAction,
                  TypeResponsible = structplan.TypeResponsible
                }));
              }
            }
          }
        }
        else
        {
          foreach (var item in monitoring.SkillsCompany)
          {
            var listSkillsCompany = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                deadline = plan.Deadline;

                if (plan.StructPlans == null)
                  plan.StructPlans = new List<StructPlan>();

                plan.StructPlans.Add(AddStructPlan(new StructPlan()
                {
                  _id = structplan._id,
                  Course = (structplan.Course == null) ? null : serviceCourse.GetAll(p => p._id == structplan.Course._id).FirstOrDefault(),
                  PlanActivity = (structplan.PlanActivity == null) ? null : new PlanActivity() { _id = structplan.PlanActivity._id, _idAccount = _user._idAccount, Status = EnumStatus.Enabled, Name = structplan.PlanActivity.Name },
                  TypeAction = structplan.TypeAction,
                  TypeResponsible = structplan.TypeResponsible
                }));
              }
            }
          }
        }

        if (structplan.Course != null)
        {
          var trainingPlan = new TrainingPlan
          {
            Course = (structplan.Course == null) ? null : serviceCourse.GetAll(p => p._id == structplan.Course._id).FirstOrDefault(),
            Deadline = deadline,
            Origin = EnumOrigin.Monitoring,
            Person = monitoring.Person,
            Include = DateTime.Now,
            StatusTrainingPlan = EnumStatusTrainingPlan.Open
          };
          serviceMandatoryTraining.NewTrainingPlanInternal(trainingPlan);
        }

        serviceMonitoring.Update(monitoring, null);
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
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

        //verify plan;
        if (sourceplan == EnumSourcePlan.Activite)
        {
          foreach (var item in monitoring.Activities)
          {
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == idstructplan)
                  {
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
                }
              }
            }
          }
        }
        else if (sourceplan == EnumSourcePlan.Schooling)
        {
          foreach (var item in monitoring.Schoolings)
          {
            var listSchoolings = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == idstructplan)
                  {
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
                }
              }
            }
          }
        }
        else
        {
          foreach (var item in monitoring.SkillsCompany)
          {
            var listSkillsCompany = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == idstructplan)
                  {
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
                }
              }
            }
          }
        }

        return null;
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
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

        //verify plan;
        if (sourceplan == EnumSourcePlan.Activite)
        {
          foreach (var item in monitoring.Activities)
          {
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == structplanedit._id)
                  {

                    if ((structplan.Course == null) & (structplanedit.Course != null))
                    {
                      var trainingPlan = new TrainingPlan
                      {
                        Course = structplan.Course ?? null,
                        Deadline = plan.Deadline,
                        Origin = EnumOrigin.Monitoring,
                        Person = monitoring.Person,
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
                    serviceMonitoring.Update(monitoring, null);
                    return "update";
                  }
                }
              }
            }
          }
        }
        else if (sourceplan == EnumSourcePlan.Schooling)
        {
          foreach (var item in monitoring.Schoolings)
          {
            var listSchoolings = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == structplanedit._id)
                  {

                    if ((structplan.Course == null) & (structplanedit.Course != null))
                    {
                      var trainingPlan = new TrainingPlan
                      {
                        Course = structplan.Course ?? null,
                        Deadline = plan.Deadline,
                        Origin = EnumOrigin.Monitoring,
                        Person = monitoring.Person,
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
                    serviceMonitoring.Update(monitoring, null);
                    return "update";
                  }
                }
              }
            }
          }
        }
        else
        {
          foreach (var item in monitoring.SkillsCompany)
          {
            var listSkillsCompany = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == structplanedit._id)
                  {

                    if ((structplan.Course == null) & (structplanedit.Course != null))
                    {
                      var trainingPlan = new TrainingPlan
                      {
                        Course = structplan.Course ?? null,
                        Deadline = plan.Deadline,
                        Origin = EnumOrigin.Monitoring,
                        Person = monitoring.Person,
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
                      PlanActivity = (structplanedit.PlanActivity ==null)?null: new PlanActivity() { _id = structplanedit.PlanActivity._id, Name = structplanedit.PlanActivity.Name },
                      TypeAction = structplanedit.TypeAction,
                      TypeResponsible = structplanedit.TypeResponsible
                    });
                    serviceMonitoring.Update(monitoring, null);
                    return "update";
                  }
                }
              }
            }
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
              UserInclude = (item.UserInclude == null) ? null :
              servicePerson.GetAll(p => p._id == item.UserInclude).FirstOrDefault(),
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
              UserInclude = (item.UserInclude == null) ? null :
              servicePerson.GetAll(p => p._id == item.UserInclude).FirstOrDefault(),
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

        if (_user._idPerson == person._id)
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
            UpdatePlan(idmonitoring, planUpdate);
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
        var detail = serviceMonitoring.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), p.Person, p._id }).FirstOrDefault();

        var detailSchoolings = serviceMonitoring.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), p.Person, p._id }).FirstOrDefault();

        var detailSkillsCompany = serviceMonitoring.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), p.Person, p._id }).FirstOrDefault();

        if (detail == null)
          return new ViewGetPlan() { _id = idmonitoring };

        ViewPlan view = new ViewPlan();
        bool exists = false;

        foreach (var plan in detail.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
            {
              exists = true;
              view.DateInclude = res.DateInclude;
              view.Deadline = res.Deadline;
              view.Name = res.Name;
              view.Description = res.Description;
              view.SourcePlan = res.SourcePlan;
              view.StatusPlan = res.StatusPlan;
              view.StatusPlanApproved = res.StatusPlanApproved;
              view.UserInclude = res.UserInclude;
              view.TypeAction = res.TypeAction;
              view.TypePlan = res.TypePlan;
              view.Evaluation = res.Evaluation;
              view.Skills = res.Skills.OrderBy(p => p.Name).ToList();
              view._id = res._id;
              view._idAccount = res._idAccount;
              view.IdMonitoring = detail._id;
              view.TextEnd = res.TextEnd;
              view.TextEndManager = res.TextEndManager;
              view.Status = res.Status;
              view.DateEnd = res.DateEnd;
              view.Attachments = res.Attachments;
              view.NewAction = res.NewAction;
            }
            //else if ((res.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (res.Name == view.Name))
            else if ((res.Name == view.Name))
              view.PlanNew = res;
          }
        }

        if (exists)
          return new ViewGetPlan()
          {
            _id = view._id,
            Name = view.Name,
            DateInclude = view.DateInclude,
            Deadline = view.Deadline,
            Description = view.Description,
            Skills = view.Skills?.Select(p => new ViewListSkill()
            {
              _id = p._id,
              TypeSkill = p.TypeSkill,
              Concept = p.Concept,
              Name = p.Name
            }).ToList(),
            UserInclude = view.UserInclude?._id,
            TypePlan = view.TypePlan,
            _idPerson = view.IdPerson,
            NamePerson = view.NamePerson,
            SourcePlan = view.SourcePlan,
            IdMonitoring = view.IdMonitoring,
            Evaluation = view.Evaluation,
            StatusPlan = view.StatusPlan,
            TypeAction = view.TypeAction,
            StatusPlanApproved = view.StatusPlanApproved,
            TextEnd = view.TextEnd,
            TextEndManager = view.TextEndManager,
            Status = view.Status,
            DateEnd = view.DateEnd,
            NewAction = view.NewAction,
            Attachments = (view.Attachments == null)? null : view.Attachments.Select(p => new ViewCrudAttachmentField() {
              Name  = p.Name,
              Url = p.Url,
              _idAttachment = p._idAttachment
            }).ToList(),
            Bomb = GetBomb((DateTime.Parse(view.Deadline.ToString()) - DateTime.Now).Days),
            PlanNew = (view.PlanNew == null) ? null : new ViewCrudPlan()
            {
              _id = view.PlanNew._id,
              TypePlan = view.PlanNew.TypePlan,
              SourcePlan = view.PlanNew.SourcePlan,
              Name = view.PlanNew.Name,
              Deadline = view.PlanNew.Deadline,
              Description = view.PlanNew.Description,
              Skills = view.PlanNew.Skills?.Select(p => new ViewListSkill()
              {
                _id = p._id,
                Name = p.Name,
                Concept = p.Name,
                TypeSkill = p.TypeSkill
              }).ToList()
            }
          };

        foreach (var plan in detailSchoolings.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
            {
              exists = true;
              view.DateInclude = res.DateInclude;
              view.Deadline = res.Deadline;
              view.Name = res.Name;
              view.Description = res.Description;
              view.SourcePlan = res.SourcePlan;
              view.StatusPlan = res.StatusPlan;
              view.StatusPlanApproved = res.StatusPlanApproved;
              view.UserInclude = res.UserInclude;
              view.TypeAction = res.TypeAction;
              view.TypePlan = res.TypePlan;
              view.Evaluation = res.Evaluation;
              view.Skills = res.Skills;
              view._id = res._id;
              view._idAccount = res._idAccount;
              view.IdMonitoring = detail._id;
              view.TextEnd = res.TextEnd;
              view.TextEndManager = res.TextEndManager;
              view.Status = res.Status;
              view.DateEnd = res.DateEnd;
              view.Attachments = res.Attachments;
              view.NewAction = res.NewAction;
            }
            //else if ((res.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (res.Name == view.Name))
            else if ((res.Name == view.Name))
              view.PlanNew = res;
          }
        }

        if (exists)
          return new ViewGetPlan()
          {
            _id = view._id,
            Name = view.Name,
            DateInclude = view.DateInclude,
            Deadline = view.Deadline,
            Description = view.Description,
            Skills = view.Skills?.Select(p => new ViewListSkill()
            {
              _id = p._id,
              TypeSkill = p.TypeSkill,
              Concept = p.Concept,
              Name = p.Name
            }).ToList(),
            UserInclude = view.UserInclude?._id,
            TypePlan = view.TypePlan,
            _idPerson = view.IdPerson,
            NamePerson = view.NamePerson,
            SourcePlan = view.SourcePlan,
            IdMonitoring = view.IdMonitoring,
            Evaluation = view.Evaluation,
            StatusPlan = view.StatusPlan,
            TypeAction = view.TypeAction,
            StatusPlanApproved = view.StatusPlanApproved,
            TextEnd = view.TextEnd,
            TextEndManager = view.TextEndManager,
            Status = view.Status,
            DateEnd = view.DateEnd,
            NewAction = view.NewAction,
            Attachments = (view.Attachments == null) ? null : view.Attachments.Select(p => new ViewCrudAttachmentField()
            {
              Name = p.Name,
              Url = p.Url,
              _idAttachment = p._idAttachment
            }).ToList(),
            Bomb = GetBomb((DateTime.Parse(view.Deadline.ToString()) - DateTime.Now).Days),
            PlanNew = (view.PlanNew == null) ? null : new ViewCrudPlan()
            {
              _id = view.PlanNew._id,
              TypePlan = view.PlanNew.TypePlan,
              SourcePlan = view.PlanNew.SourcePlan,
              Name = view.PlanNew.Name,
              Deadline = view.PlanNew.Deadline,
              Description = view.PlanNew.Description,
              Skills = view.PlanNew.Skills?.Select(p => new ViewListSkill()
              {
                _id = p._id,
                Name = p.Name,
                Concept = p.Name,
                TypeSkill = p.TypeSkill
              }).ToList()
            }
          };

        foreach (var plan in detailSkillsCompany.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
            {
              exists = true;
              view.DateInclude = res.DateInclude;
              view.Deadline = res.Deadline;
              view.Name = res.Name;
              view.Description = res.Description;
              view.SourcePlan = res.SourcePlan;
              view.StatusPlan = res.StatusPlan;
              view.StatusPlanApproved = res.StatusPlanApproved;
              view.UserInclude = res.UserInclude;
              view.TypeAction = res.TypeAction;
              view.TypePlan = res.TypePlan;
              view.Evaluation = res.Evaluation;
              view.Skills = res.Skills;
              view._id = res._id;
              view._idAccount = res._idAccount;
              view.IdMonitoring = detail._id;
              view.TextEnd = res.TextEnd;
              view.TextEndManager = res.TextEndManager;
              view.Status = res.Status;
              view.DateEnd = res.DateEnd;
              view.Attachments = res.Attachments;
              view.NewAction = res.NewAction;
            }
            //else if ((res.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (res.Name == view.Name))
            else if ((res.Name == view.Name))
              view.PlanNew = res;
          }
        }

        if (exists)
          return new ViewGetPlan()
          {
            _id = view._id,
            Name = view.Name,
            DateInclude = view.DateInclude,
            Deadline = view.Deadline,
            Description = view.Description,
            Skills = view.Skills?.Select(p => new ViewListSkill()
            {
              _id = p._id,
              TypeSkill = p.TypeSkill,
              Concept = p.Concept,
              Name = p.Name
            }).ToList(),
            UserInclude = view.UserInclude?._id,
            TypePlan = view.TypePlan,
            _idPerson = view.IdPerson,
            NamePerson = view.NamePerson,
            SourcePlan = view.SourcePlan,
            IdMonitoring = view.IdMonitoring,
            Evaluation = view.Evaluation,
            StatusPlan = view.StatusPlan,
            TypeAction = view.TypeAction,
            StatusPlanApproved = view.StatusPlanApproved,
            TextEnd = view.TextEnd,
            TextEndManager = view.TextEndManager,
            Status = view.Status,
            DateEnd = view.DateEnd,
            NewAction = view.NewAction,
            Attachments = (view.Attachments == null) ? null : view.Attachments.Select(p => new ViewCrudAttachmentField()
            {
              Name = p.Name,
              Url = p.Url,
              _idAttachment = p._idAttachment
            }).ToList(),
            Bomb = GetBomb((DateTime.Parse(view.Deadline.ToString()) - DateTime.Now).Days),
            PlanNew = (view.PlanNew == null) ? null : new ViewCrudPlan()
            {
              _id = view.PlanNew._id,
              TypePlan = view.PlanNew.TypePlan,
              SourcePlan = view.PlanNew.SourcePlan,
              Name = view.PlanNew.Name,
              Deadline = view.PlanNew.Deadline,
              Description = view.PlanNew.Description,
              Skills = view.PlanNew.Skills?.Select(p => new ViewListSkill()
              {
                _id = p._id,
                Name = p.Name,
                Concept = p.Name,
                TypeSkill = p.TypeSkill
              }).ToList()
            }
          };

        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }



    #endregion

    #region Old



    public List<ViewPlanShort> ListPlansPersonOld(ref long total, string id, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewPlan> result = new List<ViewPlan>();

        var detail = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
        .Select(p => new { Plans = p.Activities.Select(x => x.Plans), p.Person, p._id }).ToList();


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
                NamePerson = item.Person.User.Name,
                SourcePlan = res.SourcePlan,
                IdMonitoring = item._id,
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
        }


        var detailSchool = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), p.Person, p._id }).ToList();
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
                NamePerson = item.Person.User.Name,
                SourcePlan = res.SourcePlan,
                IdMonitoring = item._id,
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
        }


        var detailSkills = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), p.Person, p._id }).ToList();

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
                NamePerson = item.Person.User.Name,
                SourcePlan = res.SourcePlan,
                IdMonitoring = item._id,
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
    public List<ViewPlan> ListPlansOld(ref long total, string id, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewPlan> result = new List<ViewPlan>();

        var detail = (from monitoring in
          serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
        .Select(p => new { Plans = p.Activities.Select(x => x.Plans), p.Person, p._id }).ToList()
                      join person in servicePerson.GetAll() on monitoring.Person._id equals person._id
                      where person.Manager._id == id
                      select monitoring
        ).ToList();


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
                NamePerson = item.Person.User.Name,
                SourcePlan = res.SourcePlan,
                IdMonitoring = item._id,
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
        }


        var detailSchool = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), p.Person, p._id }).ToList();
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
                NamePerson = item.Person.User.Name,
                SourcePlan = res.SourcePlan,
                IdMonitoring = item._id,
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
        }


        var detailSkills = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), p.Person, p._id }).ToList();

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
                NamePerson = item.Person.User.Name,
                SourcePlan = res.SourcePlan,
                IdMonitoring = item._id,
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
        }


        result = result.Where(p => p.StatusPlanApproved != EnumStatusPlanApproved.Invisible).ToList();


        total = result.Count();
        result = result.Skip(skip).Take(count).OrderBy(p => p.SourcePlan).ThenBy(p => p.Deadline).ToList();
        var viewReturn = result.GroupBy(i => i.Name).Select(g => new ViewPlanShort
        {
          Name = g.Key,
          LastAction = g.Max(row => row.Deadline)
        }).ToList();

        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewPlan> ListPlansOld(ref long total, string id, string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewPlan> result = new List<ViewPlan>();

        if (activities == 1)
        {
          var detail = (from monitoring in
         serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
       .Select(p => new { Plans = p.Activities.Select(x => x.Plans), p.Person, p._id }).ToList()
                        join person in servicePerson.GetAll() on monitoring.Person._id equals person._id
                        where person.Manager._id == id
                        select monitoring
       ).ToList();

          /*var detail = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), Person = p.Person, _id = p._id }).ToList();*/


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
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
          }
        }

        if (schooling == 1)
        {
          //var detailSchool = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          //  .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), Person = p.Person, _id = p._id }).ToList();

          var detailSchool = (from monitoring in
         serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
       .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), p.Person, p._id }).ToList()
                              join person in servicePerson.GetAll() on monitoring.Person._id equals person._id
                              where person.Manager._id == id
                              select monitoring
       ).ToList();

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
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
          }
        }

        if (skillcompany == 1)
        {
          //var detailSkills = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Manager._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          //  .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), Person = p.Person, _id = p._id }).ToList();

          var detailSkills = (from monitoring in
         serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
       .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), p.Person, p._id }).ToList()
                              join person in servicePerson.GetAll() on monitoring.Person._id equals person._id
                              where person.Manager._id == id
                              select monitoring
       ).ToList();

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
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
          }
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

    public List<ViewPlanStruct> ListPlansStructOld(ref long total, string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte structplan)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewPlanStruct> result = new List<ViewPlanStruct>();

        if (activities == 1)
        {
          var detail = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End)
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), p.Person, p._id }).ToList();


          foreach (var item in detail)
          {
            foreach (var plan in item.Plans)
            {
              foreach (var res in plan)
              {
                var view = new ViewPlanStruct
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
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
                    view.StructPlans = res.StructPlans;
                }
                result.Add(view);
              }
            }
          }
        }

        if (schooling == 1)
        {
          var detailSchool = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End)
            .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), p.Person, p._id }).ToList();
          foreach (var item in detailSchool)
          {
            foreach (var plan in item.Plans)
            {
              foreach (var res in plan)
              {
                var view = new ViewPlanStruct
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
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
                    view.StructPlans = res.StructPlans;
                }
                result.Add(view);
              }
            }
          }
        }

        if (skillcompany == 1)
        {
          var detailSkills = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End)
            .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), p.Person, p._id }).ToList();

          foreach (var item in detailSkills)
          {
            foreach (var plan in item.Plans)
            {
              foreach (var res in plan)
              {
                var view = new ViewPlanStruct
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
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
                    view.StructPlans = res.StructPlans;
                }
                result.Add(view);
              }
            }
          }
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

    public List<ViewPlan> ListPlansPersonOld(ref long total, string id, string filter, int count, int page, byte activities, byte skillcompany, byte schooling, byte open, byte expired, byte end, byte wait)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewPlan> result = new List<ViewPlan>();

        if (activities == 1)
        {
          var detail = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), p.Person, p._id }).ToList();


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
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
          }
        }

        if (schooling == 1)
        {
          var detailSchool = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
            .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), p.Person, p._id }).ToList();
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
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
          }
        }

        if (skillcompany == 1)
        {
          var detailSkills = serviceMonitoring.GetAll(p => p.StatusMonitoring == EnumStatusMonitoring.End & p.Person._id == id & p.Person.User.Name.ToUpper().Contains(filter.ToUpper()))
            .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), p.Person, p._id }).ToList();

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
                  NamePerson = item.Person.User.Name,
                  SourcePlan = res.SourcePlan,
                  IdMonitoring = item._id,
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
          }
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

    public string UpdatePlanOld(string idmonitoring, Plan viewPlan)
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
            var listActivities = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == viewPlan._id)
              {
                UpdatePlan(viewPlan, monitoring.Person);
                listActivities.Add(viewPlan);
              }
              else if ((plan.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (viewPlan.NewAction == EnumNewAction.Yes))
              {
                if (viewPlan.NewAction == EnumNewAction.Yes)
                  plan.StatusPlanApproved = EnumStatusPlanApproved.Open;

                UpdatePlan(plan, monitoring.Person);
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
            var listSchoolings = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == viewPlan._id)
              {
                UpdatePlan(viewPlan, monitoring.Person);
                listSchoolings.Add(viewPlan);
              }
              else if ((plan.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (viewPlan.NewAction == EnumNewAction.Yes))
              {
                if (viewPlan.NewAction == EnumNewAction.Yes)
                  plan.StatusPlanApproved = EnumStatusPlanApproved.Open;

                UpdatePlan(plan, monitoring.Person);
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
            var listSkillsCompany = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == viewPlan._id)
              {
                UpdatePlan(viewPlan, monitoring.Person);
                listSkillsCompany.Add(viewPlan);
              }
              else if ((plan.StatusPlanApproved == EnumStatusPlanApproved.Invisible))
              {
                if (viewPlan.NewAction == EnumNewAction.Yes)
                  plan.StatusPlanApproved = EnumStatusPlanApproved.Open;

                UpdatePlan(plan, monitoring.Person);
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

    public string NewPlanOld(string idmonitoring, string idplanold, Plan viewPlan)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

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


        serviceMonitoring.Update(monitoring, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewStructPlanOld(string idmonitoring, string idplan, EnumSourcePlan sourceplan, StructPlan structplan)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        DateTime? deadline = DateTime.Now;

        //verify plan;
        if (sourceplan == EnumSourcePlan.Activite)
        {
          foreach (var item in monitoring.Activities)
          {
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                deadline = plan.Deadline;

                if (plan.StructPlans == null)
                  plan.StructPlans = new List<StructPlan>();

                plan.StructPlans.Add(AddStructPlan(structplan));
              }
            }
          }
        }
        else if (sourceplan == EnumSourcePlan.Schooling)
        {
          foreach (var item in monitoring.Schoolings)
          {
            var listSchoolings = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                deadline = plan.Deadline;

                if (plan.StructPlans == null)
                  plan.StructPlans = new List<StructPlan>();

                plan.StructPlans.Add(AddStructPlan(structplan));
              }
            }
          }
        }
        else
        {
          foreach (var item in monitoring.SkillsCompany)
          {
            var listSkillsCompany = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                deadline = plan.Deadline;

                if (plan.StructPlans == null)
                  plan.StructPlans = new List<StructPlan>();

                plan.StructPlans.Add(AddStructPlan(structplan));
              }
            }
          }
        }

        if (structplan.Course != null)
        {
          var trainingPlan = new TrainingPlan
          {
            Course = structplan.Course,
            Deadline = deadline,
            Origin = EnumOrigin.Monitoring,
            Person = monitoring.Person,
            Include = DateTime.Now,
            StatusTrainingPlan = EnumStatusTrainingPlan.Open
          };
          serviceMandatoryTraining.NewTrainingPlanInternal(trainingPlan);
        }

        serviceMonitoring.Update(monitoring, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public StructPlan GetStructPlanOld(string idmonitoring, string idplan, EnumSourcePlan sourceplan, string idstructplan)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

        //verify plan;
        if (sourceplan == EnumSourcePlan.Activite)
        {
          foreach (var item in monitoring.Activities)
          {
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == idstructplan)
                  {
                    return structplan;
                  }
                }
              }
            }
          }
        }
        else if (sourceplan == EnumSourcePlan.Schooling)
        {
          foreach (var item in monitoring.Schoolings)
          {
            var listSchoolings = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == idstructplan)
                  {
                    return structplan;
                  }
                }
              }
            }
          }
        }
        else
        {
          foreach (var item in monitoring.SkillsCompany)
          {
            var listSkillsCompany = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == idstructplan)
                  {
                    return structplan;
                  }
                }
              }
            }
          }
        }

        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateStructPlanOld(string idmonitoring, string idplan, EnumSourcePlan sourceplan, StructPlan structplanedit)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

        //verify plan;
        if (sourceplan == EnumSourcePlan.Activite)
        {
          foreach (var item in monitoring.Activities)
          {
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == structplanedit._id)
                  {

                    if ((structplan.Course == null) & (structplanedit.Course != null))
                    {
                      var trainingPlan = new TrainingPlan
                      {
                        Course = structplan.Course,
                        Deadline = plan.Deadline,
                        Origin = EnumOrigin.Monitoring,
                        Person = monitoring.Person,
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
                    plan.StructPlans.Add(structplanedit);
                    serviceMonitoring.Update(monitoring, null);
                    return "update";
                  }
                }
              }
            }
          }
        }
        else if (sourceplan == EnumSourcePlan.Schooling)
        {
          foreach (var item in monitoring.Schoolings)
          {
            var listSchoolings = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == structplanedit._id)
                  {

                    if ((structplan.Course == null) & (structplanedit.Course != null))
                    {
                      var trainingPlan = new TrainingPlan
                      {
                        Course = structplan.Course,
                        Deadline = plan.Deadline,
                        Origin = EnumOrigin.Monitoring,
                        Person = monitoring.Person,
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
                    plan.StructPlans.Add(structplanedit);
                    serviceMonitoring.Update(monitoring, null);
                    return "update";
                  }
                }
              }
            }
          }
        }
        else
        {
          foreach (var item in monitoring.SkillsCompany)
          {
            var listSkillsCompany = new List<Plan>();
            foreach (var plan in item.Plans)
            {
              if (plan._id == idplan)
              {
                foreach (var structplan in plan.StructPlans)
                {
                  if (structplan._id == structplanedit._id)
                  {

                    if ((structplan.Course == null) & (structplanedit.Course != null))
                    {
                      var trainingPlan = new TrainingPlan
                      {
                        Course = structplan.Course,
                        Deadline = plan.Deadline,
                        Origin = EnumOrigin.Monitoring,
                        Person = monitoring.Person,
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
                    plan.StructPlans.Add(structplanedit);
                    serviceMonitoring.Update(monitoring, null);
                    return "update";
                  }
                }
              }
            }
          }
        }

        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewPlanViewOld(string idmonitoring, Plan planOld, Plan viewPlan)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

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
                UpdatePlan(planOld, monitoring.Person);
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
                UpdatePlan(planOld, monitoring.Person);
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
                UpdatePlan(planOld, monitoring.Person);
                listSkillsCompany.Add(planOld);
                listSkillsCompany.Add(viewPlan);
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

    public string NewUpdatePlanOld(string idmonitoring, List<ViewPlanNewUp> viewPlan)
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
              TextEndManager = item.TextEndManager,
              DateEnd = item.DateEnd,
              Evaluation = item.Evaluation,
              Result = item.Result,
              StatusPlanApproved = EnumStatusPlanApproved.Open,
              Status = item.Status,
              NewAction = item.NewAction
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
              TextEndManager = item.TextEndManager,
              DateEnd = item.DateEnd,
              Evaluation = item.Evaluation,
              Result = item.Result,
              StatusPlanApproved = item.StatusPlanApproved,
              Status = item.Status,
              NewAction = item.NewAction
            };
        }

        if (_user._idPerson == person._id)
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
            UpdatePlan(idmonitoring, planUpdate);
        }
        return "newupdate";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public ViewPlan GetPlanOld(string idmonitoring, string idplan)
    {
      try
      {
        var detail = serviceMonitoring.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), p.Person, p._id }).FirstOrDefault();

        var detailSchoolings = serviceMonitoring.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), p.Person, p._id }).FirstOrDefault();

        var detailSkillsCompany = serviceMonitoring.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), p.Person, p._id }).FirstOrDefault();

        if (detail == null)
          return new ViewPlan() { _idAccount = serviceMonitoring._user._idAccount, _id = idmonitoring };

        ViewPlan view = new ViewPlan();
        bool exists = false;

        foreach (var plan in detail.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
            {
              exists = true;
              view.DateInclude = res.DateInclude;
              view.Deadline = res.Deadline;
              view.Name = res.Name;
              view.Description = res.Description;
              view.SourcePlan = res.SourcePlan;
              view.StatusPlan = res.StatusPlan;
              view.StatusPlanApproved = res.StatusPlanApproved;
              view.UserInclude = res.UserInclude;
              view.TypeAction = res.TypeAction;
              view.TypePlan = res.TypePlan;
              view.Evaluation = res.Evaluation;
              view.Skills = res.Skills.OrderBy(p => p.Name).ToList();
              view._id = res._id;
              view._idAccount = res._idAccount;
              view.IdMonitoring = detail._id;
              view.TextEnd = res.TextEnd;
              view.TextEndManager = res.TextEndManager;
              view.Status = res.Status;
              view.DateEnd = res.DateEnd;
              view.Attachments = res.Attachments;
              view.NewAction = res.NewAction;
            }
            else if ((res.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (res.Name == view.Name))
              view.PlanNew = res;
          }
        }

        if (exists)
          return view;

        foreach (var plan in detailSchoolings.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
            {
              exists = true;
              view.DateInclude = res.DateInclude;
              view.Deadline = res.Deadline;
              view.Name = res.Name;
              view.Description = res.Description;
              view.SourcePlan = res.SourcePlan;
              view.StatusPlan = res.StatusPlan;
              view.StatusPlanApproved = res.StatusPlanApproved;
              view.UserInclude = res.UserInclude;
              view.TypeAction = res.TypeAction;
              view.TypePlan = res.TypePlan;
              view.Evaluation = res.Evaluation;
              view.Skills = res.Skills;
              view._id = res._id;
              view._idAccount = res._idAccount;
              view.IdMonitoring = detail._id;
              view.TextEnd = res.TextEnd;
              view.TextEndManager = res.TextEndManager;
              view.Status = res.Status;
              view.DateEnd = res.DateEnd;
              view.Attachments = res.Attachments;
              view.NewAction = res.NewAction;
            }
            else if ((res.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (res.Name == view.Name))
              view.PlanNew = res;
          }
        }

        if (exists)
          return view;

        foreach (var plan in detailSkillsCompany.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
            {
              exists = true;
              view.DateInclude = res.DateInclude;
              view.Deadline = res.Deadline;
              view.Name = res.Name;
              view.Description = res.Description;
              view.SourcePlan = res.SourcePlan;
              view.StatusPlan = res.StatusPlan;
              view.StatusPlanApproved = res.StatusPlanApproved;
              view.UserInclude = res.UserInclude;
              view.TypeAction = res.TypeAction;
              view.TypePlan = res.TypePlan;
              view.Evaluation = res.Evaluation;
              view.Skills = res.Skills;
              view._id = res._id;
              view._idAccount = res._idAccount;
              view.IdMonitoring = detail._id;
              view.TextEnd = res.TextEnd;
              view.TextEndManager = res.TextEndManager;
              view.Status = res.Status;
              view.DateEnd = res.DateEnd;
              view.Attachments = res.Attachments;
              view.NewAction = res.NewAction;
            }
            else if ((res.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (res.Name == view.Name))
              view.PlanNew = res;
          }
        }

        if (exists)
          return view;

        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewPlanStruct GetPlanStructOld(string idmonitoring, string idplan)
    {
      try
      {
        var detail = serviceMonitoring.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.Activities.Select(x => x.Plans), p.Person, p._id }).FirstOrDefault();

        var detailSchoolings = serviceMonitoring.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.Schoolings.Select(x => x.Plans), p.Person, p._id }).FirstOrDefault();

        var detailSkillsCompany = serviceMonitoring.GetAll(p => p._id == idmonitoring)
          .Select(p => new { Plans = p.SkillsCompany.Select(x => x.Plans), p.Person, p._id }).FirstOrDefault();

        if (detail == null)
          return new ViewPlanStruct() { _idAccount = serviceMonitoring._user._idAccount, _id = idmonitoring };

        ViewPlanStruct view = new ViewPlanStruct();
        bool exists = false;

        foreach (var plan in detail.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
            {
              exists = true;
              view.DateInclude = res.DateInclude;
              view.Deadline = res.Deadline;
              view.Name = res.Name;
              view.Description = res.Description;
              view.SourcePlan = res.SourcePlan;
              view.StatusPlan = res.StatusPlan;
              view.StatusPlanApproved = res.StatusPlanApproved;
              view.UserInclude = res.UserInclude;
              view.TypeAction = res.TypeAction;
              view.TypePlan = res.TypePlan;
              view.Evaluation = res.Evaluation;
              view.Skills = res.Skills.OrderBy(p => p.Name).ToList();
              view._id = res._id;
              view._idAccount = res._idAccount;
              view.IdMonitoring = detail._id;
              view.TextEnd = res.TextEnd;
              view.TextEndManager = res.TextEndManager;
              view.Status = res.Status;
              view.DateEnd = res.DateEnd;
              view.Attachments = res.Attachments;
              view.NewAction = res.NewAction;
              if (res.StructPlans != null)
              {
                if (res.StructPlans.Count() == 0)
                  view.StructPlans = null;
                else
                  view.StructPlans = res.StructPlans;
              }
            }
            else if ((res.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (res.Name == view.Name))
              view.PlanNew = res;
          }
        }

        if (exists)
          return view;

        foreach (var plan in detailSchoolings.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
            {
              exists = true;
              view.DateInclude = res.DateInclude;
              view.Deadline = res.Deadline;
              view.Name = res.Name;
              view.Description = res.Description;
              view.SourcePlan = res.SourcePlan;
              view.StatusPlan = res.StatusPlan;
              view.StatusPlanApproved = res.StatusPlanApproved;
              view.UserInclude = res.UserInclude;
              view.TypeAction = res.TypeAction;
              view.TypePlan = res.TypePlan;
              view.Evaluation = res.Evaluation;
              view.Skills = res.Skills;
              view._id = res._id;
              view._idAccount = res._idAccount;
              view.IdMonitoring = detail._id;
              view.TextEnd = res.TextEnd;
              view.TextEndManager = res.TextEndManager;
              view.Status = res.Status;
              view.DateEnd = res.DateEnd;
              view.Attachments = res.Attachments;
              view.NewAction = res.NewAction;
              if (res.StructPlans != null)
              {
                if (res.StructPlans.Count() == 0)
                  view.StructPlans = null;
                else
                  view.StructPlans = res.StructPlans;
              }
            }
            else if ((res.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (res.Name == view.Name))
              view.PlanNew = res;
          }
        }

        if (exists)
          return view;

        foreach (var plan in detailSkillsCompany.Plans)
        {
          foreach (var res in plan)
          {
            if (res._id == idplan)
            {
              exists = true;
              view.DateInclude = res.DateInclude;
              view.Deadline = res.Deadline;
              view.Name = res.Name;
              view.Description = res.Description;
              view.SourcePlan = res.SourcePlan;
              view.StatusPlan = res.StatusPlan;
              view.StatusPlanApproved = res.StatusPlanApproved;
              view.UserInclude = res.UserInclude;
              view.TypeAction = res.TypeAction;
              view.TypePlan = res.TypePlan;
              view.Evaluation = res.Evaluation;
              view.Skills = res.Skills;
              view._id = res._id;
              view._idAccount = res._idAccount;
              view.IdMonitoring = detail._id;
              view.TextEnd = res.TextEnd;
              view.TextEndManager = res.TextEndManager;
              view.Status = res.Status;
              view.DateEnd = res.DateEnd;
              view.Attachments = res.Attachments;
              view.NewAction = res.NewAction;
              if (res.StructPlans != null)
              {
                if (res.StructPlans.Count() == 0)
                  view.StructPlans = null;
                else
                  view.StructPlans = res.StructPlans;
              }

            }
            else if ((res.StatusPlanApproved == EnumStatusPlanApproved.Invisible) & (res.Name == view.Name))
              view.PlanNew = res;
          }
        }

        if (exists)
          return view;

        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<PlanActivity> ListPlanActivityOld(ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewPlan> result = new List<ViewPlan>();

        var detail = servicePlanActivity.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
        total = servicePlanActivity.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public PlanActivity GetPlanActivityOld(string id)
    {
      try
      {
        return servicePlanActivity.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewPlanActivityOld(PlanActivity model)
    {
      try
      {
        servicePlanActivity.Insert(model);
        return "add plan activity";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdatePlanActivityOld(PlanActivity model)
    {
      try
      {
        servicePlanActivity.Update(model, null);
        return "update";
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
