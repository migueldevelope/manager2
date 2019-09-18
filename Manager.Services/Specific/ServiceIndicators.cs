using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
#pragma warning disable 4014
  public class ServiceIndicators : Repository<Monitoring>, IServiceIndicators
  {
    private readonly ServiceGeneric<Monitoring> serviceMonitoring;
    private readonly ServiceGeneric<OnBoarding> serviceOnboarding;
    private readonly ServiceGeneric<Occupation> serviceOccupation;
    private readonly ServiceGeneric<Area> serviceArea;
    private readonly ServiceGeneric<Checkpoint> serviceCheckpoint;
    private readonly ServiceGeneric<Workflow> serviceWorkflow;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Plan> servicePlan;
    private readonly ServiceGeneric<SalaryScale> serviceSalaryScale;
    private readonly ServiceLog serviceLog;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceGeneric<Account> serviceAccount;
    private readonly ServiceGeneric<Certification> serviceCertification;
    private readonly ServiceGeneric<CertificationPerson> serviceCertificationPerson;
    private readonly ServiceGeneric<Recommendation> serviceRecommendation;
    private readonly ServiceGeneric<RecommendationPerson> serviceRecommendationPerson;
    private readonly ServiceGeneric<Parameter> serviceParameter;
    private readonly IServicePerson serviceIPerson;

    public string path;
    private HubConnection hubConnection;

    public ServiceIndicators(DataContext context, DataContext contextLog, string pathToken, IServicePerson _serviceIPerson)
      : base(context)
    {
      try
      {
        serviceMonitoring = new ServiceGeneric<Monitoring>(context);
        serviceOnboarding = new ServiceGeneric<OnBoarding>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        servicePlan = new ServiceGeneric<Plan>(context);
        serviceCheckpoint = new ServiceGeneric<Checkpoint>(context);
        serviceLog = new ServiceLog(context, contextLog);
        serviceMailModel = new ServiceMailModel(context);
        serviceMail = new ServiceGeneric<MailLog>(contextLog);
        serviceWorkflow = new ServiceGeneric<Workflow>(context);
        serviceAccount = new ServiceGeneric<Account>(context);
        serviceCertification = new ServiceGeneric<Certification>(context);
        serviceCertificationPerson = new ServiceGeneric<CertificationPerson>(context);
        serviceRecommendation = new ServiceGeneric<Recommendation>(context);
        serviceRecommendationPerson = new ServiceGeneric<RecommendationPerson>(context);
        serviceParameter = new ServiceGeneric<Parameter>(context);
        serviceSalaryScale = new ServiceGeneric<SalaryScale>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        serviceArea = new ServiceGeneric<Area>(context);
        serviceIPerson = _serviceIPerson;

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
      servicePerson._user = _user;
      serviceOnboarding._user = _user;
      serviceMonitoring._user = _user;
      servicePlan._user = _user;
      serviceLog._user = _user;
      serviceMailModel._user = _user;
      serviceWorkflow._user = _user;
      serviceMail._user = _user;
      serviceCertification._user = _user;
      serviceCheckpoint._user = _user;
      serviceRecommendation._user = _user;
      serviceRecommendationPerson._user = _user;
      serviceParameter._user = _user;
      serviceSalaryScale._user = _user;
      serviceOccupation._user = _user;
      serviceArea._user = _user;
      serviceCertificationPerson._user = _user;
      serviceIPerson.SetUser(_user);
    }

    public List<ViewListPending> OnboardingInDay(List<ViewListIdIndicators> persons)
    {
      try
      {
        var list = new List<ViewListPending>();
        var onboardings = serviceOnboarding.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        var parameterget = serviceParameter.GetNewVersion(p => p.Key == "DeadlineAdmOnboarding").Result;
        long parameter = 90;
        if (parameterget != null)
          parameter = long.Parse(parameterget.Content);

        persons = persons.Where(p => (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation)).ToList();

        foreach (var item in persons)
        {
          DateTime dateadm = item.DateAdm == null ? DateTime.Now : item.DateAdm.Value;
          if (item.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
            if (item.DateLastOccupation != null)
              dateadm = item.DateLastOccupation.Value;

          var days = (dateadm.AddDays(parameter) - DateTime.Now).Days;

          if (days >= 10)
          {
            var view = onboardings.Where(p => p.Person._id == item._id && p.StatusOnBoarding == EnumStatusOnBoarding.End).FirstOrDefault();
            if (view == null)
              list.Add(new ViewListPending()
              {
                Manager = item.Manager,
                Person = item.Name,
                Days = days
              });
          }
        }

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPending> OnboardingToWin(List<ViewListIdIndicators> persons)
    {
      try
      {
        var list = new List<ViewListPending>();
        var onboardings = serviceOnboarding.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        var parameterget = serviceParameter.GetNewVersion(p => p.Key == "DeadlineAdmOnboarding").Result;
        long parameter = 90;
        if (parameterget != null)
          parameter = long.Parse(parameterget.Content);
        persons = persons.Where(p => (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation)).ToList();

        foreach (var item in persons)
        {
          DateTime dateadm = item.DateAdm == null ? DateTime.Now : item.DateAdm.Value;
          if (item.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
            if (item.DateLastOccupation != null)
              dateadm = item.DateLastOccupation.Value;

          var days = (dateadm.AddDays(parameter) - DateTime.Now).Days;
          if ((days < 10) && (days >= 0))
          {
            var view = onboardings.Where(p => p.Person._id == item._id && p.StatusOnBoarding == EnumStatusOnBoarding.End).FirstOrDefault();
            if (view == null)
              list.Add(new ViewListPending()
              {
                Manager = item.Manager,
                Person = item.Name,
                Days = days
              });
          }
        }

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPending> OnboardingLate(List<ViewListIdIndicators> persons)
    {
      try
      {
        var list = new List<ViewListPending>();
        var onboardings = serviceOnboarding.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        var parameterget = serviceParameter.GetNewVersion(p => p.Key == "DeadlineAdmOnboarding").Result;
        long parameter = 90;
        if (parameterget != null)
          parameter = long.Parse(parameterget.Content);
        persons = persons.Where(p => (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation)).ToList();

        foreach (var item in persons)
        {
          DateTime dateadm = item.DateAdm == null ? DateTime.Now : item.DateAdm.Value;
          if (item.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
            if (item.DateLastOccupation != null)
              dateadm = item.DateLastOccupation.Value;

          var days = (dateadm.AddDays(parameter) - DateTime.Now).Days;
          if (days < 0)
          {
            var view = onboardings.Where(p => p.Person._id == item._id && p.StatusOnBoarding == EnumStatusOnBoarding.End).FirstOrDefault();
            if (view == null)
              list.Add(new ViewListPending()
              {
                Manager = item.Manager,
                Person = item.Name,
                Days = (days * -1)
              });
          }
        }

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPending> CheckpointInDay(List<ViewListIdIndicators> persons)
    {
      try
      {
        var list = new List<ViewListPending>();
        var checkpoints = serviceCheckpoint.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        var parameterget = serviceParameter.GetNewVersion(p => p.Key == "DeadlineAdm").Result;
        long parameter = 90;
        if (parameterget != null)
          parameter = long.Parse(parameterget.Content);
        persons = persons.Where(p => p.TypeJourney == EnumTypeJourney.Checkpoint).ToList();

        foreach (var item in persons)
        {
          DateTime dateadm = item.DateAdm == null ? DateTime.Now : item.DateAdm.Value;
          var days = (dateadm.AddDays(parameter) - DateTime.Now).Days;
          if (days >= 10)
          {
            var view = checkpoints.Where(p => p.Person._id == item._id && p.StatusCheckpoint == EnumStatusCheckpoint.End).FirstOrDefault();
            if (view == null)
              list.Add(new ViewListPending()
              {
                Manager = item.Manager,
                Person = item.Name,
                Days = days
              });
          }
        }

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPending> CheckpointToWin(List<ViewListIdIndicators> persons)
    {
      try
      {
        var list = new List<ViewListPending>();
        var checkpoints = serviceCheckpoint.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        var parameterget = serviceParameter.GetNewVersion(p => p.Key == "DeadlineAdm").Result;
        long parameter = 90;
        if (parameterget != null)
          parameter = long.Parse(parameterget.Content);
        persons = persons.Where(p => p.TypeJourney == EnumTypeJourney.Checkpoint).ToList();

        foreach (var item in persons)
        {
          DateTime dateadm = item.DateAdm == null ? DateTime.Now : item.DateAdm.Value;
          var days = (dateadm.AddDays(parameter) - DateTime.Now).Days;
          if ((days < 10) && (days >= 0))
          {
            var view = checkpoints.Where(p => p.Person._id == item._id && p.StatusCheckpoint == EnumStatusCheckpoint.End).FirstOrDefault();
            if (view == null)
              list.Add(new ViewListPending()
              {
                Manager = item.Manager,
                Person = item.Name,
                Days = days
              });
          }
        }

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPending> CheckpointLate(List<ViewListIdIndicators> persons)
    {
      try
      {
        var list = new List<ViewListPending>();
        var checkpoints = serviceCheckpoint.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        var parameterget = serviceParameter.GetNewVersion(p => p.Key == "DeadlineAdm").Result;
        long parameter = 90;
        if (parameterget != null)
          parameter = long.Parse(parameterget.Content);

        persons = persons.Where(p => p.TypeJourney == EnumTypeJourney.Checkpoint).ToList();

        foreach (var item in persons)
        {
          DateTime dateadm = item.DateAdm == null ? DateTime.Now : item.DateAdm.Value;
          var days = (dateadm.AddDays(parameter) - DateTime.Now).Days;
          if (days < 0)
          {
            var view = checkpoints.Where(p => p.Person._id == item._id && p.StatusCheckpoint == EnumStatusCheckpoint.End).FirstOrDefault();
            if (view == null)
              list.Add(new ViewListPending()
              {
                Manager = item.Manager,
                Person = item.Name,
                Days = (days * -1)
              });
          }
        }

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewPersonsNotInfo> GetPersonsNotInfo(int count, int page, ref long total, string filter)
    {
      try
      {
        var list = new List<ViewPersonsNotInfo>();
        int skip = (count * (page - 1));

        var salaryscale = serviceSalaryScale.CountNewVersion(p => p.Status == EnumStatus.Enabled).Result == 0 ? false : true;

        var persons = servicePerson.GetAllNewVersion(p => p.TypeJourney != EnumTypeJourney.OutOfJourney
        && p.StatusUser != EnumStatusUser.Disabled && p.TypeUser != EnumTypeUser.Administrator
        && p.User.Name.Contains(filter)
        && (p.Manager == null || p.Occupation == null || p.SalaryScales == null ||
        p.User.Schooling == null || p.User.DateAdm == null)).Result;


        foreach (var item in persons)
        {
          if (salaryscale == false)
            item.SalaryScales = new SalaryScalePerson();


          if ((item.Occupation == null) || (item.Manager == null) || (item.SalaryScales == null)
            || (item.User?.Schooling == null) || (item.User?.DateAdm == null))
          {
            var view = new ViewPersonsNotInfo()
            {
              DateAdm = false,
              Occupation = false,
              Manager = false,
              SalaryScale = false,
              Schooling = false
            };

            if (item.User?.DateAdm == null)
              view.DateAdm = true;
            if (item.Occupation == null)
              view.Occupation = true;
            if (item.Manager == null)
              view.Manager = true;
            if (item.SalaryScales == null)
              view.SalaryScale = true;
            if (item.User?.Schooling == null)
              view.Schooling = true;

            view.Person = item.User.Name;
            list.Add(view);
          }
        }
        total = list.Count();

        return list.OrderBy(p => p.Person).Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public ViewMoninitoringQtd GetMoninitoringQtd(ViewFilterDate date, string idManager)
    {
      try
      {
        var view = new ViewMoninitoringQtd();
        List<Monitoring> monitorings = new List<Monitoring>();

        if (idManager != string.Empty)
          monitorings = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
              && p.DateEndEnd >= date.Begin && p.DateEndEnd <= date.End & p.Person._idManager == idManager).Result;
        else
          monitorings = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
              && p.DateEndEnd >= date.Begin && p.DateEndEnd <= date.End).Result;

        long comments = 0;
        long praises = 0;
        long plans = 0;

        foreach (var moni in monitorings)
        {
          if (moni.SkillsCompany != null)
          {
            foreach (var item in moni.SkillsCompany)
            {
              if (item.Comments != null)
                comments += item.Comments.Count();
              if (item.Plans != null)
                plans += item.Plans.Count();
              if (item.Praise != null)
                praises += 1;
            }
          }
          if (moni.Schoolings != null)
          {
            foreach (var item in moni.Schoolings)
            {
              if (item.Comments != null)
                comments += item.Comments.Count();
              if (item.Plans != null)
                plans += item.Plans.Count();
              if (item.Praise != null)
                praises += 1;
            }
          }
          if (moni.Activities != null)
          {
            foreach (var item in moni.Activities)
            {
              if (item.Comments != null)
                comments += item.Comments.Count();
              if (item.Plans != null)
                plans += item.Plans.Count();
              if (item.Praise != null)
                praises += 1;
            }
          }
        }

        view.Comments = comments;
        view.Praises = praises;
        view.Plans = plans;

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListPlanQtdGerals GetPlanQtd(ViewFilterDate date, int count, int page, ref long total, string filter)
    {
      try
      {
        var view = new ViewListPlanQtdGerals();
        int skip = (count * (page - 1));

        var result = new List<ViewPlanQtd>();
        List<Monitoring> monitorings = new List<Monitoring>();

        monitorings = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
              && p.DateEndEnd >= date.Begin && p.DateEndEnd <= date.End
              && p.Person.Manager.Contains(filter)).Result;

        foreach (var moni in monitorings)
        {
          if (result.Where(p => p._idManager == moni.Person._idManager).Count() == 0)
            result.Add(new ViewPlanQtd() { _idManager = moni.Person._idManager, Manager = moni.Person.Manager });

          var person = servicePerson.GetNewVersion(p => p._id == moni.Person._id).Result;
          foreach (var lst in result.Where(p => p._idManager == person.Manager?._id))
          {
            var plans = servicePlan.GetAllNewVersion(p => p._idMonitoring == moni._id).Result;
            lst.Schedule += plans.Count();
            lst.Realized += plans.Where(p => p.StatusPlan != EnumStatusPlan.Open).Count();
            lst.Late += plans.Where(p => p.StatusPlan == EnumStatusPlan.Open && p.Deadline < DateTime.Now).Count();
            lst.Balance = lst.Realized - lst.Late;
          }
        }

        total = result.Where(p => p.Schedule > 0).Count();
        result = result.Where(p => p.Schedule > 0).ToList();
        view.Schedule = result.Average(p => p.Schedule);
        view.Realized = result.Average(p => p.Realized);
        view.Late = result.Average(p => p.Late);
        view.Balance = result.Average(p => p.Balance);

        view.List = result.OrderByDescending(p => p.Balance).Skip(skip).Take(count).ToList();
        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListMonitoringQtdManagerGeral GetMoninitoringQtdManager(ViewFilterDate date, string idManager, int count, int page, ref long total, string filter)
    {
      try
      {
        int skip = (count * (page - 1));
        var result = new ViewListMonitoringQtdManagerGeral();

        var list = new List<ViewMoninitoringQtdManager>();
        List<Monitoring> monitorings = new List<Monitoring>();

        if (idManager != string.Empty)
          monitorings = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
              && p.DateEndEnd >= date.Begin && p.DateEndEnd <= date.End & p.Person._idManager == idManager).Result;
        else
          monitorings = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
              && p.DateEndEnd >= date.Begin && p.DateEndEnd <= date.End).Result;

        foreach (var moni in monitorings)
        {
          if (list.Where(p => p._idManager == moni.Person._idManager).Count() == 0)
            list.Add(new ViewMoninitoringQtdManager { Manager = moni.Person.Manager, _idManager = moni.Person._idManager, Praises = 0, Comments = 0, Plans = 0 });

          foreach (var lst in list.Where(p => p._idManager == moni.Person._idManager))
          {
            if (moni.SkillsCompany != null)
            {
              foreach (var item in moni.SkillsCompany)
              {
                if (item.Comments != null)
                {
                  lst.Comments += item.Comments.Count();
                  lst.Total += item.Comments.Count();
                }
                if (item.Plans != null)
                {
                  lst.Plans += item.Plans.Count();
                  lst.Total += item.Plans.Count();
                }
                if (item.Praise != null)
                {
                  lst.Praises += 1;
                  lst.Total += 1;
                }
              }
            }
            if (moni.Schoolings != null)
            {
              foreach (var item in moni.Schoolings)
              {
                if (item.Comments != null)
                {
                  lst.Comments += item.Comments.Count();
                  lst.Total += item.Comments.Count();
                }
                if (item.Plans != null)
                {
                  lst.Plans += item.Plans.Count();
                  lst.Total += item.Plans.Count();
                }
                if (item.Praise != null)
                {
                  lst.Praises += 1;
                  lst.Total += 1;
                }
              }
            }
            if (moni.Activities != null)
            {
              foreach (var item in moni.Activities)
              {
                if (item.Comments != null)
                {
                  lst.Comments += item.Comments.Count();
                  lst.Total += item.Comments.Count();
                }
                if (item.Plans != null)
                {
                  lst.Plans += item.Plans.Count();
                  lst.Total += item.Plans.Count();
                }
                if (item.Praise != null)
                {
                  lst.Praises += 1;
                  lst.Total += 1;
                }
              }
            }
          }

        }
        total = list.Where(p => (p.Plans > 0 || p.Praises > 0 || p.Comments > 0)).Count();

        list = list.Where(p => (p.Plans > 0 || p.Praises > 0 || p.Comments > 0)).ToList();

        if(list.Count > 0)
        {
          result.Comments = list.Average(p => p.Comments);
          result.Plans = list.Average(p => p.Plans);
          result.Praises = list.Average(p => p.Praises);
        }
        

        result.List = list.Where(p => (p.Plans > 0 || p.Praises > 0 || p.Comments > 0) && p.Manager.Contains(filter)).OrderByDescending(p => p.Total).Skip(skip).Take(count).ToList();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewTagsCloud> ListTagsCloudCompanyPeriod(ViewFilterManagerAndDate filters, string idmanager)
    {
      try
      {
        var list = new List<Monitoring>();

        if (idmanager != string.Empty)
          list = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
            && p.DateEndEnd >= filters.Date.Begin && p.DateEndEnd <= filters.Date.End && p.Person._idManager == idmanager).Result.ToList();
        else
          list = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
            && p.DateEndEnd >= filters.Date.Begin && p.DateEndEnd <= filters.Date.End).Result.ToList();

        if (filters.Managers.Count() > 0)
        {
          foreach (var item in list)
          {
            foreach (var manager in filters.Managers)
            {
              if (manager._id == item.Person._idManager)
                item.Status = EnumStatus.Disabled;
            }
          }

          list = list.Where(p => p.Status == EnumStatus.Disabled).ToList();
        }

        List<ViewTagsCloud> listResult = new List<ViewTagsCloud>();
        foreach (var item in list)
        {
          foreach (var row in item.SkillsCompany)
          {
            if (row.Plans.Count() > 0)
              listResult.Add(new ViewTagsCloud() { text = row.Skill.Name });
          }
        }


        var result = listResult.GroupBy(x => x.text)
            .Select(x => new ViewTagsCloud()
            {
              text = x.Key,
              weight = x.Count()
            }).ToList();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTagsCloud> ListTagsCloudPeriod(ViewFilterManagerAndDate filters, string idmanager)
    {
      try
      {
        var list = new List<Monitoring>();

        if (idmanager != string.Empty)
          list = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
            && p.DateEndEnd >= filters.Date.Begin && p.DateEndEnd <= filters.Date.End && p.Person._idManager == idmanager).Result.ToList();
        else
          list = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
            && p.DateEndEnd >= filters.Date.Begin && p.DateEndEnd <= filters.Date.End).Result.ToList();

        if (filters.Managers.Count() > 0)
        {
          foreach (var item in list)
          {
            foreach (var manager in filters.Managers)
            {
              if (manager._id == item.Person._idManager)
                item.Status = EnumStatus.Disabled;
            }
          }

          list = list.Where(p => p.Status == EnumStatus.Disabled).ToList();
        }


        List<ViewTagsCloud> listResult = new List<ViewTagsCloud>();
        foreach (var item in list)
        {
          foreach (var row in item.Activities)
          {
            foreach (var skill in row.Plans)
            {
              foreach (var view in skill.Skills)
              {
                listResult.Add(new ViewTagsCloud() { text = view.Name });
              }
            }
          }
        }


        var result = listResult.GroupBy(x => x.text)
            .Select(x => new ViewTagsCloud()
            {
              text = x.Key,
              weight = x.Count()
            }).ToList();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTagsCloudPerson> ListTagsCloudCompanyPeriodPerson(ViewFilterManagerAndDate filters, string idmanager, int count, int page, ref long total, string filter)
    {
      try
      {
        int skip = (count * (page - 1));
        var list = new List<Monitoring>();

        if (idmanager != string.Empty)
          list = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
            && p.DateEndEnd >= filters.Date.Begin && p.DateEndEnd <= filters.Date.End && p.Person._idManager == idmanager).Result.ToList();
        else
          list = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
            && p.DateEndEnd >= filters.Date.Begin && p.DateEndEnd <= filters.Date.End).Result.ToList();

        if (filters.Managers.Count() > 0)
        {
          foreach (var item in list)
          {
            foreach (var manager in filters.Managers)
            {
              if (manager._id == item.Person._idManager)
                item.Status = EnumStatus.Disabled;
            }
          }

          list = list.Where(p => p.Status == EnumStatus.Disabled).ToList();
        }

        List<ViewTagsCloudPerson> listResult = new List<ViewTagsCloudPerson>();
        foreach (var item in list)
        {
          foreach (var row in item.SkillsCompany)
          {
            if (row.Plans.Count() > 0)
              listResult.Add(new ViewTagsCloudPerson() { Item = item.Activities.FirstOrDefault()?.Activities?.Name, Text = row.Skill.Name, Person = item.Person?.Name });
          }
        }

        total = listResult.Count();
        return listResult.Where(p => p.Person.Contains(filter)).Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTagsCloudPerson> ListTagsCloudPeriodPerson(ViewFilterManagerAndDate filters, string idmanager, int count, int page, ref long total, string filter)
    {
      try
      {
        int skip = (count * (page - 1));
        var list = new List<Monitoring>();

        if (idmanager != string.Empty)
          list = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
            && p.DateEndEnd >= filters.Date.Begin && p.DateEndEnd <= filters.Date.End && p.Person._idManager == idmanager).Result.ToList();
        else
          list = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
            && p.DateEndEnd >= filters.Date.Begin && p.DateEndEnd <= filters.Date.End).Result.ToList();

        if (filters.Managers.Count() > 0)
        {
          foreach (var item in list)
          {
            foreach (var manager in filters.Managers)
            {
              if (manager._id == item.Person._idManager)
                item.Status = EnumStatus.Disabled;
            }
          }

          list = list.Where(p => p.Status == EnumStatus.Disabled).ToList();
        }

        List<ViewTagsCloudPerson> listResult = new List<ViewTagsCloudPerson>();
        foreach (var item in list)
        {
          foreach (var row in item.Activities)
          {
            foreach (var skill in row.Plans)
            {
              foreach (var view in skill.Skills)
              {
                listResult.Add(new ViewTagsCloudPerson() { Item = row.Activities.Name, Text = view.Name, Person = item.Person?.Name });
              }
            }
          }
        }

        total = listResult.Count();
        return listResult.Where(p => p.Person.Contains(filter)).Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListPlanQtd GetListPlanQtd(ViewFilterDate date, string idManager)
    {
      try
      {
        var view = new ViewListPlanQtd();
        var plans = new List<Plan>();

        var monitorings = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End).Result;

        if (idManager != string.Empty)
          plans = servicePlan.GetAllNewVersion(p => p.Person._idManager == idManager && p.DateInclude >= date.Begin && p.DateInclude <= date.End).Result.ToList();
        else
          plans = servicePlan.GetAllNewVersion(p => p.DateInclude >= date.Begin && p.DateInclude <= date.End).Result.ToList();

        foreach (var item in plans)
        {
          if (monitorings.Where(p => p._id == item._idMonitoring).FirstOrDefault() == null)
            item.Status = EnumStatus.Disabled;
        }

        plans = plans.Where(p => p.Status == EnumStatus.Enabled).ToList();

        view.Schedules = plans.Count(p => p.StatusPlan == EnumStatusPlan.Open && p.Deadline >= DateTime.Now);
        view.Ends = plans.Where(p => p.StatusPlan != EnumStatusPlan.Open).Count();
        view.Lates = plans.Where(p => p.StatusPlan == EnumStatusPlan.Open && p.Deadline < DateTime.Now).Count();

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewAccountEnableds> GetAccountEnableds()
    {
      try
      {
        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled
        && p.TypeJourney != EnumTypeJourney.OutOfJourney)
          .Result.Select(p => new ViewAccountEnableds()
          {
            Person = p.User?.Name,
            DateAdm = p.User?.DateAdm,
            Mail = p.User?.Mail,
            Manager = p.Manager?.Name,
            OccupationName = p.Occupation?.Name,
            TypeJourney = p.TypeJourney.ToString(),
            TypeUser = p.TypeUser.ToString(),
          }).ToList();

        return persons;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewIndicatorsNotes> GetNotes(string id)
    {
      try
      {
        List<ViewIndicatorsNotes> result = new List<ViewIndicatorsNotes>();
        long totalqtd = 0;
        var monitorings = serviceMonitoring.CountNewVersion(p => p.Person._idManager == id & p.StatusMonitoring != EnumStatusMonitoring.InProgressPerson & p.StatusMonitoring != EnumStatusMonitoring.Wait & p.StatusMonitoring != EnumStatusMonitoring.End).Result;
        var onboardings = serviceOnboarding.CountNewVersion(p => p.Person._idManager == id & p.StatusOnBoarding != EnumStatusOnBoarding.InProgressPerson & p.StatusOnBoarding != EnumStatusOnBoarding.WaitPerson & p.StatusOnBoarding != EnumStatusOnBoarding.End).Result;
        var workflows = serviceWorkflow.CountNewVersion(p => p.Requestor._id == id & p.StatusWorkflow == EnumWorkflow.Open).Result;

        totalqtd = monitorings + onboardings + workflows;

        result.Add(new ViewIndicatorsNotes() { Name = "Monitoring", qtd = monitorings, total = totalqtd });
        result.Add(new ViewIndicatorsNotes() { Name = "Onboarding", qtd = onboardings, total = totalqtd });
        result.Add(new ViewIndicatorsNotes() { Name = "Workflow", qtd = workflows, total = totalqtd });

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTagsCloud> ListTagsCloudCompany(string idmanager)
    {
      try
      {
        var list = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End).Result.ToList();
        var persons = servicePerson.GetAllNewVersion(p => p.Manager._id == idmanager).Result.Select(p => p._id).ToList();
        foreach (var item in list)
        {
          if (persons.Where(p => p == item.Person?._id).Count() == 0)
            list.Where(p => p._id == item._id).FirstOrDefault().Status = EnumStatus.Disabled;
        }
        list = list.Where(p => p.Status == EnumStatus.Enabled).ToList();

        List<ViewTagsCloud> listResult = new List<ViewTagsCloud>();
        foreach (var item in list)
        {
          foreach (var row in item.SkillsCompany)
          {
            if (row.Plans.Count() > 0)
              listResult.Add(new ViewTagsCloud() { text = row.Skill.Name });
          }
        }


        var result = listResult.GroupBy(x => x.text)
            .Select(x => new ViewTagsCloud()
            {
              text = x.Key,
              weight = x.Count()
            }).ToList();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTagsCloud> ListTagsCloud(string idmanager)
    {
      try
      {
        var list = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End).Result.ToList();
        var persons = servicePerson.GetAllNewVersion(p => p.Manager._id == idmanager).Result.Select(p => p._id).ToList();
        foreach (var item in list)
        {
          if (persons.Where(p => p == item.Person?._id).Count() == 0)
            list.Where(p => p._id == item._id).FirstOrDefault().Status = EnumStatus.Disabled;
        }
        list = list.Where(p => p.Status == EnumStatus.Enabled).ToList();

        List<ViewTagsCloud> listResult = new List<ViewTagsCloud>();
        foreach (var item in list)
        {
          foreach (var row in item.Activities)
          {
            foreach (var skill in row.Plans)
            {
              foreach (var view in skill.Skills)
              {
                listResult.Add(new ViewTagsCloud() { text = view.Name });
              }
            }
          }
        }


        var result = listResult.GroupBy(x => x.text)
            .Select(x => new ViewTagsCloud()
            {
              text = x.Key,
              weight = x.Count()
            }).ToList();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTagsCloud> ListTagsCloudPerson(string idperson)
    {
      try
      {

        var list = serviceMonitoring.GetAllNewVersion(p => p.Person._id == idperson & p.StatusMonitoring == EnumStatusMonitoring.End).Result.ToList();

        List<ViewTagsCloud> listResult = new List<ViewTagsCloud>();
        foreach (var item in list)
        {
          foreach (var row in item.Activities)
          {
            foreach (var skill in row.Plans)
            {
              foreach (var view in skill.Skills)
              {
                listResult.Add(new ViewTagsCloud() { text = view.Name });
              }
            }
          }
        }


        var result = listResult.GroupBy(x => x.text)
            .Select(x => new ViewTagsCloud()
            {
              text = x.Key,
              weight = x.Count()
            }).ToList();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTagsCloud> ListTagsCloudCompanyPerson(string idperson)
    {
      try
      {
        var list = serviceMonitoring.GetAllNewVersion(p => p.Person._id == idperson & p.StatusMonitoring == EnumStatusMonitoring.End).Result.ToList();

        List<ViewTagsCloud> listResult = new List<ViewTagsCloud>();
        foreach (var item in list)
        {
          foreach (var row in item.SkillsCompany)
          {
            if (row.Plans.Count() > 0)
              listResult.Add(new ViewTagsCloud() { text = row.Skill.Name });
          }
        }


        var result = listResult.GroupBy(x => x.text)
            .Select(x => new ViewTagsCloud()
            {
              text = x.Key,
              weight = x.Count()
            }).ToList();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewIndicatorsNotes> GetNotesPerson(string id)
    {
      try
      {
        List<ViewIndicatorsNotes> result = new List<ViewIndicatorsNotes>();
        long totalqtd = 0;
        var monitorings = serviceMonitoring.CountNewVersion(p => p.Person._id == id & p.StatusMonitoring != EnumStatusMonitoring.InProgressManager & p.StatusMonitoring != EnumStatusMonitoring.WaitManager & p.StatusMonitoring != EnumStatusMonitoring.End).Result;
        var onboardings = serviceOnboarding.CountNewVersion(p => p.Person._id == id & p.StatusOnBoarding != EnumStatusOnBoarding.InProgressManager & p.StatusOnBoarding != EnumStatusOnBoarding.WaitManager & p.StatusOnBoarding != EnumStatusOnBoarding.End).Result;
        var workflows = serviceWorkflow.CountNewVersion(p => p.Requestor._id == id & p.StatusWorkflow == EnumWorkflow.Open).Result;

        totalqtd = monitorings + onboardings + workflows;
        result.Add(new ViewIndicatorsNotes() { Name = "Monitoring", qtd = monitorings, total = totalqtd });
        result.Add(new ViewIndicatorsNotes() { Name = "Onboarding", qtd = onboardings, total = totalqtd });
        result.Add(new ViewIndicatorsNotes() { Name = "Workflow", qtd = workflows, total = totalqtd });

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public bool VerifyAccount(string id)
    {
      try
      {
        var account = serviceAccount.GetFreeNewVersion(p => p._id == id).Result;
        if (account == null)
          return false;
        else
          return true;

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetUser(BaseUser baseUser)
    {
      try
      {
        _user = baseUser;
        servicePerson._user = _user;
        serviceOnboarding._user = _user;
        serviceMonitoring._user = _user;
        serviceLog._user = _user;
        serviceMailModel._user = _user;
        serviceMail._user = _user;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SendMessages(string link)
    {
      try
      {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(link + "messagesHub")
            .Build();

        hubConnection.StartAsync();

        DoWork();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void DoWork()
    {
      try
      {
        while (true)
        {
          foreach (var person in servicePerson.GetAllFreeNewVersion(p => p.Status != EnumStatus.Disabled & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration).Result)
          {
            hubConnection.InvokeAsync("GetNotes", person._id, person._idAccount);
            hubConnection.InvokeAsync("GetNotesPerson", person._id, person._idAccount);
          }
          Task.Delay(1000);
        }

      }
      catch (Exception e)
      {
        var message = e;
      }
    }

    //public string[] ExportStatusOnboarding(ref  long total,  string filter, int count,int page)
    //{
    //  try
    //  {
    //    int skip = (count * (page - 1));

    //    var list = servicePerson.GetAllNewVersion(p => p.TypeJourney == EnumTypeJourney.OnBoarding
    //    & p.Name.ToUpper().Contains(filter.ToUpper()))
    //    .ToList().Select(p => new { Person = p, OnBoarding = serviceOnboarding.GetAllNewVersion(x => x.Person._id == p._id).FirstOrDefault() })
    //    .ToList().Skip(skip).Take(count).ToList();

    //    string head = "Name;NameManager;Status;";
    //    string[] rel = new string[1];
    //    rel[0] = head;

    //    foreach (var item in list)
    //    {
    //      string itemView = item.Person.User.Name + ";";
    //      if (item.Person.Manager == null)
    //        itemView += "Sem Gestor;";
    //      else
    //        itemView += item.Person.Manager + ";";
    //      if (item.OnBoarding == null)
    //        itemView += EnumStatusOnBoarding.Open.ToString() + ";";
    //      else
    //        itemView += item.OnBoarding.StatusOnBoarding + ";";


    //      rel = Export(rel, itemView);
    //    }

    //    return rel;
    //  }
    //  catch (Exception e)
    //  {
    //    throw e;
    //  }
    //}

    public string[] Export(string[] rel, string message)
    {
      try
      {
        string[] text = rel;
        string[] lines = null;
        try
        {
          lines = new string[text.Count() + 1];
          var count = 0;
          foreach (var item in text)
          {
            lines.SetValue(item, count);
            count += 1;
          }
          lines.SetValue(message, text.Count());
        }
        catch (Exception)
        {
          lines = new string[1];
          lines.SetValue(message, 0);
        }

        return lines;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string[] FileNew(string name, string message)
    {
      try
      {
        string[] text = { message };
        string[] lines = null;
        try
        {
          text = System.IO.File.ReadAllLines(name + ".csv");
          lines = new string[text.Count() + 1];
          var count = 0;
          foreach (var item in text)
          {
            lines.SetValue(item, count);
            count += 1;
          }
          lines.SetValue(message, text.Count());
        }
        catch (Exception)
        {
          lines = new string[1];
          lines.SetValue(message, 0);
        }

        System.IO.File.WriteAllLines(name + ".csv", lines);

        return lines;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public IEnumerable<ViewChartOnboarding> ChartOnboarding(List<ViewListIdIndicators> persons)
    {
      try
      {
        //var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator).Result;
        var onboardings = serviceOnboarding.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;

        List<ViewChartOnboarding> result = new List<ViewChartOnboarding>();
        for (byte i = 0; i <= 6; i++) result.Add(new ViewChartOnboarding() { Status = (EnumStatusOnBoarding)i, Count = 0 });

        foreach (var item in persons)
        {
          var list = onboardings.Where(p => p.Person._id == item._id);
          if (list.Count() == 0)
          {
            if (item.TypeJourney == EnumTypeJourney.OnBoarding)
              result.Where(p => p.Status == EnumStatusOnBoarding.WaitBegin).FirstOrDefault().Count += 1;
          }
          else
          {
            foreach (var view in list)
            {
              result.Where(p => p.Status == view.StatusOnBoarding).FirstOrDefault().Count += 1;
            }
          }
        };

        return result.Where(p => p.Count > 0).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public IEnumerable<ViewChartStatus> ChartOnboardingRealized(List<ViewListIdIndicators> persons)
    {
      try
      {
        var onboradings = serviceOnboarding.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        List<ViewChartStatus> result = new List<ViewChartStatus>();
        result.Add(new ViewChartStatus() { Status = "Realizado", Count = 0 });
        result.Add(new ViewChartStatus() { Status = "Não Realizado", Count = 0 });

        foreach (var item in persons)
        {
          var list = onboradings.Where(p => p.Person._id == item._id);
          if (list.Count() == 0)
          {
            if (item.TypeJourney == EnumTypeJourney.OnBoarding)
              result.Where(p => p.Status == "Não Realizado").FirstOrDefault().Count += 1;
          }
          else
          {
            foreach (var view in list)
            {
              if (view.StatusOnBoarding == EnumStatusOnBoarding.End)
                result.Where(p => p.Status == "Realizado").FirstOrDefault().Count += 1;
              else
                result.Where(p => p.Status == "Não Realizado").FirstOrDefault().Count += 1;
            }
          }
        };

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public IEnumerable<ViewChartMonitoring> ChartMonitoring(List<ViewListIdIndicators> persons)
    {
      try
      {
        var monitorings = serviceMonitoring.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        List<ViewChartMonitoring> result = new List<ViewChartMonitoring>();
        for (byte i = 0; i <= 7; i++) result.Add(new ViewChartMonitoring() { Status = (EnumStatusMonitoring)i, Count = 0 });

        foreach (var item in persons)
        {
          var list = monitorings.Where(p => p.Person._id == item._id);
          if (list.Count() == 0)
          {
            if (item.TypeJourney == EnumTypeJourney.Monitoring)
              result.Where(p => p.Status == EnumStatusMonitoring.Open).FirstOrDefault().Count += 1;
          }
          else
          {
            foreach (var view in list)
            {
              result.Where(p => p.Status == view.StatusMonitoring).FirstOrDefault().Count += 1;
            }
          }
        };

        return result.Where(p => p.Count > 0).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public IEnumerable<ViewChartStatus> ChartMonitoringRealized(List<ViewListIdIndicators> persons)
    {
      try
      {
        var monitorings = serviceMonitoring.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        List<ViewChartStatus> result = new List<ViewChartStatus>();
        result.Add(new ViewChartStatus() { Status = "Realizado", Count = 0 });
        result.Add(new ViewChartStatus() { Status = "Não Realizado", Count = 0 });

        foreach (var item in persons)
        {
          var list = monitorings.Where(p => p.Person._id == item._id);
          if (list.Count() == 0)
          {


            if (item.TypeJourney == EnumTypeJourney.Monitoring)
              result.Where(p => p.Status == "Não Realizado").FirstOrDefault().Count += 1;
          }
          else
          {
            foreach (var view in list)
            {
              if (view.StatusMonitoring == EnumStatusMonitoring.End)
                result.Where(p => p.Status == "Realizado").FirstOrDefault().Count += 1;
              else
              {
                result.Where(p => p.Status == "Não Realizado").FirstOrDefault().Count += 1;
              }
            }
          }
        };

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public IEnumerable<ViewChartCheckpoint> ChartCheckpoint(List<ViewListIdIndicators> persons)
    {
      try
      {
        var checkpoints = serviceCheckpoint.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;

        List<ViewChartCheckpoint> result = new List<ViewChartCheckpoint>();
        for (byte i = 0; i <= 2; i++) result.Add(new ViewChartCheckpoint() { Status = (EnumStatusCheckpoint)i, Count = 0 });

        foreach (var item in persons)
        {
          var list = checkpoints.Where(p => p.Person._id == item._id);
          if (list.Count() == 0)
          {
            if (item.TypeJourney == EnumTypeJourney.Checkpoint)
              result.Where(p => p.Status == EnumStatusCheckpoint.Open).FirstOrDefault().Count += 1;
          }
          else
          {
            foreach (var view in list)
            {
              result.Where(p => p.Status == view.StatusCheckpoint).FirstOrDefault().Count += 1;
            }
          }
        };

        return result.Where(p => p.Count > 0).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public IEnumerable<ViewChartStatus> ChartCheckpointRealized(List<ViewListIdIndicators> persons)
    {
      try
      {
        var checkpoints = serviceCheckpoint.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;

        List<ViewChartStatus> result = new List<ViewChartStatus>();
        result.Add(new ViewChartStatus() { Status = "Realizado", Count = 0 });
        result.Add(new ViewChartStatus() { Status = "Não Realizado", Count = 0 });

        foreach (var item in persons)
        {
          var list = checkpoints.Where(p => p.Person._id == item._id);
          if (list.Count() == 0)
            result.Where(p => p.Status == "Não Realizado").FirstOrDefault().Count += 1;
          else
          {
            foreach (var view in list)
            {
              if (view.StatusCheckpoint == EnumStatusCheckpoint.End)
                result.Where(p => p.Status == "Realizado").FirstOrDefault().Count += 1;
              else
              {
                if (item.TypeJourney == EnumTypeJourney.Checkpoint)
                  result.Where(p => p.Status == "Não Realizado").FirstOrDefault().Count += 1;
              }
            }
          }
        };

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public IEnumerable<ViewChartPlan> ChartPlan(List<ViewListIdIndicators> persons)
    {
      try
      {
        var monitorings = serviceMonitoring.GetAllNewVersion(p => p.Status == EnumStatus.Enabled
        && p.StatusMonitoring == EnumStatusMonitoring.End).Result;

        List<dynamic> result = new List<dynamic>();

        foreach (var item in persons)
        {
          var list = monitorings.Where(p => p.Person._id == item._id);
          foreach (var view in list)
          {
            foreach (var rows in view.Schoolings)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  Name = item._id,
                  Status = plan == null ? EnumStatusPlan.Open.ToString() : plan.StatusPlan.ToString()
                });
              }
            }

            foreach (var rows in view.SkillsCompany)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  Name = item._id,
                  Status = plan == null ? EnumStatusPlan.Open.ToString() : plan.StatusPlan.ToString()
                });
              }
            }

            foreach (var rows in view.Activities)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  Name = item._id,
                  Status = plan == null ? EnumStatusPlan.Open.ToString() : plan.StatusPlan.ToString()
                });
              }
            }

          }

        }

        return result.GroupBy(p => p.Status).Select(x => new ViewChartPlan
        {
          Status = x.Key,
          Count = x.Count()
        }).ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public IEnumerable<ViewChartRecommendation> ChartRecommendation(List<ViewListIdIndicators> persons)
    {
      try
      {
        var recommendations = serviceRecommendationPerson.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;

        List<dynamic> result = new List<dynamic>();

        foreach (var item in persons)
        {
          var list = recommendations.Where(p => p.Person._id == item._id);
          foreach (var view in list)
          {
            result.Add(new
            {
              Name = view.Recommendation.Name,
              _id = view.Person._id
            });
          }

        }

        return result.GroupBy(p => p.Name).Select(x => new ViewChartRecommendation
        {
          Name = x.Key,
          Count = x.Count()
        }).ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public IEnumerable<ViewChartStatus> ChartPlanRealized(List<ViewListIdIndicators> persons)
    {
      try
      {
        var monitorings = serviceMonitoring.GetAllNewVersion(p => p.Status == EnumStatus.Enabled
        && p.StatusMonitoring == EnumStatusMonitoring.End).Result;

        List<dynamic> result = new List<dynamic>();

        foreach (var item in persons)
        {
          var list = monitorings.Where(p => p.Person._id == item._id);
          foreach (var view in list)
          {
            foreach (var rows in view.Schoolings)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  Name = item._id,
                  Status = plan.StatusPlan == EnumStatusPlan.Realized ? "Realizado" : "Não Realizado"
                });
              }
            }

            foreach (var rows in view.SkillsCompany)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  Name = item._id,
                  Status = plan.StatusPlan == EnumStatusPlan.Realized ? "Realizado" : "Não Realizado"
                });
              }
            }

            foreach (var rows in view.Activities)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  Name = item._id,
                  Status = plan.StatusPlan == EnumStatusPlan.Realized ? "Realizado" : "Não Realizado"
                });
              }
            }

          }

        }

        return result.GroupBy(p => p.Status).Select(x => new ViewChartStatus
        {
          Status = x.Key,
          Count = x.Count()
        }).ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public IEnumerable<ViewChartCeritificationStatus> ChartCertificationStatus(List<ViewListIdIndicators> persons)
    {
      try
      {
        var certifications = serviceCertification.GetAllNewVersion(p => p.StatusCertification != EnumStatusCertification.Open).Result;

        List<dynamic> result = new List<dynamic>();

        foreach (var item in persons)
        {
          var list = certifications.Where(p => p.Person._id == item._id);
          foreach (var view in list)
          {
            result.Add(new
            {
              Name = view.StatusCertification,
              _id = view.Person._id
            });
          }

        }

        return result.GroupBy(p => p.Name).Select(x => new ViewChartCeritificationStatus
        {
          Status = x.Key,
          Count = x.Count()
        }).ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public IEnumerable<ViewChartCeritification> ChartCertification(ViewFilterDate date)
    {
      try
      {
        var certifications = serviceCertification.GetAllNewVersion(p => p.StatusCertification == EnumStatusCertification.Approved
        && p.DateEnd >= date.Begin && p.DateEnd <= date.End).Result;

        List<ViewChartCeritification> result = new List<ViewChartCeritification>();

        foreach (var item in certifications)
        {
          result.Add(new ViewChartCeritification()
          {
            Person = item.Person.Name,
            Skill = item.CertificationItem.Name
          });
        }

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public IEnumerable<ViewChartCertificationCount> ChartCertificationCount(ViewFilterDate date)
    {
      try
      {
        var certifications = serviceCertification.GetAllNewVersion(p => p.StatusCertification == EnumStatusCertification.Approved
        && p.DateEnd >= date.Begin && p.DateEnd <= date.End).Result;

        List<ViewChartCeritification> result = new List<ViewChartCeritification>();

        foreach (var item in certifications)
        {
          result.Add(new ViewChartCeritification()
          {
            Skill = item.CertificationItem.Name
          });
        }

        return result.GroupBy(p => p.Skill).Select(x => new ViewChartCertificationCount
        {
          Item = x.Key,
          Count = x.Count()
        }).ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListSucessFactors1> ListSucessFactors1()
    {
      try
      {
        return servicePerson.GetAllFreeNewVersion(p => p.Status == EnumStatus.Enabled).Result.
          Select(p => new ViewListSucessFactors1()
          {
            Person = p.User?.Name,
            Company = p.Company?.Name,
            DateAdm = p.User?.DateAdm
          }).OrderBy(p => p.Company).ThenBy(p => p.Person).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListSucessFactors2> ListSucessFactors2()
    {
      try
      {
        var persons = servicePerson.GetAllFreeNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        var list = new List<ViewListSucessFactors2>();

        foreach (var item in persons)
        {
          var view = new ViewListSucessFactors2()
          {
            Person = item.User?.Name,
            Company = item.Company?.Name,
            Manager = item.Manager?.Name
          };
          if (item.Occupation != null)
          {
            var area = serviceArea.GetFreeNewVersion(p => p._id == item.Occupation._idArea).Result;
            view.Area = area?.Name;
          }
          list.Add(view);
        }

        return list.OrderBy(p => p.Company).ThenBy(p => p.Manager).ThenBy(p => p.Person).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListSucessFactors3> ListSucessFactors3()
    {
      try
      {
        var persons = servicePerson.GetAllFreeNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        var list = new List<ViewListSucessFactors3>();

        foreach (var item in persons)
        {
          var view = new ViewListSucessFactors3()
          {
            Person = item.User?.Name,
            Company = item.Company?.Name,
            Mail = item.User?.Mail
          };
          if (item.Occupation != null)
          {
            var area = serviceArea.GetFreeNewVersion(p => p._id == item.Occupation._idArea).Result;
            view.Area = area?.Name;
          }

          list.Add(view);

        }

        return list.OrderBy(p => p.Company).ThenBy(p => p.Person).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }

}

#pragma warning restore 4014

