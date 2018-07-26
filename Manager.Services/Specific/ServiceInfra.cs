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
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AddArea(ViewAddArea view)
    {
      throw new NotImplementedException();
    }

    public string AddAxis(ViewAddAxis view)
    {
      throw new NotImplementedException();
    }

    public string AddEssential(ViewAddEssential view)
    {
      throw new NotImplementedException();
    }

    public string AddGroup(ViewAddGroup view)
    {
      throw new NotImplementedException();
    }

    public string AddMapGroupSchooling(ViewAddMapGroupSchooling view)
    {
      throw new NotImplementedException();
    }

    public string AddMapGroupScope(ViewAddMapGroupScope view)
    {
      throw new NotImplementedException();
    }

    public string AddMapGroupSkill(ViewAddMapGroupSkill view)
    {
      throw new NotImplementedException();
    }

    public string AddOccupation(ViewAddOccupation view)
    {
      throw new NotImplementedException();
    }

    public string AddOccupationActivities(ViewAddOccupationActivities view)
    {
      throw new NotImplementedException();
    }

    public string AddOccupationSkill(ViewAddOccupationSkill view)
    {
      throw new NotImplementedException();
    }

    public string AddSchooling(ViewAddOccupationSchooling view)
    {
      throw new NotImplementedException();
    }

    public string AddSkill(ViewAddSkill view)
    {
      throw new NotImplementedException();
    }

    public string AddSphere(ViewAddSphere view)
    {
      throw new NotImplementedException();
    }

    public string DeleteArea(Area area)
    {
      throw new NotImplementedException();
    }

    public string DeleteAxis(Axis axis)
    {
      throw new NotImplementedException();
    }

    public string DeleteEssential(Company company, string id)
    {
      throw new NotImplementedException();
    }

    public string DeleteGroup(Group group)
    {
      throw new NotImplementedException();
    }

    public string DeleteMapGroupSchooling(Group group, string id)
    {
      throw new NotImplementedException();
    }

    public string DeleteMapGroupSkill(Group group, string id)
    {
      throw new NotImplementedException();
    }

    public string DeleteOccupation(Occupation occupation)
    {
      throw new NotImplementedException();
    }

    public string DeleteOccupationActivities(Occupation occupation, string activitie)
    {
      throw new NotImplementedException();
    }

    public string DeleteOccupationSkill(Occupation occupation, string id)
    {
      throw new NotImplementedException();
    }

    public string DeleteSchooling(Occupation occupation, string id)
    {
      throw new NotImplementedException();
    }

    public string DeleteSkill(Skill skill)
    {
      throw new NotImplementedException();
    }

    public string DeleteSphere(Sphere sphere)
    {
      throw new NotImplementedException();
    }

    public List<Area> GetAreas()
    {
      throw new NotImplementedException();
    }

    public List<Axis> GetAxis()
    {
      throw new NotImplementedException();
    }

    public List<Company> GetCompanies()
    {
      throw new NotImplementedException();
    }

    public Group GetGroup(string id)
    {
      throw new NotImplementedException();
    }

    public List<Group> GetGroups()
    {
      throw new NotImplementedException();
    }

    public Occupation GetOccupation(string id)
    {
      throw new NotImplementedException();
    }

    public List<Occupation> GetOccupations()
    {
      throw new NotImplementedException();
    }

    public List<Schooling> GetSchooling()
    {
      throw new NotImplementedException();
    }

    public List<Skill> GetSkills(ref long total, string filter, int count, int page)
    {
      throw new NotImplementedException();
    }

    public List<Sphere> GetSpheres()
    {
      throw new NotImplementedException();
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
    }

    public string UpdateArea(Area area)
    {
      throw new NotImplementedException();
    }

    public string UpdateAxis(Axis axis)
    {
      throw new NotImplementedException();
    }

    public string UpdateEssential(ViewAddEssential view)
    {
      throw new NotImplementedException();
    }

    public string UpdateGroup(Group group)
    {
      throw new NotImplementedException();
    }

    public string UpdateOccupation(Occupation occupation)
    {
      throw new NotImplementedException();
    }

    public string UpdateSkill(Skill skill)
    {
      throw new NotImplementedException();
    }

    public string UpdateSphere(Sphere sphere)
    {
      throw new NotImplementedException();
    }
  }
}
