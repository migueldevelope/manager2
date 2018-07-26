using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

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
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddArea(ViewAddArea view)
    {
      try
      {
        var area = new Area()
        {
          Name = view.Name,
          Company = view.Company,
          Order = view.Order,
          Status = EnumStatus.Enabled
        };
        areaService.Insert(area);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddAxis(ViewAddAxis view)
    {
      try
      {
        var axis = new Axis()
        {
          Name = view.Name,
          Sphere = view.Sphere,
          TypeAxis = view.Type
        };
        axisService.Insert(axis);
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
        view.Company.Skills.Add(view.Skill);
        companyService.Update(view.Company, null);
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
          Scope = new List<string>()
        };
        groupService.Insert(group);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private async void UpdateSkillGroup()
    {

    }

    private async void UpdateSkillEssential()
    {

    }

    public string AddMapGroupSchooling(ViewAddMapGroupSchooling view)
    {
      try
      {
        view.Group.Schooling.Add(view.Schooling);
        groupService.Update(view.Group, null);
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
        view.Group.Scope.Add(view.Scope);
        groupService.Update(view.Group, null);
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
        view.Group.Skills.Add(view.Skill);
        groupService.Update(view.Group, null);
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
        var item = occupationService.GetAll(p => p.Line == view.Line).Count();
        if (item > 0)
          return "error_line";

        var occupation = new Occupation()
        {
          Name = view.Name,
          Group = view.Group,
          Area = view.Area,
          Line = view.Line,
          Status = EnumStatus.Enabled,
          Activities = new List<string>(),
          Schooling = new List<Schooling>(),
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

        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddSchooling(ViewAddOccupationSchooling view)
    {
      try
      {

        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddSkill(ViewAddSkill view)
    {
      try
      {
        var skill = new Skill()
        {
          Name = view.Name,
          Concept = view.Concept,
          TypeSkill = view.Type,
          Status = EnumStatus.Enabled
        };
        skillService.Insert(skill);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddSphere(ViewAddSphere view)
    {
      try
      {
        var sphere = new Sphere()
        {
          Name = view.Name,
          Company = view.Company,
          Status = EnumStatus.Enabled
        };
        sphereService.Insert(sphere);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteArea(Area area)
    {
      try
      {

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteAxis(Axis axis)
    {
      try
      {

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteEssential(Company company, string id)
    {
      try
      {

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteGroup(Group group)
    {
      try
      {

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteMapGroupSchooling(Group group, string id)
    {
      try
      {

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteMapGroupSkill(Group group, string id)
    {
      try
      {

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteOccupation(Occupation occupation)
    {
      try
      {

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteOccupationActivities(Occupation occupation, string activitie)
    {
      try
      {

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteOccupationSkill(Occupation occupation, string id)
    {
      try
      {

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteSchooling(Occupation occupation, string id)
    {
      try
      {

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteSkill(Skill skill)
    {
      try
      {

        return "delete";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteSphere(Sphere sphere)
    {
      try
      {

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
        return groupService.GetAll(p => p._id == id).FirstOrDefault();
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
        return groupService.GetAll().ToList();
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
    }

    public string UpdateArea(Area area)
    {
      try
      {

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

        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateEssential(ViewAddEssential view)
    {
      try
      {

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

        return "update";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
  }
}
