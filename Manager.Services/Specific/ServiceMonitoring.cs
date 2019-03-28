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
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Manager.Services.Specific
{
#pragma warning disable 1998
  public class ServiceMonitoring : Repository<Monitoring>, IServiceMonitoring
  {
    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceGeneric<Monitoring> serviceMonitoring;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Occupation> occupationService;
    private readonly ServiceGeneric<Plan> planService;
    private readonly ServiceLog logService;
    private readonly ServiceMailModel mailModelService;
    private readonly ServiceGeneric<MailMessage> mailMessageService;
    private readonly ServiceGeneric<MonitoringActivities> monitoringActivitiesService;
    private readonly ServiceGeneric<MailLog> mailService;
    private readonly ServiceLogMessages logMessagesService;

    public string path;

    #region Constructor

    public ServiceMonitoring(DataContext context, string pathToken)
      : base(context)
    {
      try
      {
        serviceMonitoring = new ServiceGeneric<Monitoring>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        planService = new ServiceGeneric<Plan>(context);
        occupationService = new ServiceGeneric<Occupation>(context);
        logService = new ServiceLog(_context);
        mailModelService = new ServiceMailModel(context);
        mailMessageService = new ServiceGeneric<MailMessage>(context);
        monitoringActivitiesService = new ServiceGeneric<MonitoringActivities>(context);
        mailService = new ServiceGeneric<MailLog>(context);
        logMessagesService = new ServiceLogMessages(context);
        serviceAuthentication = new ServiceAuthentication(context);
        path = pathToken;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      servicePerson._user = _user;
      serviceMonitoring._user = _user;
      planService._user = _user;
      logService._user = _user;
      mailModelService._user = _user;
      mailMessageService._user = _user;
      mailService._user = _user;
      occupationService._user = _user;
      monitoringActivitiesService._user = _user;
      logMessagesService._user = _user;
      mailModelService.SetUser(contextAccessor);
    }

    #endregion

    #region Old
    public List<Monitoring> ListMonitoringsEndOld(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(idmanager, "ListEnd");
        int skip = (count * (page - 1));
        var detail = serviceMonitoring.GetAll(p => p.Person.Manager._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).ToList();
        total = serviceMonitoring.GetAll(p => p.Person.Manager._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Monitoring> ListMonitoringsWaitOld(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        //LogSave(idmanager, "List");
        int skip = (count * (page - 1));
        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.TypeJourney == EnumTypeJourney.Monitoring & p.Manager._id == idmanager
        & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name)
        .ToList().Select(p => new { Person = p, Monitoring = serviceMonitoring.GetAll(x => x.StatusMonitoring != EnumStatusMonitoring.End & x.Person._id == p._id).FirstOrDefault() })
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

    public List<Monitoring> PersonMonitoringsEndOld(string idmanager)
    {
      try
      {
        LogSave(idmanager, "PersonEnd");
        return serviceMonitoring.GetAll(p => p.Person._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End).OrderBy(p => p.Person.User.Name).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Monitoring PersonMonitoringsWaitOld(string idmanager)
    {
      try
      {
        LogSave(idmanager, "PersonWait");
        var item = servicePerson.GetAll(p => p.TypeJourney == EnumTypeJourney.Monitoring & p._id == idmanager)
        .ToList().Select(p => new { Person = p, Monitoring = serviceMonitoring.GetAll(x => x.StatusMonitoring != EnumStatusMonitoring.End & x.Person._id == p._id).FirstOrDefault() })
        .FirstOrDefault();

        if (item == null)
          return null;

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

    public Monitoring GetMonitoringsOld(string id)
    {
      try
      {
        return serviceMonitoring.GetAll(p => p._id == id).ToList().Select(p => new Monitoring()
        {
          _id = p._id,
          _idAccount = p._idAccount,
          Status = p.Status,
          Person = p.Person,
          DateBeginPerson = p.DateBeginPerson,
          DateBeginManager = p.DateBeginManager,
          DateBeginEnd = p.DateBeginEnd,
          DateEndPerson = p.DateEndPerson,
          DateEndManager = p.DateEndManager,
          DateEndEnd = p.DateEndEnd,
          CommentsPerson = p.CommentsPerson,
          CommentsManager = p.CommentsManager,
          CommentsEnd = p.CommentsEnd,
          SkillsCompany = p.SkillsCompany.OrderBy(x => x.Skill.Name).ToList(),
          Schoolings = p.Schoolings.OrderBy(x => x.Schooling.Order).ToList(),
          Activities = p.Activities.OrderBy(x => x.Activities.Order).ToList(),
          StatusMonitoring = p.StatusMonitoring
        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public MonitoringActivities GetMonitoringActivitiesOld(string idmonitoring, string idactivitie)
    {
      try
      {
        return serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault().
          Activities.Where(p => p._id == idactivitie).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateMonitoringActivitiesOld(string idmonitoring, MonitoringActivities activitie)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

        foreach (var item in monitoring.Activities)
        {
          if (item._id == activitie._id)
          {
            monitoring.Activities.Remove(item);
            monitoring.Activities.Add(activitie);
            serviceMonitoring.Update(monitoring, null);
            return "update";
          }

        }
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddMonitoringActivitiesOld(string idmonitoring, Activitie activitie)
    {
      try
      {

        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (monitoring.Person._id == _user._idPerson)
          {
            monitoring.DateBeginPerson = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressPerson;
          }
          else
          {
            monitoring.DateBeginManager = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressManager;
          }
        }

        var monitoringActivitie = new MonitoringActivities();

        var activities = new List<Activitie>();

        foreach (var item in monitoring.Activities)
        {
          activities.Add(item.Activities);
        }

        long order = 1;
        try
        {
          order = activities.Max(p => p.Order) + 1;
          if (order == 0)
          {
            order = 1;
          }
        }
        catch (Exception)
        {
          order = 1;
        }


        activitie.Status = EnumStatus.Enabled;
        activitie._id = ObjectId.GenerateNewId().ToString();
        activitie._idAccount = _user._idAccount;
        activitie.Order = order;

        monitoringActivitie.Activities = activitie;
        monitoringActivitie.Status = EnumStatus.Enabled;
        monitoringActivitie._id = ObjectId.GenerateNewId().ToString();
        monitoringActivitie._idAccount = _user._idAccount;
        monitoringActivitie.TypeAtivitie = EnumTypeAtivitie.Individual;
        monitoringActivitie.Plans = new List<Plan>();
        monitoring.Activities.Add(monitoringActivitie);
        serviceMonitoring.Update(monitoring, null);
        return "add sucess";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Monitoring NewMonitoringOld(Monitoring monitoring, string idperson)
    {
      try
      {
        LogSave(monitoring.Person._id, "Monitoring Process");
        if (monitoring._id == null)
        {
          LoadMap(monitoring);

          monitoring.StatusMonitoring = EnumStatusMonitoring.Show;
          serviceMonitoring.Insert(monitoring);
        }
        else
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
            LoadMap(monitoring);

          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Wait)
          {
            monitoring.DateBeginEnd = DateTime.Now;
          }
          serviceMonitoring.Update(monitoring, null);
        }

        return monitoring;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateMonitoringOld(Monitoring monitoring, string idperson)
    {
      try
      {
        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (monitoring.Person._id == _user._idPerson)
          {
            monitoring.DateBeginPerson = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressPerson;
          }
          else
          {
            monitoring.DateBeginManager = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressManager;
          }
        }

        var userInclude = servicePerson.GetAll(p => p._id == idperson).FirstOrDefault();

        if (monitoring.Person._id != idperson)
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Wait)
          {
            monitoring.DateEndManager = DateTime.Now;
            Mail(monitoring.Person);
          }
        }
        else
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.End)
          {
            logMessagesService.NewLogMessage("Monitoring", " Monitoring realizado do colaborador " + monitoring.Person.User.Name, monitoring.Person);
            if ((monitoring.Activities.Where(p => p.Praise != null).Count() > 0)
              || (monitoring.SkillsCompany.Where(p => p.Praise != null).Count() > 0)
              || (monitoring.Schoolings.Where(p => p.Praise != null).Count() > 0))
            {
              logMessagesService.NewLogMessage("Monitoring", " Colaborador " + monitoring.Person.User.Name + " foi elogiado pelo gestor", monitoring.Person);
            }
            monitoring.DateEndEnd = DateTime.Now;
          }
          else if (monitoring.StatusMonitoring == EnumStatusMonitoring.WaitManager)
          {
            monitoring.DateEndPerson = DateTime.Now;
            MailManager(monitoring.Person);

          }
          else if (monitoring.StatusMonitoring == EnumStatusMonitoring.Disapproved)
          {
            MailDisApproval(monitoring.Person);

          }
        }


        ////verify plan;
        //foreach (var item in monitoring.Activities)
        //{
        //  if (item._id == null)
        //    item._id = ObjectId.GenerateNewId().ToString();

        //  var listActivities = new List<Plan>();
        //  foreach (var plan in item.Plans)
        //  {
        //    if (plan._id == null)
        //    {
        //      logMessagesService.NewLogMessage("Plano", " Ação de desenvolvimento acordada do colaborador " + monitoring.Person.User.Name, monitoring.Person);
        //      listActivities.Add(AddPlan(plan, userInclude));
        //    }

        //    else
        //    {
        //      UpdatePlan(plan);
        //      listActivities.Add(plan);
        //    }
        //  }
        //  item.Plans = listActivities;
        //}

        //foreach (var item in monitoring.Schoolings)
        //{
        //  var listSchoolings = new List<Plan>();
        //  foreach (var plan in item.Plans)
        //  {
        //    if (plan._id == null)
        //    {
        //      logMessagesService.NewLogMessage("Plano", " Ação de desenvolvimento acordada do colaborador " + monitoring.Person.User.Name, monitoring.Person);
        //      listSchoolings.Add(AddPlan(plan, userInclude));
        //    }

        //    else
        //    {
        //      UpdatePlan(plan);
        //      listSchoolings.Add(plan);
        //    }
        //  }
        //  item.Plans = listSchoolings;
        //}

        //foreach (var item in monitoring.SkillsCompany)
        //{
        //  var listSkillsCompany = new List<Plan>();
        //  foreach (var plan in item.Plans)
        //  {
        //    if (plan._id == null)
        //    {
        //      logMessagesService.NewLogMessage("Plano", " Ação de desenvolvimento acordada do colaborador " + monitoring.Person.User.Name, monitoring.Person);
        //      listSkillsCompany.Add(AddPlan(plan, userInclude));
        //    }

        //    else
        //    {
        //      UpdatePlan(plan);
        //      listSkillsCompany.Add(plan);
        //    }
        //  }
        //  item.Plans = listSkillsCompany;
        //}



        serviceMonitoring.Update(monitoring, null);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Plan> AddPlanOld(string idmonitoring, string iditem, Plan plan)
    {
      try
      {
        var userInclude = servicePerson.GetAll(p => p._id == _user._idPerson).FirstOrDefault();
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (monitoring.Person._id == _user._idPerson)
          {
            monitoring.DateBeginPerson = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressPerson;
          }
          else
          {
            monitoring.DateBeginManager = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressManager;
          }
        }

        logMessagesService.NewLogMessage("Plano", " Ação de desenvolvimento acordada do colaborador " + monitoring.Person.User.Name, monitoring.Person);


        var newPlan = AddPlan(plan, userInclude);

        if (plan.SourcePlan == EnumSourcePlan.Activite)
        {
          foreach (var item in monitoring.Activities)
          {
            if (item._id == iditem)
            {
              if (item.Plans == null)
                item.Plans = new List<Plan>();

              item.Plans.Add(newPlan);
              serviceMonitoring.Update(monitoring, null);
              return item.Plans;
            }
          }
        }

        if (plan.SourcePlan == EnumSourcePlan.Schooling)
        {
          foreach (var item in monitoring.Schoolings)
          {
            if (item._id == iditem)
            {
              if (item.Plans == null)
                item.Plans = new List<Plan>();

              item.Plans.Add(newPlan);
              serviceMonitoring.Update(monitoring, null);
              return item.Plans;
            }
          }
        }

        if (plan.SourcePlan == EnumSourcePlan.Skill)
        {
          foreach (var item in monitoring.SkillsCompany)
          {
            if (item._id == iditem)
            {
              if (item.Plans == null)
                item.Plans = new List<Plan>();

              item.Plans.Add(newPlan);
              serviceMonitoring.Update(monitoring, null);
              return item.Plans;
            }
          }
        }


        return null;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Plan> UpdatePlanOld(string idmonitoring, string iditem, Plan plan)
    {
      try
      {
        //var userInclude = servicePerson.GetAll(p => p._id == _user._idPerson).FirstOrDefault();
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        logMessagesService.NewLogMessage("Plano", " Ação de desenvolvimento acordada do colaborador " + monitoring.Person.User.Name, monitoring.Person);




        if (plan.SourcePlan == EnumSourcePlan.Activite)
        {
          foreach (var item in monitoring.Activities)
          {
            if (item._id == iditem)
            {
              foreach (var row in item.Plans)
              {
                if (row._id == plan._id)
                {
                  item.Plans.Remove(row);
                  item.Plans.Add(plan);
                  UpdatePlan(plan);
                  serviceMonitoring.Update(monitoring, null);
                  return item.Plans;
                }
              }


            }
          }
        }

        if (plan.SourcePlan == EnumSourcePlan.Schooling)
        {
          foreach (var item in monitoring.Schoolings)
          {
            if (item._id == iditem)
            {
              foreach (var row in item.Plans)
              {
                if (row._id == plan._id)
                {
                  item.Plans.Remove(row);
                  item.Plans.Add(plan);
                  UpdatePlan(plan);
                  serviceMonitoring.Update(monitoring, null);
                  return item.Plans;
                }
              }
            }
          }
        }

        if (plan.SourcePlan == EnumSourcePlan.Skill)
        {
          foreach (var item in monitoring.SkillsCompany)
          {
            if (item._id == iditem)
            {
              foreach (var row in item.Plans)
              {
                if (row._id == plan._id)
                {
                  item.Plans.Remove(row);
                  item.Plans.Add(plan);
                  UpdatePlan(plan);
                  serviceMonitoring.Update(monitoring, null);
                  return item.Plans;
                }
              }
            }
          }
        }

        serviceMonitoring.Update(monitoring, null);
        return null;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Skill> GetSkillsOld(string idperson)
    {
      try
      {
        var skills = servicePerson.GetAll(p => p._id == idperson).FirstOrDefault().Occupation.Group.Skills;
        var skillsgroup = servicePerson.GetAll(p => p._id == idperson).FirstOrDefault().Occupation.Skills;
        var list = skillsgroup;
        foreach (var item in skills)
        {
          list.Add(item);
        }
        return list.OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Monitoring> GetListExcludOld(ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(_user._idPerson, "ListExclud");
        int skip = (count * (page - 1));
        var detail = serviceMonitoring.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceMonitoring.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ListComments> AddCommentsOld(string idmonitoring, string iditem, ListComments comments)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (monitoring.Person._id == _user._idPerson)
          {
            monitoring.DateBeginPerson = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressPerson;
          }
          else
          {
            monitoring.DateBeginManager = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressManager;
          }
        }


        foreach (var item in monitoring.Activities)
        {
          if (item._id == iditem)
          {
            if (item.Comments == null)
              item.Comments = new List<ListComments>();

            if (comments.UserComment == EnumUserComment.Person)
            {
              item.StatusViewManager = EnumStatusView.None;
              item.StatusViewPerson = EnumStatusView.View;
            }
            else
            {
              item.StatusViewManager = EnumStatusView.View;
              item.StatusViewPerson = EnumStatusView.None;
            }
            comments._id = ObjectId.GenerateNewId().ToString(); comments._idAccount = _user._idAccount; item.Comments.Add(comments);

            serviceMonitoring.Update(monitoring, null);
            //return "ok";

            return item.Comments;
          }
        }

        foreach (var item in monitoring.Schoolings)
        {
          if (item._id == iditem)
          {
            if (item.Comments == null)
              item.Comments = new List<ListComments>();

            if (comments.UserComment == EnumUserComment.Person)
            {
              item.StatusViewManager = EnumStatusView.None;
              item.StatusViewPerson = EnumStatusView.View;
            }
            else
            {
              item.StatusViewManager = EnumStatusView.View;
              item.StatusViewPerson = EnumStatusView.None;
            }
            comments._id = ObjectId.GenerateNewId().ToString(); comments._idAccount = _user._idAccount; item.Comments.Add(comments);

            serviceMonitoring.Update(monitoring, null);
            //return "ok";

            return item.Comments;
          }
        }


        foreach (var item in monitoring.SkillsCompany)
        {
          if (item._id == iditem)
          {
            if (item.Comments == null)
              item.Comments = new List<ListComments>();

            if (comments.UserComment == EnumUserComment.Person)
            {
              item.StatusViewManager = EnumStatusView.None;
              item.StatusViewPerson = EnumStatusView.View;
            }
            else
            {
              item.StatusViewManager = EnumStatusView.View;
              item.StatusViewPerson = EnumStatusView.None;
            }
            comments._id = ObjectId.GenerateNewId().ToString(); comments._idAccount = _user._idAccount; item.Comments.Add(comments);

            serviceMonitoring.Update(monitoring, null);
            //return "ok";
            return item.Comments;
          }
        }



        //return "not found";
        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateCommentsOld(string idmonitoring, string iditem, ListComments comments)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        foreach (var item in monitoring.Activities)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == comments._id)
              {
                comment.StatusView = comments.StatusView;
                comment.Comments = comments.Comments;
                comment.Date = comment.Date;

                serviceMonitoring.Update(monitoring, null);
                return "ok";
              }
            }
          }
        }


        foreach (var item in monitoring.Schoolings)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == comments._id)
              {
                comment.StatusView = comments.StatusView;
                comment.Comments = comments.Comments;
                comment.Date = comment.Date;

                serviceMonitoring.Update(monitoring, null);
                return "ok";
              }
            }
          }
        }


        foreach (var item in monitoring.SkillsCompany)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == comments._id)
              {
                comment.StatusView = comments.StatusView;
                comment.Comments = comments.Comments;
                comment.Date = comment.Date;

                serviceMonitoring.Update(monitoring, null);
                return "ok";
              }
            }
          }
        }

        return "not found";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ListComments> GetListCommentsOld(string idmonitoring, string iditem)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        foreach (var item in monitoring.Activities)
        {
          if (item._id == iditem)
          {
            return item.Comments;
          }
        }


        foreach (var item in monitoring.Schoolings)
        {
          if (item._id == iditem)
          {
            return item.Comments;
          }
        }

        foreach (var item in monitoring.SkillsCompany)
        {
          if (item._id == iditem)
          {
            return item.Comments;
          }
        }

        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region Monitoring
    public string RemoveMonitoringActivities(string idmonitoring, string idactivitie)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        //var monitoringActivitie = monitoring.Activities.Where(p => p._id == idactivitie).FirstOrDefault();
        //monitoring.Activities.Remove(monitoringActivitie);
        foreach (var item in monitoring.Activities)
        {
          if (item._id == idactivitie)
          {
            item.Status = EnumStatus.Disabled;
            serviceMonitoring.Update(monitoring, null);
            return "remove";
          }

        }

        return "not found";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public string ScriptComments()
    {
      try
      {
        var list = serviceMonitoring.GetAuthentication(p => p.Status == EnumStatus.Enabled).ToList();

        foreach (var item in list)
        {
          foreach (var row in item.Activities)
          {
            row.Comments = new List<ListComments>();
            var commentPerson = new ListComments
            {
              _id = ObjectId.GenerateNewId().ToString(),
              _idAccount = item._idAccount,
              Comments = row.CommentsPerson,
              StatusView = EnumStatusView.View,
              Date = item.DateBeginPerson,
              UserComment = EnumUserComment.Person
            };

            var commentManager = new ListComments
            {
              _id = ObjectId.GenerateNewId().ToString(),
              _idAccount = item._idAccount,
              Comments = row.CommentsManager,
              StatusView = EnumStatusView.View,
              Date = item.DateBeginManager,
              UserComment = EnumUserComment.Manager
            };

            if (item.DateBeginPerson > item.DateBeginManager)
            {
              if (commentPerson.Comments != null)
                row.Comments.Add(commentPerson);

              if (commentManager.Comments != null)
                row.Comments.Add(commentManager);
            }
            else
            {
              if (commentManager.Comments != null)
                row.Comments.Add(commentManager);

              if (commentPerson.Comments != null)
                row.Comments.Add(commentPerson);
            }
            row.StatusViewManager = EnumStatusView.View;
            row.StatusViewPerson = EnumStatusView.View;

          }

          foreach (var row in item.Schoolings)
          {
            row.Comments = new List<ListComments>();
            var commentPerson = new ListComments
            {
              _id = ObjectId.GenerateNewId().ToString(),
              _idAccount = item._idAccount,
              Comments = row.CommentsPerson,
              StatusView = EnumStatusView.View,
              Date = item.DateBeginPerson,
              UserComment = EnumUserComment.Person
            };

            var commentManager = new ListComments
            {
              _id = ObjectId.GenerateNewId().ToString(),
              _idAccount = item._idAccount,
              Comments = row.CommentsManager,
              StatusView = EnumStatusView.View,
              Date = item.DateBeginManager,
              UserComment = EnumUserComment.Manager
            };

            if (item.DateBeginPerson > item.DateBeginManager)
            {
              if (commentPerson.Comments != null)
                row.Comments.Add(commentPerson);

              if (commentManager.Comments != null)
                row.Comments.Add(commentManager);
            }
            else
            {
              if (commentManager.Comments != null)
                row.Comments.Add(commentManager);

              if (commentPerson.Comments != null)
                row.Comments.Add(commentPerson);
            }
            row.StatusViewManager = EnumStatusView.View;
            row.StatusViewPerson = EnumStatusView.View;

          }

          foreach (var row in item.SkillsCompany)
          {
            row.Comments = new List<ListComments>();
            var commentPerson = new ListComments
            {
              _id = ObjectId.GenerateNewId().ToString(),
              _idAccount = item._idAccount,
              Comments = row.CommentsPerson,
              StatusView = EnumStatusView.View,
              Date = item.DateBeginPerson,
              UserComment = EnumUserComment.Person
            };

            var commentManager = new ListComments
            {
              _id = ObjectId.GenerateNewId().ToString(),
              _idAccount = item._idAccount,
              Comments = row.CommentsManager,
              StatusView = EnumStatusView.View,
              Date = item.DateBeginManager,
              UserComment = EnumUserComment.Manager
            };

            if (item.DateBeginPerson > item.DateBeginManager)
            {
              if (commentPerson.Comments != null)
                row.Comments.Add(commentPerson);

              if (commentManager.Comments != null)
                row.Comments.Add(commentManager);
            }
            else
            {
              if (commentManager.Comments != null)
                row.Comments.Add(commentManager);

              if (commentPerson.Comments != null)
                row.Comments.Add(commentPerson);
            }


            row.StatusViewManager = EnumStatusView.View;
            row.StatusViewPerson = EnumStatusView.View;

          }

          serviceMonitoring.UpdateAccount(item, null);
        }

        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string RemoveAllMonitoring(string idperson)
    {
      try
      {

        LogSave(_user._idPerson, "RemoveAllMonitoring:" + idperson);
        var monitorings = serviceMonitoring.GetAll(p => p.Person._id == idperson).ToList();
        foreach (var monitoring in monitorings)
        {
          monitoring.Status = EnumStatus.Disabled;
          serviceMonitoring.Update(monitoring, null);
        }

        return "deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public string RemoveMonitoring(string idmonitoring)
    {
      try
      {
        LogSave(_user._idPerson, "RemoveMonitoring:" + idmonitoring);
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        monitoring.Status = EnumStatus.Disabled;
        serviceMonitoring.Update(monitoring, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public string RemoveLastMonitoring(string idperson)
    {
      try
      {
        LogSave(_user._idPerson, "RemoveLastMonitoring:" + idperson);
        var monitoring = serviceMonitoring.GetAll(p => p.Person._id == idperson).LastOrDefault();
        monitoring.Status = EnumStatus.Disabled;
        serviceMonitoring.Update(monitoring, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    private async void LogSave(string iduser, string local)
    {
      try
      {
        var user = servicePerson.GetAll(p => p._id == iduser).FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "Access Monitoring ",
          Local = local,
          _idPerson = user._id
        };
        logService.NewLog(log);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateCommentsView(string idmonitoring, string iditem, EnumUserComment userComment)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        foreach (var item in monitoring.Activities)
        {
          if (item._id == iditem)
          {
            if (userComment == EnumUserComment.Manager)
              item.StatusViewManager = EnumStatusView.View;
            else
              item.StatusViewPerson = EnumStatusView.View;

            serviceMonitoring.Update(monitoring, null);
            return "ok";
          }
        }


        foreach (var item in monitoring.Schoolings)
        {
          if (item._id == iditem)
          {
            if (userComment == EnumUserComment.Manager)
              item.StatusViewManager = EnumStatusView.View;
            else
              item.StatusViewPerson = EnumStatusView.View;

            serviceMonitoring.Update(monitoring, null);
            return "ok";
          }
        }



        foreach (var item in monitoring.SkillsCompany)
        {
          if (item._id == iditem)
          {
            if (userComment == EnumUserComment.Manager)
              item.StatusViewManager = EnumStatusView.View;
            else
              item.StatusViewPerson = EnumStatusView.View;

            serviceMonitoring.Update(monitoring, null);
            return "ok";
          }
        }


        return "not found";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string DeleteComments(string idmonitoring, string iditem, string idcomments)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        foreach (var item in monitoring.Activities)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                serviceMonitoring.Update(monitoring, null);
                return "ok";
              }
            }
          }
        }

        foreach (var item in monitoring.Schoolings)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                serviceMonitoring.Update(monitoring, null);
                return "ok";
              }
            }
          }
        }


        foreach (var item in monitoring.SkillsCompany)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                serviceMonitoring.Update(monitoring, null);
                return "ok";
              }
            }
          }
        }

        return "not found";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public bool ValidComments(string id)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == id).FirstOrDefault();

        var count = monitoring.Activities.Where(p => p.StatusViewManager == EnumStatusView.None
        & p.Comments != null).Count()
          + monitoring.Schoolings.Where(p => p.StatusViewManager == EnumStatusView.None
          & p.Comments != null).Count()
          + monitoring.SkillsCompany.Where(p => p.StatusViewManager == EnumStatusView.None
          & p.Comments != null).Count();

        if (count > 0)
          return true;
        else
          return false;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    //public List<ViewCrudPlan> AddPlan(string idmonitoring, string iditem, ViewCrudPlan plan)
    //{
    //  try
    //  {
    //    var userInclude = servicePerson.GetAll(p => p._id == _user._idPerson).FirstOrDefault();
    //    var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

    //    if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
    //    {
    //      if (monitoring.Person._id == _user._idPerson)
    //      {
    //        monitoring.DateBeginPerson = DateTime.Now;
    //        monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressPerson;
    //      }
    //      else
    //      {
    //        monitoring.DateBeginManager = DateTime.Now;
    //        monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressManager;
    //      }
    //    }

    //    logMessagesService.NewLogMessage("Plano", " Ação de desenvolvimento acordada do colaborador " + monitoring.Person.User.Name, monitoring.Person);


    //    var newPlan = AddPlan(plan, userInclude);

    //    if (plan.SourcePlan == EnumSourcePlan.Activite)
    //    {
    //      foreach (var item in monitoring.Activities)
    //      {
    //        if (item._id == iditem)
    //        {
    //          if (item.Plans == null)
    //            item.Plans = new List<Plan>();

    //          item.Plans.Add(newPlan);
    //          serviceMonitoring.Update(monitoring, null);
    //          return item.Plans;
    //        }
    //      }
    //    }

    //    if (plan.SourcePlan == EnumSourcePlan.Schooling)
    //    {
    //      foreach (var item in monitoring.Schoolings)
    //      {
    //        if (item._id == iditem)
    //        {
    //          if (item.Plans == null)
    //            item.Plans = new List<Plan>();

    //          item.Plans.Add(newPlan);
    //          serviceMonitoring.Update(monitoring, null);
    //          return item.Plans;
    //        }
    //      }
    //    }

    //    if (plan.SourcePlan == EnumSourcePlan.Skill)
    //    {
    //      foreach (var item in monitoring.SkillsCompany)
    //      {
    //        if (item._id == iditem)
    //        {
    //          if (item.Plans == null)
    //            item.Plans = new List<Plan>();

    //          item.Plans.Add(newPlan);
    //          serviceMonitoring.Update(monitoring, null);
    //          return item.Plans;
    //        }
    //      }
    //    }


    //    return null;
    //  }
    //  catch (Exception e)
    //  {
    //    throw new ServiceException(_user, e, this._context);
    //  }
    //}
    //public List<ViewCrudPlan> UpdatePlan(string idmonitoring, string iditem, ViewCrudPlan plan)
    //{
    //  try
    //  {
    //    //var userInclude = servicePerson.GetAll(p => p._id == _user._idPerson).FirstOrDefault();
    //    var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
    //    logMessagesService.NewLogMessage("Plano", " Ação de desenvolvimento acordada do colaborador " + monitoring.Person.User.Name, monitoring.Person);




    //    if (plan.SourcePlan == EnumSourcePlan.Activite)
    //    {
    //      foreach (var item in monitoring.Activities)
    //      {
    //        if (item._id == iditem)
    //        {
    //          foreach (var row in item.Plans)
    //          {
    //            if (row._id == plan._id)
    //            {
    //              item.Plans.Remove(row);
    //              item.Plans.Add(plan);
    //              UpdatePlan(plan);
    //              serviceMonitoring.Update(monitoring, null);
    //              return item.Plans;
    //            }
    //          }


    //        }
    //      }
    //    }

    //    if (plan.SourcePlan == EnumSourcePlan.Schooling)
    //    {
    //      foreach (var item in monitoring.Schoolings)
    //      {
    //        if (item._id == iditem)
    //        {
    //          foreach (var row in item.Plans)
    //          {
    //            if (row._id == plan._id)
    //            {
    //              item.Plans.Remove(row);
    //              item.Plans.Add(plan);
    //              UpdatePlan(plan);
    //              serviceMonitoring.Update(monitoring, null);
    //              return item.Plans;
    //            }
    //          }
    //        }
    //      }
    //    }

    //    if (plan.SourcePlan == EnumSourcePlan.Skill)
    //    {
    //      foreach (var item in monitoring.SkillsCompany)
    //      {
    //        if (item._id == iditem)
    //        {
    //          foreach (var row in item.Plans)
    //          {
    //            if (row._id == plan._id)
    //            {
    //              item.Plans.Remove(row);
    //              item.Plans.Add(plan);
    //              UpdatePlan(plan);
    //              serviceMonitoring.Update(monitoring, null);
    //              return item.Plans;
    //            }
    //          }
    //        }
    //      }
    //    }

    //    serviceMonitoring.Update(monitoring, null);
    //    return null;
    //  }
    //  catch (Exception e)
    //  {
    //    throw new ServiceException(_user, e, this._context);
    //  }
    //}

    public List<ViewListMonitoring> ListMonitoringsEnd(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        //LogSave(idmanager, "ListEnd");
        int skip = (count * (page - 1));
        var detail = serviceMonitoring.GetAll(p => p.Person.Manager._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).ToList();
        total = serviceMonitoring.GetAll(p => p.Person.Manager._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.Select(p => new ViewListMonitoring()
        {
          _id = p._id,
          Name = p.Person.User.Name,
          idPerson = p.Person._id,
          StatusMonitoring = p.StatusMonitoring
        }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public List<ViewListMonitoring> ListMonitoringsWait(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        //LogSave(idmanager, "List");

        int skip = (count * (page - 1));
        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.TypeJourney == EnumTypeJourney.Monitoring & p.Manager._id == idmanager
        & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name)
        .ToList().Select(p => new { Person = p, Monitoring = serviceMonitoring.GetAll(x => x.StatusMonitoring != EnumStatusMonitoring.End & x.Person._id == p._id).FirstOrDefault() })
        .ToList();

        var detail = new List<Monitoring>();

        if (serviceMonitoring.Exists("Monitoring"))
        {
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

        }

        total = detail.Count();
        return detail.Skip(skip).Take(count).Select(p => new ViewListMonitoring()
        {
          _id = p._id,
          Name = p.Person.User.Name,
          idPerson = p.Person._id,
          StatusMonitoring = p.StatusMonitoring
        }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public List<ViewListMonitoring> PersonMonitoringsEnd(string idmanager)
    {
      try
      {
        //LogSave(idmanager, "PersonEnd");
        return serviceMonitoring.GetAll(p => p.Person._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End).OrderBy(p => p.Person.User.Name)
          .Select(p => new ViewListMonitoring()
          {
            _id = p._id,
            Name = p.Person.User.Name,
            idPerson = p.Person._id,
            StatusMonitoring = p.StatusMonitoring
          }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public ViewListMonitoring PersonMonitoringsWait(string idmanager)
    {
      try
      {
        //LogSave(idmanager, "PersonWait");
        if (serviceMonitoring.Exists("Monitoring") == null)
          return null;

        var item = servicePerson.GetAll(p => p.TypeJourney == EnumTypeJourney.Monitoring & p._id == idmanager)
        .ToList().Select(p => new { Person = p, Monitoring = serviceMonitoring.GetAll(x => x.StatusMonitoring != EnumStatusMonitoring.End & x.Person._id == p._id).FirstOrDefault() })
        .FirstOrDefault();



        if (item == null)
          return null;

        if (item.Monitoring == null)
          return new ViewListMonitoring()
          {
            _id = null,
            Name = item.Person.User.Name,
            idPerson = item.Person._id,
            StatusMonitoring = EnumStatusMonitoring.Open
          };
        else
         if (item.Monitoring.StatusMonitoring != EnumStatusMonitoring.End)
          return new ViewListMonitoring()
          {
            _id = item.Monitoring._id,
            Name = item.Person.User.Name,
            idPerson = item.Person._id,
            StatusMonitoring = item.Monitoring.StatusMonitoring
          };
        else
          return null;


      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public ViewCrudMonitoring GetMonitorings(string id)
    {
      try
      {
        return serviceMonitoring.GetAll(p => p._id == id).ToList().Select(p => new ViewCrudMonitoring()
        {
          _id = p._id,
          _idPerson = p.Person._id,
          DateBeginPerson = p.DateBeginPerson,
          DateBeginManager = p.DateBeginManager,
          DateBeginEnd = p.DateBeginEnd,
          DateEndPerson = p.DateEndPerson,
          DateEndManager = p.DateEndManager,
          DateEndEnd = p.DateEndEnd,
          CommentsEnd = p.CommentsEnd,
          //SkillsCompany = p.SkillsCompany.OrderBy(x => x.Skill.Name).ToList(),
          //Schoolings = p.Schoolings.OrderBy(x => x.Schooling.Order).ToList(),
          //Activities = p.Activities.OrderBy(x => x.Activities.Order).ToList(),
          StatusMonitoring = p.StatusMonitoring
        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public ViewCrudMonitoringActivities GetMonitoringActivities(string idmonitoring, string idactivitie)
    {
      try
      {
        var view = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault().
          Activities.Where(p => p._id == idactivitie).FirstOrDefault();

        return new ViewCrudMonitoringActivities()
        {
          _id = view._id,
          Activities = new ViewListActivitie() { _id = view.Activities._id, Name = view.Activities.Name, Order = view.Activities.Order },
          Comments = view.Comments.Select(p => new ViewCrudComment()
          {
            _id = p._id,
            Comments = p.Comments,
            Date = p.Date,
            StatusView = p.StatusView,
            UserComment = p.UserComment
          }).ToList(),
          TypeAtivitie = view.TypeAtivitie,
          Praise = view.Praise,
          Plans = view.Plans.Select(p => new ViewCrudPlan() { }).ToList(),
          StatusViewManager = view.StatusViewManager,
          StatusViewPerson = view.StatusViewPerson
        };
      }
      catch (Exception e)
      {
        return null;
        //throw new ServiceException(_user, e, this._context);
      }
    }
    public string UpdateMonitoringActivities(string idmonitoring, ViewCrudMonitoringActivities view)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

        foreach (var item in monitoring.Activities)
        {
          if (item._id == view._id)
          {
            monitoring.Activities.Remove(item);
            var monitoringActivitie = new MonitoringActivities()
            {
              _id = item._id,
              _idAccount = item._idAccount,
              Status = item.Status,
              TypeAtivitie = view.TypeAtivitie,
              StatusViewManager = view.StatusViewManager,
              StatusViewPerson = view.StatusViewPerson,
              Praise = view.Praise,
              Activities = item.Activities,
              Plans = item.Plans,
              Comments = new List<ListComments>()
            };
            foreach (var com in view.Comments)
              monitoringActivitie.Comments.Add(new ListComments()
              {
                _id = (com._id == null) ? ObjectId.GenerateNewId().ToString() : com._id,
                Date = com.Date,
                Comments = com.Comments,
                Status = EnumStatus.Enabled,
                StatusView = com.StatusView,
                UserComment = com.UserComment,
                _idAccount = _user._idAccount
              });

            monitoring.Activities.Add(monitoringActivitie);
            serviceMonitoring.Update(monitoring, null);
            return "update";
          }

        }
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public string AddMonitoringActivities(string idmonitoring, ViewCrudActivities view)
    {
      try
      {

        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();

        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (monitoring.Person._id == _user._idPerson)
          {
            monitoring.DateBeginPerson = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressPerson;
          }
          else
          {
            monitoring.DateBeginManager = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressManager;
          }
        }

        var monitoringActivitie = new MonitoringActivities();

        var activities = new List<Activitie>();

        foreach (var item in monitoring.Activities)
        {
          activities.Add(item.Activities);
        }

        long order = 1;
        try
        {
          order = activities.Max(p => p.Order) + 1;
          if (order == 0)
          {
            order = 1;
          }
        }
        catch (Exception)
        {
          order = 1;
        }

        var activitie = new Activitie()
        {
          Name = view.Name,
          Status = EnumStatus.Enabled,
          _id = ObjectId.GenerateNewId().ToString(),
          _idAccount = _user._idAccount,
          Order = order,
        };


        monitoringActivitie.Activities = activitie;
        monitoringActivitie.Status = EnumStatus.Enabled;
        monitoringActivitie._id = ObjectId.GenerateNewId().ToString();
        monitoringActivitie._idAccount = _user._idAccount;
        monitoringActivitie.TypeAtivitie = EnumTypeAtivitie.Individual;
        monitoringActivitie.Plans = new List<Plan>();
        monitoring.Activities.Add(monitoringActivitie);
        serviceMonitoring.Update(monitoring, null);
        return "add sucess";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public ViewListMonitoring NewMonitoring(ViewCrudMonitoring view, string idperson)
    {
      try
      {
        LogSave(view._idPerson, "Monitoring Process");
        var monitoring = serviceMonitoring.GetAll(p => p._id == view._id).FirstOrDefault();

        if (monitoring == null)
        {
          monitoring = new Monitoring()
          {
            Person = servicePerson.GetAll(p => p._id == view._idPerson).FirstOrDefault()
          };

          LoadMap(monitoring);

          monitoring.StatusMonitoring = EnumStatusMonitoring.Show;
          serviceMonitoring.Insert(monitoring);
        }
        else
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
            LoadMap(monitoring);

          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Wait)
          {
            monitoring.DateBeginEnd = DateTime.Now;
          }
          serviceMonitoring.Update(monitoring, null);
        }

        return new ViewListMonitoring()
        {
          _id = monitoring._id,
          Name = monitoring.Person.User.Name,
          idPerson = monitoring.Person._id,
          StatusMonitoring = monitoring.StatusMonitoring
        };
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateMonitoring(ViewCrudMonitoring view, string idperson)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == view._id).FirstOrDefault();
        monitoring.StatusMonitoring = view.StatusMonitoring;
        

        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (monitoring.Person._id == _user._idPerson)
          {
            monitoring.DateBeginPerson = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressPerson;
          }
          else
          {
            monitoring.DateBeginManager = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressManager;
          }
        }

        var userInclude = servicePerson.GetAll(p => p._id == idperson).FirstOrDefault();

        if (monitoring.Person._id != idperson)
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Wait)
          {
            monitoring.DateEndManager = DateTime.Now;
            Mail(monitoring.Person);
          }
        }
        else
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.End)
          {
            logMessagesService.NewLogMessage("Monitoring", " Monitoring realizado do colaborador " + monitoring.Person.User.Name, monitoring.Person);
            if ((monitoring.Activities.Where(p => p.Praise != null).Count() > 0)
              || (monitoring.SkillsCompany.Where(p => p.Praise != null).Count() > 0)
              || (monitoring.Schoolings.Where(p => p.Praise != null).Count() > 0))
            {
              logMessagesService.NewLogMessage("Monitoring", " Colaborador " + monitoring.Person.User.Name + " foi elogiado pelo gestor", monitoring.Person);
            }
            monitoring.DateEndEnd = DateTime.Now;
          }
          else if (monitoring.StatusMonitoring == EnumStatusMonitoring.WaitManager)
          {
            monitoring.DateEndPerson = DateTime.Now;
            MailManager(monitoring.Person);

          }
          else if (monitoring.StatusMonitoring == EnumStatusMonitoring.Disapproved)
          {
            MailDisApproval(monitoring.Person);

          }
        }


        ////verify plan;
        //foreach (var item in monitoring.Activities)
        //{
        //  if (item._id == null)
        //    item._id = ObjectId.GenerateNewId().ToString();

        //  var listActivities = new List<Plan>();
        //  foreach (var plan in item.Plans)
        //  {
        //    if (plan._id == null)
        //    {
        //      logMessagesService.NewLogMessage("Plano", " Ação de desenvolvimento acordada do colaborador " + monitoring.Person.User.Name, monitoring.Person);
        //      listActivities.Add(AddPlan(plan, userInclude));
        //    }

        //    else
        //    {
        //      UpdatePlan(plan);
        //      listActivities.Add(plan);
        //    }
        //  }
        //  item.Plans = listActivities;
        //}

        //foreach (var item in monitoring.Schoolings)
        //{
        //  var listSchoolings = new List<Plan>();
        //  foreach (var plan in item.Plans)
        //  {
        //    if (plan._id == null)
        //    {
        //      logMessagesService.NewLogMessage("Plano", " Ação de desenvolvimento acordada do colaborador " + monitoring.Person.User.Name, monitoring.Person);
        //      listSchoolings.Add(AddPlan(plan, userInclude));
        //    }

        //    else
        //    {
        //      UpdatePlan(plan);
        //      listSchoolings.Add(plan);
        //    }
        //  }
        //  item.Plans = listSchoolings;
        //}

        //foreach (var item in monitoring.SkillsCompany)
        //{
        //  var listSkillsCompany = new List<Plan>();
        //  foreach (var plan in item.Plans)
        //  {
        //    if (plan._id == null)
        //    {
        //      logMessagesService.NewLogMessage("Plano", " Ação de desenvolvimento acordada do colaborador " + monitoring.Person.User.Name, monitoring.Person);
        //      listSkillsCompany.Add(AddPlan(plan, userInclude));
        //    }

        //    else
        //    {
        //      UpdatePlan(plan);
        //      listSkillsCompany.Add(plan);
        //    }
        //  }
        //  item.Plans = listSkillsCompany;
        //}



        serviceMonitoring.Update(monitoring, null);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }


    public List<ViewListSkill> GetSkills(string idperson)
    {
      try
      {
        var skills = servicePerson.GetAll(p => p._id == idperson).FirstOrDefault().Occupation.Group.Skills;
        var skillsgroup = servicePerson.GetAll(p => p._id == idperson).FirstOrDefault().Occupation.Skills;
        var list = skillsgroup;
        foreach (var item in skills)
        {
          list.Add(item);
        }
        return list.OrderBy(p => p.Name)
          .Select(p => new ViewListSkill()
          {
            _id = p._id,
            Name = p.Name,
            Concept = p.Concept,
            TypeSkill = p.TypeSkill
          }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public List<ViewListMonitoring> GetListExclud(ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(_user._idPerson, "ListExclud");
        int skip = (count * (page - 1));
        var detail = serviceMonitoring.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceMonitoring.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.Select(p => new ViewListMonitoring()
        {
          _id = p._id,
          Name = p.Person.User.Name,
          idPerson = p.Person._id,
          StatusMonitoring = p.StatusMonitoring
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewCrudComment> AddComments(string idmonitoring, string iditem, ViewCrudComment comments)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (monitoring.Person._id == _user._idPerson)
          {
            monitoring.DateBeginPerson = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressPerson;
          }
          else
          {
            monitoring.DateBeginManager = DateTime.Now;
            monitoring.StatusMonitoring = EnumStatusMonitoring.InProgressManager;
          }
        }


        foreach (var item in monitoring.Activities)
        {
          if (item._id == iditem)
          {
            if (item.Comments == null)
              item.Comments = new List<ListComments>();

            if (comments.UserComment == EnumUserComment.Person)
            {
              item.StatusViewManager = EnumStatusView.None;
              item.StatusViewPerson = EnumStatusView.View;
            }
            else
            {
              item.StatusViewManager = EnumStatusView.View;
              item.StatusViewPerson = EnumStatusView.None;
            }
            item.Comments.Add(
              new ListComments()
              {
                _id = comments._id = ObjectId.GenerateNewId().ToString(),
                _idAccount = _user._idAccount,
                Comments = comments.Comments,
                Date = comments.Date,
                Status = EnumStatus.Enabled,
                StatusView = comments.StatusView,
                UserComment = comments.UserComment
              });

            serviceMonitoring.Update(monitoring, null);
            //return "ok";

            return item.Comments.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment
            }).ToList();
          }
        }

        foreach (var item in monitoring.Schoolings)
        {
          if (item._id == iditem)
          {
            if (item.Comments == null)
              item.Comments = new List<ListComments>();

            if (comments.UserComment == EnumUserComment.Person)
            {
              item.StatusViewManager = EnumStatusView.None;
              item.StatusViewPerson = EnumStatusView.View;
            }
            else
            {
              item.StatusViewManager = EnumStatusView.View;
              item.StatusViewPerson = EnumStatusView.None;
            }
            item.Comments.Add(
             new ListComments()
             {
               _id = comments._id = ObjectId.GenerateNewId().ToString(),
               _idAccount = _user._idAccount,
               Comments = comments.Comments,
               Date = comments.Date,
               Status = EnumStatus.Enabled,
               StatusView = comments.StatusView,
               UserComment = comments.UserComment
             });

            serviceMonitoring.Update(monitoring, null);
            //return "ok";

            return item.Comments.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment
            }).ToList();
          }
        }


        foreach (var item in monitoring.SkillsCompany)
        {
          if (item._id == iditem)
          {
            if (item.Comments == null)
              item.Comments = new List<ListComments>();

            if (comments.UserComment == EnumUserComment.Person)
            {
              item.StatusViewManager = EnumStatusView.None;
              item.StatusViewPerson = EnumStatusView.View;
            }
            else
            {
              item.StatusViewManager = EnumStatusView.View;
              item.StatusViewPerson = EnumStatusView.None;
            }
            item.Comments.Add(
             new ListComments()
             {
               _id = comments._id = ObjectId.GenerateNewId().ToString(),
               _idAccount = _user._idAccount,
               Comments = comments.Comments,
               Date = comments.Date,
               Status = EnumStatus.Enabled,
               StatusView = comments.StatusView,
               UserComment = comments.UserComment
             });

            serviceMonitoring.Update(monitoring, null);
            //return "ok";
            return item.Comments.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment
            }).ToList();
          }
        }



        //return "not found";
        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateComments(string idmonitoring, string iditem, ViewCrudComment comments)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        foreach (var item in monitoring.Activities)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == comments._id)
              {
                comment.StatusView = comments.StatusView;
                comment.Comments = comments.Comments;
                comment.Date = comment.Date;

                serviceMonitoring.Update(monitoring, null);
                return "ok";
              }
            }
          }
        }


        foreach (var item in monitoring.Schoolings)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == comments._id)
              {
                comment.StatusView = comments.StatusView;
                comment.Comments = comments.Comments;
                comment.Date = comment.Date;

                serviceMonitoring.Update(monitoring, null);
                return "ok";
              }
            }
          }
        }


        foreach (var item in monitoring.SkillsCompany)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == comments._id)
              {
                comment.StatusView = comments.StatusView;
                comment.Comments = comments.Comments;
                comment.Date = comment.Date;

                serviceMonitoring.Update(monitoring, null);
                return "ok";
              }
            }
          }
        }

        return "not found";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewCrudComment> GetListComments(string idmonitoring, string iditem)
    {
      try
      {


        var monitoring = serviceMonitoring.GetAll(p => p._id == idmonitoring).FirstOrDefault();
        foreach (var item in monitoring.Activities)
        {
          if (item._id == iditem)
          {
            return item.Comments.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment
            }).ToList();
          }
        }


        foreach (var item in monitoring.Schoolings)
        {
          if (item._id == iditem)
          {
            return item.Comments.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment
            }).ToList();
          }
        }

        foreach (var item in monitoring.SkillsCompany)
        {
          if (item._id == iditem)
          {
            return item.Comments.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment
            }).ToList();
          }
        }

        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region private
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
    private Monitoring LoadMap(Monitoring monitoring)
    {
      try
      {
        var occupation = occupationService.GetAll(p => p._id == monitoring.Person.Occupation._id).FirstOrDefault();

        monitoring.SkillsCompany = new List<MonitoringSkills>();
        foreach (var item in occupation.Group.Company.Skills)
        {
          monitoring.SkillsCompany.Add(new MonitoringSkills() { Skill = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<Plan>() });
        }

        monitoring.Activities = new List<MonitoringActivities>();
        foreach (var item in occupation.Activities)
        {
          monitoring.Activities.Add(new MonitoringActivities() { Activities = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<Plan>() });
        }

        monitoring.Schoolings = new List<MonitoringSchooling>();
        foreach (var item in occupation.Schooling)
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
    // send mail
    private async void Mail(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.MonitoringApproval(path);
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
        var idMessage = mailMessageService.Insert(message)._id;
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
    private async void MailManager(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.MonitoringApprovalManager(path);
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
        var idMessage = mailMessageService.Insert(message)._id;
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person.Manager._id,
          NamePerson = person.Manager.Name,
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
    private async void MailDisApproval(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.MonitoringDisApproval(path);
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
        var idMessage = mailMessageService.Insert(message)._id;
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
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
    private string SendMail(string link, Person person, string idmail)
    {
      try
      {
        ViewPerson view = serviceAuthentication.AuthenticationMail(person);
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link);
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
        throw new ServiceException(_user, e, this._context);
      }
    }

    #endregion




#pragma warning restore 1998
  }
}