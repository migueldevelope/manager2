using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
#pragma warning disable 1998
#pragma warning disable 4014
  public class ServiceInfra : Repository<Group>, IServiceInfra
  {
    private readonly ServiceGeneric<Sphere> sphereService;
    private readonly ServiceGeneric<DictionarySphere> dictionarySphereService;
    private readonly ServiceGeneric<Axis> axisService;
    private readonly ServiceGeneric<Group> groupService;
    private readonly ServiceGeneric<Occupation> occupationService;
    private readonly ServiceGeneric<Area> areaService;
    private readonly ServiceGeneric<Company> companyService;
    private readonly ServiceGeneric<Skill> skillService;
    private readonly ServiceGeneric<Schooling> schoolingService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<Parameter> parameterService;
    private readonly ServiceGeneric<ProcessLevelOne> processLevelOneService;
    private readonly ServiceGeneric<ProcessLevelTwo> processLevelTwoService;
    private readonly ServiceGeneric<Questions> questionsService;
    private readonly ServiceGeneric<TextDefault> textDefaultService;
    private readonly ServiceGeneric<CBO> cboService;


    public ServiceInfra(DataContext context)
      : base(context)
    {
      try
      {
        sphereService = new ServiceGeneric<Sphere>(context);
        dictionarySphereService = new ServiceGeneric<DictionarySphere>(context);
        axisService = new ServiceGeneric<Axis>(context);
        groupService = new ServiceGeneric<Group>(context);
        occupationService = new ServiceGeneric<Occupation>(context);
        areaService = new ServiceGeneric<Area>(context);
        companyService = new ServiceGeneric<Company>(context);
        skillService = new ServiceGeneric<Skill>(context);
        schoolingService = new ServiceGeneric<Schooling>(context);
        personService = new ServiceGeneric<Person>(context);
        parameterService = new ServiceGeneric<Parameter>(context);
        processLevelOneService = new ServiceGeneric<ProcessLevelOne>(context);
        processLevelTwoService = new ServiceGeneric<ProcessLevelTwo>(context);
        questionsService = new ServiceGeneric<Questions>(context);
        textDefaultService = new ServiceGeneric<TextDefault>(context);
        cboService = new ServiceGeneric<CBO>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddArea(Area view)
    {
      try
      {
        //var item = areaService.GetAll(p => p.Order == view.Order).Count();
        //if (item > 0)
        //  return "error_line";


        areaService.Insert(view);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddCBO(CBO view)
    {
      try
      {
        cboService.InsertAccount(view);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddAxis(Axis view)
    {
      try
      {
        axisService.Insert(view);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddEssential(ViewAddEssential view)
    {
      try
      {
        if (view.Skill._id == null)
        {
          var skill = AddSkill(new ViewAddSkill()
          {
            Name = view.Skill.Name,
            Concept = view.Skill.Concept,
            TypeSkill = view.Skill.TypeSkill
          });
          view.Skill = skill;
        }

        view.Company.Skills.Add(view.Skill);
        companyService.Update(view.Company, null);

        UpdateCompanyAll(view.Company);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Group AddGroup(ViewAddGroup view)
    {
      try
      {
        long line = 0;
        try
        {
          line = groupService.GetAll(p => p.Sphere._id == view.Sphere._id & p.Axis._id == view.Axis._id).Max(p => p.Line);
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
        return groupService.Insert(group);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private async Task UpdateSkillAll(Skill skill, bool remove)
    {
      try
      {
        foreach (var company in companyService.GetAll().ToList())
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

          companyService.Update(company, null);
          UpdateCompanyAll(company);

          foreach (var group in groupService.GetAll(p => p.Company._id == company._id).ToList())
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
            groupService.Update(group, null);
            UpdateGroupAll(group);
          }

          foreach (var occupation in occupationService.GetAll(p => p.Group.Company._id == company._id).ToList())
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
            occupationService.Update(occupation, null);
            UpdateOccupationAll(occupation);
          }
        }
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }

    }

    public string AddMapGroupSchooling(ViewAddMapGroupSchooling view)
    {
      try
      {


        //if (view.Group.Schooling.Where(p => p.Type == view.Schooling.Type).Count() > 0)
        //  return "error_exists_schooling";

        //view.Group.Schooling.Add(AddSchooling(view.Schooling));
        view.Group.Schooling.Add(view.Schooling);
        groupService.Update(view.Group, null);
        UpdateGroupAll(view.Group);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddMapGroupScope(ViewAddMapGroupScope view)
    {
      try
      {

        long order = 1;
        try
        {
          order = groupService.GetAll(p => p._id == view.Group._id).FirstOrDefault().Scope.Max(p => p.Order) + 1;
          if (order == 0)
          {
            order = 1;
          }
        }
        catch (Exception)
        {
          order = 1;
        }

        view.Scope._id = ObjectId.GenerateNewId().ToString();
        view.Scope._idAccount = view.Group._idAccount;
        view.Scope.Order = order;

        view.Group.Scope.Add(view.Scope);



        groupService.Update(view.Group, null);
        UpdateGroupAll(view.Group);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddMapGroupSkill(ViewAddMapGroupSkill view)
    {
      try
      {
        if (view.Skill._id == null)
        {
          var skill = AddSkill(new ViewAddSkill()
          {
            Name = view.Skill.Name,
            Concept = view.Skill.Concept,
            TypeSkill = view.Skill.TypeSkill
          });
          view.Skill = skill;
        }

        view.Group.Skills.Add(view.Skill);
        groupService.Update(view.Group, null);
        UpdateGroupAll(view.Group);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddOccupation(Occupation occupation)
    {
      try
      {
        occupationService.Insert(occupation);
        return "ok";
      }
      catch (Exception)
      {
        throw;
      }
    }
    public string AddOccupation(ViewAddOccupation view)
    {
      try
      {
        //var item = occupationService.GetAll(p => p.Line == view.Line).Count();
        //if (item > 0)
        //  return "error_line";

        var areas = new List<Area>();
        foreach (var item in view.Process)
          areas.Add(item.ProcessLevelOne.Area);

        var occupation = new Occupation()
        {
          Name = view.Name,
          Group = view.Group,
          Area = view.Area,
          Line = view.Line,
          ProcessLevelTwo = view.ProcessLevelTwo,
          Status = EnumStatus.Enabled,
          Activities = new List<Activitie>(),
          Schooling = view.Group.Schooling,
          Skills = new List<Skill>(),
          Process = view.Process,
          Areas = areas
        };
        occupationService.Insert(occupation);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddOccupationActivities(ViewAddOccupationActivities view)
    {
      try
      {
        long order = 1;
        try
        {
          order = occupationService.GetAll(p => p._id == view.Occupation._id).FirstOrDefault().Activities.Max(p => p.Order) + 1;
          if (order == 0)
          {
            order = 1;
          }
        }
        catch (Exception)
        {
          order = 1;
        }

        view.Activities.Order = order;
        view.Activities._id = ObjectId.GenerateNewId().ToString();
        view.Activities._idAccount = view.Occupation._idAccount;
        view.Occupation.Activities.Add(view.Activities);
        occupationService.Update(view.Occupation, null);
        UpdateOccupationAll(view.Occupation);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddOccupationActivitiesList(List<ViewAddOccupationActivities> list)
    {
      try
      {
        foreach (var view in list)
        {
          AddOccupationActivities(view);
        }

        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddSpecificRequirements(string idoccupation, ViewAddSpecificRequirements view)
    {
      try
      {
        var occupation = occupationService.GetAll(p => p._id == idoccupation).FirstOrDefault();
        occupation.SpecificRequirements = view.Name;
        occupationService.Update(occupation, null);
        UpdateOccupationAll(occupation);

        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddOccupationSkill(ViewAddOccupationSkill view)
    {
      try
      {
        if (view.Skill._id == null)
        {
          var skill = AddSkill(new ViewAddSkill()
          {
            Name = view.Skill.Name,
            Concept = view.Skill.Concept,
            TypeSkill = view.Skill.TypeSkill
          });
          view.Skill = skill;
        }

        view.Occupation.Skills.Add(view.Skill);
        occupationService.Update(view.Occupation, null);
        UpdateOccupationAll(view.Occupation);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Skill AddSkill(ViewAddSkill view)
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
        skillService.Insert(skill);
        return skill;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Schooling AddSchooling(Schooling schooling)
    {
      try
      {
        return schoolingService.Insert(schooling);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddSphere(Sphere view)
    {
      try
      {
        sphereService.Insert(view);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddQuestions(Questions view)
    {
      try
      {
        questionsService.Insert(view);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteArea(string idarea)
    {
      try
      {
        if (processLevelOneService.GetAll(p => p.Area._id == idarea).Count() > 0)
          return "erro_exists_nivelone";

        var area = areaService.GetAll(p => p._id == idarea).FirstOrDefault();


        foreach (var item in occupationService.GetAll(p => p.Area._id == area._id).ToList())
        {
          return "error_exists_register";
        }

        area.Status = EnumStatus.Disabled;
        areaService.Update(area, null);

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteAxis(string idaxis)
    {
      try
      {
        var axis = axisService.GetAll(p => p._id == idaxis).FirstOrDefault();


        foreach (var item in groupService.GetAll(p => p.Axis._id == axis._id).ToList())
        {
          return "error_exists_register";
        }
        axis.Status = EnumStatus.Disabled;
        axisService.Update(axis, null);

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteEssential(string idcompany, string id)
    {
      try
      {
        var company = companyService.GetAll(p => p._id == idcompany).FirstOrDefault();
        foreach (var item in company.Skills)
        {
          if (item._id == id)
          {
            company.Skills.Remove(item);
            this.companyService.Update(company, null);
            UpdateCompanyAll(company);
            return "delete";
          }
        }

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private async Task UpdateCompanyAll(Company company)
    {
      try
      {
        foreach (var item in personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Company._id == company._id).ToList())
        {
          item.Company = company;
          personService.Update(item, null);
        }

        foreach (var item in sphereService.GetAll(p => p.Company._id == company._id).ToList())
        {
          item.Company = company;
          sphereService.Update(item, null);
        }

        foreach (var item in axisService.GetAll(p => p.Sphere.Company._id == company._id).ToList())
        {
          item.Sphere.Company = company;
          axisService.Update(item, null);
        }

        foreach (var item in occupationService.GetAll(p => p.Group.Company._id == company._id).ToList())
        {
          item.Group.Company = company;
          occupationService.Update(item, null);
        }

        foreach (var item in groupService.GetAll(p => p.Company._id == company._id).ToList())
        {
          item.Company = company;
          groupService.Update(item, null);
        }

        foreach (var item in areaService.GetAll(p => p.Company._id == company._id).ToList())
        {
          item.Company = company;
          areaService.Update(item, null);
        }


      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private async Task UpdateGroupAll(Group group)
    {
      try
      {
        foreach (var item in occupationService.GetAll(p => p.Group._id == group._id).ToList())
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
          occupationService.Update(item, null);
          UpdateOccupationAll(item);
        }


        foreach (var item in personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Occupation.Group._id == group._id).ToList())
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
          personService.Update(item, null);
        }

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private async Task UpdateCBOAll(CBO cbo)
    {
      try
      {
        foreach (var item in occupationService.GetAuthentication(p => p.CBO._id == cbo._id).ToList())
        {
          item.CBO = cbo;
          occupationService.UpdateAccount(item, null);
          UpdateOccupationAllCBO(item);
        }

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private async Task UpdateOccupationAllCBO(Occupation occupation)
    {
      try
      {
        foreach (var item in personService.GetAuthentication(p => p.Occupation._id == occupation._id).ToList())
        {
          item.Occupation = occupation;
          personService.UpdateAccount(item, null);
        }

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private async Task UpdateOccupationAll(Occupation occupation)
    {
      try
      {
        foreach (var item in personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Occupation._id == occupation._id).ToList())
        {
          item.Occupation = occupation;
          personService.Update(item, null);
        }

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteGroup(string idgroup)
    {
      try
      {
        var group = groupService.GetAll(p => p._id == idgroup).FirstOrDefault();

        foreach (var item in personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Occupation.Group._id == group._id).ToList())
        {
          return "error_exists_register";
        }


        foreach (var item in occupationService.GetAll(p => p.Group._id == group._id).ToList())
        {
          return "error_exists_register";
        }

        group.Status = EnumStatus.Disabled;
        groupService.Update(group, null);
        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteMapGroupSchooling(string idgroup, string id)
    {
      try
      {
        var group = groupService.GetAll(p => p._id == idgroup).FirstOrDefault();
        var schooling = group.Schooling.Where(p => p._id == id).FirstOrDefault();
        group.Schooling.Remove(schooling);
        groupService.Update(group, null);
        UpdateGroupAll(group);

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteMapGroupSkill(string idgroup, string id)
    {
      try
      {
        var group = groupService.GetAll(p => p._id == idgroup).FirstOrDefault();
        var skill = group.Skills.Where(p => p._id == id).FirstOrDefault();
        group.Skills.Remove(skill);
        groupService.Update(group, null);
        UpdateGroupAll(group);

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteMapGroupScope(string idgroup, string idscope)
    {
      try
      {
        var group = groupService.GetAll(p => p._id == idgroup).FirstOrDefault();
        var scope = group.Scope.Where(p => p._id == idscope).FirstOrDefault();
        group.Scope.Remove(scope);
        groupService.Update(group, null);
        UpdateGroupAll(group);

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteOccupation(string idoccupation)
    {
      try
      {
        var occupation = occupationService.GetAll(p => p._id == idoccupation).FirstOrDefault();

        foreach (var item in personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Occupation._id == occupation._id).ToList())
        {
          return "error_exists_register";
        }

        occupation.Status = EnumStatus.Disabled;
        occupationService.Update(occupation, null);
        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteOccupationActivities(string idoccupation, string idactivitie)
    {
      try
      {
        var occupation = occupationService.GetAll(p => p._id == idoccupation).FirstOrDefault();
        var activitie = occupation.Activities.Where(p => p._id == idactivitie).FirstOrDefault();
        occupation.Activities.Remove(activitie);
        occupationService.Update(occupation, null);
        UpdateOccupationAll(occupation);

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteOccupationSkill(string idoccupation, string id)
    {
      try
      {
        var occupation = occupationService.GetAll(p => p._id == idoccupation).FirstOrDefault();
        var skill = occupation.Skills.Where(p => p._id == id).FirstOrDefault();
        occupation.Skills.Remove(skill);
        occupationService.Update(occupation, null);
        UpdateOccupationAll(occupation);

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteSkill(string idskill)
    {
      try
      {
        var skill = skillService.GetAll(p => p._id == idskill).FirstOrDefault();
        skill.Status = EnumStatus.Disabled;
        skillService.Update(skill, null);

        UpdateSkillAll(skill, true);
        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteSphere(string idsphere)
    {
      try
      {
        var sphere = sphereService.GetAll(p => p._id == idsphere).FirstOrDefault();

        foreach (var item in axisService.GetAll(p => p.Sphere._id == sphere._id).ToList())
        {
          return "error_exists_register";
        }

        foreach (var item in groupService.GetAll(p => p.Sphere._id == sphere._id).ToList())
        {
          return "error_exists_register";
        }
        sphere.Status = EnumStatus.Disabled;
        sphereService.Update(sphere, null);

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteSchooling(string idschooling)
    {
      try
      {
        var schooling = schoolingService.GetAll(p => p._id == idschooling).FirstOrDefault();
        schooling.Status = EnumStatus.Disabled;
        schoolingService.Update(schooling, null);
        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteQuestion(string idquestion)
    {
      try
      {
        var questions = questionsService.GetAll(p => p._id == idquestion).FirstOrDefault();
        questions.Status = EnumStatus.Disabled;
        questionsService.Update(questions, null);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteCBO(string id)
    {
      try
      {
        var cbo = cboService.GetAuthentication(p => p._id == id).FirstOrDefault();
        cbo.Status = EnumStatus.Disabled;
        cboService.UpdateAccount(cbo, null);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Area> GetAreas()
    {
      try
      {
        var areas = areaService.GetAll().OrderBy(p => p.Name).ToList();
        foreach (var item in areas)
        {
          item.ProcessLevelOnes = new List<ProcessLevelOne>();
          var process = processLevelOneService.GetAll(p => p.Area._id == item._id).OrderBy(p => p.Order).ToList();
          foreach (var row in process)
          {
            row.Process = new List<ProcessLevelTwo>();
            foreach (var leveltwo in processLevelTwoService.GetAll(p => p.ProcessLevelOne._id == row._id).OrderBy(p => p.Order).ToList())
            {
              row.Process.Add(leveltwo);
            }

            item.ProcessLevelOnes.Add(row);
          }
        }
        return areas.OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Area> GetAreas(string idcompany)
    {
      try
      {
        var areas = areaService.GetAll(p => p.Company._id == idcompany).OrderBy(p => p.Name).ToList();
        foreach (var item in areas)
        {
          item.ProcessLevelOnes = new List<ProcessLevelOne>();
          var process = processLevelOneService.GetAll(p => p.Area._id == item._id).OrderBy(p => p.Order).ToList();
          foreach (var row in process)
          {
            row.Process = new List<ProcessLevelTwo>();
            foreach (var leveltwo in processLevelTwoService.GetAll(p => p.ProcessLevelOne._id == row._id).OrderBy(p => p.Order).ToList())
            {
              row.Process.Add(leveltwo);
            }

            item.ProcessLevelOnes.Add(row);
          }
        }
        return areas.OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Axis> GetAxis()
    {
      try
      {
        return axisService.GetAll().OrderBy(p => p.TypeAxis).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Axis> GetAxis(string idcompany)
    {
      try
      {
        return axisService.GetAll(p => p.Sphere.Company._id == idcompany).OrderBy(p => p.TypeAxis).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Questions> ListQuestions(string idcompany)
    {
      try
      {
        return questionsService.GetAll(p => p.Company._id == idcompany).OrderBy(p => p.Order).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Questions GetQuestions(string id)
    {
      try
      {
        return questionsService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Company> GetCompanies()
    {
      try
      {
        return companyService.GetAll().ToList().Select(p => new Company()
        {
          _id = p._id,
          _idAccount = p._idAccount,
          Status = p.Status,
          Name = p.Name,
          Logo = p.Logo,
          Skills = p.Skills?.OrderBy(x => x.Name).ToList(),
          Template = p.Template
        }).OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Group GetGroup(string id)
    {
      try
      {
        var group = groupService.GetAll(p => p._id == id).ToList().Select(
          p => new Group()
          {
            _id = p._id,
            _idAccount = p._idAccount,
            Status = p.Status,
            Name = p.Name,
            Company = new Company()
            {
              _id = p.Company._id,
              _idAccount = p.Company._idAccount,
              Status = p.Company.Status,
              Name = p.Company.Name,
              Logo = p.Company.Logo,
              Skills = p.Company.Skills.OrderBy(x => x.Name).ToList(),
              Template = p.Company.Template
            },
            Axis = p.Axis,
            Sphere = p.Sphere,
            Line = p.Line,
            Skills = p.Skills.OrderBy(x => x.Name).ToList(),
            Schooling = p.Schooling.OrderBy(x => x.Order).ToList(),
            Scope = p.Scope.OrderBy(x => x.Order).ToList(),
            Template = p.Template,
            Occupations = p.Occupations
          }).FirstOrDefault();
        group.Occupations = occupationService.GetAll(p => p.Group._id == group._id).OrderBy(p => p.Name).ToList();
        return group;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Group GetGroup(string idCompany, string filterName)
    {
      try
      {
        var group = groupService.GetAll(p => p.Company._id == idCompany && p.Name.ToUpper().Contains(filterName.ToUpper())).ToList().Select(
          p => new Group()
          {
            _id = p._id,
            _idAccount = p._idAccount,
            Status = p.Status,
            Name = p.Name,
            Company = new Company()
            {
              _id = p.Company._id,
              _idAccount = p.Company._idAccount,
              Status = p.Company.Status,
              Name = p.Company.Name,
              Logo = p.Company.Logo,
              Skills = p.Company.Skills.OrderBy(x => x.Name).ToList(),
              Template = p.Company.Template
            },
            Axis = p.Axis,
            Sphere = p.Sphere,
            Line = p.Line,
            Skills = p.Skills.OrderBy(x => x.Name).ToList(),
            Schooling = p.Schooling.OrderBy(x => x.Order).ToList(),
            Scope = p.Scope.OrderBy(x => x.Order).ToList(),
            Template = p.Template,
            Occupations = p.Occupations
          }).FirstOrDefault();
        group.Occupations = occupationService.GetAll(p => p.Group._id == group._id).OrderBy(p => p.Name).ToList();
        return group;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public CBO GetCBO(string id)
    {
      try
      {
        var cbo = cboService.GetAuthentication(p => p._id == id).FirstOrDefault();
        return cbo;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<CBO> ListCBO()
    {
      try
      {
        var cbo = cboService.GetAuthentication(p => p.Status == EnumStatus.Enabled).ToList();
        return cbo;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Group> GetGroups()
    {
      try
      {
        List<Group> groups = new List<Group>();
        foreach (var item in groupService.GetAll())
        {
          item.Occupations = occupationService.GetAll(p => p.Group._id == item._id).OrderBy(p => p.Name).ToList();
          groups.Add(item);
        }
        return groups.OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Group> GetGroups(string idcompany)
    {
      try
      {
        List<Group> groups = new List<Group>();
        foreach (var item in groupService.GetAll(p => p.Company._id == idcompany))
        {
          item.Occupations = occupationService.GetAll(p => p.Group._id == item._id).ToList();
          groups.Add(item);
        }
        return groups.OrderBy(p => p.Sphere.TypeSphere).ThenBy(p => p.Axis.TypeAxis).ThenBy(p => p.Line).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Group> GetGroupsPrint(string idcompany)
    {
      try
      {
        List<Group> groups = new List<Group>();
        foreach (var item in groupService.GetAll(p => p.Company._id == idcompany))
        {
          item.Occupations = occupationService.GetAll(p => p.Group._id == item._id).ToList();
          groups.Add(item);
        }
        return groups.OrderByDescending(p => p.Sphere.TypeSphere).ThenByDescending(p => p.Axis.TypeAxis).ThenByDescending(p => p.Line).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Occupation GetOccupation(string id)
    {
      try
      {
        var occupation = occupationService.GetAll(p => p._id == id).ToList().Select(p =>
          new Occupation()
          {
            _id = p._id,
            _idAccount = p._idAccount,
            Status = p.Status,
            Name = p.Name,
            Group = new Group()
            {
              _id = p.Group._id,
              _idAccount = p.Group._idAccount,
              Status = p.Group.Status,
              Name = p.Name,
              Company = new Company()
              {
                _id = p.Group.Company._id,
                _idAccount = p.Group.Company._idAccount,
                Status = p.Group.Company.Status,
                Name = p.Group.Company.Name,
                Logo = p.Group.Company.Logo,
                Skills = p.Group.Company.Skills.OrderBy(x => x.Name).ToList(),
                Template = p.Group.Company.Template
              },
              Axis = p.Group.Axis,
              Sphere = p.Group.Sphere,
              Line = p.Group.Line,
              Skills = p.Group.Skills.OrderBy(x => x.Name).ToList(),
              Schooling = p.Group.Schooling.OrderBy(x => x.Order).ToList(),
              Scope = p.Group.Scope.OrderBy(x => x.Order).ToList(),
              Template = p.Group.Template,
              Occupations = p.Group.Occupations
            },
            Area = p.Area,
            Areas = p.Areas,
            Line = p.Line,
            Skills = p.Skills.OrderBy(x => x.Name).ToList(),
            Schooling = p.Schooling.OrderBy(x => x.Order).ToList(),
            Activities = p.Activities.OrderBy(x => x.Order).ToList(),
            Template = p.Template,
            ProcessLevelTwo = p.ProcessLevelTwo,
            SpecificRequirements = p.SpecificRequirements,
            Process = (p.Process != null) ? p.Process.OrderBy(x => x.ProcessLevelOne.Area.Name).ThenBy(x => x.ProcessLevelOne.Order).ThenBy(x => x.Order).ToList() : null
          }).FirstOrDefault();

        return occupation;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Occupation GetOccupation(string idCompany, string filterName)
    {
      try
      {
        return occupationService.GetAll(p => p.ProcessLevelTwo.ProcessLevelOne.Area.Company._id == idCompany && p.Name.ToUpper() == filterName.ToUpper()).ToList().Select(p =>
          new Occupation()
          {
            _id = p._id,
            _idAccount = p._idAccount,
            Status = p.Status,
            Name = p.Name,
            Group = new Group()
            {
              _id = p.Group._id,
              _idAccount = p.Group._idAccount,
              Status = p.Group.Status,
              Name = p.Name,
              Company = new Company()
              {
                _id = p.Group.Company._id,
                _idAccount = p.Group.Company._idAccount,
                Status = p.Group.Company.Status,
                Name = p.Group.Company.Name,
                Logo = p.Group.Company.Logo,
                Skills = p.Group.Company.Skills.OrderBy(x => x.Name).ToList(),
                Template = p.Group.Company.Template
              },
              Axis = p.Group.Axis,
              Sphere = p.Group.Sphere,
              Line = p.Group.Line,
              Skills = p.Group.Skills.OrderBy(x => x.Name).ToList(),
              Schooling = p.Group.Schooling.OrderBy(x => x.Order).ToList(),
              Scope = p.Group.Scope.OrderBy(x => x.Order).ToList(),
              Template = p.Group.Template,
              Occupations = p.Group.Occupations
            },
            Area = p.Area,
            Areas = p.Areas,
            Line = p.Line,
            Skills = p.Skills.OrderBy(x => x.Name).ToList(),
            Schooling = p.Schooling.OrderBy(x => x.Order).ToList(),
            Activities = p.Activities.OrderBy(x => x.Order).ToList(),
            Template = p.Template,
            ProcessLevelTwo = p.ProcessLevelTwo,
            SpecificRequirements = p.SpecificRequirements,
            Process = (p.Process != null) ? p.Process.OrderBy(x => x.ProcessLevelOne.Area.Name).ThenBy(x => x.ProcessLevelOne.Order).ThenBy(x => x.Order).ToList() : null
          }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Occupation> GetOccupations()
    {
      try
      {
        return occupationService.GetAll().OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Occupation> GetOccupationsInfra(ref long total, string filter, int count, int page)
    {
      try
      {
        var parameter = parameterService.GetAuthentication(p => p.Name == "Account_Resolution");
        var idresolution = "";

        if (parameter.Count() == 0)
          idresolution = DefaultParameter().Content;
        else
          idresolution = parameter.FirstOrDefault().Content;

        int skip = (count * (page - 1));
        var detail = occupationService.GetAuthentication(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = occupationService.GetAuthentication(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private async void AdjustOccuptaions()
    {
      try
      {
        var list = occupationService.GetAuthentication(p => p.Process == null).ToList();
        foreach (var item in list)
        {
          item.Process = new List<ProcessLevelTwo>();
          if (item.ProcessLevelTwo != null)
          {
            item.Process.Add(item.ProcessLevelTwo);
            occupationService.UpdateAccount(item, null);
          }
        }

        var listAreas = occupationService.GetAuthentication(p => p.Areas == null).ToList();
        foreach (var item in listAreas)
        {
          item.Areas = new List<Area>();
          if (item.Areas != null)
          {
            item.Areas.Add(item.Area);
            occupationService.UpdateAccount(item, null);
          }
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Occupation> GetOccupations(string idcompany, string idarea)
    {
      try
      {
        AdjustOccuptaions();
        var area = areaService.GetAll(p => p._id == idarea).FirstOrDefault();
        //return occupationService.GetAll(p => p.Area._id == idarea & p.Group.Company._id == idcompany).OrderBy(p => p.Name).ToList();
        var itens = occupationService.GetAll(p => p.Group.Company._id == idcompany).OrderBy(p => p.Name).ToList();
        List<Occupation> list = new List<Occupation>();
        foreach (var item in itens)
        {
          foreach (var proc in item.Process)
          {
            if (proc.ProcessLevelOne.Area != null)
              if (proc.ProcessLevelOne.Area._id == area._id)
              {
                item.ProcessLevelTwo = proc;
                list.Add(new Occupation()
                {
                  Name = item.Name,
                  Group = item.Group,
                  Area = proc.ProcessLevelOne.Area,
                  Line = item.Line,
                  Skills = item.Skills,
                  Schooling = item.Schooling,
                  Activities = item.Activities,
                  Template = item.Template,
                  ProcessLevelTwo = proc,
                  CBO = item.CBO,
                  SpecificRequirements = item.SpecificRequirements,
                  Process = item.Process,
                  _id = item._id,
                  _idAccount = item._idAccount,
                  Status = item.Status,
                  Areas = item.Areas
                });
              }
          }
        }
        return list.OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewOccupationListEdit> ListOccupationsEdit(string idcompany, ref long total, string filter, int count, int page, string filterGroup)
    {
      try
      {
        int skip = (count * (page - 1));
        var itens = occupationService.GetAll(p => p.Group.Company._id == idcompany
        & p.Name.ToUpper().Contains(filter.ToUpper())).
          Skip(skip).Take(count)
          .OrderBy(p => p.Name).ToList().Select(p => new ViewOccupationListEdit
          {
            _id = p._id,
            Name = p.Name,
            NameGroup = p.Group.Name,
            Company = p.Group.Company.Skills.Count(),
            Group = p.Group.Scope.Count(),
            Occupation = p.Activities.Count()
          });

        total = occupationService.GetAll(p => p.Group.Company._id == idcompany & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return null;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Schooling> GetSchooling()
    {
      try
      {
        return schoolingService.GetAll().OrderBy(p => p.Order).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Skill GetSkill(string filterName)
    {
      try
      {
        var detail = skillService.GetAll(p => p.Name.ToUpper() == filterName.ToUpper()).ToList();
        if (detail.Count == 1)
          return detail[0];
        detail = skillService.GetAll(p => p.Name.ToUpper().Contains(filterName.ToUpper())).ToList();
        if (detail.Count == 1)
          return detail[0];
        if (detail.Count > 1)
          throw new Exception("Mais de uma skill!");
        return null;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Skill> GetSkills(ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = skillService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = skillService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewSkills> GetSkills(string company, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var skills = (List<string>)(from comp in companyService.GetAll()
                                    where comp._id == company
                                    select new
                                    {
                                      Name = comp.Skills.Select(p => p.Name)
                                    }
                    ).FirstOrDefault().Name;


        var detail = skillService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper()))
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

        total = skillService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewSkills> GetSkillsGroup(string idgroup, string idcompany, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var skills = (List<string>)(from comp in companyService.GetAll()
                                    where comp._id == idcompany
                                    select new
                                    {
                                      Name = comp.Skills.Select(p => p.Name)
                                    }
                    ).FirstOrDefault().Name;

        var skillsGroup = (List<string>)(from groups in groupService.GetAll()
                                         where groups._id == idgroup
                                         select new
                                         {
                                           Name = groups.Skills.Select(p => p.Name)
                                         }
                   ).FirstOrDefault().Name;


        var detail = skillService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper()))
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

        total = skillService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewSkills> GetSkillsOccupation(string idgroup, string idcompany, string idoccupation, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var skills = (List<string>)(from comp in companyService.GetAll()
                                    where comp._id == idcompany
                                    select new
                                    {
                                      Name = comp.Skills.Select(p => p.Name)
                                    }
                    ).FirstOrDefault().Name;

        var skillsGroup = (List<string>)(from groups in groupService.GetAll()
                                         where groups._id == idgroup
                                         select new
                                         {
                                           Name = groups.Skills.Select(p => p.Name)
                                         }
                   ).FirstOrDefault().Name;

        var skillsOccupation = (List<string>)(from occupation in occupationService.GetAll()
                                              where occupation._id == idoccupation
                                              select new
                                              {
                                                Name = occupation.Skills.Select(p => p.Name)
                                              }
                 ).FirstOrDefault().Name;


        var detail = skillService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper()))
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

        total = skillService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Sphere> GetSpheres()
    {
      try
      {
        return sphereService.GetAll().OrderBy(p => p.TypeSphere).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Sphere> GetSpheres(string idcompany)
    {
      try
      {
        return sphereService.GetAll(p => p.Company._id == idcompany).OrderBy(p => p.TypeSphere).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Skill> GetSkillsInfra(ref long total, string filter, int count, int page)
    {
      try
      {
        var parameter = parameterService.GetAuthentication(p => p.Name == "Account_Resolution");
        var idresolution = "";

        if (parameter.Count() == 0)
          idresolution = DefaultParameter().Content;
        else
          idresolution = parameter.FirstOrDefault().Content;

        int skip = (count * (page - 1));
        var detail = skillService.GetAuthentication(p => p._idAccount == idresolution & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        total = skillService.GetAuthentication(p => p._idAccount == idresolution & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      sphereService._user = _user;
      dictionarySphereService._user = _user;
      axisService._user = _user;
      groupService._user = _user;
      occupationService._user = _user;
      areaService._user = _user;
      companyService._user = _user;
      skillService._user = _user;
      schoolingService._user = _user;
      personService._user = _user;
      processLevelOneService._user = _user;
      processLevelTwoService._user = _user;
      questionsService._user = _user;
      textDefaultService._user = _user;
    }

    public string UpdateArea(Area area)
    {
      try
      {
        areaService.Update(area, null);
        UpdateAreaAll(area);
        UpdateAreaProcessAll(area);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateAxis(Axis axis)
    {
      try
      {
        axisService.Update(axis, null);
        UpdateAxisAll(axis, false);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateCBO(CBO model)
    {
      try
      {
        cboService.UpdateAccount(model, null);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateGroup(Group group)
    {
      try
      {
        var groupOld = groupService.GetAll(p => p._id == group._id).FirstOrDefault();

        if ((groupOld.Sphere._id != group.Sphere._id) || (groupOld.Axis._id != group.Axis._id))
        {
          UpdateGroupSphereAxis(group, groupOld);
          return "update";
        }


        groupService.Update(group, null);
        UpdateGroupAll(group);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateGroupSphereAxis(Group group, Group groupOld)
    {
      try
      {
        //var groupOld = groupService.GetAll(p => p._id == group._id).FirstOrDefault();
        groupOld.Status = EnumStatus.Disabled;
        groupService.Update(groupOld, null);

        var groupnew = AddGroup(new ViewAddGroup()
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

        groupService.Update(groupnew, null);

        UpdateGroupAll(groupnew);
        UpdateGroupOccupationAll(groupnew, groupOld);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private async Task UpdateGroupOccupationAll(Group groupnew, Group groupold)
    {
      try
      {
        foreach (var item in occupationService.GetAll(p => p.Group._id == groupold._id).ToList())
        {
          item.Group = groupnew;
          occupationService.Update(item, null);
          UpdateOccupationAll(item);
        }

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateOccupation(Occupation occupation)
    {
      try
      {
        var areas = new List<Area>();
        var groupOld = occupationService.GetAll(p => p._id == occupation._id).FirstOrDefault().Group;

        foreach (var item in occupation.Process)
          areas.Add(item.ProcessLevelOne.Area);
        if (groupOld != occupation.Group)
        {
          foreach (var school in occupation.Group.Schooling)
          {
            foreach (var schoolOccupation in groupOld.Schooling)
            {
              if (school._id == schoolOccupation._id)
                school.Complement = schoolOccupation.Complement;
            }
          }

          occupation.Schooling = occupation.Group.Schooling;
        }


        occupationService.Update(occupation, null);
        UpdateOccupationAll(occupation);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateSkill(Skill skill)
    {
      try
      {
        skillService.Update(skill, null);
        UpdateSkillAll(skill, false);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateQuestions(Questions questions)
    {
      try
      {
        questionsService.Update(questions, null);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateSphere(Sphere sphere)
    {
      try
      {
        sphereService.Update(sphere, null);
        UpdateSphereAll(sphere, false);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private async Task UpdateSphereAll(Sphere sphere, bool remove)
    {
      foreach (var item in axisService.GetAll(p => p.Sphere._id == sphere._id).ToList())
      {
        if (remove == true)
          item.Sphere = null;
        else
          item.Sphere = sphere;

        this.axisService.Update(item, null);
      }

      foreach (var item in groupService.GetAll(p => p.Sphere._id == sphere._id).ToList())
      {
        if (remove == true)
          item.Sphere = null;
        else
          item.Sphere = sphere;

        this.groupService.Update(item, null);
        UpdateGroupAll(item);
      }

    }

    private async Task UpdateAxisAll(Axis axis, bool remove)
    {
      foreach (var item in groupService.GetAll(p => p.Axis._id == axis._id).ToList())
      {
        if (remove == true)
          item.Axis = null;
        else
          item.Axis = axis;

        this.groupService.Update(item, null);
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
        foreach (var item in occupationService.GetAll().ToList())
        {
          foreach (var ar in item.Areas)
          {
            if (ar != null)
            {
              if (ar._id == area._id)
              {
                item.Areas.Remove(ar);
                item.Areas.Add(area);
                this.occupationService.Update(item, null);
                UpdateOccupationAll(item);
                break;
              }
            }
          }
        }


      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private async Task UpdateAreaProcessAll(Area area)
    {
      try
      {
        foreach (var item in occupationService.GetAll().ToList())
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
                  this.occupationService.Update(item, null);
                  UpdateOccupationAll(item);
                }
              }
            }

          }
        }
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private async Task UpdateProcessLevelTwoAll(ProcessLevelTwo processLevelTwo)
    {
      try
      {
        foreach (var item in occupationService.GetAll(p => p.Areas.Contains(processLevelTwo.ProcessLevelOne.Area)).ToList())
        {
          foreach (var proc in item.Process)
          {
            if (proc._id == processLevelTwo._id)
            {
              item.Process.Remove(proc);
              item.Process.Add(processLevelTwo);
              this.occupationService.Update(item, null);
              UpdateOccupationAll(item);
              break;
            }
          }
        }
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private async Task UpdateSchoolingAll(Schooling schooling)
    {
      try
      {
        foreach (var item in groupService.GetAll().ToList())
        {
          foreach (var row in item.Schooling)
          {
            if (row._id == schooling._id)
            {
              row.Name = schooling.Name;
              row.Order = schooling.Order;
            }
          }
          this.groupService.Update(item, null);
          UpdateGroupAll(item);
        }

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateMapOccupationSchooling(string idoccupation, Schooling schooling)
    {
      try
      {
        var occupation = occupationService.GetAll(p => p._id == idoccupation).FirstOrDefault();

        var schoolOld = occupation.Schooling.Where(p => p._id == schooling._id).FirstOrDefault();
        occupation.Schooling.Remove(schoolOld);
        occupation.Schooling.Add(schooling);

        occupationService.Update(occupation, null);
        UpdateOccupationAll(occupation);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateMapOccupationActivities(string idoccupation, Activitie activitie)
    {
      try
      {
        var occupation = occupationService.GetAll(p => p._id == idoccupation).FirstOrDefault();

        var activitieOld = occupation.Activities.Where(p => p._id == activitie._id).FirstOrDefault();
        occupation.Activities.Remove(activitieOld);
        occupation.Activities.Add(activitie);

        occupationService.Update(occupation, null);
        UpdateOccupationAll(occupation);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateMapGroupScope(string idgroup, Scope scope)
    {
      try
      {
        var group = groupService.GetAll(p => p._id == idgroup).FirstOrDefault();

        var scopeOld = group.Scope.Where(p => p._id == scope._id).FirstOrDefault();
        group.Scope.Remove(scopeOld);
        group.Scope.Add(scope);

        groupService.Update(group, null);
        UpdateGroupAll(group);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateMapGroupSchooling(string idgroup, Schooling schooling)
    {
      try
      {
        var group = groupService.GetAll(p => p._id == idgroup).FirstOrDefault();

        var schoolOld = group.Schooling.Where(p => p._id == schooling._id).FirstOrDefault();
        group.Schooling.Remove(schoolOld);
        group.Schooling.Add(schooling);

        groupService.Update(group, null);
        UpdateGroupAll(group);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateSchooling(Schooling schooling)
    {
      try
      {
        schoolingService.Update(schooling, null);
        UpdateSchoolingAll(schooling);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Parameter DefaultParameter()
    {
      try
      {
        var account = new Parameter()
        {
          Name = "Account_Resolution",
          Content = "5b6852b4eb2b3849044d9cb8"
        };
        return parameterService.Insert(account);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public async Task CopyTemplateInfra(Company company)
    {
      try
      {
        var parameter = parameterService.GetAuthentication(p => p.Name == "Account_Resolution");
        var idresolution = "";

        if (parameter.Count() == 0)
          idresolution = DefaultParameter().Content;
        else
          idresolution = parameter.FirstOrDefault().Content;

        ////company
        //var companyAccount = companyService.GetAuthentication(p => p._idAccount == idresolution).FirstOrDefault();
        //company.Template = companyAccount;
        //company._idAccount = _idAccount;
        //foreach (var item in companyAccount.Skills)
        //{
        //  item._idAccount = _idAccount;
        //  company.Skills.Add(item);
        //}
        //companyService.UpdateAccount(company, null);



        //skill
        foreach (var item in skillService.GetAuthentication(p => p._idAccount == idresolution).ToList())
        {
          var skill = new Skill();
          item._idAccount = _idAccount;
          skill = item;
          skill.Template = new Skill()
          {
            _id = item._id,
            _idAccount = _idAccount,
            Name = item.Name,
            Concept = item.Concept,
            TypeSkill = item.TypeSkill,
            Status = item.Status
          };
          skillService.InsertAccount(skill);

        }

        //text default
        foreach (var item in textDefaultService.GetAuthentication(p => p._idAccount == idresolution).ToList())
        {
          var textDefault = new TextDefault();
          item._idAccount = _idAccount;
          item.Company = company;
          textDefault = item;
          textDefault.Template = new TextDefault()
          {
            _id = item._id,
            _idAccount = _idAccount,
            Company = company,
            Name = item.Name,
            TypeText = item.TypeText,
            Content = item.Content,
            Status = item.Status,
          };
          textDefaultService.InsertAccount(textDefault);

        }

        //questions
        foreach (var item in questionsService.GetAuthentication(p => p._idAccount == idresolution).ToList())
        {
          var questions = new Questions();
          item._idAccount = _idAccount;
          item.Company = company;
          questions = item;
          questions.Template = new Questions()
          {
            _id = item._id,
            _idAccount = _idAccount,
            Name = item.Name,
            Content = item.Content,
            Company = company,
            TypeRotine = item.TypeRotine,
            TypeQuestion = item.TypeQuestion,
            Status = item.Status,
            Order = item.Order
          };
          questionsService.InsertAccount(questions);

        }


        //schooling
        foreach (var item in schoolingService.GetAuthentication(p => p._idAccount == idresolution).ToList())
        {
          var schooling = new Schooling();
          schooling = item;
          schooling._idAccount = _idAccount;
          schooling.Template = new Schooling()
          {
            _id = item._id,
            _idAccount = _idAccount,
            Name = item.Name,
            Complement = item.Complement,
            Type = item.Type,
            Status = item.Status,
            Order = item.Order
          };
          schoolingService.InsertAccount(schooling);
        }

        //axis
        foreach (var itemAxis in axisService.GetAuthentication(p => p._idAccount == idresolution).ToList())
        {
          var axis = new Axis();
          axis = itemAxis;
          axis.Sphere = sphereService.GetAuthentication(p => p._idAccount == idresolution & p.TypeSphere == EnumTypeSphere.Operational).FirstOrDefault();
          axis.Sphere._idAccount = _idAccount;
          axis.Sphere.Company = company;
          axis._idAccount = _idAccount;
          axis.Template = new Axis()
          {
            _id = itemAxis._id,
            _idAccount = _idAccount,
            Name = itemAxis.Name,
            TypeAxis = itemAxis.TypeAxis,
            Status = itemAxis.Status,
            Sphere = itemAxis.Sphere
          };
          axisService.InsertAccount(axis);
        }


        //spheres
        foreach (var item in sphereService.GetAuthentication(p => p._idAccount == idresolution).ToList())
        {
          var sphere = new Sphere();
          sphere = item;
          item._idAccount = _idAccount;
          item.Company = company;
          sphere.Template = new Sphere()
          {
            _id = item._id,
            _idAccount = _idAccount,
            Name = item.Name,
            TypeSphere = item.TypeSphere,
            Status = item.Status
          };
          sphereService.InsertAccount(sphere);

          foreach (var itemaxis in axisService.GetAuthentication(p => p._idAccount == _idAccount).ToList())
          {
            //group
            foreach (var itemGroup in groupService.GetAuthentication(p => p._idAccount == idresolution & p.Axis._id == itemaxis.Template._id & p.Sphere._id == sphere.Template._id).ToList())
            {
              var group = new Group();
              group = itemGroup;
              group.Sphere = sphere;
              group.Axis = itemaxis;
              group.Company = company;
              group._idAccount = _idAccount;
              if (group.Skills != null)
              {
                foreach (var skill in group.Skills)
                {
                  skill._idAccount = _idAccount;

                }
              }
              group.Template = new Group()
              {
                _id = itemGroup._id,
                _idAccount = _idAccount,
                Name = itemGroup.Name,
                Company = itemGroup.Company,
                Axis = itemGroup.Axis,
                Schooling = itemGroup.Schooling,
                Line = itemGroup.Line,
                Scope = itemGroup.Scope,
                Sphere = itemGroup.Sphere,
                Skills = itemGroup.Skills,
                Status = itemGroup.Status,
              };
              groupService.InsertAccount(group);
            }
          }

        }


        ////area
        //foreach (var item in areaService.GetAuthentication(p => p._idAccount == idresolution).ToList())
        //{
        //  var area = new Area();
        //  item._idAccount = _idAccount;
        //  item.Company = company;
        //  area = item;
        //  area.Template = new Area()
        //  {
        //    _id = item._id,
        //    _idAccount = _idAccount,
        //    Name = item.Name,
        //    Order = item.Order,
        //    Status = item.Status
        //  };
        //  areaService.InsertAccount(item);

        //}




        ////occupation
        //foreach (var item in occupationService.GetAuthentication(p => p._idAccount == idresolution).ToList())
        //{
        //  var occupation = new Occupation();
        //  item._idAccount = _idAccount;
        //  item.Group.Company = company;
        //  occupation = item;
        //  foreach (var skill in occupation.Skills)
        //  {
        //    occupation.Skills.Remove(skill);
        //    skill._idAccount = _idAccount;
        //    occupation.Skills.Add(skill);
        //  }
        //  occupation.Template = new Occupation()
        //  {
        //    _id = item._id,
        //    _idAccount = _idAccount,
        //    Name = item.Name,
        //    Group = item.Group,
        //    Schooling = item.Schooling,
        //    Line = item.Line,
        //    Activities = item.Activities,
        //    Skills = item.Skills,
        //    Status = item.Status,
        //    Area = item.Area
        //  };
        //  occupationService.InsertAccount(occupation);

        //}


      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AreaOrder(string idcompany, string idarea, long order, bool sum)
    {
      try
      {
        var area = areaService.GetAll(p => p._id == idarea).FirstOrDefault();
        var areas = areaService.GetAll(p => p.Company._id == idcompany).ToList();

        return "reorder";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddProcessLevelOne(ProcessLevelOne model)
    {
      try
      {
        try
        {
          var order = processLevelOneService.GetAll(p => p.Area._id == model.Area._id).Max(p => p.Order) + 1;
          model.Order = order;
        }
        catch (Exception)
        {
          model.Order = 1;
        }


        model.Process = new List<ProcessLevelTwo>();
        processLevelOneService.Insert(model);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddTextDefault(TextDefault model)
    {
      try
      {
        textDefaultService.Insert(model);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddProcessLevelTwo(ProcessLevelTwo model)
    {
      try
      {
        try
        {
          var order = processLevelTwoService.GetAll(p => p.ProcessLevelOne._id == model.ProcessLevelOne._id).Max(p => p.Order) + 1;
          model.Order = order;
        }
        catch (Exception)
        {
          model.Order = 1;
        }

        processLevelTwoService.Insert(model);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteProcessLevelOne(string id)
    {
      try
      {
        if (processLevelTwoService.GetAll(p => p.ProcessLevelOne._id == id).Count() > 0)
          return "error_leveltwo_exists";

        var item = processLevelOneService.GetAll(p => p._id == id).FirstOrDefault();
        item.Status = EnumStatus.Disabled;
        processLevelOneService.Update(item, null);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteProcessLevelTwo(string id)
    {
      try
      {
        //if (occupationService.GetAll(p => p.ProcessLevelTwo._id == id).Count() > 0)
        //return "error_occupation_exists";
        var item = processLevelTwoService.GetAll(p => p._id == id).FirstOrDefault();

        if (occupationService.GetAll(p => p.Process.Contains(item)).Count() > 0)
          return "error_occupation_exists";



        item.Status = EnumStatus.Disabled;
        processLevelTwoService.Update(item, null);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateProcessLevelOne(ProcessLevelOne model)
    {
      try
      {
        processLevelOneService.Update(model, null);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateProcessLevelTwo(ProcessLevelTwo model)
    {
      try
      {
        processLevelTwoService.Update(model, null);
        UpdateProcessLevelTwoAll(model);
        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public ProcessLevelTwo GetProcessLevelTwo(string id)
    {
      try
      {
        var detail = processLevelTwoService.GetAll(p => p._id == id).ToList();
        if (detail.Count == 1)
          return detail[0];
        return null;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ProcessLevelTwo> GetProcessLevelTwo()
    {
      try
      {
        //var result = processLevelOneService.GetAll(p => p.Area._id == idarea);
        var result = processLevelOneService.GetAll().OrderBy(p => p.Order).ToList();
        var list = new List<ProcessLevelTwo>();
        foreach (var item in result)
        {
          foreach (var row in processLevelTwoService.GetAll(p => p.ProcessLevelOne._id == item._id).OrderBy(p => p.Order).ToList())
          {
            list.Add(row);
          }
        }
        return list.OrderBy(p => p.ProcessLevelOne.Area.Name).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddSkills(List<ViewAddSkill> view)
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
          skillService.Insert(skill);
        }

        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string ReorderGroupScope(string idcompany, string idgroup, string idscope, bool sum)
    {
      try
      {
        var group = groupService.GetAll(p => p.Company._id == idcompany & p._id == idgroup).FirstOrDefault();
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

        groupService.Update(group, null);
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
        var occupation = occupationService.GetAll(p => p.Group.Company._id == idcompany & p._id == idoccupation).FirstOrDefault();
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

        occupationService.Update(occupation, null);

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
        var group = groupService.GetAll(p => p.Company._id == idcompany & p._id == idgroup).FirstOrDefault();
        var scope = group.Scope.Where(p => p._id == idscope).FirstOrDefault();

        foreach (var item in group.Scope)
        {
          if (item._id == scope._id)
            item.Order = order;
        }

        groupService.Update(group, null);
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
        var occupation = occupationService.GetAll(p => p.Group.Company._id == idcompany & p._id == idoccupation).FirstOrDefault();
        var activities = occupation.Activities.Where(p => p._id == idactivitie).FirstOrDefault();

        foreach (var item in occupation.Activities)
        {
          if (item._id == activities._id)
            item.Order = order;
        }
        occupationService.Update(occupation, null);

        UpdateOccupationAll(occupation);
        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public TextDefault GetTextDefault(string idcompany, string name)
    {
      try
      {
        return textDefaultService.GetAll(p => p.Company._id == idcompany & p.Name == name).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public TextDefault GetTextDefault(string id)
    {
      try
      {
        return textDefaultService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<TextDefault> ListTextDefault(string idcompany)
    {
      try
      {
        return textDefaultService.GetAll(p => p.Company._id == idcompany).ToList();
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
        var textDefault = textDefaultService.GetAll(p => p._id == id).FirstOrDefault();
        textDefault.Status = EnumStatus.Disabled;
        textDefaultService.Update(textDefault, null);
        return "delete";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateTextDefault(TextDefault textDefault)
    {
      try
      {
        textDefaultService.Update(textDefault, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string[] GetCSVCompareGroup(string idcompany)
    {
      try
      {

        var groups = groupService.GetAll(p => p.Company._id == idcompany).OrderBy(p => p.Sphere.TypeSphere).ThenBy(p => p.Axis.TypeAxis).ToList();

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
        //List<string[]> listFinal = new List<string[]>();

        long line = 0;
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

            var result = new ViewCSVLO();
            result.Name = scope.Name;
            result.Line = line;
            result.Type = EnumTypeLO.Scope;

            list.Add(result);
            line += 1;
          }

          line = 0;
          foreach (var skill in item.Skills)
          {
            if (line > maxLineSkill)
              maxLineSkill = line;

            var result = new ViewCSVLO();
            result.Name = skill.Name + ":" + skill.Concept;
            result.Line = line;
            result.Type = EnumTypeLO.Skill;

            list.Add(result);
            line += 1;
          }

          line = 0;
          foreach (var scholling in item.Schooling)
          {
            if (line > maxLineSchooling)
              maxLineSchooling = line = 0; ;

            var result = new ViewCSVLO();
            result.Name = scholling.Name;
            result.Line = line;
            result.Type = EnumTypeLO.Schooling;

            list.Add(result);
            line += 1;
          }


        }

        for (var row = 0; row <= maxLine; row++)
        {
          var itemView = string.Empty;
          foreach (var item in list.Where(p => p.Type == EnumTypeLO.Scope & p.Line == row).ToList())
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

        for (var row = 0; row <= maxLineSkill; row++)
        {
          var itemView = string.Empty;
          foreach (var item in list.Where(p => p.Type == EnumTypeLO.Skill & p.Line == row).ToList())
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
          foreach (var item in list.Where(p => p.Type == EnumTypeLO.Schooling & p.Line == row).ToList())
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

        System.IO.File.WriteAllLines("LO.csv", rel);

        return rel;
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


  }
#pragma warning restore 1998
#pragma warning restore 4014
}
