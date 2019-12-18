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
    private readonly ServiceGeneric<Schooling> serviceSchooling;
    private readonly ServiceLog serviceLog;
    private readonly ServiceLogMessages serviceLogMessages;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<User> serviceUser;
    private readonly ServiceGeneric<MeritocracyNameLevel> serviceMeritocracyNameLevel;


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
        serviceSchooling = new ServiceGeneric<Schooling>(context);
        serviceUser = new ServiceGeneric<User>(context);
        serviceMeritocracyNameLevel = new ServiceGeneric<MeritocracyNameLevel>(context);
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
      serviceSchooling._user = _user;
      serviceUser._user = _user;
      serviceMeritocracyNameLevel._user = _user;
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
      serviceMail._user = user;
      serviceSchooling._user = _user;
      serviceUser._user = _user;
      serviceMeritocracyNameLevel._user = _user;
      serviceMailModel.SetUser(_user);
    }

    private string NewSalaryScaleScore()
    {
      try
      {
        for (byte step = 0; step <= 7; step++)
        {
          for (byte countSteps = 1; countSteps <= 8; countSteps++)
          {
            serviceSalaryScaleScore.InsertFreeNewVersion(new SalaryScaleScore()
            {
              CountSteps = countSteps,
              Step = (EnumSteps)step,
              Value = decimal.Parse("0")
            }).Wait();
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

    private void GradeScale(Meritocracy meritocracy)
    {
      try
      {
        var person = servicePerson.GetAllNewVersion(p => p._id == meritocracy.Person._id).Result.FirstOrDefault();
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == person.Occupation._id).Result.FirstOrDefault();
        if ((occupation.SalaryScales != null) && (person.SalaryScales != null))
        {
          var grades = occupation.SalaryScales.Where(p => p._idSalaryScale == person.SalaryScales._idSalaryScale).FirstOrDefault();
          var salaryscale = serviceSalaryScale.GetAllNewVersion(p => p._id == person.SalaryScales._idSalaryScale).Result.FirstOrDefault();
          var grade = salaryscale.Grades.Where(p => p._id == grades._idGrade).FirstOrDefault();
          meritocracy.GradeScale = grade;
          foreach (var item in grade.ListSteps.OrderBy(p => p.Step))
          {
            if (person.Salary <= item.Salary)
            {
              meritocracy.ResultStepScale = item.Step;
              serviceMeritocracy.Update(meritocracy, null).Wait();
              return;
            }
          }

        }

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
        var person = servicePerson.GetNewVersion(p => p._id == meritocracy.Person._id).Result;
        var user = serviceUser.GetNewVersion(p => p._id == person.User._id).Result;
        if (meritocracy.Person.CurrentSchooling == null)
          meritocracy.Person.CurrentSchooling = user.Schooling?.Name;

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
          if (meritocracy.MeritocracyActivities.Count == 0)
            return false;

          var count = 0;
          foreach (var item in meritocracy.MeritocracyActivities)
          {
            if (item.Mark > 0)
              count += 1;
          }
          if (count == 0)
            return false;

        }

        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }


    private void LogSave(string iduser, string local)
    {
      try
      {
        var user = servicePerson.GetAllNewVersion(p => p._id == iduser).Result.FirstOrDefault();
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
    private void MathMeritocracy(Meritocracy meritocracy)
    {
      try
      {
        var person = servicePerson.GetAllNewVersion(p => p._id == meritocracy.Person._id).Result.FirstOrDefault();
        if ((person.User.DateAdm == null) && (meritocracy.Person.CompanyDate != null))
        {
          person.User.DateAdm = meritocracy.Person.CompanyDate;
          servicePerson.Update(person, null).Wait();
        }
        if ((person.DateLastOccupation == null) && (meritocracy.Person.OccupationDate != null))
        {
          person.DateLastOccupation = meritocracy.Person.OccupationDate;
          servicePerson.Update(person, null).Wait();
        }


        byte schoolingWeight = 0;
        byte companyDateWeight = 0;
        byte occupationDateWeight = 0;
        byte maturityWeight = 0;
        EnumMeritocracyGoals goalsWeight = EnumMeritocracyGoals.NotReach;

        if (person.User.Schooling != null)
        {
          var schoolingPerson = serviceSchooling.GetAllNewVersion(p => p._id == person.User.Schooling._id).Result.FirstOrDefault();
          var occupation = serviceOccupation.GetAllNewVersion(p => p._id == person.Occupation._id).Result.FirstOrDefault().Schooling.Where(x => x.Type == EnumTypeSchooling.Basic).FirstOrDefault()._id;
          //var idschooling = occupation.Schooling.Where(x => x.Type == EnumTypeSchooling.Basic).FirstOrDefault()._id;
          var schoolingOccupation = serviceSchooling.GetAllFreeNewVersion(p => p._id == occupation).Result.FirstOrDefault();


          //schooling
          var schoolingResult = schoolingPerson.Order - schoolingOccupation.Order;
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
          if (goals.AchievementEnd == 0)
            goalsWeight = EnumMeritocracyGoals.NotReach;
          else if (goals.AchievementEnd < 100)
            goalsWeight = EnumMeritocracyGoals.Partial;
          else if ((goals.AchievementEnd >= 100) & (goals.AchievementEnd < 120))
            goalsWeight = EnumMeritocracyGoals.Reached;
          else
            goalsWeight = EnumMeritocracyGoals.Best;
        }

        //maturity
        var maturity = serviceMaturity.GetAllNewVersion(p => p._idPerson == person._id).Result.FirstOrDefault();
        if (maturity != null)
          maturityWeight = maturity.Value;



        meritocracy.WeightSchooling = schoolingWeight;
        meritocracy.WeightCompanyDate = companyDateWeight;
        meritocracy.WeightOccupationDate = occupationDateWeight;
        meritocracy.WeightMaturity = maturityWeight;
        meritocracy.WeightGoals = goalsWeight;


        serviceMeritocracy.Update(meritocracy, null).Wait();

        Task.Run(() => EndMath(meritocracy));
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    //private decimal ResultLevel(byte level, decimal point)
    //{
    //  try
    //  {
    //    switch (level)
    //    {
    //      case 1:
    //        return 0;
    //      case 2:
    //        return decimal.Parse((double.Parse(point.ToString()) * 0.8).ToString());
    //      case 3:
    //        return decimal.Parse((double.Parse(point.ToString()) * 0.9333).ToString());
    //      case 4:
    //        return decimal.Parse((double.Parse(point.ToString()) * 1.066).ToString());
    //      case 5:
    //        return decimal.Parse((double.Parse(point.ToString()) * 1.2).ToString());
    //    }

    //    return 0;
    //  }
    //  catch (Exception e)
    //  {
    //    throw e;
    //  }
    //}

    private decimal ResultLevel(decimal level, decimal point)
    {
      try
      {
        if (point == 0)
          return 0;

        decimal result = (level * 100) / 5;
        result = decimal.Parse(((result * point) / 100).ToString());
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void EndMath(Meritocracy meritocracy)
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


        meritocracy.Score = score;

        serviceMeritocracy.Update(meritocracy, null).Wait();

        Task.Run(() => MathSalaryScale(meritocracy));
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void MathSalaryScale(Meritocracy meritocracy)
    {
      try
      {

        var resultEnd = meritocracy.ResultEnd;
        EnumSteps resultStep = EnumSteps.A;
        Grade grade = null;
        decimal salarynew = 0;

        var person = servicePerson.GetAllNewVersion(p => p._id == meritocracy.Person._id).Result.FirstOrDefault();
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == person.Occupation._id).Result.FirstOrDefault();
        if (occupation.SalaryScales != null)
        {
          var scales = occupation.SalaryScales.Where(p => p._idSalaryScale == person.SalaryScales._idSalaryScale).FirstOrDefault();
          grade = serviceSalaryScale.GetAllNewVersion(p => p._id == scales._idSalaryScale).Result.FirstOrDefault().
            Grades.Where(p => p._id == scales._idGrade).FirstOrDefault();
          var steps = grade.ListSteps;
          var count = steps.Count();
          var scores = serviceSalaryScaleScore.GetAllFreeNewVersion(p => p.CountSteps == count).Result.ToList();

          var max = scores.Where(p => p.Value > 0).Max(p => p.Value);
          var min = scores.Where(p => p.Value > 0).Min(p => p.Value);
          var diff = max - min;
          resultEnd = ((resultEnd * diff) / 100) + min;


          meritocracy.Grade = grade;

          if (count == 2)
          {
            var a = scores.Where(p => p.Step == EnumSteps.A).FirstOrDefault().Value;
            var b = scores.Where(p => p.Step == EnumSteps.B).FirstOrDefault().Value;
            if (resultEnd <= a)
            {
              resultStep = EnumSteps.A;
            }
            else
            {
              resultStep = EnumSteps.B;
            }

          }

          else if (count == 3)
          {
            var a = scores.Where(p => p.Step == EnumSteps.A).FirstOrDefault().Value;
            var b = scores.Where(p => p.Step == EnumSteps.B).FirstOrDefault().Value;
            var c = scores.Where(p => p.Step == EnumSteps.C).FirstOrDefault().Value;

            if (resultEnd <= a)
            {
              resultStep = EnumSteps.A;
            }
            else if ((resultEnd > a) && (resultEnd <= b))
            {
              resultStep = EnumSteps.B;
            }
            else if (resultEnd > b)
            {
              resultStep = EnumSteps.C;
            }

          }

          else if (count == 4)
          {
            var a = scores.Where(p => p.Step == EnumSteps.A).FirstOrDefault().Value;
            var b = scores.Where(p => p.Step == EnumSteps.B).FirstOrDefault().Value;
            var c = scores.Where(p => p.Step == EnumSteps.C).FirstOrDefault().Value;
            var d = scores.Where(p => p.Step == EnumSteps.D).FirstOrDefault().Value;

            if (resultEnd <= a)
            {
              resultStep = EnumSteps.A;
            }
            else if ((resultEnd > a) && (resultEnd <= b))
            {
              resultStep = EnumSteps.B;
            }
            else if ((resultEnd > b) && (resultEnd <= c))
            {
              resultStep = EnumSteps.C;
            }
            else if (resultEnd > c)
            {
              resultStep = EnumSteps.D;
            }

          }

          else if (count == 5)
          {
            var a = scores.Where(p => p.Step == EnumSteps.A).FirstOrDefault().Value;
            var b = scores.Where(p => p.Step == EnumSteps.B).FirstOrDefault().Value;
            var c = scores.Where(p => p.Step == EnumSteps.C).FirstOrDefault().Value;
            var d = scores.Where(p => p.Step == EnumSteps.D).FirstOrDefault().Value;
            var e = scores.Where(p => p.Step == EnumSteps.E).FirstOrDefault().Value;

            if (resultEnd <= a)
            {
              resultStep = EnumSteps.A;
            }
            else if ((resultEnd > a) && (resultEnd <= b))
            {
              resultStep = EnumSteps.B;
            }
            else if ((resultEnd > b) && (resultEnd <= c))
            {
              resultStep = EnumSteps.C;
            }
            else if ((resultEnd > c) && (resultEnd <= d))
            {
              resultStep = EnumSteps.D;
            }
            else if (resultEnd > d)
            {
              resultStep = EnumSteps.E;
            }

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
            {
              resultStep = EnumSteps.A;
            }
            else if ((resultEnd > a) && (resultEnd <= b))
            {
              resultStep = EnumSteps.B;
            }
            else if ((resultEnd > b) && (resultEnd <= c))
            {
              resultStep = EnumSteps.C;
            }
            else if ((resultEnd > c) && (resultEnd <= d))
            {
              resultStep = EnumSteps.D;
            }
            else if ((resultEnd > d) && (resultEnd <= e))
            {
              resultStep = EnumSteps.E;
            }
            else if (resultEnd > e)
            {
              resultStep = EnumSteps.F;
            }

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
            {
              resultStep = EnumSteps.A;
            }
            else if ((resultEnd > a) && (resultEnd <= b))
            {
              resultStep = EnumSteps.B;
            }
            else if ((resultEnd > b) && (resultEnd <= c))
            {
              resultStep = EnumSteps.C;
            }
            else if ((resultEnd > c) && (resultEnd <= d))
            {
              resultStep = EnumSteps.D;
            }
            else if ((resultEnd > d) && (resultEnd <= e))
            {
              resultStep = EnumSteps.E;
            }
            else if ((resultEnd > e) && (resultEnd <= f))
            {
              resultStep = EnumSteps.F;
            }
            else if (resultEnd > f)
            {
              resultStep = EnumSteps.G;
            }

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
            {
              resultStep = EnumSteps.A;
            }
            else if ((resultEnd > a) && (resultEnd <= b))
            {
              resultStep = EnumSteps.B;
            }
            else if ((resultEnd > b) && (resultEnd <= c))
            {
              resultStep = EnumSteps.C;
            }
            else if ((resultEnd > c) && (resultEnd <= d))
            {
              resultStep = EnumSteps.D;
            }
            else if ((resultEnd > d) && (resultEnd <= e))
            {
              resultStep = EnumSteps.E;
            }
            else if ((resultEnd > e) && (resultEnd <= f))
            {
              resultStep = EnumSteps.F;
            }
            else if ((resultEnd > f) && (resultEnd <= g))
            {
              resultStep = EnumSteps.G;
            }
            else if (resultEnd > g)
            {
              resultStep = EnumSteps.H;
            }

          }

        }
        if (meritocracy.Grade != null)
        {
          salarynew = meritocracy.Grade.ListSteps.Where(p => p.Step == resultStep).FirstOrDefault().Salary;
        }
        meritocracy.SalaryNew = salarynew;
        meritocracy.SalaryDifference = salarynew - meritocracy.Person.Salary;
        meritocracy.PercentSalary = decimal.Parse(((double.Parse(meritocracy.SalaryDifference.ToString()) * 100.0) / double.Parse((meritocracy.Person.Salary == 0 ? 1 : meritocracy.Person.Salary).ToString())).ToString());
        meritocracy.ResultStep = resultStep;

        serviceMeritocracy.Update(meritocracy, null).Wait();
      }
      catch (Exception)
      {

      }
    }

    #endregion

    #region Name Level
    public string UpdateMeritocracyNameLevel(ViewCrudMeritocracyNameLevel meritocracyNameLevel)
    {
      try
      {
        var meritocracyname = serviceMeritocracyNameLevel.GetFreeNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        var names = new MeritocracyNameLevel()
        {
          _id = meritocracyname._id,
          NameLevel1 = meritocracyNameLevel.NameLevel1,
          NameLevel2 = meritocracyNameLevel.NameLevel2,
          NameLevel3 = meritocracyNameLevel.NameLevel3,
          NameLevel4 = meritocracyNameLevel.NameLevel4,
          NameLevel5 = meritocracyNameLevel.NameLevel5,
          NameLevel6 = meritocracyNameLevel.NameLevel6,
          NameLevel7 = meritocracyNameLevel.NameLevel7,
          NameLevel8 = meritocracyNameLevel.NameLevel8,
          NameLevel9 = meritocracyNameLevel.NameLevel9,
          NameLevel10 = meritocracyNameLevel.NameLevel10
        };

        if (meritocracyname == null)
          meritocracyname = serviceMeritocracyNameLevel.InsertNewVersion(names).Result;

        var i = serviceMeritocracyNameLevel.Update(names, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }

    }

    public ViewCrudMeritocracyNameLevel GetMeritocracyNameLevel()
    {
      try
      {
        var result = new ViewCrudMeritocracyNameLevel();

        var meritocracyname = serviceMeritocracyNameLevel.GetFreeNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        if (meritocracyname == null)
          meritocracyname = serviceMeritocracyNameLevel.InsertNewVersion(new MeritocracyNameLevel()
          {
            NameLevel1 = "Sol",
            NameLevel2 = "Mercúrio",
            NameLevel3 = "Vênus",
            NameLevel4 = "Terra",
            NameLevel5 = "Marte",
            NameLevel6 = "Júpiter",
            NameLevel7 = "Saturno",
            NameLevel8 = "Urano",
            NameLevel9 = "Netuno",
            NameLevel10 = "Plutão"
          }).Result;
        else
          result = new ViewCrudMeritocracyNameLevel()
          {
            NameLevel1 = meritocracyname.NameLevel1,
            NameLevel2 = meritocracyname.NameLevel2,
            NameLevel3 = meritocracyname.NameLevel3,
            NameLevel4 = meritocracyname.NameLevel4,
            NameLevel5 = meritocracyname.NameLevel5,
            NameLevel6 = meritocracyname.NameLevel6,
            NameLevel7 = meritocracyname.NameLevel7,
            NameLevel8 = meritocracyname.NameLevel8,
            NameLevel9 = meritocracyname.NameLevel9,
            NameLevel10 = meritocracyname.NameLevel10,
            _id = meritocracyname._id
          };


        return result;
      }
      catch (Exception e)
      {
        throw e;
      }

    }
    #endregion

    #region Meritocracy

    public ViewListMeritocracyResume ListMeritocracyPerson(string idperson, ref long total, int count, int page, string filter)
    {
      try
      {
        int skip = (count * (page - 1));
        var item = servicePerson.GetNewVersion(p => p._id == idperson && p.StatusUser != EnumStatusUser.Disabled).Result;

        //var list = new List<ViewListMeritocracyResume>();

        //foreach (var item in persons)
        //{
        var result = serviceMeritocracy.GetAllNewVersion(p => p.Person._id == idperson
        && p.StatusMeritocracy == EnumStatusMeritocracy.End
        && p.ShowPerson == true).Result.LastOrDefault();
        if (result == null)
          return null;
        //foreach (var result in meritocracy)
        //{
        //list.Add(
        var view = new ViewListMeritocracyResume()
        {
          _id = result._id,
          Name = item.User?.Name,
          Manager = item.Manager?.Name,
          Occupation = item.Occupation?.Name,
          ResultEnd = result.ResultEnd,
          Photo = item.User?.PhotoUrl,
          ShowPerson = result.ShowPerson,
          DateEnd = result.DateEnd
        };

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListMeritocracyResume> ListMeritocracy(string idmanager, List<_ViewList> occupations, ref long total, int count, int page, string filter)
    {
      try
      {
        int skip = (count * (page - 1));
        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.Manager._id == idmanager).Result;
        var list = new List<ViewListMeritocracyResume>();

        if (occupations.Count > 0)
        {
          foreach (var occ in occupations)
          {
            foreach (var item in persons)
            {
              if (item.Occupation?._id == occ._id)
              {
                var meritocracy = serviceMeritocracy.GetAllNewVersion(p => p.Person._id == item._id
                && p.StatusMeritocracy == EnumStatusMeritocracy.End).Result;
                if (meritocracy.Count() > 0)
                {
                  var date = meritocracy.Max(p => p.DateEnd);
                  var result = meritocracy.Where(p => p.DateEnd == date).LastOrDefault();
                  list.Add(new ViewListMeritocracyResume()
                  {
                    _id = result._id,
                    Name = item.User?.Name,
                    Manager = item.Manager?.Name,
                    Occupation = item.Occupation?.Name,
                    ResultEnd = result.ResultEnd,
                    Photo = item.User?.PhotoUrl,
                    ShowPerson = result.ShowPerson,
                    DateEnd = result.DateEnd
                  });
                }
              }
            }
          }
        }
        else
        {
          foreach (var item in persons)
          {
            var meritocracy = serviceMeritocracy.GetAllNewVersion(p => p.Person._id == item._id
            && p.StatusMeritocracy == EnumStatusMeritocracy.End).Result;
            if (meritocracy.Count() > 0)
            {
              var date = meritocracy.Max(p => p.DateEnd);
              var result = meritocracy.Where(p => p.DateEnd == date).LastOrDefault();
              list.Add(new ViewListMeritocracyResume()
              {
                _id = result._id,
                Name = item.User?.Name,
                Manager = item.Manager?.Name,
                Occupation = item.Occupation?.Name,
                ResultEnd = result.ResultEnd,
                Photo = item.User?.PhotoUrl,
                ShowPerson = result.ShowPerson,
                DateEnd = result.DateEnd
              });
            }
          }
        }



        total = list.Count();
        list = list.Where(p => p.Name.Contains(filter)).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListMeritocracyResume> ListMeritocracyRH(ViewFilterOccupationsAndManagers view, ref long total, int count, int page, string filter)
    {
      try
      {
        int skip = (count * (page - 1));
        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled).Result;
        var list = new List<ViewListMeritocracyResume>();

        if (view.Managers.Count > 0)
        {
          foreach (var manager in view.Managers)
          {
            foreach (var item in persons)
            {
              if (manager._id == item.Manager?._id)
              {
                item.Status = EnumStatus.Disabled;
              }
            }
          }
          persons = persons.Where(p => p.Status == EnumStatus.Disabled).ToList();
        }

        if (view.Occupations.Count > 0)
        {
          foreach (var occ in view.Occupations)
          {
            foreach (var item in persons)
            {
              if (item.Occupation?._id == occ._id)
              {
                var meritocracy = serviceMeritocracy.GetAllNewVersion(p => p.Person._id == item._id
                && p.StatusMeritocracy == EnumStatusMeritocracy.End).Result;
                if (meritocracy.Count() > 0)
                {
                  var date = meritocracy.Max(p => p.DateEnd);
                  var result = meritocracy.Where(p => p.DateEnd == date).LastOrDefault();
                  list.Add(new ViewListMeritocracyResume()
                  {
                    Name = item.User?.Name,
                    Manager = item.Manager?.Name,
                    Occupation = item.Occupation?.Name,
                    ResultEnd = result.ResultEnd,
                    Photo = item.User?.PhotoUrl,
                    DateEnd = result.DateEnd,
                  });
                }
              }
            }
          }
        }
        else
        {
          foreach (var item in persons)
          {
            var meritocracy = serviceMeritocracy.GetAllNewVersion(p => p.Person._id == item._id
            && p.StatusMeritocracy == EnumStatusMeritocracy.End).Result;
            if (meritocracy.Count() > 0)
            {
              var date = meritocracy.Max(p => p.DateEnd);
              var result = meritocracy.Where(p => p.DateEnd == date).LastOrDefault();
              list.Add(new ViewListMeritocracyResume()
              {
                Name = item.User?.Name,
                Manager = item.Manager?.Name,
                Occupation = item.Occupation?.Name,
                ResultEnd = result.ResultEnd,
                Photo = item.User?.PhotoUrl,
                DateEnd = result.DateEnd,
              });
            }
          }
        }



        total = list.Count();
        list = list.Where(p => p.Name.Contains(filter)).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        return list;
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
        Meritocracy item = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceMeritocracy.Update(item, null).Wait();

        Task.Run(() => LogSave(_user._idPerson, string.Format("Remove process | {0}", item._id)));
        return "Meritocracy deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string New(string idperson)
    {
      try
      {
        var person = servicePerson.GetAllNewVersion(p => p._id == idperson).Result.FirstOrDefault();
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == person.Occupation._id).Result.FirstOrDefault();

        List<ViewListActivitie> activities = null;
        if (occupation != null)
          activities = occupation.Activities;

        var meritocracy = serviceMeritocracy.GetAllNewVersion(p => p.Person._id == idperson && p.StatusMeritocracy != EnumStatusMeritocracy.End).Result.FirstOrDefault();

        if (meritocracy == null)
        {
          Task.Run(() => LogSave(_user._idPerson, string.Format("Start new process | {0}", meritocracy._id)));


          meritocracy = serviceMeritocracy.InsertNewVersion(new Meritocracy()
          {
            ShowPerson = false,
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
              OccupationSchooling = occupation?.Schooling.FirstOrDefault()?.Name
            },
            MeritocracyActivities = new List<MeritocracyActivities>()
          }).Result;

          foreach (var item in activities)
            meritocracy.MeritocracyActivities.Add(new MeritocracyActivities()
            {
              Activities = item,
              Mark = 0
            });

          serviceMeritocracy.Update(meritocracy, null).Wait();
        }

        Task.Run(() => MathMeritocracy(meritocracy));
        Task.Run(() => GradeScale(meritocracy));

        return meritocracy._id;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string End(string id)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;

        Task.Run(() => LogSave(_user._idPerson, string.Format("End process | {0}", meritocracy._id)));

        meritocracy.StatusMeritocracy = EnumStatusMeritocracy.End;
        meritocracy.DateEnd = DateTime.Now;

        serviceMeritocracy.Update(meritocracy, null).Wait();
        Task.Run(() => MathMeritocracy(meritocracy));

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Update(ViewCrudMeritocracy view)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == view._id).Result;

        Task.Run(() => LogSave(_user._idPerson, string.Format("Update process | {0}", meritocracy._id)));

        meritocracy.ShowPerson = view.ShowPerson;
        if (meritocracy.StatusMeritocracy == EnumStatusMeritocracy.End)
          meritocracy.DateEnd = DateTime.Now;

        serviceMeritocracy.Update(meritocracy, null).Wait();
        Task.Run(() => MathMeritocracy(meritocracy));

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateShow(string idmeritocracy, bool showperson)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == idmeritocracy).Result;

        meritocracy.ShowPerson = showperson;

        serviceMeritocracy.Update(meritocracy, null).Wait();

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public string UpdateSchooling(string id, string idschooling)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        var schooling = serviceSchooling.GetNewVersion(p => p._id == idschooling).Result;
        var person = servicePerson.GetNewVersion(p => p._id == meritocracy.Person._id).Result;
        var user = serviceUser.GetNewVersion(p => p._id == person.User._id).Result;

        person.User.Schooling = schooling.GetViewList();
        Task.Run(() => servicePerson.Update(person, null));
        Task.Run(() => serviceUser.Update(user, null));

        meritocracy.Person.CurrentSchooling = schooling?.Name;
        serviceMeritocracy.Update(meritocracy, null).Wait();
        Task.Run(() => MathMeritocracy(meritocracy));

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateCompanyDate(ViewCrudMeritocracyDate view, string id)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        meritocracy.Person.CompanyDate = view.Date;
        serviceMeritocracy.Update(meritocracy, null).Wait();
        Task.Run(() => MathMeritocracy(meritocracy));

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateOccupationDate(ViewCrudMeritocracyDate view, string id)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        meritocracy.Person.OccupationDate = view.Date;
        serviceMeritocracy.Update(meritocracy, null).Wait();
        Task.Run(() => MathMeritocracy(meritocracy));

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateOccupationActivitiesExcellence(ViewCrudMeritocracyWeight view, string id)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        meritocracy.ActivitiesExcellence = view.Weight;
        serviceMeritocracy.Update(meritocracy, null).Wait();

        Task.Run(() => MathMeritocracy(meritocracy));
        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateOccupationMaturity(ViewCrudMeritocracyWeight view, string id)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        meritocracy.Maturity = view.Weight;
        serviceMeritocracy.Update(meritocracy, null).Wait();
        Task.Run(() => MathMeritocracy(meritocracy));

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudMeritocracy Get(string id)
    {
      try
      {
        MeritocracyScore meritocracyScore = serviceMeritocracyScore.GetNewVersion(p => p.Status == EnumStatus.Enabled).Result;

        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        Person person = servicePerson.GetNewVersion(p => p._id == meritocracy.Person._id).Result;
        Occupation occupation = serviceOccupation.GetNewVersion(p => p._id == person.Occupation._id).Result;

        MathMeritocracy(meritocracy);
        GradeScale(meritocracy);
        meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudMeritocracy()
        {
          _id = meritocracy._id,
          ShowPerson = meritocracy.ShowPerson,
          ActivitiesExcellence = meritocracy.ActivitiesExcellence,
          Maturity = meritocracy.Maturity,
          Person = new ViewListPersonMeritocracy()
          {
            _id = person._id,
            CompanyDate = person.User.DateAdm ?? meritocracy.Person.CompanyDate,
            OccupationDate = person.DateLastOccupation ?? meritocracy.Person.OccupationDate,
            OccupationName = person.Occupation?.Name,
            Name = person.User.Name,
            CurrentSchooling = meritocracy.Person.CurrentSchooling ?? person.User.Schooling?.Name,
            Salary = person.Salary,
            OccupationSchooling = occupation?.Schooling.FirstOrDefault()?.Name,
            Photo = person.User?.PhotoUrl
          },
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
          ResultEnd = meritocracy.ResultEnd,
          ResultStep = meritocracy.ResultStep,
          ResultStepScale = meritocracy.ResultStepScale,
          PercentSalary = meritocracy.PercentSalary,
          SalaryDifference = meritocracy.SalaryDifference,
          SalaryNew = meritocracy.SalaryNew,
          Score = meritocracy.Score == null ? null : new ViewCrudMeritocracyScore()
          {
            _id = meritocracy.Score._id,
            EnabledCompanyDate = meritocracy.Score.EnabledCompanyDate,
            EnabledOccupationDate = meritocracy.Score.EnabledOccupationDate,
            EnabledSchooling = meritocracy.Score.EnabledSchooling,
            EnabledActivitiesExcellence = meritocracy.Score.EnabledActivitiesExcellence,
            EnabledMaturity = meritocracy.Score.EnabledMaturity,
            EnabledGoals = meritocracy.Score.EnabledGoals,
            WeightCompanyDate = meritocracy.Score.WeightCompanyDate,
            WeightOccupationDate = meritocracy.Score.WeightOccupationDate,
            WeightSchooling = meritocracy.Score.WeightSchooling,
            WeightActivitiesExcellence = meritocracy.Score.WeightActivitiesExcellence,
            WeightMaturity = meritocracy.Score.WeightMaturity,
            WeightGoals = meritocracy.Score.WeightGoals
          },
          Grade = meritocracy.Grade == null ? null : new ViewListGrade()
          {
            _id = meritocracy.Grade._id,
            Name = meritocracy.Grade.Name,
            Order = meritocracy.Grade.Order,
            StepMedium = meritocracy.Grade.StepMedium,
            Steps = meritocracy.Grade.ListSteps.Select
        (x => new ViewListStep() { Step = x.Step, Salary = x.Salary }).ToList()
          },
          GradeScale = meritocracy.GradeScale == null ? null : new ViewListGrade()
          {
            _id = meritocracy.GradeScale._id,
            Name = meritocracy.GradeScale.Name,
            Order = meritocracy.GradeScale.Order,
            StepMedium = meritocracy.GradeScale.StepMedium,
            Steps = meritocracy.GradeScale.ListSteps.Select
          (x => new ViewListStep() { Step = x.Step, Salary = x.Salary }).ToList()
          }
        };

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListMeritocracy> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListMeritocracy> detail = serviceMeritocracy.GetAllNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(x => new ViewListMeritocracy()
          {
            _id = x._id,
            Name = x.Person.Name,

          }).ToList();
        total = serviceMeritocracy.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewListMeritocracyActivitie> ListMeritocracyActivitie(string idmeritocracy)
    {
      try
      {
        List<ViewListMeritocracyActivitie> detail = serviceMeritocracy.GetAllNewVersion(p => p._id == idmeritocracy).Result.
          FirstOrDefault().MeritocracyActivities
          .Select(x => new ViewListMeritocracyActivitie()
          {
            Activitie = x.Activities,
            Mark = x.Mark
          }).ToList();

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListMeritocracy> ListWaitManager(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        List<ViewListMeritocracy> list = servicePerson.GetAllNewVersion(p => p.Manager._id == idmanager && p.TypeJourney == EnumTypeJourney.Monitoring &&
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
          foreach (ViewListMeritocracy item in list)
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

        total = servicePerson.CountNewVersion(p => p.Manager._id == idmanager && p.TypeJourney == EnumTypeJourney.Monitoring &&
                                p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateActivitieMark(string idmeritocracy, string idactivitie, byte mark)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == idmeritocracy).Result;
        decimal averageActivities = 0;
        var counttotal = 0;

        foreach (var item in meritocracy.MeritocracyActivities)
        {
          if (item.Activities._id == idactivitie)
            item.Mark = mark;

          averageActivities += item.Mark;
          if (item.Mark > 0)
            counttotal += 1;
        }

        meritocracy.ActivitiesExcellence = ((averageActivities) / (counttotal == 0 ? 1 : counttotal));

        meritocracy.WeightActivitiesExcellence = byte.Parse(Math.Truncate(meritocracy.ActivitiesExcellence).ToString());
        serviceMeritocracy.Update(meritocracy, null).Wait();

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region MeritocracyScore

    public string NewMeritocracyScore(ViewCrudMeritocracyScore view)
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
    public string RemoveMeritocracyScore(string id)
    {
      try
      {
        MeritocracyScore item = serviceMeritocracyScore.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceMeritocracyScore.Update(item, null).Wait();
        return "MeritocracyScore deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateMeritocracyScore(ViewCrudMeritocracyScore view)
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
        serviceMeritocracyScore.Update(meritocracyScore, null).Wait();
        return "MeritocracyScore altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudMeritocracyScore GetMeritocracyScore(string id)
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
    public ViewListMeritocracyScore ListMeritocracyScore()
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
    public string DeleteSalaryScaleScore(string id)
    {
      try
      {
        SalaryScaleScore item = serviceSalaryScaleScore.GetFreeNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceSalaryScaleScore.UpdateAccount(item, null).Wait();
        return "SalaryScaleScore deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateSalaryScaleScore(ViewCrudSalaryScaleScore view)
    {
      try
      {
        SalaryScaleScore salaryScaleScore = serviceSalaryScaleScore.GetFreeNewVersion(p => p._id == view._id).Result;
        salaryScaleScore.CountSteps = view.CountSteps;
        salaryScaleScore.Step = view.Step;
        salaryScaleScore.Value = view.Value;
        serviceSalaryScaleScore.UpdateAccount(salaryScaleScore, null).Wait();
        return "SalaryScaleScore altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudSalaryScaleScore GetSalaryScaleScore(string id)
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
    public List<ViewCrudSalaryScaleScore> ListSalaryScaleScore(ref long total, int count = 10, int page = 1, string filter = "")
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
