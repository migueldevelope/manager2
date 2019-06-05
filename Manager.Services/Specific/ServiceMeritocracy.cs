using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceMeritocracy : Repository<Meritocracy>, IServiceMeritocracy
  {
    private readonly ServiceGeneric<Meritocracy> serviceMeritocracy;
    private readonly ServiceGeneric<MeritocracyScore> serviceMeritocracyScore;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Occupation> serviceOccupation;
    private readonly ServiceGeneric<SalaryScale> serviceSalaryScale;
    private readonly ServiceGeneric<SalaryScaleScore> serviceSalaryScaleScore;
    private readonly ServiceGeneric<GoalsPersonControl> serviceGoalsPersonControl;
    private readonly ServiceGeneric<Maturity> serviceMaturity;
    private readonly ServiceLog serviceLog;
    private readonly ServiceLogMessages serviceLogMessages;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceMailModel serviceMailModel;

    #region Constructor
    public ServiceMeritocracy(DataContext context, DataContext contextLog) : base(context)
    {
      try
      {
        serviceMeritocracy = new ServiceGeneric<Meritocracy>(context);
        serviceMeritocracyScore = new ServiceGeneric<MeritocracyScore>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceSalaryScale = new ServiceGeneric<SalaryScale>(context);
        serviceSalaryScaleScore = new ServiceGeneric<SalaryScaleScore>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        serviceGoalsPersonControl = new ServiceGeneric<GoalsPersonControl>(context);
        serviceMaturity = new ServiceGeneric<Maturity>(context);
        serviceLog = new ServiceLog(context, contextLog);
        serviceLogMessages = new ServiceLogMessages(context);
        serviceMail = new ServiceGeneric<MailLog>(context);
        serviceMailModel = new ServiceMailModel(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceMeritocracy._user = _user;
      serviceMeritocracyScore._user = _user;
      servicePerson._user = _user;
      serviceSalaryScale._user = _user;
      serviceSalaryScaleScore._user = _user;
      serviceOccupation._user = _user;
      serviceGoalsPersonControl._user = _user;
      serviceMaturity._user = _user;
      serviceLog.SetUser(_user);
      serviceLogMessages.SetUser(_user);
      serviceMail._user = _user;
      serviceMailModel.SetUser(_user);
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceMeritocracy._user = user;
      serviceMeritocracyScore._user = user;
      servicePerson._user = user;
      serviceSalaryScale._user = user;
      serviceSalaryScaleScore._user = user;
      serviceOccupation._user = user;
      serviceGoalsPersonControl._user = user;
      serviceMaturity._user = user;
      serviceLog.SetUser(_user);
      serviceLogMessages.SetUser(_user);
      serviceMail._user = _user;
      serviceMailModel.SetUser(_user);
    }

    private async Task<string> NewSalaryScaleScore()
    {
      try
      {
        for (byte step = 0; step <= 7; step++)
        {
          for (byte countSteps = 1; countSteps <= 8; countSteps++)
          {
            await serviceSalaryScaleScore.InsertFreeNewVersion(new SalaryScaleScore()
            {
              CountSteps = countSteps,
              Step = (EnumSteps)step,
              Value = decimal.Parse("0")
            });
          }
        }

        return "SalaryScaleScore added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private string NewSalaryScaleScore(ViewCrudSalaryScaleScore view)
    {
      try
      {
        SalaryScaleScore salaryScaleScore = serviceSalaryScaleScore.InsertFreeNewVersion(
          new SalaryScaleScore()
          {
            _id = view._id,
            CountSteps = view.CountSteps,
            Step = view.Step,
            Value = view.Value
          }).Result;
        return "SalaryScaleScore added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region private

    private bool ValidCompanyDate(Meritocracy meritocracy, bool enabled)
    {
      try
      {
        if ((enabled) && meritocracy.Person.CompanyDate == null)
          return false;
        return true;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private bool ValidOccupationDate(Meritocracy meritocracy, bool enabled)
    {
      try
      {
        if ((enabled) && meritocracy.Person.OccupationDate == null)
          return false;
        return true;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private bool ValidSchooling(Meritocracy meritocracy, bool enabled)
    {
      try
      {
        if ((enabled) && meritocracy.Person.CurrentSchooling == null)
          return false;
        return true;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private bool ValidActivitiesExcellence(Meritocracy meritocracy, bool enabled)
    {
      try
      {
        if (enabled)
        {
          foreach (var item in meritocracy.MeritocracyActivities)
          {
            if (item.Mark == 0)
              return false;
          }
        }

        return true;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    private async Task LogSave(string iduser, string local)
    {
      try
      {
        var user = servicePerson.GetAll(p => p._id == iduser).FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "Access Meritocracy",
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
    private async Task MathMeritocracy(Meritocracy meritocracy)
    {
      try
      {
        var person = servicePerson.GetAllNewVersion(p => p._id == meritocracy.Person._id).Result.FirstOrDefault();

        byte schoolingWeight = 0;
        byte companyDateWeight = 0;
        byte occupationDateWeight = 0;
        byte maturityWeight = 0;
        EnumMeritocracyGoals goalsWeight = EnumMeritocracyGoals.NotReach;

        if (person.User.Schooling != null)
        {
          //schooling
          var schoolingResult = person.User.Schooling.Order - person.Occupation.Schooling.Where(p => p.Type == EnumTypeSchooling.Basic).FirstOrDefault().Order;
          if (schoolingResult <= -2)
            schoolingWeight = 1;
          else if (schoolingResult == -1)
            schoolingWeight = 2;
          else if (schoolingResult == 0)
            schoolingWeight = 3;
          else if (schoolingResult == 1)
            schoolingWeight = 4;
          else
            schoolingWeight = 5;
        }


        if (person.User.DateAdm != null)
        {
          //company time
          var companyTimeResult = ((12 * (DateTime.Now.Year - person.User.DateAdm.Value.Year)) + (DateTime.Now.Month - person.User.DateAdm.Value.Month));
          if (companyTimeResult < 13)
            companyDateWeight = 1;
          else if ((companyTimeResult >= 13) && (companyTimeResult <= 24))
            companyDateWeight = 2;
          else if ((companyTimeResult >= 25) && (companyTimeResult <= 48))
            companyDateWeight = 3;
          else if ((companyTimeResult >= 49) && (companyTimeResult <= 84))
            companyDateWeight = 4;
          else
            companyDateWeight = 5;
        }


        if (person.DateLastOccupation != null)
        {
          //occupation time
          var occupationTimeResult = ((12 * (DateTime.Now.Year - person.DateLastOccupation.Value.Year)) + (DateTime.Now.Month - person.DateLastOccupation.Value.Month));
          if (occupationTimeResult < 7)
            occupationDateWeight = 1;
          else if ((occupationTimeResult >= 7) && (occupationTimeResult <= 12))
            occupationDateWeight = 2;
          else if ((occupationTimeResult >= 13) && (occupationTimeResult <= 24))
            occupationDateWeight = 3;
          else if ((occupationTimeResult >= 25) && (occupationTimeResult <= 48))
            occupationDateWeight = 4;
          else
            occupationDateWeight = 5;
        }


        //goals
        var goals = serviceGoalsPersonControl.GetAllNewVersion(p => p.Person._id == person._id & p.StatusGoalsPerson == EnumStatusGoalsPerson.End).Result.LastOrDefault();
        if (goals != null)
        {
          if (goals.AchievementEnd < 100)
            goalsWeight = EnumMeritocracyGoals.NotReach;
          else if ((goals.AchievementEnd >= 100) & (goals.AchievementEnd < 120))
            goalsWeight = EnumMeritocracyGoals.Reached;
          else
            goalsWeight = EnumMeritocracyGoals.Best;
        }

        //maturity
        var maturity = serviceMaturity.GetAll(p => p._idPerson == person._id).FirstOrDefault();
        if (maturity != null)
          maturityWeight = maturity.Value;



        meritocracy.WeightSchooling = schoolingWeight;
        meritocracy.WeightCompanyDate = companyDateWeight;
        meritocracy.WeightOccupationDate = occupationDateWeight;
        meritocracy.WeightMaturity = maturityWeight;
        meritocracy.WeightGoals = goalsWeight;

        serviceMeritocracy.Update(meritocracy, null);

        Task.Run(() => EndMath(meritocracy));
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private decimal ResultLevel(byte level, decimal point)
    {
      try
      {
        switch (level)
        {
          case 1:
            return 0;
          case 2:
            return decimal.Parse((double.Parse(point.ToString()) * 0.8).ToString());
          case 3:
            return decimal.Parse((double.Parse(point.ToString()) * 0.9333).ToString());
          case 4:
            return decimal.Parse((double.Parse(point.ToString()) * 1.066).ToString());
          case 5:
            return decimal.Parse((double.Parse(point.ToString()) * 1.2).ToString());
        }

        return 0;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task EndMath(Meritocracy meritocracy)
    {
      try
      {
        var score = serviceMeritocracyScore.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result.FirstOrDefault();
        decimal percMaturity = ResultLevel(meritocracy.WeightMaturity, score.WeightMaturity);
        decimal percSchooling = ResultLevel(meritocracy.WeightSchooling, score.WeightSchooling);
        decimal percCompanyDate = ResultLevel(meritocracy.WeightCompanyDate, score.WeightCompanyDate);
        decimal percOccupationDate = ResultLevel(meritocracy.WeightOccupationDate, score.WeightOccupationDate);
        decimal percGoals = meritocracy.WeightGoals == EnumMeritocracyGoals.NotReach ? 80 : meritocracy.WeightGoals == EnumMeritocracyGoals.Reached ? 100 : 120;
        decimal percActivitie = ResultLevel(meritocracy.WeightActivitiesExcellence, score.WeightActivitiesExcellence);


        meritocracy.PercentCompanyDate = percCompanyDate;
        meritocracy.PercentOccupationDate = percOccupationDate;
        meritocracy.PercentSchooling = percSchooling;
        meritocracy.PercentActivitiesExcellence = percActivitie;
        if (score.EnabledGoals)
          meritocracy.PercentGoals = percGoals;
        else
          percGoals = 0;

        meritocracy.PercentMaturity = percMaturity;

        meritocracy.ResultEnd = percCompanyDate + percOccupationDate + percSchooling
          + percMaturity + percGoals + percActivitie;

        serviceMeritocracy.Update(meritocracy, null);

        Task.Run(() => MathSalaryScale(meritocracy));
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task MathSalaryScale(Meritocracy meritocracy)
    {
      try
      {

        var resultEnd = meritocracy.ResultEnd;
        EnumSteps resultStep = EnumSteps.A;
        Grade grade = null;

        var person = servicePerson.GetAllNewVersion(p => p._id == meritocracy.Person._id).Result.FirstOrDefault();
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == person.Occupation._id).Result.FirstOrDefault();
        if (occupation.SalaryScales != null)
        {
          var scales = occupation.SalaryScales.Where(p => p._idSalaryScale == person.SalaryScales._idSalaryScale).FirstOrDefault();
          grade = serviceSalaryScale.GetAllNewVersion(p => p._id == scales._idSalaryScale).Result.FirstOrDefault().
            Grades.Where(p => p._id == scales._idGrade).FirstOrDefault();
          var steps = grade.ListSteps;
          var count = steps.Count();
          var scores = serviceSalaryScaleScore.GetAllNewVersion(p => p.CountSteps == count).Result;

          if (count == 2)
          {
            var a = scores.Where(p => p.Step == EnumSteps.A).FirstOrDefault().Value;
            var b = scores.Where(p => p.Step == EnumSteps.B).FirstOrDefault().Value;
            if (resultEnd <= a)
              resultStep = EnumSteps.A;
            else
              resultStep = EnumSteps.B;
          }

          else if (count == 3)
          {
            var a = scores.Where(p => p.Step == EnumSteps.A).FirstOrDefault().Value;
            var b = scores.Where(p => p.Step == EnumSteps.B).FirstOrDefault().Value;
            var c = scores.Where(p => p.Step == EnumSteps.C).FirstOrDefault().Value;

            if (resultEnd <= a)
              resultStep = EnumSteps.A;
            else if ((resultEnd > a) && (resultEnd <= b))
              resultStep = EnumSteps.B;
            else if (resultEnd > b)
              resultStep = EnumSteps.C;
          }

          else if (count == 4)
          {
            var a = scores.Where(p => p.Step == EnumSteps.A).FirstOrDefault().Value;
            var b = scores.Where(p => p.Step == EnumSteps.B).FirstOrDefault().Value;
            var c = scores.Where(p => p.Step == EnumSteps.C).FirstOrDefault().Value;
            var d = scores.Where(p => p.Step == EnumSteps.D).FirstOrDefault().Value;

            if (resultEnd <= a)
              resultStep = EnumSteps.A;
            else if ((resultEnd > a) && (resultEnd <= b))
              resultStep = EnumSteps.B;
            else if ((resultEnd > b) && (resultEnd <= c))
              resultStep = EnumSteps.C;
            else if (resultEnd > c)
              resultStep = EnumSteps.D;
          }

          else if (count == 5)
          {
            var a = scores.Where(p => p.Step == EnumSteps.A).FirstOrDefault().Value;
            var b = scores.Where(p => p.Step == EnumSteps.B).FirstOrDefault().Value;
            var c = scores.Where(p => p.Step == EnumSteps.C).FirstOrDefault().Value;
            var d = scores.Where(p => p.Step == EnumSteps.D).FirstOrDefault().Value;
            var e = scores.Where(p => p.Step == EnumSteps.E).FirstOrDefault().Value;

            if (resultEnd <= a)
              resultStep = EnumSteps.A;
            else if ((resultEnd > a) && (resultEnd <= b))
              resultStep = EnumSteps.B;
            else if ((resultEnd > b) && (resultEnd <= c))
              resultStep = EnumSteps.C;
            else if ((resultEnd > c) && (resultEnd <= d))
              resultStep = EnumSteps.D;
            else if (resultEnd > d)
              resultStep = EnumSteps.E;
          }

          else if (count == 6)
          {
            var a = scores.Where(p => p.Step == EnumSteps.A).FirstOrDefault().Value;
            var b = scores.Where(p => p.Step == EnumSteps.B).FirstOrDefault().Value;
            var c = scores.Where(p => p.Step == EnumSteps.C).FirstOrDefault().Value;
            var d = scores.Where(p => p.Step == EnumSteps.D).FirstOrDefault().Value;
            var e = scores.Where(p => p.Step == EnumSteps.E).FirstOrDefault().Value;
            var f = scores.Where(p => p.Step == EnumSteps.F).FirstOrDefault().Value;

            if (resultEnd <= a)
              resultStep = EnumSteps.A;
            else if ((resultEnd > a) && (resultEnd <= b))
              resultStep = EnumSteps.B;
            else if ((resultEnd > b) && (resultEnd <= c))
              resultStep = EnumSteps.C;
            else if ((resultEnd > c) && (resultEnd <= d))
              resultStep = EnumSteps.D;
            else if ((resultEnd > d) && (resultEnd <= e))
              resultStep = EnumSteps.E;
            else if (resultEnd > e)
              resultStep = EnumSteps.F;
          }


          else if (count == 7)
          {
            var a = scores.Where(p => p.Step == EnumSteps.A).FirstOrDefault().Value;
            var b = scores.Where(p => p.Step == EnumSteps.B).FirstOrDefault().Value;
            var c = scores.Where(p => p.Step == EnumSteps.C).FirstOrDefault().Value;
            var d = scores.Where(p => p.Step == EnumSteps.D).FirstOrDefault().Value;
            var e = scores.Where(p => p.Step == EnumSteps.E).FirstOrDefault().Value;
            var f = scores.Where(p => p.Step == EnumSteps.F).FirstOrDefault().Value;
            var g = scores.Where(p => p.Step == EnumSteps.G).FirstOrDefault().Value;

            if (resultEnd <= a)
              resultStep = EnumSteps.A;
            else if ((resultEnd > a) && (resultEnd <= b))
              resultStep = EnumSteps.B;
            else if ((resultEnd > b) && (resultEnd <= c))
              resultStep = EnumSteps.C;
            else if ((resultEnd > c) && (resultEnd <= d))
              resultStep = EnumSteps.D;
            else if ((resultEnd > d) && (resultEnd <= e))
              resultStep = EnumSteps.E;
            else if ((resultEnd > e) && (resultEnd <= f))
              resultStep = EnumSteps.F;
            else if (resultEnd > f)
              resultStep = EnumSteps.G;
          }

          else if (count == 8)
          {
            var a = scores.Where(p => p.Step == EnumSteps.A).FirstOrDefault().Value;
            var b = scores.Where(p => p.Step == EnumSteps.B).FirstOrDefault().Value;
            var c = scores.Where(p => p.Step == EnumSteps.C).FirstOrDefault().Value;
            var d = scores.Where(p => p.Step == EnumSteps.D).FirstOrDefault().Value;
            var e = scores.Where(p => p.Step == EnumSteps.E).FirstOrDefault().Value;
            var f = scores.Where(p => p.Step == EnumSteps.F).FirstOrDefault().Value;
            var g = scores.Where(p => p.Step == EnumSteps.G).FirstOrDefault().Value;
            var h = scores.Where(p => p.Step == EnumSteps.H).FirstOrDefault().Value;

            if (resultEnd <= a)
              resultStep = EnumSteps.A;
            else if ((resultEnd > a) && (resultEnd <= b))
              resultStep = EnumSteps.B;
            else if ((resultEnd > b) && (resultEnd <= c))
              resultStep = EnumSteps.C;
            else if ((resultEnd > c) && (resultEnd <= d))
              resultStep = EnumSteps.D;
            else if ((resultEnd > d) && (resultEnd <= e))
              resultStep = EnumSteps.E;
            else if ((resultEnd > e) && (resultEnd <= f))
              resultStep = EnumSteps.F;
            else if ((resultEnd > f) && (resultEnd <= g))
              resultStep = EnumSteps.G;
            else if (resultEnd > g)
              resultStep = EnumSteps.H;
          }

        }
        meritocracy.ResultStep = resultStep;

        serviceMeritocracy.Update(meritocracy, null);
      }
      catch (Exception e)
      {

      }
    }

    #endregion

    #region Meritocracy
    public async Task<string> Delete(string id)
    {
      try
      {
        Meritocracy item = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceMeritocracy.Update(item, null);

        Task.Run(() => LogSave(_user._idPerson, string.Format("Remove process | {0}", item._id)));
        return "Meritocracy deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<string> New(string idperson)
    {
      try
      {
        var person = servicePerson.GetAllNewVersion(p => p._id == idperson).Result.FirstOrDefault();
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == person.Occupation._id).Result.FirstOrDefault();

        List<Activitie> activities = null;
        if (occupation != null)
          activities = occupation.Activities;

        var meritocracy = serviceMeritocracy.GetAllNewVersion(p => p.Person._id == idperson && p.StatusMeritocracy != EnumStatusMeritocracy.End).Result.FirstOrDefault();

        if (meritocracy == null)
        {
          Task.Run(() => LogSave(_user._idPerson, string.Format("Start new process | {0}", meritocracy._id)));

          meritocracy = serviceMeritocracy.InsertNewVersion(new Meritocracy()
          {
            ActivitiesExcellence = 0,
            Maturity = 0,
            DateBegin = DateTime.Now,
            StatusMeritocracy = EnumStatusMeritocracy.Wait,
            Status = EnumStatus.Enabled,
            Person = new ViewListPersonMeritocracy()
            {
              _id = person._id,
              CompanyDate = person.User.DateAdm,
              OccupationDate = person.DateLastOccupation,
              OccupationName = person.Occupation?.Name,
              Name = person.User.Name,
              CurrentSchooling = person.User.Schooling?.Name,
              Salary = person.Salary,
              OccupationSchooling = person.Occupation?.Schooling.FirstOrDefault()?.Name
            },
            MeritocracyActivities = new List<MeritocracyActivities>()
          }).Result;

          foreach (var item in activities)
            meritocracy.MeritocracyActivities.Add(new MeritocracyActivities()
            {
              Activities = item,
              Mark = 0
            });

          serviceMeritocracy.Update(meritocracy, null);
        }

        Task.Run(() => MathMeritocracy(meritocracy));

        return meritocracy._id;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<string> Update(ViewCrudMeritocracy view)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == view._id).Result;

        Task.Run(() => LogSave(_user._idPerson, string.Format("Update process | {0}", meritocracy._id)));

        meritocracy.StatusMeritocracy = view.StatusMeritocracy;
        if (meritocracy.StatusMeritocracy == EnumStatusMeritocracy.End)
          meritocracy.DateEnd = DateTime.Now;

        serviceMeritocracy.Update(meritocracy, null);
        Task.Run(() => MathMeritocracy(meritocracy));

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<string> UpdateCompanyDate(ViewCrudMeritocracyDate view, string id)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        meritocracy.Person.CompanyDate = view.Date;
        serviceMeritocracy.Update(meritocracy, null);
        Task.Run(() => MathMeritocracy(meritocracy));

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<string> UpdateOccupationDate(ViewCrudMeritocracyDate view, string id)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        meritocracy.Person.OccupationDate = view.Date;
        serviceMeritocracy.Update(meritocracy, null);
        Task.Run(() => MathMeritocracy(meritocracy));

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<string> UpdateOccupationActivitiesExcellence(ViewCrudMeritocracyWeight view, string id)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        meritocracy.ActivitiesExcellence = view.Weight;
        serviceMeritocracy.Update(meritocracy, null);

        Task.Run(() => MathMeritocracy(meritocracy));
        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<string> UpdateOccupationMaturity(ViewCrudMeritocracyWeight view, string id)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        meritocracy.Maturity = view.Weight;
        serviceMeritocracy.Update(meritocracy, null);
        Task.Run(() => MathMeritocracy(meritocracy));

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<ViewCrudMeritocracy> Get(string id)
    {
      try
      {
        MeritocracyScore meritocracyScore = serviceMeritocracyScore.GetNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        MathMeritocracy(meritocracy);
        meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudMeritocracy()
        {
          _id = meritocracy._id,
          ActivitiesExcellence = meritocracy.ActivitiesExcellence,
          Maturity = meritocracy.Maturity,
          Person = meritocracy.Person,
          WeightCompanyDate = meritocracy.WeightCompanyDate,
          WeightOccupationDate = meritocracy.WeightOccupationDate,
          WeightSchooling = meritocracy.WeightSchooling,
          WeightMaturity = meritocracy.WeightMaturity,
          WeightActivitiesExcellence = meritocracy.WeightActivitiesExcellence,
          WeightGoals = meritocracy.WeightGoals,
          EnabledCompanyDate = meritocracyScore.EnabledCompanyDate,
          EnabledOccupationDate = meritocracyScore.EnabledOccupationDate,
          EnabledSchooling = meritocracyScore.EnabledSchooling,
          EnabledMaturity = meritocracyScore.EnabledMaturity,
          EnabledActivitiesExcellence = meritocracyScore.EnabledActivitiesExcellence,
          EnabledGoals = meritocracyScore.EnabledGoals,
          StatusMeritocracy = meritocracy.StatusMeritocracy,
          ValidCompanyDate = ValidCompanyDate(meritocracy, meritocracyScore.EnabledCompanyDate),
          ValidOccupationDate = ValidOccupationDate(meritocracy, meritocracyScore.EnabledOccupationDate),
          ValidActivitiesExcellence = ValidActivitiesExcellence(meritocracy, meritocracyScore.EnabledActivitiesExcellence),
          ValidSchooling = ValidSchooling(meritocracy, meritocracyScore.EnabledSchooling),
          ResultEnd = meritocracy.ResultEnd
        };

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Task<List<ViewListMeritocracy>> List( ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListMeritocracy> detail = serviceMeritocracy.GetAllNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(x => new ViewListMeritocracy()
          {
            _id = x._id,
            Name = x.Person.Name
          }).ToList();
        total = serviceMeritocracy.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return Task.FromResult(detail);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<List<ViewListMeritocracyActivitie>> ListMeritocracyActivitie(string idmeritocracy)
    {
      try
      {
        List<ViewListMeritocracyActivitie> detail = serviceMeritocracy.GetAllNewVersion(p => p._id == idmeritocracy).Result.
          FirstOrDefault().MeritocracyActivities
          .Select(x => new ViewListMeritocracyActivitie()
          {
            Activitie = new ViewListActivitie() { _id = x.Activities._id, Name = x.Activities.Name, Order = x.Activities.Order },
            Mark = x.Mark
          }).ToList();

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<List<ViewListMeritocracy>> ListWaitManager(string idmanager,  string filter, int count, int page)
    {
      try
      {
        List<ViewListMeritocracy> list = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.StatusUser != EnumStatusUser.ErrorIntegration &&
                                        p.TypeUser != EnumTypeUser.Administrator &&
                                        p.Manager._id == idmanager &&
                                        p.User.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "User.Name").Result
                                        .Select(p => new ViewListMeritocracy()
                                        {
                                          _id = string.Empty,
                                          _idPerson = p._id,
                                          Name = p.User.Name,
                                          OccupationName = p.Occupation?.Name,
                                          StatusMeritocracy = EnumStatusMeritocracy.Open,
                                        }).ToList();
        List<ViewListMeritocracy> detail = new List<ViewListMeritocracy>();
        if (serviceMeritocracy.Exists("Meritocracy"))
        {
          Meritocracy meritocracy;
          foreach (var item in list)
          {
            meritocracy = serviceMeritocracy.GetNewVersion(x => x.Person._id == item._idPerson && x.StatusMeritocracy != EnumStatusMeritocracy.End).Result;
            if (meritocracy != null)
            {
              item._id = meritocracy._id;
              item.StatusMeritocracy = meritocracy.StatusMeritocracy;
            }
            else
              meritocracy = serviceMeritocracy.GetNewVersion(x => x.Person._id == item._idPerson && x.StatusMeritocracy == EnumStatusMeritocracy.End).Result;

            detail.Add(item);
          }
        }
        else
          detail = list;

        var total = servicePerson.CountNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.StatusUser != EnumStatusUser.ErrorIntegration &&
                                p.TypeUser != EnumTypeUser.Administrator &&
                                p.Manager._id == idmanager &&
                                p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<string> UpdateActivitieMark(string idmeritocracy, string idactivitie, byte mark)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == idmeritocracy).Result;
        decimal averageActivities = 0;

        foreach (var item in meritocracy.MeritocracyActivities)
        {
          if (item.Activities._id == idactivitie)
            item.Mark = mark;

          averageActivities += item.Mark;
        }

        meritocracy.ActivitiesExcellence = ((averageActivities) / (meritocracy.MeritocracyActivities.Count() == 0 ? 1 : meritocracy.MeritocracyActivities.Count()));

        meritocracy.WeightActivitiesExcellence = byte.Parse(Math.Truncate(meritocracy.ActivitiesExcellence).ToString());
        serviceMeritocracy.Update(meritocracy, null);

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region MeritocracyScore

    public async Task<string> NewMeritocracyScore(ViewCrudMeritocracyScore view)
    {
      try
      {
        MeritocracyScore meritocracyScore = serviceMeritocracyScore.InsertNewVersion(
          new MeritocracyScore()
          {
            _id = view._id,
            EnabledCompanyDate = view.EnabledCompanyDate,
            EnabledOccupationDate = view.EnabledOccupationDate,
            EnabledSchooling = view.EnabledSchooling,
            EnabledActivitiesExcellence = view.EnabledActivitiesExcellence,
            EnabledMaturity = view.EnabledMaturity,
            EnabledGoals = view.EnabledGoals,
            WeightCompanyDate = view.WeightCompanyDate,
            WeightOccupationDate = view.WeightOccupationDate,
            WeightSchooling = view.WeightSchooling,
            WeightActivitiesExcellence = view.WeightActivitiesExcellence,
            WeightMaturity = view.WeightMaturity,
            WeightGoals = view.WeightGoals
          }).Result;
        return "MeritocracyScore added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<string> RemoveMeritocracyScore(string id)
    {
      try
      {
        MeritocracyScore item = serviceMeritocracyScore.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceMeritocracyScore.Update(item, null);
        return "MeritocracyScore deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<string> UpdateMeritocracyScore(ViewCrudMeritocracyScore view)
    {
      try
      {
        MeritocracyScore meritocracyScore = serviceMeritocracyScore.GetNewVersion(p => p._id == view._id).Result;
        meritocracyScore.EnabledCompanyDate = view.EnabledCompanyDate;
        meritocracyScore.EnabledOccupationDate = view.EnabledOccupationDate;
        meritocracyScore.EnabledSchooling = view.EnabledSchooling;
        meritocracyScore.EnabledActivitiesExcellence = view.EnabledActivitiesExcellence;
        meritocracyScore.EnabledMaturity = view.EnabledMaturity;
        meritocracyScore.EnabledGoals = view.EnabledGoals;
        meritocracyScore.WeightCompanyDate = view.WeightCompanyDate;
        meritocracyScore.WeightOccupationDate = view.WeightOccupationDate;
        meritocracyScore.WeightSchooling = view.WeightSchooling;
        meritocracyScore.WeightActivitiesExcellence = view.WeightActivitiesExcellence;
        meritocracyScore.WeightMaturity = view.WeightMaturity;
        meritocracyScore.WeightGoals = view.WeightGoals;
        serviceMeritocracyScore.Update(meritocracyScore, null);
        return "MeritocracyScore altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<ViewCrudMeritocracyScore> GetMeritocracyScore(string id)
    {
      try
      {
        MeritocracyScore meritocracyScore = serviceMeritocracyScore.GetNewVersion(p => p._id == id).Result;

        return new ViewCrudMeritocracyScore()
        {
          _id = meritocracyScore._id,
          EnabledCompanyDate = meritocracyScore.EnabledCompanyDate,
          EnabledOccupationDate = meritocracyScore.EnabledOccupationDate,
          EnabledSchooling = meritocracyScore.EnabledSchooling,
          EnabledActivitiesExcellence = meritocracyScore.EnabledActivitiesExcellence,
          EnabledMaturity = meritocracyScore.EnabledMaturity,
          EnabledGoals = meritocracyScore.EnabledGoals,
          WeightCompanyDate = meritocracyScore.WeightCompanyDate,
          WeightOccupationDate = meritocracyScore.WeightOccupationDate,
          WeightSchooling = meritocracyScore.WeightSchooling,
          WeightActivitiesExcellence = meritocracyScore.WeightActivitiesExcellence,
          WeightMaturity = meritocracyScore.WeightMaturity,
          WeightGoals = meritocracyScore.WeightGoals
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<ViewListMeritocracyScore> ListMeritocracyScore()
    {
      try
      {
        ViewListMeritocracyScore detail = serviceMeritocracyScore.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result
          .Select(p => new ViewListMeritocracyScore
          {
            _id = p._id,
            EnabledCompanyDate = p.EnabledCompanyDate,
            EnabledOccupationDate = p.EnabledOccupationDate,
            EnabledSchooling = p.EnabledSchooling,
            EnabledActivitiesExcellence = p.EnabledActivitiesExcellence,
            EnabledMaturity = p.EnabledMaturity,
            EnabledGoals = p.EnabledGoals,
            WeightCompanyDate = p.WeightCompanyDate,
            WeightOccupationDate = p.WeightOccupationDate,
            WeightSchooling = p.WeightSchooling,
            WeightActivitiesExcellence = p.WeightActivitiesExcellence,
            WeightMaturity = p.WeightMaturity,
            WeightGoals = p.WeightGoals
          }).FirstOrDefault();
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region SalaryScaleScore
    public async Task<string> DeleteSalaryScaleScore(string id)
    {
      try
      {
        SalaryScaleScore item = serviceSalaryScaleScore.GetFreeNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceSalaryScaleScore.UpdateAccount(item, null);
        return "SalaryScaleScore deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<string> UpdateSalaryScaleScore(ViewCrudSalaryScaleScore view)
    {
      try
      {
        SalaryScaleScore salaryScaleScore = serviceSalaryScaleScore.GetFreeNewVersion(p => p._id == view._id).Result;
        salaryScaleScore.CountSteps = view.CountSteps;
        salaryScaleScore.Step = view.Step;
        salaryScaleScore.Value = view.Value;
        serviceSalaryScaleScore.UpdateAccount(salaryScaleScore, null);
        return "SalaryScaleScore altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<ViewCrudSalaryScaleScore> GetSalaryScaleScore(string id)
    {
      try
      {
        SalaryScaleScore salaryScaleScore = serviceSalaryScaleScore.GetFreeNewVersion(p => p._id == id).Result;
        return new ViewCrudSalaryScaleScore()
        {
          _id = salaryScaleScore._id,
          CountSteps = salaryScaleScore.CountSteps,
          Step = salaryScaleScore.Step,
          Value = salaryScaleScore.Value
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public Task<List<ViewCrudSalaryScaleScore>> ListSalaryScaleScore( ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        if (!serviceSalaryScaleScore.Exists("SalaryScaleScore"))
          NewSalaryScaleScore();

        List<ViewCrudSalaryScaleScore> detail = serviceSalaryScaleScore.GetAllFreeNewVersion(p => p.Status == EnumStatus.Enabled, count, count * (page - 1), "_id").Result
          .Select(p => new ViewCrudSalaryScaleScore
          {
            _id = p._id,
            CountSteps = p.CountSteps,
            Step = p.Step,
            Value = p.Value,
          }).ToList();
        total = serviceSalaryScaleScore.CountNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        return Task.FromResult(detail);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion
  }
}
