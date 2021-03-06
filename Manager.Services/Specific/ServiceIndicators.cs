﻿using Manager.Core.Base;
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
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
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
    private readonly ServiceGeneric<OffBoarding> serviceOffboarding;
    private readonly ServiceGeneric<Occupation> serviceOccupation;
    private readonly ServiceGeneric<Area> serviceArea;
    private readonly ServiceGeneric<Checkpoint> serviceCheckpoint;
    private readonly ServiceGeneric<Workflow> serviceWorkflow;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<User> serviceUser;
    private readonly ServiceGeneric<Plan> servicePlan;
    private readonly ServiceGeneric<SalaryScale> serviceSalaryScale;
    private readonly ServiceLog serviceLog;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceGeneric<Account> serviceAccount;
    private readonly ServiceGeneric<ElearningFluid> serviceElearningFluid;
    private readonly ServiceGeneric<Certification> serviceCertification;
    private readonly ServiceGeneric<CertificationPerson> serviceCertificationPerson;
    private readonly ServiceGeneric<Recommendation> serviceRecommendation;
    private readonly ServiceGeneric<Maturity> serviceMaturity;
    private readonly ServiceGeneric<RecommendationPerson> serviceRecommendationPerson;
    private readonly ServiceGeneric<Parameter> serviceParameter;
    private readonly ServiceGeneric<FluidCareers> serviceFluidCareers;
    private readonly ServiceGeneric<EventHistoric> serviceEventHistoric;

    public string path;
    private HubConnection hubConnection;

    public ServiceIndicators(DataContext context, DataContext contextLog, string pathToken)
      : base(context)
    {
      try
      {
        serviceMonitoring = new ServiceGeneric<Monitoring>(context);
        serviceOnboarding = new ServiceGeneric<OnBoarding>(context);
        serviceOffboarding = new ServiceGeneric<OffBoarding>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        servicePlan = new ServiceGeneric<Plan>(context);
        serviceMaturity = new ServiceGeneric<Maturity>(context);
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
        serviceFluidCareers = new ServiceGeneric<FluidCareers>(context);
        serviceElearningFluid = new ServiceGeneric<ElearningFluid>(context);
        serviceEventHistoric = new ServiceGeneric<EventHistoric>(context);
        serviceUser = new ServiceGeneric<User>(context);

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
      serviceOffboarding._user = _user;
      serviceMonitoring._user = _user;
      servicePlan._user = _user;
      serviceLog._user = _user;
      serviceMailModel._user = _user;
      serviceWorkflow._user = _user;
      serviceMail._user = _user;
      serviceFluidCareers._user = _user;
      serviceCertification._user = _user;
      serviceCheckpoint._user = _user;
      serviceRecommendation._user = _user;
      serviceRecommendationPerson._user = _user;
      serviceParameter._user = _user;
      serviceSalaryScale._user = _user;
      serviceOccupation._user = _user;
      serviceArea._user = _user;
      serviceMaturity._user = _user;
      serviceCertificationPerson._user = _user;
      serviceElearningFluid._user = _user;
      serviceEventHistoric._user = _user;
      serviceUser._user = _user;


    }

    public ViewTotalHourTraining TotalWorkloadTraining()
    {
      var dateyear = DateTime.Now.AddYears(-1);
      var eventhistoric = serviceEventHistoric.GetAllNewVersion(p => p.End >= dateyear).Result;

      return new ViewTotalHourTraining { Workload = eventhistoric.Sum(p => p.Workload) };
    }

    public List<ViewHourTraining> WorkloadTraining()
    {
      try
      {
        var dateyear = DateTime.Now.AddYears(-1);
        var eventhistoric = serviceEventHistoric.GetAllNewVersion(p => p.End >= dateyear).Result;
        var list = new List<ViewHourTraining>();

        for (var i = 0; i <= 12; i++)
        {
          var view = new ViewHourTraining() { Month = i };
          var datemonthb = DateTime.Now.AddMonths(i * -1);
          datemonthb = DateTime.Parse(datemonthb.Year + "-" + datemonthb.Month + "-01");
          var datemonthe = DateTime.Now.AddMonths(i * -1).AddMonths(1).AddDays(-1);
          var item = eventhistoric.Where(p => p.End >= datemonthb && p.End <= datemonthe).ToList();
          view.Workload = item.Sum(p => p.Workload);

          list.Add(view);
        }

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewDashboard GetDashboard()
    {
      try
      {
        var view = new ViewDashboard();
        //var maturity = serviceMaturity.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;

        var monitorings = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End).Result;
        //var onboardings = serviceOnboarding.GetAllNewVersion(p => p.StatusOnBoarding != EnumStatusOnBoarding.End).Result;
        var offboardings = serviceOffboarding.GetAllNewVersion(p => p.Step1.StatusFormOffBoarding == EnumStatusFormOffBoarding.End
        || p.Step2.StatusFormOffBoarding == EnumStatusFormOffBoarding.End).Result;
        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled).Result;
        var plans = servicePlan.GetAllNewVersion(p => p.StatusPlan != EnumStatusPlan.NoRealized).Result;
        var recommendation = serviceRecommendationPerson.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        var certification = serviceCertification.GetAllNewVersion(p => p.StatusCertification == EnumStatusCertification.Approved).Result;
        var elearningFluid = serviceElearningFluid.GetAllNewVersion(p => p.ElearningCertificate == true && p.ElearningVideo == true).Result;
        plans = plans.Where(p => monitorings.Select(x => x._id).Contains(p._idMonitoring)).ToList();

        view.OffBoardingRealized = offboardings.Count();
        view.MonitoringRealized = monitorings.Count();
        view.CertificationRealized = certification.Count();
        view.Recommendation = recommendation.Count();
        view.ElearningFluid = elearningFluid.Count();
        view.OnBoardingWait = persons.Where(p => p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation).Count();
        view.CheckpointWait = persons.Where(p => p.TypeJourney == EnumTypeJourney.Checkpoint).Count();
        view.Plans = plans.Count();

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPending> OnboardingInDayMap(List<_ViewList> managers)
    {
      try
      {
        List<string> filtermanager = null;
        if (managers.Count > 0)
          filtermanager = managers.Select(p => p._id).ToList();

        var list = new List<ViewListPending>();
        var parameterget = serviceParameter.GetNewVersion(p => p.Key == "DeadlineAdmOnboarding").Result;
        long parameter = 90;
        if (parameterget != null)
          parameter = long.Parse(parameterget.Content);

        var onboardings = serviceOnboarding.GetAllNewVersion(p => p.StatusOnBoarding == EnumStatusOnBoarding.End).Result.Select(p => p.Person._id).ToList();

        string mapper = "function() {";
        mapper += " var manager = null;";
        mapper += " var person = null;";
        mapper += " if (this.Manager != null)";
        mapper += " manager = this.Manager.Name;";
        mapper += " if (this.User != null){";
        mapper += " person = this.User.Name; }";
        mapper += " var getdate = new Date();";
        mapper += " var dateadm = this.User.DateAdm;";
        mapper += " if (this.TypeJourney == 3) {";
        mapper += " if (this.DateLastOccupation != null)";
        mapper += " dateadm = this.DateLastOccupation; }";
        //mapper += " if (dateadm == null)";
        //mapper += " dateadm = getdate;";
        mapper += " var datea = new Date(dateadm);";
        mapper += " var diff = Date.parse(getdate) - Date.parse(datea.toString());";
        mapper += " var days = Math.floor(diff / 86400000);";
        mapper += " emit({ manager: manager, person: person }, days)};";

        var map = new BsonJavaScript(mapper);
        var reduce = new BsonJavaScript("function(Manager, val) { return Array.sum(val); };");
        var coll = serviceOnboarding._context._db.GetCollection<BsonDocument>("Person");
        var options = new MapReduceOptions<BsonDocument, ViewListMapPersonManager>();
        FilterDefinition<Person> filters = null;


        if (managers.Count > 0)
          filters = Builders<Person>.Filter.Where(p => p._idAccount == _user._idAccount
          && p.StatusUser != EnumStatusUser.Disabled
          && p.Status == EnumStatus.Enabled
            && (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
            && !onboardings.Contains(p._id) && filtermanager.Contains(p.Manager._id)
            && p.Manager != null);
        else
          filters = Builders<Person>.Filter.Where(p => p._idAccount == _user._idAccount && p.Status == EnumStatus.Enabled
            && p.StatusUser != EnumStatusUser.Disabled
            && (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
            && !onboardings.Contains(p._id)
            && p.Manager != null);



        var json = filters.RenderToBsonDocument().ToJson();
        options.Filter = json;
        options.OutputOptions = MapReduceOutputOptions.Inline;
        var result = coll.MapReduce(map, reduce, options).ToList();

        list = result.Where(p => (parameter - p.value) >= 10).Select(p => new ViewListPending()
        {
          Person = p._id.person,
          Manager = p._id.manager,
          Days = parameter - long.Parse(p.value.ToString())
        }).ToList();

        return list.OrderBy(p => p.Days).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPending> OnboardingToWinMap(List<_ViewList> managers)
    {
      try
      {
        List<string> filtermanager = null;
        if (managers.Count > 0)
          filtermanager = managers.Select(p => p._id).ToList();

        var list = new List<ViewListPending>();
        var parameterget = serviceParameter.GetNewVersion(p => p.Key == "DeadlineAdmOnboarding").Result;
        long parameter = 90;
        if (parameterget != null)
          parameter = long.Parse(parameterget.Content);

        var onboardings = serviceOnboarding.GetAllNewVersion(p => p.StatusOnBoarding == EnumStatusOnBoarding.End).Result.Select(p => p.Person._id).ToList();

        string mapper = "function() {";
        mapper += " var manager = null;";
        mapper += " var person = null;";
        mapper += " if (this.Manager != null)";
        mapper += " manager = this.Manager.Name;";
        mapper += " if (this.User != null){";
        mapper += " person = this.User.Name; } ";
        mapper += " var getdate = new Date();";
        mapper += " var dateadm = this.User.DateAdm;";
        mapper += " if (this.TypeJourney == 3) {";
        mapper += " if (this.DateLastOccupation != null)";
        mapper += " dateadm = this.DateLastOccupation; }";
        //mapper += " if (dateadm == null)";
        //mapper += " dateadm = getdate;";
        mapper += " var datea = new Date(dateadm);";
        mapper += " var diff = Date.parse(getdate) - Date.parse(datea.toString());";
        mapper += " var days = Math.floor(diff / 86400000);";
        mapper += " emit({ manager: manager, person: person }, days)};";

        var map = new BsonJavaScript(mapper);
        var reduce = new BsonJavaScript("function(Manager, val) { return Array.sum(val); };");
        var coll = serviceOnboarding._context._db.GetCollection<BsonDocument>("Person");
        var options = new MapReduceOptions<BsonDocument, ViewListMapPersonManager>();
        FilterDefinition<Person> filters = null;

        if (managers.Count > 0)
          filters = Builders<Person>.Filter.Where(p => p._idAccount == _user._idAccount && p.Status == EnumStatus.Enabled
            && p.StatusUser != EnumStatusUser.Disabled
            && (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
            && !onboardings.Contains(p._id) && filtermanager.Contains(p.Manager._id)
            && p.Manager != null);
        else
          filters = Builders<Person>.Filter.Where(p => p._idAccount == _user._idAccount && p.Status == EnumStatus.Enabled
          && p.StatusUser != EnumStatusUser.Disabled
          && (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
          && !onboardings.Contains(p._id)
          && p.Manager != null);
        var json = filters.RenderToBsonDocument().ToJson();
        options.Filter = json;
        options.OutputOptions = MapReduceOutputOptions.Inline;
        var result = coll.MapReduce(map, reduce, options).ToList();

        list = result.Where(p => (parameter - p.value) < 10 && (parameter - p.value) >= 0).Select(p => new ViewListPending()
        {
          Person = p._id.person,
          Manager = p._id.manager,
          Days = parameter - long.Parse(p.value.ToString())
        }).ToList();

        return list.OrderBy(p => p.Days).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPending> OnboardingLateMap(List<_ViewList> managers)
    {
      try
      {
        List<string> filtermanager = null;
        if (managers.Count > 0)
          filtermanager = managers.Select(p => p._id).ToList();

        var list = new List<ViewListPending>();
        var parameterget = serviceParameter.GetNewVersion(p => p.Key == "DeadlineAdmOnboarding").Result;
        long parameter = 90;
        if (parameterget != null)
          parameter = long.Parse(parameterget.Content);

        var onboardings = serviceOnboarding.GetAllNewVersion(p => p.StatusOnBoarding == EnumStatusOnBoarding.End).Result.Select(p => p.Person._id).ToList();

        string mapper = "function() {";
        mapper += " var manager = null;";
        mapper += " var person = null;";
        mapper += " if (this.Manager != null)";
        mapper += " manager = this.Manager.Name;";
        mapper += " if (this.User != null){";
        mapper += " person = this.User.Name; } ";
        mapper += " var getdate = new Date();";
        mapper += " var dateadm = this.User.DateAdm;";
        mapper += " if (this.TypeJourney == 3) {";
        mapper += " if (this.DateLastOccupation != null)";
        mapper += " dateadm = this.DateLastOccupation; }";
        //mapper += " if (dateadm == null)";
        //mapper += " dateadm = getdate;";
        mapper += " var datea = new Date(dateadm);";
        mapper += " var diff = Date.parse(getdate) - Date.parse(datea.toString());";
        mapper += " var days = Math.floor(diff / 86400000);";
        mapper += " emit({ manager: manager, person: person }, days)};";

        var map = new BsonJavaScript(mapper);
        var reduce = new BsonJavaScript("function(Manager, val) { return Array.sum(val); };");
        var coll = serviceOnboarding._context._db.GetCollection<BsonDocument>("Person");
        var options = new MapReduceOptions<BsonDocument, ViewListMapPersonManager>();
        FilterDefinition<Person> filters = null;

        if (managers.Count > 0)
          filters = Builders<Person>.Filter.Where(p => p._idAccount == _user._idAccount && p.Status == EnumStatus.Enabled
            && p.StatusUser != EnumStatusUser.Disabled
            && (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
            && !onboardings.Contains(p._id) && filtermanager.Contains(p.Manager._id)
            && p.Manager != null);
        else
          filters = Builders<Person>.Filter.Where(p => p._idAccount == _user._idAccount && p.Status == EnumStatus.Enabled
          && p.StatusUser != EnumStatusUser.Disabled
          && (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation)
          && !onboardings.Contains(p._id)
          && p.Manager != null);

        var json = filters.RenderToBsonDocument().ToJson();
        options.Filter = json;
        options.OutputOptions = MapReduceOutputOptions.Inline;
        var result = coll.MapReduce(map, reduce, options).ToList();

        list = result.Where(p => (parameter - p.value) < 0).Select(p => new ViewListPending()
        {
          Person = p._id.person,
          Manager = p._id.manager,
          Days = (parameter - long.Parse(p.value.ToString())) * -1
        }).ToList();

        return list.OrderByDescending(p => p.Days).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewExportLogs> ExportLogs()
    {
      try
      {
        var logs = serviceLog.GetAllNewVersion(p => p.DataLog >= DateTime.Now.AddMonths(-3)).Result;

        return logs.Select(p => new ViewExportLogs()
        {
          Date = p.DataLog,
          Description = p.Description,
          Local = p.Local,
          Person = p.Person?.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewAccessAccount AccessAccount()
    {
      try
      {
        var logs = serviceLog.GetAllNewVersion(p => p.DataLog >= DateTime.Now.AddMonths(-3)
        && p.Local == "Authentication").Result;

        var list = new List<ViewAccessAccountDay>();
        foreach (var item in logs.Where(p => p.DataLog >= DateTime.Now.AddDays(-15)))
        {
          var day = item.DataLog.Value.ToString("dd/MM/yyyy");
          if (list.Where(p => p.Day == day).Count() > 0)
          {
            foreach (var lst in list)
            {
              if (lst.Day == day)
                lst.Qtd += 1;
            }
          }
          else
            list.Add(new ViewAccessAccountDay()
            {
              Day = day,
              Qtd = 1
            });

        }

        var view = new ViewAccessAccount()
        {
          Last3Month = logs.Count(),
          Days = list
        };

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPending> CheckpointInDayMap(List<_ViewList> managers)
    {
      try
      {
        List<string> filtermanager = null;
        if (managers.Count > 0)
          filtermanager = managers.Select(p => p._id).ToList();

        var list = new List<ViewListPending>();
        var parameterget = serviceParameter.GetNewVersion(p => p.Key == "DeadlineAdm").Result;
        long parameter = 90;
        if (parameterget != null)
          parameter = long.Parse(parameterget.Content);

        var checkpoints = serviceCheckpoint.GetAllNewVersion(p => p.StatusCheckpoint == EnumStatusCheckpoint.End).Result.Select(p => p.Person._id).ToList();

        string mapper = "function() {";
        mapper += " var manager = null;";
        mapper += " var person = null;";
        mapper += " if (this.Manager != null)";
        mapper += " manager = this.Manager.Name;";
        mapper += " if (this.User != null)";
        mapper += " person = this.User.Name;";
        mapper += " var getdate = new Date();";
        mapper += " var dateadm = this.User.DateAdm;";
        //mapper += " if (dateadm == null)";
        //mapper += " dateadm = getdate;";
        mapper += " var datea = new Date(dateadm);";
        mapper += " var diff = Date.parse(getdate) - Date.parse(datea.toString());";
        mapper += " var days = Math.floor(diff / 86400000);";
        mapper += " emit({ manager: manager, person: person }, days)};";

        var map = new BsonJavaScript(mapper);
        var reduce = new BsonJavaScript("function(Manager, val) { return Array.sum(val); };");
        var coll = serviceOnboarding._context._db.GetCollection<BsonDocument>("Person");
        var options = new MapReduceOptions<BsonDocument, ViewListMapPersonManager>();
        FilterDefinition<Person> filters = null;

        if (managers.Count > 0)
          filters = Builders<Person>.Filter.Where(p => p._idAccount == _user._idAccount && p.Status == EnumStatus.Enabled
          && p.TypeJourney == EnumTypeJourney.Checkpoint && !checkpoints.Contains(p._id)
          && filtermanager.Contains(p.Manager._id)
          && p.Manager != null);
        else
          filters = Builders<Person>.Filter.Where(p => p._idAccount == _user._idAccount && p.Status == EnumStatus.Enabled
          && p.TypeJourney == EnumTypeJourney.Checkpoint && !checkpoints.Contains(p._id)
          && p.Manager != null);

        var json = filters.RenderToBsonDocument().ToJson();
        options.Filter = json;
        options.OutputOptions = MapReduceOutputOptions.Inline;
        var result = coll.MapReduce(map, reduce, options).ToList();

        list = result.Where(p => (parameter - p.value) >= 10).Select(p => new ViewListPending()
        {
          Person = p._id.person,
          Manager = p._id.manager,
          Days = parameter - long.Parse(p.value.ToString())
        }).ToList();

        return list.OrderBy(p => p.Days).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPending> CheckpointToWinMap(List<_ViewList> managers)
    {
      try
      {
        List<string> filtermanager = null;
        if (managers.Count > 0)
          filtermanager = managers.Select(p => p._id).ToList();

        var list = new List<ViewListPending>();
        var parameterget = serviceParameter.GetNewVersion(p => p.Key == "DeadlineAdm").Result;
        long parameter = 90;
        if (parameterget != null)
          parameter = long.Parse(parameterget.Content);

        var checkpoints = serviceCheckpoint.GetAllNewVersion(p => p.StatusCheckpoint == EnumStatusCheckpoint.End).Result.Select(p => p.Person._id).ToList();

        string mapper = "function() {";
        mapper += " var manager = null;";
        mapper += " var person = null;";
        mapper += " if (this.Manager != null)";
        mapper += " manager = this.Manager.Name;";
        mapper += " if (this.User != null)";
        mapper += " person = this.User.Name;";
        mapper += " var getdate = new Date();";
        mapper += " var dateadm = this.User.DateAdm;";
        //mapper += " if (dateadm == null)";
        //mapper += " dateadm = getdate;";
        mapper += " var datea = new Date(dateadm);";
        mapper += " var diff = Date.parse(getdate) - Date.parse(datea.toString());";
        mapper += " var days = Math.floor(diff / 86400000);";
        mapper += " emit({ manager: manager, person: person }, days)};";

        var map = new BsonJavaScript(mapper);
        var reduce = new BsonJavaScript("function(Manager, val) { return Array.sum(val); };");
        var coll = serviceOnboarding._context._db.GetCollection<BsonDocument>("Person");
        var options = new MapReduceOptions<BsonDocument, ViewListMapPersonManager>();
        FilterDefinition<Person> filters = null;

        if (managers.Count > 0)
          filters = Builders<Person>.Filter.Where(p => p._idAccount == _user._idAccount && p.Status == EnumStatus.Enabled
          && p.TypeJourney == EnumTypeJourney.Checkpoint && !checkpoints.Contains(p._id)
          && filtermanager.Contains(p.Manager._id)
          && p.Manager != null);
        else
          filters = Builders<Person>.Filter.Where(p => p._idAccount == _user._idAccount && p.Status == EnumStatus.Enabled
          && p.TypeJourney == EnumTypeJourney.Checkpoint && !checkpoints.Contains(p._id)
          && p.Manager != null);

        var json = filters.RenderToBsonDocument().ToJson();
        options.Filter = json;
        options.OutputOptions = MapReduceOutputOptions.Inline;
        var result = coll.MapReduce(map, reduce, options).ToList();

        list = result.Where(p => (parameter - p.value) < 10 && (parameter - p.value) >= 0).Select(p => new ViewListPending()
        {
          Person = p._id.person,
          Manager = p._id.manager,
          Days = parameter - long.Parse(p.value.ToString())
        }).ToList();

        return list.OrderBy(p => p.Days).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPending> CheckpointLateMap(List<_ViewList> managers)
    {
      try
      {
        List<string> filtermanager = null;
        if (managers.Count > 0)
          filtermanager = managers.Select(p => p._id).ToList();

        var list = new List<ViewListPending>();
        var parameterget = serviceParameter.GetNewVersion(p => p.Key == "DeadlineAdm").Result;
        long parameter = 90;
        if (parameterget != null)
          parameter = long.Parse(parameterget.Content);

        var checkpoints = serviceCheckpoint.GetAllNewVersion(p => p.StatusCheckpoint == EnumStatusCheckpoint.End).Result.Select(p => p.Person._id).ToList();

        string mapper = "function() {";
        mapper += " var manager = null;";
        mapper += " var person = null;";
        mapper += " if (this.Manager != null)";
        mapper += " manager = this.Manager.Name;";
        mapper += " if (this.User != null)";
        mapper += " person = this.User.Name;";
        mapper += " var getdate = new Date();";
        mapper += " var dateadm = this.User.DateAdm;";
        //mapper += " if (dateadm == null)";
        //mapper += " dateadm = getdate;";
        mapper += " var datea = new Date(dateadm);";
        mapper += " var diff = Date.parse(getdate) - Date.parse(datea.toString());";
        mapper += " var days = Math.floor(diff / 86400000);";
        mapper += " emit({ manager: manager, person: person }, days)};";

        var map = new BsonJavaScript(mapper);
        var reduce = new BsonJavaScript("function(Manager, val) { return Array.sum(val); };");
        var coll = serviceOnboarding._context._db.GetCollection<BsonDocument>("Person");
        var options = new MapReduceOptions<BsonDocument, ViewListMapPersonManager>();
        FilterDefinition<Person> filters = null;

        if (managers.Count > 0)
          filters = Builders<Person>.Filter.Where(p => p._idAccount == _user._idAccount && p.Status == EnumStatus.Enabled
          && p.TypeJourney == EnumTypeJourney.Checkpoint && !checkpoints.Contains(p._id)
          && filtermanager.Contains(p.Manager._id)
          && p.Manager != null);
        else
          filters = Builders<Person>.Filter.Where(p => p._idAccount == _user._idAccount && p.Status == EnumStatus.Enabled
          && p.TypeJourney == EnumTypeJourney.Checkpoint && !checkpoints.Contains(p._id)
          && p.Manager != null);

        var json = filters.RenderToBsonDocument().ToJson();
        options.Filter = json;
        options.OutputOptions = MapReduceOutputOptions.Inline;
        var result = coll.MapReduce(map, reduce, options).ToList();

        list = result.Where(p => (parameter - p.value) < 0).Select(p => new ViewListPending()
        {
          Person = p._id.person,
          Manager = p._id.manager,
          Days = (parameter - long.Parse(p.value.ToString())) * -1
        }).ToList();

        return list.OrderByDescending(p => p.Days).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
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
            var view = onboardings.Where(p => p.Person._id == item._id).FirstOrDefault();
            if (view == null || view?.StatusOnBoarding != EnumStatusOnBoarding.End)
              list.Add(new ViewListPending()
              {
                Manager = item.Manager,
                Person = item.Name,
                Days = days,
                StatusOnBoarding = view == null ? EnumStatusOnBoarding.WaitBegin.ToString() : view.StatusOnBoarding.ToString()
              });
          }
        }

        return list.OrderBy(p => p.Days).ToList();
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

        return list.OrderBy(p => p.Days).ToList();
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

        return list.OrderByDescending(p => p.Days).ToList();
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

        return list.OrderBy(p => p.Days).ToList();
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

        return list.OrderBy(p => p.Days).ToList();
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

        return list.OrderByDescending(p => p.Days).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewGetOffBoarding> GetOffBoarding(int count, int page, ref long total, string filter)
    {
      try
      {
        var list = new List<ViewGetOffBoarding>();
        int skip = (count * (page - 1));

        var offboardins = serviceOffboarding.GetAllNewVersion(p => p.Person.Name.Contains(filter) &&
        (p.Step1.StatusFormOffBoarding == EnumStatusFormOffBoarding.End || p.Step2.StatusFormOffBoarding == EnumStatusFormOffBoarding.End)).Result;

        foreach (var item in offboardins)
        {
          var view = new ViewGetOffBoarding();
          view.Person = item.Person.Name;
          view.Manager = item.Person.Manager;
          view.DateOff = (item.Step1.DateOff != null) ? item.Step1.DateOff : item.Step2.DateOff;
          view.DateRealized = (item.DateEndStep1 != null) ? item.DateEndStep1 : item.DateEndStep2;
          var hrtotal = 0;
          var managertotal = 0;
          foreach (var qt in item.Step1.Questions)
            hrtotal += qt.Mark == 1 ? 20 : qt.Mark == 2 ? 40 : qt.Mark == 3 ? 60 : qt.Mark == 4 ? 80 : qt.Mark == 5 ? 100 : 0;

          foreach (var qt in item.Step2.Questions)
            managertotal += qt.Mark == 1 ? 20 : qt.Mark == 2 ? 40 : qt.Mark == 3 ? 60 : qt.Mark == 4 ? 80 : qt.Mark == 5 ? 100 : 0;

          foreach (var qt in item.Step2.QuestionsManager)
            managertotal += qt.Mark == 1 ? 20 : qt.Mark == 2 ? 40 : qt.Mark == 3 ? 60 : qt.Mark == 4 ? 80 : qt.Mark == 5 ? 100 : 0;

          if (item.Step1.Questions.Count() > 0)
            view.ScoreHR = hrtotal / item.Step1.Questions.Count();

          if (item.Step2.Questions.Count() > 0)
            view.ScoreManager = managertotal / (item.Step2.Questions.Count());

          //if ((item.Step2.Questions.Count() + item.Step2.QuestionsManager.Count()) > 0)
          //view.ScoreManager = managertotal / (item.Step2.Questions.Count() + item.Step2.QuestionsManager.Count());

          view.Diff = view.ScoreHR - view.ScoreManager;
          if (view.Diff < 0)
            view.Diff = view.Diff * -1;

          list.Add(view);
        }

        total = list.Count();

        return list.OrderBy(p => p.Person).Skip(skip).Take(count).ToList();
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

        var salaryscale = serviceSalaryScale.CountNewVersion(p => p.Status == EnumStatus.Enabled).Result != 0;

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
              DateLastOccupation = false,
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
            if (item.DateLastOccupation == null)
              view.DateLastOccupation = true;

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
          plans = servicePlan.CountNewVersion(p => p._idMonitoring == moni._id).Result;
          if (moni.SkillsCompany != null)
          {
            foreach (var item in moni.SkillsCompany)
            {
              if (item.Comments != null)
                comments += item.Comments.Count();
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
              if (item.Praise != null)
                praises += 1;
            }
          }
        }

        view.Plans = plans;
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
        if (result.Count > 0)
        {
          view.Schedule = result.Average(p => p.Schedule);
          view.Realized = result.Average(p => p.Realized);
          view.Late = result.Average(p => p.Late);
          view.Balance = result.Average(p => p.Balance);
        }

        long ranking = 1;
        foreach (var item in result.OrderByDescending(p => p.Balance))
        {
          item.Ranking = ranking;
          ranking += 1;
        }

        view.List = result.OrderByDescending(p => p.Balance).Skip(skip).Take(count).ToList();

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListPlanQtdGerals GetPlanQtdMap(ViewFilterDate date, int count, int page, ref long total, string filter)
    {
      try
      {
        int skip = (count * (page - 1));
        var view = new ViewListPlanQtdGerals();

        string mapper = "function() { ";
        mapper += " if (this.Person != null) {";
        mapper += " if (this.Person.Manager != null) {";
        mapper += " if (this.SkillsCompany != null) {";
        mapper += " this.SkillsCompany.forEach((it) => {";
        mapper += " if (it.Plans != null) {";
        mapper += " var getdate = new Date();";
        mapper += " emit({ manager: this.Person.Manager, type: 0}, it.Plans.length);";
        mapper += " emit({ manager: this.Person.Manager, type: 1}, it.Plans.filter(function(el) { return el.StatusPlan != 0 }).length);";
        mapper += " emit({ manager: this.Person.Manager, type: 2}, it.Plans.filter(function(el) {";
        mapper += " return el.StatusPlan == 0&& el.Deadline < getdate }).length);";
        mapper += " } }); }";
        mapper += " if (this.Schoolings != null) {";
        mapper += " this.Schoolings.forEach((it) => {";
        mapper += " if (it.Plans != null) {";
        mapper += " var getdate = new Date();";
        mapper += " emit({ manager: this.Person.Manager, type: 0}, it.Plans.length);";
        mapper += " emit({ manager: this.Person.Manager, type: 1}, it.Plans.filter(function(el) { return el.StatusPlan != 0 }).length);";
        mapper += " emit({ manager: this.Person.Manager, type: 2}, it.Plans.filter(function(el) {";
        mapper += " return el.StatusPlan == 0 && el.Deadline < getdate }).length);";
        mapper += " } }); }";
        mapper += " if (this.Activities != null){";
        mapper += " this.Activities.forEach((it) => {";
        mapper += " if (it.Plans != null)  {";
        mapper += " var getdate = new Date();";
        mapper += " emit({ manager: this.Person.Manager, type: 0}, it.Plans.length);";
        mapper += " emit({ manager: this.Person.Manager, type: 1}, it.Plans.filter(function(el) { return el.StatusPlan != 0 }).length);";
        mapper += " emit({ manager: this.Person.Manager, type: 2}, it.Plans.filter(function(el) {";
        mapper += " return el.StatusPlan == 0 && el.Deadline < getdate }).length);";
        mapper += " } });";
        mapper += " }}}};";

        var map = new BsonJavaScript(mapper);
        var reduce = new BsonJavaScript("function(Manager, val) { return Array.sum(val); };");
        var coll = serviceOnboarding._context._db.GetCollection<BsonDocument>("Monitoring");
        var options = new MapReduceOptions<BsonDocument, ViewListMapCompose>();
        FilterDefinition<Monitoring> filters = null;

        filters = Builders<Monitoring>.Filter.Where(p => p._idAccount == _user._idAccount
        && p.DateEndEnd >= date.Begin && p.DateEndEnd <= date.End && p.Status == EnumStatus.Enabled && p.StatusMonitoring == EnumStatusMonitoring.End);

        var json = filters.RenderToBsonDocument().ToJson();
        options.Filter = json;
        options.OutputOptions = MapReduceOutputOptions.Inline;
        var result = coll.MapReduce(map, reduce, options).ToList();

        var list = new List<ViewPlanQtd>();

        list = result.GroupBy(x => new { x._id.manager })
            .Select(x => new ViewPlanQtd()
            {
              Manager = x.Key.manager,
              Schedule = long.Parse(x.Where(y => y._id.type == 0).Sum(y => y.value).ToString()),
              Realized = long.Parse(x.Where(y => y._id.type == 1).Sum(y => y.value).ToString()),
              Late = long.Parse(x.Where(y => y._id.type == 2).Sum(y => y.value).ToString()),
              Balance = long.Parse(x.Where(y => y._id.type == 1).Sum(y => y.value).ToString()) -
              long.Parse(x.Where(y => y._id.type == 2).Sum(y => y.value).ToString()),
            }).ToList();

        total = list.Where(p => p.Schedule > 0).Count();
        list = list.Where(p => p.Schedule > 0).ToList();
        if (result.Count > 0)
        {
          view.Schedule = list.Average(p => p.Schedule);
          view.Realized = list.Average(p => p.Realized);
          view.Late = list.Average(p => p.Late);
          view.Balance = list.Average(p => p.Balance);
        }

        long ranking = 1;
        foreach (var item in list.OrderByDescending(p => p.Balance))
        {
          item.Ranking = ranking;
          ranking += 1;
        }

        view.List = list.OrderByDescending(p => p.Balance).Skip(skip).Take(count).ToList();

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
            var plans = servicePlan.CountNewVersion(p => p._idMonitoring == moni._id).Result;
            lst.Plans = plans;

            if (moni.SkillsCompany != null)
            {
              foreach (var item in moni.SkillsCompany)
              {
                if (item.Comments != null)
                {
                  lst.Comments += item.Comments.Count();
                  lst.Total += item.Comments.Count();
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

        //list = list.Where(p => (p.Plans > 0 || p.Praises > 0 || p.Comments > 0)).ToList();
        list = list.ToList();

        if (list.Count > 0)
        {
          result.Comments = list.Average(p => p.Comments);
          result.Plans = list.Average(p => p.Plans);
          result.Praises = list.Average(p => p.Praises);
        }

        foreach (var item in list)
        {
          byte stars = 0;

          if (item.Praises > result.Praises)
            stars += 1;

          if (item.Comments > result.Comments)
            stars += 1;

          if (item.Plans > result.Plans)
            stars += 1;

          item.Stars = stars;
        }

        long ranking = 1;
        foreach (var item in list.OrderByDescending(p => p.Stars).ThenByDescending(p => p.Total))
        {
          item.Ranking = ranking;
          ranking += 1;
          if (item.Praises > result.Praises)
            item.PraisesAvg = true;
          else
            item.PraisesAvg = false;

          if (item.Comments > result.Comments)
            item.CommentsAvg = true;
          else
            item.CommentsAvg = false;

          if (item.Plans > result.Plans)
            item.PlansAvg = true;
          else
            item.PlansAvg = false;
        }

        result.List = list.Where(p => (p.Plans > 0 || p.Praises > 0 || p.Comments > 0) && p.Manager.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Ranking).Skip(skip).Take(count).ToList();
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListMonitoringQtdManagerGeral GetMoninitoringQtdManagerMap(ViewFilterDate date, string idManager, int count, int page, ref long total, string filter)
    {
      try
      {

        int skip = (count * (page - 1));
        List<string> persons = null;
        if (idManager == string.Empty)
          persons = servicePerson.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result.Select(p => p._id).ToList();
        else
          persons = servicePerson.GetAllNewVersion(p => p.Manager._id == idManager).Result.Select(p => p._id).ToList();


        string mapper = "function() { ";
        mapper += "if(this.Person != null){";
        mapper += " if(this.Person.Manager != null){";
        mapper += " if (this.SkillsCompany != null){";
        mapper += " this.SkillsCompany.forEach((it) => {";
        mapper += " if (it.Comments != null)";
        mapper += " emit({ manager: this.Person.Manager, type: 0}, it.Comments.length);";
        mapper += " if (it.Plans != null)    ";
        mapper += " emit({ manager: this.Person.Manager, type: 1}, it.Plans.length); ";
        mapper += " if (it.Praise != null)";
        mapper += " emit({ manager: this.Person.Manager, type: 2}, 1);  ";
        mapper += " });";
        mapper += " }";
        mapper += " if (this.Schoolings != null){";
        mapper += " this.Schoolings.forEach((it) => {";
        mapper += " if (it.Comments != null)";
        mapper += " emit({ manager: this.Person.Manager, type: 0}, it.Comments.length);";
        mapper += " if (it.Plans != null)    ";
        mapper += " emit({ manager: this.Person.Manager, type: 1}, it.Plans.length); ";
        mapper += " if (it.Praise != null)";
        mapper += " emit({ manager: this.Person.Manager, type: 2}, 1); ";
        mapper += " });";
        mapper += " }";
        mapper += " if (this.Activities != null){";
        mapper += " this.Activities.forEach((it) => {";
        mapper += " if (it.Comments != null)";
        mapper += " emit({ manager: this.Person.Manager, type: 0}, it.Comments.length);";
        mapper += " if (it.Plans != null)    ";
        mapper += " emit({ manager: this.Person.Manager, type: 1}, it.Plans.length); ";
        mapper += " if (it.Praise != null)";
        mapper += " emit({ manager: this.Person.Manager, type: 2}, 1); ";
        mapper += " }); }}}};";

        var map = new BsonJavaScript(mapper);
        var reduce = new BsonJavaScript("function(Manager, val) { return Array.sum(val); };");
        var coll = serviceOnboarding._context._db.GetCollection<BsonDocument>("Monitoring");
        var options = new MapReduceOptions<BsonDocument, ViewListMapCompose>();
        FilterDefinition<Monitoring> filters = null;

        if (idManager != string.Empty)
          filters = Builders<Monitoring>.Filter.Where(p => p._idAccount == _user._idAccount
          && p.Status == EnumStatus.Enabled && p.StatusMonitoring == EnumStatusMonitoring.End
          && p.DateEndEnd >= date.Begin && p.DateEndEnd <= date.End && persons.Contains(p.Person._id));
        else
          filters = Builders<Monitoring>.Filter.Where(p => p._idAccount == _user._idAccount
          && p.DateEndEnd >= date.Begin && p.DateEndEnd <= date.End && p.Status == EnumStatus.Enabled && p.StatusMonitoring == EnumStatusMonitoring.End);

        var json = filters.RenderToBsonDocument().ToJson();
        options.Filter = json;
        options.OutputOptions = MapReduceOutputOptions.Inline;
        var result = coll.MapReduce(map, reduce, options).ToList();

        var view = new ViewListMonitoringQtdManagerGeral();
        var list = new List<ViewMoninitoringQtdManager>();


        list = result.GroupBy(x => x._id.manager)
            .Select(x => new ViewMoninitoringQtdManager()
            {
              Manager = x.Key,
              Comments = long.Parse(x.Where(y => y._id.type == 0).Sum(y => y.value).ToString()),
              Plans = long.Parse(x.Where(y => y._id.type == 1).Sum(y => y.value).ToString()),
              Praises = long.Parse(x.Where(y => y._id.type == 2).Sum(y => y.value).ToString()),
              Total = long.Parse(x.Sum(y => y.value).ToString())
            }).ToList();


        total = list.Count();

        if (list.Count > 0)
        {
          view.Comments = list.Average(p => p.Comments);
          view.Plans = list.Average(p => p.Plans);
          view.Praises = list.Average(p => p.Praises);
        }

        foreach (var item in list)
        {
          byte stars = 0;

          if (item.Praises > view.Praises)
            stars += 1;

          if (item.Comments > view.Comments)
            stars += 1;

          if (item.Plans > view.Plans)
            stars += 1;

          item.Stars = stars;
        }

        long ranking = 1;
        foreach (var item in list.OrderByDescending(p => p.Stars).ThenByDescending(p => p.Total))
        {
          item.Ranking = ranking;
          ranking += 1;
          if (item.Praises > view.Praises)
            item.PraisesAvg = true;
          else
            item.PraisesAvg = false;

          if (item.Comments > view.Comments)
            item.CommentsAvg = true;
          else
            item.CommentsAvg = false;

          if (item.Plans > view.Plans)
            item.PlansAvg = true;
          else
            item.PlansAvg = false;
        }

        view.List = list.Where(p => (p.Plans > 0 || p.Praises > 0 || p.Comments > 0) && p.Manager.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Ranking).Skip(skip).Take(count).ToList();
        return view;
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
          var plans = servicePlan.GetAllNewVersion(p => p._idMonitoring == item._id).Result;

          foreach (var row in item.SkillsCompany)
          {
            if (plans.Where(p => p._idItem == row._id).Count() > 0)
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
          var plans = servicePlan.GetAllNewVersion(p => p._idMonitoring == item._id).Result;
          foreach (var row in item.Activities)
          {
            foreach (var plan in plans)
            {
              foreach (var view in plan.Skills)
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
          var plans = servicePlan.GetAllNewVersion(p => p._id == item._id).Result;

          foreach (var row in item.SkillsCompany)
          {

            if (plans.Where(p => p._idItem == row._id).Count() > 0)
              listResult.Add(new ViewTagsCloudPerson() { Item = item.Activities.FirstOrDefault()?.Activities?.Name, Text = row.Skill.Name, Person = item.Person?.Name });
          }

          foreach (var row in item.SkillsCompany)
          {
            if (plans.Where(p => p._idItem == row._id).Count() > 0)
              listResult.Add(new ViewTagsCloudPerson() { Item = row.Skill.Name, Text = row.Skill.Name, Person = item.Person?.Name });

          }

          foreach (var row in item.SkillsGroup)
          {

            if (plans.Where(p => p._idItem == row._id).Count() > 0)
              listResult.Add(new ViewTagsCloudPerson() { Item = row.Skill.Name, Text = row.Skill.Name, Person = item.Person?.Name });
          }

          foreach (var row in item.SkillsOccupation)
          {
            if (plans.Where(p => p._idItem == row._id).Count() > 0)
              listResult.Add(new ViewTagsCloudPerson() { Item = row.Skill.Name, Text = row.Skill.Name, Person = item.Person?.Name });
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
          var plans = servicePlan.GetAllNewVersion(p => p._idMonitoring == item._id).Result;
          foreach (var row in plans.Where(p => p.SourcePlan == EnumSourcePlan.Activite))
          {
            foreach (var view in row.Skills)
            {
              listResult.Add(new ViewTagsCloudPerson() { Item = row.Name, Text = view.Name, Person = item.Person?.Name });
            }
          }

          foreach (var row in item.SkillsCompany)
          {

            if (plans.Where(p => p.SourcePlan == EnumSourcePlan.Skill && p._idItem == row._id).Count() > 0)
              listResult.Add(new ViewTagsCloudPerson() { Item = row.Skill.Name, Text = row.Skill.Name, Person = item.Person?.Name });

          }

          foreach (var row in item.SkillsGroup)
          {

            if (plans.Where(p => p.SourcePlan == EnumSourcePlan.SkillGroup && p._idItem == row._id).Count() > 0)
              listResult.Add(new ViewTagsCloudPerson() { Item = row.Skill.Name, Text = row.Skill.Name, Person = item.Person?.Name });
          }

          foreach (var row in item.SkillsOccupation)
          {
            if (plans.Where(p => p.SourcePlan == EnumSourcePlan.SkillOccupation && p._idItem == row._id).Count() > 0)
              listResult.Add(new ViewTagsCloudPerson() { Item = row.Skill.Name, Text = row.Skill.Name, Person = item.Person?.Name });
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

    public ViewListPlanQtd GetListPlanQtdMap(ViewFilterDate date, string idManager)
    {
      try
      {
        var view = new ViewListPlanQtd();
        string mapper = "function() { ";
        mapper += " if (this.Person != null) {";
        mapper += " if (this.Person.Manager != null) {";
        mapper += " if (this.SkillsCompany != null) {";
        mapper += " this.SkillsCompany.forEach((it) => {";
        mapper += " if (it.Plans != null) {";
        mapper += " var getdate = new Date();";
        mapper += " emit({ manager: this.Person.Manager, type: 0}, it.Plans.length);";
        mapper += " emit({ manager: this.Person.Manager, type: 1}, it.Plans.filter(function(el) { return el.StatusPlan != 0 }).length);";
        mapper += " emit({ manager: this.Person.Manager, type: 2}, it.Plans.filter(function(el) {";
        mapper += " return el.StatusPlan == 0&& el.Deadline < getdate }).length);";
        mapper += " } }); }";
        mapper += " if (this.Schoolings != null) {";
        mapper += " this.Schoolings.forEach((it) => {";
        mapper += " if (it.Plans != null) {";
        mapper += " var getdate = new Date();";
        mapper += " emit({ manager: this.Person.Manager, type: 0}, it.Plans.length);";
        mapper += " emit({ manager: this.Person.Manager, type: 1}, it.Plans.filter(function(el) { return el.StatusPlan != 0 }).length);";
        mapper += " emit({ manager: this.Person.Manager, type: 2}, it.Plans.filter(function(el) {";
        mapper += " return el.StatusPlan == 0 && el.Deadline < getdate }).length);";
        mapper += " } }); }";
        mapper += " if (this.Activities != null){";
        mapper += " this.Activities.forEach((it) => {";
        mapper += " if (it.Plans != null)  {";
        mapper += " var getdate = new Date();";
        mapper += " emit({ manager: this.Person.Manager, type: 0}, it.Plans.length);";
        mapper += " emit({ manager: this.Person.Manager, type: 1}, it.Plans.filter(function(el) { return el.StatusPlan != 0 }).length);";
        mapper += " emit({ manager: this.Person.Manager, type: 2}, it.Plans.filter(function(el) {";
        mapper += " return el.StatusPlan == 0 && el.Deadline < getdate }).length);";
        mapper += " } });";
        mapper += " }}}};";

        var map = new BsonJavaScript(mapper);
        var reduce = new BsonJavaScript("function(Manager, val) { return Array.sum(val); };");
        var coll = serviceOnboarding._context._db.GetCollection<BsonDocument>("Monitoring");
        var options = new MapReduceOptions<BsonDocument, ViewListMapCompose>();
        FilterDefinition<Monitoring> filters = null;

        filters = Builders<Monitoring>.Filter.Where(p => p._idAccount == _user._idAccount
        && p.DateEndEnd >= date.Begin && p.DateEndEnd <= date.End && p.Status == EnumStatus.Enabled && p.StatusMonitoring == EnumStatusMonitoring.End);

        var json = filters.RenderToBsonDocument().ToJson();
        options.Filter = json;
        options.OutputOptions = MapReduceOutputOptions.Inline;
        var result = coll.MapReduce(map, reduce, options).ToList();

        var list = new List<ViewPlanQtd>();

        list = result.GroupBy(x => new { x._id.manager })
             .Select(x => new ViewPlanQtd()
             {
               Manager = x.Key.manager,
               Schedule = long.Parse(x.Where(y => y._id.type == 0).Sum(y => y.value).ToString()),
               Realized = long.Parse(x.Where(y => y._id.type == 1).Sum(y => y.value).ToString()),
               Late = long.Parse(x.Where(y => y._id.type == 2).Sum(y => y.value).ToString()),
               Balance = long.Parse(x.Where(y => y._id.type == 1).Sum(y => y.value).ToString()) -
               long.Parse(x.Where(y => y._id.type == 2).Sum(y => y.value).ToString()),
             }).ToList();

        view.Schedules = list.Sum(p => p.Schedule);
        view.Ends = list.Sum(p => p.Realized);
        view.Lates = list.Sum(p => p.Late);

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
        var showsalary = serviceUser.GetNewVersion(p => p._id == _user._idUser).Result.ShowSalary;

        var persons = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled)
          .Result.Select(p => new ViewAccountEnableds()
          {
            Person = p.User?.Name,
            DateAdm = p.User?.DateAdm,
            DateLastOccupation = p.DateLastOccupation,
            Mail = p.User?.Mail,
            Manager = p.Manager?.Name,
            Document = p.User?.Document,
            Salary = (showsalary == true) ? p.Salary : 0,
            OccupationName = p.Occupation?.Name + (p.Occupation?.Description == null ? "" : p.Occupation?.Description),
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
        var persons = servicePerson.GetAllNewVersion(p => p.Manager._id == id).Result;
        var idmonitorings = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End).Result.Select(p => p._id);
        long onboardings = 0;
        long monitorings = 0;
        long workflows = 0;
        long plans = 0;
        foreach (var item in persons)
        {

          plans += servicePlan.GetAllNewVersion(p => p.Person._id == item._id & p.StatusPlan == EnumStatusPlan.Open && p.StatusPlanApproved == EnumStatusPlanApproved.Approved).Result.Where(p => idmonitorings.Contains(p._idMonitoring)).Count();

          var countonboardings = serviceOnboarding.CountNewVersion(p => p.Person._id == item._id & (p.StatusOnBoarding == EnumStatusOnBoarding.WaitManager || p.StatusOnBoarding == EnumStatusOnBoarding.WaitManagerRevision || p.StatusOnBoarding == EnumStatusOnBoarding.InProgressManager)).Result;
          var countmonitorings = serviceMonitoring.CountNewVersion(p => p.Person._id == item._id & (p.StatusMonitoring == EnumStatusMonitoring.WaitManager || p.StatusMonitoring == EnumStatusMonitoring.InProgressManager)).Result;

          if ((item.TypeJourney == EnumTypeJourney.OnBoarding) || (item.TypeJourney == EnumTypeJourney.OnBoardingOccupation))
            onboardings += countonboardings;

          if (item.TypeJourney == EnumTypeJourney.Monitoring)
            monitorings += countmonitorings;
          if (((item.TypeJourney == EnumTypeJourney.OnBoarding) || (item.TypeJourney == EnumTypeJourney.OnBoardingOccupation)) && (countonboardings == 0) && (serviceOnboarding.CountNewVersion(p => p.Person._id == item._id & p.StatusOnBoarding == EnumStatusOnBoarding.End).Result == 0))
          {
            onboardings += 1;
          }
          //if ((item.TypeJourney == EnumTypeJourney.Monitoring) && (countmonitorings == 0) && (serviceMonitoring.CountNewVersion(p => p.Person._id == item._id & p.StatusMonitoring == EnumStatusMonitoring.End).Result == 0))
          if ((item.TypeJourney == EnumTypeJourney.Monitoring) && (countmonitorings == 0))
          {
            monitorings += 1;
          }

        }

        workflows = serviceWorkflow.CountNewVersion(p => p.Requestor._id == id & p.StatusWorkflow == EnumWorkflow.Open).Result;

        totalqtd = monitorings + onboardings + workflows + plans;

        result.Add(new ViewIndicatorsNotes() { Type = EnumTypeWork.Monitoring, Qtd = monitorings, Total = totalqtd });
        result.Add(new ViewIndicatorsNotes() { Type = EnumTypeWork.OnBoarding, Qtd = onboardings, Total = totalqtd });
        result.Add(new ViewIndicatorsNotes() { Type = EnumTypeWork.Workflow, Qtd = workflows, Total = totalqtd });
        result.Add(new ViewIndicatorsNotes() { Type = EnumTypeWork.Plan, Qtd = plans, Total = totalqtd });

        return result.OrderBy(p => p.Type).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTagsCloud> ListTagsCloudCompany(string idmanager, string idperson)
    {
      try
      {
        var list = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End).Result.ToList();
        var persons = servicePerson.GetAllNewVersion(p => p.Manager._id == idmanager).Result.Select(p => p._id).ToList();

        if (idperson != "")
          list = list.Where(p => p.Person._id == idperson).ToList();

        foreach (var item in list)
        {
          if (persons.Where(p => p == item.Person?._id).Count() == 0)
            list.Where(p => p._id == item._id).FirstOrDefault().Status = EnumStatus.Disabled;
        }
        list = list.Where(p => p.Status == EnumStatus.Enabled).ToList();

        List<ViewTagsCloud> listResult = new List<ViewTagsCloud>();
        foreach (var item in list)
        {
          var plans = servicePlan.GetAllNewVersion(p => p._idMonitoring == item._id).Result;
          foreach (var row in item.SkillsCompany)
          {
            if (plans.Where(p => p._idItem == row._id).Count() > 0)
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

    public List<ViewTagsCloudFull> ListTagsCloudFull(string idmanager, string idperson)
    {
      try
      {
        var list = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End).Result.ToList();

        if (idperson != "")
          list = list.Where(p => p.Person._id == idperson).ToList();

        if (idmanager != "")
        {
          var persons = servicePerson.GetAllNewVersion(p => p.Manager._id == idmanager).Result.Select(p => p._id).ToList();
          foreach (var item in list)
          {
            if (persons.Where(p => p == item.Person?._id).Count() == 0)
              list.Where(p => p._id == item._id).FirstOrDefault().Status = EnumStatus.Disabled;
          }

          list = list.Where(p => p.Status == EnumStatus.Enabled).ToList();
        }



        var listResult = new List<ViewTagsCloudFull>();
        foreach (var item in list)
        {
          var plans = servicePlan.GetAllNewVersion(p => p._idMonitoring == item._id).Result;

          foreach (var skill in plans.Where(p => p.SourcePlan == EnumSourcePlan.Activite))
          {
            foreach (var view in skill.Skills)
            {
              listResult.Add(new ViewTagsCloudFull() { text = view.Name, color = "#c7750a" });
            }
          }


          foreach (var row in item.SkillsCompany)
          {
            if (plans.Where(p => p._idItem == row._id).Count() > 0)
              listResult.Add(new ViewTagsCloudFull() { text = row.Skill.Name, color = "#0ac79f" });

          }

          foreach (var row in item.SkillsGroup)
          {
            if (plans.Where(p => p._idItem == row._id).Count() > 0)
              listResult.Add(new ViewTagsCloudFull() { text = row.Skill.Name, color = "#c7750a" });
          }

          foreach (var row in item.SkillsOccupation)
          {
            if (plans.Where(p => p._idItem == row._id).Count() > 0)
              listResult.Add(new ViewTagsCloudFull() { text = row.Skill.Name, color = "#c7750a" });
          }
        }


        var result = listResult.GroupBy(x => new { x.text, x.color })
            .Select(x => new ViewTagsCloudFull()
            {
              text = x.Key.text,
              color = x.Key.color,
              weight = x.Count()
            }).ToList();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTagsCloudFull> ListTagsCloudFullRH(ViewFilterManagerAndDate filters, string idmanager, string idperson)
    {
      try
      {
        var list = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End
        && p.DateEndEnd >= filters.Date.Begin && p.DateEndEnd <= filters.Date.End).Result.ToList();

        if (idperson != "")
          list = list.Where(p => p.Person._id == idperson).ToList();


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
        }

        if (idmanager != "")
        {
          var persons = servicePerson.GetAllNewVersion(p => p.Manager._id == idmanager).Result.Select(p => p._id).ToList();
          foreach (var item in list)
          {
            if (persons.Where(p => p == item.Person?._id).Count() == 0)
              list.Where(p => p._id == item._id).FirstOrDefault().Status = EnumStatus.Disabled;
          }

        }

        list = list.Where(p => p.Status == EnumStatus.Enabled).ToList();

        var listResult = new List<ViewTagsCloudFull>();
        foreach (var item in list)
        {
          var plans = servicePlan.GetAllNewVersion(p => p._idMonitoring == item._id).Result;

          foreach (var skill in plans.Where(p => p.SourcePlan == EnumSourcePlan.Activite))
          {
            foreach (var view in skill.Skills)
            {
              listResult.Add(new ViewTagsCloudFull() { text = view.Name, color = "#c7750a" });
            }
          }


          foreach (var row in item.SkillsCompany)
          {
            if (plans.Where(p => p._idItem == row._id).Count() > 0)
              listResult.Add(new ViewTagsCloudFull() { text = row.Skill.Name, color = "#0ac79f" });

          }

          foreach (var row in item.SkillsGroup)
          {
            if (plans.Where(p => p._idItem == row._id).Count() > 0)
              listResult.Add(new ViewTagsCloudFull() { text = row.Skill.Name, color = "#c7750a" });
          }

          foreach (var row in item.SkillsOccupation)
          {
            if (plans.Where(p => p._idItem == row._id).Count() > 0)
              listResult.Add(new ViewTagsCloudFull() { text = row.Skill.Name, color = "#c7750a" });
          }
        }


        var result = listResult.GroupBy(x => new { x.text, x.color })
            .Select(x => new ViewTagsCloudFull()
            {
              text = x.Key.text,
              color = x.Key.color,
              weight = x.Count()
            }).ToList();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }



    public List<ViewTagsCloud> ListTagsCloud(string idmanager, string idperson)
    {
      try
      {
        var list = serviceMonitoring.GetAllNewVersion(p => p.StatusMonitoring == EnumStatusMonitoring.End).Result.ToList();
        var persons = servicePerson.GetAllNewVersion(p => p.Manager._id == idmanager).Result.Select(p => p._id).ToList();

        if (idperson != "")
          list = list.Where(p => p.Person._id == idperson).ToList();

        foreach (var item in list)
        {
          if (persons.Where(p => p == item.Person?._id).Count() == 0)
            list.Where(p => p._id == item._id).FirstOrDefault().Status = EnumStatus.Disabled;
        }
        list = list.Where(p => p.Status == EnumStatus.Enabled).ToList();

        List<ViewTagsCloud> listResult = new List<ViewTagsCloud>();
        foreach (var item in list)
        {
          var plans = servicePlan.GetAllNewVersion(p => p._idMonitoring == item._id).Result;

          foreach (var skill in plans.Where(p => p.SourcePlan == EnumSourcePlan.Activite))
          {
            foreach (var view in skill.Skills)
            {
              listResult.Add(new ViewTagsCloud() { text = view.Name });
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
        var fluidcarrers = serviceFluidCareers.GetAllNewVersion(p => p.Person._id == idperson).Result;

        List<ViewTagsCloud> listResult = new List<ViewTagsCloud>();
        foreach (var item in list)
        {
          var plans = servicePlan.GetAllNewVersion(p => p._idMonitoring == item._id).Result;
          foreach (var row in plans.Where(p => p.SourcePlan == EnumSourcePlan.Activite))
          {
            foreach (var view in row.Skills)
            {
              listResult.Add(new ViewTagsCloud() { text = view.Name });
            }
          }
        }

        foreach (var item in fluidcarrers)
        {
          if (item.Plan != null)
          {
            foreach (var row in item.Plan.Skills)
              listResult.Add(new ViewTagsCloud() { text = row.Name });
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
        var fluidcarrers = serviceFluidCareers.GetAllNewVersion(p => p.Person._id == idperson).Result;

        List<ViewTagsCloud> listResult = new List<ViewTagsCloud>();
        foreach (var item in list)
        {
          var plans = servicePlan.GetAllNewVersion(p => p._idMonitoring == item._id).Result;
          foreach (var row in item.SkillsCompany)
          {
            if (plans.Where(p => p._idItem == row._id).Count() > 0)
              listResult.Add(new ViewTagsCloud() { text = row.Skill.Name });
          }
        }

        foreach (var item in fluidcarrers)
        {
          if (item.Plan != null)
          {
            foreach (var row in item.Plan.Skills)
              listResult.Add(new ViewTagsCloud() { text = row.Name });
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
        var monitorings = serviceMonitoring.CountNewVersion(p => p.Person._id == id & p.StatusMonitoring != EnumStatusMonitoring.InProgressManager & p.StatusMonitoring != EnumStatusMonitoring.WaitManager & p.StatusMonitoring != EnumStatusMonitoring.End & p.StatusMonitoring != EnumStatusMonitoring.Show).Result;
        var onboardings = serviceOnboarding.CountNewVersion(p => p.Person.TypeJourney == EnumTypeJourney.OnBoarding && p.Person._id == id & p.StatusOnBoarding != EnumStatusOnBoarding.InProgressManager & p.StatusOnBoarding != EnumStatusOnBoarding.WaitManager & p.StatusOnBoarding != EnumStatusOnBoarding.End).Result;
        var onboardingsoccupation = serviceOnboarding.CountNewVersion(p => p.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation && p.Person._id == id & p.StatusOnBoarding != EnumStatusOnBoarding.InProgressManager & p.StatusOnBoarding != EnumStatusOnBoarding.WaitManager & p.StatusOnBoarding != EnumStatusOnBoarding.End).Result;
        var workflows = serviceWorkflow.CountNewVersion(p => p.Requestor._id == id & p.StatusWorkflow == EnumWorkflow.Open).Result;

        totalqtd = monitorings + onboardings + workflows + onboardingsoccupation;
        result.Add(new ViewIndicatorsNotes() { Type = EnumTypeWork.Monitoring, Qtd = monitorings, Total = totalqtd });
        result.Add(new ViewIndicatorsNotes() { Type = EnumTypeWork.OnBoarding, Qtd = onboardings, Total = totalqtd });
        result.Add(new ViewIndicatorsNotes() { Type = EnumTypeWork.OnBoardingOccupation, Qtd = onboardingsoccupation, Total = totalqtd });
        result.Add(new ViewIndicatorsNotes() { Type = EnumTypeWork.Workflow, Qtd = workflows, Total = totalqtd });

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

    ////public void UpdateStatusPlanMonitoring()
    ////{
    ////  try
    ////  {
    ////    var monitorings = serviceMonitoring.GetAllFreeNewVersion(p => p.Status == EnumStatus.Enabled
    ////    && p.StatusMonitoring == EnumStatusMonitoring.End).Result;
    ////    foreach (var moni in monitorings)
    ////    {
    ////      if (moni.SkillsCompany != null)
    ////      {
    ////        foreach (var item in moni.SkillsCompany)
    ////        {
    ////          if (item.Plans != null)
    ////          {
    ////            foreach (var plan in item.Plans)
    ////            {
    ////              var status = servicePlan.GetNewVersion(p => p._id == plan._id).Result.StatusPlan;
    ////              plan.StatusPlan = status;
    ////            }
    ////          }
    ////        }
    ////      }

    ////      if (moni.Schoolings != null)
    ////      {
    ////        foreach (var item in moni.Schoolings)
    ////        {
    ////          if (item.Plans != null)
    ////          {
    ////            foreach (var plan in item.Plans)
    ////            {
    ////              var status = servicePlan.GetNewVersion(p => p._id == plan._id).Result.StatusPlan;
    ////              plan.StatusPlan = status;
    ////            }
    ////          }
    ////        }
    ////      }

    ////      if (moni.Activities != null)
    ////      {
    ////        foreach (var item in moni.Activities)
    ////        {
    ////          if (item.Plans != null)
    ////          {
    ////            foreach (var plan in item.Plans)
    ////            {
    ////              var status = servicePlan.GetNewVersion(p => p._id == plan._id).Result.StatusPlan;
    ////              plan.StatusPlan = status;
    ////            }
    ////          }
    ////        }
    ////      }
    ////    }
    ////  }
    ////  catch (Exception e)
    ////  {
    ////    throw e;
    ////  }
    ////}

    private void DoWork()
    {
      try
      {
        while (true)
        {
          foreach (var person in servicePerson.GetAllFreeNewVersion(p => p.Status != EnumStatus.Disabled & p.StatusUser != EnumStatusUser.Disabled).Result)
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

    public IEnumerable<ViewChartOnboarding> ChartOnboardingMap(List<ViewListIdIndicators> persons)
    {
      try
      {
        var filterperson = persons.Where(p => (p.TypeJourney == EnumTypeJourney.OnBoarding)
        || (p.TypeJourney == EnumTypeJourney.OnBoardingOccupation)).Select(p => p._id).ToList();
        var map = new BsonJavaScript(@" function() { emit(this.StatusOnBoarding, 1);};");
        var reduce = new BsonJavaScript("function(StatusOnboarding, count) { return Array.sum(count);};");
        var coll = serviceOnboarding._context._db.GetCollection<BsonDocument>("OnBoarding");
        var options = new MapReduceOptions<BsonDocument, ViewListMap>();
        var filter = Builders<OnBoarding>.Filter.Where(p => p._idAccount == _user._idAccount
        && p.Status == EnumStatus.Enabled && filterperson.Contains(p.Person._id));
        var json = filter.RenderToBsonDocument().ToJson();
        options.Filter = json;

        options.OutputOptions = MapReduceOutputOptions.Inline;


        var res = coll.MapReduce(map, reduce, options).ToList();

        var list = new List<ViewChartOnboarding>();
        foreach (var item in res)
        {
          list.Add(new ViewChartOnboarding()
          {
            Count = long.Parse(item.value.ToString()),
            Status = (EnumStatusOnBoarding)byte.Parse(item._id.ToString())
          });
        }

        return list;
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
        List<ViewChartStatus> result = new List<ViewChartStatus>
        {
          new ViewChartStatus() { Status = "Realizado", Count = 0 },
          new ViewChartStatus() { Status = "Não Realizado", Count = 0 }
        };

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
        List<ViewChartStatus> result = new List<ViewChartStatus>
        {
          new ViewChartStatus() { Status = "Realizado", Count = 0 },
          new ViewChartStatus() { Status = "Não Realizado", Count = 0 }
        };

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

        List<ViewChartStatus> result = new List<ViewChartStatus>
        {
          new ViewChartStatus() { Status = "Realizado", Count = 0 },
          new ViewChartStatus() { Status = "Não Realizado", Count = 0 }
        };

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
            var plans = servicePlan.GetAllNewVersion(p => p._idMonitoring == view._id).Result;

            foreach (var plan in plans)
            {
              result.Add(new
              {
                Name = item._id,
                Status = plan == null ? EnumStatusPlan.Open.ToString() : plan.StatusPlan.ToString()
              });
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
              view.Recommendation.Name,
              view.Person._id
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

    public IEnumerable<ViewChartRecommendation> ChartRecommendationPersons(ViewFilterIdAndDate filters, int count, int page, ref long total, string filter)
    {
      try
      {

        int skip = (count * (page - 1));

        var recommendations = serviceRecommendationPerson.GetAllNewVersion(p => p.Status == EnumStatus.Enabled
        && p.Date >= filters.Date.Begin && p.Date <= filters.Date.End).Result;

        List<dynamic> result = new List<dynamic>();

        foreach (var item in filters.Persons)
        {
          var list = recommendations.Where(p => p.Person._id == item._id);
          foreach (var view in list)
          {
            result.Add(new
            {
              view.Person.Name,
              view.Recommendation._id
            });
          }

        }

        var response = result.GroupBy(p => p.Name).Select(x => new ViewChartRecommendation
        {
          Name = x.Key,
          Count = x.Count()
        }).ToList();

        total = response.Count();
        return response.Where(p => p.Name.Contains(filter)).OrderByDescending(p => p.Count).Skip(skip).Take(count).ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public IEnumerable<ViewChartRecommendation> ChartRecommendationPersonsMap(ViewFilterManagerAndDate filters, int count, int page, ref long total, string filter)
    {
      try
      {

        int skip = (count * (page - 1));

        var recommendations = serviceRecommendationPerson.GetAllNewVersion(p => p.Status == EnumStatus.Enabled
        && p.Date >= filters.Date.Begin && p.Date <= filters.Date.End).Result;

        List<dynamic> result = new List<dynamic>();

        //foreach (var item in filters.Persons)
        //{
        //  var list = recommendations.Where(p => p.Person._id == item._id);
        //  foreach (var view in list)
        //  {
        //    result.Add(new
        //    {
        //      Name = view.Person.Name,
        //      _id = view.Recommendation._id
        //    });
        //  }

        //}

        var response = result.GroupBy(p => p.Name).Select(x => new ViewChartRecommendation
        {
          Name = x.Key,
          Count = x.Count()
        }).ToList();

        total = response.Count();
        return response.Where(p => p.Name.Contains(filter)).OrderByDescending(p => p.Count).Skip(skip).Take(count).ToList();

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
            var plans = servicePlan.GetAllNewVersion(p => p._idMonitoring == view._id).Result;
            foreach (var plan in plans)
            {
              result.Add(new
              {
                Name = item._id,
                Status = plan.StatusPlan == EnumStatusPlan.Realized ? "Realizado" : "Não Realizado"
              });
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
              view.Person._id
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
            Skill = item.CertificationItem.Name,
            ItemCertificationView = item.CertificationItem.ItemCertification == EnumItemCertification.SkillCompanyHard ? EnumItemCertificationView.Company :
                item.CertificationItem.ItemCertification == EnumItemCertification.SkillCompanySoft ? EnumItemCertificationView.Company :
                item.CertificationItem.ItemCertification == EnumItemCertification.SkillGroupHard ? EnumItemCertificationView.Hard :
                item.CertificationItem.ItemCertification == EnumItemCertification.SkillOccupationHard ? EnumItemCertificationView.Hard : EnumItemCertificationView.Soft
          });
        }

        return result.GroupBy(p => new { p.Skill, p.ItemCertificationView }).Select(x => new ViewChartCertificationCount
        {
          Item = x.Key.Skill,
          ItemCertificationView = x.Key.ItemCertificationView,
          Count = x.Count()
        }).OrderByDescending(p => p.Count).ToList();

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
            DateAdm = p.User?.DateAdm == null ? null : p.User.DateAdm.Value.ToString("dd/MM/yyyy")
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

    public List<ViewListScheduleManager> ListScheduleManager(ViewFilterDate date)
    {
      try
      {
        var list = new List<ViewListScheduleManager>();
        var manager = servicePerson.GetAllNewVersion(p => (p.TypeUser == EnumTypeUser.ManagerHR ||
        p.TypeUser == EnumTypeUser.Manager) && p.StatusUser != EnumStatusUser.Disabled).Result.OrderBy(p => p.Manager?.Name).ToList();

        foreach (var item in manager)
        {
          var team = servicePerson.GetAllNewVersion(p => p.Manager._id == item._id
            && p.StatusUser != EnumStatusUser.Disabled).Result.OrderBy(p => p.Manager?.Name).ToList();

          var persons = team.Select(p => p._id).ToList();

          var view = new ViewListScheduleManager
          {
            Manager = item.User?.Name,
            QtdTeam = team.Count(),
            Occupation = item.Occupation?.Name,
            Establishment = item.Establishment?.Name,

            QtdOnboarding = serviceOnboarding.GetAllNewVersion(p => persons.Contains(p.Person._id)
            && p.StatusOnBoarding == EnumStatusOnBoarding.End
            && p.Person.TypeJourney != EnumTypeJourney.OnBoardingOccupation
            && p.DateEndEnd >= date.Begin && p.DateEndEnd <= date.End).Result.Count(),

            QtdOnboardingOccupation = serviceOnboarding.GetAllNewVersion(p => persons.Contains(p.Person._id)
            && p.StatusOnBoarding == EnumStatusOnBoarding.End
            && p.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation
            && p.DateEndEnd >= date.Begin && p.DateEndEnd <= date.End).Result.Count(),

            QtdCheckpoint = serviceCheckpoint.GetAllNewVersion(p => persons.Contains(p.Person._id)
            && p.StatusCheckpoint == EnumStatusCheckpoint.End
            && p.DateEnd >= date.Begin && p.DateEnd <= date.End).Result.Count(),

            QtdMonitoring = serviceMonitoring.GetAllNewVersion(p => persons.Contains(p.Person._id)
            && p.StatusMonitoring == EnumStatusMonitoring.End
            && p.DateEndEnd >= date.Begin && p.DateEndEnd <= date.End).Result.Count()
          };

          list.Add(view);
        }

        return list;
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
            Mail = item.User?.Mail,
            Document = item.User?.Document,
            StatusUser = item.StatusUser
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

