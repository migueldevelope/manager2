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
    private readonly ServiceGeneric<CompanyMandatory> serviceCompanyMandatory;
    private readonly ServiceGeneric<DictionarySphere> serviceDictionarySphere;
    private readonly ServiceGeneric<Group> serviceGroup;
    private readonly ServiceGeneric<MailModel> serviceMailModel;
    private readonly ServiceGeneric<Occupation> serviceOccupation;
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
        serviceCompanyMandatory = new ServiceGeneric<CompanyMandatory>(context);
        serviceDictionarySphere = new ServiceGeneric<DictionarySphere>(context);
        serviceGroup = new ServiceGeneric<Group>(context);
        serviceMailModel = new ServiceGeneric<MailModel>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
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
      serviceCompanyMandatory._user = _user;
      serviceDictionarySphere._user = _user;
      serviceGroup._user = _user;
      serviceMailModel._user = _user;
      serviceOccupation._user = _user;
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
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceAccount._user = user;
      serviceArea._user = user;
      serviceAxis._user = user;
      serviceCbo._user = user;
      serviceCompany._user = user;
      serviceCompanyMandatory._user = user;
      serviceDictionarySphere._user = user;
      serviceGroup._user = user;
      serviceMailModel._user = user;
      serviceOccupation._user = user;
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
          Parameter result = serviceParameter.InsertFreeNewVersion(parameterLocal).Result;
        }

        TextDefault textDefaultLocal;
        // Text default
        foreach (TextDefault textDefault in serviceTextDefault.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          textDefaultLocal = new TextDefault()
          {
            Template = new TextDefault()
            {
              _id = textDefault._id,
              _idAccount = textDefault._idAccount,
              Company = textDefault.Company,
              Name = textDefault.Name,
              TypeText = textDefault.TypeText,
              Content = textDefault.Content,
              Status = textDefault.Status,
              Template = null
            },
            Company = company,
            _idAccount = _user._idAccount,
            Content = textDefault.Content,
            Name = textDefault.Name,
            Status = EnumStatus.Enabled,
            TypeText = textDefault.TypeText,
            _id = ObjectId.GenerateNewId().ToString()
          };
          TextDefault result = serviceTextDefault.InsertFreeNewVersion(textDefaultLocal).Result;
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
          MailModel result = serviceMailModel.InsertFreeNewVersion(mailModelLocal).Result;
        }

        // Questions
        Questions questionsLocal;
        foreach (Questions question in serviceQuestions.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          questionsLocal = new Questions()
          {
            Template = new Questions()
            {
              Content = question.Content,
              Company = question.Company,
              Name = question.Name,
              Order = question.Order,
              Status = question.Status,
              TypeQuestion = question.TypeQuestion,
              TypeRotine = question.TypeRotine,
              _idAccount = question._idAccount,
              _id = question._id,
              Template = null
            },
            Content = question.Content,
            Company = company,
            Name = question.Name,
            Order = question.Order,
            Status = question.Status,
            TypeQuestion = question.TypeQuestion,
            TypeRotine = question.TypeRotine,
            _idAccount = _user._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          };
          Questions result = serviceQuestions.InsertFreeNewVersion(questionsLocal).Result;
        }

        // Skill
        Skill skillLocal;
        foreach (Skill item in serviceSkill.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          skillLocal = new Skill()
          {
            Template = new Skill()
            {
              Concept = item.Concept,
              Name = item.Name,
              Status = item.Status,
              TypeSkill = item.TypeSkill,
              _id = item._id,
              _idAccount = item._idAccount,
              Template = null
            },
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
            Template = new Schooling()
            {
              Complement = item.Complement,
              Name = item.Name,
              Order = item.Order,
              Status = item.Status,
              Type = item.Type,
              _id = item._id,
              _idAccount = item._idAccount,
              Template = null
            },
            Complement = item.Complement,
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
            Template = new Sphere()
            {
              Company = item.Company,
              Name = item.Name,
              Status = item.Status,
              TypeSphere = item.TypeSphere,
              _id = item._id,
              _idAccount = item._idAccount,
              Template = null
            },
            Company = company,
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
            Template = new Axis()
            {
              Name = item.Name,
              Company = item.Company,
              Status = item.Status,
              TypeAxis = item.TypeAxis,
              _id = item._id,
              _idAccount = item._idAccount,
              Template = null
            },
            Company = company,
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
            Template = new Group()
            {
              Name = item.Name,
              Sphere = item.Sphere,
              Status = item.Status,
              _id = item._id,
              _idAccount = item._idAccount,
              Axis = item.Axis,
              Company = item.Company,
              Line = item.Line,
              Schooling = item.Schooling,
              Skills = item.Skills,
              Scope = item.Scope,
              Template = null
            },
            Company = company,
            Name = item.Name,
            Status = item.Status,
            _id = item._id,
            _idAccount = _user._idAccount,
            Line = item.Line,
            Schooling = new List<Schooling>(),
            Skills = new List<Skill>(),
            Scope = new List<Scope>()
          };

          if (item.Schooling != null)
            foreach (Schooling schooling in item.Schooling)
              groupLocal.Schooling.Add(serviceSchooling.GetFreeNewVersion(p => p._idAccount == _user._idAccount && p.Template._id == schooling._id).Result);

          if (item.Skills != null)
            foreach (Skill skill in item.Skills)
              groupLocal.Skills.Add(serviceSkill.GetFreeNewVersion(p => p._idAccount == _user._idAccount && p.Template._id == skill._id).Result);

          if (item.Scope != null)
            foreach (Scope scope in item.Scope)
            {
              scope._idAccount = _user._idAccount;
              scope._id = ObjectId.GenerateNewId().ToString();
              groupLocal.Scope.Add(scope);
            };


          groupLocal.Sphere = serviceSphere.GetFreeNewVersion(p => p._idAccount == _user._idAccount && p.Template._id == item.Sphere._id).Result;
          groupLocal.Axis = serviceAxis.GetFreeNewVersion(p => p._idAccount == _user._idAccount && p.Template._id == item.Axis._id).Result;

          groupLocal._idAccount = _user._idAccount;
          groupLocal._id = ObjectId.GenerateNewId().ToString();
          Group result = serviceGroup.InsertFreeNewVersion(groupLocal).Result;
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
            company = serviceCompany.GetAllFreeNewVersion(p => p._idAccount == accountTextDefault._id).Result.FirstOrDefault();
            local = serviceTextDefault.GetFreeNewVersion(p => p._idAccount == accountTextDefault._id && p.Template._id == textDefault._id).Result;
            if (local == null)
            {
              local = new TextDefault()
              {
                Template = new TextDefault()
                {
                  _id = textDefault._id,
                  _idAccount = textDefault._idAccount,
                  Company = textDefault.Company,
                  Name = textDefault.Name,
                  TypeText = textDefault.TypeText,
                  Content = textDefault.Content,
                  Status = textDefault.Status,
                  Template = null
                },
                Company = company,
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
            company = serviceCompany.GetAllFreeNewVersion(p => p._idAccount == accountQuestion._id).Result.FirstOrDefault();
            local = serviceQuestions.GetFreeNewVersion(p => p._idAccount == accountQuestion._idAccount && p.Template._id == question._id).Result;
            if (local == null)
            {
              local = new Questions()
              {
                Template = new Questions()
                {
                  Content = question.Content,
                  Company = question.Company,
                  Name = question.Name,
                  Order = question.Order,
                  Status = question.Status,
                  TypeQuestion = question.TypeQuestion,
                  TypeRotine = question.TypeRotine,
                  _idAccount = question._idAccount,
                  _id = question._id,
                  Template = null
                },
                Content = question.Content,
                Company = company,
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
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Infra
    public List<ViewListSkill> GetEssential(string idcompany)
    {
      try
      {
        return serviceCompany.GetAll(p => p._id == idcompany).FirstOrDefault()
          .Skills.Select(p => new ViewListSkill()
          {
            _id = p._id,
            Name = p.Name,
            Concept = p.Concept,
            TypeSkill = p.TypeSkill
          }).ToList();
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
        if (serviceProcessLevelTwo.GetAll(p => p.ProcessLevelOne._id == id).Count() > 0)
          return "error_leveltwo_exists";

        var item = serviceProcessLevelOne.GetAll(p => p._id == id).FirstOrDefault();
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
        //if (occupationService.GetAll(p => p.ProcessLevelTwo._id == id).Count() > 0)
        //return "error_occupation_exists";
        var item = serviceProcessLevelTwo.GetAll(p => p._id == id).FirstOrDefault();

        if (serviceOccupation.GetAll(p => p.Process.Contains(item)).Count() > 0)
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
        var group = serviceGroup.GetAll(p => p.Company._id == idcompany & p._id == idgroup).FirstOrDefault();
        var scope = group.Scope.Where(p => p._id == idscope).FirstOrDefault();
        Scope scopeOld;
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
        UpdateGroupAll(group);
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
        var occupation = serviceOccupation.GetAll(p => p.Group.Company._id == idcompany & p._id == idoccupation).FirstOrDefault();
        var activities = occupation.Activities.Where(p => p._id == idactivitie).FirstOrDefault();
        Activitie activitiesOld;
        if (sum)
        {
          var min = occupation.Activities.Where(p => p.Order > activities.Order).Min(p => p.Order);
          activitiesOld = occupation.Activities.Where(p => p.Order == min).FirstOrDefault();
        }
        else
        {
          var max = occupation.Activities.Where(p => p.Order < activities.Order).Max(p => p.Order);
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

        UpdateOccupationAll(occupation);
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
        var group = serviceGroup.GetAll(p => p.Company._id == idcompany & p._id == idgroup).FirstOrDefault();
        var scope = group.Scope.Where(p => p._id == idscope).FirstOrDefault();

        foreach (var item in group.Scope)
        {
          if (item._id == scope._id)
            item.Order = order;
        }

        serviceGroup.Update(group, null);
        UpdateGroupAll(group);
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
        var occupation = serviceOccupation.GetAll(p => p.Group.Company._id == idcompany & p._id == idoccupation).FirstOrDefault();
        var activities = occupation.Activities.Where(p => p._id == idactivitie).FirstOrDefault();

        foreach (var item in occupation.Activities)
        {
          if (item._id == activities._id)
            item.Order = order;
        }
        serviceOccupation.Update(occupation, null);

        UpdateOccupationAll(occupation);
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
        var textDefault = serviceTextDefault.GetAll(p => p._id == id).FirstOrDefault();
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
        var area = serviceArea.GetAll(p => p._id == idarea).FirstOrDefault();
        var areas = serviceArea.GetAll(p => p.Company._id == idcompany).ToList();

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
        var groups = serviceGroup.GetAll(p => p.Company._id == idcompany).OrderBy(p => p.Sphere.TypeSphere).ThenBy(p => p.Axis.TypeAxis).ThenBy(p => p.Line).ToList();

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
              Name = skill.Name.Replace("\n", "").Replace(";", ".") + ":" + skill.Concept.Replace("\n", "").Replace(";", "."),
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


        for (var row = 0; row < maxLineSchooling; row++)
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

        for (var row = 0; row < maxLineSchooling; row++)
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


        var person = servicePerson.GetAll(p => p.User.Mail == _user.Mail).FirstOrDefault();

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

    private async Task UpdateCompanyAll(Company company)
    {
      try
      {
        foreach (var item in servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Company._id == company._id).ToList())
        {
          item.Company = company;
          servicePerson.Update(item, null);
        }

        foreach (var item in serviceSphere.GetAll(p => p.Company._id == company._id).ToList())
        {
          item.Company = company;
          serviceSphere.Update(item, null);
        }

        foreach (var item in serviceAxis.GetAll(p => p.Company._id == company._id).ToList())
        {
          item.Company = company;
          serviceAxis.Update(item, null);
        }

        foreach (var item in serviceOccupation.GetAll(p => p.Group.Company._id == company._id).ToList())
        {
          item.Group.Company = company;
          serviceOccupation.Update(item, null);
        }

        foreach (var item in serviceGroup.GetAll(p => p.Company._id == company._id).ToList())
        {
          item.Company = company;
          serviceGroup.Update(item, null);
        }

        foreach (var item in serviceArea.GetAll(p => p.Company._id == company._id).ToList())
        {
          item.Company = company;
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
        foreach (var item in serviceOccupation.GetAll(p => p.Group._id == group._id).ToList())
        {
          item.Group = new Group()
          {
            Name = group.Name,
            Company = group.Company,
            Axis = group.Axis,
            Sphere = group.Sphere,
            Line = group.Line,
            Skills = group.Skills,
            Schooling = group.Schooling,
            Scope = group.Scope,
            Template = group.Template,
            _id = group._id,
            _idAccount = group._idAccount,
            Status = group.Status
          };
          foreach (var school in group.Schooling)
          {
            foreach (var schoolOccupation in item.Schooling)
            {
              if (school._id == schoolOccupation._id)
                school.Complement = schoolOccupation.Complement;
            }
          }
          item.Schooling = group.Schooling;
          serviceOccupation.Update(item, null);
          UpdateOccupationAll(item);
        }


        foreach (var item in servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Occupation.Group._id == group._id).ToList())
        {
          item.Occupation.Group = group;
          foreach (var school in group.Schooling)
          {
            foreach (var schoolOccupation in item.Occupation.Schooling)
            {
              if (school._id == schoolOccupation._id)
                school.Complement = schoolOccupation.Complement;
            }
          }
          item.Occupation.Schooling = group.Schooling;
          servicePerson.Update(item, null);
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateCBOAll(Cbo cbo)
    {
      try
      {
        foreach (var item in serviceOccupation.GetAuthentication(p => p.CBO._id == cbo._id).ToList())
        {
          item.CBO = cbo;
          serviceOccupation.UpdateAccount(item, null);
          UpdateOccupationAllCBO(item);
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateOccupationAllCBO(Occupation occupation)
    {
      try
      {
        foreach (var item in servicePerson.GetAuthentication(p => p.Occupation._id == occupation._id).ToList())
        {
          item.Occupation = occupation;
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
        //foreach (var item in personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Occupation._id == occupation._id).ToList())
        //{
        //  item.Occupation = occupation;
        //  personService.Update(item, null);
        //}

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
        foreach (var item in serviceOccupation.GetAll(p => p.Group._id == groupold._id).ToList())
        {
          item.Group = groupnew;
          serviceOccupation.Update(item, null);
          UpdateOccupationAll(item);
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateSphereAll(Sphere sphere, bool remove)
    {
      //foreach (var item in axisService.GetAll(p => p.Sphere._id == sphere._id).ToList())
      //{
      //  if (remove == true)
      //    item.Sphere = null;
      //  else
      //    item.Sphere = sphere;

      //  this.axisService.Update(item, null);
      //}

      foreach (var item in serviceGroup.GetAll(p => p.Sphere._id == sphere._id).ToList())
      {
        if (remove == true)
          item.Sphere = null;
        else
          item.Sphere = sphere;

        this.serviceGroup.Update(item, null);
        UpdateGroupAll(item);
      }

    }

    private async Task UpdateAxisAll(Axis axis, bool remove)
    {
      foreach (var item in serviceGroup.GetAll(p => p.Axis._id == axis._id).ToList())
      {
        if (remove == true)
          item.Axis = null;
        else
          item.Axis = axis;

        this.serviceGroup.Update(item, null);
        UpdateGroupAll(item);
      }

    }

    private async Task UpdateAreaAll(Area area)
    {
      try
      {
        //foreach (var item in occupationService.GetAll(p => p.Area._id == area._id).ToList())
        //{
        //  item.Area = area;
        //  this.occupationService.Update(item, null);
        //  UpdateOccupationAll(item);
        //}

        foreach (var item in serviceProcessLevelOne.GetAll().ToList())
        {
          if (item.Area._id == area._id)
          {
            item.Area.Name = area.Name;
            serviceProcessLevelOne.Update(item, null);
          }
        }

        foreach (var item in serviceProcessLevelTwo.GetAll().ToList())
        {
          if (item.ProcessLevelOne.Area._id == area._id)
          {
            item.ProcessLevelOne.Area.Name = area.Name;
            serviceProcessLevelTwo.Update(item, null);
          }
        }


        //foreach (var item in occupationService.GetAll().ToList())
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
        foreach (var item in serviceOccupation.GetAll().ToList())
        {
          foreach (var proc in item.Process)
          {
            if (proc.ProcessLevelOne != null)
            {
              if (proc.ProcessLevelOne.Area != null)
              {
                if (proc.ProcessLevelOne.Area._id == area._id)
                {
                  proc.ProcessLevelOne.Area = area;
                  this.serviceOccupation.Update(item, null);
                  UpdateOccupationAll(item);
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
        var processLevelTwo = serviceProcessLevelTwo.GetAll(p => p._id == id).FirstOrDefault();

        var list = serviceOccupation.GetAll().ToList();

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
                item.Process.Add(processLevelTwo);
                this.serviceOccupation.Update(item, null);
                UpdateOccupationAll(item);
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
        foreach (var item in serviceGroup.GetAll().ToList())
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
          UpdateGroupAll(item);
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListCbo> ListCBO()
    {
      try
      {
        var cbo = serviceCbo.GetAuthentication(p => p.Status == EnumStatus.Enabled)
          .Select(p => new ViewListCbo()
          {
            _id = p._id,
            Code = p.Code,
            Name = p.Name
          })
          .ToList();
        return cbo;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudCbo GetCBO(string id)
    {
      try
      {
        var cbo = serviceCbo.GetAuthentication(p => p._id == id)
          .Select(p => new ViewCrudCbo()
          {
            _id = p._id,
            Name = p.Name,
            Code = p.Code
          })
          .FirstOrDefault();
        return cbo;
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
        return serviceSphere.GetAll().OrderBy(p => p.TypeSphere)
          .Select(p => new ViewListSphere()
          {
            _id = p._id,
            Name = p.Name,
            TypeSphere = p.TypeSphere
          }).ToList();
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
        return serviceSphere.GetAll(p => p.Company._id == idcompany).OrderBy(p => p.TypeSphere)
           .Select(p => new ViewListSphere()
           {
             _id = p._id,
             Name = p.Name,
             TypeSphere = p.TypeSphere
           }).ToList();
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
        var areas = serviceArea.GetAll().OrderBy(p => p.Name).ToList();
        foreach (var item in areas)
        {
          item.ProcessLevelOnes = new List<ProcessLevelOne>();
          var process = serviceProcessLevelOne.GetAll(p => p.Area._id == item._id).OrderBy(p => p.Order).ToList();
          foreach (var row in process)
          {
            row.Process = new List<ProcessLevelTwo>();
            foreach (var leveltwo in serviceProcessLevelTwo.GetAll(p => p.ProcessLevelOne._id == row._id).OrderBy(p => p.Order).ToList())
            {
              row.Process.Add(leveltwo);
            }

            item.ProcessLevelOnes.Add(row);
          }
        }
        return areas.OrderBy(p => p.Name)
           .Select(p => new ViewListArea()
           {
             _id = p._id,
             Name = p.Name
           }).ToList();
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
        var item = serviceSchooling.GetAll(p => p._id == id).OrderBy(p => p.Name).FirstOrDefault();
        return new ViewCrudSchooling()
        {
          _id = item._id,
          Name = item.Name,
          Complement = item.Complement,
          Order = item.Order,
          Type = item.Type
        };
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
        var item = serviceAxis.GetAll(p => p._id == id).OrderBy(p => p.Name).FirstOrDefault();
        return new ViewCrudAxis()
        {
          _id = item._id,
          Name = item.Name,
          Company = new ViewListCompany() { _id = item._id, Name = item.Name },
          TypeAxis = item.TypeAxis
        };
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
        var item = serviceSphere.GetAll(p => p._id == id).OrderBy(p => p.Name).FirstOrDefault();
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
        var item = serviceSkill.GetAll(p => p._id == id).OrderBy(p => p.Name).FirstOrDefault();
        return new ViewCrudSkill()
        {
          _id = item._id,
          Name = item.Name,
          Concept = item.Concept,
          TypeSkill = item.TypeSkill
        };
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
        var areas = serviceArea.GetAll(p => p.Company._id == idcompany).OrderBy(p => p.Name).ToList();
        foreach (var item in areas)
        {
          item.ProcessLevelOnes = new List<ProcessLevelOne>();
          var process = serviceProcessLevelOne.GetAll(p => p.Area._id == item._id).OrderBy(p => p.Order).ToList();
          foreach (var row in process)
          {
            row.Process = new List<ProcessLevelTwo>();
            foreach (var leveltwo in serviceProcessLevelTwo.GetAll(p => p.ProcessLevelOne._id == row._id).OrderBy(p => p.Order).ToList())
            {
              row.Process.Add(leveltwo);
            }

            item.ProcessLevelOnes.Add(row);
          }
        }
        return areas.OrderBy(p => p.Name)
           .Select(p => new ViewListArea()
           {
             _id = p._id,
             Name = p.Name
           }).ToList();
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
        return serviceAxis.GetAll().OrderBy(p => p.TypeAxis)
          .Select(p => new ViewListAxis()
          {
            _id = p._id,
            Name = p.Name,
            TypeAxis = p.TypeAxis
          }).ToList();
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
        return serviceAxis.GetAll(p => p.Company._id == idcompany).OrderBy(p => p.TypeAxis)
          .Select(p => new ViewListAxis()
          {
            _id = p._id,
            Name = p.Name,
            TypeAxis = p.TypeAxis
          }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListQuestions> ListQuestions(string idcompany)
    {
      try
      {
        return serviceQuestions.GetAll(p => p.Company._id == idcompany).OrderBy(p => p.Order)
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
        return serviceQuestions.GetAll(p => p._id == id).Select(p => new ViewCrudQuestions()
        {
          _id = p._id,
          Name = p.Name,
          Content = p.Content,
          Company = new ViewListCompany() { _id = p.Company._id, Name = p.Company.Name },
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
        return serviceSchooling.GetAll().OrderBy(p => p.Order)
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

    private Group AddGroupInternal(ViewAddGroup view)
    {
      try
      {
        var group = AddGroup(view);
        return serviceGroup.GetAll(p => p._id == group._id).FirstOrDefault();
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
          line = serviceGroup.GetAll(p => p.Sphere._id == view.Sphere._id & p.Axis._id == view.Axis._id).Max(p => p.Line);
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
          Skills = new List<Skill>(),
          Schooling = new List<Schooling>(),
          Line = line,
          Company = view.Company,
          Scope = new List<Scope>()
        };
        var result = serviceGroup.InsertNewVersion(group).Result;

        return new ViewCrudGroup()
        {
          _id = result._id,
          Name = result.Name,
          Line = result.Line,
          Company = new ViewListCompany() { _id = result.Company._id, Name = result.Company.Name },
          Axis = new ViewListAxis() { _id = result.Axis._id, Name = result.Axis.Name },
          Sphere = new ViewListSphere() { _id = result.Sphere._id, Name = result.Sphere.Name }
        };
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
        var company = serviceCompany.GetAll(p => p._id == view._idCompany).FirstOrDefault();

        Skill skill = serviceSkill.GetAll(p => p._id == view.Skill._id).FirstOrDefault();

        if (view.Skill._id == null)
        {
          skill = AddSkillInternal(new ViewCrudSkill()
          {
            Name = view.Skill.Name,
            Concept = view.Skill.Concept,
            TypeSkill = view.Skill.TypeSkill
          });
        }

        company.Skills.Add(skill);
        serviceCompany.Update(company, null);

        UpdateCompanyAll(company);
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
          line = serviceGroup.GetAll(p => p.Sphere._id == view.Sphere._id & p.Axis._id == view.Axis._id).Max(p => p.Line);
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
          Axis = serviceAxis.GetAll(p => p._id == view.Axis._id).FirstOrDefault(),
          Sphere = serviceSphere.GetAll(p => p._id == view.Sphere._id).FirstOrDefault(),
          Status = EnumStatus.Enabled,
          Skills = new List<Skill>(),
          Schooling = new List<Schooling>(),
          Line = line,
          Company = serviceCompany.GetAll(p => p._id == view.Company._id).FirstOrDefault(),
          Scope = new List<Scope>()
        };
        var result = serviceGroup.InsertNewVersion(group).Result;

        return new ViewCrudGroup()
        {
          _id = result._id,
          Name = result.Name,
          Line = result.Line,
          Company = new ViewListCompany() { _id = result.Company._id, Name = result.Company.Name },
          Axis = new ViewListAxis() { _id = result.Axis._id, Name = result.Axis.Name },
          Sphere = new ViewListSphere() { _id = result.Sphere._id, Name = result.Sphere.Name }
        };
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

        var group = serviceGroup.GetAll(p => p._id == view._idGroup).FirstOrDefault();
        var schooling = serviceSchooling.GetAll(p => p._id == view.Schooling._id).FirstOrDefault();

        schooling.Complement = view.Schooling.Complement;
        schooling.Type = view.Schooling.Type;

        //if (view.Group.Schooling.Where(p => p.Type == view.Schooling.Type).Count() > 0)
        //  return "error_exists_schooling";

        //view.Group.Schooling.Add(AddSchooling(view.Schooling));
        group.Schooling.Add(schooling);
        serviceGroup.Update(group, null);
        UpdateGroupAll(group);
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
        var group = serviceGroup.GetAll(p => p._id == view._idGroup).FirstOrDefault();

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
          _idAccount = _user._idAccount,
          Order = order,
          Name = view.Scope.Name,
          Status = EnumStatus.Enabled
        };

        if (scope._id == null)
          scope._id = ObjectId.GenerateNewId().ToString();

        group.Scope.Add(scope);
        serviceGroup.Update(group, null);
        UpdateGroupAll(group);
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
        var group = serviceGroup.GetAll(p => p._id == view._idGroup).FirstOrDefault();
        Skill skill = serviceSkill.GetAll(p => p._id == view.Skill._id).FirstOrDefault();

        if (view.Skill._id == null)
        {
          skill = AddSkillInternal(new ViewCrudSkill()
          {
            Name = view.Skill.Name,
            Concept = view.Skill.Concept,
            TypeSkill = view.Skill.TypeSkill
          });
        }

        group.Skills.Add(skill);
        serviceGroup.Update(group, null);
        UpdateGroupAll(group);
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
        var occupation = serviceOccupation.GetAll(p => p._id == view.idOccupation).FirstOrDefault();
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
          _idAccount = _user._idAccount,
          Name = view.Activities.Name,
          Status = EnumStatus.Enabled
        };


        occupation.Activities.Add(activitie);
        serviceOccupation.Update(occupation, null);
        UpdateOccupationAll(occupation);
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

        var occupation = serviceOccupation.GetAll(p => p._id == list.FirstOrDefault().idOccupation).FirstOrDefault();
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
              _idAccount = _user._idAccount,
              Name = view.Activities.Name,
              Status = EnumStatus.Enabled
            };

            occupation.Activities.Add(activitie);
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
        var occupation = serviceOccupation.GetAll(p => p._id == idoccupation).FirstOrDefault();
        occupation.SpecificRequirements = view.Name;
        serviceOccupation.Update(occupation, null);
        UpdateOccupationAll(occupation);

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
        var occupation = serviceOccupation.GetAll(p => p._id == view._idOccupation).FirstOrDefault();
        var skill = serviceSkill.GetAll(p => p._id == view.Skill._id).FirstOrDefault();

        if (skill == null)
        {
          skill = AddSkillInternal(new ViewCrudSkill()
          {
            Name = view.Skill.Name,
            Concept = view.Skill.Concept,
            TypeSkill = view.Skill.TypeSkill
          });
        }

        occupation.Skills.Add(skill);
        serviceOccupation.Update(occupation, null);
        UpdateOccupationAll(occupation);
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
          Name = view.Name,
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

    private Skill AddSkillInternal(ViewCrudSkill view)
    {
      try
      {
        var skill = new Skill()
        {
          Name = view.Name,
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


    public string AddArea(ViewCrudArea view)
    {
      try
      {
        //var item = areaService.GetAll(p => p.Order == view.Order).Count();
        //if (item > 0)
        //  return "error_line";


        serviceArea.InsertNewVersion(new Area()
        {
          Name = view.Name,
          Order = view.Order,
          Status = EnumStatus.Enabled,
          Company = serviceCompany.GetAll(p => p._id == view.Company._id).FirstOrDefault(),
          ProcessLevelOnes = new List<ProcessLevelOne>()
        });
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddCBO(ViewCrudCbo view)
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
          Company = serviceCompany.GetAll(p => p._id == view.Company._id).FirstOrDefault()
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
        var group = serviceGroup.GetAll(p => p._id == view.Group._id).FirstOrDefault();

        var occupation = new Occupation()
        {
          Name = view.Name,
          Line = view.Line,
          Status = EnumStatus.Enabled,
          Group = group,
          Skills = new List<Skill>(),
          CBO = (view.Cbo == null) ? null : serviceCbo.GetAll(p => p._id == view.Cbo._id).FirstOrDefault(),
          Activities = new List<Activitie>(),
          SalaryScales = new List<SalaryScaleGrade>(),
          Schooling = new List<Schooling>(),
          Process = new List<ProcessLevelTwo>()
        };

        foreach (var item in view.Process)
          occupation.Process.Add(serviceProcessLevelTwo.GetAll(p => p._id == item._id).FirstOrDefault());

        if (view.SalaryScales != null)
          foreach (var item in view.SalaryScales)
            occupation.SalaryScales.Add(new SalaryScaleGrade()
            {
              _id = ObjectId.GenerateNewId().ToString(),
              _idAccount = _user._idAccount,
              Status = EnumStatus.Enabled,
              NameGrade = item.NameGrade,
              NameSalaryScale = item.Name,
              _idGrade = item._idGrade,
              _idSalaryScale = item._id
            });

        if (group.Schooling != null)
          occupation.Schooling = group.Schooling;

        serviceOccupation.InsertNewVersion(occupation);
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
          Complement = schooling.Complement,
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
          Company = serviceCompany.GetAll(p => p._id == view.Company._id).FirstOrDefault(),
          Status = EnumStatus.Enabled
        });
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddQuestions(ViewCrudQuestions view)
    {
      try
      {
        serviceQuestions.InsertNewVersion(new Questions()
        {
          Name = view.Name,
          Order = view.Order,
          TypeQuestion = view.TypeQuestion,
          TypeRotine = view.TypeRotine,
          Status = EnumStatus.Enabled,
          Company = serviceCompany.GetAll(p => p._id == view.Company._id).FirstOrDefault(),
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
          var order = serviceProcessLevelOne.GetAll(p => p.Area._id == model.Area._id).Max(p => p.Order) + 1;
          model.Order = order;
        }
        catch (Exception)
        {
          model.Order = 1;
        }


        serviceProcessLevelOne.InsertNewVersion(new ProcessLevelOne()
        {
          Name = model.Name,
          Area = serviceArea.GetAll(p => p._id == model.Area._id).FirstOrDefault(),
          Process = new List<ProcessLevelTwo>(),
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
          Company = serviceCompany.GetAll(p => p._id == model.Company._id).FirstOrDefault(),
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
          var order = serviceProcessLevelTwo.GetAll(p => p.ProcessLevelOne._id == model.ProcessLevelOne._id).Max(p => p.Order) + 1;
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
          ProcessLevelOne = serviceProcessLevelOne.GetAll(p => p._id == model.ProcessLevelOne._id).FirstOrDefault(),
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
        if (serviceProcessLevelOne.GetAll(p => p.Area._id == idarea).Count() > 0)
          return "erro_exists_nivelone";

        var area = serviceArea.GetAll(p => p._id == idarea).FirstOrDefault();


        foreach (var item in serviceOccupation.GetAll(p => p.Process.Exists(i => i.ProcessLevelOne.Area._id == area._id)).ToList())
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
        var axis = serviceAxis.GetAll(p => p._id == idaxis).FirstOrDefault();


        foreach (var item in serviceGroup.GetAll(p => p.Axis._id == axis._id).ToList())
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
        var company = serviceCompany.GetAll(p => p._id == idcompany).FirstOrDefault();
        foreach (var item in company.Skills)
        {
          if (item._id == id)
          {
            company.Skills.Remove(item);
            this.serviceCompany.Update(company, null);
            UpdateCompanyAll(company);
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
        var group = serviceGroup.GetAll(p => p._id == idgroup).FirstOrDefault();

        foreach (var item in servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Occupation.Group._id == group._id).ToList())
        {
          return "error_exists_register";
        }


        foreach (var item in serviceOccupation.GetAll(p => p.Group._id == group._id).ToList())
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
        var group = serviceGroup.GetAll(p => p._id == idgroup).FirstOrDefault();
        var schooling = group.Schooling.Where(p => p._id == id).FirstOrDefault();
        group.Schooling.Remove(schooling);
        serviceGroup.Update(group, null);
        UpdateGroupAll(group);

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
        var group = serviceGroup.GetAll(p => p._id == idgroup).FirstOrDefault();
        var skill = group.Skills.Where(p => p._id == id).FirstOrDefault();
        group.Skills.Remove(skill);
        serviceGroup.Update(group, null);
        UpdateGroupAll(group);

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
        var group = serviceGroup.GetAll(p => p._id == idgroup).FirstOrDefault();
        var scope = group.Scope.Where(p => p._id == idscope).FirstOrDefault();
        group.Scope.Remove(scope);
        serviceGroup.Update(group, null);
        UpdateGroupAll(group);

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
        var occupation = serviceOccupation.GetAll(p => p._id == idoccupation).FirstOrDefault();

        foreach (var item in servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Occupation._id == occupation._id).ToList())
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
        var occupation = serviceOccupation.GetAll(p => p._id == idoccupation).FirstOrDefault();
        var activitie = occupation.Activities.Where(p => p._id == idactivitie).FirstOrDefault();
        occupation.Activities.Remove(activitie);
        serviceOccupation.Update(occupation, null);
        UpdateOccupationAll(occupation);

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
        var occupation = serviceOccupation.GetAll(p => p._id == idoccupation).FirstOrDefault();
        var skill = occupation.Skills.Where(p => p._id == id).FirstOrDefault();
        occupation.Skills.Remove(skill);
        serviceOccupation.Update(occupation, null);
        UpdateOccupationAll(occupation);

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
        var skill = serviceSkill.GetAll(p => p._id == idskill).FirstOrDefault();
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
        var sphere = serviceSphere.GetAll(p => p._id == idsphere).FirstOrDefault();

        //foreach (var item in axisService.GetAll(p => p.Sphere._id == sphere._id).ToList())
        //{
        //  return "error_exists_register";
        //}

        foreach (var item in serviceGroup.GetAll(p => p.Sphere._id == sphere._id).ToList())
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
        var schooling = serviceSchooling.GetAll(p => p._id == idschooling).FirstOrDefault();
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
        var questions = serviceQuestions.GetAll(p => p._id == idquestion).FirstOrDefault();
        questions.Status = EnumStatus.Disabled;
        serviceQuestions.Update(questions, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteCBO(string id)
    {
      try
      {
        var cbo = serviceCbo.GetAuthentication(p => p._id == id).FirstOrDefault();
        cbo.Status = EnumStatus.Disabled;
        serviceCbo.UpdateAccount(cbo, null);
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
        return serviceCompany.GetAll().ToList().Select(p => new ViewListCompany()
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
        foreach (var item in serviceGroup.GetAll(p => p.Company._id == idcompany))
        {
          var view = new ViewGroupListLO
          {
            _id = item._id,
            Name = item.Name,
            Axis = new ViewListAxis() { _id = item.Axis._id, Name = item.Axis.Name, TypeAxis = item.Axis.TypeAxis },
            Sphere = new ViewListSphere() { _id = item.Sphere._id, Name = item.Sphere.Name, TypeSphere = item.Sphere.TypeSphere },
            Company = new ViewListCompany() { _id = item.Company._id, Name = item.Company.Name },
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

    public List<ViewGetOccupation> GetOccupations(string idcompany, string idarea)
    {
      try
      {
        AdjustOccuptaions();
        var area = serviceArea.GetAll(p => p._id == idarea).FirstOrDefault();
        //return occupationService.GetAll(p => p.Area._id == idarea & p.Group.Company._id == idcompany).OrderBy(p => p.Name).ToList();
        var itens = serviceOccupation.GetAll(p => p.Group.Company._id == idcompany).OrderBy(p => p.Name).ToList();
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
        var area = serviceArea.GetAll(p => p._id == idarea).FirstOrDefault();
        var itens = serviceOccupation.GetAll(p => p.Group.Company._id == idcompany).OrderBy(p => p.Name).ToList();
        List<Occupation> list = new List<Occupation>();
        foreach (var item in itens)
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
                  CBO = item.CBO,
                  SpecificRequirements = item.SpecificRequirements,
                  Process = item.Process,
                  _id = item._id,
                  _idAccount = item._idAccount,
                  Status = item.Status,
                  //Areas = item.Areas
                });
              }

            }
          }
        }
        list.OrderBy(p => p.Name).ToList();

        int skip = (count * (page - 1));

        var itensResult = list.Where(p => p.Group.Company._id == idcompany
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
            Schooling = p.Schooling.Where(x => x.Complement != null).Count()
          }).ToList();

        total = list.Count();

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
        var detail = serviceSkill.GetAll(p => p.Name.ToUpper() == filterName.ToUpper())
          .Select(p => new ViewCrudSkill()
          {
            _id = p._id,
            Name = p.Name,
            Concept = p.Concept,
            TypeSkill = p.TypeSkill
          })
          .ToList();
        if (detail.Count == 1)
          return detail[0];
        detail = serviceSkill.GetAll(p => p.Name.ToUpper().Contains(filterName.ToUpper())).Select(p => new ViewCrudSkill()
        {
          _id = p._id,
          Name = p.Name,
          Concept = p.Concept,
          TypeSkill = p.TypeSkill
        }).ToList();
        if (detail.Count == 1)
          return detail[0];
        if (detail.Count > 1)
          throw new Exception("Mais de uma skill!");
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
        int skip = (count * (page - 1));
        var detail = serviceSkill.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = serviceSkill.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewListSkill()
        {
          _id = p._id,
          Name = p.Name,
          Concept = p.Concept,
          TypeSkill = p.TypeSkill
        }).ToList();
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

        var skills = (List<string>)(from comp in serviceCompany.GetAll()
                                    where comp._id == company
                                    select new
                                    {
                                      Name = comp.Skills.Select(p => p.Name)
                                    }
                    ).FirstOrDefault().Name;


        var detail = serviceSkill.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper()))
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

        var skills = (List<string>)(from comp in serviceCompany.GetAll()
                                    where comp._id == idcompany
                                    select new
                                    {
                                      Name = comp.Skills.Select(p => p.Name)
                                    }
                    ).FirstOrDefault().Name;

        var skillsGroup = (List<string>)(from groups in serviceGroup.GetAll()
                                         where groups._id == idgroup
                                         select new
                                         {
                                           Name = groups.Skills.Select(p => p.Name)
                                         }
                   ).FirstOrDefault().Name;


        var detail = serviceSkill.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper()))
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

        var skills = (List<string>)(from comp in serviceCompany.GetAll()
                                    where comp._id == idcompany
                                    select new
                                    {
                                      Name = comp.Skills.Select(p => p.Name)
                                    }
                    ).FirstOrDefault().Name;

        var skillsGroup = (List<string>)(from groups in serviceGroup.GetAll()
                                         where groups._id == idgroup
                                         select new
                                         {
                                           Name = groups.Skills.Select(p => p.Name)
                                         }
                   ).FirstOrDefault().Name;

        var skillsOccupation = (List<string>)(from occupation in serviceOccupation.GetAll()
                                              where occupation._id == idoccupation
                                              select new
                                              {
                                                Name = occupation.Skills.Select(p => p.Name)
                                              }
                 ).FirstOrDefault().Name;


        var detail = serviceSkill.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper()))
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
        var detail = serviceProcessLevelTwo.GetAll(p => p._id == id)
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
              Area = new ViewListArea()
              {
                _id = p.ProcessLevelOne.Area._id,
                Name = p.ProcessLevelOne.Area.Name
              }
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
        //var result = processLevelOneService.GetAll(p => p.Area._id == idarea);
        var result = serviceProcessLevelOne.GetAll(p => p.Area._id == idarea).OrderBy(p => p.Order).ToList();
        var list = new List<ViewListProcessLevelTwo>();
        foreach (var item in result)
        {
          foreach (var row in serviceProcessLevelTwo.GetAll(p => p.ProcessLevelOne._id == item._id).OrderBy(p => p.Order).ToList())
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
                Area = new ViewListArea()
                {
                  _id = row.ProcessLevelOne.Area._id,
                  Name = row.ProcessLevelOne.Area.Name
                }
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
        //var result = processLevelOneService.GetAll(p => p.Area._id == idarea);
        var result = serviceProcessLevelOne.GetAll().OrderBy(p => p.Order).ToList();
        var list = new List<ViewListProcessLevelTwo>();
        foreach (var item in result)
        {
          foreach (var row in serviceProcessLevelTwo.GetAll(p => p.ProcessLevelOne._id == item._id).OrderBy(p => p.Order).ToList())
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
                Area = new ViewListArea()
                {
                  _id = row.ProcessLevelOne.Area._id,
                  Name = row.ProcessLevelOne.Area.Name
                }
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
            Name = item.Name,
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

    private async Task UpdateSkillAll(Skill skill, bool remove)
    {
      try
      {
        foreach (var company in serviceCompany.GetAll().ToList())
        {
          foreach (var item in company.Skills)
          {
            if (item._id == skill._id)
            {
              company.Skills.Remove(item);
              if (remove == false)
                company.Skills.Add(skill);

              break;
            }
          }

          serviceCompany.Update(company, null);
          UpdateCompanyAll(company);

          foreach (var group in serviceGroup.GetAll(p => p.Company._id == company._id).ToList())
          {
            foreach (var item in group.Skills)
            {
              if (item._id == skill._id)
              {
                group.Skills.Remove(item);
                if (remove == false)
                  group.Skills.Add(skill);

                break;
              }
            }
            serviceGroup.Update(group, null);
            UpdateGroupAll(group);
          }

          foreach (var occupation in serviceOccupation.GetAll(p => p.Group.Company._id == company._id).ToList())
          {
            foreach (var item in occupation.Skills)
            {
              if (item._id == skill._id)
              {
                occupation.Skills.Remove(item);
                if (remove == false)
                  occupation.Skills.Add(skill);

                break;
              }
            }
            serviceOccupation.Update(occupation, null);
            UpdateOccupationAll(occupation);
          }
        }
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
        var group = serviceGroup.GetAll(p => p._id == id).ToList().Select(
          p => new ViewCrudGroup()
          {
            _id = p._id,
            Name = p.Name,
            Company = new ViewListCompany()
            {
              _id = p.Company._id,
              Name = p.Company.Name,
            },
            Axis = new ViewListAxis() { _id = p.Axis._id, Name = p.Axis.Name, TypeAxis = p.Axis.TypeAxis },
            Sphere = new ViewListSphere() { _id = p.Sphere._id, Name = p.Sphere.Name, TypeSphere = p.Sphere.TypeSphere },
            Line = p.Line
          }).FirstOrDefault();
        //group.Occupations = occupationService.GetAll(p => p.Group._id == group._id).OrderBy(p => p.Name).ToList();
        return group;
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
        var group = serviceGroup.GetAll(p => p.Company._id == idCompany && p.Name.ToUpper().Contains(filterName.ToUpper())).ToList().Select(
            p => new ViewCrudGroup()
            {
              _id = p._id,
              Name = p.Name,
              Company = new ViewListCompany()
              {
                _id = p.Company._id,
                Name = p.Company.Name,
              },
              Axis = new ViewListAxis() { _id = p.Axis._id, Name = p.Axis.Name, TypeAxis = p.Axis.TypeAxis },
              Sphere = new ViewListSphere() { _id = p.Sphere._id, Name = p.Sphere.Name, TypeSphere = p.Sphere.TypeSphere },
              Line = p.Line
            }).FirstOrDefault();
        //group.Occupations = occupationService.GetAll(p => p.Group._id == group._id).OrderBy(p => p.Name).ToList();
        return group;
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
        //foreach (var item in groupService.GetAll())
        //{
        //  item.Occupations = occupationService.GetAll(p => p.Group._id == item._id).OrderBy(p => p.Name).ToList();
        //  groups.Add(item);
        //}
        //return groups.OrderBy(p => p.Name)
        //  .Select()
        //  .ToList();
        return serviceGroup.GetAll().Select(p => new ViewListGroup()
        {
          _id = p._id,
          Name = p.Name,
          Axis = new ViewListAxis() { _id = p.Axis._id, Name = p.Axis.Name, TypeAxis = p.Axis.TypeAxis },
          Sphere = new ViewListSphere() { _id = p.Sphere._id, Name = p.Sphere.Name, TypeSphere = p.Sphere.TypeSphere },
          Line = p.Line
        }).ToList();
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
        foreach (var item in serviceGroup.GetAll(p => p.Company._id == idcompany))
        {
          item.Occupations = serviceOccupation.GetAll(p => p.Group._id == item._id).ToList();
          groups.Add(item);
        }
        return groups.Select(p => new ViewListGroup()
        {
          _id = p._id,
          Name = p.Name,
          Axis = new ViewListAxis() { _id = p.Axis._id, Name = p.Axis.Name, TypeAxis = p.Axis.TypeAxis },
          Sphere = new ViewListSphere() { _id = p.Sphere._id, Name = p.Sphere.Name, TypeSphere = p.Sphere.TypeSphere },
          Line = p.Line
        }).OrderByDescending(p => p.Sphere.TypeSphere).ThenByDescending(p => p.Axis.TypeAxis).ThenByDescending(p => p.Line).ToList();
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
        var occupation = serviceOccupation.GetAll(p => p._id == id).ToList().Select(p =>
          new ViewCrudOccupation()
          {
            _id = p._id,
            Name = p.Name,
            SalaryScales = p.SalaryScales?.Select(s => new ViewCrudSalaryScaleOccupation()
            {
              _id = s._idSalaryScale,
              Name = s.NameSalaryScale,
              NameGrade = s.NameGrade,
              _idGrade = s._idGrade
            }).ToList(),
            Group = new ViewListGroup()
            {
              _id = p.Group._id,
              Name = p.Group.Name,
              Axis = new ViewListAxis() { _id = p.Group.Axis._id, Name = p.Group.Axis.Name, TypeAxis = p.Group.Axis.TypeAxis },
              Sphere = new ViewListSphere() { _id = p.Group.Sphere._id, Name = p.Group.Sphere.Name, TypeSphere = p.Group.Sphere.TypeSphere },
              Line = p.Group.Line,
            },
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
                Area = new ViewListArea() { _id = x.ProcessLevelOne.Area._id, Name = x.ProcessLevelOne.Area.Name }
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
        var idcompany = serviceOccupation.GetAll(p => p._id == idoccuation).FirstOrDefault().Group.Company._id;
        var occupations = serviceOccupationMandatory.GetAll(p => p.Occupation._id == idoccuation & p.TypeMandatoryTraining == type).ToList();
        var company = serviceCompanyMandatory.GetAll(p => p.Company._id == idcompany & p.TypeMandatoryTraining == type).ToList();

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
        return serviceOccupation.GetAll(p => p.Group.Company._id == idCompany && p.Name.ToUpper() == filterName.ToUpper()).ToList().Select(p =>
          new ViewCrudOccupation()
          {
            _id = p._id,
            Name = p.Name,
            Group = new ViewListGroup()
            {
              _id = p.Group._id,
              Name = p.Group.Name,
              Axis = new ViewListAxis() { _id = p.Group.Axis._id, Name = p.Group.Axis.Name, TypeAxis = p.Group.Axis.TypeAxis },
              Sphere = new ViewListSphere() { _id = p.Group.Sphere._id, Name = p.Group.Sphere.Name, TypeSphere = p.Group.Sphere.TypeSphere },
              Line = p.Group.Line,
            },
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
                Area = new ViewListArea() { _id = x.ProcessLevelOne.Area._id, Name = x.ProcessLevelOne.Area.Name }
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

    public List<ViewListOccupationView> GetOccupations()
    {
      try
      {
        var list = serviceOccupation.GetAll().OrderBy(p => p.Name).ToList();
        List<ViewListOccupationView> result = new List<ViewListOccupationView>();
        foreach (var item in list)
        {
          foreach (var proc in item.Process)
          {
            result.Add(new ViewListOccupationView()
            {
              _id = item._id,
              Name = item.Name,
              NameGroup = item.Group.Name,
              NameArea = proc.ProcessLevelOne.Area.Name
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
        var item = serviceTextDefault.GetAll(p => p.Company._id == idcompany & p.Name == name).FirstOrDefault();
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
        var item = serviceTextDefault.GetAll(p => p._id == id).FirstOrDefault();
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
        return serviceTextDefault.GetAll(p => p.Company._id == idcompany)
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

    public string UpdateMapOccupationSchooling(string idoccupation, ViewCrudSchooling view)
    {
      try
      {
        var occupation = serviceOccupation.GetAll(p => p._id == idoccupation).FirstOrDefault();
        //var schooling = serviceSchooling.GetAll(p => p._id == view._id).FirstOrDefault();

        var schoolOld = occupation.Schooling.Where(p => p._id == view._id).FirstOrDefault();
        var schooling = schoolOld;
        schooling.Complement = view.Complement;
        schooling.Type = view.Type;

        occupation.Schooling.Remove(schoolOld);
        occupation.Schooling.Add(schooling);

        serviceOccupation.Update(occupation, null);
        UpdateOccupationAll(occupation);
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
        var occupation = serviceOccupation.GetAll(p => p._id == idoccupation).FirstOrDefault();
        var activitie = new Activitie()
        {
          Name = view.Name,
          Order = view.Order,
          Status = EnumStatus.Enabled,
          _idAccount = _user._idAccount,
          _id = view._id
        };
        if (activitie._id == null)
          activitie._id = ObjectId.GenerateNewId().ToString();

        var activitieOld = occupation.Activities.Where(p => p._id == activitie._id).FirstOrDefault();
        occupation.Activities.Remove(activitieOld);
        occupation.Activities.Add(activitie);

        serviceOccupation.Update(occupation, null);
        UpdateOccupationAll(occupation);
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
        var group = serviceGroup.GetAll(p => p._id == idgroup).FirstOrDefault();
        var scope = new Scope()
        {
          Name = view.Name,
          Order = view.Order,
          Status = EnumStatus.Enabled,
          _idAccount = _user._idAccount,
          _id = view._id
        };
        if (scope._id == null)
          scope._id = ObjectId.GenerateNewId().ToString();

        var scopeOld = group.Scope.Where(p => p._id == scope._id).FirstOrDefault();
        group.Scope.Remove(scopeOld);
        group.Scope.Add(scope);

        serviceGroup.Update(group, null);
        UpdateGroupAll(group);
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
        var group = serviceGroup.GetAll(p => p._id == idgroup).FirstOrDefault();
        var schooling = serviceSchooling.GetAll(p => p._id == view._id).FirstOrDefault();

        schooling.Complement = view.Complement;
        schooling.Type = view.Type;

        var schoolOld = group.Schooling.Where(p => p._id == schooling._id).FirstOrDefault();
        group.Schooling.Remove(schoolOld);
        group.Schooling.Add(schooling);

        serviceGroup.Update(group, null);
        UpdateGroupAll(group);
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
        //var groupOld = groupService.GetAll(p => p._id == group._id).FirstOrDefault();
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

        UpdateGroupAll(groupnew);
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
        var model = serviceArea.GetAll(p => p._id == view._id).FirstOrDefault();
        model.Name = view.Name;
        model.Order = view.Order;
        model.Company = serviceCompany.GetAll(p => p._id == view.Company._id).FirstOrDefault();

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
        var model = serviceAxis.GetAll(p => p._id == view._id).FirstOrDefault();
        model.Name = view.Name;
        model.Company = serviceCompany.GetAll(p => p._id == view.Company._id).FirstOrDefault();
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

    public string UpdateCBO(ViewCrudCbo view)
    {
      try
      {
        var model = serviceCbo.GetAll(p => p._id == view._id).FirstOrDefault();
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
        var groupOld = serviceGroup.GetAll(p => p._id == view._id).FirstOrDefault();
        var group = serviceGroup.GetAll(p => p._id == view._id).FirstOrDefault();

        group.Name = view.Name;
        group.Company = serviceCompany.GetAll(p => p._id == view.Company._id).FirstOrDefault();
        group.Line = view.Line;
        group.Axis = serviceAxis.GetAll(p => p._id == view.Axis._id).FirstOrDefault();
        group.Sphere = serviceSphere.GetAll(p => p._id == view.Sphere._id).FirstOrDefault();

        if ((groupOld.Sphere._id != view.Sphere._id) || (groupOld.Axis._id != view.Axis._id))
        {
          UpdateGroupSphereAxis(group, groupOld);
          return "update";
        }


        serviceGroup.Update(group, null);
        UpdateGroupAll(group);
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
        var occupationOld = serviceOccupation.GetAll(p => p._id == view._id).FirstOrDefault();
        var occupation = serviceOccupation.GetAll(p => p._id == view._id).FirstOrDefault();

        occupation.Group = serviceGroup.GetAll(p => p._id == view.Group._id).FirstOrDefault();
        occupation.Name = view.Name;
        occupation.Line = view.Line;

        if (view.Cbo != null)
          occupation.CBO = serviceCbo.GetAll(p => p._id == view.Cbo._id).FirstOrDefault();

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
              Status = EnumStatus.Enabled,
              _idAccount = _user._idAccount,
              _id = ObjectId.GenerateNewId().ToString()
            });
          }
        }


        occupation.Process = new List<ProcessLevelTwo>();
        if (view.Process != null)
        {
          foreach (var item in view.Process)
            occupation.Process.Add(serviceProcessLevelTwo.GetAll(p => p._id == item._id).FirstOrDefault());
        }


        //foreach (var item in occupation.Process)
        //  areas.Add(item.ProcessLevelOne.Area);

        if (occupationOld.Group != occupation.Group)
        {
          foreach (var school in occupation.Group.Schooling)
          {
            foreach (var schoolOccupation in occupationOld.Schooling)
            {
              if (school._id == schoolOccupation._id)
                school.Complement = schoolOccupation.Complement;
            }
          }

          occupation.Schooling = occupation.Schooling;
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

            item._idAccount = _user._idAccount;
            occupation.SalaryScales.Add(item);
          }
        }

        serviceOccupation.Update(occupation, null);

        UpdateOccupationAll(occupation);
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
        var model = serviceSkill.GetAll(p => p._id == view._id).FirstOrDefault();
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
        var model = serviceQuestions.GetAll(p => p._id == view._id).FirstOrDefault();
        model.Name = view.Name;
        model.Content = view.Content;
        model.Company = serviceCompany.GetAll(p => p._id == view.Company._id).FirstOrDefault();
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
        var model = serviceSphere.GetAll(p => p._id == view._id).FirstOrDefault();
        model.Name = view.Name;
        model.Company = serviceCompany.GetAll(p => p._id == view.Company._id).FirstOrDefault();
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
        var model = serviceSchooling.GetAll(p => p._id == view._id).FirstOrDefault();
        model.Name = view.Name;
        model.Complement = view.Complement;
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
        var model = serviceProcessLevelOne.GetAll(p => p._id == view._id).FirstOrDefault();
        model.Name = view.Name;
        model.Area = serviceArea.GetAll(p => p._id == view.Area._id).FirstOrDefault();
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
        var model = serviceProcessLevelTwo.GetAll(p => p._id == view._id).FirstOrDefault();
        model.Name = view.Name;
        model.ProcessLevelOne = serviceProcessLevelOne.GetAll(p => p._id == view.ProcessLevelOne._id).FirstOrDefault();
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
        var model = serviceTextDefault.GetAll(p => p._id == view._id).FirstOrDefault();
        model.Name = view.Name;
        model.Company = serviceCompany.GetAll(p => p._id == view.Company._id).FirstOrDefault();
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
        var group = serviceGroup.GetAll(p => p._id == id).FirstOrDefault();
        var company = serviceCompany.GetAll(p => p._id == group.Company._id).FirstOrDefault();

        var view = new ViewMapGroup()
        {
          _id = group._id,
          Name = group.Name,
          Line = group.Line,
          Company = new ViewListCompany() { _id = group.Company._id, Name = group.Company.Name },
          Axis = new ViewListAxis() { _id = group.Axis._id, Name = group.Axis.Name, TypeAxis = group.Axis.TypeAxis },
          Sphere = new ViewListSphere() { _id = group.Sphere._id, Name = group.Sphere.Name, TypeSphere = group.Sphere.TypeSphere },
          Schooling = group.Schooling?.OrderBy(o => o.Order).Select(p => new ViewListSchooling()
          {
            _id = p._id,
            Name = p.Name,
            Order = p.Order
          }).ToList(),
          Scope = group.Scope?.OrderBy(o => o.Order).Select(p => new ViewListScope()
          {
            _id = p._id,
            Name = p.Name,
            Order = p.Order
          }).ToList(),
          Skills = group.Skills?.OrderBy(o => o.Name).Select(p => new ViewListSkill()
          {
            _id = p._id,
            Name = p.Name,
            Concept = p.Concept,
            TypeSkill = p.TypeSkill
          }).ToList(),
          SkillsCompany = group.Company.Skills?.OrderBy(o => o.Name).Select(p => new ViewListSkill()
          {
            _id = p._id,
            Name = p.Name,
            Concept = p.Concept,
            TypeSkill = p.TypeSkill
          }).ToList()
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
        var group = serviceGroup.GetAll(p => p._id == idgroup).FirstOrDefault();


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
        var list = serviceProcessLevelOne.GetAll(p => p.Area._id == idarea).ToList();

        var result = new List<ViewListProcessLevelOneByArea>();
        foreach (var item in list)
        {
          var view = new ViewListProcessLevelOneByArea()
          {
            _id = item._id,
            Name = item.Name,
            Area = new ViewListArea() { _id = item.Area._id, Name = item.Area.Name },
            Order = item.Order,
            Process = serviceProcessLevelTwo.GetAll(p => p.ProcessLevelOne._id == item._id).Select(x => new ViewCrudProcessLevelTwo()
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
        var area = serviceArea.GetAll(p => p._id == id).FirstOrDefault();
        return new ViewCrudArea()
        {
          _id = area._id,
          Name = area.Name,
          Order = area.Order,
          Company = new ViewListCompany() { _id = area.Company._id, Name = area.Company.Name }
        };
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
        var occupation = serviceOccupation.GetAll(p => p._id == id).FirstOrDefault();

        var group = serviceGroup.GetAll(p => p._id == occupation.Group._id).FirstOrDefault();
        var company = serviceCompany.GetAll(p => p._id == group.Company._id).FirstOrDefault();

        var map = new ViewMapOccupation()
        {
          _id = occupation._id,
          Name = occupation.Name,
          SpecificRequirements = occupation.SpecificRequirements,
          Company = new ViewListCompany() { _id = company._id, Name = company.Name },
          Group = new ViewListGroup()
          {
            _id = group._id,
            Name = group.Name,
            Line = group.Line,
            Axis = new ViewListAxis() { _id = group.Axis._id, Name = group.Axis.Name, TypeAxis = group.Axis.TypeAxis },
            Sphere = new ViewListSphere() { _id = group.Sphere._id, Name = group.Sphere.Name, TypeSphere = group.Sphere.TypeSphere }
          },
          Activities = occupation.Activities?.OrderBy(o => o.Order).Select(x => new ViewListActivitie()
          {
            _id = x._id,
            Name = x.Name,
            Order = x.Order
          }).ToList(),
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
              Area = new ViewListArea()
              {
                _id = x.ProcessLevelOne?.Area._id,
                Name = x.ProcessLevelOne?.Area.Name
              }
            }
          }).ToList(),
          Schooling = occupation.Schooling?.OrderBy(o => o.Order).Select(x => new ViewCrudSchooling()
          {
            _id = x._id,
            Name = x.Name,
            Complement = x.Complement,
            Type = x.Type,
            Order = x.Order
          }).ToList(),
          Skills = occupation.Skills?.OrderBy(o => o?.Name).Select(x => new ViewListSkill()
          {
            _id = x?._id,
            Name = x?.Name,
            Concept = x?.Concept,
            TypeSkill = (x == null) ? 0 : x.TypeSkill
          }).ToList(),
          SkillsCompany = company.Skills?.OrderBy(o => o.Name).Select(x => new ViewListSkill()
          {
            _id = x._id,
            Name = x.Name,
            Concept = x.Concept,
            TypeSkill = x.TypeSkill
          }).ToList(),
          SkillsGroup = group.Skills?.OrderBy(o => o.Name).Select(x => new ViewListSkill()
          {
            _id = x._id,
            Name = x.Name,
            Concept = x.Concept,
            TypeSkill = x.TypeSkill
          }).ToList(),
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
    public ViewCrudProcessLevelOne GetListProcessLevelOneById(string id)
    {
      try
      {
        return serviceProcessLevelOne.GetAll(p => p._id == id).Select(p => new ViewCrudProcessLevelOne()
        {
          _id = p._id,
          Name = p.Name,
          Order = p.Order,
          Area = new ViewListArea()
          {
            _id = p.Area._id,
            Name = p.Area.Name
          }
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
        return serviceProcessLevelTwo.GetAll(p => p._id == id).Select(p => new ViewCrudProcessLevelTwo()
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
            Area = new ViewListArea()
            {
              _id = p.ProcessLevelOne.Area._id,
              Name = p.ProcessLevelOne.Area.Name
            }
          }
        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion Infra

  }
#pragma warning restore 1998
#pragma warning restore 4014
}
