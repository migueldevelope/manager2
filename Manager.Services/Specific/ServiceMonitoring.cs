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
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    private readonly ServiceGeneric<MonitoringActivities> serviceMonitoringActivities;
    private readonly ServiceGeneric<Occupation> serviceOccupation;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Plan> servicePlan;
    private readonly IServiceControlQueue serviceControlQueue;

    public string path;

    #region Constructor
    public ServiceMonitoring(DataContext context, DataContext contextLog, string pathToken, IServiceControlQueue _serviceControlQueue) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context, contextLog);
        serviceLog = new ServiceLog(context, contextLog);
        serviceLogMessages = new ServiceLogMessages(context);
        serviceMail = new ServiceGeneric<MailLog>(context);
        serviceMailModel = new ServiceMailModel(context);
        serviceMonitoring = new ServiceGeneric<Monitoring>(context);
        serviceMonitoringActivities = new ServiceGeneric<MonitoringActivities>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        servicePlan = new ServiceGeneric<Plan>(context);
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
      User(contextAccessor);
      serviceLog.SetUser(_user);
      serviceLogMessages.SetUser(_user);
      serviceMail._user = _user;
      serviceMailModel.SetUser(_user);
      serviceMonitoring._user = _user;
      serviceMonitoringActivities._user = _user;
      serviceOccupation._user = _user;
      servicePerson._user = _user;
      servicePlan._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceLog.SetUser(user);
      serviceLogMessages.SetUser(user);
      serviceMail._user = user;
      serviceMailModel.SetUser(user);
      serviceMonitoring._user = user;
      serviceMonitoringActivities._user = user;
      serviceOccupation._user = user;
      servicePerson._user = user;
      servicePlan._user = user;
    }
    #endregion

    #region Monitoring
    public async Task<string> RemoveMonitoringActivities(string idmonitoring, string idactivitie)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();
        foreach (var item in monitoring.Activities)
        {
          if (item._id == idactivitie)
          {
            item.Status = EnumStatus.Disabled;
            await serviceMonitoring.Update(monitoring, null);
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
    public async Task<string> RemoveAllMonitoring(string idperson)
    {
      try
      {


        var monitorings = serviceMonitoring.GetAllNewVersion(p => p.Person._id == idperson).Result.ToList();
        foreach (var monitoring in monitorings)
        {
          monitoring.Status = EnumStatus.Disabled;
          await serviceMonitoring.Update(monitoring, null);
        }
        return "Monitorings deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<string> RemoveMonitoring(string idmonitoring)
    {
      try
      {

        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();
        monitoring.Status = EnumStatus.Disabled;
        await serviceMonitoring.Update(monitoring, null);
        return "Monitoring deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<string> RemoveLastMonitoring(string idperson)
    {
      try
      {

        var monitoring = serviceMonitoring.GetAllNewVersion(p => p.Person._id == idperson).Result.LastOrDefault();
        monitoring.Status = EnumStatus.Disabled;
        await serviceMonitoring.Update(monitoring, null);
        return "Monitoring deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<string> UpdateCommentsView(string idmonitoring, string iditem, EnumUserComment userComment)
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

            await serviceMonitoring.Update(monitoring, null);
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

            await serviceMonitoring.Update(monitoring, null);
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

            await serviceMonitoring.Update(monitoring, null);
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
    public async Task<string> DeleteComments(string idmonitoring, string iditem, string idcomments)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();
        foreach (var item in monitoring.Activities)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                await serviceMonitoring.Update(monitoring, null);
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
                await serviceMonitoring.Update(monitoring, null);
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
                await serviceMonitoring.Update(monitoring, null);
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
    public async Task<bool> ValidComments(string id)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
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

    public async Task<List<ViewCrudPlan>> AddPlan(string idmonitoring, string iditem, ViewCrudPlan plan)
    {
      try
      {
        var userInclude = servicePerson.GetAllNewVersion(p => p.User._id == _user._idUser).Result.FirstOrDefault();
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();

        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (monitoring.Person.User._id == _user._idUser)
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
        await serviceLogMessages.NewLogMessage("Plano", " Ação de desenvolvimento acordada para o colaborador " + monitoring.Person.User.Name, monitoring.Person);
        var newPlan = AddPlan(new Plan()
        {
          _id = plan._id,
          Name = plan.Name,
          Description = plan.Description,
          Deadline = plan.Deadline,
          Skills = plan.Skills?.Select(x => new Skill()
          {
            _id = x._id,
            Name = x.Name,
            Concept = x.Concept,
            TypeSkill = x.TypeSkill
          }).ToList(),
          SourcePlan = plan.SourcePlan,
          TypePlan = plan.TypePlan
        }, monitoring.Person, idmonitoring, iditem);

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
                Skills = newPlan.Skills?.Select(x => new ViewListSkill()
                {
                  _id = x._id,
                  Name = x.Name,
                  Concept = x.Concept,
                  TypeSkill = x.TypeSkill
                }).ToList(),
                SourcePlan = newPlan.SourcePlan,
                TypePlan = newPlan.TypePlan
              });
              await serviceMonitoring.Update(monitoring, null);
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
                Skills = newPlan.Skills?.Select(x => new ViewListSkill()
                {
                  _id = x._id,
                  Name = x.Name,
                  Concept = x.Concept,
                  TypeSkill = x.TypeSkill
                }).ToList(),
                SourcePlan = newPlan.SourcePlan,
                TypePlan = newPlan.TypePlan
              });
              await serviceMonitoring.Update(monitoring, null);
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
                Skills = newPlan.Skills?.Select(x => new ViewListSkill()
                {
                  _id = x._id,
                  Name = x.Name,
                  Concept = x.Concept,
                  TypeSkill = x.TypeSkill
                }).ToList(),
                SourcePlan = newPlan.SourcePlan,
                TypePlan = newPlan.TypePlan
              });
              await serviceMonitoring.Update(monitoring, null);
              return item.Plans.Select(p => new ViewCrudPlan()
              {
                _id = p._id,
                Name = p.Name,
                Description = p.Description,
                Deadline = p.Deadline,
                Skills = p.Skills?.Select(x => new ViewListSkill()
                {
                  _id = x._id,
                  Name = x.Name,
                  Concept = x.Concept,
                  TypeSkill = x.TypeSkill
                }).ToList(),
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
    public async Task<List<ViewCrudPlan>> UpdatePlan(string idmonitoring, string iditem, ViewCrudPlan view)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();
        await serviceLogMessages.NewLogMessage("Plano", " Ação de desenvolvimento acordada do colaborador " + monitoring.Person.User.Name, monitoring.Person);
        var plan = servicePlan.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
        plan.Description = view.Description;
        plan.Deadline = view.Deadline;
        plan.Name = view.Name;
        plan.SourcePlan = view.SourcePlan;
        plan.TypePlan = view.TypePlan;
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
                  await UpdatePlan(plan);
                  await serviceMonitoring.Update(monitoring, null);
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
                  await UpdatePlan(plan);
                  await serviceMonitoring.Update(monitoring, null);
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
                  await UpdatePlan(plan);
                  await serviceMonitoring.Update(monitoring, null);
                  return item.Plans;
                }
              }
            }
          }
        }
        await serviceMonitoring.Update(monitoring, null);
        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public Task<List<ViewListMonitoring>> ListMonitoringsEnd(string idmanager, ref  long total,  string filter, int count,int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = serviceMonitoring.GetAllNewVersion(p => p.Person.Manager._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result.Skip(skip).Take(count).ToList();
        total = serviceMonitoring.CountNewVersion(p => p.Person.Manager._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return Task.FromResult(detail.Select(p => new ViewListMonitoring()
        {
          _id = p._id,
          Name = p.Person.User.Name,
          idPerson = p.Person._id,
          StatusMonitoring = p.StatusMonitoring
        }).ToList());
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public Task<List<ViewListMonitoring>> ListMonitoringsWait(string idmanager, ref  long total,  string filter, int count,int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var list = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.TypeJourney == EnumTypeJourney.Monitoring & p.Manager._id == idmanager
        & p.User.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.User.Name)
        .ToList().Select(p => new { Person = p, Monitoring = serviceMonitoring.GetAllNewVersion(x => x.StatusMonitoring != EnumStatusMonitoring.End & x.Person._id == p._id).Result.FirstOrDefault() })
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
        return Task.FromResult(detail.Skip(skip).Take(count).Select(p => new ViewListMonitoring()
        {
          _id = p._id,
          Name = p.Person.User.Name,
          idPerson = p.Person._id,
          StatusMonitoring = p.StatusMonitoring,
          DateEndEnd = p.DateEndEnd,
          OccupationName = p.Person.Occupation.Name
        }).ToList());
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<List<ViewListMonitoring>> PersonMonitoringsEnd(string idmanager)
    {
      try
      {
        return serviceMonitoring.GetAllNewVersion(p => p.Person._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End).Result.OrderBy(p => p.Person.User.Name)
          .Select(p => new ViewListMonitoring()
          {
            _id = p._id,
            Name = p.Person.User.Name,
            idPerson = p.Person._id,
            StatusMonitoring = p.StatusMonitoring,
            OccupationName = p.Person.Occupation.Name,
            DateEndEnd = p.DateEndEnd
          }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<ViewListMonitoring> PersonMonitoringsWait(string idmanager)
    {
      try
      {
        if (!serviceMonitoring.Exists("Monitoring"))
          return null;

        var item = (await servicePerson.GetAllNewVersion(p => p.TypeJourney == EnumTypeJourney.Monitoring & p._id == idmanager)
        ).ToList().Select(p => new { Person = p, Monitoring = serviceMonitoring.GetAllNewVersion(x => x.StatusMonitoring != EnumStatusMonitoring.End & x.Person._id == p._id).Result.FirstOrDefault() })
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
        throw e;
      }
    }
    public async Task<ViewCrudMonitoring> GetMonitorings(string id)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();

        var view = new ViewCrudMonitoring()
        {
          _id = monitoring._id,
          Person = new ViewListPersonInfo()
          {
            _id = monitoring.Person._id,
            TypeJourney = monitoring.Person.TypeJourney,
            Occupation = monitoring.Person.Occupation.Name,
            Name = monitoring.Person.User.Name,
            Manager = monitoring.Person.Manager.Name,
            Company = new ViewListCompany() { _id = monitoring.Person.Company._id, Name = monitoring.Person.Company.Name },
            Establishment = (monitoring.Person.Establishment == null) ? null : new ViewListEstablishment() { _id = monitoring.Person.Establishment._id, Name = monitoring.Person.Establishment.Name },
            Registration = monitoring.Person.Registration,
            User = new ViewListUser()
            {
              Document = monitoring.Person.User.Document,
              Mail = monitoring.Person.User.Mail,
              Name = monitoring.Person.User.Name,
              Phone = monitoring.Person.User.Phone,
              _id = monitoring.Person.User._id
            }
          },
          CommentsPerson = monitoring.CommentsPerson,
          CommentsEnd = monitoring.CommentsEnd,
          CommentsManager = monitoring.CommentsManager,
          SkillsCompany = monitoring.SkillsCompany?.Select(p => new ViewCrudMonitoringSkills()
          {
            _id = p._id,
            Comments = p.Comments?.Select(x => new ViewCrudComment()
            {
              _id = x._id,
              Comments = x.Comments,
              Date = x.Date,
              StatusView = x.StatusView,
              UserComment = x.UserComment
            }).ToList(),
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Skill = new ViewListSkill()
            {
              _id = p.Skill._id,
              Name = p.Skill.Name,
              Concept = p.Skill.Concept,
              TypeSkill = p.Skill.TypeSkill
            },
            Praise = p.Praise,
            Plans = p.Plans?.Select(x => new ViewCrudPlan()
            {
              _id = x._id,
              Name = x.Name,
              Description = x.Description,
              Deadline = x.Deadline,
              Skills = x.Skills?.Select(y => new ViewListSkill()
              {
                _id = y._id,
                Name = y.Name,
                Concept = y.Concept,
                TypeSkill = y.TypeSkill
              }).ToList(),
              SourcePlan = x.SourcePlan,
              TypePlan = x.TypePlan
            }).ToList()
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
              UserComment = x.UserComment
            }).ToList(),
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Praise = p.Praise,
            Schooling = new ViewCrudSchooling()
            {
              _id = p.Schooling._id,
              Name = p.Schooling.Name,
              Complement = p.Schooling.Complement,
              Order = p.Schooling.Order,
              Type = p.Schooling.Type
            },
            Plans = p.Plans?.Select(x => new ViewCrudPlan()
            {
              _id = x._id,
              Name = x.Name,
              Description = x.Description,
              Deadline = x.Deadline,
              Skills = x.Skills?.Select(y => new ViewListSkill()
              {
                _id = y._id,
                Name = y.Name,
                Concept = y.Concept,
                TypeSkill = y.TypeSkill
              }).ToList(),
              SourcePlan = x.SourcePlan,
              TypePlan = x.TypePlan
            }).ToList()
          }).ToList(),
          Activities = monitoring.Activities?.Select(p => new ViewCrudMonitoringActivities()
          {
            Status = p.Status,
            _id = p._id,
            Comments = p.Comments?.Select(x => new ViewCrudComment()
            {
              _id = x._id,
              Comments = x.Comments,
              Date = x.Date,
              StatusView = x.StatusView,
              UserComment = x.UserComment
            }).ToList(),
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            TypeAtivitie = p.TypeAtivitie,
            Praise = p.Praise,
            Activities = new ViewListActivitie()
            {
              _id = p.Activities._id,
              Name = p.Activities.Name,
              Order = p.Activities.Order
            },
            Plans = p.Plans?.Select(x => new ViewCrudPlan()
            {
              _id = x._id,
              Name = x.Name,
              Description = x.Description,
              Deadline = x.Deadline,
              Skills = x.Skills?.Select(y => new ViewListSkill()
              {
                _id = y._id,
                Name = y.Name,
                Concept = y.Concept,
                TypeSkill = y.TypeSkill
              }).ToList(),
              SourcePlan = x.SourcePlan,
              TypePlan = x.TypePlan
            }).ToList()
          }).ToList(),
          StatusMonitoring = monitoring.StatusMonitoring
        };

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<ViewCrudMonitoringActivities> GetMonitoringActivities(string idmonitoring, string idactivitie)
    {
      try
      {
        var view = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault().
          Activities.Where(p => p._id == idactivitie).FirstOrDefault();

        return new ViewCrudMonitoringActivities()
        {
          Status = view.Status,
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
      catch (Exception)
      {
        return null;
        //throw e;
      }
    }
    public async Task<string> UpdateMonitoringActivities(string idmonitoring, ViewCrudMonitoringActivities view)
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
              _idAccount = item._idAccount,
              Status = item.Status,
              TypeAtivitie = view.TypeAtivitie,
              StatusViewManager = view.StatusViewManager,
              StatusViewPerson = view.StatusViewPerson,
              Praise = view.Praise,
              Activities = new Activitie()
              {
                _id = item._id,
                _idAccount = item._idAccount,
                Name = view.Activities.Name,
                Order = view.Activities.Order,
                Status = item.Status
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
                  Status = EnumStatus.Enabled,
                  StatusView = com.StatusView,
                  UserComment = com.UserComment,
                  _idAccount = _user._idAccount
                });

            monitoring.Activities.Add(monitoringActivitie);
            await serviceMonitoring.Update(monitoring, null);
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
    public async Task<string> AddMonitoringActivities(string idmonitoring, ViewCrudActivities view)
    {
      try
      {

        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();

        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (monitoring.Person.User._id == _user._idUser)
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
        monitoringActivitie.Plans = new List<ViewCrudPlan>();
        monitoring.Activities.Add(monitoringActivitie);
        await serviceMonitoring.Update(monitoring, null);
        return "add sucess";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<ViewListMonitoring> NewMonitoring(string idperson)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p.Person._id == idperson && p.StatusMonitoring != EnumStatusMonitoring.End).Result.FirstOrDefault();

        if (monitoring == null)
        {
          monitoring = new Monitoring()
          {
            Person = servicePerson.GetAllNewVersion(p => p._id == idperson).Result.FirstOrDefault()
          };

          LoadMap(monitoring);

          monitoring.StatusMonitoring = EnumStatusMonitoring.Show;
          await serviceMonitoring.InsertNewVersion(monitoring);
          await Task.Run(() => LogSave(_user._idPerson, string.Format("Start new process | {0}", monitoring._id)));
        }
        else
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
            LoadMap(monitoring);

          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Wait)
          {
            monitoring.DateBeginEnd = DateTime.Now;
          }
          await serviceMonitoring.Update(monitoring, null);
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
        throw e;
      }
    }
    public async Task<string> UpdateMonitoring(ViewCrudMonitoring view)
    {
      try
      {
        List<string> countpraise = new List<string>();

        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
        monitoring.StatusMonitoring = view.StatusMonitoring;
        monitoring.CommentsEnd = view.CommentsEnd;
        monitoring.CommentsPerson = view.CommentsPerson;
        monitoring.CommentsManager = view.CommentsManager;

        foreach (var row in monitoring.SkillsCompany)
        {
          var item = view.SkillsCompany.Where(p => p.Skill._id == row.Skill._id).FirstOrDefault();
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
          if (monitoring.Person.User._id == _user._idUser)
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

        if (monitoring.Person.User._id != _user._idUser)
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.Wait)
          {
            monitoring.DateEndManager = DateTime.Now;
            await Task.Run(() => Mail(monitoring.Person));
            await Task.Run(() => LogSave(_user._idPerson, string.Format("Send person approval | {0}", monitoring._id)));
          }
        }
        else
        {
          if (monitoring.StatusMonitoring == EnumStatusMonitoring.End)
          {
            await serviceLogMessages.NewLogMessage("Monitoring", " Monitoring realizado do colaborador " + monitoring.Person.User.Name, monitoring.Person);
            if ((monitoring.Activities.Where(p => p.Praise != null).Count() > 0)
              || (monitoring.SkillsCompany.Where(p => p.Praise != null).Count() > 0)
              || (monitoring.Schoolings.Where(p => p.Praise != null).Count() > 0))
            {
              await serviceLogMessages.NewLogMessage("Monitoring", " Colaborador " + monitoring.Person.User.Name + " foi elogiado pelo gestor", monitoring.Person);
            }
            monitoring.DateEndEnd = DateTime.Now;
            await Task.Run(() => SendQueue(monitoring._id, monitoring.Person._id, countpraise));
            await Task.Run(() => LogSave(_user._idPerson, string.Format("Conclusion process | {0}", monitoring._id)));
          }
          else if (monitoring.StatusMonitoring == EnumStatusMonitoring.WaitManager)
          {
            monitoring.DateEndPerson = DateTime.Now;
            await Task.Run(() => MailManager(monitoring.Person));
            await Task.Run(() => LogSave(_user._idPerson, string.Format("Send manager approval | {0}", monitoring._id)));

          }
          else if (monitoring.StatusMonitoring == EnumStatusMonitoring.Disapproved)
          {
            await Task.Run(() => MailDisApproval(monitoring.Person));
            await Task.Run(() => LogSave(_user._idPerson, string.Format("Send manager review | {0}", monitoring._id)));
          }
        }
        await serviceMonitoring.Update(monitoring, null);
        return "Monitoring altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<List<ViewListSkill>> GetSkills(string idperson)
    {
      try
      {
        var skills = servicePerson.GetAllNewVersion(p => p._id == idperson).Result.FirstOrDefault().Occupation.Group.Skills;
        var skillsgroup = servicePerson.GetAllNewVersion(p => p._id == idperson).Result.FirstOrDefault().Occupation.Skills;
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
        throw e;
      }
    }
    public Task<List<ViewListMonitoring>> GetListExclud(ref  long total,  string filter, int count,int page)
    {
      try
      {

        int skip = (count * (page - 1));
        var detail = serviceMonitoring.GetAllNewVersion(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceMonitoring.CountNewVersion(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return Task.FromResult(detail.Select(p => new ViewListMonitoring()
        {
          _id = p._id,
          Name = p.Person.User.Name,
          idPerson = p.Person._id,
          StatusMonitoring = p.StatusMonitoring
        }).ToList());
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<List<ViewCrudComment>> AddComments(string idmonitoring, string iditem, ViewCrudComment comments)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();
        if (monitoring.StatusMonitoring == EnumStatusMonitoring.Show)
        {
          if (monitoring.Person.User._id == _user._idUser)
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
            await Task.Run(() => LogSave(_user._idPerson, string.Format("Add comment | {0}", idmonitoring)));
            await serviceMonitoring.Update(monitoring, null);

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

            await serviceMonitoring.Update(monitoring, null);
            await Task.Run(() => LogSave(_user._idPerson, string.Format("Add comment | {0}", idmonitoring)));

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

            await serviceMonitoring.Update(monitoring, null);
            await Task.Run(() => LogSave(_user._idPerson, string.Format("Add comment | {0}", idmonitoring)));
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
    public async Task<string> UpdateComments(string idmonitoring, string iditem, ViewCrudComment comments)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();
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

                await serviceMonitoring.Update(monitoring, null);
                return "Comment altered!";
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

                await serviceMonitoring.Update(monitoring, null);
                return "Comment altered!";
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

                await serviceMonitoring.Update(monitoring, null);
                return "Comment altered!";
              }
            }
          }
        }
        return "Comment not found!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<List<ViewCrudComment>> GetListComments(string idmonitoring, string iditem)
    {
      try
      {
        var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring).Result.FirstOrDefault();
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
                UserComment = p.UserComment
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
                UserComment = p.UserComment
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

    private async Task SendQueue(string id, string idperson, List<string> countpraise)
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

        await serviceControlQueue.SendMessageAsync(JsonConvert.SerializeObject(data));


        foreach(var item in countpraise)
        {
          data = new ViewCrudMaturityRegister
          {
            _idPerson = idperson,
            TypeMaturity = EnumTypeMaturity.Praise,
            _idRegister = item,
            Date = DateTime.Now,
            _idAccount = _user._idAccount
          };
          await serviceControlQueue.SendMessageAsync(JsonConvert.SerializeObject(data));
        }


        //serviceControlQueue.RegisterOnMessageHandlerAndReceiveMesssages();
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
          Description = "Access Monitoring",
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
    private async Task<string> UpdatePlan(Plan plan)
    {
      try
      {
        await servicePlan.Update(plan, null);
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
    private Monitoring LoadMap(Monitoring monitoring)
    {
      try
      {
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == monitoring.Person.Occupation._id).Result.FirstOrDefault();

        monitoring.SkillsCompany = new List<MonitoringSkills>();
        foreach (var item in occupation.Group.Company.Skills)
        {
          monitoring.SkillsCompany.Add(new MonitoringSkills() { Skill = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<ViewCrudPlan>() });
        }

        monitoring.Activities = new List<MonitoringActivities>();
        foreach (var item in occupation.Activities)
        {
          monitoring.Activities.Add(new MonitoringActivities() { Activities = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<ViewCrudPlan>() });
        }

        monitoring.Schoolings = new List<MonitoringSchooling>();
        foreach (var item in occupation.Schooling)
        {
          monitoring.Schoolings.Add(new MonitoringSchooling() { Schooling = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString(), Plans = new List<ViewCrudPlan>() });
        }

        return monitoring;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    // send mail
    private async Task Mail(Person person)
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

        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", managername).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
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
        var mailObj = serviceMail.InsertNewVersion(sendMail).Result;
        var token = SendMail(path, person, mailObj._id.ToString());
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async Task MailManager(Person person)
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

        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", managername).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
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
        var mailObj = serviceMail.InsertNewVersion(sendMail).Result;
        var token = SendMail(path, person, mailObj._id.ToString());
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async Task MailDisApproval(Person person)
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

        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", managername).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
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


#pragma warning restore 1998
  }
}