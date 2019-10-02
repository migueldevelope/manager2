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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceGoals : Repository<Goals>, IServiceGoals
  {
    private readonly ServiceGeneric<Goals> serviceGoals;
    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceGeneric<GoalsPeriod> serviceGoalsPeriod;
    private readonly ServiceGeneric<GoalsCompany> serviceGoalsCompany;
    private readonly ServiceGeneric<GoalsManager> serviceGoalsManager;
    private readonly ServiceGeneric<GoalsPerson> serviceGoalsPerson;
    private readonly ServiceGeneric<GoalsPersonControl> serviceGoalsPersonControl;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceLog serviceLog;
    public string path;

    #region Constructor
    public ServiceGoals(DataContext context, DataContext contextLog, string pathToken, IServiceControlQueue _serviceControlQueue) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context, contextLog, _serviceControlQueue, pathToken);
        serviceGoals = new ServiceGeneric<Goals>(context);
        serviceGoalsCompany = new ServiceGeneric<GoalsCompany>(context);
        serviceGoalsPeriod = new ServiceGeneric<GoalsPeriod>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceGoalsPersonControl = new ServiceGeneric<GoalsPersonControl>(context);
        serviceGoalsManager = new ServiceGeneric<GoalsManager>(context);
        serviceGoalsPerson = new ServiceGeneric<GoalsPerson>(context);
        serviceLog = new ServiceLog(context, contextLog);
        serviceMail = new ServiceGeneric<MailLog>(context);
        serviceMailModel = new ServiceMailModel(context);
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
      serviceGoals._user = _user;
      serviceLog._user = _user;
      serviceGoalsCompany._user = _user;
      serviceGoalsPeriod._user = _user;
      servicePerson._user = _user;
      serviceGoalsManager._user = _user;
      serviceGoalsPerson._user = _user;
      serviceGoalsPersonControl._user = _user;
      serviceMail._user = _user;
      serviceMailModel.SetUser(_user);
    }

    public void SetUser(BaseUser baseUser)
    {
      _user = baseUser;
      serviceGoals._user = baseUser;
      serviceGoalsCompany._user = baseUser;
      serviceGoalsPeriod._user = baseUser;
      servicePerson._user = baseUser;
      serviceGoalsManager._user = baseUser;
      serviceGoalsPerson._user = baseUser;
      serviceGoalsPersonControl._user = baseUser;
      serviceLog._user = baseUser;
      serviceMail._user = baseUser;
      serviceMailModel.SetUser(baseUser);
    }
    #endregion

    #region private
    private double GetAchievement(decimal achievement)
    {
      switch (achievement)
      {
        case 1:
          return 0;
        case 2:
          return 0.8;
        case 3:
          return 1;
        case 4:
          return 1.2;
      }

      return 0;
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

    private void Mail(Person person)
    {
      try
      {
        var model = serviceMailModel.GoalsApproval(path);
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
          From = new MailLogAddress("suporte@analisa.solutions", "Suporte ao Cliente | Analisa fluid careers"),
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
        var model = serviceMailModel.GoalsApprovalManager(path);
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
          From = new MailLogAddress("suporte@analisa.solutions", "Suporte ao Cliente | Analisa fluid careers"),
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

    private void MailDisapproval(Person person)
    {
      try
      {
        var model = serviceMailModel.GoalsDisapproval(path);
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
          From = new MailLogAddress("suporte@analisa.solutions", "Suporte ao Cliente | Analisa fluid careers"),
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

    #endregion

    #region Goals
    public string New(ViewCrudGoal view)
    {
      try
      {
        Goals goal = new Goals()
        {
          Name = view.Name,
          Concept = view.Concept,
          TypeGoals = view.TypeGoals
        };
        serviceGoals.InsertNewVersion(goal).Wait();
        return goal._id;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudGoal view)
    {
      try
      {
        Goals goal = serviceGoals.GetNewVersion(p => p._id == view._id).Result;
        if (goal == null)
          throw new Exception("Goal not available!");
        goal.Name = view.Name;
        goal.Concept = view.Concept;
        goal.TypeGoals = view.TypeGoals;
        serviceGoals.Update(goal, null).Wait();
        return "Goal altered!";
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
        var count = serviceGoalsCompany.CountNewVersion(p => p.GoalsCompanyList.Goals._id == id).Result
        + serviceGoalsManager.CountNewVersion(p => p.GoalsManagerList.Goals._id == id).Result
        + serviceGoalsPerson.CountNewVersion(p => p.GoalsPersonList.Goals._id == id).Result;

        if (count > 0)
          return "exists";

        Goals item = serviceGoals.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceGoals.Update(item, null).Wait();

        return "Goal deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudGoal Get(string id)
    {
      try
      {
        var goal = serviceGoals.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudGoal()
        {
          _id = goal._id,
          Name = goal.Name,
          Concept = goal.Concept,
          TypeGoals = goal.TypeGoals
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListGoal> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListGoal> detail = serviceGoals.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewListGoal()
          {
            _id = p._id,
            Name = p.Name
          }).ToList();
        total = serviceGoals.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListGoal> ListCompany(string id, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListGoal> detail = serviceGoals.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewListGoal()
          {
            _id = p._id,
            Name = p.Name
          }).ToList();

        var goals = serviceGoalsCompany.GetAllNewVersion(p => p.Company._id == id).Result.Select(p => new ViewListGoal()
        {
          _id = p.GoalsCompanyList.Goals._id,
          Name = p.GoalsCompanyList.Goals.Name
        }).ToList();

        foreach (var item in goals)
        {
          var goal = detail.Where(p => p.Name == item.Name).FirstOrDefault();
          detail.Remove(goal);
        }


        total = serviceGoals.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewListGoal> ListManager(string id, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListGoal> detail = serviceGoals.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewListGoal()
          {
            _id = p._id,
            Name = p.Name
          }).ToList();

        var goals = serviceGoalsManager.GetAllNewVersion(p => p.Manager._id == id).Result.Select(p => new ViewListGoal()
        {
          _id = p.GoalsManagerList.Goals._id,
          Name = p.GoalsManagerList.Goals.Name
        }).ToList();

        foreach (var item in goals)
        {
          var goal = detail.Where(p => p.Name == item.Name).FirstOrDefault();
          detail.Remove(goal);
        }

        total = serviceGoals.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region Period Goals
    public string NewGoalsPeriod(ViewCrudGoalPeriod view)
    {
      try
      {
        GoalsPeriod goalsPeriod = new GoalsPeriod()
        {
          Review = view.Review,
          Name = view.Name,
          ChangeCheck = view.ChangeCheck,
          DateBegin = view.DateBegin,
          DateEnd = view.DateEnd
        };
        serviceGoalsPeriod.InsertNewVersion(goalsPeriod).Wait();
        return "Period goals added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateGoalsPeriod(ViewCrudGoalPeriod view)
    {
      try
      {
        GoalsPeriod goalsPeriod = serviceGoalsPeriod.GetNewVersion(p => p._id == view._id).Result;
        goalsPeriod.Review = view.Review;
        goalsPeriod.Name = view.Name;
        goalsPeriod.ChangeCheck = view.ChangeCheck;
        goalsPeriod.DateBegin = view.DateBegin;
        goalsPeriod.DateEnd = view.DateEnd;
        serviceGoalsPeriod.Update(goalsPeriod, null).Wait();
        return "Period goals altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string DeleteGoalsPeriod(string id)
    {
      try
      {
        GoalsPeriod item = serviceGoalsPeriod.GetNewVersion(p => p._id == id).Result;
        if (item == null)
          return "Company goal deleted!";

        var count = serviceGoalsCompany.CountNewVersion(p => p.GoalsPeriod._id == id).Result
        + serviceGoalsManager.CountNewVersion(p => p.GoalsPeriod._id == id).Result
        + serviceGoalsPerson.CountNewVersion(p => p.GoalsPeriod._id == id).Result;

        if (count > 0)
          return "exists";


        item.Status = EnumStatus.Disabled;
        serviceGoalsPeriod.Update(item, null).Wait();
        return "Period goals deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudGoalPeriod GetGoalsPeriod(string id)
    {
      try
      {
        GoalsPeriod goalsPeriod = serviceGoalsPeriod.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudGoalPeriod()
        {
          _id = goalsPeriod._id,
          Review = goalsPeriod.Review,
          Name = goalsPeriod.Name,
          ChangeCheck = goalsPeriod.ChangeCheck,
          DateBegin = goalsPeriod.DateBegin,
          DateEnd = goalsPeriod.DateEnd
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewCrudGoalPeriod> ListGoalsPeriod(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewCrudGoalPeriod> detail = serviceGoalsPeriod.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewCrudGoalPeriod()
          {
            _id = p._id,
            Review = p.Review,
            Name = p.Name,
            ChangeCheck = p.ChangeCheck,
            DateBegin = p.DateBegin,
            DateEnd = p.DateEnd
          }).ToList();

        total = serviceGoalsPeriod.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region Company Goals
    public string NewGoalsCompany(ViewCrudGoalCompany view)
    {
      try
      {

        GoalsCompany goalsCompany = new GoalsCompany()
        {
          GoalsPeriod = view.GoalsPeriod,
          Company = view.Company,
          GoalsCompanyList = view.GoalsCompanyList == null ? null : new GoalsItem()
          {
            _id = ObjectId.GenerateNewId().ToString(),
            Weight = view.GoalsCompanyList.Weight,
            Achievement = view.GoalsCompanyList.Achievement,
            Deadline = view.GoalsCompanyList.Deadline,
            Goals = new ViewListGoal
            {
              _id = view.GoalsCompanyList.Goals._id,
              Name = view.GoalsCompanyList.Goals.Name
            },
            Realized = view.GoalsCompanyList.Realized,
            Result = view.GoalsCompanyList.Result,
            Target = view.GoalsCompanyList.Target
          }
        };
        serviceGoalsCompany.InsertNewVersion(goalsCompany).Wait();
        return "Company goal added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateGoalsCompany(ViewCrudGoalCompany view)
    {
      try
      {
        GoalsCompany goalsCompany = serviceGoalsCompany.GetNewVersion(p => p._id == view._id).Result;
        goalsCompany.GoalsPeriod = view.GoalsPeriod;
        goalsCompany.Company = view.Company;
        goalsCompany.GoalsCompanyList = view.GoalsCompanyList == null ? null : new GoalsItem()
        {
          _id = view._id,
          Weight = view.GoalsCompanyList.Weight,
          Achievement = view.GoalsCompanyList.Achievement,
          Deadline = view.GoalsCompanyList.Deadline,
          Goals = new ViewListGoal
          {
            _id = view.GoalsCompanyList.Goals._id,
            Name = view.GoalsCompanyList.Goals.Name
          },
          Realized = view.GoalsCompanyList.Realized,
          Result = view.GoalsCompanyList.Result,
          Target = view.GoalsCompanyList.Target
        };
        serviceGoalsCompany.Update(goalsCompany, null).Wait();
        return "Company goal altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateGoalsCompanyAchievement(ViewCrudAchievement view)
    {
      try
      {
        GoalsCompany goalsCompany = serviceGoalsCompany.GetNewVersion(p => p._id == view._id).Result;
        goalsCompany.GoalsCompanyList.Achievement = view.Achievement;

        serviceGoalsCompany.Update(goalsCompany, null).Wait();
        return "Company goal altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteGoalsCompany(string id)
    {
      try
      {
        GoalsCompany goalsCompany = serviceGoalsCompany.GetNewVersion(p => p._id == id).Result;
        if (goalsCompany == null)
          return "Company goal deleted!";

        goalsCompany.Status = EnumStatus.Disabled;
        serviceGoalsCompany.Update(goalsCompany, null).Wait();
        return "Company goal deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudGoalCompany GetGoalsCompany(string id)
    {
      try
      {
        GoalsCompany goalsCompany = serviceGoalsCompany.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudGoalCompany()
        {

          _id = goalsCompany._id,
          GoalsPeriod = goalsCompany.GoalsPeriod,
          Company = goalsCompany.Company,
          GoalsCompanyList = new ViewCrudGoalItem()
          {
            _id = goalsCompany.GoalsCompanyList._id,
            Weight = goalsCompany.GoalsCompanyList.Weight,
            Achievement = goalsCompany.GoalsCompanyList.Achievement,
            Deadline = goalsCompany.GoalsCompanyList.Deadline,
            Goals = serviceGoals.GetAllNewVersion(p => p._id == goalsCompany.GoalsCompanyList.Goals._id).Result.Select(p => new ViewCrudGoal()
            {
              _id = p._id,
              Name = p.Name,
              Concept = p.Concept,
              TypeGoals = p.TypeGoals
            }).FirstOrDefault(),
            Realized = goalsCompany.GoalsCompanyList.Realized,
            Result = goalsCompany.GoalsCompanyList.Result,
            Name = goalsCompany.GoalsCompanyList.Goals.Name,
            Target = goalsCompany.GoalsCompanyList.Target
          }
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewCrudGoalItem> ListGoalsCompany(string idGoalsPeriod, string idCompany, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewCrudGoalItem> detail = serviceGoalsCompany.GetAllNewVersion(p => p.GoalsPeriod._id == idGoalsPeriod
                  && p.Company._id == idCompany && p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Company.Name").Result
          .Select(p => new ViewCrudGoalItem()
          {
            _id = p._id,
            Weight = p.GoalsCompanyList.Weight,
            Achievement = p.GoalsCompanyList.Achievement,
            Deadline = p.GoalsCompanyList.Deadline,
            Goals = serviceGoals.GetAllNewVersion(x => x._id == p.GoalsCompanyList.Goals._id).Result.Select(x => new ViewCrudGoal()
            {
              _id = x._id,
              Name = x.Name,
              Concept = x.Concept,
              TypeGoals = x.TypeGoals
            }).FirstOrDefault(),
            Realized = p.GoalsCompanyList.Realized,
            Result = p.GoalsCompanyList.Result,
            Name = p.GoalsCompanyList.Goals.Name,
            Target = p.GoalsCompanyList.Target
          }).ToList();

        total = serviceGoalsCompany.CountNewVersion(p => p.GoalsPeriod._id == idGoalsPeriod && p.Company._id == idCompany && p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListGoalCompany> ListGoalsCompany(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListGoalCompany> detail = serviceGoalsCompany.GetAllNewVersion(p => p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Company.Name").Result
          .Select(p => new ViewListGoalCompany()
          {
            _id = p._id,
            Company = p.Company,
            GoalsPeriod = p.GoalsPeriod
          }).ToList();
        total = serviceGoalsCompany.CountNewVersion(p => p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion


    #region Manager Goals
    public string NewGoalsManager(ViewCrudGoalManager view)
    {
      try
      {

        GoalsManager goalsManager = new GoalsManager()
        {
          GoalsPeriod = view.GoalsPeriod,
          Manager = view.Manager,
          GoalsManagerList = view.GoalsManagerList == null ? null : new GoalsItem()
          {
            _id = ObjectId.GenerateNewId().ToString(),
            Weight = view.GoalsManagerList.Weight,
            Achievement = view.GoalsManagerList.Achievement,
            Deadline = view.GoalsManagerList.Deadline,
            Goals = new ViewListGoal
            {
              _id = view.GoalsManagerList.Goals._id,
              Name = view.GoalsManagerList.Goals.Name
            },
            Realized = view.GoalsManagerList.Realized,
            Result = view.GoalsManagerList.Result,
            Target = view.GoalsManagerList.Target
          }
        };
        serviceGoalsManager.InsertNewVersion(goalsManager).Wait();
        return "Manager goal added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewGoalsManagerPortal(ViewCrudGoalManagerPortal view)
    {
      try
      {

        view.GoalsManagerList.Goals._id = ObjectId.GenerateNewId().ToString();
        var goal = Get(New(view.GoalsManagerList.Goals));

        GoalsManager goalsManager = new GoalsManager()
        {
          GoalsPeriod = view.GoalsPeriod,
          Manager = view.Manager,
          GoalsManagerList = view.GoalsManagerList == null ? null : new GoalsItem()
          {
            _id = ObjectId.GenerateNewId().ToString(),
            Weight = view.GoalsManagerList.Weight,
            Achievement = view.GoalsManagerList.Achievement,
            Deadline = view.GoalsManagerList.Deadline,
            Goals = new ViewListGoal() { _id = goal._id, Name = goal.Name },
            Realized = view.GoalsManagerList.Realized,
            Result = view.GoalsManagerList.Result,
            Target = view.GoalsManagerList.Target
          }
        };
        serviceGoalsManager.InsertNewVersion(goalsManager).Wait();
        return "Manager goal added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateGoalsManagerPortal(ViewCrudGoalManagerPortal view)
    {
      try
      {
        GoalsManager goalsManager = serviceGoalsManager.GetNewVersion(p => p._id == view._id).Result;
        goalsManager.GoalsPeriod = view.GoalsPeriod;
        goalsManager.Manager = view.Manager;
        Update(view.GoalsManagerList.Goals);
        var goal = Get(view.GoalsManagerList.Goals._id);

        goalsManager.GoalsManagerList = view.GoalsManagerList == null ? null : new GoalsItem()
        {
          _id = view._id,
          Weight = view.GoalsManagerList.Weight,
          Achievement = view.GoalsManagerList.Achievement,
          Deadline = view.GoalsManagerList.Deadline,
          Goals = new ViewListGoal() { _id = goal._id, Name = goal.Name },
          Realized = view.GoalsManagerList.Realized,
          Result = view.GoalsManagerList.Result,
          Target = view.GoalsManagerList.Target
        };
        serviceGoalsManager.Update(goalsManager, null).Wait();
        return "Manager goal altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateGoalsManager(ViewCrudGoalManager view)
    {
      try
      {
        GoalsManager goalsManager = serviceGoalsManager.GetNewVersion(p => p._id == view._id).Result;
        goalsManager.GoalsPeriod = view.GoalsPeriod;
        goalsManager.Manager = view.Manager;
        goalsManager.GoalsManagerList = view.GoalsManagerList == null ? null : new GoalsItem()
        {
          _id = view._id,
          Weight = view.GoalsManagerList.Weight,
          Achievement = view.GoalsManagerList.Achievement,
          Deadline = view.GoalsManagerList.Deadline,
          Goals = new ViewListGoal
          {
            _id = view.GoalsManagerList.Goals._id,
            Name = view.GoalsManagerList.Goals.Name
          },
          Realized = view.GoalsManagerList.Realized,
          Result = view.GoalsManagerList.Result,
          Target = view.GoalsManagerList.Target
        };
        serviceGoalsManager.Update(goalsManager, null).Wait();
        return "Manager goal altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateGoalsManagerAchievement(ViewCrudAchievement view)
    {
      try
      {
        GoalsManager goalsManager = serviceGoalsManager.GetNewVersion(p => p._id == view._id).Result;
        goalsManager.GoalsManagerList.Achievement = view.Achievement;

        serviceGoalsManager.Update(goalsManager, null).Wait();
        return "Company goal altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteGoalsManager(string id)
    {
      try
      {
        GoalsManager goalsManager = serviceGoalsManager.GetNewVersion(p => p._id == id).Result;
        if (goalsManager == null)
          return "Company goal deleted!";

        goalsManager.Status = EnumStatus.Disabled;
        serviceGoalsManager.Update(goalsManager, null).Wait();
        return "Manager goal deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudGoalManager GetGoalsManager(string id)
    {
      try
      {
        GoalsManager goalsManager = serviceGoalsManager.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudGoalManager()
        {
          _id = goalsManager._id,
          GoalsPeriod = goalsManager.GoalsPeriod,
          Manager = goalsManager.Manager,
          GoalsManagerList = new ViewCrudGoalItem()
          {
            _id = goalsManager.GoalsManagerList._id,
            Weight = goalsManager.GoalsManagerList.Weight,
            Achievement = goalsManager.GoalsManagerList.Achievement,
            Deadline = goalsManager.GoalsManagerList.Deadline,
            Goals = serviceGoals.GetAllNewVersion(p => p._id == goalsManager.GoalsManagerList.Goals._id).Result.Select(p => new ViewCrudGoal()
            {
              _id = p._id,
              Name = p.Name,
              Concept = p.Concept,
              TypeGoals = p.TypeGoals
            }).FirstOrDefault(),

            Realized = goalsManager.GoalsManagerList.Realized,
            Result = goalsManager.GoalsManagerList.Result,
            Name = goalsManager.GoalsManagerList.Goals.Name,
            Target = goalsManager.GoalsManagerList.Target
          }
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudGoalManagerPortal GetGoalsManagerPortal(string id)
    {
      try
      {
        GoalsManager goalsManager = serviceGoalsManager.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudGoalManagerPortal()
        {
          _id = goalsManager._id,
          GoalsPeriod = goalsManager.GoalsPeriod,
          Manager = goalsManager.Manager,
          GoalsManagerList = new ViewCrudGoalItemPortal()
          {
            _id = goalsManager.GoalsManagerList._id,
            Weight = goalsManager.GoalsManagerList.Weight,
            Achievement = goalsManager.GoalsManagerList.Achievement,
            Deadline = goalsManager.GoalsManagerList.Deadline,
            Goals = serviceGoals.GetAllNewVersion(p => p._id == goalsManager.GoalsManagerList.Goals._id).Result.Select(p => new ViewCrudGoal()
            {
              _id = p._id,
              Name = p.Name,
              Concept = p.Concept,
              TypeGoals = p.TypeGoals
            }).FirstOrDefault(),

            Realized = goalsManager.GoalsManagerList.Realized,
            Result = goalsManager.GoalsManagerList.Result,
            Name = goalsManager.GoalsManagerList.Goals.Name,
            Target = goalsManager.GoalsManagerList.Target
          }
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListGoalsItem ListGoalsManager(string idGoalsPeriod, string idManager, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        var idCompany = servicePerson.GetAllNewVersion(p => p._id == idManager).Result.FirstOrDefault().Company?._id;
        List<ViewCrudGoalItem> detailCompany = serviceGoalsCompany.GetAllNewVersion(p => p.GoalsPeriod._id == idGoalsPeriod
                  && p.Company._id == idCompany && p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Company.Name").Result
                  .Select(p => new ViewCrudGoalItem()
                  {
                    _id = p._id,
                    Weight = p.GoalsCompanyList.Weight,
                    Achievement = p.GoalsCompanyList.Achievement,
                    Deadline = p.GoalsCompanyList.Deadline,
                    Goals = serviceGoals.GetAllNewVersion(x => x._id == p.GoalsCompanyList.Goals._id).Result.Select(x => new ViewCrudGoal()
                    {
                      _id = x._id,
                      Name = x.Name,
                      Concept = x.Concept,
                      TypeGoals = x.TypeGoals
                    }).FirstOrDefault(),
                    Realized = p.GoalsCompanyList.Realized,
                    Result = p.GoalsCompanyList.Result,
                    Name = p.GoalsCompanyList.Goals.Name,
                    Target = p.GoalsCompanyList.Target
                  }).ToList();

        List<ViewCrudGoalItem> detail = serviceGoalsManager.GetAllNewVersion(p => p.GoalsPeriod._id == idGoalsPeriod
                  && p.Manager._id == idManager && p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Manager.Name").Result
          .Select(p => new ViewCrudGoalItem()
          {
            _id = p._id,
            Weight = p.GoalsManagerList.Weight,
            Achievement = p.GoalsManagerList.Achievement,
            Deadline = p.GoalsManagerList.Deadline,
            Goals = serviceGoals.GetAllNewVersion(x => x._id == p.GoalsManagerList.Goals._id).Result.Select(x => new ViewCrudGoal()
            {
              _id = x._id,
              Name = x.Name,
              Concept = x.Concept,
              TypeGoals = x.TypeGoals
            }).FirstOrDefault(),
            Realized = p.GoalsManagerList.Realized,
            Result = p.GoalsManagerList.Result,
            Name = p.GoalsManagerList.Goals.Name,
            Target = p.GoalsManagerList.Target
          }).ToList();


        ViewListGoalsItem view = new ViewListGoalsItem()
        {
          GoalsCompany = detailCompany,
          GoalsManager = detail
        };

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListGoalManager> ListGoalsManager(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListGoalManager> detail = serviceGoalsManager.GetAllNewVersion(p => p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Manager.Name").Result
          .Select(p => new ViewListGoalManager()
          {
            _id = p._id,
            Manager = p.Manager,
            GoalsPeriod = p.GoalsPeriod
          }).ToList();
        total = serviceGoalsManager.CountNewVersion(p => p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region Person Goals
    public string NewGoalsPersonPortal(ViewCrudGoalPerson view)
    {
      try
      {
        view.GoalsPersonList.Goals._id = ObjectId.GenerateNewId().ToString();
        var goal = Get(New(view.GoalsPersonList.Goals));


        GoalsPerson goalsPerson = new GoalsPerson()
        {
          GoalsPeriod = view.GoalsPeriod,
          Person = view.Person,
          GoalsPersonList = view.GoalsPersonList == null ? null : new GoalsItem()
          {
            _id = ObjectId.GenerateNewId().ToString(),
            Weight = view.GoalsPersonList.Weight,
            Achievement = view.GoalsPersonList.Achievement,
            Deadline = view.GoalsPersonList.Deadline,
            Goals = new ViewListGoal() { _id = goal._id, Name = goal.Name },
            Realized = view.GoalsPersonList.Realized,
            Result = view.GoalsPersonList.Result,
            Target = view.GoalsPersonList.Target
          }
        };
        serviceGoalsPerson.InsertNewVersion(goalsPerson).Wait();
        return "Person goal added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    //public string NewGoalsPerson(ViewCrudGoalPerson view)
    //{
    //  try
    //  {

    //    GoalsPerson goalsPerson = new GoalsPerson()
    //    {
    //      GoalsPeriod = view.GoalsPeriod,
    //      Person = view.Person,
    //      GoalsPersonList = view.GoalsPersonList == null ? null : new GoalsItem()
    //      {
    //        _id = ObjectId.GenerateNewId().ToString(),
    //        Weight = view.GoalsPersonList.Weight,
    //        Achievement = view.GoalsPersonList.Achievement,
    //        Deadline = view.GoalsPersonList.Deadline,
    //        Goals = new ViewListGoal
    //        {
    //          _id = view.GoalsPersonList.Goals._id,
    //          Name = view.GoalsPersonList.Goals.Name
    //        },
    //        Realized = view.GoalsPersonList.Realized,
    //        Result = view.GoalsPersonList.Result,
    //        Target = view.GoalsPersonList.Target
    //      }
    //    };
    //    goalsPerson = serviceGoalsPerson.InsertNewVersion(goalsPerson).Result;
    //    return "Person goal added!";
    //  }
    //  catch (Exception e)
    //  {
    //    throw e;
    //  }
    //}

    //public string UpdateGoalsPerson(ViewCrudGoalPerson view)
    //{
    //  try
    //  {
    //    GoalsPerson goalsPerson = serviceGoalsPerson.GetNewVersion(p => p._id == view._id).Result;
    //    goalsPerson.GoalsPeriod = view.GoalsPeriod;
    //    goalsPerson.Person = view.Person;
    //    goalsPerson.GoalsPersonList = view.GoalsPersonList == null ? null : new GoalsItem()
    //    {
    //      _id = view._id,
    //      _idAccount = _user._idAccount,
    //      Status = EnumStatus.Enabled,
    //      Weight = view.GoalsPersonList.Weight,
    //      Achievement = view.GoalsPersonList.Achievement,
    //      Deadline = view.GoalsPersonList.Deadline,
    //      Goals = new ViewListGoal
    //      {
    //        _id = view.GoalsPersonList.Goals._id,
    //        Name = view.GoalsPersonList.Goals.Name
    //      },
    //      Realized = view.GoalsPersonList.Realized,
    //      Result = view.GoalsPersonList.Result,
    //      Target = view.GoalsPersonList.Target
    //    };
    //    serviceGoalsPerson.Update(goalsPerson, null);
    //    return "Person goal altered!";
    //  }
    //  catch (Exception e)
    //  {
    //    throw e;
    //  }
    //}
    public string UpdateGoalsPersonPortal(ViewCrudGoalPerson view)
    {
      try
      {
        GoalsPerson goalsPerson = serviceGoalsPerson.GetNewVersion(p => p._id == view._id).Result;
        goalsPerson.GoalsPeriod = view.GoalsPeriod;
        goalsPerson.Person = view.Person;
        Update(view.GoalsPersonList.Goals);
        var goal = Get(view.GoalsPersonList.Goals._id);

        goalsPerson.GoalsPersonList = view.GoalsPersonList == null ? null : new GoalsItem()
        {
          _id = view._id,
          Weight = view.GoalsPersonList.Weight,
          Achievement = view.GoalsPersonList.Achievement,
          Deadline = view.GoalsPersonList.Deadline,
          Goals = new ViewListGoal() { _id = goal._id, Name = goal.Name },
          Realized = view.GoalsPersonList.Realized,
          Result = view.GoalsPersonList.Result,
          Target = view.GoalsPersonList.Target
        };
        serviceGoalsPerson.Update(goalsPerson, null).Wait();
        return "Person goal altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateGoalsPersonAchievement(ViewCrudAchievement view)
    {
      try
      {
        GoalsPerson goalsPerson = serviceGoalsPerson.GetNewVersion(p => p._id == view._id).Result;
        goalsPerson.GoalsPersonList.Achievement = view.Achievement;

        serviceGoalsPerson.Update(goalsPerson, null).Wait();
        return "Company goal altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string DeleteGoalsPerson(string id)
    {
      try
      {
        GoalsPerson goalsPerson = serviceGoalsPerson.GetNewVersion(p => p._id == id).Result;
        if (goalsPerson == null)
          return "Company goal deleted!";

        goalsPerson.Status = EnumStatus.Disabled;
        serviceGoalsPerson.Update(goalsPerson, null).Wait();
        return "Person goal deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudGoalPerson GetGoalsPerson(string id)
    {
      try
      {
        GoalsPerson goalsPerson = serviceGoalsPerson.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudGoalPerson()
        {
          _id = goalsPerson._id,
          GoalsPeriod = goalsPerson.GoalsPeriod,
          Person = goalsPerson.Person,
          GoalsPersonList = new ViewCrudGoalItem()
          {
            _id = goalsPerson.GoalsPersonList._id,
            Weight = goalsPerson.GoalsPersonList.Weight,
            Achievement = goalsPerson.GoalsPersonList.Achievement,
            Deadline = goalsPerson.GoalsPersonList.Deadline,
            Goals = serviceGoals.GetAllNewVersion(p => p._id == goalsPerson.GoalsPersonList.Goals._id).Result.Select(p => new ViewCrudGoal()
            {
              _id = p._id,
              Name = p.Name,
              Concept = p.Concept,
              TypeGoals = p.TypeGoals
            }).FirstOrDefault(),

            Realized = goalsPerson.GoalsPersonList.Realized,
            Result = goalsPerson.GoalsPersonList.Result,
            Name = goalsPerson.GoalsPersonList.Goals.Name,
            Target = goalsPerson.GoalsPersonList.Target
          }
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewListGoalsItem ListGoalsPerson(string idGoalsPeriod, string idPerson, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        var person = servicePerson.GetNewVersion(p => p._id == idPerson).Result;
        List<ViewCrudGoalItem> detailCompany = serviceGoalsCompany.GetAllNewVersion(p => p.GoalsPeriod._id == idGoalsPeriod
                  && p.Company._id == person.Company._id && p.Company.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Company.Name").Result
                  .Select(p => new ViewCrudGoalItem()
                  {
                    _id = p._id,
                    Weight = p.GoalsCompanyList.Weight,
                    Achievement = p.GoalsCompanyList.Achievement,
                    Deadline = p.GoalsCompanyList.Deadline,
                    Goals = p.GoalsCompanyList == null ? null : serviceGoals.GetAllNewVersion(x => x._id == p.GoalsCompanyList.Goals._id).Result.Select(x => new ViewCrudGoal()
                    {
                      _id = x._id,
                      Name = x.Name,
                      Concept = x.Concept,
                      TypeGoals = x.TypeGoals
                    }).FirstOrDefault(),
                    Realized = p.GoalsCompanyList.Realized,
                    Result = p.GoalsCompanyList.Result,
                    Name = p.GoalsCompanyList.Goals.Name,
                    Target = p.GoalsCompanyList.Target
                  }).ToList();

        List<ViewCrudGoalItem> detailManager = null;
        if (person.Manager != null)
        {
          detailManager = serviceGoalsManager.GetAllNewVersion(p => p.GoalsPeriod._id == idGoalsPeriod
                    && p.Manager._id == person.Manager._id).Result
                    .Select(p => new ViewCrudGoalItem()
                    {
                      _id = p._id,
                      Weight = p.GoalsManagerList.Weight,
                      Achievement = p.GoalsManagerList.Achievement,
                      Deadline = p.GoalsManagerList.Deadline,
                      Goals = p.GoalsManagerList == null ? null : new ViewCrudGoal()
                      {
                        _id = p.GoalsManagerList.Goals._id,
                        Name = p.GoalsManagerList.Goals.Name
                      },
                      Realized = p.GoalsManagerList.Realized,
                      Result = p.GoalsManagerList.Result,
                      Name = p.GoalsManagerList.Goals.Name,
                      Target = p.GoalsManagerList.Target
                    }).ToList();

          foreach (var item in detailManager)
          {
            var goal = serviceGoals.GetAllNewVersion(p => p._id == item.Goals._id).Result.FirstOrDefault();
            item.Goals.Concept = goal.Concept;
            item.Goals.TypeGoals = goal.TypeGoals;
          }
        }

        List<ViewCrudGoalItem> detail = serviceGoalsPerson.GetAllNewVersion(p => p.GoalsPeriod._id == idGoalsPeriod
                  && p.Person._id == idPerson).Result.ToList()
          .Select(p => new ViewCrudGoalItem()
          {
            _id = p._id,
            Weight = p.GoalsPersonList.Weight,
            Achievement = p.GoalsPersonList.Achievement,
            Deadline = p.GoalsPersonList.Deadline,
            Goals = p.GoalsPersonList == null ? null : serviceGoals.GetAllNewVersion(x => x._id == p.GoalsPersonList.Goals._id).Result.Select(x => new ViewCrudGoal()
            {
              _id = x._id,
              Name = x.Name,
              Concept = x.Concept,
              TypeGoals = x.TypeGoals
            }).FirstOrDefault(),
            Realized = p.GoalsPersonList.Realized,
            Result = p.GoalsPersonList.Result,
            Name = p.GoalsPersonList.Goals.Name,
            Target = p.GoalsPersonList.Target
          }).ToList();

        double dist = 100 / detail.Count();
        double totalPoints = detail.Sum(p => p.Weight * dist);
        foreach (var item in detail)
        {
          var value = ((dist * item.Weight) / totalPoints) * 100;
          var achievement = GetAchievement(item.Achievement);
          item.Points = Math.Round((value * achievement), 2);
        };



        ViewListGoalsItem view = new ViewListGoalsItem()
        {
          GoalsCompany = detailCompany,
          GoalsManager = detailManager,
          GoalsPerson = detail,
        };

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListGoalPerson> ListGoalsPerson(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListGoalPerson> detail = serviceGoalsPerson.GetAllNewVersion(p => p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Person.Name").Result
          .Select(p => new ViewListGoalPerson()
          {
            _id = p._id,
            Person = p.Person,
            GoalsPeriod = p.GoalsPeriod
          }).ToList();
        total = serviceGoalsPerson.CountNewVersion(p => p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListGoalPerson> ListGoalsPerson(string id, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListGoalPerson> detail = serviceGoalsPerson.GetAllNewVersion(p => p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Person.Name").Result
          .Select(p => new ViewListGoalPerson()
          {
            _id = p._id,
            Person = p.Person,
            GoalsPeriod = p.GoalsPeriod
          }).ToList();
        total = serviceGoalsPerson.CountNewVersion(p => p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion


    #region Person Control Goals

    public string NewGoalsPersonControl(string idperson, string idperiod)
    {
      try
      {
        var period = serviceGoalsPeriod.GetAllNewVersion(p => p._id == idperiod).Result
          .Select(p => new ViewListGoalPeriod
          {
            _id = p._id,
            Name = p.Name
          }).FirstOrDefault();
        var person = servicePerson.GetNewVersion(p => p._id == idperson).Result;

        GoalsPersonControl goalsPerson = serviceGoalsPersonControl.GetAllNewVersion(p => p.Person._id == person._id & p.GoalsPeriod._id == period._id).Result.FirstOrDefault();

        if (goalsPerson == null)
        {
          goalsPerson = serviceGoalsPersonControl.InsertNewVersion(goalsPerson = new GoalsPersonControl()
          {
            GoalsPeriod = period,
            Person = person.GetViewListBaseManager(),
            StatusGoalsPerson = EnumStatusGoalsPerson.Open
          }).Result;

          if (_user._idUser == person.User._id)
          {
            goalsPerson.DateBeginPerson = DateTime.Now;
            goalsPerson.StatusGoalsPerson = EnumStatusGoalsPerson.InProgressPerson;
          }
          else
          {
            goalsPerson.DateBeginManager = DateTime.Now;
            goalsPerson.StatusGoalsPerson = EnumStatusGoalsPerson.InProgressManager;
          }

          serviceGoalsPersonControl.Update(goalsPerson, null).Wait();
        }


        return goalsPerson._id;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateGoalsPersonControl(ViewCrudGoalPersonControl view)
    {
      try
      {
        GoalsPersonControl goalsPerson = serviceGoalsPersonControl.GetNewVersion(p => p.Person._id == view.Person._id & p.GoalsPeriod._id == view.GoalsPeriod._id).Result;
        var person = servicePerson.GetFreeNewVersion(p => p._id == view.Person._id).Result;

        goalsPerson.GoalsPeriod = view.GoalsPeriod;
        goalsPerson.Person = person.GetViewListBaseManager();
        goalsPerson.StatusGoalsPerson = view.StatusGoalsPerson;

        if (person.User._id != _user._idUser)
        {
          if (goalsPerson.StatusGoalsPerson == EnumStatusGoalsPerson.Wait)
          {
            goalsPerson.DateEndManager = DateTime.Now;
            Task.Run(() => Mail(person));
            Task.Run(() => LogSave(_user._idPerson, string.Format("Send person approval | {0}", goalsPerson._id)));
          }
          else
          {
            if (goalsPerson.StatusGoalsPerson == EnumStatusGoalsPerson.End)
            {
              goalsPerson.DateEndEnd = DateTime.Now;
              Task.Run(() => LogSave(_user._idPerson, string.Format("Conclusion process | {0}", goalsPerson._id)));
            }
            else if (goalsPerson.StatusGoalsPerson == EnumStatusGoalsPerson.WaitManager)
            {
              goalsPerson.DateEndPerson = DateTime.Now;
              Task.Run(() => MailManager(person));
              Task.Run(() => LogSave(_user._idPerson, string.Format("Send manager approval | {0}", goalsPerson._id)));

            }
            else if (goalsPerson.StatusGoalsPerson == EnumStatusGoalsPerson.Disapproved)
            {
              Task.Run(() => MailDisapproval(person));
              Task.Run(() => LogSave(_user._idPerson, string.Format("Send manager review | {0}", goalsPerson._id)));
            }

          }

        }

        if (goalsPerson.StatusGoalsPerson == EnumStatusGoalsPerson.End)
        {
          var goalsPersonAvg = serviceGoalsPerson.GetAllNewVersion(p => p.Person._id == goalsPerson.Person._id & p.GoalsPeriod._id == goalsPerson.GoalsPeriod._id).Result.ToList();
          var personGoals = servicePerson.GetNewVersion(p => p._id == goalsPerson.Person._id).Result;

          var goalsCompanyAvg = serviceGoalsCompany.GetAllNewVersion(p => p.Company._id == personGoals.Company._id & p.GoalsPeriod._id == goalsPerson.GoalsPeriod._id).Result.ToList();
          var goalsManager = serviceGoalsManager.GetAllNewVersion(p => p.Manager._id == person.Manager._id & p.GoalsPeriod._id == goalsPerson.GoalsPeriod._id).Result.ToList();


          decimal avgPerson = 0;
          decimal totalPerson = 0;

          decimal avgCompany = 0;
          decimal totalCompany = 0;

          decimal avgManager = 0;
          decimal totalManager = 0;

          if (goalsPersonAvg.Count() > 0)
          {
            foreach (var item in goalsPersonAvg)
            {
              totalPerson += item.GoalsPersonList.Achievement;
            }
            avgPerson = totalPerson / goalsPersonAvg.Count();
          }
          if (goalsCompanyAvg.Count() > 0)
          {
            foreach (var item in goalsCompanyAvg)
            {
              totalCompany += item.GoalsCompanyList.Achievement;
            }
            avgCompany = totalCompany / goalsCompanyAvg.Count();
          }
          if (goalsManager.Count() > 0)
          {
            foreach (var item in goalsManager)
            {
              totalManager += item.GoalsManagerList.Achievement;
            }
            avgManager = totalManager / goalsManager.Count();
          }

          if ((goalsCompanyAvg.Count > 0) & (goalsManager.Count == 0))
            goalsPerson.AchievementEnd = (avgPerson + avgCompany) / 2;
          else if ((goalsCompanyAvg.Count == 0) & (goalsManager.Count > 0))
            goalsPerson.AchievementEnd = (avgPerson + avgManager) / 2;
          else
            goalsPerson.AchievementEnd = (avgPerson + avgCompany + avgManager) / 3;
        }

        serviceGoalsPersonControl.Update(goalsPerson, null).Wait();
        return "Person goal altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public string DeleteGoalsPersonControl(string idperson, string idperiod)
    {
      try
      {
        GoalsPersonControl goalsPerson = serviceGoalsPersonControl.GetNewVersion(p => p.Person._id == idperson & p.GoalsPeriod._id == idperson).Result;
        if (goalsPerson == null)
          return "Company goal deleted!";

        goalsPerson.Status = EnumStatus.Disabled;
        serviceGoalsPersonControl.Update(goalsPerson, null).Wait();
        return "Person goal deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudGoalPersonControl GetGoalsPersonControl(string id)
    {
      try
      {
        GoalsPersonControl goalsPerson = serviceGoalsPersonControl.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudGoalPersonControl()
        {
          _id = goalsPerson._id,
          GoalsPeriod = goalsPerson.GoalsPeriod,
          Person = goalsPerson.Person,
          StatusGoalsPerson = goalsPerson.StatusGoalsPerson
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListGoalPersonControl> ListGoalsPersonControl(string idmanager, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        var viewperiod = serviceGoalsPeriod.GetNewVersion(p => p.ChangeCheck == true).Result;
        if(viewperiod == null)
          return null;

        var period = new ViewListGoalPeriod
        {
          _id = viewperiod?._id,
          Name = viewperiod?.Name
        };

        List<ViewListGoalPersonControl> detail = servicePerson.GetAllNewVersion(p => p.Manager._id == idmanager & p.User.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "User.Name").Result
          .Select(p => new ViewListGoalPersonControl()
          {
            _id = null,
            Person = p.GetViewListBase(),
            GoalsPeriod = period,
            StatusGoalsPerson = EnumStatusGoalsPerson.Open
          }).ToList();

        foreach (var item in detail)
        {
          try
          {
            var goals = serviceGoalsPersonControl.GetAllNewVersion(p => p.Person._id == item.Person._id
          & p.GoalsPeriod._id == item.GoalsPeriod._id).Result.FirstOrDefault();
            if (goals != null)
            {
              item.StatusGoalsPerson = goals.StatusGoalsPerson;
              item._id = goals._id;
            }
          }
          catch (Exception)
          {
          }
        }

        total = servicePerson.CountNewVersion(x=> x.Manager._id == idmanager).Result;

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListGoalPersonControl ListGoalsPersonControlMe(string idperson)
    {
      try
      {
        var viewperiod = serviceGoalsPeriod.GetNewVersion(p => p.ChangeCheck == true).Result;
        if (viewperiod == null)
          return null;

        var period = new ViewListGoalPeriod
        {
          _id = viewperiod._id,
          Name = viewperiod.Name
        };

        var detail = servicePerson.GetNewVersion(p => p._id == idperson).Result;

        ViewListGoalPersonControl view = new ViewListGoalPersonControl()
        {
          _id = null,
          Person = detail.GetViewListBase(),
          GoalsPeriod = period,
          StatusGoalsPerson = EnumStatusGoalsPerson.Open
        };

        try
        {
          var goals = serviceGoalsPersonControl.GetAllNewVersion(p => p.Person._id == view.Person._id
        & p.GoalsPeriod._id == view.GoalsPeriod._id).Result.FirstOrDefault();
          if (goals != null)
          {
            view.StatusGoalsPerson = goals.StatusGoalsPerson;
            view._id = goals._id;
          }
        }
        catch (Exception)
        {

        }


        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    #endregion
  }
}
