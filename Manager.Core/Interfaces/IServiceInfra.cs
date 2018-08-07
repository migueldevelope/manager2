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
    List<Skill> GetSkillsInfra(ref long total, string filter, int count, int page);
    List<Skill> GetSkills(ref long total, string filter, int count, int page);
    List<ViewSkills> GetSkills(string company, ref long total, string filter, int count, int page);
    List<ViewSkills> GetSkillsGroup(string idgroup, string idcompany, ref long total, string filter, int count, int page);
    List<ViewSkills> GetSkillsOccupation(string idgroup, string idcompany, string idoccupation, ref long total, string filter, int count, int page);
    List<Schooling> GetSchooling();
    List<Sphere> GetSpheres();
    List<Sphere> GetSpheres(string idcompany);
    List<Axis> GetAxis();
    List<Axis> GetAxis(string idcompany);
    List<Area> GetAreas();
    List<Area> GetAreas(string idcompany);
    Group GetGroup(string id);
    Occupation GetOccupation(string id);
    List<Group> GetGroups();
    List<Group> GetGroups(string idcompany);
    List<Occupation> GetOccupations();
    List<Occupation> GetOccupationsInfra(ref long total, string filter, int count, int page);
    List<Occupation> GetOccupations(string idcompany);
    string AddEssential(ViewAddEssential view);
    Skill AddSkill(ViewAddSkill view);
    Schooling AddSchooling(Schooling schooling);
    string AddAxis(Axis view);
    string AddGroup(ViewAddGroup view);
    string AddMapGroupSkill(ViewAddMapGroupSkill view);
    string AddMapGroupScope(ViewAddMapGroupScope view);
    string AddMapGroupSchooling(ViewAddMapGroupSchooling view);
    string AddArea(Area view);
    string AddSphere(Sphere view);
    string AddOccupation(ViewAddOccupation view);
    string AddOccupationSkill(ViewAddOccupationSkill view);
    string AddOccupationActivities(ViewAddOccupationActivities view);
    string AddProcessLevelOne(ProcessLevelOne model);
    string AddProcessLevelTwo(ProcessLevelTwo model);
    string DeleteEssential(string idcompany, string id);
    string DeleteSkill(string idskill);
    string DeleteSphere(string idsphere);
    string DeleteAxis(string idaxis);
    string DeleteGroup(string idgroup);
    string DeleteMapGroupSkill(string idgroup, string id);
    string DeleteMapGroupSchooling(string idgroup, string id);
    string DeleteArea(string idarea);
    string DeleteOccupation(string idoccupation);
    string DeleteOccupationSkill(string idoccupation, string id);
    string DeleteOccupationActivities(string idoccupation, string idactivitie);
    string DeleteMapGroupScope(string idgroup, string idscope);
    string DeleteSchooling(string idschooling);
    string DeleteProcessLevelOne(string id);
    string DeleteProcessLevelTwo(string id);
    string UpdateSkill(Skill skill);
    string UpdateSphere(Sphere sphere);
    string UpdateAxis(Axis axis);
    string UpdateGroup(Group group);
    string UpdateArea(Area area);
    string UpdateOccupation(Occupation occupation);
    string UpdateMapGroupSchooling(string idgroup, Schooling schooling);
    string UpdateMapOccupationSchooling(string idoccupation, Schooling schooling);
    string UpdateMapOccupationActivities(string idoccupation, Activitie activitie);
    string UpdateSchooling(Schooling schooling);
    string UpdateProcessLevelOne(ProcessLevelOne model);
    string UpdateProcessLevelTwo(ProcessLevelTwo model);
    string AreaOrder(string idcompany, string idarea, long order, bool sum);
  }
}
