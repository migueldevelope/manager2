using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Test.Commons;
using Manager.Views.Enumns;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Manager.Test.Test.Complete
{
  public class TestOnBoarding : TestCommons<OnBoarding>
  {
    private ServiceOnBoarding serviceOnBoarding;
    private readonly ServiceGeneric<Person> servicePerson;

    public TestOnBoarding()
    {
      base.Init();
      serviceOnBoarding = new ServiceOnBoarding(base.context, "http://10.0.0.15/");
      serviceOnBoarding.SetUser(base.contextAccessor);

      servicePerson = new ServiceGeneric<Person>(base.context);
      servicePerson._user = base.baseUser;
    }

    [Fact]
    public void ScriptComments()
    {
      try
      {
        var list = serviceOnBoarding.GetAuthentication(p => p.Status == EnumStatus.Enabled).ToList();

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

          serviceOnBoarding.UpdateAccount(item, null);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void TestOnBoardingComplete()
    {
      try
      {
        long total = 0;
        var person = servicePerson.GetAll(p => p.User.Name.Contains("Ariel")).FirstOrDefault();

        var list = serviceOnBoarding.ListOnBoardingsWaitOld(person.Manager._id, ref total, "Ariel", 10, 1).FirstOrDefault();
        var newOn = serviceOnBoarding.NewOnBoardingOld(list, person.Manager._id);

        foreach(var item in newOn.SkillsCompany)
        {
          item.CommentsManager = "teste 1";
        }
        newOn.StatusOnBoarding = EnumStatusOnBoarding.WaitPerson;
        //serviceOnBoarding.UpdateOnBoarding(newOn, person.Manager._id);


      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
