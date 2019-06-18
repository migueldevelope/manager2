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
using System.Threading;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceOnBoarding : Repository<OnBoarding>, IServiceOnBoarding
  {
    private readonly ServiceAuthentication serviceAuthentication;

    private readonly ServiceGeneric<Company> serviceCompany;
    private readonly ServiceGeneric<Group> serviceGroup;
    private readonly ServiceLog serviceLog;
    private readonly ServiceLogMessages serviceLogMessages;
    private readonly ServiceGeneric<MailLog> serviceMailLog;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Occupation> serviceOccupation;
    private readonly ServiceGeneric<OnBoarding> serviceOnboarding;
    private readonly IServiceControlQueue serviceControlQueue;

    public string pathToken;

    #region Constructor
    public ServiceOnBoarding(DataContext context, DataContext contextLog, string _pathToken, IServiceControlQueue _serviceControlQueue) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context, contextLog);
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceGroup = new ServiceGeneric<Group>(context);
        serviceLog = new ServiceLog(context, contextLog);
        serviceLogMessages = new ServiceLogMessages(context);
        serviceMailLog = new ServiceGeneric<MailLog>(contextLog);
        serviceMailModel = new ServiceMailModel(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        serviceOnboarding = new ServiceGeneric<OnBoarding>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceControlQueue = _serviceControlQueue;


        pathToken = _pathToken;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceCompany._user = _user;
      serviceGroup._user = _user;
      serviceLog.SetUser(_user);
      serviceLogMessages.SetUser(_user);
      serviceMailLog._user = _user;
      serviceMailModel.SetUser(_user);
      serviceOccupation._user = _user;
      serviceOnboarding._user = _user;
      servicePerson._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceCompany._user = user;
      serviceGroup._user = user;
      serviceLog.SetUser(user);
      serviceLogMessages.SetUser(user);
      serviceMailLog._user = user;
      serviceMailModel.SetUser(user);
      serviceOccupation._user = user;
      serviceOnboarding._user = user;
      servicePerson._user = user;
    }
    #endregion

    #region OnBoarding

    public List<ViewListOnBoarding> List(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        //LogSave(idmanager, "List");
        List<ViewListOnBoarding> list = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled &&
                                             p.TypeUser > EnumTypeUser.Administrator &&
                                             (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation) &&
                                             p.Manager._id == idmanager && p.Occupation != null &&
                                             p.User.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "User.Name").Result
          .Select(p => new ViewListOnBoarding()
          {
            _idPerson = p._id,
            Name = p.User.Name,
            OccupationName = p.Occupation.Name,
            StatusOnBoarding = EnumStatusOnBoarding.WaitBegin
          }).ToList();
        List<ViewListOnBoarding> detail = new List<ViewListOnBoarding>();
        if (serviceOnboarding.Exists("OnBoarding"))
        {
          OnBoarding onboarding;
          foreach (var item in list)
          {
            onboarding = serviceOnboarding.GetNewVersion(x => x.Person._id == item._idPerson && x.StatusOnBoarding != EnumStatusOnBoarding.End).Result;
            if (onboarding != null)
            {
              item._id = onboarding._id;
              item.StatusOnBoarding = onboarding.StatusOnBoarding;
            }
            detail.Add(item);
          }
        }
        else
          detail = list;

        total = servicePerson.CountNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.TypeUser > EnumTypeUser.Administrator &&
          (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation) &&
          p.Manager._id == idmanager && p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  ViewListOnBoarding PersonWait(string idperson)
    {
      try
      {
        //LogSave(idperson, "ListWait");
        Person person = servicePerson.GetNewVersion(p => p.Manager != null & p.StatusUser != EnumStatusUser.Disabled &&
                                             p.TypeUser > EnumTypeUser.Administrator &&
                                             (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation) &&
                                             p._id == idperson).Result;
        if (person == null)
          return null;

        var result = new ViewListOnBoarding()
        {
          _idPerson = person._id,
          Name = person.User.Name,
          StatusOnBoarding = EnumStatusOnBoarding.WaitBegin
        };

        if (serviceOnboarding.Exists("OnBoarding"))
        {
          OnBoarding onboarding = serviceOnboarding.GetNewVersion(x => x.Person._id == result._idPerson && x.StatusOnBoarding != EnumStatusOnBoarding.End).Result;
          if (onboarding != null)
          {
            result._id = onboarding._id;
            result.StatusOnBoarding = onboarding.StatusOnBoarding;
          }
        }
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  ViewListOnBoarding New(string idperson)
    {
      try
      {

        Person person = servicePerson.GetNewVersion(p => p.StatusUser != EnumStatusUser.Disabled &&
                                             p.TypeUser > EnumTypeUser.Administrator &&
                                             (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation) &&
                                             p._id == idperson).Result;

        var manager = servicePerson.GetAllNewVersion(p => p._id == person.Manager._id).Result.FirstOrDefault();

        if (person == null)
          throw new Exception("Person not available!");

        if (person.User._id != _user._idUser && manager.User._id != _user._idUser)
          throw new Exception("Person not available!");

        OnBoarding onBoarding = serviceOnboarding.GetNewVersion(x => x.Person._id == idperson && x.StatusOnBoarding != EnumStatusOnBoarding.End).Result;
        if (onBoarding == null)
        {
          onBoarding = LoadMap(person);
          if (onBoarding.Person.User._id == _user._idUser)
          {
            onBoarding.DateBeginPerson = DateTime.Now;
            onBoarding.StatusOnBoarding = EnumStatusOnBoarding.InProgressPerson;
          }
          else
          {
            onBoarding.DateBeginManager = DateTime.Now;
            onBoarding.StatusOnBoarding = EnumStatusOnBoarding.InProgressManager;
          }
          onBoarding = serviceOnboarding.InsertNewVersion(onBoarding).Result;
           Task.Run(() => LogSave(_user._idPerson, string.Format("Start process | {0}", onBoarding._id)));
        }
        else
        {
          if (onBoarding.StatusOnBoarding == EnumStatusOnBoarding.WaitPerson)
          {
            onBoarding.DateBeginEnd = DateTime.Now;
             Task.Run(() => LogSave(_user._idPerson, string.Format("Send person approval | {0}", onBoarding._id)));
             serviceOnboarding.Update(onBoarding, null).Wait();
          }
        }
        return new ViewListOnBoarding()
        {
          Name = onBoarding.Person.User.Name,
          StatusOnBoarding = onBoarding.StatusOnBoarding,
          _id = onBoarding._id,
          _idPerson = onBoarding.Person._id
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  ViewCrudOnboarding Get(string id)
    {
      try
      {
        OnBoarding onBoarding =  serviceOnboarding.GetNewVersion(p => p._id == id).Result;
        if (onBoarding == null)
          return null;
        //throw new Exception("OnBoarding not available!");

        ViewCrudOnboarding result = new ViewCrudOnboarding()
        {
          _id = onBoarding._id,
          Person = new ViewListPersonInfo()
          {
            _id = onBoarding.Person._id,
            TypeJourney = onBoarding.Person.TypeJourney,
            Occupation = onBoarding.Person.Occupation.Name,
            Name = onBoarding.Person.User.Name,
            Manager = onBoarding.Person.Manager?.Name,
            Company = onBoarding.Person.Company.GetViewList(),
            Establishment = onBoarding.Person.Establishment?.GetViewList(),
            Registration = onBoarding.Person.Registration,
            User = onBoarding.Person.User.GetViewList(),
          },
          Occupation = onBoarding.Person.Occupation.GetViewList(),
          CommentsPerson = onBoarding.CommentsPerson,
          CommentsManager = onBoarding.CommentsManager,
          CommentsEnd = onBoarding.CommentsEnd,
          StatusOnBoarding = onBoarding.StatusOnBoarding,
          SkillsCompany = onBoarding.SkillsCompany?.OrderBy(o => o.Skill.Name).Select(p => new ViewCrudOnboardingSkill()
          {
            _id = p._id,
            Skill = new ViewListSkill() { _id = p.Skill._id, Name = p.Skill.Name, TypeSkill = p.Skill.TypeSkill, Concept = p.Skill.Concept },
            CommentsManager = p.CommentsManager,
            CommentsPerson = p.CommentsPerson,
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Comments = p.Comments?.Select(c => new ViewCrudComment()
            {
              _id = c._id,
              Date = c.Date,
              StatusView = c.StatusView,
              UserComment = c.UserComment,
              Comments = c.Comments
            }).ToList()
          }).ToList(),
          SkillsGroup = onBoarding.SkillsGroup?.OrderBy(o => o.Skill.Name).Select(p => new ViewCrudOnboardingSkill()
          {
            _id = p._id,
            Skill = new ViewListSkill() { _id = p.Skill._id, Name = p.Skill.Name, TypeSkill = p.Skill.TypeSkill, Concept = p.Skill.Concept },
            CommentsManager = p.CommentsManager,
            CommentsPerson = p.CommentsPerson,
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Comments = p.Comments?.Select(c => new ViewCrudComment()
            {
              _id = c._id,
              Date = c.Date,
              StatusView = c.StatusView,
              UserComment = c.UserComment,
              Comments = c.Comments
            }).ToList()
          }).ToList(),
          SkillsOccupation = onBoarding.SkillsOccupation?.OrderBy(o => o.Skill.Name).Select(p => new ViewCrudOnboardingSkill()
          {
            _id = p._id,
            Skill = new ViewListSkill() { _id = p.Skill._id, Name = p.Skill.Name, TypeSkill = p.Skill.TypeSkill, Concept = p.Skill.Concept },
            CommentsManager = p.CommentsManager,
            CommentsPerson = p.CommentsPerson,
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Comments = p.Comments?.Select(c => new ViewCrudComment()
            {
              _id = c._id,
              Date = c.Date,
              StatusView = c.StatusView,
              UserComment = c.UserComment,
              Comments = c.Comments
            }).ToList()
          }).ToList(),
          Scopes = onBoarding.Scopes?.OrderBy(o => o.Scope.Order).Select(p => new ViewCrudOnboardingScope()
          {
            _id = p._id,
            Scope = new ViewListScope() { _id = p.Scope._id, Name = p.Scope.Name, Order = p.Scope.Order },
            CommentsManager = p.CommentsManager,
            CommentsPerson = p.CommentsPerson,
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Comments = p.Comments?.Select(c => new ViewCrudComment()
            {
              _id = c._id,
              Date = c.Date,
              StatusView = c.StatusView,
              UserComment = c.UserComment,
              Comments = c.Comments
            }).ToList()
          }).ToList(),
          Schoolings = onBoarding.Schoolings?.OrderBy(o => o.Schooling.Order).Select(p => new ViewCrudOnboardingSchooling()
          {
            _id = p._id,
            Schooling = new ViewCrudSchooling()
            {
              _id = p.Schooling._id,
              Name = p.Schooling.Name,
              Order = p.Schooling.Order,
              Complement = p.Schooling.Complement,
              Type = p.Schooling.Type
            },
            CommentsManager = p.CommentsManager,
            CommentsPerson = p.CommentsPerson,
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Comments = p.Comments?.Select(c => new ViewCrudComment()
            {
              _id = c._id,
              Date = c.Date,
              StatusView = c.StatusView,
              UserComment = c.UserComment,
              Comments = c.Comments
            }).ToList()
          }).ToList(),
          Activities = onBoarding.Activities?.OrderBy(o => o.Activitie.Order).Select(p => new ViewCrudOnboardingActivitie()
          {
            _id = p._id,
            Activitie = p.Activitie.GetViewList(),
            CommentsManager = p.CommentsManager,
            CommentsPerson = p.CommentsPerson,
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Comments = p.Comments?.Select(c => new ViewCrudComment()
            {
              _id = c._id,
              Date = c.Date,
              StatusView = c.StatusView,
              UserComment = c.UserComment,
              Comments = c.Comments
            }).ToList()
          }).ToList()
        };
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  string UpdateCommentsView(string idonboarding, string iditem, EnumUserComment userComment)
    {
      try
      {
        var onboarding = serviceOnboarding.GetAllNewVersion(p => p._id == idonboarding).Result.FirstOrDefault();
        foreach (var item in onboarding.Activities)
        {
          if (item._id == iditem)
          {
            if (userComment == EnumUserComment.Manager)
              item.StatusViewManager = EnumStatusView.View;
            else
              item.StatusViewPerson = EnumStatusView.View;

             serviceOnboarding.Update(onboarding, null).Wait();
            return "ok";
          }
        }
        foreach (var item in onboarding.Schoolings)
        {
          if (item._id == iditem)
          {
            if (userComment == EnumUserComment.Manager)
              item.StatusViewManager = EnumStatusView.View;
            else
              item.StatusViewPerson = EnumStatusView.View;

             serviceOnboarding.Update(onboarding, null).Wait();
            return "ok";
          }
        }

        foreach (var item in onboarding.Scopes)
        {
          if (item._id == iditem)
          {
            if (userComment == EnumUserComment.Manager)
              item.StatusViewManager = EnumStatusView.View;
            else
              item.StatusViewPerson = EnumStatusView.View;

             serviceOnboarding.Update(onboarding, null).Wait();
            return "ok";
          }
        }

        foreach (var item in onboarding.SkillsCompany)
        {
          if (item._id == iditem)
          {
            if (userComment == EnumUserComment.Manager)
              item.StatusViewManager = EnumStatusView.View;
            else
              item.StatusViewPerson = EnumStatusView.View;

             serviceOnboarding.Update(onboarding, null).Wait();
            return "ok";
          }
        }

        foreach (var item in onboarding.SkillsGroup)
        {
          if (item._id == iditem)
          {
            if (userComment == EnumUserComment.Manager)
              item.StatusViewManager = EnumStatusView.View;
            else
              item.StatusViewPerson = EnumStatusView.View;

             serviceOnboarding.Update(onboarding, null).Wait();
            return "ok";
          }
        }

        foreach (var item in onboarding.SkillsOccupation)
        {
          if (item._id == iditem)
          {
            if (userComment == EnumUserComment.Manager)
              item.StatusViewManager = EnumStatusView.View;
            else
              item.StatusViewPerson = EnumStatusView.View;

             serviceOnboarding.Update(onboarding, null).Wait();
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
    public  string DeleteComments(string idonboarding, string iditem, string idcomments)
    {
      try
      {

        var onboarding = serviceOnboarding.GetAllNewVersion(p => p._id == idonboarding).Result.FirstOrDefault();
         Task.Run(() => LogSave(_user._idPerson, string.Format("Delete comment | {0} item {1} comment {2}", idonboarding, iditem, idcomments)));
        foreach (var item in onboarding.Activities)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                 serviceOnboarding.Update(onboarding, null).Wait();
                return "Comment deleted!";
              }
            }
          }
        }

        foreach (var item in onboarding.Schoolings)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                 serviceOnboarding.Update(onboarding, null).Wait();
                return "Comment deleted!";
              }
            }
          }
        }

        foreach (var item in onboarding.Scopes)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                 serviceOnboarding.Update(onboarding, null).Wait();
                return "Comment deleted!";
              }
            }
          }
        }

        foreach (var item in onboarding.SkillsCompany)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                 serviceOnboarding.Update(onboarding, null).Wait();
                return "Comment deleted!";
              }
            }
          }
        }

        foreach (var item in onboarding.SkillsGroup)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                 serviceOnboarding.Update(onboarding, null).Wait();
                return "Comment deleted!";
              }
            }
          }
        }

        foreach (var item in onboarding.SkillsOccupation)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                 serviceOnboarding.Update(onboarding, null).Wait();
                return "Comment deleted!";
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
    public  string Delete(string id)
    {
      try
      {

        var onboarding = serviceOnboarding.GetNewVersion(p => p._id == id).Result;
         Task.Run(() => LogSave(_user._idPerson, string.Format("Delete | {0}", id)));
        onboarding.Status = EnumStatus.Disabled;
         serviceOnboarding.Update(onboarding, null).Wait();
        return "OnBoarding deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListOnBoarding> ListEnded(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, "List ended"));
        int skip = (count * (page - 1));
        var detail = serviceOnboarding.GetAllNewVersion(p => p.Person.Manager._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceOnboarding.CountNewVersion(p => p.Person.Manager._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewListOnBoarding()
        {
          _id = p._id,
          Name = p.Person.User.Name,
          _idPerson = p.Person._id,
          StatusOnBoarding = p.StatusOnBoarding,
          OccupationName = p.Person.Occupation.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  List<ViewCrudComment> AddComments(string idonboarding, string iditem, ViewCrudComment comments)
    {
      try
      {

        var onboarding = serviceOnboarding.GetAllNewVersion(p => p._id == idonboarding).Result.FirstOrDefault();
         Task.Run(() => LogSave(_user._idPerson, string.Format("Add comment | {0} item {1}", idonboarding, iditem)));
        foreach (var item in onboarding.Activities)
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
             serviceOnboarding.Update(onboarding, null).Wait();
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
        foreach (var item in onboarding.Schoolings)
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

             serviceOnboarding.Update(onboarding, null).Wait();

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

        foreach (var item in onboarding.Scopes)
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

             serviceOnboarding.Update(onboarding, null).Wait();

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

        foreach (var item in onboarding.SkillsCompany)
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

             serviceOnboarding.Update(onboarding, null).Wait();

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

        foreach (var item in onboarding.SkillsGroup)
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

             serviceOnboarding.Update(onboarding, null).Wait();

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

        foreach (var item in onboarding.SkillsOccupation)
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

             serviceOnboarding.Update(onboarding, null).Wait();

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
    public  string UpdateComments(string idonboarding, string iditem, ViewCrudComment comments)
    {
      try
      {
        var onboarding = serviceOnboarding.GetAllNewVersion(p => p._id == idonboarding).Result.FirstOrDefault();
        foreach (var item in onboarding.Activities)
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

                 serviceOnboarding.Update(onboarding, null).Wait();
                return "ok";
              }
            }
          }
        }


        foreach (var item in onboarding.Schoolings)
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

                 serviceOnboarding.Update(onboarding, null).Wait();
                return "ok";
              }
            }
          }
        }

        foreach (var item in onboarding.Scopes)
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

                 serviceOnboarding.Update(onboarding, null).Wait();
                return "ok";
              }
            }
          }
        }

        foreach (var item in onboarding.SkillsCompany)
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

                 serviceOnboarding.Update(onboarding, null).Wait();
                return "ok";
              }
            }
          }
        }

        foreach (var item in onboarding.SkillsGroup)
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

                 serviceOnboarding.Update(onboarding, null).Wait();
                return "ok";
              }
            }
          }
        }

        foreach (var item in onboarding.SkillsOccupation)
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

                 serviceOnboarding.Update(onboarding, null).Wait();
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
    public  List<ViewCrudComment> ListComments(string idonboarding, string iditem)
    {
      try
      {
        var onboarding = serviceOnboarding.GetAllNewVersion(p => p._id == idonboarding).Result.FirstOrDefault();
        foreach (var item in onboarding.Activities)
        {
          if (item._id == iditem)
          {
            return item.Comments?.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment
            }).ToList();
          }
        }

        foreach (var item in onboarding.Scopes)
        {
          if (item._id == iditem)
          {
            return item.Comments?.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment
            }).ToList();
          }
        }

        foreach (var item in onboarding.Schoolings)
        {
          if (item._id == iditem)
          {
            return item.Comments?.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment
            }).ToList();
          }
        }

        foreach (var item in onboarding.SkillsCompany)
        {
          if (item._id == iditem)
          {
            return item.Comments?.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment
            }).ToList();
          }
        }

        foreach (var item in onboarding.SkillsGroup)
        {
          if (item._id == iditem)
          {
            return item.Comments?.Select(p => new ViewCrudComment()
            {
              _id = p._id,
              Comments = p.Comments,
              Date = p.Date,
              StatusView = p.StatusView,
              UserComment = p.UserComment
            }).ToList();
          }
        }

        foreach (var item in onboarding.SkillsOccupation)
        {
          if (item._id == iditem)
          {
            return item.Comments?.Select(p => new ViewCrudComment()
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
    public List<ViewListOnBoarding> ListPersonEnd(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        Task.Run(() => LogSave(_user._idPerson, "List ended for person"));
        int skip = (count * (page - 1));
        var detail = serviceOnboarding.GetAllNewVersion(p => p.Person._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceOnboarding.CountNewVersion(p => p.Person._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewListOnBoarding()
        {
          _id = p._id,
          Name = p.Person.User.Name,
          _idPerson = p.Person._id,
          StatusOnBoarding = p.StatusOnBoarding,
          OccupationName = p.Person.Occupation.Name,
          DateEndEnd = p.DateEndEnd
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  ViewCrudOnboarding GetOnBoardings(string id)
    {
      try
      {
        var onboarding = serviceOnboarding.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();

        var view = new ViewCrudOnboarding()
        {
          _id = onboarding._id,
          Person = new ViewListPersonInfo()
          {
            _id = onboarding.Person._id,
            TypeJourney = onboarding.Person.TypeJourney,
            Occupation = onboarding.Person.Occupation.Name,
            Name = onboarding.Person.User.Name,
            Manager = onboarding.Person.Manager.Name,
            Registration = onboarding.Person.Registration,
            User = onboarding.Person.User.GetViewList(),
            Company = onboarding.Person.Company.GetViewList(),
            Establishment = onboarding.Person.Establishment?.GetViewList()
          },
          Occupation = onboarding.Person.Occupation.GetViewList(),
          CommentsPerson = onboarding.CommentsPerson,
          CommentsManager = onboarding.CommentsManager,
          CommentsEnd = onboarding.CommentsEnd,
          SkillsCompany = onboarding.SkillsCompany.Select(p => new ViewCrudOnboardingSkill()
          {
            _id = p._id,
            Comments = p.Comments.Select(x => new ViewCrudComment()
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
            }
          }).ToList(),
          SkillsGroup = onboarding.SkillsGroup.Select(p => new ViewCrudOnboardingSkill()
          {
            _id = p._id,
            Comments = p.Comments.Select(x => new ViewCrudComment()
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
            }
          }).ToList(),
          SkillsOccupation = onboarding.SkillsOccupation.Select(p => new ViewCrudOnboardingSkill()
          {
            _id = p._id,
            Comments = p.Comments.Select(x => new ViewCrudComment()
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
            }
          }).ToList(),
          Scopes = onboarding.Scopes.Select(p => new ViewCrudOnboardingScope()
          {
            _id = p._id,
            Comments = p.Comments.Select(x => new ViewCrudComment()
            {
              _id = x._id,
              Comments = x.Comments,
              Date = x.Date,
              StatusView = x.StatusView,
              UserComment = x.UserComment
            }).ToList(),
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Scope = new ViewListScope()
            {
              _id = p.Scope._id,
              Name = p.Scope.Name,
              Order = p.Scope.Order
            }
          }).ToList(),
          Schoolings = onboarding.Schoolings.Select(p => new ViewCrudOnboardingSchooling()
          {
            _id = p._id,
            Comments = p.Comments.Select(x => new ViewCrudComment()
            {
              _id = x._id,
              Comments = x.Comments,
              Date = x.Date,
              StatusView = x.StatusView,
              UserComment = x.UserComment
            }).ToList(),
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Schooling = new ViewCrudSchooling()
            {
              _id = p.Schooling._id,
              Name = p.Schooling.Name,
              Order = p.Schooling.Order,
              Complement = p.Schooling._id,
              Type = p.Schooling.Type
            }
          }).ToList(),
          Activities = onboarding.Activities.Select(p => new ViewCrudOnboardingActivitie()
          {
            _id = p._id,
            Comments = p.Comments.Select(x => new ViewCrudComment()
            {
              _id = x._id,
              Comments = x.Comments,
              Date = x.Date,
              StatusView = x.StatusView,
              UserComment = x.UserComment
            }).ToList(),
            StatusViewManager = p.StatusViewManager,
            StatusViewPerson = p.StatusViewPerson,
            Activitie = p.Activitie.GetViewList()
          }).ToList(),
          StatusOnBoarding = onboarding.StatusOnBoarding,
        };

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  string Update(ViewCrudOnboarding view)
    {
      try
      {
        var onboarding = serviceOnboarding.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
        onboarding.StatusOnBoarding = view.StatusOnBoarding;
        onboarding.CommentsEnd = view.CommentsEnd;
        onboarding.CommentsPerson = view.CommentsPerson;
        onboarding.CommentsManager = view.CommentsManager;

        if (onboarding.Person.User._id != _user._idUser)
        {
          if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.WaitPerson)
          {
            onboarding.DateEndManager = DateTime.Now;
            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
               Task.Run(() => MailOccupation(onboarding.Person));
            else
               Task.Run(() => Mail(onboarding.Person));
             Task.Run(() => LogSave(_user._idPerson, string.Format("Send person approval | {0}", onboarding._id)));
          }

          if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.End)
          {
            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
               Task.Run(()=>serviceLogMessages.NewLogMessage("OnBoarding", string.Format("Embarque | OnBoarding realizado para {0}.", onboarding.Person.User.Name), onboarding.Person));
            else
               Task.Run(()=>serviceLogMessages.NewLogMessage("OnBoarding", string.Format("Embarque | OnBoarding de troca de cargo realizado para {0}.", onboarding.Person.User.Name), onboarding.Person));

            onboarding.DateEndEnd = DateTime.Now;

            Person person = servicePerson.GetNewVersion(p => p._id == onboarding.Person._id).Result;
            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
              person.TypeJourney = EnumTypeJourney.Monitoring;
            else
              person.TypeJourney = EnumTypeJourney.Checkpoint;
             servicePerson.Update(person, null).Wait();

             Task.Run(() => LogSave(_user._idPerson, string.Format("Conclusion process | {0}", onboarding._id)));
          }
        }
        else
        {
          if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.End)
          {
            if (ValidOnboardingComments(onboarding))
            {
              onboarding.StatusOnBoarding = EnumStatusOnBoarding.WaitManagerRevision;
              onboarding.DateEndPerson = DateTime.Now;
              if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
                 Task.Run(() => MailDisapprovedOccupation(onboarding.Person));
              else
                 Task.Run(() => MailDisapproved(onboarding.Person));
               Task.Run(() => LogSave(_user._idPerson, string.Format("Send manager review | {0}", onboarding._id)));
            }
            else
            {
              if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
                 Task.Run(()=>serviceLogMessages.NewLogMessage("OnBoarding", string.Format("Embarque | OnBoarding realizado para {0}.", onboarding.Person.User.Name), onboarding.Person));
              else
                 Task.Run(()=>serviceLogMessages.NewLogMessage("OnBoarding", string.Format("Embarque | OnBoarding de troca de cargo realizado para {0}.", onboarding.Person.User.Name), onboarding.Person));
              onboarding.DateEndEnd = DateTime.Now;
              Person person = servicePerson.GetNewVersion(p => p._id == onboarding.Person._id).Result;
              if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
                person.TypeJourney = EnumTypeJourney.Monitoring;
              else
                person.TypeJourney = EnumTypeJourney.Checkpoint;
               Task.Run(() => LogSave(_user._idPerson, string.Format("Conclusion process | {0}", onboarding._id)));
               servicePerson.Update(person, null).Wait();
            }


          }
          else if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.WaitManager)
          {
            onboarding.DateEndPerson = DateTime.Now;
            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
               Task.Run(() => MailManagerOccupation(onboarding.Person));
            else
               Task.Run(() => MailManager(onboarding.Person));
             Task.Run(() => LogSave(_user._idPerson, string.Format("Send manager approval | {0}", onboarding._id)));
          }
          else if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.WaitManagerRevision)
          {
            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
               Task.Run(() => MailDisapprovedOccupation(onboarding.Person));
            else
               Task.Run(() => MailDisapproved(onboarding.Person));
             Task.Run(() => LogSave(_user._idPerson, string.Format("Send manager review | {0}", onboarding._id)));
          }
        }
         serviceOnboarding.Update(onboarding, null).Wait();
        return "OnBoarding altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListOnBoarding> ListExcluded(ref long total, string filter, int count, int page)
    {
      try
      {


        int skip = (count * (page - 1));
        var detail = serviceOnboarding.GetAllNewVersion(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceOnboarding.CountNewVersion(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        // Task.Run(() => LogSaves(detail.Person._id, "OnBoarding list for exclud"));

        return detail.Select(p => new ViewListOnBoarding()
        {
          _id = p._id,
          Name = p.Person.User.Name,
          _idPerson = p.Person._id,
          StatusOnBoarding = p.StatusOnBoarding,
          OccupationName = p.Person.Occupation?.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListOnBoarding> ListOnBoardingsWait(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var list = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator & (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation) & p.Manager._id == idmanager
        & p.User.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.User.Name)
        .ToList().Select(p => new { Person = p, OnBoarding = serviceOnboarding.GetAllNewVersion(x => x.Person._id == p._id & x.StatusOnBoarding != EnumStatusOnBoarding.End).Result.FirstOrDefault() })
        .ToList();

        var detail = new List<OnBoarding>();
        foreach (var item in list)
        {


          if ((item.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation) || (item.Person.TypeJourney == EnumTypeJourney.OnBoarding))
          {
            if (item.OnBoarding == null)
              detail.Add(new OnBoarding
              {
                Person = item.Person,
                _id = null,
                StatusOnBoarding = EnumStatusOnBoarding.WaitBegin
              });
            else
              detail.Add(item.OnBoarding);
          }
        }

        total = detail.Count();
        return detail.Skip(skip).Take(count)
          .Select(p => new ViewListOnBoarding()
          {
            _id = p._id,
            Name = p.Person.User.Name,
            _idPerson = p.Person._id,
            StatusOnBoarding = p.StatusOnBoarding,
            OccupationName = p.Person.Occupation.Name
          }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region Private
    private OnBoarding LoadMap(Person person)
    {
      try
      {
        OnBoarding onBoarding = new OnBoarding
        {
          Person = person
        };
        Company company = serviceCompany.GetNewVersion(p => p._id == person.Company._id).Result;
        onBoarding.SkillsCompany = new List<OnBoardingSkills>();
        foreach (Skill item in company.Skills)
          onBoarding.SkillsCompany.Add(new OnBoardingSkills()
          {
            Skill = item,
            _idAccount = item._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          });

        Group group = serviceGroup.GetNewVersion(p => p._id == person.Occupation.Group._id).Result;
        onBoarding.SkillsGroup = new List<OnBoardingSkills>();
        foreach (Skill item in group.Skills)
          onBoarding.SkillsGroup.Add(new OnBoardingSkills()
          {
            Skill = item,
            _idAccount = item._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          });

        onBoarding.Scopes = new List<OnBoardingScope>();
        foreach (Scope item in group.Scope)
          onBoarding.Scopes.Add(new OnBoardingScope()
          {
            Scope = item,
            _idAccount = item._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          });

        Occupation occupation = serviceOccupation.GetNewVersion(p => p._id == person.Occupation._id).Result;
        onBoarding.SkillsOccupation = new List<OnBoardingSkills>();
        foreach (Skill item in occupation.Skills)
          onBoarding.SkillsOccupation.Add(new OnBoardingSkills()
          {
            Skill = item,
            _idAccount = item._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          });

        onBoarding.Activities = new List<OnBoardingActivities>();
        foreach (Activitie item in occupation.Activities)
          onBoarding.Activities.Add(new OnBoardingActivities()
          {
            Activitie = item,
            _idAccount = item._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          });

        onBoarding.Schoolings = new List<OnBoardingSchooling>();
        foreach (Schooling item in occupation.Schooling)
          onBoarding.Schoolings.Add(new OnBoardingSchooling()
          {
            Schooling = item,
            _idAccount = item._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          });

        return onBoarding;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private OnBoarding LoadMapOld(OnBoarding onBoarding)
    {
      try
      {
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == onBoarding.Person.Occupation._id).Result.FirstOrDefault();

        onBoarding.SkillsCompany = new List<OnBoardingSkills>();
        foreach (var item in occupation.Group.Company.Skills)
        {
          onBoarding.SkillsCompany.Add(new OnBoardingSkills() { Skill = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString() });
        }

        onBoarding.SkillsGroup = new List<OnBoardingSkills>();
        foreach (var item in occupation.Group.Skills)
        {
          onBoarding.SkillsGroup.Add(new OnBoardingSkills() { Skill = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString() });
        }

        onBoarding.SkillsOccupation = new List<OnBoardingSkills>();
        foreach (var item in occupation.Skills)
        {
          onBoarding.SkillsOccupation.Add(new OnBoardingSkills() { Skill = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString() });
        }

        onBoarding.Scopes = new List<OnBoardingScope>();
        foreach (var item in occupation.Group.Scope)
        {
          onBoarding.Scopes.Add(new OnBoardingScope() { Scope = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString() });
        }

        onBoarding.Activities = new List<OnBoardingActivities>();
        foreach (var item in occupation.Activities)
        {
          onBoarding.Activities.Add(new OnBoardingActivities() { Activitie = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString() });
        }

        onBoarding.Schoolings = new List<OnBoardingSchooling>();
        foreach (var item in occupation.Schooling)
        {
          onBoarding.Schoolings.Add(new OnBoardingSchooling() { Schooling = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString() });
        }

        return onBoarding;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void LogSave(string idperson, string local)
    {
      try
      {
        var user = servicePerson.GetAllNewVersion(p => p._id == idperson).Result.FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "OnBoarding",
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
        var model = serviceMailModel.OnBoardingApproval(pathToken);
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
        var mailObj =  serviceMailLog.InsertNewVersion(sendMail).Result;
        SendMail(pathToken, person, mailObj._id.ToString());
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
        var model = serviceMailModel.OnBoardingApprovalManager(pathToken);
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
        var mailObj = serviceMailLog.InsertNewVersion(sendMail).Result;
        var token = SendMail(pathToken, person, mailObj._id.ToString());
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void MailOccupation(Person person)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.OnBoardingOccupationApproval(pathToken);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
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
        var mailObj =  serviceMailLog.InsertNewVersion(sendMail).Result;
        var token = SendMail(pathToken, person, mailObj._id.ToString());
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void MailManagerOccupation(Person person)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.OnBoardingOccupationApprovalManager(pathToken);
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
        var mailObj =  serviceMailLog.InsertNewVersion(sendMail).Result;
        var token = SendMail(pathToken, person, mailObj._id.ToString());
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void MailDisapproved(Person person)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.OnBoardingDisapproval(pathToken);
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
        var mailObj =  serviceMailLog.InsertNewVersion(sendMail).Result;
        var token = SendMail(pathToken, person, mailObj._id.ToString());
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void MailDisapprovedOccupation(Person person)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.OnBoardingOccupationDisapproval(pathToken);
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
        var mailObj = serviceMailLog.InsertNewVersion(sendMail).Result;
        var token = SendMail(pathToken, person, mailObj._id.ToString());
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
      catch (Exception)
      {
        return "none";
      }
    }
    private bool ValidOnboardingComments(OnBoarding onBoarding)
    {
      try
      {
        var count = onBoarding.Activities.Where(p => p.StatusViewManager == EnumStatusView.None & p.Comments != null).Count()
          + onBoarding.Schoolings.Where(p => p.StatusViewManager == EnumStatusView.None & p.Comments != null).Count()
          + onBoarding.Scopes.Where(p => p.StatusViewManager == EnumStatusView.None & p.Comments != null).Count()
          + onBoarding.SkillsCompany.Where(p => p.StatusViewManager == EnumStatusView.None & p.Comments != null).Count()
          + onBoarding.SkillsGroup.Where(p => p.StatusViewManager == EnumStatusView.None & p.Comments != null).Count()
          + onBoarding.SkillsOccupation.Where(p => p.StatusViewManager == EnumStatusView.None & p.Comments != null).Count();

        if (count > 0)
          return true;

        return false;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    #endregion

  }
}
