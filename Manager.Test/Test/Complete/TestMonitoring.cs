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
    public class TestMonitoring : TestCommons<Monitoring>
  {
    private ServiceMonitoring serviceMonitoring;
    private readonly ServiceGeneric<Person> servicePerson;

    public TestMonitoring()
    {
      base.Init();
      //serviceMonitoring = new ServiceMonitoring(context, context, "");
      //serviceMonitoring.SetUser(base.baseUser);

      servicePerson = new ServiceGeneric<Person>(base.context);
      servicePerson._user = base.baseUser;
    }

    //[Fact]
    //public void TestMonitoringComplete()
    //{
    //  try
    //  {
    //    long total = 0;
    //    var person = servicePerson.GetAll(p => p.User.Name.Contains("Ariel")).FirstOrDefault();

    //    var list = serviceMonitoring.ListMonitoringsWait(person.Manager._id, ref total, "Ariel", 10, 1).FirstOrDefault();
    //    var newOn = serviceMonitoring.NewMonitoring(list, person.Manager._id);

    //    foreach (var item in newOn.SkillsCompany)
    //    {
    //      item.CommentsManager = "teste 1";
    //    }
    //    newOn.StatusMonitoring = EnumStatusMonitoring.Wait;
    //    serviceMonitoring.UpdateMonitoring(newOn, person.Manager._id);

    //    var listskills = serviceMonitoring.GetSkills(person._id);


    //  }
    //  catch (Exception e)
    //  {
    //    throw e;
    //  }
    //}

    [Fact]
    public void ScriptComments()
    {
      try
      {
        var list = serviceMonitoring.GetAuthentication(p => p.Status == EnumStatus.Enabled).ToList();

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

          serviceMonitoring.UpdateAccount(item, null);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
