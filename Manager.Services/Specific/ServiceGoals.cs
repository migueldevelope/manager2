using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Services.Specific
{
  public class ServiceGoals : Repository<Goals>, IServiceGoals
  {
    private readonly ServiceGeneric<Goals> serviceGoals;
    private readonly ServiceGeneric<GoalsPeriod> serviceGoalsPeriod;
    private readonly ServiceGeneric<GoalsCompany> serviceGoalsCompany;
    private readonly ServiceGeneric<GoalsManager> serviceGoalsManager;
    private readonly ServiceGeneric<GoalsPerson> serviceGoalsPerson;
    private readonly ServiceGeneric<Person> servicePerson;

    #region Constructor
    public ServiceGoals(DataContext context) : base(context)
    {
      try
      {
        serviceGoals = new ServiceGeneric<Goals>(context);
        serviceGoalsCompany = new ServiceGeneric<GoalsCompany>(context);
        serviceGoalsPeriod = new ServiceGeneric<GoalsPeriod>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceGoalsManager = new ServiceGeneric<GoalsManager>(context);
        serviceGoalsPerson = new ServiceGeneric<GoalsPerson>(context);
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
      serviceGoalsCompany._user = _user;
      serviceGoalsPeriod._user = _user;
      servicePerson._user = _user;
      serviceGoalsManager._user = _user;
      serviceGoalsPerson._user = _user;
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
        goal = serviceGoals.InsertNewVersion(goal).Result;
        return "Goal added!";
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
        serviceGoals.Update(goal, null);
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
        Goals item = serviceGoals.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceGoals.Update(item, null);
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
        goalsPeriod = serviceGoalsPeriod.InsertNewVersion(goalsPeriod).Result;
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
        serviceGoalsPeriod.Update(goalsPeriod, null);
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

        item.Status = EnumStatus.Disabled;
        serviceGoalsPeriod.Update(item, null);
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
            Weight = view.GoalsCompanyList.Weight,
            Achievement = view.GoalsCompanyList.Achievement,
            Deadline = view.GoalsCompanyList.Deadline,
            Goals = view.GoalsCompanyList.Goals,
            Realized = view.GoalsCompanyList.Realized,
            Result = view.GoalsCompanyList.Result,
            Target = view.GoalsCompanyList.Target
          }
        };
        goalsCompany = serviceGoalsCompany.InsertNewVersion(goalsCompany).Result;
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
          _idAccount = _user._idAccount,
          Status = EnumStatus.Enabled,
          Weight = view.GoalsCompanyList.Weight,
          Achievement = view.GoalsCompanyList.Achievement,
          Deadline = view.GoalsCompanyList.Deadline,
          Goals = view.GoalsCompanyList.Goals,
          Realized = view.GoalsCompanyList.Realized,
          Result = view.GoalsCompanyList.Result,
          Target = view.GoalsCompanyList.Target
        };
        serviceGoalsCompany.Update(goalsCompany, null);
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
        serviceGoalsCompany.Update(goalsCompany, null);
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
          GoalsPeriod = goalsCompany.GoalsPeriod,
          Company = goalsCompany.Company,
          GoalsCompanyList = new ViewCrudGoalItem()
          {
            _id = goalsCompany.GoalsCompanyList._id,
            Weight = goalsCompany.GoalsCompanyList.Weight,
            Achievement = goalsCompany.GoalsCompanyList.Achievement,
            Deadline = goalsCompany.GoalsCompanyList.Deadline,
            Goals = goalsCompany.GoalsCompanyList.Goals,
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
            Goals = p.GoalsCompanyList.Goals,
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
            Weight = view.GoalsManagerList.Weight,
            Achievement = view.GoalsManagerList.Achievement,
            Deadline = view.GoalsManagerList.Deadline,
            Goals = view.GoalsManagerList.Goals,
            Realized = view.GoalsManagerList.Realized,
            Result = view.GoalsManagerList.Result,
            Target = view.GoalsManagerList.Target
          }
        };
        goalsManager = serviceGoalsManager.InsertNewVersion(goalsManager).Result;
        return "Manager goal added!";
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
          _idAccount = _user._idAccount,
          Status = EnumStatus.Enabled,
          Weight = view.GoalsManagerList.Weight,
          Achievement = view.GoalsManagerList.Achievement,
          Deadline = view.GoalsManagerList.Deadline,
          Goals = view.GoalsManagerList.Goals,
          Realized = view.GoalsManagerList.Realized,
          Result = view.GoalsManagerList.Result,
          Target = view.GoalsManagerList.Target
        };
        serviceGoalsManager.Update(goalsManager, null);
        return "Manager goal altered!";
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
        GoalsManager goalsManager = serviceGoalsManager.GetNewVersion(p => p.GoalsManagerList._id == id).Result;
        if (goalsManager == null)
          return "Company goal deleted!";

        goalsManager.Status = EnumStatus.Disabled;
        serviceGoalsManager.Update(goalsManager, null);
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
          GoalsPeriod = goalsManager.GoalsPeriod,
          Manager = goalsManager.Manager,
          GoalsManagerList = new ViewCrudGoalItem()
          {
            _id = goalsManager.GoalsManagerList._id,
            Weight = goalsManager.GoalsManagerList.Weight,
            Achievement = goalsManager.GoalsManagerList.Achievement,
            Deadline = goalsManager.GoalsManagerList.Deadline,
            Goals = goalsManager.GoalsManagerList.Goals,
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
    public List<ViewCrudGoalItem> ListGoalsManager(string idGoalsPeriod, string idManager, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewCrudGoalItem> detail = serviceGoalsManager.GetAllNewVersion(p => p.GoalsPeriod._id == idGoalsPeriod
                  && p.Manager._id == idManager && p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Manager.Name").Result
          .Select(p => new ViewCrudGoalItem()
          {
            _id = p._id,
            Weight = p.GoalsManagerList.Weight,
            Achievement = p.GoalsManagerList.Achievement,
            Deadline = p.GoalsManagerList.Deadline,
            Goals = p.GoalsManagerList.Goals,
            Realized = p.GoalsManagerList.Realized,
            Result = p.GoalsManagerList.Result,
            Name = p.GoalsManagerList.Goals.Name,
            Target = p.GoalsManagerList.Target
          }).ToList();

        total = serviceGoalsManager.CountNewVersion(p => p.GoalsPeriod._id == idGoalsPeriod && p.Manager._id == idManager && p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
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
    public string NewGoalsPerson(ViewCrudGoalPerson view)
    {
      try
      {

        GoalsPerson goalsPerson = new GoalsPerson()
        {
          GoalsPeriod = view.GoalsPeriod,
          Person = view.Person,
          GoalsPersonList = view.GoalsPersonList == null ? null : new GoalsItem()
          {
            Weight = view.GoalsPersonList.Weight,
            Achievement = view.GoalsPersonList.Achievement,
            Deadline = view.GoalsPersonList.Deadline,
            Goals = view.GoalsPersonList.Goals,
            Realized = view.GoalsPersonList.Realized,
            Result = view.GoalsPersonList.Result,
            Target = view.GoalsPersonList.Target
          }
        };
        goalsPerson = serviceGoalsPerson.InsertNewVersion(goalsPerson).Result;
        return "Person goal added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateGoalsPerson(ViewCrudGoalPerson view)
    {
      try
      {
        GoalsPerson goalsPerson = serviceGoalsPerson.GetNewVersion(p => p._id == view._id).Result;
        goalsPerson.GoalsPeriod = view.GoalsPeriod;
        goalsPerson.Person = view.Person;
        goalsPerson.GoalsPersonList = view.GoalsPersonList == null ? null : new GoalsItem()
        {
          _id = view._id,
          _idAccount = _user._idAccount,
          Status = EnumStatus.Enabled,
          Weight = view.GoalsPersonList.Weight,
          Achievement = view.GoalsPersonList.Achievement,
          Deadline = view.GoalsPersonList.Deadline,
          Goals = view.GoalsPersonList.Goals,
          Realized = view.GoalsPersonList.Realized,
          Result = view.GoalsPersonList.Result,
          Target = view.GoalsPersonList.Target
        };
        serviceGoalsPerson.Update(goalsPerson, null);
        return "Person goal altered!";
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
        GoalsPerson goalsPerson = serviceGoalsPerson.GetNewVersion(p => p.GoalsPersonList._id == id).Result;
        if (goalsPerson == null)
          return "Company goal deleted!";

        goalsPerson.Status = EnumStatus.Disabled;
        serviceGoalsPerson.Update(goalsPerson, null);
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
          GoalsPeriod = goalsPerson.GoalsPeriod,
          Person = goalsPerson.Person,
          GoalsPersonList = new ViewCrudGoalItem()
          {
            _id = goalsPerson.GoalsPersonList._id,
            Weight = goalsPerson.GoalsPersonList.Weight,
            Achievement = goalsPerson.GoalsPersonList.Achievement,
            Deadline = goalsPerson.GoalsPersonList.Deadline,
            Goals = goalsPerson.GoalsPersonList.Goals,
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
    public List<ViewCrudGoalItem> ListGoalsPerson(string idGoalsPeriod, string idPerson, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewCrudGoalItem> detail = serviceGoalsPerson.GetAllNewVersion(p => p.GoalsPeriod._id == idGoalsPeriod
                  && p.Person._id == idPerson && p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Person.Name").Result
          .Select(p => new ViewCrudGoalItem()
          {
            _id = p._id,
            Weight = p.GoalsPersonList.Weight,
            Achievement = p.GoalsPersonList.Achievement,
            Deadline = p.GoalsPersonList.Deadline,
            Goals = p.GoalsPersonList.Goals,
            Realized = p.GoalsPersonList.Realized,
            Result = p.GoalsPersonList.Result,
            Name = p.GoalsPersonList.Goals.Name,
            Target = p.GoalsPersonList.Target
          }).ToList();

        total = serviceGoalsPerson.CountNewVersion(p => p.GoalsPeriod._id == idGoalsPeriod && p.Person._id == idPerson && p.GoalsPeriod.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
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
    #endregion
  }
}
