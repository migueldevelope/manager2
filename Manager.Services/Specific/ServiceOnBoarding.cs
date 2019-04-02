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
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

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
    private readonly ServiceGeneric<MailMessage> serviceMailMessage;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Occupation> serviceOccupation;
    private readonly ServiceGeneric<OnBoarding> serviceOnboarding;
    public string pathToken;

    #region Constructor
    public ServiceOnBoarding(DataContext context, string _pathToken) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context);
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceGroup = new ServiceGeneric<Group>(context);
        serviceLog = new ServiceLog(_context);
        serviceLogMessages = new ServiceLogMessages(context);
        serviceMailLog = new ServiceGeneric<MailLog>(context);
        serviceMailMessage = new ServiceGeneric<MailMessage>(context);
        serviceMailModel = new ServiceMailModel(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        serviceOnboarding = new ServiceGeneric<OnBoarding>(context);
        servicePerson = new ServiceGeneric<Person>(context);
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
      serviceMailMessage._user = _user;
      serviceMailModel.SetUser(_user);
      serviceOccupation._user = _user;
      serviceOnboarding._user = _user;
      servicePerson._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      serviceCompany._user = user;
      serviceGroup._user = user;
      serviceLog.SetUser(user);
      serviceLogMessages.SetUser(user);
      serviceMailLog._user = user;
      serviceMailMessage._user = user;
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
    public ViewListOnBoarding PersonWait(string idperson)
    {
      try
      {
        //LogSave(idperson, "ListWait");
        Person person = servicePerson.GetNewVersion(p => p.StatusUser != EnumStatusUser.Disabled &&
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
    public ViewListOnBoarding New(string idperson)
    {
      try
      {
        Person person = servicePerson.GetNewVersion(p => p.StatusUser != EnumStatusUser.Disabled &&
                                             p.TypeUser > EnumTypeUser.Administrator &&
                                             (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation) &&
                                             p._id == idperson).Result;
        if (person == null)
          throw new Exception("Person not available!");

        if (person._id != _user._idPerson && person.Manager._id != _user._idPerson)
          throw new Exception("Person not available!");

        OnBoarding onBoarding = serviceOnboarding.GetNewVersion(x => x.Person._id == idperson && x.StatusOnBoarding != EnumStatusOnBoarding.End).Result;
        if (onBoarding == null)
        {
          onBoarding = LoadMap(person);
          if (onBoarding.Person._id == _user._idPerson)
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
          LogSave(_user._idPerson, string.Format("Start process | {0}", onBoarding._id));
        }
        else
        {
          if (onBoarding.StatusOnBoarding == EnumStatusOnBoarding.WaitPerson)
          {
            onBoarding.DateBeginEnd = DateTime.Now;
            LogSave(_user._idPerson, string.Format("Send person approval | {0}", onBoarding._id));
            serviceOnboarding.Update(onBoarding, null);
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
    public ViewCrudOnboarding Get(string id)
    {
      try
      {
        OnBoarding onBoarding = serviceOnboarding.GetNewVersion(p => p._id == id).Result;
        if (onBoarding == null)
          return null;
        //throw new Exception("OnBoarding not available!");

        ViewCrudOnboarding result = new ViewCrudOnboarding()
        {
          _id = onBoarding._id,
          Person = new ViewInfoPerson()
          {
            _id = onBoarding.Person._id,
            TypeJourney = onBoarding.Person.TypeJourney,
            Occupation = onBoarding.Person.Occupation.Name,
            Name = onBoarding.Person.User.Name,
            Manager = onBoarding.Person.Manager?.Name,
            Company = new ViewListCompany() { _id = onBoarding.Person.Company._id, Name = onBoarding.Person.Company.Name },
            Establishment = (onBoarding.Person.Establishment == null) ? null : new ViewListEstablishment() { _id = onBoarding.Person.Establishment._id, Name = onBoarding.Person.Establishment.Name },
            Registration = onBoarding.Person.Registration,
            User = new ViewListUser()
            {
              Document = onBoarding.Person.User.Document,
              Mail = onBoarding.Person.User.Mail,
              Name = onBoarding.Person.User.Name,
              Phone = onBoarding.Person.User.Phone,
              _id = onBoarding.Person.User._id
            }
          },
          Occupation = new ViewListOccupation()
          {
            _id = onBoarding.Person.Occupation._id,
            Name = onBoarding.Person.Occupation.Name,
            Line = onBoarding.Person.Occupation.Line,
            Company = new ViewListCompany() { _id = onBoarding.Person.Occupation.Group.Company._id, Name = onBoarding.Person.Occupation.Group.Company.Name },
            Group = new ViewListGroup()
            {
              _id = onBoarding.Person.Occupation.Group._id,
              Name = onBoarding.Person.Occupation.Group.Name,
              Line = onBoarding.Person.Occupation.Group.Line,
              Axis = new ViewListAxis()
              {
                _id = onBoarding.Person.Occupation.Group.Axis._id,
                Name = onBoarding.Person.Occupation.Group.Axis.Name,
                TypeAxis = onBoarding.Person.Occupation.Group.Axis.TypeAxis
              },
              Sphere = new ViewListSphere()
              {
                _id = onBoarding.Person.Occupation.Group.Sphere._id,
                Name = onBoarding.Person.Occupation.Group.Sphere.Name,
                TypeSphere = onBoarding.Person.Occupation.Group.Sphere.TypeSphere
              }
            },
            Process = onBoarding.Person.Occupation.Process.Select(p => new ViewListProcessLevelTwo()
            {
              _id = p._id,
              Name = p.Name,
              Order = p.Order,
              ProcessLevelOne = new ViewListProcessLevelOne()
              {
                _id = p.ProcessLevelOne._id,
                Name = p.ProcessLevelOne.Name,
                Order = p.ProcessLevelOne.Order,
                Area = new ViewListArea()
                {
                  _id = p.ProcessLevelOne.Area._id,
                  Name = p.ProcessLevelOne.Area.Name
                }
              }
            }).ToList()
          },
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
            Activitie = new ViewListActivitie() { _id = p.Activitie._id, Name = p.Activitie.Name, Order = p.Activitie.Order },
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
    public string UpdateCommentsView(string idonboarding, string iditem, EnumUserComment userComment)
    {
      try
      {
        var onboarding = serviceOnboarding.GetAll(p => p._id == idonboarding).FirstOrDefault();
        foreach (var item in onboarding.Activities)
        {
          if (item._id == iditem)
          {
            if (userComment == EnumUserComment.Manager)
              item.StatusViewManager = EnumStatusView.View;
            else
              item.StatusViewPerson = EnumStatusView.View;

            serviceOnboarding.Update(onboarding, null);
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

            serviceOnboarding.Update(onboarding, null);
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

            serviceOnboarding.Update(onboarding, null);
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

            serviceOnboarding.Update(onboarding, null);
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

            serviceOnboarding.Update(onboarding, null);
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

            serviceOnboarding.Update(onboarding, null);
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
    public string DeleteComments(string idonboarding, string iditem, string idcomments)
    {
      try
      {
        LogSave(_user._idPerson, string.Format("Delete comment | {0} item {1} comment {2}", idonboarding, iditem, idcomments));
        var onboarding = serviceOnboarding.GetAll(p => p._id == idonboarding).FirstOrDefault();
        foreach (var item in onboarding.Activities)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                serviceOnboarding.Update(onboarding, null);
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
                serviceOnboarding.Update(onboarding, null);
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
                serviceOnboarding.Update(onboarding, null);
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
                serviceOnboarding.Update(onboarding, null);
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
                serviceOnboarding.Update(onboarding, null);
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
                serviceOnboarding.Update(onboarding, null);
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
    public string Delete(string id)
    {
      try
      {
        LogSave(_user._idPerson, string.Format("Delete | {0}", id));
        var onboarding = serviceOnboarding.GetNewVersion(p => p._id == id).Result;
        onboarding.Status = EnumStatus.Disabled;
        serviceOnboarding.Update(onboarding, null);
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
        LogSave(idmanager, "List ended");
        int skip = (count * (page - 1));
        var detail = serviceOnboarding.GetAll(p => p.Person.Manager._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceOnboarding.GetAll(p => p.Person.Manager._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

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
    public List<ViewCrudComment> AddComments(string idonboarding, string iditem, ViewCrudComment comments)
    {
      try
      {
        LogSave(_user._idPerson, string.Format("Add comment | {0} item {1}", idonboarding, iditem));
        var onboarding = serviceOnboarding.GetAll(p => p._id == idonboarding).FirstOrDefault();
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
            serviceOnboarding.Update(onboarding, null);
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

            serviceOnboarding.Update(onboarding, null);

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

            serviceOnboarding.Update(onboarding, null);

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

            serviceOnboarding.Update(onboarding, null);

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

            serviceOnboarding.Update(onboarding, null);

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

            serviceOnboarding.Update(onboarding, null);

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
    public string UpdateComments(string idonboarding, string iditem, ViewCrudComment comments)
    {
      try
      {
        var onboarding = serviceOnboarding.GetAll(p => p._id == idonboarding).FirstOrDefault();
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

                serviceOnboarding.Update(onboarding, null);
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

                serviceOnboarding.Update(onboarding, null);
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

                serviceOnboarding.Update(onboarding, null);
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

                serviceOnboarding.Update(onboarding, null);
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

                serviceOnboarding.Update(onboarding, null);
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

                serviceOnboarding.Update(onboarding, null);
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
    public List<ViewCrudComment> ListComments(string idonboarding, string iditem)
    {
      try
      {
        var onboarding = serviceOnboarding.GetAll(p => p._id == idonboarding).FirstOrDefault();
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
        LogSave(idmanager, "List ended for person");
        int skip = (count * (page - 1));
        var detail = serviceOnboarding.GetAll(p => p.Person._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceOnboarding.GetAll(p => p.Person._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.Select(p => new ViewListOnBoarding()
        {
          _id = p._id,
          Name = p.Person.User.Name,
          _idPerson = p.Person._id,
          StatusOnBoarding = p.StatusOnBoarding,
          OccupationName = p.Person.Occupation.Name,
          DateEndEnd = p.DateEndEnd
        }).ToList(); ;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudOnboarding GetOnBoardings(string id)
    {
      try
      {
        var onboarding = serviceOnboarding.GetAll(p => p._id == id).FirstOrDefault();

        var view = new ViewCrudOnboarding()
        {
          _id = onboarding._id,
          Person = new ViewInfoPerson()
          {
            _id = onboarding.Person._id,
            TypeJourney = onboarding.Person.TypeJourney,
            Occupation = onboarding.Person.Occupation.Name,
            Name = onboarding.Person.User.Name,
            Manager = onboarding.Person.Manager.Name,
            Registration = onboarding.Person.Registration,
            User = new ViewListUser()
            {
              _id = onboarding.Person.User._id,
              Name = onboarding.Person.User.Name,
              Document = onboarding.Person.User.Document,
              Mail = onboarding.Person.User.Mail,
              Phone = onboarding.Person.User.Phone
            },
            Company = new ViewListCompany()
            {
              _id = onboarding.Person.Company._id,
              Name = onboarding.Person.Company.Name
            },
            Establishment = new ViewListEstablishment()
            {
              _id = onboarding.Person.Establishment._id,
              Name = onboarding.Person.Establishment.Name
            }
          },
          Occupation = new ViewListOccupation()
          {
            _id = onboarding.Person.Occupation._id,
            Name = onboarding.Person.Occupation.Name,
            Line = onboarding.Person.Occupation.Line,
            Company = new ViewListCompany()
            {
              _id = onboarding.Person.Occupation.Group.Company._id,
              Name = onboarding.Person.Occupation.Group.Company.Name
            },
            Group = new ViewListGroup()
            {
              _id = onboarding.Person.Occupation.Group._id,
              Name = onboarding.Person.Occupation.Group.Name,
              Line = onboarding.Person.Occupation.Group.Line,
              Axis = new ViewListAxis() { _id = onboarding.Person.Occupation.Group.Axis._id, Name = onboarding.Person.Occupation.Group.Axis.Name, TypeAxis = onboarding.Person.Occupation.Group.Axis.TypeAxis },
              Sphere = new ViewListSphere() { _id = onboarding.Person.Occupation.Group.Sphere._id, Name = onboarding.Person.Occupation.Group.Sphere.Name, TypeSphere = onboarding.Person.Occupation.Group.Sphere.TypeSphere }
            },
            Process = null,
          },
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
            Activitie = new ViewListActivitie()
            {
              _id = p.Activitie._id,
              Name = p.Activitie.Name,
              Order = p.Activitie.Order
            }
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
    public string Update(ViewCrudOnboarding view)
    {
      try
      {
        var onboarding = serviceOnboarding.GetAll(p => p._id == view._id).FirstOrDefault();
        onboarding.StatusOnBoarding = view.StatusOnBoarding;
        onboarding.CommentsEnd = view.CommentsEnd;
        onboarding.CommentsPerson = view.CommentsPerson;
        onboarding.CommentsManager = view.CommentsManager;

        if (onboarding.Person._id != _user._idPerson)
        {
          if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.WaitPerson)
          {
            onboarding.DateEndManager = DateTime.Now;
            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
              MailOccupation(onboarding.Person);
            else
              Mail(onboarding.Person);
            LogSave(view.Person._id, string.Format("Send person approval | {0}", onboarding._id));
          }

          if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.End)
          {
            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
              serviceLogMessages.NewLogMessage("OnBoarding", string.Format("Embarque | OnBoarding realizado para {0}.", onboarding.Person.User.Name), onboarding.Person);
            else
              serviceLogMessages.NewLogMessage("OnBoarding", string.Format("Embarque | OnBoarding de troca de cargo realizado para {0}.", onboarding.Person.User.Name), onboarding.Person);

            onboarding.DateEndEnd = DateTime.Now;

            Person person = servicePerson.GetNewVersion(p => p._id == onboarding.Person._id).Result;
            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
              person.TypeJourney = EnumTypeJourney.Monitoring;
            else
              person.TypeJourney = EnumTypeJourney.Checkpoint;
            servicePerson.Update(person, null);

            LogSave(view.Person._id, string.Format("Conclusion process | {0}", onboarding._id));
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
                MailDisapprovedOccupation(onboarding.Person);
              else
                MailDisapproved(onboarding.Person);
              LogSave(view.Person._id, string.Format("Send manager review | {0}", onboarding._id));
            }
            else
            {
              if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
                serviceLogMessages.NewLogMessage("OnBoarding", string.Format("Embarque | OnBoarding realizado para {0}.", onboarding.Person.User.Name), onboarding.Person);
              else
                serviceLogMessages.NewLogMessage("OnBoarding", string.Format("Embarque | OnBoarding de troca de cargo realizado para {0}.", onboarding.Person.User.Name), onboarding.Person);
              onboarding.DateEndEnd = DateTime.Now;
              Person person = servicePerson.GetNewVersion(p => p._id == onboarding.Person._id).Result;
              if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
                person.TypeJourney = EnumTypeJourney.Monitoring;
              else
                person.TypeJourney = EnumTypeJourney.Checkpoint;
              LogSave(view.Person._id, string.Format("Conclusion process | {0}", onboarding._id));
              servicePerson.Update(person, null);
            }


          }
          else if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.WaitManager)
          {
            onboarding.DateEndPerson = DateTime.Now;
            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
              MailManagerOccupation(onboarding.Person);
            else
              MailManager(onboarding.Person);
            LogSave(view.Person._id, string.Format("Send manager approval | {0}", onboarding._id));
          }
          else if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.WaitManagerRevision)
          {
            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
              MailDisapprovedOccupation(onboarding.Person);
            else
              MailDisapproved(onboarding.Person);
            LogSave(view.Person._id, string.Format("Send manager review | {0}", onboarding._id));
          }
        }
        serviceOnboarding.Update(onboarding, null);
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
        LogSave(_user._idPerson, "OnBoarding list for exclud");
        int skip = (count * (page - 1));
        var detail = serviceOnboarding.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceOnboarding.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

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
        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator & (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation) & p.Manager._id == idmanager
        & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name)
        .ToList().Select(p => new { Person = p, OnBoarding = serviceOnboarding.GetAll(x => x.Person._id == p._id & x.StatusOnBoarding != EnumStatusOnBoarding.End).FirstOrDefault() })
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
        var occupation = serviceOccupation.GetAll(p => p._id == onBoarding.Person.Occupation._id).FirstOrDefault();

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
        var user = servicePerson.GetAll(p => p._id == idperson).FirstOrDefault();
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
    private async void Mail(Person person)
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
        var mailObj = serviceMailLog.Insert(sendMail);
        var token = SendMail(pathToken, person, mailObj._id.ToString());
        var messageEnd = serviceMailMessage.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        serviceMailMessage.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async void MailManager(Person person)
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
        var mailObj = serviceMailLog.Insert(sendMail);
        var token = SendMail(pathToken, person, mailObj._id.ToString());
        var messageEnd = serviceMailMessage.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        serviceMailMessage.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async void MailOccupation(Person person)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.OnBoardingOccupationApproval(pathToken);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
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
        var mailObj = serviceMailLog.Insert(sendMail);
        var token = SendMail(pathToken, person, mailObj._id.ToString());
        var messageEnd = serviceMailMessage.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        serviceMailMessage.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async void MailManagerOccupation(Person person)
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
        var mailObj = serviceMailLog.Insert(sendMail);
        var token = SendMail(pathToken, person, mailObj._id.ToString());
        var messageEnd = serviceMailMessage.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        serviceMailMessage.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async void MailDisapproved(Person person)
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
        var mailObj = serviceMailLog.Insert(sendMail);
        var token = SendMail(pathToken, person, mailObj._id.ToString());
        var messageEnd = serviceMailMessage.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        serviceMailMessage.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async void MailDisapprovedOccupation(Person person)
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
        var mailObj = serviceMailLog.Insert(sendMail);
        var token = SendMail(pathToken, person, mailObj._id.ToString());
        var messageEnd = serviceMailMessage.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        serviceMailMessage.Update(messageEnd, null);
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
        //throw e;
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

    #region Old

    public List<OnBoarding> ListOnBoardingsWaitOld(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator & (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation) & p.Manager._id == idmanager
        & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name)
        .ToList().Select(p => new { Person = p, OnBoarding = serviceOnboarding.GetAll(x => x.Person._id == p._id & x.StatusOnBoarding != EnumStatusOnBoarding.End).FirstOrDefault() })
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
        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public OnBoarding PersonOnBoardingsWaitOld(string idmanager)
    {
      try
      {
        LogSave(idmanager, "ListWait");
        var item = servicePerson.GetAll(p => (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation) & p._id == idmanager)
        .ToList().Select(p => new { Person = p, OnBoarding = serviceOnboarding.GetAll(x => x.Person._id == p._id & x.StatusOnBoarding != EnumStatusOnBoarding.End).FirstOrDefault() })
        .FirstOrDefault();

        if (item == null)
        {
          item = servicePerson.GetAll(p => (p.TypeJourney == null) & p._id == idmanager)
        .ToList().Select(p => new { Person = p, OnBoarding = serviceOnboarding.GetAll(x => x.Person._id == p._id & x.StatusOnBoarding != EnumStatusOnBoarding.End).FirstOrDefault() })
        .FirstOrDefault();
          return null;
        }

        if ((item.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation) || (item.Person.TypeJourney == EnumTypeJourney.OnBoarding))
        {
          if (item.OnBoarding == null)
            return new OnBoarding
            {
              Person = item.Person,
              _id = null,
              StatusOnBoarding = EnumStatusOnBoarding.WaitBegin
            };
          else
            return item.OnBoarding;
        }
        else
          return null;

      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public OnBoarding NewOnBoardingOld(OnBoarding onBoarding, string idperson)
    {
      try
      {
        LogSave(onBoarding.Person._id, "OnBoarding Process");
        if (onBoarding._id == null)
        {
          LoadMapOld(onBoarding);

          if (onBoarding.Person._id == idperson)
          {
            onBoarding.DateBeginPerson = DateTime.Now;
            onBoarding.StatusOnBoarding = EnumStatusOnBoarding.InProgressPerson;
          }
          else
          {
            onBoarding.DateBeginManager = DateTime.Now;
            onBoarding.StatusOnBoarding = EnumStatusOnBoarding.InProgressManager;
          }

          serviceOnboarding.Insert(onBoarding);
        }
        else
        {
          if (onBoarding.StatusOnBoarding == EnumStatusOnBoarding.WaitPerson)
          {
            onBoarding.DateBeginEnd = DateTime.Now;
          }
          serviceOnboarding.Update(onBoarding, null);
        }

        return onBoarding;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<OnBoarding> ListOnBoardingsEndOld(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(idmanager, "ListEnd");
        int skip = (count * (page - 1));
        var detail = serviceOnboarding.GetAll(p => p.Person.Manager._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceOnboarding.GetAll(p => p.Person.Manager._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ListComments> AddCommentsOld(string idonboarding, string iditem, ListComments comments)
    {
      try
      {
        var onboarding = serviceOnboarding.GetAll(p => p._id == idonboarding).FirstOrDefault();
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


            comments._id = ObjectId.GenerateNewId().ToString(); comments._idAccount = _user._idAccount; item.Comments.Add(comments);

            serviceOnboarding.Update(onboarding, null);

            return item.Comments;
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

            comments._id = ObjectId.GenerateNewId().ToString(); comments._idAccount = _user._idAccount; item.Comments.Add(comments);

            serviceOnboarding.Update(onboarding, null);

            return item.Comments;
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

            comments._id = ObjectId.GenerateNewId().ToString(); comments._idAccount = _user._idAccount; item.Comments.Add(comments);

            serviceOnboarding.Update(onboarding, null);

            return item.Comments;
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

            comments._id = ObjectId.GenerateNewId().ToString(); comments._idAccount = _user._idAccount; item.Comments.Add(comments);

            serviceOnboarding.Update(onboarding, null);

            return item.Comments;
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
            comments._id = ObjectId.GenerateNewId().ToString(); comments._idAccount = _user._idAccount; item.Comments.Add(comments);

            serviceOnboarding.Update(onboarding, null);
            return item.Comments;
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

            comments._id = ObjectId.GenerateNewId().ToString(); comments._idAccount = _user._idAccount; item.Comments.Add(comments);

            serviceOnboarding.Update(onboarding, null);
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
    public string UpdateCommentsOld(string idonboarding, string iditem, ListComments comments)
    {
      try
      {
        var onboarding = serviceOnboarding.GetAll(p => p._id == idonboarding).FirstOrDefault();
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

                serviceOnboarding.Update(onboarding, null);
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

                serviceOnboarding.Update(onboarding, null);
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

                serviceOnboarding.Update(onboarding, null);
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

                serviceOnboarding.Update(onboarding, null);
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

                serviceOnboarding.Update(onboarding, null);
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

                serviceOnboarding.Update(onboarding, null);
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
    public List<ListComments> GetListCommentsOld(string idonboarding, string iditem)
    {
      try
      {
        var onboarding = serviceOnboarding.GetAll(p => p._id == idonboarding).FirstOrDefault();
        foreach (var item in onboarding.Activities)
        {
          if (item._id == iditem)
          {
            return item.Comments;
          }
        }

        foreach (var item in onboarding.Scopes)
        {
          if (item._id == iditem)
          {
            return item.Comments;
          }
        }

        foreach (var item in onboarding.Schoolings)
        {
          if (item._id == iditem)
          {
            return item.Comments;
          }
        }

        foreach (var item in onboarding.SkillsCompany)
        {
          if (item._id == iditem)
          {
            return item.Comments;
          }
        }

        foreach (var item in onboarding.SkillsGroup)
        {
          if (item._id == iditem)
          {
            return item.Comments;
          }
        }

        foreach (var item in onboarding.SkillsOccupation)
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
    public List<OnBoarding> PersonOnBoardingsEndOld(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(idmanager, "ListPersonEnd");
        int skip = (count * (page - 1));
        var detail = serviceOnboarding.GetAll(p => p.Person._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceOnboarding.GetAll(p => p.Person._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public OnBoarding GetOnBoardingsOld(string id)
    {
      try
      {
        return serviceOnboarding.GetAll(p => p._id == id)
          .ToList().Select(p => new OnBoarding()
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
            SkillsGroup = p.SkillsGroup.OrderBy(x => x.Skill.Name).ToList(),
            SkillsOccupation = p.SkillsOccupation.OrderBy(x => x.Skill.Name).ToList(),
            Scopes = p.Scopes.OrderBy(x => x.Scope.Order).ToList(),
            Schoolings = p.Schoolings.OrderBy(x => x.Schooling.Order).ToList(),
            Activities = p.Activities.OrderBy(x => x.Activitie.Order).ToList(),
            StatusOnBoarding = p.StatusOnBoarding,
          })
          .FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateOnBoardingOld(OnBoarding onboarding, string idperson)
    {
      try
      {
        LogSave(onboarding.Person._id, string.Format("Update id {0}", onboarding._id));

        if (onboarding.Person._id != idperson)
        {
          if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.WaitPerson)
          {
            onboarding.DateEndManager = DateTime.Now;
            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
              MailOccupation(onboarding.Person);
            else
              Mail(onboarding.Person);
          }

          if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.End)
          {
            serviceLogMessages.NewLogMessage("Onboarding", "Gestor e Colaborador realizaram o Onboarding de " + onboarding.Person.User.Name, onboarding.Person);
            onboarding.DateEndEnd = DateTime.Now;

            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
              onboarding.Person.TypeJourney = EnumTypeJourney.Monitoring;
            else
              onboarding.Person.TypeJourney = EnumTypeJourney.Checkpoint;

            servicePerson.Update(onboarding.Person, null);
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
            }
            else
            {
              serviceLogMessages.NewLogMessage("Onboarding", "Gestor e Colaborador realizaram o Onboarding de " + onboarding.Person.User.Name, onboarding.Person);
              onboarding.DateEndEnd = DateTime.Now;

              if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
                onboarding.Person.TypeJourney = EnumTypeJourney.Monitoring;
              else
                onboarding.Person.TypeJourney = EnumTypeJourney.Checkpoint;

              servicePerson.Update(onboarding.Person, null);
            }


          }
          else if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.WaitManager)
          {
            onboarding.DateEndPerson = DateTime.Now;
            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
              MailManagerOccupation(onboarding.Person);
            else
              MailManager(onboarding.Person);
          }
          else if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.WaitManagerRevision)
          {
            MailDisapproved(onboarding.Person);
          }
        }
        serviceOnboarding.Update(onboarding, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<OnBoarding> GetListExcludOld(ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(_user._idPerson, "ListExclud");
        int skip = (count * (page - 1));
        var detail = serviceOnboarding.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceOnboarding.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

  }
}
