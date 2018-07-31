﻿using Manager.Core.Business;
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
        var item = areaService.GetAll(p => p.Order == view.Order).Count();
        if (item > 0)
          return "error_line";


        areaService.Insert(view);
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

    public string AddGroup(ViewAddGroup view)
    {
      try
      {
        var item = groupService.GetAll(p => p.Line == view.Line).Count();
        if (item > 0)
          return "error_line";

        var group = new Group()
        {
          Name = view.Name,
          Axis = view.Axis,
          Sphere = view.Sphere,
          Status = EnumStatus.Enabled,
          Skills = new List<Skill>(),
          Schooling = new List<Schooling>(),
          Line = view.Line,
          Company = view.Company,
          Scope = new List<Scope>()
        };
        groupService.Insert(group);
        return "ok";
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
        view.Group.Schooling.Add(AddSchooling(view.Schooling));
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
        view.Scope._id = ObjectId.GenerateNewId().ToString();
        view.Scope._idAccount = view.Group._idAccount;
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

    public string AddOccupation(ViewAddOccupation view)
    {
      try
      {
        //var item = occupationService.GetAll(p => p.Line == view.Line).Count();
        //if (item > 0)
        //  return "error_line";

        var occupation = new Occupation()
        {
          Name = view.Name,
          Group = view.Group,
          Area = view.Area,
          Line = view.Line,
          Status = EnumStatus.Enabled,
          Activities = new List<Activitie>(),
          Schooling = view.Group.Schooling,
          Skills = new List<Skill>()
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

    public string DeleteArea(string idarea)
    {
      try
      {
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
        foreach (var item in personService.GetAll(p => p.Company._id == company._id).ToList())
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
          item.Group = group;
          item.Schooling = group.Schooling;
          occupationService.Update(item, null);
        }


        foreach (var item in personService.GetAll(p => p.Occupation.Group._id == group._id).ToList())
        {
          item.Occupation.Group = group;
          item.Occupation.Schooling = group.Schooling;
          personService.Update(item, null);
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
        foreach (var item in personService.GetAll(p => p.Occupation._id == occupation._id).ToList())
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

        foreach (var item in personService.GetAll(p => p.Occupation.Group._id == group._id).ToList())
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

        foreach (var item in personService.GetAll(p => p.Occupation._id == occupation._id).ToList())
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

    public List<Area> GetAreas()
    {
      try
      {
        return areaService.GetAll().ToList();
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
        return areaService.GetAll(p => p.Company._id == idcompany).ToList();
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
        return axisService.GetAll().ToList();
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
        return axisService.GetAll(p => p.Sphere.Company._id == idcompany).ToList();
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
        return companyService.GetAll().ToList();
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
        var group = groupService.GetAll(p => p._id == id).FirstOrDefault();
        group.Occupations = occupationService.GetAll(p => p.Group._id == group._id).ToList();
        return group;
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
          item.Occupations = occupationService.GetAll(p => p.Group._id == item._id).ToList();
          groups.Add(item);
        }
        return groups;
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
        return groups;
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
        return occupationService.GetAll(p => p._id == id).FirstOrDefault();
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
        return occupationService.GetAll().ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Occupation> GetOccupations(string idcompany)
    {
      try
      {
        return occupationService.GetAll(p => p.Group.Company._id == idcompany).ToList();
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
        return schoolingService.GetAll().ToList();
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
        var detail = skillService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();
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
                      }).ToList();

        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();
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
                      }).ToList();

        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();
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
        return sphereService.GetAll().ToList();
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
        return sphereService.GetAll(p => p.Company._id == idcompany).ToList();
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
    }

    public string UpdateArea(Area area)
    {
      try
      {
        areaService.Update(area, null);
        UpdateAreaAll(area);
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

    public string UpdateGroup(Group group)
    {
      try
      {
        groupService.Update(group, null);
        UpdateGroupAll(group);
        return "update";
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
      foreach (var item in groupService.GetAll(p => p.Sphere._id == axis._id).ToList())
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
        foreach (var item in occupationService.GetAll(p => p.Area._id == area._id).ToList())
        {
          item.Area = area;
          this.occupationService.Update(item, null);
          UpdateOccupationAll(item);
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
                      }).ToList();

        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();
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
          Content = "5b5b76fdfaea7c328012e9f2"
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
        var id = "";

        if (parameter.Count() == 0)
          id = DefaultParameter().Content;
        else
          id = parameter.FirstOrDefault().Content;

        //company
        var companyAccount = companyService.GetAuthentication(p => p._idAccount == id).FirstOrDefault();
        company.Template = companyAccount;
        company._idAccount = _idAccount;
        foreach (var item in companyAccount.Skills)
        {
          item._idAccount = _idAccount;
          company.Skills.Add(item);
        }
        companyService.Update(company, null);



        //skill
        foreach (var item in skillService.GetAuthentication(p => p._idAccount == id).ToList())
        {
          var skill = new Skill();
          item._idAccount = _idAccount;
          skill = item;
          skillService.Insert(skill);

          skill.Template = item;
          skillService.Update(skill, null);
        }

        //spheres
        foreach (var item in sphereService.GetAuthentication(p => p._idAccount == id).ToList())
        {
          var sphere = new Sphere();
          item._idAccount = _idAccount;
          item.Company = company;
          sphere = item;
          sphereService.Insert(item);

          sphere.Template = item;
          sphereService.Update(sphere, null);
        }

        //axis
        foreach (var item in axisService.GetAuthentication(p => p._idAccount == id).ToList())
        {
          var axis = new Axis();
          item._idAccount = _idAccount;
          item.Sphere.Company = company;
          item.Sphere._idAccount = _idAccount;
          axis = item;
          axisService.Insert(item);

          axis.Template = item;
          axisService.Update(axis, null);
        }

        //area
        foreach (var item in areaService.GetAuthentication(p => p._idAccount == id).ToList())
        {
          var area = new Area();
          item._idAccount = _idAccount;
          item.Company = company;
          area = item;
          area.Template = item;
          areaService.Insert(item);

          item.Template = item;
          areaService.Update(item, null);
        }

        //group
        foreach (var item in groupService.GetAuthentication(p => p._idAccount == id).ToList())
        {
          var group = new Group();
          item._idAccount = _idAccount;
          item.Company = company;
          foreach (var skill in group.Skills)
          {
            group.Skills.Remove(skill);
            skill._idAccount = _idAccount;
            group.Skills.Add(skill);

          }
          group = item;
          groupService.Insert(item);

          group.Template = item;
          groupService.Update(group, null);
        }

        //schooling
        foreach (var item in schoolingService.GetAuthentication(p => p._idAccount == id).ToList())
        {
          var schooling = new Schooling();
          item._idAccount = _idAccount;
          schooling = item;
          schoolingService.Insert(schooling);

          schooling.Template = item;
          schoolingService.Update(schooling, null);
        }

        //occupation
        foreach (var item in occupationService.GetAuthentication(p => p._idAccount == id).ToList())
        {
          var occupation = new Occupation();
          item._idAccount = _idAccount;
          item.Group.Company = company;
          occupation = item;
          foreach (var skill in occupation.Skills)
          {
            occupation.Skills.Remove(skill);
            skill._idAccount = _idAccount;
            occupation.Skills.Add(skill);
          }
          occupationService.Insert(occupation);

          occupation.Template = item;
          occupationService.Update(occupation, null);
        }


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
  }
}
