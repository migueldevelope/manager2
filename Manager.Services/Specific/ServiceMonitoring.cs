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
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.ServiceBus;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
#pragma warning disable 1998
  public class ServiceMonitoring : Repository<Monitoring>, IServiceMonitoring
  {
    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceLog serviceLog;
    private readonly ServiceLogMessages serviceLogMessages;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<Monitoring> serviceMonitoring;
    private readonly ServiceGeneric<Occupation> serviceOccupation;
    private readonly ServiceGeneric<OccupationLog> serviceOccupationLog;
    private readonly ServiceGeneric<Group> serviceGroup;
    private readonly ServiceGeneric<Company> serviceCompany;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Reports> serviceReport;
    private readonly ServiceGeneric<Plan> servicePlan;
    private readonly IServiceControlQueue serviceControlQueue;
    private readonly IQueueClient queueClient;
    private readonly IQueueClient queueClientReturn;

    private readonly string path;

    #region Constructor
    public ServiceMonitoring(DataContext context, DataContext contextLog, string pathToken, IServiceControlQueue _serviceControlQueue) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context, contextLog, _serviceControlQueue, pathToken);
        serviceLog = new ServiceLog(context, contextLog);
        serviceLogMessages = new ServiceLogMessages(context);
        serviceMail = new ServiceGeneric<MailLog>(context);
        serviceMailModel = new ServiceMailModel(context);
        serviceMonitoring = new ServiceGeneric<Monitoring>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        serviceOccupationLog = new ServiceGeneric<OccupationLog>(context);
        serviceGroup = new ServiceGeneric<Group>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        servicePlan = new ServiceGeneric<Plan>(context);
        serviceReport = new ServiceGeneric<Reports>(context);
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceControlQueue = _serviceControlQueue;
        path = pathToken;
        queueClient = new QueueClient(serviceControlQueue.ServiceBusConnectionString(), "audios");
        queueClientReturn = new QueueClient(serviceControlQueue.ServiceBusConnectionString(), "audiosreturn");
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceLog.SetUser(_user);
      serviceLogMessages.SetUser(_user);
      serviceMail._user = _user;
      serviceMailModel.SetUser(_user);
      serviceMonitoring._user = _user;
      serviceOccupation._user = _user;
      serviceGroup._user = _user;
      serviceCompany._user = _user;
      servicePerson._user = _user;
      servicePlan._user = _user;
      serviceReport._user = _user;
      serviceOccupationLog._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceLog.SetUser(user);
      serviceLogMessages.SetUser(user);
      serviceMail._user = user;
      serviceMailModel.SetUser(user);
      serviceMonitoring._user = user;
      serviceOccupation._user = user;
      serviceCompany._user = user;
      serviceGroup._user = user;
      servicePerson._user = user;
      servicePlan._user = user;
      serviceReport._user = user;
      serviceOccupationLog._user = user;
    }
    #endregion

    #region Monitoring
    public string RemoveMonitoringActivities(string idmonitoring, string idactivitie)
    {
      try
      {
        var monitoring = serviceMonitoring.GetNewVersion(p => p._id == idmonitoring).Result;
        foreach (var item in monitoring.Activities)
        {
          if (item._id == idactivitie)
          {
            item.Status = EnumStatus.Disabled;
            serviceMonitoring.Update(monitoring, null).Wait();
            return "Monitoring activitie deleted!";
          }
        }
        return "Monitoring activitie not found!";
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


        var monitorings = serviceMonitoring.GetAllNewVersion(p => p.Person._id == idperson).Result.ToList();
        foreach (var monitoring in monitorings)
        {
          monitoring.Status = EnumStatus.Disabled;
          serviceMonitoring.Update(monitoring, null).Wait();
        }
        return "Monitorings deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string RemoveMonitoring(string idmonitoring)
    {
      try
      {

        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();
        monitoring.Status = EnumStatus.Disabled;
        serviceMonitoring.Update(monitoring, null).Wait();
        return "Monitoring deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string RemoveLastMonitoring(string idperson)
    {
      try
      {

        var monitoring = serviceMonitoring.GetAllNewVersion(p => p.Person._id == idperson).Result.LastOrDefault();
        monitoring.Status = EnumStatus.Disabled;
        serviceMonitoring.Update(monitoring, null).Wait();
        return "Monitoring deleted!";
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
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();
        foreach (var item in monitoring.Activities)
        {
          if (item._id == iditem)
          {
            if (userComment == EnumUserComment.Manager)
              item.StatusViewManager = EnumStatusView.View;
            else
              item.StatusViewPerson = EnumStatusView.View;

            serviceMonitoring.Update(monitoring, null).Wait();
            return "Update comment altered!";
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

            serviceMonitoring.Update(monitoring, null).Wait();
            return "Update comment altered!";
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

            serviceMonitoring.Update(monitoring, null).Wait();
            return "Update comment altered!";
          }
        }

        foreach (var item in monitoring.SkillsGroup)
        {
          if (item._id == iditem)
          {
            if (userComment == EnumUserComment.Manager)
              item.StatusViewManager = EnumStatusView.View;
            else
              item.StatusViewPerson = EnumStatusView.View;

            serviceMonitoring.Update(monitoring, null).Wait();
            return "Update comment altered!";
          }
        }

        foreach (var item in monitoring.SkillsOccupation)
        {
          if (item._id == iditem)
          {
            if (userComment == EnumUserComment.Manager)
              item.StatusViewManager = EnumStatusView.View;
            else
              item.StatusViewPerson = EnumStatusView.View;

            serviceMonitoring.Update(monitoring, null).Wait();
            return "Update comment altered!";
          }
        }

        return "Comment not found!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string DeleteComments(string idmonitoring, string iditem, string idcomments, string plataform)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();
        Task.Run(() => LogSave(_user._idPerson, string.Format("Delete comment | {0}", idmonitoring), plataform));

        foreach (var item in monitoring.Activities)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                serviceMonitoring.Update(monitoring, null).Wait();
                return "Delete comment!";
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
                serviceMonitoring.Update(monitoring, null).Wait();
                return "Delete comment!";
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
                serviceMonitoring.Update(monitoring, null).Wait();
                return "Delete comment!";
              }
            }
          }
        }

        foreach (var item in monitoring.SkillsGroup)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                serviceMonitoring.Update(monitoring, null).Wait();
                return "Delete comment!";
              }
            }
          }
        }

        foreach (var item in monitoring.SkillsOccupation)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                serviceMonitoring.Update(monitoring, null).Wait();
                return "Delete comment!";
              }
            }
          }
        }
        return "Delete comment not found!";
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
        var monitoring = serviceMonitoring.GetNewVersion(p => p._id == id).Result;
        var count = monitoring.Activities.Where(p => p.StatusViewManager == EnumStatusView.None
        & p.Comments != null && p.Comments.Count() > 0).Count()
          + monitoring.Schoolings.Where(p => p.StatusViewManager == EnumStatusView.None
          & p.Comments != null && p.Comments.Count() > 0).Count()
          + monitoring.SkillsCompany.Where(p => p.StatusViewManager == EnumStatusView.None
          & p.Comments != null && p.Comments.Count() > 0).Count()
          + monitoring.SkillsGroup.Where(p => p.StatusViewManager == EnumStatusView.None
          & p.Comments != null && p.Comments.Count() > 0).Count()
          + monitoring.SkillsOccupation.Where(p => p.StatusViewManager == EnumStatusView.None
          & p.Comments != null && p.Comments.Count() > 0).Count();

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

    public List<ViewCrudPlan> AddPlan(string idmonitoring, string iditem, ViewCrudPlan plan, string plataform)
    {
      try
      {
        var userInclude = servicePerson.GetNewVersion(p => p.User._id == _user._idUser).Result.GetViewListBase();
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();
        var person = servicePerson.GetNewVersion(p => p._id == monitoring.Person._id).Result;
        Task.Run(() => LogSave(_user._idPerson, string.Format("Add plan | {0}", idmonitoring), plataform));

        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (person.User._id == _user._idUser)
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
        Task.Run(() => serviceLogMessages.NewLogMessage("Plano", " Ação de desenvolvimento acordada para o colaborador " + person.User.Name, person));
        var newPlan = AddPlan(new Plan()
        {
          _id = plan._id,
          Name = plan.Name,
          Description = plan.Description,
          Deadline = plan.Deadline,
          Skills = plan.Skills,
          SourcePlan = plan.SourcePlan,
          TypePlan = plan.TypePlan
        }, person, idmonitoring, iditem);

        if (plan.SourcePlan == EnumSourcePlan.Activite)
        {
          foreach (var item in monitoring.Activities)
          {
            if (item._id == iditem)
            {
              if (item.Plans == null)
                item.Plans = new List<ViewCrudPlan>();

              item.Plans.Add(new ViewCrudPlan()
              {
                _id = newPlan._id,
                Name = newPlan.Name,
                Description = newPlan.Description,
                Deadline = newPlan.Deadline,
                Skills = newPlan.Skills,
                SourcePlan = newPlan.SourcePlan,
                TypePlan = newPlan.TypePlan
              });
              serviceMonitoring.Update(monitoring, null).Wait();
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
                item.Plans = new List<ViewCrudPlan>();

              item.Plans.Add(new ViewCrudPlan()
              {
                _id = newPlan._id,
                Name = newPlan.Name,
                Description = newPlan.Description,
                Deadline = newPlan.Deadline,
                Skills = newPlan.Skills,
                SourcePlan = newPlan.SourcePlan,
                TypePlan = newPlan.TypePlan
              });
              serviceMonitoring.Update(monitoring, null).Wait();
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
                item.Plans = new List<ViewCrudPlan>();

              item.Plans.Add(new ViewCrudPlan()
              {
                _id = newPlan._id,
                Name = newPlan.Name,
                Description = newPlan.Description,
                Deadline = newPlan.Deadline,
                Skills = newPlan.Skills,
                SourcePlan = newPlan.SourcePlan,
                TypePlan = newPlan.TypePlan
              });
              serviceMonitoring.Update(monitoring, null).Wait();
              return item.Plans.Select(p => new ViewCrudPlan()
              {
                _id = p._id,
                Name = p.Name,
                Description = p.Description,
                Deadline = p.Deadline,
                Skills = p.Skills,
                SourcePlan = p.SourcePlan,
                TypePlan = p.TypePlan
              }).ToList();
            }
          }
        }
        if (plan.SourcePlan == EnumSourcePlan.SkillGroup)
        {
          foreach (var item in monitoring.SkillsGroup)
          {
            if (item._id == iditem)
            {
              if (item.Plans == null)
                item.Plans = new List<ViewCrudPlan>();

              item.Plans.Add(new ViewCrudPlan()
              {
                _id = newPlan._id,
                Name = newPlan.Name,
                Description = newPlan.Description,
                Deadline = newPlan.Deadline,
                Skills = newPlan.Skills,
                SourcePlan = newPlan.SourcePlan,
                TypePlan = newPlan.TypePlan
              });
              serviceMonitoring.Update(monitoring, null).Wait();
              return item.Plans.Select(p => new ViewCrudPlan()
              {
                _id = p._id,
                Name = p.Name,
                Description = p.Description,
                Deadline = p.Deadline,
                Skills = p.Skills,
                SourcePlan = p.SourcePlan,
                TypePlan = p.TypePlan
              }).ToList();
            }
          }
        }
        if (plan.SourcePlan == EnumSourcePlan.SkillOccupation)
        {
          foreach (var item in monitoring.SkillsOccupation)
          {
            if (item._id == iditem)
            {
              if (item.Plans == null)
                item.Plans = new List<ViewCrudPlan>();

              item.Plans.Add(new ViewCrudPlan()
              {
                _id = newPlan._id,
                Name = newPlan.Name,
                Description = newPlan.Description,
                Deadline = newPlan.Deadline,
                Skills = newPlan.Skills,
                SourcePlan = newPlan.SourcePlan,
                TypePlan = newPlan.TypePlan
              });
              serviceMonitoring.Update(monitoring, null).Wait();
              return item.Plans.Select(p => new ViewCrudPlan()
              {
                _id = p._id,
                Name = p.Name,
                Description = p.Description,
                Deadline = p.Deadline,
                Skills = p.Skills,
                SourcePlan = p.SourcePlan,
                TypePlan = p.TypePlan
              }).ToList();
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

    public List<ViewCrudPlan> ListPlansMobile(string idmonitoring, string iditem)
    {
      try
      {
        var plans = servicePlan.GetAllNewVersion(p => p._idMonitoring == idmonitoring && p._idItem == iditem).Result;
        var list = new List<ViewCrudPlan>();
        foreach (var item in plans)
        {
          var plan = new ViewCrudPlan()
          {
            _id = item._id,
            Attachments = item.Attachments,
            Deadline = item.Deadline,
            Description = item.Description,
            Evaluation = byte.Parse(item.Evaluation.ToString()),
            Name = item.Name,
            NewAction = item.NewAction,
            Skills = item.Skills,
            SourcePlan = item.SourcePlan,
            StatusPlan = item.StatusPlan,
            StatusPlanApproved = item.StatusPlanApproved,
            TextEnd = item.TextEnd,
            TextEndManager = item.TextEndManager,
            TypeAction = item.TypeAction,
            TypePlan = item.TypePlan
          };



          list.Add(plan);
        }
        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewCrudPlan> UpdatePlan(string idmonitoring, string iditem, ViewCrudPlan view)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();
        var person = servicePerson.GetNewVersion(p => p._id == monitoring.Person._id).Result;

        Task.Run(() => serviceLogMessages.NewLogMessage("Plano", " Ação de desenvolvimento acordada do colaborador " + monitoring.Person.Name, person));
        var plan = servicePlan.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
        plan.Description = view.Description;
        plan.Deadline = view.Deadline;
        plan.Name = view.Name;
        plan.SourcePlan = view.SourcePlan;
        plan.TypePlan = view.TypePlan;
        plan.Skills = view.Skills;
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
                  item.Plans.Add(new ViewCrudPlan()
                  {
                    _id = plan._id,
                    Name = plan.Name,
                    Description = plan.Description,
                    Deadline = plan.Deadline,
                    Skills = plan.Skills,
                    SourcePlan = plan.SourcePlan,
                    TypePlan = plan.TypePlan
                  });
                  UpdatePlan(plan);
                  serviceMonitoring.Update(monitoring, null).Wait();
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
                  item.Plans.Add(new ViewCrudPlan()
                  {
                    _id = plan._id,
                    Name = plan.Name,
                    Description = plan.Description,
                    Deadline = plan.Deadline,
                    Skills = plan.Skills,
                    SourcePlan = plan.SourcePlan,
                    TypePlan = plan.TypePlan
                  });
                  UpdatePlan(plan);
                  serviceMonitoring.Update(monitoring, null).Wait();
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
                  item.Plans.Add(new ViewCrudPlan()
                  {
                    _id = plan._id,
                    Name = plan.Name,
                    Description = plan.Description,
                    Deadline = plan.Deadline,
                    Skills = plan.Skills,
                    SourcePlan = plan.SourcePlan,
                    TypePlan = plan.TypePlan
                  });
                  UpdatePlan(plan);
                  serviceMonitoring.Update(monitoring, null).Wait();
                  return item.Plans;
                }
              }
            }
          }
        }

        if (plan.SourcePlan == EnumSourcePlan.SkillGroup)
        {
          foreach (var item in monitoring.SkillsGroup)
          {
            if (item._id == iditem)
            {
              foreach (var row in item.Plans)
              {
                if (row._id == plan._id)
                {
                  item.Plans.Remove(row);
                  item.Plans.Add(new ViewCrudPlan()
                  {
                    _id = plan._id,
                    Name = plan.Name,
                    Description = plan.Description,
                    Deadline = plan.Deadline,
                    Skills = plan.Skills,
                    SourcePlan = plan.SourcePlan,
                    TypePlan = plan.TypePlan
                  });
                  UpdatePlan(plan);
                  serviceMonitoring.Update(monitoring, null).Wait();
                  return item.Plans;
                }
              }
            }
          }
        }

        if (plan.SourcePlan == EnumSourcePlan.SkillOccupation)
        {
          foreach (var item in monitoring.SkillsOccupation)
          {
            if (item._id == iditem)
            {
              foreach (var row in item.Plans)
              {
                if (row._id == plan._id)
                {
                  item.Plans.Remove(row);
                  item.Plans.Add(new ViewCrudPlan()
                  {
                    _id = plan._id,
                    Name = plan.Name,
                    Description = plan.Description,
                    Deadline = plan.Deadline,
                    Skills = plan.Skills,
                    SourcePlan = plan.SourcePlan,
                    TypePlan = plan.TypePlan
                  });
                  UpdatePlan(plan);
                  serviceMonitoring.Update(monitoring, null).Wait();
                  return item.Plans;
                }
              }
            }
          }
        }

        serviceMonitoring.Update(monitoring, null).Wait();
        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeletePlan(string idmonitoring, string iditem, string idplan, string plataform)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();
        var person = servicePerson.GetNewVersion(p => p._id == monitoring.Person._id).Result;
        Task.Run(() => LogSave(_user._idPerson, string.Format("Delete plan | {0}", idmonitoring), plataform));

        Task.Run(() => serviceLogMessages.NewLogMessage("Plano", " Ação removida" + monitoring.Person.Name, person));
        var plan = servicePlan.GetNewVersion(p => p._id == idplan).Result;
        plan.Status = EnumStatus.Disabled;

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
                  UpdatePlan(plan);
                  serviceMonitoring.Update(monitoring, null).Wait();
                  return "remove";
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
                  UpdatePlan(plan);
                  serviceMonitoring.Update(monitoring, null).Wait();
                  return "remove";
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
                  UpdatePlan(plan);
                  serviceMonitoring.Update(monitoring, null).Wait();
                  return "remove";
                }
              }
            }
          }
        }

        if (plan.SourcePlan == EnumSourcePlan.SkillGroup)
        {
          foreach (var item in monitoring.SkillsGroup)
          {
            if (item._id == iditem)
            {
              foreach (var row in item.Plans)
              {
                if (row._id == plan._id)
                {
                  item.Plans.Remove(row);
                  UpdatePlan(plan);
                  serviceMonitoring.Update(monitoring, null).Wait();
                  return "remove";
                }
              }
            }
          }
        }

        if (plan.SourcePlan == EnumSourcePlan.SkillOccupation)
        {
          foreach (var item in monitoring.SkillsOccupation)
          {
            if (item._id == iditem)
            {
              foreach (var row in item.Plans)
              {
                if (row._id == plan._id)
                {
                  item.Plans.Remove(row);
                  UpdatePlan(plan);
                  serviceMonitoring.Update(monitoring, null).Wait();
                  return "remove";
                }
              }
            }
          }
        }
        serviceMonitoring.Update(monitoring, null).Wait();
        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListMonitoring> ListMonitoringsEnd(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceMonitoring.GetAllNewVersion(p => p.Person._idManager == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result.Skip(skip).Take(count).ToList();
        total = serviceMonitoring.CountNewVersion(p => p.Person._idManager == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewListMonitoring()
        {
          _id = p._id,
          Name = p.Person.Name,
          idPerson = p.Person._id,
          StatusMonitoring = p.StatusMonitoring
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListMonitoring> ListMonitoringsWait(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        List<Person> list = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.Manager._id == idmanager && p.Occupation != null && p.TypeJourney == EnumTypeJourney.Monitoring
            && p.User.Name.ToUpper().Contains(filter.ToUpper()), count, skip, "User.Name").Result.ToList();

        List<ViewListMonitoring> detail = new List<ViewListMonitoring>();
        if (serviceMonitoring.Exists("Monitoring"))
        {
          foreach (Person item in list)
          {
            Monitoring monitoring = serviceMonitoring.GetNewVersion(x => x.Person._id == item._id && x.StatusMonitoring != EnumStatusMonitoring.End).Result;
            if (monitoring == null)
            {
              var view = new ViewListMonitoring()
              {
                idPerson = item._id,
                Name = item.User?.Name,
                OccupationName = item.Occupation?.Name,
                StatusMonitoring = EnumStatusMonitoring.Open,
                Photo = item.User?.PhotoUrl
              };
              detail.Add(view);
            }

            else
             if (monitoring.StatusMonitoring != EnumStatusMonitoring.End)
            {
              var view = new ViewListMonitoring()
              {
                idPerson = monitoring.Person._id,
                Name = monitoring.Person.Name,
                OccupationName = monitoring.Person.Occupation,
                StatusMonitoring = monitoring.StatusMonitoring,
                Photo = item.User?.PhotoUrl,
                _id = monitoring._id,
                DateEndEnd = monitoring.DateEndEnd
              };
              detail.Add(view);
            }
          }
        }
        total = servicePerson.CountNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.Manager._id == idmanager && p.Occupation != null && p.TypeJourney == EnumTypeJourney.Monitoring
            && p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListMonitoring> ListMonitoringsWait_V2(List<ViewListIdIndicators> persons, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        List<ViewListMonitoring> detail = new List<ViewListMonitoring>();

        persons = persons.Where(p => p.TypeJourney == EnumTypeJourney.Monitoring).ToList();


        foreach (var item in persons)
        {
          Monitoring monitoring = serviceMonitoring.GetNewVersion(x => x.Person._id == item._id && x.StatusMonitoring != EnumStatusMonitoring.End).Result;
          var view = new ViewListMonitoring
          {
            _id = null,
            StatusMonitoring = EnumStatusMonitoring.Open,
            idPerson = item._id,
            Name = item.Name,
            OccupationName = item.OccupationName
          };

          if (monitoring != null)
          {
            view._id = monitoring._id;
            view.StatusMonitoring = monitoring.StatusMonitoring;
          }

          detail.Add(view);
        }

        total = detail.Count();
        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListMonitoring> PersonMonitoringsEnd(string idmanager)
    {
      try
      {
        return serviceMonitoring.GetAllNewVersion(p => p.Person._id == idmanager && p.StatusMonitoring == EnumStatusMonitoring.End).Result.OrderBy(p => p.Person.Name)
          .Select(p => new ViewListMonitoring()
          {
            _id = p._id,
            Name = p.Person.Name,
            idPerson = p.Person._id,
            StatusMonitoring = p.StatusMonitoring,
            OccupationName = p.Person.Occupation,
            DateEndEnd = p.DateEndEnd
          }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewListMonitoring PersonMonitoringsWait(string idmanager)
    {
      try
      {
        if (!serviceMonitoring.Exists("Monitoring"))
          return null;

        var item = servicePerson.GetNewVersion(p => p._id == idmanager && p.Occupation != null && p.TypeJourney == EnumTypeJourney.Monitoring).Result;
        var monitoring = serviceMonitoring.GetNewVersion(x => x.Person._id == idmanager && x.StatusMonitoring != EnumStatusMonitoring.End).Result;

        if (item == null)
          return null;

        if (monitoring == null)
          return new ViewListMonitoring()
          {
            _id = null,
            Name = item.User.Name,
            idPerson = item._id,
            StatusMonitoring = EnumStatusMonitoring.Open
          };
        else
         if (monitoring.StatusMonitoring != EnumStatusMonitoring.End)
          return new ViewListMonitoring()
          {
            _id = monitoring._id,
            Name = item.User.Name,
            idPerson = item._id,
            StatusMonitoring = monitoring.StatusMonitoring
          };
        else
          return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudMonitoring GetMonitorings(string id)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
        var person = servicePerson.GetNewVersion(p => p._id == monitoring.Person._id).Result;


        var view = new ViewCrudMonitoring()
        {
          _id = monitoring._id,
          OccupationDiff = false,
          MapDiff = false,
          Person = monitoring.Person,
          CommentsPerson = monitoring.CommentsPerson,
          CommentsEnd = monitoring.CommentsEnd,
          CommentsManager = monitoring.CommentsManager,
          CommentWarning = monitoring.CommentWarning,
          SkillsCompany = monitoring.SkillsCompany?.Select(p => new ViewCrudMonitoringSkills()
          {
            _id = p._id,
            Comments = p.Comments?.Select(x => new ViewCrudComment()
            {
              _id = x._id,
              Comments = x.Comments,
              Date = x.Date,
              StatusView = x.StatusView,
              UserComment = x.UserComment,
              SpeechLink = x.SpeechLink,
              TotalTime = x.TotalTime
            }).ToList(),
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Skill = p.Skill,
            Praise = p.Praise,

            Plans = p.Plans
          }).ToList(),
          SkillsGroup = monitoring.SkillsGroup?.Select(p => new ViewCrudMonitoringSkills()
          {
            _id = p._id,
            Comments = p.Comments?.Select(x => new ViewCrudComment()
            {
              _id = x._id,
              Comments = x.Comments,
              Date = x.Date,
              StatusView = x.StatusView,
              UserComment = x.UserComment,
              SpeechLink = x.SpeechLink,
              TotalTime = x.TotalTime
            }).ToList(),
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Skill = p.Skill,
            Praise = p.Praise,
            Plans = p.Plans
          }).ToList(),
          SkillsOccupation = monitoring.SkillsOccupation?.Select(p => new ViewCrudMonitoringSkills()
          {
            _id = p._id,
            Comments = p.Comments?.Select(x => new ViewCrudComment()
            {
              _id = x._id,
              Comments = x.Comments,
              Date = x.Date,
              StatusView = x.StatusView,
              UserComment = x.UserComment,
              SpeechLink = x.SpeechLink,
              TotalTime = x.TotalTime
            }).ToList(),
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Skill = p.Skill,
            Praise = p.Praise,
            Plans = p.Plans
          }).ToList(),
          Schoolings = monitoring.Schoolings?.Select(p => new ViewCrudMonitoringSchooling()
          {
            _id = p._id,
            Comments = p.Comments?.Select(x => new ViewCrudComment()
            {
              _id = x._id,
              Comments = x.Comments,
              Date = x.Date,
              StatusView = x.StatusView,
              UserComment = x.UserComment,
              SpeechLink = x.SpeechLink,
              TotalTime = x.TotalTime
            }).ToList(),
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Praise = p.Praise,
            Schooling = p.Schooling,
            Plans = p.Plans
          }).ToList(),
          Activities = monitoring.Activities?.Select(p => new ViewCrudMonitoringActivities()
          {
            _id = p._id,
            Comments = p.Comments?.Select(x => new ViewCrudComment()
            {
              _id = x._id,
              Comments = x.Comments,
              Date = x.Date,
              StatusView = x.StatusView,
              UserComment = x.UserComment,
              SpeechLink = x.SpeechLink,
              TotalTime = x.TotalTime
            }).ToList(),
            Status = p.Status,
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            TypeAtivitie = p.TypeAtivitie,
            Praise = p.Praise,
            Activities = p.Activities,
            Plans = p.Plans,
          }).ToList(),
          StatusMonitoring = monitoring.StatusMonitoring
        };
        var occupationLog = serviceOccupationLog.GetAllNewVersion(p => p._idOccupationPrevious == person.Occupation._id).Result.LastOrDefault();

        if (monitoring.Person.OccupationId != null)
          if (person.Occupation?._id != monitoring.Person.OccupationId)
            view.OccupationDiff = true;

        var lastdate = monitoring.DateBeginManager;
        if ((monitoring.DateBeginPerson != null) && (monitoring.DateBeginManager != null))
        {
          if (monitoring.DateBeginPerson < monitoring.DateBeginManager)
            lastdate = monitoring.DateBeginPerson;
        }

        if (lastdate == null)
          lastdate = monitoring.DateBeginPerson;

        if (occupationLog != null)
        {
          if (occupationLog.DateLog > lastdate)
            view.MapDiff = true;
        }



        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudMonitoringMobile GetMonitoringsMobile(string id)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();

        var view = new ViewCrudMonitoringMobile()
        {
          _id = monitoring._id,
          Person = monitoring.Person,
          CommentsPerson = monitoring.CommentsPerson,
          CommentsEnd = monitoring.CommentsEnd,
          CommentWarning = monitoring.CommentWarning,
          CommentsManager = monitoring.CommentsManager,
          Items = new List<ViewListItensMobile>(),
          StatusMonitoring = monitoring.StatusMonitoring

        };

        foreach (var item in monitoring.SkillsCompany)
        {
          var result = new ViewListItensMobile()
          {
            _id = item._id,
            Comments = item.Comments?.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              CommentsSpeech = p.CommentsSpeech,
              Date = p.Date,
              SpeechLink = p.SpeechLink,
              StatusView = p.StatusView,
              UserComment = p.UserComment,
              TotalTime = p.TotalTime
            }).ToList(),
            CommentsManager = item.CommentsManager,
            CommentsPerson = item.CommentsPerson,
            StatusViewManager = item.StatusViewManager,
            StatusViewPerson = item.StatusViewPerson,
            Praise = item.Praise,
            Plans = item.Plans
          };
          var detail = new ViewListItensDetailMobile()
          {
            _id = item.Skill._id,
            Name = item.Skill.Name,
            Concept = item.Skill.Concept,
            TypeItem = EnumTypeItem.SkillCompany
          };
          result.Item = detail;

          view.Items.Add(result);
        }

        foreach (var item in monitoring.SkillsGroup)
        {
          var result = new ViewListItensMobile()
          {
            _id = item._id,
            Comments = item.Comments?.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              CommentsSpeech = p.CommentsSpeech,
              Date = p.Date,
              SpeechLink = p.SpeechLink,
              StatusView = p.StatusView,
              UserComment = p.UserComment,
              TotalTime = p.TotalTime
            }).ToList(),
            CommentsManager = item.CommentsManager,
            CommentsPerson = item.CommentsPerson,
            StatusViewManager = item.StatusViewManager,
            StatusViewPerson = item.StatusViewPerson,
            Praise = item.Praise,
            Plans = item.Plans
          };
          var detail = new ViewListItensDetailMobile()
          {
            _id = item.Skill._id,
            Name = item.Skill.Name,
            Concept = item.Skill.Concept,
            TypeItem = EnumTypeItem.SkillGroup
          };
          result.Item = detail;

          view.Items.Add(result);
        }

        foreach (var item in monitoring.SkillsOccupation)
        {
          var result = new ViewListItensMobile()
          {
            _id = item._id,
            Comments = item.Comments?.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              CommentsSpeech = p.CommentsSpeech,
              Date = p.Date,
              SpeechLink = p.SpeechLink,
              StatusView = p.StatusView,
              UserComment = p.UserComment,
              TotalTime = p.TotalTime
            }).ToList(),
            CommentsManager = item.CommentsManager,
            CommentsPerson = item.CommentsPerson,
            StatusViewManager = item.StatusViewManager,
            StatusViewPerson = item.StatusViewPerson,
            Praise = item.Praise,
            Plans = item.Plans
          };
          var detail = new ViewListItensDetailMobile()
          {
            _id = item.Skill._id,
            Name = item.Skill.Name,
            Concept = item.Skill.Concept,
            TypeItem = EnumTypeItem.SkillOccupation
          };
          result.Item = detail;

          view.Items.Add(result);
        }

        foreach (var item in monitoring.Activities)
        {
          var result = new ViewListItensMobile()
          {
            _id = item._id,
            Comments = item.Comments?.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              CommentsSpeech = p.CommentsSpeech,
              Date = p.Date,
              SpeechLink = p.SpeechLink,
              StatusView = p.StatusView,
              UserComment = p.UserComment,
              TotalTime = p.TotalTime
            }).ToList(),
            CommentsManager = item.CommentsManager,
            CommentsPerson = item.CommentsPerson,
            StatusViewManager = item.StatusViewManager,
            StatusViewPerson = item.StatusViewPerson,
            Praise = item.Praise,
            Plans = item.Plans
          };
          var detail = new ViewListItensDetailMobile()
          {
            _id = item.Activities._id,
            Name = item.Activities.Name,
            Order = item.Activities.Order,
            TypeItem = (item.TypeAtivitie == EnumTypeAtivitie.Scope) ? EnumTypeItem.Scope : EnumTypeItem.Activitie
          };
          result.Item = detail;

          view.Items.Add(result);
        }

        foreach (var item in monitoring.Schoolings)
        {
          var result = new ViewListItensMobile()
          {
            _id = item._id,
            Comments = item.Comments?.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              CommentsSpeech = p.CommentsSpeech,
              Date = p.Date,
              SpeechLink = p.SpeechLink,
              StatusView = p.StatusView,
              UserComment = p.UserComment,
              TotalTime = p.TotalTime
            }).ToList(),
            CommentsManager = item.CommentsManager,
            CommentsPerson = item.CommentsPerson,
            StatusViewManager = item.StatusViewManager,
            StatusViewPerson = item.StatusViewPerson,
            Praise = item.Praise,
            Plans = item.Plans
          };
          var detail = new ViewListItensDetailMobile()
          {
            _id = item.Schooling._id,
            Name = item.Schooling.Name,
            Order = item.Schooling.Order,
            TypeItem = EnumTypeItem.Schooling
          };
          result.Item = detail;

          view.Items.Add(result);
        }

        view.Items = view.Items.OrderBy(p => p.Item.TypeItem).ToList();
        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudMonitoringActivities GetMonitoringActivities(string idmonitoring, string idactivitie)
    {
      try
      {
        var view = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault().
          Activities.Where(p => p._id == idactivitie).FirstOrDefault();

        return new ViewCrudMonitoringActivities()
        {
          _id = view._id,
          Activities = view.Activities,
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
      catch (Exception)
      {
        return null;
        //throw e;
      }
    }
    public string UpdateMonitoringActivities(string idmonitoring, ViewCrudMonitoringActivities view)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();

        foreach (var item in monitoring.Activities)
        {
          if (item._id == view._id)
          {
            monitoring.Activities.Remove(item);
            var monitoringActivitie = new MonitoringActivities()
            {
              _id = item._id,
              TypeAtivitie = view.TypeAtivitie,
              StatusViewManager = view.StatusViewManager,
              StatusViewPerson = view.StatusViewPerson,
              Praise = view.Praise,
              Activities = new ViewListActivitie()
              {
                _id = item._id,
                Name = view.Activities.Name,
                Order = view.Activities.Order,
              },
              Plans = item.Plans,
              Comments = new List<ListComments>()
            };
            if (view.Comments != null)
              foreach (var com in view.Comments)
                monitoringActivitie.Comments.Add(new ListComments()
                {
                  _id = com._id ?? ObjectId.GenerateNewId().ToString(),
                  Date = com.Date,
                  Comments = com.Comments,
                  StatusView = com.StatusView,
                  UserComment = com.UserComment,
                });

            monitoring.Activities.Add(monitoringActivitie);
            serviceMonitoring.Update(monitoring, null).Wait();
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
    public string AddMonitoringActivities(string idmonitoring, ViewCrudActivities view)
    {
      try
      {
        var monitoring = serviceMonitoring.GetNewVersion(p => p._id == idmonitoring).Result;
        var person = servicePerson.GetNewVersion(p => p._id == monitoring.Person._id).Result;

        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (person.User._id == _user._idUser)
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

        var activities = new List<ViewListActivitie>();

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
          _id = ObjectId.GenerateNewId().ToString(),
          Order = order,
        };


        monitoringActivitie.Activities = activitie.GetViewList();
        monitoringActivitie._id = ObjectId.GenerateNewId().ToString();
        monitoringActivitie.TypeAtivitie = EnumTypeAtivitie.Individual;
        monitoringActivitie.Plans = new List<ViewCrudPlan>();
        monitoring.Activities.Add(monitoringActivitie);
        serviceMonitoring.Update(monitoring, null).Wait();
        return "add sucess";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListMonitoring NewMonitoring(string idperson, string plataform)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p.Person._id == idperson && p.StatusMonitoring != EnumStatusMonitoring.End).Result.FirstOrDefault();

        if (monitoring == null)
        {
          monitoring = new Monitoring()
          {
            Person = servicePerson.GetNewVersion(p => p._id == idperson).Result.GetViewListPersonInfo()
          };

          LoadMap(monitoring);

          monitoring.StatusMonitoring = EnumStatusMonitoring.Show;
          serviceMonitoring.InsertNewVersion(monitoring).Wait();
          Task.Run(() => LogSave(_user._idPerson, string.Format("Start new process | {0}", monitoring._id), plataform));
        }
        else
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
            LoadMap(monitoring);

          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Wait)
          {
            monitoring.DateBeginEnd = DateTime.Now;
          }
          serviceMonitoring.Update(monitoring, null).Wait();
        }

        return new ViewListMonitoring()
        {
          _id = monitoring._id,
          Name = monitoring.Person.Name,
          idPerson = monitoring.Person._id,
          StatusMonitoring = monitoring.StatusMonitoring
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateMonitoring(ViewCrudMonitoring view, string plataform)
    {
      try
      {
        List<string> countpraise = new List<string>();

        var monitoring = serviceMonitoring.GetNewVersion(p => p._id == view._id).Result;
        var person = servicePerson.GetNewVersion(p => p._id == monitoring.Person._id).Result;

        monitoring.StatusMonitoring = view.StatusMonitoring;
        monitoring.CommentsEnd = view.CommentsEnd;
        monitoring.CommentsPerson = view.CommentsPerson;
        monitoring.CommentsManager = view.CommentsManager;
        monitoring.CommentWarning = view.CommentWarning;

        if (person.Manager != null)
        {
          monitoring.Person.Manager = person.Manager.Name;
          monitoring.Person._idManager = person.Manager._id;
        }

        foreach (var row in monitoring.SkillsCompany)
        {
          var item = view.SkillsCompany.Where(p => p.Skill._id == row.Skill._id).FirstOrDefault();
          row.Praise = item.Praise;
          if (item.Praise != null)
            countpraise.Add(item.Praise);

        };

        
        foreach (var row in monitoring.SkillsGroup)
        {
          var item = view.SkillsGroup.Where(p => p.Skill._id == row.Skill._id).FirstOrDefault();
          row.Praise = item.Praise;
          if (item.Praise != null)
            countpraise.Add(item.Praise);

        };

        foreach (var row in monitoring.SkillsOccupation)
        {
          var item = view.SkillsOccupation.Where(p => p.Skill._id == row.Skill._id).FirstOrDefault();
          row.Praise = item.Praise;
          if (item.Praise != null)
            countpraise.Add(item.Praise);

        };

        foreach (var row in monitoring.Schoolings)
        {
          var item = view.Schoolings.Where(p => p.Schooling._id == row.Schooling._id).FirstOrDefault();
          row.Praise = item.Praise;
          if (item.Praise != null)
            countpraise.Add(item.Praise);

        };
        foreach (var row in monitoring.Activities)
        {
          var item = view.Activities.Where(p => p.Activities._id == row.Activities._id).FirstOrDefault();
          row.Praise = item.Praise;
          if (item.Praise != null)
            countpraise.Add(item.Praise);
        };


        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (person.User._id == _user._idUser)
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

        var userInclude = servicePerson.GetAllNewVersion(p => p.User._id == _user._idUser).Result.FirstOrDefault();

        if (person.User._id != _user._idUser)
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Wait)
          {
            monitoring.DateEndManager = DateTime.Now;
            Task.Run(() => Mail(person));
            Task.Run(() => LogSave(_user._idPerson, string.Format("Send person approval | {0}", monitoring._id), plataform));
          }
        }
        else
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.End)
          {
            Task.Run(() => serviceLogMessages.NewLogMessage("Monitoring", " Monitoring realizado do colaborador " + person.User.Name, person));
            if ((monitoring.Activities.Where(p => p.Praise != null).Count() > 0)
              || (monitoring.SkillsCompany.Where(p => p.Praise != null).Count() > 0)
              || (monitoring.SkillsGroup.Where(p => p.Praise != null).Count() > 0)
              || (monitoring.SkillsOccupation.Where(p => p.Praise != null).Count() > 0)
              || (monitoring.Schoolings.Where(p => p.Praise != null).Count() > 0))
            {
              Task.Run(() => serviceLogMessages.NewLogMessage("Monitoring", " Colaborador " + person.User.Name + " foi elogiado pelo gestor", person));
            }
            monitoring.DateEndEnd = DateTime.Now;
            Task.Run(() => SendQueue(monitoring._id, person._id, countpraise));
            Task.Run(() => LogSave(_user._idPerson, string.Format("Conclusion process | {0}", monitoring._id), plataform));
          }
          else if (monitoring.StatusMonitoring == EnumStatusMonitoring.WaitManager)
          {
            monitoring.DateEndPerson = DateTime.Now;
            Task.Run(() => MailManager(person));
            Task.Run(() => LogSave(_user._idPerson, string.Format("Send manager approval | {0}", monitoring._id), plataform));

          }
          else if (monitoring.StatusMonitoring == EnumStatusMonitoring.Disapproved)
          {
            Task.Run(() => MailDisApproval(person));
            Task.Run(() => LogSave(_user._idPerson, string.Format("Send manager review | {0}", monitoring._id), plataform));
          }
        }
        serviceMonitoring.Update(monitoring, null).Wait();
        return "Monitoring altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public string UpdateStatusMonitoring(string idmonitoring, EnumStatusMonitoring status, string plataform)
    {
      try
      {
        List<string> countpraise = new List<string>();

        var monitoring = serviceMonitoring.GetNewVersion(p => p._id == idmonitoring).Result;
        var person = servicePerson.GetNewVersion(p => p._id == monitoring.Person._id).Result;

        monitoring.StatusMonitoring = status;

        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (person.User._id == _user._idUser)
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

        var userInclude = servicePerson.GetAllNewVersion(p => p.User._id == _user._idUser).Result.FirstOrDefault();

        if (person.User._id != _user._idUser)
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Wait)
          {
            monitoring.DateEndManager = DateTime.Now;
            Task.Run(() => Mail(person));
            Task.Run(() => LogSave(_user._idPerson, string.Format("Send person approval | {0}", monitoring._id), plataform));
          }
        }
        else
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.End)
          {
            Task.Run(() => serviceLogMessages.NewLogMessage("Monitoring", " Monitoring realizado do colaborador " + person.User.Name, person));
            if ((monitoring.Activities.Where(p => p.Praise != null).Count() > 0)
              || (monitoring.SkillsCompany.Where(p => p.Praise != null).Count() > 0)
              || (monitoring.SkillsGroup.Where(p => p.Praise != null).Count() > 0)
              || (monitoring.SkillsOccupation.Where(p => p.Praise != null).Count() > 0)
              || (monitoring.Schoolings.Where(p => p.Praise != null).Count() > 0))
            {
              Task.Run(() => serviceLogMessages.NewLogMessage("Monitoring", " Colaborador " + person.User.Name + " foi elogiado pelo gestor", person));
            }
            monitoring.DateEndEnd = DateTime.Now;
            Task.Run(() => SendQueue(monitoring._id, person._id, countpraise));
            Task.Run(() => LogSave(_user._idPerson, string.Format("Conclusion process | {0}", monitoring._id), plataform));
          }
          else if (monitoring.StatusMonitoring == EnumStatusMonitoring.WaitManager)
          {
            monitoring.DateEndPerson = DateTime.Now;
            Task.Run(() => MailManager(person));
            Task.Run(() => LogSave(_user._idPerson, string.Format("Send manager approval | {0}", monitoring._id), plataform));

          }
          else if (monitoring.StatusMonitoring == EnumStatusMonitoring.Disapproved)
          {
            Task.Run(() => MailDisApproval(person));
            Task.Run(() => LogSave(_user._idPerson, string.Format("Send manager review | {0}", monitoring._id), plataform));
          }
        }
        serviceMonitoring.Update(monitoring, null).Wait();
        return "Monitoring altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListSkill> GetSkills(string idperson)
    {
      try
      {
        var person = servicePerson.GetNewVersion(p => p._id == idperson).Result;
        var group = serviceGroup.GetNewVersion(p => p._id == person.Occupation._idGroup).Result;

        var skillscompany = serviceCompany.GetAllNewVersion(p => p._id == group.Company._id).Result.FirstOrDefault().Skills;
        var skills = serviceOccupation.GetAllNewVersion(p => p._id == person.Occupation._id).Result.FirstOrDefault().Skills;
        var skillsgroup = serviceGroup.GetAllNewVersion(p => p._id == person.Occupation._idGroup).Result.FirstOrDefault().Skills;
        var list = skillsgroup;
        foreach (var item in skills)
          list.Add(item);
        foreach (var item in skillscompany)
          list.Add(item);

        return list.OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListMonitoring> GetListExclud(ref long total, string filter, int count, int page)
    {
      try
      {

        int skip = (count * (page - 1));
        var detail = serviceMonitoring.GetAllNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Person.Name).Skip(skip).Take(count).ToList();
        total = serviceMonitoring.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewListMonitoring()
        {
          _id = p._id,
          Name = p.Person.Name,
          idPerson = p.Person._id,
          StatusMonitoring = p.StatusMonitoring
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void UpdateCommentsSpeech(string idmonitoring, string iditem, EnumUserComment user, string path, string link)
    {
      try
      {

        var viewreport = new ViewReport()
        {
          Data = link,
          Name = "audiomonitoring",
          _idReport = NewReport("audiomonitoring"),
          _idAccount = _user._idAccount
        };
        SendMessageAsync(viewreport);
        var report = new ViewCrudReport();

        while (report.StatusReport == EnumStatusReport.Open)
        {
          var rest = serviceReport.GetNewVersion(p => p._id == viewreport._idReport).Result;
          report.StatusReport = rest.StatusReport;
          report.Link = rest.Link;
        }

        string comments = "";

        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(path);
          var resultMail = client.GetAsync("speech/" + report.Link).Result;
          comments = resultMail.Content.ReadAsStringAsync().Result;
        }

        var view = new ViewCrudComment()
        {
          CommentsSpeech = comments,
          Date = DateTime.Now,
          StatusView = EnumStatusView.None,
          UserComment = user,
          SpeechLink = report.Link
        };

        UpdateComments(idmonitoring, iditem, view, "mobile");
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public string AddCommentsSpeech(string idmonitoring, string iditem, string link, EnumUserComment user, string totalimte, string plataform)
    {
      try
      {

        var view = new ViewCrudComment()
        {
          CommentsSpeech = "",
          Date = DateTime.Now,
          StatusView = EnumStatusView.None,
          UserComment = user,
          SpeechLink = link,
          TotalTime = totalimte
        };
        AddComments(idmonitoring, iditem, view, plataform);
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewCrudComment> AddComments(string idmonitoring, string iditem, ViewCrudComment comments, string plataform)
    {
      try
      {
        var monitoring = serviceMonitoring.GetNewVersion(p => p._id == idmonitoring).Result;
        var person = servicePerson.GetNewVersion(p => p._id == monitoring.Person._id).Result;
        Task.Run(() => LogSave(_user._idPerson, string.Format("Add comment | {0}", idmonitoring), plataform));


        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (person.User._id == _user._idUser)
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
                Comments = comments.Comments,
                Date = comments.Date,
                StatusView = comments.StatusView,
                UserComment = comments.UserComment,
                SpeechLink = comments.SpeechLink,
                TotalTime = comments.TotalTime
              });
            serviceMonitoring.Update(monitoring, null).Wait();

            return item.Comments.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment,
              SpeechLink = p.SpeechLink,
              TotalTime = p.TotalTime
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
               Comments = comments.Comments,
               Date = comments.Date,
               StatusView = comments.StatusView,
               UserComment = comments.UserComment,
               SpeechLink = comments.SpeechLink,
               TotalTime = comments.TotalTime
             });

            serviceMonitoring.Update(monitoring, null).Wait();
            
            return item.Comments.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment,
              SpeechLink = p.SpeechLink,
              TotalTime = p.TotalTime
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
               Comments = comments.Comments,
               Date = comments.Date,
               StatusView = comments.StatusView,
               UserComment = comments.UserComment,
               SpeechLink = comments.SpeechLink,
               TotalTime = comments.TotalTime
             });

            serviceMonitoring.Update(monitoring, null).Wait();
            return item.Comments.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment,
              SpeechLink = p.SpeechLink,
              TotalTime = p.TotalTime
            }).ToList();
          }
        }

        foreach (var item in monitoring.SkillsGroup)
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
               Comments = comments.Comments,
               Date = comments.Date,
               StatusView = comments.StatusView,
               UserComment = comments.UserComment,
               SpeechLink = comments.SpeechLink,
               TotalTime = comments.TotalTime
             });

            serviceMonitoring.Update(monitoring, null).Wait();
            return item.Comments.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment,
              SpeechLink = p.SpeechLink,
              TotalTime = p.TotalTime
            }).ToList();
          }
        }

        foreach (var item in monitoring.SkillsOccupation)
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
               Comments = comments.Comments,
               Date = comments.Date,
               StatusView = comments.StatusView,
               UserComment = comments.UserComment,
               SpeechLink = comments.SpeechLink,
               TotalTime = comments.TotalTime
             });

            serviceMonitoring.Update(monitoring, null).Wait();
            return item.Comments.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment,
              SpeechLink = p.SpeechLink,
              TotalTime = p.TotalTime
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

    public string AddPraise(string idmonitoring, string iditem, ViewText text, string plataform)
    {
      try
      {
        var monitoring = serviceMonitoring.GetNewVersion(p => p._id == idmonitoring).Result;
        var person = servicePerson.GetNewVersion(p => p._id == monitoring.Person._id).Result;
        Task.Run(() => LogSave(_user._idPerson, string.Format("Add praise | {0}", idmonitoring), plataform));


        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (person.User._id == _user._idUser)
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
            item.Praise = text.Text;
            serviceMonitoring.Update(monitoring, null).Wait();

            return text.Text;
          }
        }

        foreach (var item in monitoring.Schoolings)
        {
          if (item._id == iditem)
          {
            item.Praise = text.Text;
            serviceMonitoring.Update(monitoring, null).Wait();

            return text.Text;
          }
        }


        foreach (var item in monitoring.SkillsCompany)
        {
          if (item._id == iditem)
          {
            item.Praise = text.Text;
            serviceMonitoring.Update(monitoring, null).Wait();

            return text.Text;
          }
        }

        foreach (var item in monitoring.SkillsGroup)
        {
          if (item._id == iditem)
          {
            item.Praise = text.Text;
            serviceMonitoring.Update(monitoring, null).Wait();

            return text.Text;
          }
        }
        foreach (var item in monitoring.SkillsOccupation)
        {
          if (item._id == iditem)
          {
            item.Praise = text.Text;
            serviceMonitoring.Update(monitoring, null).Wait();

            return text.Text;
          }
        }
        return "";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewCrudComment> UpdateComments(string idmonitoring, string iditem, ViewCrudComment comments, string plataform)
    {
      try
      {
        var monitoring = serviceMonitoring.GetNewVersion(p => p._id == idmonitoring).Result;
        Task.Run(() => LogSave(_user._idPerson, string.Format("Update comment | {0}", idmonitoring), plataform));

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

                serviceMonitoring.Update(monitoring, null).Wait();
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

                serviceMonitoring.Update(monitoring, null).Wait();
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

                serviceMonitoring.Update(monitoring, null).Wait();
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
          }
        }

        foreach (var item in monitoring.SkillsGroup)
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

                serviceMonitoring.Update(monitoring, null).Wait();
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
          }
        }

        foreach (var item in monitoring.SkillsOccupation)
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

                serviceMonitoring.Update(monitoring, null).Wait();
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
          }
        }
        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateCommentsEndMobile(string idonboarding, EnumUserComment userComment, ViewCrudCommentEnd comments)
    {
      try
      {
        var onboarding = serviceMonitoring.GetNewVersion(p => p._id == idonboarding).Result;
        if (userComment == EnumUserComment.Person)
          onboarding.CommentsPerson = comments.Comments;
        else if (userComment == EnumUserComment.Manager)
          onboarding.CommentsManager = comments.Comments;
        else
          onboarding.CommentsEnd = comments.Comments;

        var i = serviceMonitoring.Update(onboarding, null);

        return "ok";
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
        var monitoring = serviceMonitoring.GetNewVersion(p => p._id == idmonitoring).Result;
        foreach (var item in monitoring.Activities)
        {
          if (item._id == iditem)
          {
            if (item.Comments != null)
              return item.Comments.Select(p => new ViewCrudComment()
              {
                _id = p._id,
                Comments = p.Comments,
                Date = p.Date,
                StatusView = p.StatusView,
                UserComment = p.UserComment,
                SpeechLink = p.SpeechLink
              }).ToList();
          }
        }
        foreach (var item in monitoring.Schoolings)
        {
          if (item._id == iditem)
          {
            if (item.Comments != null)
              return item.Comments.Select(p => new ViewCrudComment()
              {
                _id = p._id,
                Comments = p.Comments,
                Date = p.Date,
                StatusView = p.StatusView,
                UserComment = p.UserComment,
                SpeechLink = p.SpeechLink
              }).ToList();
          }
        }

        foreach (var item in monitoring.SkillsCompany)
        {
          if (item._id == iditem)
          {
            if (item.Comments != null)
              return item.Comments.Select(p => new ViewCrudComment()
              {
                _id = p._id,
                Comments = p.Comments,
                Date = p.Date,
                StatusView = p.StatusView,
                UserComment = p.UserComment,
                SpeechLink = p.SpeechLink
              }).ToList();
          }
        }

        foreach (var item in monitoring.SkillsGroup)
        {
          if (item._id == iditem)
          {
            if (item.Comments != null)
              return item.Comments.Select(p => new ViewCrudComment()
              {
                _id = p._id,
                Comments = p.Comments,
                Date = p.Date,
                StatusView = p.StatusView,
                UserComment = p.UserComment,
                SpeechLink = p.SpeechLink
              }).ToList();
          }
        }

        foreach (var item in monitoring.SkillsOccupation)
        {
          if (item._id == iditem)
          {
            if (item.Comments != null)
              return item.Comments.Select(p => new ViewCrudComment()
              {
                _id = p._id,
                Comments = p.Comments,
                Date = p.Date,
                StatusView = p.StatusView,
                UserComment = p.UserComment,
                SpeechLink = p.SpeechLink
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



    public List<ViewExportStatusMonitoring> ExportStatusMonitoring(string idperson)
    {
      try
      {

        var list = serviceMonitoring.GetAllNewVersion(p => p.Person._id == idperson).Result;
        List<ViewExportStatusMonitoring> result = new List<ViewExportStatusMonitoring>();

        foreach (var item in list)
        {
          result.Add(new ViewExportStatusMonitoring
          {
            IdMonitoring = item._id,
            NamePerson = item.Person.Name,
            Status = item.StatusMonitoring,
            Occupation = item.Person.Occupation,
            DataEnd = item.DateEndEnd
          });
        }

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewExportMonitoringComments> ExportMonitoringComments(ViewFilterIdAndDate filter)
    {
      try
      {
        List<ViewExportMonitoringComments> result = new List<ViewExportMonitoringComments>();
        foreach (ViewListIdIndicators rows in filter.Persons)
        {
          var monitorings = serviceMonitoring.GetAllNewVersion(p => p.Person._id == rows._id && p.StatusMonitoring == EnumStatusMonitoring.End
           && p.DateEndEnd >= filter.Date.Begin && p.DateEndEnd <= filter.Date.End).Result;

          foreach (var monitoring in monitorings)
          {
            var view = new ViewExportMonitoringComments();


            foreach (var item in monitoring.Schoolings)
            {
              view = new ViewExportMonitoringComments();
              view.NameManager = monitoring.Person.Manager;
              view.NamePerson = monitoring.Person.Name;
              view.NameItem = item.Schooling.Name;
              view.CommentsManager = monitoring.CommentsManager;
              view.CommentsPerson = monitoring.CommentsPerson;
              view.CommentsEnd = monitoring.CommentsEnd;
              if (item.Comments != null)
                foreach (var comm in item.Comments)
                {
                  view.Date = comm.Date;
                  view.Comments = comm.Comments;
                }

              view.Praise = item.Praise;

              if (view.Praise == string.Empty)
                view.Praise = null;
              if (view.Comments == string.Empty)
                view.Comments = null;

              if ((view.Praise != null) || (view.Comments != null))
                result.Add(view);
            }

            foreach (var item in monitoring.Activities)
            {

              view = new ViewExportMonitoringComments();
              view.NameManager = monitoring.Person.Manager;
              view.NamePerson = monitoring.Person.Name;
              view.NameItem = item.Activities.Name;
              view.CommentsManager = monitoring.CommentsManager;
              view.CommentsPerson = monitoring.CommentsPerson;
              view.CommentsEnd = monitoring.CommentsEnd;

              if (item.Comments != null)
                foreach (var comm in item.Comments)
                {
                  view.Date = comm.Date;
                  view.Comments = comm.Comments;
                }
              view.Praise = item.Praise;

              if (view.Praise == string.Empty)
                view.Praise = null;
              if (view.Comments == string.Empty)
                view.Comments = null;

              if ((view.Praise != null) || (view.Comments != null))
                result.Add(view);
            }

            foreach (var item in monitoring.SkillsCompany)
            {
              view = new ViewExportMonitoringComments();
              view.NameManager = monitoring.Person.Manager;
              view.NamePerson = monitoring.Person.Name;
              view.NameItem = item.Skill.Name;
              view.CommentsManager = monitoring.CommentsManager;
              view.CommentsPerson = monitoring.CommentsPerson;
              view.CommentsEnd = monitoring.CommentsEnd;

              if (item.Comments != null)
                foreach (var comm in item.Comments)
                {
                  view.Date = comm.Date;
                  view.Comments = comm.Comments;
                }
              view.Praise = item.Praise;
              if (view.Praise == string.Empty)
                view.Praise = null;
              if (view.Comments == string.Empty)
                view.Comments = null;

              if ((view.Praise != null) || (view.Comments != null))
                result.Add(view);
            }

            foreach (var item in monitoring.SkillsGroup)
            {
              view = new ViewExportMonitoringComments();
              view.NameManager = monitoring.Person.Manager;
              view.NamePerson = monitoring.Person.Name;
              view.NameItem = item.Skill.Name;
              view.CommentsManager = monitoring.CommentsManager;
              view.CommentsPerson = monitoring.CommentsPerson;
              view.CommentsEnd = monitoring.CommentsEnd;

              if (item.Comments != null)
                foreach (var comm in item.Comments)
                {
                  view.Date = comm.Date;
                  view.Comments = comm.Comments;
                }
              view.Praise = item.Praise;
              if (view.Praise == string.Empty)
                view.Praise = null;
              if (view.Comments == string.Empty)
                view.Comments = null;

              if ((view.Praise != null) || (view.Comments != null))
                result.Add(view);
            }

            foreach (var item in monitoring.SkillsOccupation)
            {
              view = new ViewExportMonitoringComments();
              view.NameManager = monitoring.Person.Manager;
              view.NamePerson = monitoring.Person.Name;
              view.NameItem = item.Skill.Name;
              view.CommentsManager = monitoring.CommentsManager;
              view.CommentsPerson = monitoring.CommentsPerson;
              view.CommentsEnd = monitoring.CommentsEnd;

              if (item.Comments != null)
                foreach (var comm in item.Comments)
                {
                  view.Date = comm.Date;
                  view.Comments = comm.Comments;
                }
              view.Praise = item.Praise;
              if (view.Praise == string.Empty)
                view.Praise = null;
              if (view.Comments == string.Empty)
                view.Comments = null;

              if ((view.Praise != null) || (view.Comments != null))
                result.Add(view);
            }
          }
        }
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewExportStatusMonitoringGeral> ExportStatusMonitoring(List<ViewListIdIndicators> persons)
    {
      try
      {

        List<Person> list = servicePerson.GetAllNewVersion(p => p.TypeJourney == EnumTypeJourney.Monitoring).Result;
        List<ViewExportStatusMonitoringGeral> result = new List<ViewExportStatusMonitoringGeral>();

        foreach (ViewListIdIndicators rows in persons)
        {
          var monitorings = serviceMonitoring.GetAllNewVersion(p => p.Person._id == rows._id).Result;
          if (monitorings != null)
          {
            foreach (var item in monitorings)
            {
              if (persons.Where(p => p._id == item.Person._id).Count() > 0)
                result.Add(new ViewExportStatusMonitoringGeral
                {
                  NameManager = item.Person.Manager ?? "Sem Gestor",
                  NamePerson = item.Person.Name,
                  Occupation = item.Person.Occupation,
                  Status =
              item == null ? "Aguardando para iniciar" :
                item.StatusMonitoring == EnumStatusMonitoring.Open ? "Aguardando para iniciar" :
                  item.StatusMonitoring == EnumStatusMonitoring.InProgressPerson ? "Em andamento pelo colaborador" :
                    item.StatusMonitoring == EnumStatusMonitoring.InProgressManager ? "Em andamento pelo gestor" :
                      item.StatusMonitoring == EnumStatusMonitoring.Wait ? "Em andamento pelo gestor" :
                        item.StatusMonitoring == EnumStatusMonitoring.End ? "Finalizado" :
                          item.StatusMonitoring == EnumStatusMonitoring.WaitManager ? "Aguardando continuação pelo gestor" :
                            item.StatusMonitoring == EnumStatusMonitoring.Disapproved ? "Aguardando revisão do gestor" : "Aguardando para iniciar",
                  DateBegin = item?.DateBeginPerson,
                  DateEnd = item?.DateEndEnd
                });
            }
          }
          else
          {
            if (rows.TypeJourney == EnumTypeJourney.Monitoring)
            {
              var person = servicePerson.GetNewVersion(p => p._id == rows._id).Result;
              result.Add(new ViewExportStatusMonitoringGeral
              {
                NameManager = person.Manager == null ? "Sem Gestor" : person.Manager.Name,
                NamePerson = person.User.Name,
                Occupation = person.Occupation.Name,
                Status = "Aguardando para iniciar"
              });
            }
          }
        }
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region private

    private string NewReport(string name)
    {
      try
      {
        var report = new Reports()
        {
          StatusReport = EnumStatusReport.Open,
          Date = DateTime.Now,
          Name = name
        };

        return serviceReport.InsertNewVersion(report).Result._id;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void SendMessageAsync(ViewReport view)
    {
      try
      {
        dynamic result = view;
        var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result)));
        queueClient.SendAsync(message);
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    private void SendQueue(string id, string idperson, List<string> countpraise)
    {
      try
      {
        var data = new ViewCrudMaturityRegister
        {
          _idPerson = idperson,
          TypeMaturity = EnumTypeMaturity.Monitoring,
          _idRegister = id,
          Date = DateTime.Now,
          _idAccount = _user._idAccount
        };

        serviceControlQueue.SendMessageAsync(JsonConvert.SerializeObject(data));


        foreach (var item in countpraise)
        {
          data = new ViewCrudMaturityRegister
          {
            _idPerson = idperson,
            TypeMaturity = EnumTypeMaturity.Praise,
            _idRegister = item,
            Date = DateTime.Now,
            _idAccount = _user._idAccount
          };
          serviceControlQueue.SendMessageAsync(JsonConvert.SerializeObject(data));
        }


        //serviceControlQueue.RegisterOnMessageHandlerAndReceiveMesssages();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void LogSave(string iduser, string local, string plataform)
    {
      try
      {
        var user = servicePerson.GetAllNewVersion(p => p._id == iduser).Result.FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "Access Monitoring " + plataform,
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
    private string UpdatePlan(Plan plan)
    {
      try
      {
        servicePlan.Update(plan, null).Wait();
        return "Plan altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private Plan AddPlan(Plan plan, Person person, string _idMonitoring, string _idItem)
    {
      try
      {
        plan.DateInclude = DateTime.Now;
        plan._idMonitoring = _idMonitoring;
        plan._idItem = _idItem;
        plan.Person = person.GetViewListBaseManager();
        return servicePlan.InsertNewVersion(plan).Result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private Monitoring LoadMap(Monitoring monitoring)
    {
      try
      {
        var person = servicePerson.GetNewVersion(p => p._id == monitoring.Person._id).Result;
        var occupation = serviceOccupation.GetNewVersion(p => p._id == person.Occupation._id).Result;
        var group = serviceGroup.GetNewVersion(p => p._id == occupation.Group._id).Result;
        var company = serviceCompany.GetNewVersion(p => p._id == group.Company._id).Result;

        monitoring.SkillsCompany = new List<MonitoringSkills>();
        foreach (var item in company.Skills)
        {
          monitoring.SkillsCompany.Add(new MonitoringSkills() { Skill = item, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<ViewCrudPlan>(), Comments = new List<ListComments>() });
        }


        monitoring.SkillsGroup = new List<MonitoringSkills>();
        foreach (var item in group.Skills)
        {
          monitoring.SkillsGroup.Add(new MonitoringSkills() { Skill = item, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<ViewCrudPlan>(), Comments = new List<ListComments>() });
        }

        monitoring.SkillsOccupation = new List<MonitoringSkills>();
        foreach (var item in occupation.Skills)
        {
          monitoring.SkillsOccupation.Add(new MonitoringSkills() { Skill = item, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<ViewCrudPlan>(), Comments = new List<ListComments>() });
        }

        monitoring.Activities = new List<MonitoringActivities>();
        foreach (var item in occupation.Activities)
        {
          monitoring.Activities.Add(new MonitoringActivities() { TypeAtivitie = EnumTypeAtivitie.Occupation, Activities = item, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<ViewCrudPlan>(), Comments = new List<ListComments>() });
        }

        foreach (var item in group.Scope)
        {
          monitoring.Activities.Add(new MonitoringActivities() { TypeAtivitie = EnumTypeAtivitie.Scope, Activities = new ViewListActivitie() { _id = item._id, Name = item.Name, Order = item.Order }, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<ViewCrudPlan>(), Comments = new List<ListComments>() });
        }

        monitoring.Schoolings = new List<MonitoringSchooling>();
        foreach (var item in occupation.Schooling)
        {
          monitoring.Schoolings.Add(new MonitoringSchooling() { Schooling = item, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<ViewCrudPlan>(), Comments = new List<ListComments>() });
        }

        return monitoring;
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
        var model = serviceMailModel.MonitoringApproval(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        string managername = "";
        try
        {
          managername = servicePerson.GetAllNewVersion(p => p._id == person.Manager._id).Result.FirstOrDefault().User.Name;
        }
        catch (Exception)
        {

        }

        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", managername).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
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
        var mailObj = serviceMail.InsertNewVersion(sendMail).Result;
        var token = SendMail(path, person, mailObj._id.ToString());
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void MailManager(Person person)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.MonitoringApprovalManager(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        string managername = "";
        try
        {
          managername = servicePerson.GetAllNewVersion(p => p._id == person.Manager._id).Result.FirstOrDefault().User.Name;
        }
        catch (Exception)
        {

        }

        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", managername).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
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
        var mailObj = serviceMail.InsertNewVersion(sendMail).Result;
        var token = SendMail(path, person, mailObj._id.ToString());
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void MailDisApproval(Person person)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.MonitoringDisapproval(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        string managername = "";
        try
        {
          managername = servicePerson.GetAllNewVersion(p => p._id == person.Manager._id).Result.FirstOrDefault().User.Name;
        }
        catch (Exception)
        {

        }

        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", managername).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
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
        var mailObj = serviceMail.InsertNewVersion(sendMail).Result;
        var token = SendMail(path, person, mailObj._id.ToString());
      }
      catch (Exception e)
      {
        throw e;
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

    public void RegisterOnMessageHandlerAndReceiveMesssages()
    {
      try
      {
        var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
        {
          MaxConcurrentCalls = 1,
          AutoComplete = false
        };

        queueClientReturn.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async Task ProcessMessagesAsync(Message message, CancellationToken token)
    {
      try
      {
        var view = JsonConvert.DeserializeObject<ViewCrudReport>(Encoding.UTF8.GetString(message.Body));
        SetUser(new BaseUser()
        {
          _idAccount = view._idAccount
        });

        if (view.StatusReport != EnumStatusReport.Open)
        {
          Reports report = serviceReport.GetFreeNewVersion(p => p._id == view._id).Result;
          report.StatusReport = view.StatusReport;
          report.Link = view.Link;
          serviceReport.UpdateAccount(report, null).Wait();

          await queueClientReturn.CompleteAsync(message.SystemProperties.LockToken);
        }
      }
      catch (Exception e)
      {
        var error = e.Message;
        await queueClientReturn.CompleteAsync(message.SystemProperties.LockToken);
      }

    }
    private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
    {
      var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
      return Task.CompletedTask;
    }


    #endregion

#pragma warning restore 1998
  }
}