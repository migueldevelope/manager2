using Manager.Core.Business;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceInfra
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    List<Company> GetCompanies();
    List<Skill> GetSkills(ref long total, string filter, int count, int page);
    List<Schooling> GetSchooling();
    List<Sphere> GetSpheres();
    List<Axis> GetAxis();
    List<Area> GetAreas();
    Group GetGroup(string id);
    Occupation GetOccupation(string id);
    List<Group> GetGroups();
    List<Occupation> GetOccupations();
    string AddEssential(ViewAddEssential view);
    string AddSkill(ViewAddSkill view);
    string AddSphere(ViewAddSphere view);
    string AddAxis(ViewAddAxis view);
    string AddGroup(ViewAddGroup view);
    string AddMapGroupSkill(ViewAddMapGroupSkill view);
    string AddMapGroupScope(ViewAddMapGroupScope view);
    string AddMapGroupSchooling(ViewAddMapGroupSchooling view);
    string AddArea(ViewAddArea view);
    string AddOccupation(ViewAddOccupation view);
    string AddOccupationSkill(ViewAddOccupationSkill view);
    string AddOccupationActivities(ViewAddOccupationActivities view);
    string AddSchooling(ViewAddOccupationSchooling view);
    string DeleteEssential(Company company, string id);
    string DeleteSkill(Skill skill);
    string DeleteSphere(Sphere sphere);
    string DeleteAxis(Axis axis);
    string DeleteGroup(Group group);
    string DeleteMapGroupSkill(Group group, string id);
    string DeleteMapGroupSchooling(Group group, string id);
    string DeleteArea(Area area);
    string DeleteOccupation(Occupation occupation);
    string DeleteOccupationSkill(Occupation occupation, string id);
    string DeleteOccupationActivities(Occupation occupation, string activitie);
    string DeleteSchooling(Occupation occupation, string id);
    string UpdateEssential(ViewAddEssential view);
    string UpdateSkill(Skill skill);
    string UpdateSphere(Sphere sphere);
    string UpdateAxis(Axis axis);
    string UpdateGroup(Group group);
    string UpdateArea(Area area);
    string UpdateOccupation(Occupation occupation);
  }
}
