﻿using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
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
  public class ServiceOnBoarding : Repository<OnBoarding>, IServiceOnBoarding
  {
    private readonly ServiceGeneric<OnBoarding> onBoardingService;
    private readonly ServiceGeneric<Occupation> occupationService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceLog logService;
    private readonly ServiceMailModel mailModelService;
    private readonly ServiceGeneric<MailMessage> mailMessageService;
    private readonly ServiceGeneric<MailLog> mailService;
    private readonly ServiceLogMessages logMessagesService;
    public string path;

    public ServiceOnBoarding(DataContext context, string pathToken)
      : base(context)
    {
      try
      {
        onBoardingService = new ServiceGeneric<OnBoarding>(context);
        personService = new ServiceGeneric<Person>(context);
        logService = new ServiceLog(_context);
        mailModelService = new ServiceMailModel(context);
        mailMessageService = new ServiceGeneric<MailMessage>(context);
        mailService = new ServiceGeneric<MailLog>(context);
        logMessagesService = new ServiceLogMessages(context);
        occupationService = new ServiceGeneric<Occupation>(context);
        path = pathToken;
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
        var list = onBoardingService.GetAuthentication(p => p.Status == EnumStatus.Enabled).ToList();

        foreach (var item in list)
        {

          foreach (var row in item.Activities)
          {
            row.Comments = new List<ListComments>();
            var commentPerson = new ListComments();
            commentPerson._id = ObjectId.GenerateNewId().ToString();
            commentPerson._idAccount = item._idAccount;
            commentPerson.Comments = row.CommentsPerson;
            commentPerson.StatusView = EnumStatusView.View;
            commentPerson.Date = item.DateBeginPerson;
            commentPerson.UserComment = EnumUserComment.Person;

            var commentManager = new ListComments();
            commentManager._id = ObjectId.GenerateNewId().ToString();
            commentManager._idAccount = item._idAccount;
            commentManager.Comments = row.CommentsManager;
            commentManager.StatusView = EnumStatusView.View;
            commentManager.Date = item.DateBeginManager;
            commentManager.UserComment = EnumUserComment.Manager;

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
            var commentPerson = new ListComments();
            commentPerson._id = ObjectId.GenerateNewId().ToString();
            commentPerson._idAccount = item._idAccount;
            commentPerson.Comments = row.CommentsPerson;
            commentPerson.StatusView = EnumStatusView.View;
            commentPerson.Date = item.DateBeginPerson;
            commentPerson.UserComment = EnumUserComment.Person;

            var commentManager = new ListComments();
            commentManager._id = ObjectId.GenerateNewId().ToString();
            commentManager._idAccount = item._idAccount;
            commentManager.Comments = row.CommentsManager;
            commentManager.StatusView = EnumStatusView.View;
            commentManager.Date = item.DateBeginManager;
            commentManager.UserComment = EnumUserComment.Manager;

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

          foreach (var row in item.Scopes)
          {
            row.Comments = new List<ListComments>();
            var commentPerson = new ListComments();
            commentPerson._id = ObjectId.GenerateNewId().ToString();
            commentPerson._idAccount = item._idAccount;
            commentPerson.Comments = row.CommentsPerson;
            commentPerson.StatusView = EnumStatusView.View;
            commentPerson.Date = item.DateBeginPerson;
            commentPerson.UserComment = EnumUserComment.Person;

            var commentManager = new ListComments();
            commentManager._id = ObjectId.GenerateNewId().ToString();
            commentManager._idAccount = item._idAccount;
            commentManager.Comments = row.CommentsManager;
            commentManager.StatusView = EnumStatusView.View;
            commentManager.Date = item.DateBeginManager;
            commentManager.UserComment = EnumUserComment.Manager;

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
            var commentPerson = new ListComments();
            commentPerson._id = ObjectId.GenerateNewId().ToString();
            commentPerson._idAccount = item._idAccount;
            commentPerson.Comments = row.CommentsPerson;
            commentPerson.StatusView = EnumStatusView.View;
            commentPerson.Date = item.DateBeginPerson;
            commentPerson.UserComment = EnumUserComment.Person;

            var commentManager = new ListComments();
            commentManager._id = ObjectId.GenerateNewId().ToString();
            commentManager._idAccount = item._idAccount;
            commentManager.Comments = row.CommentsManager;
            commentManager.StatusView = EnumStatusView.View;
            commentManager.Date = item.DateBeginManager;
            commentManager.UserComment = EnumUserComment.Manager;

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

          foreach (var row in item.SkillsGroup)
          {
            row.Comments = new List<ListComments>();
            var commentPerson = new ListComments();
            commentPerson._id = ObjectId.GenerateNewId().ToString();
            commentPerson._idAccount = item._idAccount;
            commentPerson.Comments = row.CommentsPerson;
            commentPerson.StatusView = EnumStatusView.View;
            commentPerson.Date = item.DateBeginPerson;
            commentPerson.UserComment = EnumUserComment.Person;

            var commentManager = new ListComments();
            commentManager._id = ObjectId.GenerateNewId().ToString();
            commentManager._idAccount = item._idAccount;
            commentManager.Comments = row.CommentsManager;
            commentManager.StatusView = EnumStatusView.View;
            commentManager.Date = item.DateBeginManager;
            commentManager.UserComment = EnumUserComment.Manager;

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

          foreach (var row in item.SkillsOccupation)
          {
            row.Comments = new List<ListComments>();
            var commentPerson = new ListComments();
            commentPerson._id = ObjectId.GenerateNewId().ToString();
            commentPerson._idAccount = item._idAccount;
            commentPerson.Comments = row.CommentsPerson;
            commentPerson.StatusView = EnumStatusView.View;
            commentPerson.Date = item.DateBeginPerson;
            commentPerson.UserComment = EnumUserComment.Person;

            var commentManager = new ListComments();
            commentManager._id = ObjectId.GenerateNewId().ToString();
            commentManager._idAccount = item._idAccount;
            commentManager.Comments = row.CommentsManager;
            commentManager.StatusView = EnumStatusView.View;
            commentManager.Date = item.DateBeginManager;
            commentManager.UserComment = EnumUserComment.Manager;

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

          onBoardingService.UpdateAccount(item, null);
        }

        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<OnBoarding> ListOnBoardingsEnd(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(idmanager, "ListEnd");
        int skip = (count * (page - 1));
        var detail = onBoardingService.GetAll(p => p.Person.Manager._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = onBoardingService.GetAll(p => p.Person.Manager._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private void NewOnZero()
    {
      try
      {
        var on = onBoardingService.GetAuthentication(p => p.Status == EnumStatus.Disabled).Count();
        if (on == 0)
        {
          var person = personService.GetAll().FirstOrDefault();
          var zero = onBoardingService.Insert(new OnBoarding() { Person = person, Status = EnumStatus.Disabled, StatusOnBoarding = EnumStatusOnBoarding.End });
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public List<ListComments> AddComments(string idonboarding, string iditem, ListComments comments)
    {
      try
      {
        var onboarding = onBoardingService.GetAll(p => p._id == idonboarding).FirstOrDefault();
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

            onBoardingService.Update(onboarding, null);

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

            onBoardingService.Update(onboarding, null);

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

            onBoardingService.Update(onboarding, null);

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

            onBoardingService.Update(onboarding, null);

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

            onBoardingService.Update(onboarding, null);
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

            onBoardingService.Update(onboarding, null);
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

    public string UpdateComments(string idonboarding, string iditem, ListComments comments)
    {
      try
      {
        var onboarding = onBoardingService.GetAll(p => p._id == idonboarding).FirstOrDefault();
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

                onBoardingService.Update(onboarding, null);
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

                onBoardingService.Update(onboarding, null);
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

                onBoardingService.Update(onboarding, null);
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

                onBoardingService.Update(onboarding, null);
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

                onBoardingService.Update(onboarding, null);
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

                onBoardingService.Update(onboarding, null);
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


    public string UpdateCommentsView(string idonboarding, string iditem, EnumUserComment userComment)
    {
      try
      {
        var onboarding = onBoardingService.GetAll(p => p._id == idonboarding).FirstOrDefault();
        foreach (var item in onboarding.Activities)
        {
          if (item._id == iditem)
          {
            if (userComment == EnumUserComment.Manager)
              item.StatusViewManager = EnumStatusView.View;
            else
              item.StatusViewPerson = EnumStatusView.View;

            onBoardingService.Update(onboarding, null);
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

            onBoardingService.Update(onboarding, null);
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

            onBoardingService.Update(onboarding, null);
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

            onBoardingService.Update(onboarding, null);
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

            onBoardingService.Update(onboarding, null);
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

            onBoardingService.Update(onboarding, null);
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
        var onboarding = onBoardingService.GetAll(p => p._id == idonboarding).FirstOrDefault();
        foreach (var item in onboarding.Activities)
        {
          if (item._id == iditem)
          {
            foreach (var comment in item.Comments)
            {
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                onBoardingService.Update(onboarding, null);
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
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                onBoardingService.Update(onboarding, null);
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
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                onBoardingService.Update(onboarding, null);
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
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                onBoardingService.Update(onboarding, null);
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
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                onBoardingService.Update(onboarding, null);
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
              if (comment._id == idcomments)
              {
                item.Comments.Remove(comment);
                onBoardingService.Update(onboarding, null);
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

    public List<ListComments> GetListComments(string idonboarding, string iditem)
    {
      try
      {
        var onboarding = onBoardingService.GetAll(p => p._id == idonboarding).FirstOrDefault();
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

    public List<OnBoarding> ListOnBoardingsWait(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(idmanager, "List");
        NewOnZero();
        int skip = (count * (page - 1));
        var list = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation) & p.Manager._id == idmanager
        & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name)
        .ToList().Select(p => new { Person = p, OnBoarding = onBoardingService.GetAll(x => x.Person._id == p._id & x.StatusOnBoarding != EnumStatusOnBoarding.End).FirstOrDefault() })
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
                StatusOnBoarding = EnumStatusOnBoarding.Open
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
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<OnBoarding> PersonOnBoardingsEnd(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(idmanager, "ListPersonEnd");
        int skip = (count * (page - 1));
        var detail = onBoardingService.GetAll(p => p.Person._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = onBoardingService.GetAll(p => p.Person._id == idmanager & p.StatusOnBoarding == EnumStatusOnBoarding.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public OnBoarding PersonOnBoardingsWait(string idmanager)
    {
      try
      {
        LogSave(idmanager, "ListWait");
        var item = personService.GetAll(p => (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation) & p._id == idmanager)
        .ToList().Select(p => new { Person = p, OnBoarding = onBoardingService.GetAll(x => x.Person._id == p._id & x.StatusOnBoarding != EnumStatusOnBoarding.End).FirstOrDefault() })
        .FirstOrDefault();

        if (item == null)
        {
          item = personService.GetAll(p => (p.TypeJourney == null) & p._id == idmanager)
        .ToList().Select(p => new { Person = p, OnBoarding = onBoardingService.GetAll(x => x.Person._id == p._id & x.StatusOnBoarding != EnumStatusOnBoarding.End).FirstOrDefault() })
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
              StatusOnBoarding = EnumStatusOnBoarding.Open
            };
          else
            return item.OnBoarding;
        }
        else
          return null;

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public OnBoarding GetOnBoardings(string id)
    {
      try
      {
        return onBoardingService.GetAll(p => p._id == id)
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
        throw new ServiceException(_user, e, this._context);
      }
    }

    private OnBoarding LoadMap(OnBoarding onBoarding)
    {
      try
      {
        var occupation = occupationService.GetAll(p => p._id == onBoarding.Person.Occupation._id).FirstOrDefault();

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
        throw new ServiceException(_user, e, this._context);
      }
    }

    public OnBoarding NewOnBoarding(OnBoarding onBoarding, string idperson)
    {
      try
      {
        LogSave(onBoarding.Person._id, "OnBoarding Process");
        if (onBoarding._id == null)
        {
          LoadMap(onBoarding);

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

          onBoardingService.Insert(onBoarding);
        }
        else
        {
          if (onBoarding.StatusOnBoarding == EnumStatusOnBoarding.Wait)
          {
            onBoarding.DateBeginEnd = DateTime.Now;
          }
          onBoardingService.Update(onBoarding, null);
        }

        return onBoarding;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }


    private bool validOnboardingComments(OnBoarding onBoarding)
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

    public string UpdateOnBoarding(OnBoarding onboarding, string idperson)
    {
      try
      {
        LogSave(onboarding.Person._id, "OnBoarding Process Update");

        if (onboarding.Person._id != idperson)
        {
          if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.Wait)
          {
            onboarding.DateEndManager = DateTime.Now;
            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
              MailOccupation(onboarding.Person);
            else
              Mail(onboarding.Person);
          }

          if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.End)
          {
            logMessagesService.NewLogMessage("Onboarding", "Gestor e Colaborador realizaram o Onboarding de " + onboarding.Person.User.Name, onboarding.Person);
            onboarding.DateEndEnd = DateTime.Now;

            if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
              onboarding.Person.TypeJourney = EnumTypeJourney.Monitoring;
            else
              onboarding.Person.TypeJourney = EnumTypeJourney.Checkpoint;

            personService.Update(onboarding.Person, null);
          }
        }
        else
        {
          if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.End)
          {


            if (validOnboardingComments(onboarding))
            {
              onboarding.StatusOnBoarding = EnumStatusOnBoarding.Disapproved;
              onboarding.DateEndPerson = DateTime.Now;
            }
            else
            {
              logMessagesService.NewLogMessage("Onboarding", "Gestor e Colaborador realizaram o Onboarding de " + onboarding.Person.User.Name, onboarding.Person);
              onboarding.DateEndEnd = DateTime.Now;

              if (onboarding.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
                onboarding.Person.TypeJourney = EnumTypeJourney.Monitoring;
              else
                onboarding.Person.TypeJourney = EnumTypeJourney.Checkpoint;

              personService.Update(onboarding.Person, null);
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
          else if (onboarding.StatusOnBoarding == EnumStatusOnBoarding.Disapproved)
          {
            MailDisapproved(onboarding.Person);
          }
        }
        onBoardingService.Update(onboarding, null);
        return "update";
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
          Description = "Access OnBoarding ",
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

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      personService._user = _user;
      onBoardingService._user = _user;
      occupationService._user = _user;
      logService._user = _user;
      mailModelService._user = _user;
      mailMessageService._user = _user;
      mailService._user = _user;
      logMessagesService.SetUser(_user);
      mailModelService.SetUser(contextAccessor);
    }

    // send mail
    public async void Mail(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.OnBoardingApproval(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        string managername = "";
        try
        {
          managername = personService.GetAll(p => p._id == person.Manager._id).FirstOrDefault().User.Name;
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

    public async void MailManager(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.OnBoardingApprovalManager(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        string managername = "";
        try
        {
          managername = personService.GetAll(p => p._id == person.Manager._id).FirstOrDefault().User.Name;
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
                        new MailLogAddress(person.Manager.User.Mail, person.Manager.User.Name)
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

    public async void MailOccupation(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.OnBoardingApprovalOccupation(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.User.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
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

    public async void MailManagerOccupation(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.OnBoardingApprovalManagerOccupation(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        string managername = "";
        try
        {
          managername = personService.GetAll(p => p._id == person.Manager._id).FirstOrDefault().User.Name;
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
                        new MailLogAddress(person.Manager.User.Mail, person.Manager.User.Name)
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

    public async void MailDisapproved(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.OnBoardingDisapproval(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        string managername = "";
        try
        {
          managername = personService.GetAll(p => p._id == person.Manager._id).FirstOrDefault().User.Name;
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
                        new MailLogAddress(person.Manager.User.Mail, person.Manager.User.Name)
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

    public string SendMail(string link, Person person, string idmail)
    {
      try
      {
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link);
          var data = new
          {
            mail = person.User.Mail,
            password = person.User.Password
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

    public string RemoveOnBoarding(string idperson)
    {
      try
      {
        LogSave(_user._idPerson, "RemoveOnboarding:" + idperson);
        var onboarding = onBoardingService.GetAll(p => p.Person._id == idperson).FirstOrDefault();
        onboarding.Status = EnumStatus.Disabled;
        onBoardingService.Update(onboarding, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<OnBoarding> GetListExclud(ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(_user._idPerson, "ListExclud");
        int skip = (count * (page - 1));
        var detail = onBoardingService.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = onBoardingService.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
#pragma warning restore 1998
}
