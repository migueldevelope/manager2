﻿using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Manager.Views.Enumns;
using Manager.Views.BusinessView;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessList;
using Manager.Views.BusinessCrud;
using Manager.Core.Base;

namespace Manager.Services.Specific
{
#pragma warning disable 1998
#pragma warning disable 4014
  public class ServiceInfra : Repository<Group>, IServiceInfra
  {
    private readonly ServiceGeneric<Account> serviceAccount;
    private readonly ServiceGeneric<Area> serviceArea;
    private readonly ServiceGeneric<Axis> serviceAxis;
    private readonly ServiceGeneric<Cbo> serviceCbo;
    private readonly ServiceGeneric<Company> serviceCompany;
    private readonly ServiceGeneric<CompanyLog> serviceCompanyLog;
    private readonly ServiceGeneric<CompanyMandatory> serviceCompanyMandatory;
    private readonly ServiceGeneric<DictionarySphere> serviceDictionarySphere;
    private readonly ServiceGeneric<Group> serviceGroup;
    private readonly ServiceGeneric<GroupLog> serviceGroupLog;
    private readonly ServiceGeneric<MailModel> serviceMailModel;
    private readonly ServiceGeneric<Occupation> serviceOccupation;
    private readonly ServiceGeneric<OccupationLog> serviceOccupationLog;
    private readonly ServiceGeneric<OccupationMandatory> serviceOccupationMandatory;
    private readonly ServiceGeneric<Parameter> serviceParameter;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<ProcessLevelOne> serviceProcessLevelOne;
    private readonly ServiceGeneric<ProcessLevelTwo> serviceProcessLevelTwo;
    private readonly ServiceGeneric<Questions> serviceQuestions;
    private readonly ServiceGeneric<SalaryScale> serviceSalaryScales;
    private readonly ServiceGeneric<Schooling> serviceSchooling;
    private readonly ServiceGeneric<Skill> serviceSkill;
    private readonly ServiceGeneric<Sphere> serviceSphere;
    private readonly ServiceGeneric<TextDefault> serviceTextDefault;
    private readonly ServiceGeneric<Recommendation> serviceRecommendation;


    #region Constructor
    public ServiceInfra(DataContext context) : base(context)
    {
      try
      {
        serviceAccount = new ServiceGeneric<Account>(context);
        serviceArea = new ServiceGeneric<Area>(context);
        serviceAxis = new ServiceGeneric<Axis>(context);
        serviceCbo = new ServiceGeneric<Cbo>(context);
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceCompanyLog = new ServiceGeneric<CompanyLog>(context);
        serviceCompanyMandatory = new ServiceGeneric<CompanyMandatory>(context);
        serviceDictionarySphere = new ServiceGeneric<DictionarySphere>(context);
        serviceGroup = new ServiceGeneric<Group>(context);
        serviceGroupLog = new ServiceGeneric<GroupLog>(context);
        serviceMailModel = new ServiceGeneric<MailModel>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        serviceOccupationLog = new ServiceGeneric<OccupationLog>(context);
        serviceOccupationMandatory = new ServiceGeneric<OccupationMandatory>(context);
        serviceParameter = new ServiceGeneric<Parameter>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceProcessLevelOne = new ServiceGeneric<ProcessLevelOne>(context);
        serviceProcessLevelTwo = new ServiceGeneric<ProcessLevelTwo>(context);
        serviceQuestions = new ServiceGeneric<Questions>(context);
        serviceSalaryScales = new ServiceGeneric<SalaryScale>(context);
        serviceSchooling = new ServiceGeneric<Schooling>(context);
        serviceSkill = new ServiceGeneric<Skill>(context);
        serviceSphere = new ServiceGeneric<Sphere>(context);
        serviceTextDefault = new ServiceGeneric<TextDefault>(context);
        serviceRecommendation = new ServiceGeneric<Recommendation>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceAccount._user = _user;
      serviceArea._user = _user;
      serviceAxis._user = _user;
      serviceCbo._user = _user;
      serviceCompany._user = _user;
      serviceCompanyLog._user = _user;
      serviceCompanyMandatory._user = _user;
      serviceDictionarySphere._user = _user;
      serviceGroup._user = _user;
      serviceGroupLog._user = _user;
      serviceMailModel._user = _user;
      serviceOccupation._user = _user;
      serviceOccupationLog._user = _user;
      serviceOccupationMandatory._user = _user;
      serviceParameter._user = _user;
      servicePerson._user = _user;
      serviceProcessLevelOne._user = _user;
      serviceProcessLevelTwo._user = _user;
      serviceQuestions._user = _user;
      serviceSalaryScales._user = _user;
      serviceSchooling._user = _user;
      serviceSkill._user = _user;
      serviceSphere._user = _user;
      serviceTextDefault._user = _user;
      serviceRecommendation._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceAccount._user = user;
      serviceArea._user = user;
      serviceAxis._user = user;
      serviceCbo._user = user;
      serviceCompany._user = user;
      serviceCompanyLog._user = user;
      serviceCompanyMandatory._user = user;
      serviceDictionarySphere._user = user;
      serviceGroup._user = user;
      serviceGroupLog._user = user;
      serviceMailModel._user = user;
      serviceOccupation._user = user;
      serviceOccupationLog._user = user;
      serviceOccupationMandatory._user = user;
      serviceParameter._user = user;
      servicePerson._user = user;
      serviceProcessLevelOne._user = user;
      serviceProcessLevelTwo._user = user;
      serviceQuestions._user = user;
      serviceSalaryScales._user = user;
      serviceSchooling._user = user;
      serviceSkill._user = user;
      serviceSphere._user = user;
      serviceTextDefault._user = user;
      serviceRecommendation._user = user;
    }
    #endregion

    #region Copy Template New Account
    public async Task CopyTemplateInfraAsync(Company company)
    {
      try
      {
        // Identificação da conta raiz do ANALISA
        var idresolution = "5b6c4f47d9090156f08775aa";

        // Parameter
        Parameter parameterLocal;
        foreach (Parameter parameter in serviceParameter.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          parameterLocal = new Parameter()
          {
            Content = parameter.Content,
            Help = parameter.Help,
            Key = parameter.Key,
            Name = parameter.Name,
            Status = parameter.Status,
            _idAccount = _user._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          };
          var result = serviceParameter.InsertFreeNewVersion(parameterLocal).Result._id;
        }

        TextDefault textDefaultLocal;
        // Text default
        foreach (TextDefault textDefault in serviceTextDefault.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          textDefaultLocal = new TextDefault()
          {
            Template = textDefault._id,
            Company = company.GetViewList(),
            _idAccount = _user._idAccount,
            Content = textDefault.Content,
            Name = textDefault.Name,
            Status = EnumStatus.Enabled,
            TypeText = textDefault.TypeText,
            _id = ObjectId.GenerateNewId().ToString()
          };
          var result = serviceTextDefault.InsertFreeNewVersion(textDefaultLocal).Result._id;
        }

        // MailModel
        MailModel mailModelLocal;
        foreach (MailModel mailModel in serviceMailModel.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          mailModelLocal = new MailModel()
          {
            Link = mailModel.Link,
            Message = mailModel.Message,
            Name = mailModel.Name,
            Status = mailModel.Status,
            StatusMail = mailModel.StatusMail,
            Subject = mailModel.Subject,
            _idAccount = _user._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          };
          var id = serviceMailModel.InsertFreeNewVersion(mailModelLocal).Result._id;
        }

        // Questions
        Questions questionsLocal;
        foreach (Questions question in serviceQuestions.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          questionsLocal = new Questions()
          {
            Template = question._id,
            Content = question.Content,
            Company = company.GetViewList(),
            Name = question.Name,
            Order = question.Order,
            Status = question.Status,
            TypeQuestion = question.TypeQuestion,
            TypeRotine = question.TypeRotine,
            _idAccount = _user._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          };
          serviceQuestions.InsertFreeNewVersion(questionsLocal).Result.GetViewList();
        }

        // Skill
        Skill skillLocal;
        foreach (Skill item in serviceSkill.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          skillLocal = new Skill
          {
            Template = item._id,
            Concept = item.Concept,
            Name = item.Name,
            Status = item.Status,
            TypeSkill = item.TypeSkill,
            _idAccount = _user._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          };
          Skill result = serviceSkill.InsertFreeNewVersion(skillLocal).Result;
        }

        // Schooling
        Schooling schoolingLocal;
        foreach (Schooling item in serviceSchooling.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          schoolingLocal = new Schooling()
          {
            Template = item._id,
            Name = item.Name,
            Order = item.Order,
            Status = item.Status,
            Type = item.Type,
            _idAccount = _user._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          };
          Schooling result = serviceSchooling.InsertFreeNewVersion(schoolingLocal).Result;
        }

        // Sphere
        Sphere sphereLocal;
        foreach (Sphere item in serviceSphere.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          sphereLocal = new Sphere()
          {
            Template = item._id,
            Company = company.GetViewList(),
            Name = item.Name,
            Status = item.Status,
            TypeSphere = item.TypeSphere,
            _idAccount = _user._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          };
          Sphere result = serviceSphere.InsertFreeNewVersion(sphereLocal).Result;
        }

        // Axis
        Axis axisLocal;
        foreach (Axis item in serviceAxis.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          axisLocal = new Axis()
          {
            Template = item._id,
            Company = company.GetViewList(),
            Name = item.Name,
            Status = item.Status,
            TypeAxis = item.TypeAxis,
            _idAccount = _user._idAccount,
            _id = ObjectId.GenerateNewId().ToString(),
          };
          serviceAxis.InsertFreeNewVersion(axisLocal);
        }

        // Group
        Group groupLocal;
        foreach (Group item in serviceGroup.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          groupLocal = new Group()
          {
            Template = item._id,
            Company = company.GetViewList(),
            Name = item.Name,
            Status = item.Status,
            _id = item._id,
            _idAccount = _user._idAccount,
            Line = item.Line,
            Schooling = new List<ViewCrudSchooling>(),
            Skills = new List<ViewListSkill>(),
            Scope = new List<ViewListScope>()
          };

          if (item.Schooling != null)
            foreach (var schooling in item.Schooling)
              groupLocal.Schooling.Add(serviceSchooling.GetFreeNewVersion(p => p._idAccount == _user._idAccount && p.Template == schooling._id).Result.GetViewCrud());

          if (item.Skills != null)
            foreach (var skill in item.Skills)
              groupLocal.Skills.Add(serviceSkill.GetFreeNewVersion(p => p._idAccount == _user._idAccount && p.Template == skill._id).Result.GetViewList());

          if (item.Scope != null)
            foreach (var scope in item.Scope)
            {
              scope._id = ObjectId.GenerateNewId().ToString();
              groupLocal.Scope.Add(scope);
            };


          groupLocal.Sphere = serviceSphere.GetFreeNewVersion(p => p._idAccount == _user._idAccount && p.Template == item.Sphere._id).Result.GetViewList();
          groupLocal.Axis = serviceAxis.GetFreeNewVersion(p => p._idAccount == _user._idAccount && p.Template == item.Axis._id).Result.GetViewList();

          groupLocal._idAccount = _user._idAccount;
          groupLocal._id = ObjectId.GenerateNewId().ToString();
          Group result = serviceGroup.InsertFreeNewVersion(groupLocal).Result;
        }

        //Recommendation
        Recommendation recommendationLocal;
        foreach (Recommendation recommendation in serviceRecommendation.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          recommendationLocal = new Recommendation()
          {
            Template = recommendation._id,
            Content = recommendation.Content,
            Name = recommendation.Name,
            Image = recommendation.Image,
            Skill = serviceSkill.GetFreeNewVersion(p => p._idAccount == _user._idAccount & p.Template == recommendation.Skill._id).Result.GetViewList(),
            Status = recommendation.Status,
            _idAccount = _user._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          };
          serviceRecommendation.InsertFreeNewVersion(recommendationLocal).Result.GetViewList();
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Synchronize Parameters

    public string AddParameterAccounts(Parameter parameter)
    {
      try
      {
        // Identificação da conta raiz do ANALISA
        var idresolution = "5b6c4f47d9090156f08775aa";
        List<Account> accounts = serviceAccount.GetAllFreeNewVersion(p => p._id != idresolution).Result;
        Parameter local;
        foreach (Account account in accounts)
        {
          local = new Parameter()
          {
            Content = parameter.Content,
            Help = parameter.Help,
            Key = parameter.Key,
            Name = parameter.Name,
            Status = parameter.Status,
            _idAccount = account._id,
            _id = ObjectId.GenerateNewId().ToString()
          };
          Parameter result = serviceParameter.InsertFreeNewVersion(local).Result;
        }
        return "Paramter added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task SynchronizeParametersAsync()
    {
      try
      {
        // Identificação da conta raiz do ANALISA
        var idresolution = "5b6c4f47d9090156f08775aa";
        // TODO: ver multi empresas
        Company company = serviceCompany.GetAllNewVersion().FirstOrDefault();

        List<Account> accounts = serviceAccount.GetAllFreeNewVersion(p => p._id != idresolution).Result;

        // Parameter
        foreach (Parameter parameter in serviceParameter.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          Parameter local;
          foreach (Account accountParameter in accounts)
          {
            local = serviceParameter.GetFreeNewVersion(p => p._idAccount == accountParameter._id && p.Key == parameter.Key).Result;
            if (local == null)
            {
              local = new Parameter()
              {
                Content = parameter.Content,
                Help = parameter.Help,
                Key = parameter.Key,
                Name = parameter.Name,
                Status = parameter.Status,
                _idAccount = accountParameter._id,
                _id = ObjectId.GenerateNewId().ToString()
              };
              Parameter result = serviceParameter.InsertFreeNewVersion(local).Result;
            }
          }
        }
        // Text default
        foreach (TextDefault textDefault in serviceTextDefault.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          TextDefault local;
          foreach (Account accountTextDefault in accounts)
          {
            company = serviceCompany.GetFreeNewVersion(p => p._idAccount == accountTextDefault._id).Result;
            local = serviceTextDefault.GetFreeNewVersion(p => p._idAccount == accountTextDefault._id && p.Template == textDefault._id).Result;
            if (local == null)
            {
              local = new TextDefault()
              {
                Template = textDefault._id,
                Company = company.GetViewList(),
                _idAccount = accountTextDefault._id,
                Content = textDefault.Content,
                Name = textDefault.Name,
                Status = EnumStatus.Enabled,
                TypeText = textDefault.TypeText,
                _id = ObjectId.GenerateNewId().ToString()
              };
              TextDefault result = serviceTextDefault.InsertFreeNewVersion(local).Result;
            }
          }
        }
        // MailModel
        foreach (MailModel mailModel in serviceMailModel.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          MailModel local;
          foreach (Account accountMailModel in accounts)
          {
            local = serviceMailModel.GetFreeNewVersion(p => p._idAccount == accountMailModel._idAccount && p.Name == mailModel.Name).Result;
            if (local == null)
            {
              local = new MailModel()
              {
                Link = mailModel.Link,
                Message = mailModel.Message,
                Name = mailModel.Name,
                Status = mailModel.Status,
                StatusMail = mailModel.StatusMail,
                Subject = mailModel.Subject,
                _idAccount = accountMailModel._id,
                _id = ObjectId.GenerateNewId().ToString()
              };
              MailModel result = serviceMailModel.InsertFreeNewVersion(local).Result;
            }
          }
        }
        // Questions
        foreach (Questions question in serviceQuestions.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          Questions local;
          foreach (Account accountQuestion in accounts)
          {
            company = serviceCompany.GetFreeNewVersion(p => p._idAccount == accountQuestion._id).Result;
            local = serviceQuestions.GetFreeNewVersion(p => p._idAccount == accountQuestion._idAccount && p.Template == question._id).Result;
            if (local == null)
            {
              local = new Questions()
              {
                Template = question._id,
                Content = question.Content,
                Company = company.GetViewList(),
                Name = question.Name,
                Order = question.Order,
                Status = question.Status,
                TypeQuestion = question.TypeQuestion,
                TypeRotine = question.TypeRotine,
                _idAccount = accountQuestion._id,
                _id = ObjectId.GenerateNewId().ToString()
              };
              Questions result = serviceQuestions.InsertFreeNewVersion(local).Result;
            }
          }
        }
        // Recommendation
        foreach (Recommendation recommendation in serviceRecommendation.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          Recommendation local;
          foreach (Account accountRecommendation in accounts)
          {
            local = serviceRecommendation.GetFreeNewVersion(p => p._idAccount == accountRecommendation._idAccount && p.Template == recommendation._id).Result;
            if (local == null)
            {
              local = new Recommendation()
              {
                Template = recommendation._id,
                Content = recommendation.Content,
                Name = recommendation.Name,
                Image = recommendation.Image,
                Skill = serviceSkill.GetFreeNewVersion(p => p._idAccount == accountRecommendation._id & p.Template == recommendation.Skill._id).Result.GetViewList(),
                Status = recommendation.Status,
                _idAccount = accountRecommendation._id,
                _id = ObjectId.GenerateNewId().ToString()
              };
              Recommendation result = serviceRecommendation.InsertFreeNewVersion(local).Result;
            }
          }
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion


    #region Infra

    public List<ViewListInfraSphere> GetLineOpportunity(string idarea, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var spheres = new List<ViewListInfraSphere>();
        var listareas = new List<ViewListInfraAreaQuery>();
        var listspheres = new List<ViewListInfraSphereQuery>();
        var listgroups = new List<ViewListInfraGroupQuery>();
        var listlvlones = new List<ViewListInfraProcessLevelOneQuery>();
        var listlvltwos = new List<ViewListInfraProcessLevelTwoQuery>();
        var listoccupations = new List<ViewListInfraOccupationQuery>();

        var listoccupation = serviceOccupation.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result.Skip(skip).Take(count).ToList();
        total = serviceOccupation.CountNewVersion(p => p.Status == EnumStatus.Enabled).Result;

        foreach (var occupation in listoccupation)
        {
          foreach (var pc in occupation.Process.Where(p => p.ProcessLevelOne.Area._id == idarea))
          {
            var viewArea = new ViewListInfraAreaQuery
            {
              _id = pc.ProcessLevelOne.Area._id,
              Name = pc.ProcessLevelOne.Area.Name
            };

            var viewSphere = new ViewListInfraSphereQuery
            {
              _id = occupation.Group.Sphere._id,
              Name = occupation.Group.Sphere.Name,
              _idArea = viewArea._id,
              TypeSphere = occupation.Group.Sphere.TypeSphere
            };

            var viewGroup = new ViewListInfraGroupQuery
            {
              _id = occupation.Group._id,
              Name = occupation.Group.Name,
              _idSphere = viewSphere._id,
              Line = occupation.Group.Line
            };

            var viewProcessLevelOne = new ViewListInfraProcessLevelOneQuery
            {
              _id = pc.ProcessLevelOne._id,
              Name = pc.ProcessLevelOne.Name,
              _idGroup = viewGroup._id,
              Order = pc.ProcessLevelOne.Order
            };

            var viewProcessLevelTwo = new ViewListInfraProcessLevelTwoQuery
            {
              _id = pc._id,
              Name = pc.Name,
              _idProcessLevelOne = viewProcessLevelOne._id,
              Order = pc.Order
            };
            var viewOccupation = new ViewListInfraOccupationQuery
            {
              _id = occupation._id,
              Name = occupation.Name,
              _idProcessLevelTwo = viewProcessLevelTwo._id,
              _idGroup = viewGroup._id
            };

            listareas.Add(viewArea);
            listspheres.Add(viewSphere);
            listgroups.Add(viewGroup);
            listlvlones.Add(viewProcessLevelOne);
            listlvltwos.Add(viewProcessLevelTwo);
            listoccupations.Add(viewOccupation);
          }
        }

        foreach (var sphere in listspheres.Where(p => p._idArea == idarea).OrderBy(p => p.TypeSphere))
        {
          var viewSphere = new ViewListInfraSphere()
          {
            _id = sphere._id,
            Name = sphere.Name,
            Groups = new List<ViewListInfraGroup>()
          };
          foreach (var group in listgroups.Where(p => p._idSphere == sphere._id).OrderBy(p => p.Line))
          {
            var viewGroup = new ViewListInfraGroup()
            {
              _id = group._id,
              Name = group.Name,
              ProcessLevelOnes = new List<ViewListInfraProcessLevelOne>()
            };

            foreach (var lvlone in listlvlones.Where(p => p._idGroup == group._id).OrderBy(p => p.Order))
            {
              var viewLvlOne = new ViewListInfraProcessLevelOne()
              {
                _id = lvlone._id,
                Name = lvlone.Name,
                ProcessLevelTwos = new List<ViewListInfraProcessLevelTwo>()
              };

              foreach (var lvltwo in listlvltwos.Where(p => p._idProcessLevelOne == lvlone._id).OrderBy(p => p.Order))
              {
                var viewLvlTwo = new ViewListInfraProcessLevelTwo()
                {
                  _id = lvltwo._id,
                  Name = lvltwo.Name,
                  Occupations = new List<ViewListInfraOccupation>()
                };

                foreach (var occupation in listoccupations.Where(p => p._idProcessLevelTwo == lvltwo._id
                && p._idGroup == viewGroup._id))
                {
                  var viewOccupation = new ViewListInfraOccupation()
                  {
                    _id = occupation._id,
                    Name = occupation.Name
                  };

                  viewLvlTwo.Occupations.Add(viewOccupation);
                }
                if (viewLvlOne.ProcessLevelTwos.Where(p => p._id == viewLvlTwo._id).Count() == 0)
                  viewLvlOne.ProcessLevelTwos.Add(viewLvlTwo);
              }
              if (viewGroup.ProcessLevelOnes.Where(p => p._id == viewLvlOne._id).Count() == 0)
                viewGroup.ProcessLevelOnes.Add(viewLvlOne);
            }

            if (viewSphere.Groups.Where(p => p.Name == viewGroup.Name).Count() == 0)
              viewSphere.Groups.Add(viewGroup);
          }
          if (spheres.Where(p => p._id == viewSphere._id).Count() == 0)
            spheres.Add(viewSphere);
        }


        return spheres.ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListSkill> GetEssential(string idcompany)
    {
      try
      {
        return serviceCompany.GetNewVersion(p => p._id == idcompany).Result.Skills;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteProcessLevelOne(string id)
    {
      try
      {
        if (serviceProcessLevelTwo.CountNewVersion(p => p.ProcessLevelOne._id == id).Result > 0)
          return "error_leveltwo_exists";

        var item = serviceProcessLevelOne.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        serviceProcessLevelOne.Update(item, null);
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteProcessLevelTwo(string id)
    {
      try
      {
        //if (occupationService.GetAllNewVersion(p => p.ProcessLevelTwo._id == id).Count() > 0)
        //return "error_occupation_exists";
        var item = serviceProcessLevelTwo.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();

        if (serviceOccupation.CountNewVersion(p => p.Process.Contains(item.GetViewList())).Result > 0)
          return "error_occupation_exists";



        item.Status = EnumStatus.Disabled;
        serviceProcessLevelTwo.Update(item, null);
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ReorderGroupScope(string idcompany, string idgroup, string idscope, bool sum)
    {
      try
      {
        var group = serviceGroup.GetAllNewVersion(p => p.Company._id == idcompany & p._id == idgroup).Result.FirstOrDefault();
        var scope = group.Scope.Where(p => p._id == idscope).FirstOrDefault();
        ViewListScope scopeOld;
        if (sum)
        {
          var min = group.Scope.Where(p => p.Order > scope.Order).Min(p => p.Order);
          scopeOld = group.Scope.Where(p => p.Order == min).FirstOrDefault();
        }
        else
        {
          var max = group.Scope.Where(p => p.Order < scope.Order).Max(p => p.Order);
          scopeOld = group.Scope.Where(p => p.Order == max).FirstOrDefault();
        }


        long orderold = scopeOld.Order;
        long ordernew = scope.Order;

        foreach (var item in group.Scope)
        {
          if (item._id == scope._id)
            item.Order = orderold;

          if (item._id == scopeOld._id)
            item.Order = ordernew;

        }

        serviceGroup.Update(group, null);
        Task.Run(() => UpdateGroupAll(group));
        Task.Run(() => UpdateGroupLog(group));
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ReorderOccupationActivitie(string idcompany, string idoccupation, string idactivitie, bool sum)
    {
      try
      {
        long max = 1;
        long min = 1;

        var occupation = serviceOccupation.GetNewVersion(p => p.Group.Company._id == idcompany & p._id == idoccupation).Result;
        var activities = occupation.Activities.Where(p => p._id == idactivitie).FirstOrDefault();
        ViewListActivitie activitiesOld;
        if (sum)
        {
          try
          {

            min = occupation.Activities.Where(p => p.Order > activities.Order).Min(p => p.Order);
          }
          catch
          {

          }
          activitiesOld = occupation.Activities.Where(p => p.Order == min).FirstOrDefault();
        }
        else
        {
          try
          {
            max = occupation.Activities.Where(p => p.Order < activities.Order).Max(p => p.Order);
          }
          catch
          {

          }

          activitiesOld = occupation.Activities.Where(p => p.Order == max).FirstOrDefault();
        }

        long orderold = activitiesOld.Order;
        long ordernew = activities.Order;

        foreach (var item in occupation.Activities)
        {
          if (item._id == activities._id)
            item.Order = orderold;

          if (item._id == activitiesOld._id)
            item.Order = ordernew;

        }

        serviceOccupation.Update(occupation, null);

        Task.Run(() => UpdateOccupationAll(occupation));
        Task.Run(() => UpdateOccupationLog(occupation));
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ReorderGroupScopeManual(string idcompany, string idgroup, string idscope, long order)
    {
      try
      {
        var group = serviceGroup.GetAllNewVersion(p => p.Company._id == idcompany & p._id == idgroup).Result.FirstOrDefault();
        var scope = group.Scope.Where(p => p._id == idscope).FirstOrDefault();

        foreach (var item in group.Scope)
        {
          if (item._id == scope._id)
            item.Order = order;
        }

        serviceGroup.Update(group, null);
        Task.Run(() => UpdateGroupAll(group));
        Task.Run(() => UpdateGroupLog(group));
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ReorderOccupationActivitieManual(string idcompany, string idoccupation, string idactivitie, long order)
    {
      try
      {
        var occupation = serviceOccupation.GetAllNewVersion(p => p.Group.Company._id == idcompany & p._id == idoccupation).Result.FirstOrDefault();
        var activities = occupation.Activities.Where(p => p._id == idactivitie).FirstOrDefault();

        foreach (var item in occupation.Activities)
        {
          if (item._id == activities._id)
            item.Order = order;
        }
        serviceOccupation.Update(occupation, null);

        Task.Run(() => UpdateOccupationAll(occupation));
        Task.Run(() => UpdateOccupationLog(occupation));
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteTextDefault(string id)
    {
      try
      {
        var textDefault = serviceTextDefault.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
        textDefault.Status = EnumStatus.Disabled;
        serviceTextDefault.Update(textDefault, null);
        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string AreaOrder(string idcompany, string idarea, long order, bool sum)
    {
      try
      {
        var area = serviceArea.GetAllNewVersion(p => p._id == idarea).Result.FirstOrDefault();
        var areas = serviceArea.GetAllNewVersion(p => p.Company._id == idcompany).Result.ToList();

        return "reorder";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string GetCSVCompareGroup(string idcompany, string link)
    {
      try
      {
        var groups = serviceGroup.GetAllNewVersion(p => p.Company._id == idcompany).Result.OrderBy(p => p.Sphere.TypeSphere).ThenBy(p => p.Axis.TypeAxis).ThenBy(p => p.Line).ToList();

        var head = string.Empty;
        var sphere = string.Empty;
        var axis = string.Empty;

        foreach (var item in groups)
        {
          head += item.Name + ";";
          sphere += item.Sphere.Name + ";";
          axis += item.Axis.Name + ";";
        }


        string[] rel = new string[3];
        rel[0] = head;
        rel[1] = sphere;
        rel[2] = axis;

        List<ViewCSVLO> list = new List<ViewCSVLO>();

        long line = 0;
        long col = 0;
        long maxLine = 0;
        long maxLineSkill = 0;
        long maxLineSchooling = 0;

        foreach (var item in groups)
        {
          line = 0;
          foreach (var scope in item.Scope)
          {
            if (line > maxLine)
              maxLine = line;

            var result = new ViewCSVLO
            {
              Name = scope.Name.Replace("\n", "").Replace(";", "."),
              Line = line,
              Col = col,
              Type = EnumTypeLineOpportunity.Scope,
              IdGroup = item._id
            };

            list.Add(result);
            line += 1;
          }

          if (line > maxLine)
            maxLine = line;

          line = 0;
          foreach (var skill in item.Skills)
          {
            if (line > maxLineSkill)
              maxLineSkill = line;

            var result = new ViewCSVLO
            {
              Name = skill.Name.Replace("\n", "").Replace(";", ".") + ":" + skill.Concept?.Replace("\n", "").Replace(";", "."),
              Line = line,
              Col = col,
              Type = EnumTypeLineOpportunity.Skill,
              IdGroup = item._id
            };

            list.Add(result);
            line += 1;
          }

          if (line > maxLineSkill)
            maxLineSkill = line;

          line = 0;
          foreach (var scholling in item.Schooling)
          {
            if (line > maxLineSchooling)
              maxLineSchooling = line;

            var result = new ViewCSVLO
            {
              Name = scholling.Name.Replace("\n", "").Replace(";", "."),
              Line = line,
              Col = col,
              Type = EnumTypeLineOpportunity.Schooling,
              IdGroup = item._id
            };

            list.Add(result);
            line += 1;
          }
          col += 1;
        }
        if (line > maxLineSchooling)
          maxLineSchooling = line;


        for (var row = 0; row < maxLine; row++)
        {
          col = 0;
          foreach (var group in groups)
          {
            var item = list.Where(p => p.Type == EnumTypeLineOpportunity.Scope & p.IdGroup == group._id & p.Line == row).OrderBy(p => p.Col).Count();
            if (item == 0)
            {
              var view = new ViewCSVLO
              {
                IdGroup = group._id,
                Type = EnumTypeLineOpportunity.Scope,
                Name = " ",
                Line = row,
                Col = col
              };
              list.Add(view);
            }
            col += 1;
          }
        }

        for (var row = 0; row < maxLineSkill; row++)
        {
          col = 0;
          foreach (var group in groups)
          {
            var item = list.Where(p => p.Type == EnumTypeLineOpportunity.Skill & p.IdGroup == group._id & p.Line == row).OrderBy(p => p.Col).Count();
            if (item == 0)
            {
              var view = new ViewCSVLO
              {
                IdGroup = group._id,
                Type = EnumTypeLineOpportunity.Skill,
                Name = " ",
                Line = row,
                Col = col
              };
              list.Add(view);
            }
            col += 1;
          }
        }

        for (var row = 0; row <= maxLineSchooling; row++)
        {
          col = 0;
          foreach (var group in groups)
          {
            var item = list.Where(p => p.Type == EnumTypeLineOpportunity.Schooling & p.IdGroup == group._id & p.Line == row).OrderBy(p => p.Col).Count();
            if (item == 0)
            {
              var view = new ViewCSVLO
              {
                IdGroup = group._id,
                Type = EnumTypeLineOpportunity.Schooling,
                Name = " ",
                Line = row,
                Col = col
              };
              list.Add(view);
            }
            col += 1;
          }
        }


        for (var row = 0; row < maxLine; row++)
        {
          var itemView = string.Empty;
          foreach (var item in list.Where(p => p.Type == EnumTypeLineOpportunity.Scope & p.Line == row).OrderBy(p => p.Col).ToList())
          {
            try
            {
              itemView += item.Name + ";";
            }
            catch (Exception)
            {
              itemView += " ;";
            }
          }

          rel = Export(rel, itemView);
        }

        for (var row = 0; row < maxLineSkill; row++)
        {
          var itemView = string.Empty;
          foreach (var item in list.Where(p => p.Type == EnumTypeLineOpportunity.Skill & p.Line == row).OrderBy(p => p.Col).ToList())
          {
            try
            {
              itemView += item.Name + ";";
            }
            catch (Exception)
            {
              itemView += " ;";
            }
          }

          rel = Export(rel, itemView);
        }

        for (var row = 0; row <= maxLineSchooling; row++)
        {
          var itemView = string.Empty;
          foreach (var item in list.Where(p => p.Type == EnumTypeLineOpportunity.Schooling & p.Line == row).OrderBy(p => p.Col).ToList())
          {
            try
            {
              itemView += item.Name + ";";
            }
            catch (Exception)
            {
              itemView += " ;";
            }
          }
          rel = Export(rel, itemView);
        }



        var guid = Guid.NewGuid().ToString();

        var filename = "reports/LO" + DateTime.Now.ToString("yyyyMMddHHmmss") + guid + ".csv";


        File.WriteAllLines(filename, rel, Encoding.GetEncoding("iso-8859-1"));
        var stream = new StreamReader(File.OpenText(filename).BaseStream, Encoding.GetEncoding("iso-8859-1"));


        var person = servicePerson.GetAllNewVersion(p => p.User.Mail == _user.Mail).Result.FirstOrDefault();

        CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(link);
        CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("reports");
        if (cloudBlobContainer.CreateIfNotExistsAsync().Result)
        {
          cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
          {
            PublicAccess = BlobContainerPublicAccessType.Blob
          });
        }
        CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(string.Format("{0}{1}", _user._idUser.ToString(), ".csv"));
        blockBlob.Properties.ContentType = "text/csv";
        blockBlob.UploadFromStreamAsync(stream.BaseStream).Wait();
        return blockBlob.Uri.ToString();

      }
      catch (Exception e)
      {
        throw e;
      }

    }

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


    public List<ViewListCbo> ListCbo()
    {
      try
      {
        var Cbo = serviceCbo.GetAllFreeNewVersion(p => p.Status == EnumStatus.Enabled).Result
          .Select(p => new ViewListCbo()
          {
            _id = p._id,
            Code = p.Code,
            Name = p.Name
          })
          .ToList();
        return Cbo;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudCbo GetCbo(string id)
    {
      try
      {
        return serviceCbo.GetFreeNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListSphere> GetSpheres()
    {
      try
      {
        return serviceSphere.GetAllNewVersion().OrderBy(p => p.TypeSphere)
          .Select(p => p.GetViewList()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListSphere> GetSpheres(string idcompany)
    {
      try
      {
        return serviceSphere.GetAllNewVersion(p => p.Company._id == idcompany).Result.OrderBy(p => p.TypeSphere)
           .Select(p => p.GetViewList()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListArea> GetAreas()
    {
      try
      {
        var areas = serviceArea.GetAllNewVersion().OrderBy(p => p.Name).ToList();
        foreach (var item in areas)
        {
          item.ProcessLevelOnes = new List<ViewListProcessLevelOne>();
          var process = serviceProcessLevelOne.GetAllNewVersion(p => p.Area._id == item._id).Result.OrderBy(p => p.Order).ToList();
          foreach (var row in process)
          {
            row.Process = new List<ViewListProcessLevelTwo>();
            foreach (var leveltwo in serviceProcessLevelTwo.GetAllNewVersion(p => p.ProcessLevelOne._id == row._id).Result.OrderBy(p => p.Order).ToList())
            {
              row.Process.Add(leveltwo.GetViewList());
            }

            item.ProcessLevelOnes.Add(row.GetViewList());
          }
        }
        return areas.OrderBy(p => p.Name)
           .Select(p => p.GetViewList()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudSchooling GetSchoolingById(string id)
    {
      try
      {
        var item = serviceSchooling.GetAllNewVersion(p => p._id == id).Result.OrderBy(p => p.Name).FirstOrDefault();
        return item.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudAxis GetAxisById(string id)
    {
      try
      {
        return serviceAxis.GetAllNewVersion(p => p._id == id).Result.OrderBy(p => p.Name).FirstOrDefault().GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudSphere GetSphereById(string id)
    {
      try
      {
        var item = serviceSphere.GetAllNewVersion(p => p._id == id).Result.OrderBy(p => p.Name).FirstOrDefault();
        return new ViewCrudSphere()
        {
          _id = item._id,
          Name = item.Name,
          Company = new ViewListCompany() { _id = item._id, Name = item.Name },
          TypeSphere = item.TypeSphere
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudSkill GetSkillById(string id)
    {
      try
      {
        return serviceSkill.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListArea> GetAreas(string idcompany)
    {
      try
      {
        var areas = serviceArea.GetAllNewVersion(p => p.Company._id == idcompany).Result.OrderBy(p => p.Name).ToList();
        foreach (var item in areas)
        {
          item.ProcessLevelOnes = new List<ViewListProcessLevelOne>();
          var process = serviceProcessLevelOne.GetAllNewVersion(p => p.Area._id == item._id).Result.OrderBy(p => p.Order).ToList();
          foreach (var row in process)
          {
            row.Process = new List<ViewListProcessLevelTwo>();
            foreach (var leveltwo in serviceProcessLevelTwo.GetAllNewVersion(p => p.ProcessLevelOne._id == row._id).Result.OrderBy(p => p.Order).ToList())
            {
              row.Process.Add(leveltwo.GetViewList());
            }

            item.ProcessLevelOnes.Add(row.GetViewList());
          }
        }
        return areas.OrderBy(p => p.Name)
           .Select(p => p.GetViewList()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListAxis> GetAxis()
    {
      try
      {
        return serviceAxis.GetAllNewVersion().OrderBy(p => p.TypeAxis)
          .Select(p => p.GetViewList()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListAxis> GetAxis(string idcompany)
    {
      try
      {
        return serviceAxis.GetAllNewVersion(p => p.Company._id == idcompany).Result.OrderBy(p => p.TypeAxis)
          .Select(p => p.GetViewList()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListQuestions> ListQuestions(ref long total, string idcompany)
    {
      try
      {
        total = serviceQuestions.CountNewVersion(p => p.Company._id == idcompany).Result;

        return serviceQuestions.GetAllNewVersion(p => p.Company._id == idcompany).Result.OrderBy(p => p.Order)
          .Select(p => new ViewListQuestions()
          {
            _id = p._id,
            Name = p.Name,
            Content = p.Content,
            TypeRotine = p.TypeRotine,
            Order = p.Order
          }).OrderBy(p => p.TypeRotine).ThenBy(p => p.Order).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudQuestions GetQuestions(string id)
    {
      try
      {
        return serviceQuestions.GetAllNewVersion(p => p._id == id).Result.Select(p => new ViewCrudQuestions()
        {
          _id = p._id,
          Name = p.Name,
          Content = p.Content,
          Order = p.Order,
          TypeQuestion = p.TypeQuestion,
          TypeRotine = p.TypeRotine
        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListSchooling> GetSchooling()
    {
      try
      {
        return serviceSchooling.GetAllNewVersion().OrderBy(p => p.Order)
          .Select(p => new ViewListSchooling()
          {
            _id = p._id,
            Name = p.Name,
            Order = p.Order
          }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudGroup AddGroup(ViewAddGroup view)
    {
      try
      {
        long line = 0;
        try
        {
          line = serviceGroup.GetAllNewVersion(p => p.Sphere._id == view.Sphere._id & p.Axis._id == view.Axis._id).Result.Max(p => p.Line);
          if (line == 0)
            line = 1;
          else
            line += 1;
        }
        catch (Exception)
        {
          line = 1;
        }


        var group = new Group()
        {
          Name = view.Name,
          Axis = view.Axis,
          Sphere = view.Sphere,
          Status = EnumStatus.Enabled,
          Skills = new List<ViewListSkill>(),
          Schooling = new List<ViewCrudSchooling>(),
          Line = line,
          Company = view.Company,
          Scope = new List<ViewListScope>()
        };
        return serviceGroup.InsertNewVersion(group).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddEssential(ViewCrudEssential view)
    {
      try
      {
        var company = serviceCompany.GetAllNewVersion(p => p._id == view._idCompany).Result.FirstOrDefault();

        Skill skill = serviceSkill.GetAllNewVersion(p => p._id == view.Skill._id).Result.FirstOrDefault();

        if (view.Skill._id == null)
        {
          skill = AddSkillInternal(new ViewCrudSkill()
          {
            Name = view.Skill.Name,
            Concept = view.Skill.Concept,
            TypeSkill = view.Skill.TypeSkill
          });
        }

        company.Skills.Add(skill.GetViewList());
        serviceCompany.Update(company, null);

        Task.Run(() => UpdateCompanyAll(company));
        Task.Run(() => UpdateCompanyLog(company));
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudGroup AddGroup(ViewCrudGroup view)
    {
      try
      {
        long line = 0;
        try
        {
          line = serviceGroup.GetAllNewVersion(p => p.Sphere._id == view.Sphere._id & p.Axis._id == view.Axis._id).Result.Max(p => p.Line);
          if (line == 0)
            line = 1;
          else
            line += 1;
        }
        catch (Exception)
        {
          line = 1;
        }


        var group = new Group()
        {
          Name = view.Name,
          Axis = serviceAxis.GetNewVersion(p => p._id == view.Axis._id).Result.GetViewList(),
          Sphere = serviceSphere.GetNewVersion(p => p._id == view.Sphere._id).Result.GetViewList(),
          Status = EnumStatus.Enabled,
          Skills = new List<ViewListSkill>(),
          Schooling = new List<ViewCrudSchooling>(),
          Line = line,
          Company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result.GetViewList(),
          Scope = new List<ViewListScope>()
        };
        var result = serviceGroup.InsertNewVersion(group).Result.GetViewCrud();

        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");

        // Save group version
        var groupLog = new GroupLog()
        {
          Name = group.Name,
          Company = group.Company,
          Line = group.Line,
          Skills = group.Skills,
          Schooling = group.Schooling,
          Scope = group.Scope,
          Template = group.Template,
          Status = group.Status,
          _idAccount = group._idAccount,
          Axis = group.Axis,
          Sphere = group.Sphere
        };
        groupLog._idGroupPrevious = result._id;
        groupLog.Date = datenow;
        groupLog.DateLog = DateTime.Now;

        serviceGroupLog.InsertNewVersion(groupLog);

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddMapGroupSchooling(ViewCrudMapGroupSchooling view)
    {
      try
      {

        var group = serviceGroup.GetAllNewVersion(p => p._id == view._idGroup).Result.FirstOrDefault();
        var schooling = serviceSchooling.GetAllNewVersion(p => p._id == view.Schooling._id).Result.FirstOrDefault();

        schooling.Type = view.Schooling.Type;

        //if (view.Group.Schooling.Where(p => p.Type == view.Schooling.Type).Count() > 0)
        //  return "error_exists_schooling";

        //view.Group.Schooling.Add(AddSchooling(view.Schooling));
        group.Schooling.Add(schooling.GetViewCrud());
        serviceGroup.Update(group, null);
        Task.Run(() => UpdateGroupAll(group));
        Task.Run(() => UpdateGroupLog(group));
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddMapGroupScope(ViewCrudMapGroupScope view)
    {
      try
      {
        var group = serviceGroup.GetAllNewVersion(p => p._id == view._idGroup).Result.FirstOrDefault();

        long order = 1;
        try
        {
          order = group.Scope.Max(p => p.Order) + 1;
          if (order == 0)
          {
            order = 1;
          }
        }
        catch (Exception)
        {
          order = 1;
        }

        var scope = new Scope()
        {
          _id = view.Scope._id,
          Order = order,
          Name = view.Scope.Name
        };

        if (scope._id == null)
          scope._id = ObjectId.GenerateNewId().ToString();

        group.Scope.Add(scope.GetViewList());
        serviceGroup.Update(group, null);
        Task.Run(() => UpdateGroupAll(group));
        Task.Run(() => UpdateGroupLog(group));
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddMapGroupSkill(ViewCrudMapGroupSkill view)
    {
      try
      {
        var group = serviceGroup.GetAllNewVersion(p => p._id == view._idGroup).Result.FirstOrDefault();
        Skill skill = serviceSkill.GetAllNewVersion(p => p._id == view.Skill._id).Result.FirstOrDefault();

        if (view.Skill._id == null)
        {
          skill = AddSkillInternal(new ViewCrudSkill()
          {
            Name = view.Skill.Name,
            Concept = view.Skill.Concept,
            TypeSkill = view.Skill.TypeSkill
          });
        }

        group.Skills.Add(skill.GetViewList());
        serviceGroup.Update(group, null);
        Task.Run(() => UpdateGroupAll(group));
        Task.Run(() => UpdateGroupLog(group));
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddOccupationActivities(ViewCrudOccupationActivities view)
    {
      try
      {
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == view.idOccupation).Result.FirstOrDefault();
        long order = 1;
        try
        {
          order = occupation.Activities.Max(p => p.Order) + 1;
          if (order == 0)
          {
            order = 1;
          }
        }
        catch (Exception)
        {
          order = 1;
        }

        var activitie = new Activitie()
        {
          Order = order,
          _id = ObjectId.GenerateNewId().ToString(),
          Name = view.Activities.Name
        };


        occupation.Activities.Add(activitie.GetViewList());
        serviceOccupation.Update(occupation, null);
        Task.Run(() => UpdateOccupationAll(occupation));
        Task.Run(() => UpdateOccupationLog(occupation));
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddOccupationActivitiesList(List<ViewCrudOccupationActivities> list)
    {
      try
      {

        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == list.FirstOrDefault().idOccupation).Result.FirstOrDefault();
        long order = 1;
        try
        {
          order = occupation.Activities.Max(p => p.Order) + 1;
          if (order == 0)
          {
            order = 1;
          }
        }
        catch (Exception)
        {
          order = 1;
        }


        foreach (ViewCrudOccupationActivities view in list)
        {
          if (!string.IsNullOrEmpty(view.Activities.Name.Trim()))
          {
            var activitie = new Activitie()
            {
              Order = order,
              _id = ObjectId.GenerateNewId().ToString(),
              Name = view.Activities.Name
            };

            occupation.Activities.Add(activitie.GetViewList());
            order += 1;
          }
        }

        serviceOccupation.Update(occupation, null);
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddSpecificRequirements(string idoccupation, ViewCrudSpecificRequirements view)
    {
      try
      {
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == idoccupation).Result.FirstOrDefault();
        occupation.SpecificRequirements = view.Name;
        serviceOccupation.Update(occupation, null);
        Task.Run(() => UpdateOccupationAll(occupation));
        Task.Run(() => UpdateOccupationLog(occupation));

        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddOccupationSkill(ViewCrudOccupationSkill view)
    {
      try
      {
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == view._idOccupation).Result.FirstOrDefault();
        var skill = serviceSkill.GetAllNewVersion(p => p._id == view.Skill._id).Result.FirstOrDefault();

        if (skill == null)
        {
          skill = AddSkillInternal(new ViewCrudSkill()
          {
            Name = view.Skill.Name,
            Concept = view.Skill.Concept,
            TypeSkill = view.Skill.TypeSkill
          });
        }

        occupation.Skills.Add(skill.GetViewList());
        serviceOccupation.Update(occupation, null);
        Task.Run(() => UpdateOccupationAll(occupation));
        Task.Run(() => UpdateOccupationLog(occupation));
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudSkill AddSkill(ViewCrudSkill view)
    {
      try
      {
        var skill = new Skill()
        {
          Name = view.Name.Trim(),
          Concept = view.Concept,
          TypeSkill = view.TypeSkill,
          Status = EnumStatus.Enabled
        };
        serviceSkill.InsertNewVersion(skill);
        return new ViewCrudSkill()
        {
          _id = skill._id,
          Name = skill.Name,
          Concept = skill.Concept,
          TypeSkill = skill.TypeSkill
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public string AddArea(ViewCrudArea view)
    {
      try
      {
        //var item = areaService.GetAllNewVersion(p => p.Order == view.Order).Count();
        //if (item > 0)
        //  return "error_line";


        serviceArea.InsertNewVersion(new Area()
        {
          Name = view.Name,
          Order = view.Order,
          Status = EnumStatus.Enabled,
          Company = view.Company,
          ProcessLevelOnes = new List<ViewListProcessLevelOne>()
        });
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddCbo(ViewCrudCbo view)
    {
      try
      {
        serviceCbo.InsertFreeNewVersion(new Cbo()
        {
          Name = view.Name,
          Code = view.Code,
          Status = EnumStatus.Enabled
        });
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddAxis(ViewCrudAxis view)
    {
      try
      {
        serviceAxis.InsertNewVersion(new Axis()
        {
          Name = view.Name,
          TypeAxis = view.TypeAxis,
          Status = EnumStatus.Enabled,
          Company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result.GetViewList()
        });
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddOccupation(ViewCrudOccupation view)
    {
      try
      {
        var group = serviceGroup.GetAllNewVersion(p => p._id == view.Group._id).Result.FirstOrDefault();

        var occupation = new Occupation()
        {
          Name = view.Name,
          Line = view.Line,
          Status = EnumStatus.Enabled,
          Group = group.GetViewList(),
          Description = view.Description,
          Skills = new List<ViewListSkill>(),
          Cbo = view.Cbo,
          Activities = new List<ViewListActivitie>(),
          SalaryScales = new List<SalaryScaleGrade>(),
          Schooling = new List<ViewCrudSchoolingOccupation>(),
          Process = new List<ViewListProcessLevelTwo>()
        };

        foreach (var item in view.Process)
          occupation.Process.Add(serviceProcessLevelTwo.GetNewVersion(p => p._id == item._id).Result.GetViewList());

        if (view.SalaryScales != null)
          foreach (var item in view.SalaryScales)
            occupation.SalaryScales.Add(new SalaryScaleGrade()
            {
              _id = ObjectId.GenerateNewId().ToString(),
              NameGrade = item.NameGrade,
              NameSalaryScale = item.Name,
              _idGrade = item._idGrade,
              _idSalaryScale = item._id
            });

        if (group.Schooling != null)
          occupation.Schooling = group.Schooling.Select(p => p.GetViewCrud()).ToList();

        serviceOccupation.InsertNewVersion(occupation);

        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");

        // Save occupation version
        var occupationLog = new OccupationLog()
        {
          Name = occupation.Name,
          Group = occupation.Group,
          Line = occupation.Line,
          Skills = occupation.Skills,
          Schooling = occupation.Schooling,
          Activities = occupation.Activities,
          Template = occupation.Template,
          Cbo = occupation.Cbo,
          SpecificRequirements = occupation.SpecificRequirements,
          Process = occupation.Process,
          SalaryScales = occupation.SalaryScales,
          Description = occupation.Description,
          Status = occupation.Status,
          _idAccount = occupation._idAccount
        };
        occupationLog._idOccupationPrevious = occupation._id;
        occupationLog.Date = datenow;
        occupationLog.DateLog = DateTime.Now;

        serviceOccupationLog.InsertNewVersion(occupationLog);
        return "ok";
      }
      catch (Exception)
      {
        throw;
      }
    }

    public Schooling AddSchooling(ViewCrudSchooling schooling)
    {
      try
      {

        return serviceSchooling.InsertNewVersion(new Schooling()
        {
          Name = schooling.Name,
          Order = schooling.Order,
          Type = schooling.Type,
          Status = EnumStatus.Enabled
        }).Result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddSphere(ViewCrudSphere view)
    {
      try
      {
        serviceSphere.InsertNewVersion(new Sphere()
        {
          Name = view.Name,
          TypeSphere = view.TypeSphere,
          Company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result.GetViewList(),
          Status = EnumStatus.Enabled
        });
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddQuestions(ViewCrudQuestions view, string idcompany)
    {
      try
      {
        var company = serviceCompany.GetNewVersion(p => p._id == idcompany).Result.GetViewList();

        serviceQuestions.InsertNewVersion(new Questions()
        {
          Name = view.Name,
          Order = view.Order,
          TypeQuestion = view.TypeQuestion,
          TypeRotine = view.TypeRotine,
          Company = company,
          Status = EnumStatus.Enabled,
          Content = view.Content
        });
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddProcessLevelOne(ViewCrudProcessLevelOne model)
    {
      try
      {
        try
        {
          var order = serviceProcessLevelOne.GetAllNewVersion(p => p.Area._id == model.Area._id).Result.Max(p => p.Order) + 1;
          model.Order = order;
        }
        catch (Exception)
        {
          model.Order = 1;
        }


        serviceProcessLevelOne.InsertNewVersion(new ProcessLevelOne()
        {
          Name = model.Name,
          Area = serviceArea.GetNewVersion(p => p._id == model.Area._id).Result.GetViewList(),
          Process = new List<ViewListProcessLevelTwo>(),
          Order = model.Order,
          Status = EnumStatus.Enabled
        });
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddTextDefault(ViewCrudTextDefault model)
    {
      try
      {
        serviceTextDefault.InsertNewVersion(new TextDefault()
        {
          Name = model.Name,
          Company = serviceCompany.GetNewVersion(p => p._id == model.Company._id).Result.GetViewList(),
          Content = model.Content,
          TypeText = model.TypeText,
          Status = EnumStatus.Enabled
        });
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddProcessLevelTwo(ViewCrudProcessLevelTwo model)
    {
      try
      {
        try
        {
          var order = serviceProcessLevelTwo.GetAllNewVersion(p => p.ProcessLevelOne._id == model.ProcessLevelOne._id).Result.Max(p => p.Order) + 1;
          model.Order = order;
        }
        catch (Exception)
        {
          model.Order = 1;
        }

        serviceProcessLevelTwo.InsertNewVersion(new ProcessLevelTwo()
        {
          Name = model.Name,
          Comments = model.Comments,
          Order = model.Order,
          ProcessLevelOne = serviceProcessLevelOne.GetNewVersion(p => p._id == model.ProcessLevelOne._id).Result.GetViewList(),
          Status = EnumStatus.Enabled
        });
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteArea(string idarea)
    {
      try
      {
        if (serviceProcessLevelOne.CountNewVersion(p => p.Area._id == idarea).Result > 0)
          return "erro_exists_nivelone";

        var area = serviceArea.GetAllNewVersion(p => p._id == idarea).Result.FirstOrDefault();


        foreach (var item in serviceOccupation.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result.Where(p => p.Process.Exists(i => i.ProcessLevelOne.Area._id == area._id)).ToList())
        {
          return "error_exists_register";
        }

        area.Status = EnumStatus.Disabled;
        serviceArea.Update(area, null);

        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteAxis(string idaxis)
    {
      try
      {
        var axis = serviceAxis.GetAllNewVersion(p => p._id == idaxis).Result.FirstOrDefault();


        foreach (var item in serviceGroup.GetAllNewVersion(p => p.Axis._id == axis._id).Result.ToList())
        {
          return "error_exists_register";
        }
        axis.Status = EnumStatus.Disabled;
        serviceAxis.Update(axis, null);

        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteEssential(string idcompany, string id)
    {
      try
      {
        var company = serviceCompany.GetAllNewVersion(p => p._id == idcompany).Result.FirstOrDefault();
        foreach (var item in company.Skills)
        {
          if (item._id == id)
          {
            company.Skills.Remove(item);
            this.serviceCompany.Update(company, null);
            Task.Run(() => UpdateCompanyAll(company));
            Task.Run(() => UpdateCompanyLog(company));
            return "delete";
          }
        }

        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteGroup(string idgroup)
    {
      try
      {
        var group = serviceGroup.GetAllNewVersion(p => p._id == idgroup).Result.FirstOrDefault();

        foreach (var item in servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator & p.Occupation._idGroup == group._id).Result.ToList())
        {
          return "error_exists_register";
        }


        foreach (var item in serviceOccupation.GetAllNewVersion(p => p.Group._id == group._id).Result.ToList())
        {
          return "error_exists_register";
        }

        group.Status = EnumStatus.Disabled;
        serviceGroup.Update(group, null);
        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteMapGroupSchooling(string idgroup, string id)
    {
      try
      {
        var group = serviceGroup.GetAllNewVersion(p => p._id == idgroup).Result.FirstOrDefault();
        var schooling = group.Schooling.Where(p => p._id == id).FirstOrDefault();
        group.Schooling.Remove(schooling);
        serviceGroup.Update(group, null);
        Task.Run(() => UpdateGroupAll(group));
        Task.Run(() => UpdateGroupLog(group));

        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteMapGroupSkill(string idgroup, string id)
    {
      try
      {
        var group = serviceGroup.GetAllNewVersion(p => p._id == idgroup).Result.FirstOrDefault();
        var skill = group.Skills.Where(p => p._id == id).FirstOrDefault();
        group.Skills.Remove(skill);
        serviceGroup.Update(group, null);
        Task.Run(() => UpdateGroupAll(group));
        Task.Run(() => UpdateGroupLog(group));

        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteMapGroupScope(string idgroup, string idscope)
    {
      try
      {
        var group = serviceGroup.GetAllNewVersion(p => p._id == idgroup).Result.FirstOrDefault();
        var scope = group.Scope.Where(p => p._id == idscope).FirstOrDefault();
        group.Scope.Remove(scope);
        serviceGroup.Update(group, null);
        Task.Run(() => UpdateGroupAll(group));
        Task.Run(() => UpdateGroupLog(group));

        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteOccupation(string idoccupation)
    {
      try
      {
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == idoccupation).Result.FirstOrDefault();

        foreach (var item in servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator & p.Occupation._id == occupation._id).Result.ToList())
        {
          return "error_exists_register";
        }

        occupation.Status = EnumStatus.Disabled;
        serviceOccupation.Update(occupation, null);
        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteOccupationActivities(string idoccupation, string idactivitie)
    {
      try
      {
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == idoccupation).Result.FirstOrDefault();
        var activitie = occupation.Activities.Where(p => p._id == idactivitie).FirstOrDefault();
        occupation.Activities.Remove(activitie);
        serviceOccupation.Update(occupation, null);
        Task.Run(() => UpdateOccupationAll(occupation));
        Task.Run(() => UpdateOccupationLog(occupation));

        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteOccupationSkill(string idoccupation, string id)
    {
      try
      {
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == idoccupation).Result.FirstOrDefault();
        var skill = occupation.Skills.Where(p => p._id == id).FirstOrDefault();
        occupation.Skills.Remove(skill);
        serviceOccupation.Update(occupation, null);
        Task.Run(() => UpdateOccupationAll(occupation));
        Task.Run(() => UpdateOccupationLog(occupation));

        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteSkill(string idskill)
    {
      try
      {
        var companys = serviceCompany.GetAllNewVersion().ToList();
        foreach (var company in companys)
        {
          if (company.Skills != null)
            if (company.Skills.Select(p => new { p._id }).Where(p => p._id == idskill).Count() > 0)
              return "error_exists_register";
        }
        var groups = serviceGroup.GetAllNewVersion().ToList();
        foreach (var group in groups)
        {
          if (group.Skills != null)
            if (group.Skills.Select(p => new { p._id }).Where(p => p._id == idskill).Count() > 0)
              return "error_exists_register";
        }
        var occupations = serviceOccupation.GetAllNewVersion().ToList();
        foreach (var occupation in occupations)
        {
          if (occupation.Skills != null)
            if (occupation.Skills.Select(p => new { p._id }).Where(p => p._id == idskill).Count() > 0)
              return "error_exists_register";
        }

        var skill = serviceSkill.GetAllNewVersion(p => p._id == idskill).Result.FirstOrDefault();
        skill.Status = EnumStatus.Disabled;
        serviceSkill.Update(skill, null);
        UpdateSkillAll(skill, true);
        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteSphere(string idsphere)
    {
      try
      {
        var sphere = serviceSphere.GetAllNewVersion(p => p._id == idsphere).Result.FirstOrDefault();

        //foreach (var item in axisService.GetAllNewVersion(p => p.Sphere._id == sphere._id).ToList())
        //{
        //  return "error_exists_register";
        //}

        foreach (var item in serviceGroup.GetAllNewVersion(p => p.Sphere._id == sphere._id).Result.ToList())
        {
          return "error_exists_register";
        }
        sphere.Status = EnumStatus.Disabled;
        serviceSphere.Update(sphere, null);

        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteSchooling(string idschooling)
    {
      try
      {
        var schooling = serviceSchooling.GetAllNewVersion(p => p._id == idschooling).Result.FirstOrDefault();
        schooling.Status = EnumStatus.Disabled;
        serviceSchooling.Update(schooling, null);
        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string DeleteQuestion(string idquestion)
    {
      try
      {
        var questions = serviceQuestions.GetAllNewVersion(p => p._id == idquestion).Result.FirstOrDefault();
        questions.Status = EnumStatus.Disabled;
        serviceQuestions.Update(questions, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteCbo(string id)
    {
      try
      {
        var Cbo = serviceCbo.GetFreeNewVersion(p => p._id == id).Result;
        Cbo.Status = EnumStatus.Disabled;
        serviceCbo.UpdateAccount(Cbo, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListCompany> GetCompanies()
    {
      try
      {
        return serviceCompany.GetAllNewVersion().ToList().Select(p => new ViewListCompany()
        {
          _id = p._id,
          Name = p.Name
        }).OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewGroupListLO> GetGroups(string idcompany)
    {
      try
      {
        List<ViewGroupListLO> groups = new List<ViewGroupListLO>();
        foreach (var item in serviceGroup.GetAllNewVersion(p => p.Company._id == idcompany).Result)
        {
          var view = new ViewGroupListLO
          {
            _id = item._id,
            Name = item.Name,
            Axis = item.Axis,
            Sphere = item.Sphere,
            Company = item.Company,
            Line = item.Line,
            ScopeCount = item.Scope.Count(),
            SchollingCount = item.Schooling.Count(),
            SkillCount = item.Skills.Count()
          };
          groups.Add(view);
        }
        return groups.OrderBy(p => p.Sphere.TypeSphere).ThenBy(p => p.Axis.TypeAxis).ThenBy(p => p.Line).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewGetOccupation> GetOccupations(string idcompany, string idarea)
    {
      try
      {
        AdjustOccuptaions();
        var area = serviceArea.GetNewVersion(p => p._id == idarea).Result;

        var itens = serviceOccupation.GetAllNewVersion(p => p.Group.Company._id == idcompany).Result.OrderBy(p => p.Name).ToList();
        List<ViewGetOccupation> list = new List<ViewGetOccupation>();
        foreach (var item in itens)
        {
          foreach (var proc in item.Process)
          {
            if (proc.ProcessLevelOne.Area != null)
              if (proc.ProcessLevelOne.Area._id == area._id)
              {
                //item.ProcessLevelTwo = proc;
                list.Add(new ViewGetOccupation()
                {
                  _idOccupation = item._id,
                  Description = item.Description,
                  NameOccupation = item.Name,
                  _idProcessLevelTwo = proc._id,
                  _idGroup = item.Group._id
                });
              }
          }
        }
        return list.OrderBy(p => p.NameOccupation).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewOccupationListEdit> ListOccupationsEdit(string idcompany, string idarea, ref long total, string filter, int count, int page, string filterGroup)
    {
      try
      {
        Area area = serviceArea.GetNewVersion(p => p._id == idarea).Result;
        List<Occupation> itens = serviceOccupation.GetAllNewVersion(p => p.Group.Company._id == idcompany).Result.OrderBy(p => p.Name).ThenBy(o => o.Description).ToList();
        List<Occupation> list = new List<Occupation>();
        foreach (Occupation item in itens)
        {
          if (item.Process != null)
          {
            if (item.Process.FirstOrDefault() != null)
            {
              if (item.Process.Where(p => p.ProcessLevelOne.Area._id == idarea).Count() > 0)
              {
                list.Add(new Occupation()
                {
                  Name = item.Name,
                  Group = item.Group,
                  Line = item.Line,
                  Skills = item.Skills,
                  Schooling = item.Schooling,
                  Activities = item.Activities,
                  Template = item.Template,
                  Cbo = item.Cbo,
                  SpecificRequirements = item.SpecificRequirements,
                  Process = item.Process,
                  _id = item._id,
                  Description = item.Description,
                  _idAccount = item._idAccount,
                  Status = item.Status
                });
              }
            }
          }
        }
        list.OrderBy(p => p.Name).ToList();
        int skip = (count * (page - 1));
        List<ViewOccupationListEdit> itensResult = list.Where(p => p.Group.Company._id == idcompany
          & p.Name.ToUpper().Contains(filter.ToUpper())
          & p.Group.Name.ToUpper().Contains(filterGroup.ToUpper())).Skip(skip).Take(count)
          .OrderBy(p => p.Name).ToList().Select(p => new ViewOccupationListEdit
          {
            _id = p._id,
            Name = p.Name,
            NameGroup = p.Group.Name,
            Activities = p.Activities.Count(),
            Skills = p.Skills.Count(),
            Description = p.Description,
            Schooling = p.Schooling.Where(x => x.Complement != null).Count()
          }).ToList();
        total = list.Where(p => p.Group.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())
          & p.Group.Name.ToUpper().Contains(filterGroup.ToUpper())).Count();
        return itensResult;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewOccupationListEdit> ListOccupationsEdit(string idcompany, ref long total, string filter, int count, int page, string filterGroup)
    {
      try
      {
        List<Occupation> itens = serviceOccupation.GetAllNewVersion(p => p.Group.Company._id == idcompany).Result.OrderBy(p => p.Name).ToList();
        int skip = (count * (page - 1));
        List<ViewOccupationListEdit> itensResult = itens.Where(p => p.Group.Company._id == idcompany
          & p.Name.ToUpper().Contains(filter.ToUpper())
          & p.Group.Name.ToUpper().Contains(filterGroup.ToUpper())).
          Skip(skip).Take(count)
          .OrderBy(p => p.Name).ToList().Select(p => new ViewOccupationListEdit
          {
            _id = p._id,
            Name = p.Name,
            NameGroup = p.Group.Name,
            Activities = p.Activities.Count(),
            Skills = p.Skills.Count(),
            Description = p.Description,
            Schooling = p.Schooling.Where(x => x.Complement != null).Count()
          }).ToList();
        total = itens.Where(p => p.Group.Company._id == idcompany
          & p.Name.ToUpper().Contains(filter.ToUpper())
          & p.Group.Name.ToUpper().Contains(filterGroup.ToUpper())).Count();
        return itensResult;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudSkill GetSkill(string filterName)
    {
      try
      {
        List<ViewCrudSkill> detail = serviceSkill.GetAllNewVersion(p => p.Name.ToUpper() == filterName.ToUpper()).Result
          .Select(p => p.GetViewCrud())
          .ToList();
        if (detail.Count == 1)
        {
          return detail[0];
        }
        detail = serviceSkill.GetAllNewVersion(p => p.Name.ToUpper().Contains(filterName.ToUpper())).Result.Select(p => p.GetViewCrud()).ToList();
        if (detail.Count == 1)
        {
          return detail[0];
        }
        if (detail.Count > 1)
        {
          throw new Exception("Mais de uma skill!");
        }
        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListSkill> GetSkills(ref long total, string filter, int count, int page)
    {
      try
      {
        total = serviceSkill.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return serviceSkill.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => p.GetViewList()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewSkills> GetSkills(string company, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var skills = (List<string>)(from comp in serviceCompany.GetAllNewVersion()
                                    where comp._id == company
                                    select new
                                    {
                                      Name = comp.Skills.Select(p => p.Name)
                                    }
                    ).FirstOrDefault().Name;


        var detail = serviceSkill.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result
                      .ToList().Select(p => new ViewSkills()
                      {
                        _id = p._id,
                        _idAccount = p._idAccount,
                        Name = p.Name,
                        Concept = p.Concept,
                        Status = p.Status,
                        TypeSkill = p.TypeSkill,
                        Exists = skills.Contains(p.Name)
                      }).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();

        total = serviceSkill.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewSkills> GetSkillsGroup(string idgroup, string idcompany, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var skills = (List<string>)(from comp in serviceCompany.GetAllNewVersion()
                                    where comp._id == idcompany
                                    select new
                                    {
                                      Name = comp.Skills.Select(p => p.Name)
                                    }
                    ).FirstOrDefault().Name;

        var skillsGroup = (List<string>)(from groups in serviceGroup.GetAllNewVersion()
                                         where groups._id == idgroup
                                         select new
                                         {
                                           Name = groups.Skills.Select(p => p.Name)
                                         }
                   ).FirstOrDefault().Name;


        var detail = serviceSkill.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result
                      .ToList().Select(p => new ViewSkills()
                      {
                        _id = p._id,
                        _idAccount = p._idAccount,
                        Name = p.Name,
                        Concept = p.Concept,
                        Status = p.Status,
                        TypeSkill = p.TypeSkill,
                        Exists = skills.Contains(p.Name),
                        ExistsGroup = skillsGroup.Contains(p.Name)
                      }).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();

        total = serviceSkill.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewSkills> GetSkillsOccupation(string idgroup, string idcompany, string idoccupation, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var skills = (List<string>)(from comp in serviceCompany.GetAllNewVersion()
                                    where comp._id == idcompany
                                    select new
                                    {
                                      Name = comp.Skills.Select(p => p.Name)
                                    }
                    ).FirstOrDefault().Name;

        var skillsGroup = (List<string>)(from groups in serviceGroup.GetAllNewVersion()
                                         where groups._id == idgroup
                                         select new
                                         {
                                           Name = groups.Skills.Select(p => p.Name)
                                         }
                   ).FirstOrDefault().Name;

        var skillsOccupation = (List<string>)(from occupation in serviceOccupation.GetAllNewVersion()
                                              where occupation._id == idoccupation
                                              select new
                                              {
                                                Name = occupation.Skills.Select(p => p.Name)
                                              }
                 ).FirstOrDefault().Name;


        var detail = serviceSkill.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result
                      .ToList().Select(p => new ViewSkills()
                      {
                        _id = p._id,
                        _idAccount = p._idAccount,
                        Name = p.Name,
                        Concept = p.Concept,
                        Status = p.Status,
                        TypeSkill = p.TypeSkill,
                        Exists = skills.Contains(p.Name),
                        ExistsGroup = skillsGroup.Contains(p.Name),
                        ExistsOccupation = skillsOccupation.Contains(p.Name)
                      }).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();

        total = serviceSkill.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudProcessLevelTwo GetProcessLevelTwo(string id)
    {
      try
      {
        var detail = serviceProcessLevelTwo.GetAllNewVersion(p => p._id == id).Result
          .Select(p => new ViewCrudProcessLevelTwo()
          {
            _id = p._id,
            Name = p.Name,
            Order = p.Order,
            ProcessLevelOne = new ViewListProcessLevelOne()
            {
              _id = p.ProcessLevelOne._id,
              Name = p.ProcessLevelOne.Name,
              Order = p.Order,
              Area = p.ProcessLevelOne.Area
            }
          })
          .ToList();
        if (detail.Count == 1)
          return detail[0];
        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListProcessLevelTwo> GetProcessLevelTwoFilter(string idarea)
    {
      try
      {
        //var result = processLevelOneService.GetAllNewVersion(p => p.Area._id == idarea);
        var result = serviceProcessLevelOne.GetAllNewVersion(p => p.Area._id == idarea).Result.OrderBy(p => p.Order).ToList();
        var list = new List<ViewListProcessLevelTwo>();
        foreach (var item in result)
        {
          foreach (var row in serviceProcessLevelTwo.GetAllNewVersion(p => p.ProcessLevelOne._id == item._id).Result.OrderBy(p => p.Order).ToList())
          {
            list.Add(new ViewListProcessLevelTwo()
            {
              _id = row._id,
              Name = row.Name,
              Order = row.Order,
              ProcessLevelOne = new ViewListProcessLevelOne()
              {
                _id = row.ProcessLevelOne._id,
                Name = row.ProcessLevelOne.Name,
                Order = row.Order,
                Area = row.ProcessLevelOne.Area
              }
            });
          }
        }
        return list.OrderBy(p => p.ProcessLevelOne.Area.Name).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListProcessLevelTwo> GetProcessLevelTwo()
    {
      try
      {
        //var result = processLevelOneService.GetAllNewVersion(p => p.Area._id == idarea);
        var result = serviceProcessLevelOne.GetAllNewVersion().OrderBy(p => p.Order).ToList();
        var list = new List<ViewListProcessLevelTwo>();
        foreach (var item in result)
        {
          foreach (var row in serviceProcessLevelTwo.GetAllNewVersion(p => p.ProcessLevelOne._id == item._id).Result.OrderBy(p => p.Order).ToList())
          {
            list.Add(new ViewListProcessLevelTwo()
            {
              _id = row._id,
              Name = row.Name,
              Order = row.Order,
              ProcessLevelOne = new ViewListProcessLevelOne()
              {
                _id = row.ProcessLevelOne._id,
                Name = row.ProcessLevelOne.Name,
                Order = row.Order,
                Area = row.ProcessLevelOne.Area
              }
            });
          }
        }
        return list.OrderBy(p => p.ProcessLevelOne.Area.Name).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddSkills(List<ViewCrudSkill> view)
    {
      try
      {
        foreach (var item in view)
        {
          var skill = new Skill()
          {
            Name = item.Name.Trim(),
            Concept = item.Concept,
            TypeSkill = item.TypeSkill,
            Status = EnumStatus.Enabled
          };
          serviceSkill.InsertNewVersion(skill);
        }

        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudGroup GetGroup(string id)
    {
      try
      {
        return serviceGroup.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudGroup GetGroup(string idCompany, string filterName)
    {
      try
      {
        return serviceGroup.GetNewVersion(p => p.Company._id == idCompany && p.Name.ToUpper().Contains(filterName.ToUpper())).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListGroup> GetGroups()
    {
      try
      {

        //var groups = new List<Group>();
        //foreach (var item in groupService.GetAllNewVersion())
        //{
        //  item.Occupations = occupationService.GetAllNewVersion(p => p.Group._id == item._id).OrderBy(p => p.Name).ToList();
        //  groups.Add(item);
        //}
        //return groups.OrderBy(p => p.Name)
        //  .Select()
        //  .ToList();
        return serviceGroup.GetAllNewVersion().Select(p => p.GetViewList()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListGroup> GetGroupsPrint(string idcompany)
    {
      try
      {
        List<Group> groups = new List<Group>();
        foreach (var item in serviceGroup.GetAllNewVersion(p => p.Company._id == idcompany).Result)
        {
          item.Occupations = serviceOccupation.GetAllNewVersion(p => p.Group._id == item._id).Result.Select(p => p.GetViewList()).ToList();
          groups.Add(item);
        }
        return groups.Select(p => p.GetViewList()).OrderByDescending(p => p.Sphere.TypeSphere).ThenByDescending(p => p.Axis.TypeAxis).ThenByDescending(p => p.Line).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudOccupation GetOccupation(string id)
    {
      try
      {
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == id).Result.ToList().Select(p =>
          new ViewCrudOccupation()
          {
            _id = p._id,
            Name = p.Name,
            Description = p.Description,
            SalaryScales = p.SalaryScales?.Select(s => new ViewCrudSalaryScaleOccupation()
            {
              _id = s._idSalaryScale,
              Name = s.NameSalaryScale,
              NameGrade = s.NameGrade,
              _idGrade = s._idGrade,
              Workload = s.Workload,
              StepLimit = s.StepLimit
            }).ToList(),
            Group = p.Group,
            Line = p.Line,
            Process = p.Process?.OrderBy(x => x.ProcessLevelOne.Area.Name).ThenBy(x => x.ProcessLevelOne.Order).ThenBy(x => x.Order)
            .Select(x => new ViewListProcessLevelTwo()
            {
              _id = x._id,
              Name = x.Name,
              Order = x.Order,
              ProcessLevelOne = new
            ViewListProcessLevelOne()
              {
                _id = x.ProcessLevelOne._id,
                Name = x.ProcessLevelOne.Name,
                Order = x.ProcessLevelOne.Order,
                Area = x.ProcessLevelOne.Area
              }
            })
            .ToList()
          }).FirstOrDefault();

        return occupation;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListCourse> GetCourseOccupation(string idoccuation, EnumTypeMandatoryTraining type)
    {
      try
      {

        var list = new List<ViewListCourse>();
        var idcompany = serviceOccupation.GetAllNewVersion(p => p._id == idoccuation).Result.FirstOrDefault().Group.Company._id;
        var occupations = serviceOccupationMandatory.GetAllNewVersion(p => p.Occupation._id == idoccuation & p.TypeMandatoryTraining == type).Result.ToList();
        var company = serviceCompanyMandatory.GetAllNewVersion(p => p.Company._id == idcompany & p.TypeMandatoryTraining == type).Result.ToList();

        foreach (var item in occupations)
        {
          list.Add(new ViewListCourse()
          {
            _id = item.Course._id,
            Name = item.Course.Name
          });
        }

        foreach (var item in company)
        {
          list.Add(new ViewListCourse()
          {
            _id = item.Course._id,
            Name = item.Course.Name
          });
        }

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudOccupation GetOccupation(string idCompany, string filterName)
    {
      try
      {
        return serviceOccupation.GetAllNewVersion(p => p.Group.Company._id == idCompany && p.Name.ToUpper() == filterName.ToUpper()).Result.ToList().Select(p =>
          new ViewCrudOccupation()
          {
            _id = p._id,
            Name = p.Name,
            Group = p.Group,
            Line = p.Line,
            Description = p.Description,
            Process = p.Process?.OrderBy(x => x.ProcessLevelOne.Area.Name).ThenBy(x => x.ProcessLevelOne.Order).ThenBy(x => x.Order)
            .Select(x => new ViewListProcessLevelTwo()
            {
              _id = x._id,
              Name = x.Name,
              Order = x.Order,
              ProcessLevelOne = new
            ViewListProcessLevelOne()
              {
                _id = x.ProcessLevelOne._id,
                Name = x.ProcessLevelOne.Name,
                Order = x.ProcessLevelOne.Order,
                Area = x.ProcessLevelOne.Area
              }
            })
            .ToList()
          }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListOccupationExport> GetOccupations()
    {
      try
      {
        var list = serviceOccupation.GetAllNewVersion().OrderBy(p => p.Name).ToList();
        List<ViewListOccupationExport> result = new List<ViewListOccupationExport>();
        foreach (var item in list)
        {
          foreach (var proc in item.Process)
          {
            result.Add(new ViewListOccupationExport()
            {
              _id = item._id,
              NameCargo = item.Name,
              Description = item.Description,
              NameGroup = item.Group.Name,
              NameSphere = item.Group.Sphere.Name,
              NameAxis = item.Group.Axis.Name,
              NameArea = proc.ProcessLevelOne.Area.Name,
              NameProcess = proc.ProcessLevelOne.Name,
              NameSubProcess = proc.Name,
              StatusActivites = item.Activities.Count() == 0 ? "Sem Entregas" : "Ok",
              StatusSkill = item.Skills.Count() == 0 ? "Sem Competências Específicas" : "Ok",
              StatusSchooling = item.Schooling.Where(x => x.Complement != null).Count() == 0 ? "Sem complementos" : "Ok"
            });
          }
        }
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudTextDefault GetTextDefault(string idcompany, string name)
    {
      try
      {
        var item = serviceTextDefault.GetAllNewVersion(p => p.Company._id == idcompany & p.Name == name).Result.FirstOrDefault();
        return new ViewCrudTextDefault()
        {
          Name = item.Name,
          Company = new ViewListCompany() { _id = item.Company._id, Name = item.Company.Name },
          Content = item.Content,
          TypeText = item.TypeText,
          _id = item._id
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudTextDefault GetTextDefault(string id)
    {
      try
      {
        var item = serviceTextDefault.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
        return new ViewCrudTextDefault()
        {
          Name = item.Name,
          Company = new ViewListCompany() { _id = item.Company._id, Name = item.Company.Name },
          Content = item.Content,
          TypeText = item.TypeText,
          _id = item._id
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListTextDefault> ListTextDefault(string idcompany)
    {
      try
      {
        return serviceTextDefault.GetAllNewVersion(p => p.Company._id == idcompany).Result
          .Select(p => new ViewListTextDefault()
          {
            _id = p._id,
            Name = p.Name,
            Content = p.Content
          })
          .ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateMapOccupationSchooling(string idoccupation, ViewCrudSchoolingOccupation view)
    {
      try
      {
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == idoccupation).Result.FirstOrDefault();
        //var schooling = serviceSchooling.GetAllNewVersion(p => p._id == view._id).FirstOrDefault();

        var schoolOld = occupation.Schooling.Where(p => p._id == view._id).FirstOrDefault();
        var schooling = schoolOld;
        schooling.Complement = view.Complement;
        schooling.Type = view.Type;

        occupation.Schooling.Remove(schoolOld);
        occupation.Schooling.Add(schooling);

        serviceOccupation.Update(occupation, null);
        Task.Run(() => UpdateOccupationAll(occupation));
        Task.Run(() => UpdateOccupationLog(occupation));
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateMapOccupationActivities(string idoccupation, ViewCrudActivities view)
    {
      try
      {
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == idoccupation).Result.FirstOrDefault();
        var activitie = new Activitie()
        {
          Name = view.Name,
          Order = view.Order,
          _id = view._id
        };
        if (activitie._id == null)
          activitie._id = ObjectId.GenerateNewId().ToString();

        var activitieOld = occupation.Activities.Where(p => p._id == activitie._id).FirstOrDefault();
        occupation.Activities.Remove(activitieOld);
        occupation.Activities.Add(activitie.GetViewList());

        serviceOccupation.Update(occupation, null);
        Task.Run(() => UpdateOccupationAll(occupation));
        Task.Run(() => UpdateOccupationLog(occupation));
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateMapGroupScope(string idgroup, ViewCrudScope view)
    {
      try
      {
        var group = serviceGroup.GetAllNewVersion(p => p._id == idgroup).Result.FirstOrDefault();
        var scope = new Scope()
        {
          Name = view.Name,
          Order = view.Order,
          _id = view._id
        };
        if (scope._id == null)
          scope._id = ObjectId.GenerateNewId().ToString();

        var scopeOld = group.Scope.Where(p => p._id == scope._id).FirstOrDefault();
        group.Scope.Remove(scopeOld);
        group.Scope.Add(scope.GetViewList());

        serviceGroup.Update(group, null);
        Task.Run(() => UpdateGroupAll(group));
        Task.Run(() => UpdateGroupLog(group));
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateMapGroupSchooling(string idgroup, ViewCrudSchooling view)
    {
      try
      {
        var group = serviceGroup.GetAllNewVersion(p => p._id == idgroup).Result.FirstOrDefault();
        var schooling = serviceSchooling.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();

        schooling.Type = view.Type;

        var schoolOld = group.Schooling.Where(p => p._id == schooling._id).FirstOrDefault();
        group.Schooling.Remove(schoolOld);
        group.Schooling.Add(schooling.GetViewCrud());

        serviceGroup.Update(group, null);
        Task.Run(() => UpdateGroupAll(group));
        Task.Run(() => UpdateGroupLog(group));
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateGroupSphereAxis(Group group, Group groupOld)
    {
      try
      {
        //var groupOld = groupService.GetAllNewVersion(p => p._id == group._id).FirstOrDefault();
        groupOld.Status = EnumStatus.Disabled;
        serviceGroup.Update(groupOld, null);

        var groupnew = AddGroupInternal(new ViewAddGroup()
        {
          Axis = group.Axis,
          Sphere = group.Sphere,
          Company = group.Company,
          Line = group.Line,
          Name = group.Name
        });

        groupnew.Schooling = groupOld.Schooling;
        groupnew.Skills = groupOld.Skills;
        groupnew.Scope = groupOld.Scope;
        groupnew.Template = groupOld.Template;

        serviceGroup.Update(groupnew, null);

        Task.Run(() => UpdateGroupAll(groupnew));
        Task.Run(() => UpdateGroupLog(groupnew));
        UpdateGroupOccupationAll(groupnew, groupOld);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public string UpdateArea(ViewCrudArea view)
    {
      try
      {
        var model = serviceArea.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
        model.Name = view.Name;
        model.Order = view.Order;
        model.Company = view.Company;

        serviceArea.Update(model, null);
        UpdateAreaAll(model);
        UpdateAreaProcessAll(model);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateAxis(ViewCrudAxis view)
    {
      try
      {
        var model = serviceAxis.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
        model.Name = view.Name;
        model.Company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result.GetViewList();
        model.TypeAxis = view.TypeAxis;

        serviceAxis.Update(model, null);
        UpdateAxisAll(model, false);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateCbo(ViewCrudCbo view)
    {
      try
      {
        var model = serviceCbo.GetFreeNewVersion(p => p._id == view._id).Result;
        model.Name = view.Name;
        model.Code = view.Code;

        serviceCbo.UpdateAccount(model, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateGroup(ViewCrudGroup view)
    {
      try
      {
        var groupOld = serviceGroup.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
        var group = serviceGroup.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();

        group.Name = view.Name;
        group.Company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result.GetViewList();
        group.Line = view.Line;
        group.Axis = serviceAxis.GetNewVersion(p => p._id == view.Axis._id).Result.GetViewList();
        group.Sphere = serviceSphere.GetNewVersion(p => p._id == view.Sphere._id).Result.GetViewList();

        if ((groupOld.Sphere._id != view.Sphere._id) || (groupOld.Axis._id != view.Axis._id))
        {
          UpdateGroupSphereAxis(group, groupOld);
          return "update";
        }


        serviceGroup.Update(group, null);
        Task.Run(() => UpdateGroupAll(group));
        Task.Run(() => UpdateGroupLog(group));
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateOccupation(ViewCrudOccupation view)
    {
      try
      {
        //var areas = new List<Area>();
        var occupationOld = serviceOccupation.GetNewVersion(p => p._id == view._id).Result;
        var occupation = serviceOccupation.GetNewVersion(p => p._id == view._id).Result;

        occupation.Group = serviceGroup.GetNewVersion(p => p._id == view.Group._id).Result.GetViewList();
        occupation.Name = view.Name;
        occupation.Description = view.Description;
        occupation.Line = view.Line;

        if (view.Cbo != null)
          occupation.Cbo = serviceCbo.GetNewVersion(p => p._id == view.Cbo._id).Result.GetViewList();

        occupation.SalaryScales = new List<SalaryScaleGrade>();
        if (view.SalaryScales != null)
        {
          foreach (var item in view.SalaryScales)
          {
            occupation.SalaryScales.Add(new SalaryScaleGrade()
            {
              _idSalaryScale = item._id,
              NameSalaryScale = item.Name,
              _idGrade = item._idGrade,
              NameGrade = item.NameGrade,
              Workload = item.Workload,
              StepLimit = item.StepLimit,
              _id = ObjectId.GenerateNewId().ToString()
            });
          }
        }


        occupation.Process = new List<ViewListProcessLevelTwo>();
        if (view.Process != null)
        {
          foreach (var item in view.Process)
            occupation.Process.Add(serviceProcessLevelTwo.GetNewVersion(p => p._id == item._id).Result.GetViewList());
        }


        //foreach (var item in occupation.Process)
        //  areas.Add(item.ProcessLevelOne.Area);

        if (occupationOld.Group != occupation.Group)
        {
          occupation.Schooling = new List<ViewCrudSchoolingOccupation>();
          var group = serviceGroup.GetNewVersion(p => p._id == occupation.Group._id).Result;
          foreach (var school in group.Schooling)
          {
            var schoolnew = new ViewCrudSchoolingOccupation()
            {
              Name = school.Name,
              Order = school.Order,
              Type = school.Type,
              _id = school._id
            };
            var schoolold = occupationOld.Schooling.Where(p => p._id == school._id).FirstOrDefault();
            if (schoolold != null)
              schoolnew.Complement = schoolold.Complement;

            occupation.Schooling.Add(schoolnew);

          }
        }

        //occupation.Areas = areas;
        var list = occupation.SalaryScales;
        occupation.SalaryScales = new List<SalaryScaleGrade>();
        if (list != null)
        {
          foreach (var item in list)
          {
            if (item._id == null)
              item._id = ObjectId.GenerateNewId().ToString();

            occupation.SalaryScales.Add(item);
          }
        }

        serviceOccupation.Update(occupation, null);

        Task.Run(() => UpdateOccupationAll(occupation));
        Task.Run(() => UpdateOccupationLog(occupation));
        return "update";
      }

      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateSkill(ViewCrudSkill view)
    {
      try
      {
        var model = serviceSkill.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
        model.Name = view.Name;
        model.Concept = view.Concept;
        model.TypeSkill = view.TypeSkill;

        serviceSkill.Update(model, null);
        UpdateSkillAll(model, false);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateQuestions(ViewCrudQuestions view)
    {
      try
      {
        var model = serviceQuestions.GetNewVersion(p => p._id == view._id).Result;
        model.Name = view.Name;
        model.Content = view.Content;
        model.TypeQuestion = view.TypeQuestion;
        model.TypeRotine = view.TypeRotine;

        serviceQuestions.Update(model, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateSphere(ViewCrudSphere view)
    {
      try
      {
        var model = serviceSphere.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
        model.Name = view.Name;
        model.Company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result.GetViewList();
        model.TypeSphere = view.TypeSphere;


        serviceSphere.Update(model, null);
        UpdateSphereAll(model, false);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateSchooling(ViewCrudSchooling view)
    {
      try
      {
        var model = serviceSchooling.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
        model.Name = view.Name;
        model.Type = view.Type;

        serviceSchooling.Update(model, null);
        UpdateSchoolingAll(model);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateProcessLevelOne(ViewCrudProcessLevelOne view)
    {
      try
      {
        var model = serviceProcessLevelOne.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
        model.Name = view.Name;
        model.Area = serviceArea.GetNewVersion(p => p._id == view.Area._id).Result.GetViewList();
        model.Order = view.Order;

        serviceProcessLevelOne.Update(model, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateProcessLevelTwo(ViewCrudProcessLevelTwo view)
    {
      try
      {
        var model = serviceProcessLevelTwo.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
        model.Name = view.Name;
        model.ProcessLevelOne = serviceProcessLevelOne.GetNewVersion(p => p._id == view.ProcessLevelOne._id).Result.GetViewList();
        model.Order = view.Order;
        model.Comments = view.Comments;

        serviceProcessLevelTwo.Update(model, null);

        Task.Run(() => UpdateProcessLevelTwoAll(model._id));
        //UpdateProcessLevelTwoAll(model._id);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateTextDefault(ViewCrudTextDefault view)
    {
      try
      {
        var model = serviceTextDefault.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
        model.Name = view.Name;
        model.Company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result.GetViewList();
        model.Content = view.Content;
        model.TypeText = view.TypeText;

        serviceTextDefault.Update(model, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }



    public ViewMapGroup GetMapGroup(string id)
    {
      try
      {
        var group = serviceGroup.GetNewVersion(p => p._id == id).Result;
        var company = serviceCompany.GetNewVersion(p => p._id == group.Company._id).Result;

        var view = new ViewMapGroup()
        {
          _id = group._id,
          Name = group.Name,
          Line = group.Line,
          Company = group.Company,
          Axis = group.Axis,
          Sphere = group.Sphere,
          Schooling = group.Schooling?.OrderBy(o => o.Order).ToList(),
          Scope = group.Scope?.OrderBy(o => o.Order).Select(p => new ViewListScope()
          {
            _id = p._id,
            Name = p.Name,
            Order = p.Order
          }).ToList(),
          Skills = group.Skills?.OrderBy(o => o.Name).ToList(),
          SkillsCompany = company.Skills?.OrderBy(o => o.Name).ToList()
        };
        return view;
      }
      catch (Exception e)
      {
        throw e;
      }

    }

    public ViewCrudMapGroupScope GetMapGroupScopeById(string idgroup, string idscope)
    {
      try
      {
        var group = serviceGroup.GetAllNewVersion(p => p._id == idgroup).Result.FirstOrDefault();


        return group.Scope.Where(p => p._id == idscope)
        .Select(p => new ViewCrudMapGroupScope()
        {
          _idGroup = idgroup,
          Scope = new ViewCrudScope()
          {
            _id = p._id,
            Name = p.Name,
            Order = p.Order
          }
        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListProcessLevelOneByArea> GetListProcessLevelOneByArea(string idarea)
    {
      try
      {
        var list = serviceProcessLevelOne.GetAllNewVersion(p => p.Area._id == idarea).Result.ToList();

        var result = new List<ViewListProcessLevelOneByArea>();
        foreach (var item in list)
        {
          var view = new ViewListProcessLevelOneByArea()
          {
            _id = item._id,
            Name = item.Name,
            Area = item.Area,
            Order = item.Order,
            Process = serviceProcessLevelTwo.GetAllNewVersion(p => p.ProcessLevelOne._id == item._id).Result.Select(x => new ViewCrudProcessLevelTwo()
            {
              _id = x._id,
              Name = x.Name,
              Order = x.Order,
              Comments = x.Comments
            }).OrderBy(p => p.Order).ToList()
          };

          result.Add(view);
        }



        return result.OrderBy(p => p.Order).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudArea GetAreasById(string id)
    {
      try
      {
        return serviceArea.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault().GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewMapOccupation GetMapOccupation(string id)
    {
      try
      {
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();

        var group = serviceGroup.GetAllNewVersion(p => p._id == occupation.Group._id).Result.FirstOrDefault();
        var company = serviceCompany.GetAllNewVersion(p => p._id == group.Company._id).Result.FirstOrDefault();

        var map = new ViewMapOccupation()
        {
          _id = occupation._id,
          Name = occupation.Name,
          Description = occupation.Description,
          SpecificRequirements = occupation.SpecificRequirements,
          Company = new ViewListCompany() { _id = company._id, Name = company.Name },
          Group = group.GetViewList(),
          Activities = occupation.Activities?.OrderBy(o => o.Order).ToList(),
          Process = occupation.Process.Select(x => new ViewListProcessLevelTwo()
          {
            _id = x._id,
            Name = x.Name,
            Order = x.Order,
            ProcessLevelOne = new ViewListProcessLevelOne()
            {
              _id = x.ProcessLevelOne._id,
              Name = x.ProcessLevelOne.Name,
              Order = x.ProcessLevelOne.Order,
              Area = x.ProcessLevelOne?.Area
            }
          }).ToList(),
          Schooling = occupation.Schooling?.OrderBy(o => o.Order).Select(x => x.GetViewCrudOccupation()).ToList(),
          Skills = occupation.Skills?.OrderBy(o => o?.Name).ToList(),
          SkillsCompany = company.Skills?.OrderBy(o => o.Name).ToList(),
          SkillsGroup = group.Skills?.OrderBy(o => o.Name).ToList(),
          ScopeGroup = group.Scope?.OrderBy(o => o.Order).Select(x => new ViewListScope()
          {
            _id = x._id,
            Name = x.Name,
            Order = x.Order
          }).ToList()
        };

        return map;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewListOccupationLog> ListOccupationLog(ref long total, string id, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var occupations = serviceOccupation.GetAllNewVersion(p => p.Status == EnumStatus.Enabled
        && p._id == id).Result;

        var listCompany = serviceCompanyLog.GetAllNewVersion(p => p.Status == EnumStatus.Enabled)
          .Result.Select(p => new ViewListOccupationLog()
          {
            _id = p._idCompanyPrevious,
            Name = p.Name,
            DateLog = p.DateLog,
            Date = p.Date
          }).ToList();

        var listGroup = serviceGroupLog.GetAllNewVersion(p => p.Status == EnumStatus.Enabled)
          .Result.Select(p => new ViewListOccupationLog()
          {
            _id = p._idGroupPrevious,
            Name = p.Name,
            DateLog = p.DateLog,
            Date = p.Date
          }).ToList();


        var list = serviceOccupationLog.GetAllNewVersion(p => p.Status == EnumStatus.Enabled
        && p._idOccupationPrevious == id)
          .Result.Select(p => new ViewListOccupationLog()
          {
            _id = p._idOccupationPrevious,
            Name = p.Name,
            DateLog = p.DateLog,
            Date = p.Date
          }).ToList();

        foreach (var item in occupations)
        {
          foreach (var company in listCompany.Where(p => p._id == item.Group.Company._id))
          {
            if (list.Where(p => p._id == item._id && p.Date == company.Date).Count() == 0)
            {
              if (company.Date >= list.FirstOrDefault()?.Date)
                list.Add(new ViewListOccupationLog()
                {
                  _id = item._id,
                  Name = item.Name,
                  Date = company.Date,
                  DateLog = company.DateLog
                });
            }

          }
          foreach (var group in listGroup.Where(p => p._id == item.Group._id))
          {
            if (list.Where(p => p._id == item._id && p.Date == group.Date).Count() == 0)
            {
              if (group.Date >= list.FirstOrDefault()?.Date)
                list.Add(new ViewListOccupationLog()
                {
                  _id = item._id,
                  Name = item.Name,
                  Date = group.Date,
                  DateLog = group.DateLog
                });
            }

          }
        }

        if (list.Count() == 0)
        {
          var item = occupations.FirstOrDefault();
          var company = listCompany.FirstOrDefault();
          list.Add(new ViewListOccupationLog()
          {
            _id = item._id,
            Name = item.Name,
            Date = company.Date,
            DateLog = company.DateLog
          });

        }

        total = list.Count();
        return list.Skip(skip).Take(count).OrderBy(p => p.Name).ThenByDescending(p => p.DateLog).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewMapOccupation GetMapOccupationLog(ViewFilterDateOne date, string id)
    {
      try
      {
        var occupation = serviceOccupationLog.GetNewVersion(p => p._idOccupationPrevious == id
        && p.Date <= date.Date).Result;

        if (occupation == null)
          return null;

        var group = serviceGroupLog.GetAllNewVersion(p => p._idGroupPrevious == occupation.Group._id
        && p.Date <= date.Date).Result.LastOrDefault();
        var company = serviceCompanyLog.GetAllNewVersion(p => p._idCompanyPrevious == occupation.Group.Company._id
        && p.Date <= date.Date).Result.LastOrDefault();

        var map = new ViewMapOccupation()
        {
          _id = occupation._id,
          Name = occupation.Name,
          Description = occupation.Description,
          SpecificRequirements = occupation.SpecificRequirements,
          Company = company == null ? null : new ViewListCompany() { _id = company._id, Name = company.Name },
          Group = group?.GetViewList(),
          Activities = occupation.Activities?.OrderBy(o => o.Order).ToList(),
          Process = occupation.Process?.Select(x => new ViewListProcessLevelTwo()
          {
            _id = x._id,
            Name = x.Name,
            Order = x.Order,
            ProcessLevelOne = new ViewListProcessLevelOne()
            {
              _id = x.ProcessLevelOne._id,
              Name = x.ProcessLevelOne.Name,
              Order = x.ProcessLevelOne.Order,
              Area = x.ProcessLevelOne?.Area
            }
          }).ToList(),
          Schooling = occupation.Schooling?.OrderBy(o => o.Order).Select(x => x.GetViewCrudOccupation()).ToList(),
          Skills = occupation.Skills?.OrderBy(o => o?.Name).ToList(),
          SkillsCompany = company?.Skills?.OrderBy(o => o.Name).ToList(),
          SkillsGroup = group?.Skills?.OrderBy(o => o.Name).ToList(),
          ScopeGroup = group?.Scope?.OrderBy(o => o.Order).Select(x => new ViewListScope()
          {
            _id = x._id,
            Name = x.Name,
            Order = x.Order
          }).ToList()
        };

        return map;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudProcessLevelOne GetListProcessLevelOneById(string id)
    {
      try
      {
        return serviceProcessLevelOne.GetAllNewVersion(p => p._id == id).Result.Select(p => new ViewCrudProcessLevelOne()
        {
          _id = p._id,
          Name = p.Name,
          Order = p.Order,
          Area = p.Area
        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public ViewCrudProcessLevelTwo GetListProcessLevelTwoById(string id)
    {
      try
      {
        return serviceProcessLevelTwo.GetAllNewVersion(p => p._id == id).Result.Select(p => new ViewCrudProcessLevelTwo()
        {
          _id = p._id,
          Name = p.Name,
          Comments = p.Comments,
          Order = p.Order,
          ProcessLevelOne = new ViewListProcessLevelOne()
          {
            _id = p.ProcessLevelOne._id,
            Name = p.ProcessLevelOne.Name,
            Order = p.ProcessLevelOne.Order,
            Area = p.ProcessLevelOne.Area
          }
        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListOpportunityLineExport> ListOpportunityLine(string idcompany)
    {
      try
      {
        var occupations = serviceOccupation.GetAllNewVersion(p => p.Group.Company._id == idcompany).Result;
        var list = new List<ViewListOpportunityLine>();
        foreach (var item in occupations)
        {
          Group group = serviceGroup.GetNewVersion(p => p._id == item.Group._id).Result;
          foreach (var proc in item.Process)
          {
            var view = new ViewListOpportunityLine
            {
              Occupation = item.Name,
              Group = item.Group.Name,
              LineGroup = item.Group.Line,
              Shepre = group.Sphere.Name,
              TypeShepre = group.Sphere.TypeSphere,
              Axis = group.Axis.Name,
              TypeAxis = group.Axis.TypeAxis,
              ProcessLevelOne = proc.ProcessLevelOne?.Name,
              Area = proc.ProcessLevelOne?.Area?.Name,
              ProcessLevelTwo = proc.Name
            };
            list.Add(view);
          }
        }
        return list.OrderBy(p => p.Area).ThenBy(p => p.TypeShepre).ThenBy(p => p.TypeAxis)
          .ThenBy(p => p.LineGroup).ThenBy(p => p.ProcessLevelOne)
          .ThenBy(p => p.ProcessLevelTwo).ThenBy(p => p.Occupation)
          .Select(p => new ViewListOpportunityLineExport()
          {
            Occupation = p.Occupation,
            Group = p.Group,
            Sphere = p.Shepre,
            Axis = p.Axis,
            ProcessLevelOne = p.ProcessLevelOne,
            Area = p.Area,
            ProcessLevelTwo = p.ProcessLevelTwo
          }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion Infra


    #region private 

    private void UpdateCompanyLog(Company company)
    {
      try
      {
        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");
        bool exists = true;
        var companyLog = serviceCompanyLog.GetNewVersion(p =>
        p._idCompanyPrevious == company._id
        && p.Date == datenow).Result;

        if (companyLog == null)
        {
          companyLog = new CompanyLog();
          exists = false;
        }
        companyLog.Name = company.Name;
        companyLog.Skills = company.Skills;
        companyLog.Template = company.Template;
        companyLog.Status = company.Status;
        companyLog._idAccount = company._idAccount;
        companyLog._idCompanyPrevious = company._id;
        companyLog.Logo = company.Logo;
        companyLog.Date = datenow;
        companyLog.DateLog = DateTime.Now;

        if (exists)
          serviceCompanyLog.Update(companyLog, null);
        else
          serviceCompanyLog.InsertNewVersion(companyLog);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void UpdateGroupLog(Group group)
    {
      try
      {
        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");
        bool exists = true;
        var groupLog = serviceGroupLog.GetNewVersion(p =>
        p._idGroupPrevious == group._id
        && p.Date == datenow).Result;

        if (groupLog == null)
        {
          groupLog = new GroupLog();
          exists = false;
        }
        groupLog.Name = group.Name;
        groupLog.Company = group.Company;
        groupLog.Line = group.Line;
        groupLog.Skills = group.Skills;
        groupLog.Schooling = group.Schooling;
        groupLog.Scope = group.Scope;
        groupLog.Sphere = group.Sphere;
        groupLog.Axis = group.Axis;
        groupLog.Template = group.Template;
        groupLog.Status = group.Status;
        groupLog._idAccount = group._idAccount;
        groupLog._idGroupPrevious = group._id;
        groupLog.Date = datenow;
        groupLog.DateLog = DateTime.Now;

        if (exists)
          serviceGroupLog.Update(groupLog, null);
        else
          serviceGroupLog.InsertNewVersion(groupLog);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void UpdateOccupationLog(Occupation occupation)
    {
      try
      {
        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");
        bool exists = true;
        var occupationLog = serviceOccupationLog.GetNewVersion(p =>
        p._idOccupationPrevious == occupation._id
        && p.Date == datenow).Result;

        if (occupationLog == null)
        {
          occupationLog = new OccupationLog();
          exists = false;
        }
        occupationLog.Name = occupation.Name;
        occupationLog.Group = occupation.Group;
        occupationLog.Line = occupation.Line;
        occupationLog.Skills = occupation.Skills;
        occupationLog.Schooling = occupation.Schooling;
        occupationLog.Activities = occupation.Activities;
        occupationLog.Template = occupation.Template;
        occupationLog.Cbo = occupation.Cbo;
        occupationLog.SpecificRequirements = occupation.SpecificRequirements;
        occupationLog.Process = occupation.Process;
        occupationLog.SalaryScales = occupation.SalaryScales;
        occupationLog.Description = occupation.Description;
        occupationLog.Status = occupation.Status;
        occupationLog._idAccount = occupation._idAccount;
        occupationLog._idOccupationPrevious = occupation._id;
        occupationLog.Date = datenow;
        occupationLog.DateLog = DateTime.Now;

        if (exists)
          serviceOccupationLog.Update(occupationLog, null);
        else
          serviceOccupationLog.InsertNewVersion(occupationLog);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private Group AddGroupInternal(ViewAddGroup view)
    {
      try
      {
        var group = AddGroup(view);
        return serviceGroup.GetAllNewVersion(p => p._id == group._id).Result.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateSkillAll(Skill skill, bool remove)
    {
      try
      {
        foreach (var company in serviceCompany.GetAllNewVersion().ToList())
        {
          foreach (var item in company.Skills)
          {
            if (item._id == skill._id)
            {
              company.Skills.Remove(item);
              if (remove == false)
                company.Skills.Add(skill.GetViewList());

              break;
            }
          }

          serviceCompany.Update(company, null);
          Task.Run(() => UpdateCompanyAll(company));
          Task.Run(() => UpdateCompanyLog(company));

          foreach (var group in serviceGroup.GetAllNewVersion(p => p.Company._id == company._id).Result.ToList())
          {
            foreach (var item in group.Skills)
            {
              if (item._id == skill._id)
              {
                group.Skills.Remove(item);
                if (remove == false)
                  group.Skills.Add(skill.GetViewList());

                break;
              }
            }
            serviceGroup.Update(group, null);
            Task.Run(() => UpdateGroupAll(group));
            Task.Run(() => UpdateGroupLog(group));
          }

          foreach (var occupation in serviceOccupation.GetAllNewVersion(p => p.Group.Company._id == company._id).Result.ToList())
          {
            foreach (var item in occupation.Skills)
            {
              if (item._id == skill._id)
              {
                occupation.Skills.Remove(item);
                if (remove == false)
                  occupation.Skills.Add(skill.GetViewList());

                break;
              }
            }
            serviceOccupation.Update(occupation, null);
            Task.Run(() => UpdateOccupationAll(occupation));
            Task.Run(() => UpdateOccupationLog(occupation));
          }
        }
      }
      catch (Exception e)
      {
        throw e;
      }

    }

    private Skill AddSkillInternal(ViewCrudSkill view)
    {
      try
      {
        var skill = new Skill()
        {
          Name = view.Name.Trim(),
          Concept = view.Concept,
          TypeSkill = view.TypeSkill,
          Status = EnumStatus.Enabled
        };
        return serviceSkill.InsertNewVersion(skill).Result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async void AdjustOccuptaions()
    {
      try
      {
        //var list = occupationService.GetAuthentication(p => p.Process == null).ToList();
        //foreach (var item in list)
        //{
        //  item.Process = new List<ProcessLevelTwo>();
        //  if (item.ProcessLevelTwo != null)
        //  {
        //    item.Process.Add(item.ProcessLevelTwo);
        //    occupationService.UpdateAccount(item, null);
        //  }
        //}

        //var listAreas = occupationService.GetAuthentication(p => p.Areas == null).ToList();
        //foreach (var item in listAreas)
        //{
        //  item.Areas = new List<Area>();
        //  if (item.Areas != null)
        //  {
        //    item.Areas.Add(item.Area);
        //    occupationService.UpdateAccount(item, null);
        //  }
        //}

      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async Task UpdateCompanyAll(Company company)
    {
      try
      {
        foreach (var item in servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator & p.Company._id == company._id).Result.ToList())
        {
          item.Company = company.GetViewList();
          servicePerson.Update(item, null);
        }

        foreach (var item in serviceSphere.GetAllNewVersion(p => p.Company._id == company._id).Result.ToList())
        {
          item.Company = company.GetViewList();
          serviceSphere.Update(item, null);
        }

        foreach (var item in serviceAxis.GetAllNewVersion(p => p.Company._id == company._id).Result.ToList())
        {
          item.Company = company.GetViewList();
          serviceAxis.Update(item, null);
        }

        foreach (var item in serviceOccupation.GetAllNewVersion(p => p.Group.Company._id == company._id).Result.ToList())
        {
          item.Group.Company = company.GetViewList();
          serviceOccupation.Update(item, null);
        }

        foreach (var item in serviceGroup.GetAllNewVersion(p => p.Company._id == company._id).Result.ToList())
        {
          item.Company = company.GetViewList();
          serviceGroup.Update(item, null);
        }

        foreach (var item in serviceArea.GetAllNewVersion(p => p.Company._id == company._id).Result.ToList())
        {
          item.Company = company.GetViewList();
          serviceArea.Update(item, null);
        }


      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateGroupAll(Group group)
    {
      try
      {
        var notschooling = true;
        List<List<dynamic>> listremove = new List<List<dynamic>>();
        List<List<dynamic>> listadd = new List<List<dynamic>>();

        foreach (var item in serviceOccupation.GetAllNewVersion(p => p.Group._id == group._id).Result.ToList())
        {
          item.Group = group.GetViewList();
          foreach (var school in group.Schooling)
          {
            foreach (var schoolOccupation in item.Schooling)
            {
              if (school._id == schoolOccupation._id)
              {
                school.Name = schoolOccupation.Name;
                school.Order = schoolOccupation.Order;
              }

            }

            if (item.Schooling.Where(p => p._id == school._id).Count() == 0)
            {
              notschooling = false;
              List<dynamic> view = new List<dynamic>();
              view.Add(item._id);
              view.Add(school.GetViewCrud());
              listadd.Add(view);
            }
          }

          foreach (var schoolOccupation in item.Schooling)
          {
            if (group.Schooling.Where(p => p._id == schoolOccupation._id).Count() == 0)
            {
              //item.Schooling.Remove(schoolOccupation);
              notschooling = false;
              List<dynamic> view = new List<dynamic>
              {
                item._id,
                schoolOccupation
              };
              listremove.Add(view);
            }

          }

          serviceOccupation.Update(item, null);
          if (notschooling)
          {
            Task.Run(() => UpdateOccupationAll(item));
            Task.Run(() => UpdateOccupationLog(item));
          }

        }

        foreach (var item in serviceOccupation.GetAllNewVersion(p => p.Group._id == group._id).Result.ToList())
        {
          foreach (var remove in listremove)
          {
            if (remove[0] == item._id)
            {
              var school = item.Schooling.Where(p => p._id == remove[1]._id).FirstOrDefault();
              if (school != null)
                item.Schooling.Remove(school);

              serviceOccupation.Update(item, null);
            }
          }
          foreach (var add in listadd)
          {
            if (add[0] == item._id)
            {
              item.Schooling.Add(add[1]);
              serviceOccupation.Update(item, null);
            }
          }

        }


        foreach (var item in servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator & p.Occupation._idGroup == group._id).Result.ToList())
        {
          item.Occupation._idGroup = group._id;
          item.Occupation.NameGroup = group.Name;
          servicePerson.Update(item, null);
        }


      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateCboAll(Cbo Cbo)
    {
      try
      {
        foreach (var item in serviceOccupation.GetAllFreeNewVersion(p => p.Cbo._id == Cbo._id).Result)
        {
          item.Cbo = Cbo.GetViewList();
          serviceOccupation.UpdateAccount(item, null);
          Task.Run(() => UpdateOccupationAll(item));
          Task.Run(() => UpdateOccupationLog(item));
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateOccupationAllCbo(Occupation occupation)
    {
      try
      {
        foreach (var item in servicePerson.GetAllFreeNewVersion(p => p.Occupation._id == occupation._id).Result.ToList())
        {
          item.Occupation = occupation.GetViewListResume();
          servicePerson.UpdateAccount(item, null);
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateOccupationAll(Occupation occupation)
    {
      try
      {
        foreach (var item in servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator & p.Occupation._id == occupation._id).Result)
        {
          item.Occupation = occupation.GetViewListResume();
          servicePerson.Update(item, null);
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateGroupOccupationAll(Group groupnew, Group groupold)
    {
      try
      {
        foreach (var item in serviceOccupation.GetAllNewVersion(p => p.Group._id == groupold._id).Result.ToList())
        {
          item.Group = groupnew.GetViewList();
          serviceOccupation.Update(item, null);
          Task.Run(() => UpdateOccupationAll(item));
          Task.Run(() => UpdateOccupationLog(item));
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateSphereAll(Sphere sphere, bool remove)
    {
      //foreach (var item in axisService.GetAllNewVersion(p => p.Sphere._id == sphere._id).ToList())
      //{
      //  if (remove == true)
      //    item.Sphere = null;
      //  else
      //    item.Sphere = sphere;

      //  this.axisService.Update(item, null);
      //}

      foreach (var item in serviceGroup.GetAllNewVersion(p => p.Sphere._id == sphere._id).Result.ToList())
      {
        if (remove == true)
          item.Sphere = null;
        else
          item.Sphere = sphere.GetViewList();

        this.serviceGroup.Update(item, null);
        Task.Run(() => UpdateGroupAll(item));
        Task.Run(() => UpdateGroupLog(item));
      }

    }

    private async Task UpdateAxisAll(Axis axis, bool remove)
    {
      foreach (var item in serviceGroup.GetAllNewVersion(p => p.Axis._id == axis._id).Result.ToList())
      {
        if (remove == true)
          item.Axis = null;
        else
          item.Axis = axis.GetViewList();

        this.serviceGroup.Update(item, null);
        Task.Run(() => UpdateGroupAll(item));
        Task.Run(() => UpdateGroupLog(item));
      }

    }

    private async Task UpdateAreaAll(Area area)
    {
      try
      {
        //foreach (var item in occupationService.GetAllNewVersion(p => p.Area._id == area._id).ToList())
        //{
        //  item.Area = area;
        //  this.occupationService.Update(item, null);
        //  UpdateOccupationAll(item);
        //}

        foreach (var item in serviceProcessLevelOne.GetAllNewVersion().ToList())
        {
          if (item.Area._id == area._id)
          {
            item.Area.Name = area.Name;
            serviceProcessLevelOne.Update(item, null);
          }
        }

        foreach (var item in serviceProcessLevelTwo.GetAllNewVersion().ToList())
        {
          if (item.ProcessLevelOne.Area._id == area._id)
          {
            item.ProcessLevelOne.Area.Name = area.Name;
            serviceProcessLevelTwo.Update(item, null);
          }
        }


        //foreach (var item in occupationService.GetAllNewVersion().ToList())
        //{
        //  foreach (var ar in item.Areas)
        //  {
        //    if (ar != null)
        //    {
        //      if (ar._id == area._id)
        //      {
        //        item.Areas.Remove(ar);
        //        item.Areas.Add(area);
        //        item.ProcessLevelTwo.ProcessLevelOne.Area.Name = area.Name;
        //        item.Area.Name = area.Name;
        //        this.occupationService.Update(item, null);
        //        UpdateOccupationAll(item);
        //        break;
        //      }
        //    }
        //  }
        //}



      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateAreaProcessAll(Area area)
    {
      try
      {
        foreach (var item in serviceOccupation.GetAllNewVersion().ToList())
        {
          foreach (var proc in item.Process)
          {
            if (proc.ProcessLevelOne != null)
            {
              if (proc.ProcessLevelOne.Area != null)
              {
                if (proc.ProcessLevelOne.Area._id == area._id)
                {
                  proc.ProcessLevelOne.Area = area.GetViewList();
                  this.serviceOccupation.Update(item, null);
                  Task.Run(() => UpdateOccupationAll(item));
                  Task.Run(() => UpdateOccupationLog(item));
                }
              }
            }

          }
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateProcessLevelTwoAll(string id)
    {
      try
      {
        var processLevelTwo = serviceProcessLevelTwo.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();

        var list = serviceOccupation.GetAllNewVersion().ToList();

        foreach (var item in list)
        {
          if (item.Process != null)
          {
            var process = item.Process.Where(p => p._id == id).ToList();
            foreach (var proc in process)
            {
              if (proc._id == processLevelTwo._id)
              {
                item.Process.Remove(proc);
                item.Process.Add(processLevelTwo.GetViewList());
                this.serviceOccupation.Update(item, null);
                Task.Run(() => UpdateOccupationAll(item));
                Task.Run(() => UpdateOccupationLog(item));
                break;
              }
            }
          }
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateSchoolingAll(Schooling schooling)
    {
      try
      {
        foreach (var item in serviceGroup.GetAllNewVersion().ToList())
        {
          foreach (var row in item.Schooling)
          {
            if (row._id == schooling._id)
            {
              row.Name = schooling.Name;
              row.Order = schooling.Order;
            }
          }
          this.serviceGroup.Update(item, null);
          Task.Run(() => UpdateGroupAll(item));
          Task.Run(() => UpdateGroupLog(item));
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

  }
#pragma warning restore 1998
#pragma warning restore 4014
}
