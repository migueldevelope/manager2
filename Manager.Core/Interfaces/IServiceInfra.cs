using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceInfra
  {

    #region Old
    void SetUser(IHttpContextAccessor contextAccessor);
    List<Company> GetCompanies();
    Skill GetSkill(string filterName);
    //List<Skill> GetSkillsInfra(ref long total, string filter, int count, int page);
    List<Skill> GetSkills(ref long total, string filter, int count, int page);
    List<ViewSkills> GetSkills(string company, ref long total, string filter, int count, int page);
    List<ViewSkills> GetSkillsGroup(string idgroup, string idcompany, ref long total, string filter, int count, int page);
    List<ViewSkills> GetSkillsOccupation(string idgroup, string idcompany, string idoccupation, ref long total, string filter, int count, int page);
    List<Schooling> GetSchooling();
    List<Sphere> GetSpheres();
    List<Sphere> GetSpheres(string idcompany);
    List<Axis> GetAxis();
    List<Cbo> ListCBO();
    Cbo GetCBO(string id);
    List<Axis> GetAxis(string idcompany);
    List<Area> GetAreas();
    List<Area> GetAreas(string idcompany);
    List<Questions> ListQuestions(string idcompany);
    Questions GetQuestions(string id);
    Group GetGroup(string id);
    Group GetGroup(string idcompany, string filterName);
    Occupation GetOccupation(string id);
    Occupation GetOccupation(string idcompany, string filterName);
    List<Group> GetGroups();
    List<ViewGroupList> GetGroups(string idcompany);
    List<Occupation> GetOccupations();
    TextDefault GetTextDefault(string idcompany, string name);
    TextDefault GetTextDefault(string id);
    List<TextDefault> ListTextDefault(string idcompany);
    //List<Occupation> GetOccupationsInfra(ref long total, string filter, int count, int page);
    List<Occupation> GetOccupations(string idcompany, string idarea);
    //List<ProcessLevelTwo> GetProcessLevelTwo(string idarea);
    ProcessLevelTwo GetProcessLevelTwo(string id);
    List<ProcessLevelTwo> GetProcessLevelTwo();
    List<ProcessLevelTwo> GetProcessLevelTwoFilter(string idarea);
    string AddTextDefault(TextDefault model);
    string AddCBO(Cbo model);
    string AddEssential(ViewAddEssential view);
    Skill AddSkill(ViewAddSkill view);
    string AddSkills(List<ViewAddSkill> view);
    Schooling AddSchooling(Schooling schooling);
    string AddAxis(Axis view);
    Group AddGroup(ViewAddGroup view);
    string AddMapGroupSkill(ViewAddMapGroupSkill view);
    string AddMapGroupScope(ViewAddMapGroupScope view);
    string AddMapGroupSchooling(ViewAddMapGroupSchooling view);
    string AddArea(Area view);
    string AddSphere(Sphere view);
    string AddOccupation(ViewAddOccupation view);
    string AddOccupation(Occupation occupation);
    string AddOccupationSkill(ViewAddOccupationSkill view);
    string AddOccupationActivities(ViewAddOccupationActivities view);
    string AddOccupationActivitiesList(List<ViewAddOccupationActivities> list);
    string AddProcessLevelOne(ProcessLevelOne model);
    string AddProcessLevelTwo(ProcessLevelTwo model);
    string AddQuestions(Questions view);
    string AddSpecificRequirements(string idoccupation, ViewAddSpecificRequirements view);
    string DeleteEssential(string idcompany, string id);
    string DeleteSkill(string idskill);
    string DeleteSphere(string idsphere);
    string DeleteAxis(string idaxis);
    string DeleteGroup(string idgroup);
    string DeleteMapGroupSkill(string idgroup, string id);
    string DeleteMapGroupSchooling(string idgroup, string id);
    string DeleteArea(string idarea);
    string DeleteQuestion(string idquestion);
    string DeleteOccupation(string idoccupation);
    string DeleteOccupationSkill(string idoccupation, string id);
    string DeleteOccupationActivities(string idoccupation, string idactivitie);
    string DeleteMapGroupScope(string idgroup, string idscope);
    string DeleteSchooling(string idschooling);
    string DeleteProcessLevelOne(string id);
    string DeleteProcessLevelTwo(string id);
    string DeleteTextDefault(string id);
    string DeleteCBO(string id);
    string UpdateTextDefault(TextDefault textDefault);
    string UpdateSkill(Skill skill);
    string UpdateSphere(Sphere sphere);
    string UpdateAxis(Axis axis);
    string UpdateQuestions(Questions questions);
    string UpdateGroup(Group group);
    string UpdateMapGroupScope(string idgroup, Scope scope);
    string UpdateArea(Area area);
    string UpdateOccupation(Occupation occupation);
    string UpdateMapGroupSchooling(string idgroup, Schooling schooling);
    string UpdateMapOccupationSchooling(string idoccupation, Schooling schooling);
    string UpdateMapOccupationActivities(string idoccupation, Activitie activitie);
    string UpdateSchooling(Schooling schooling);
    string UpdateProcessLevelOne(ProcessLevelOne model);
    string UpdateProcessLevelTwo(ProcessLevelTwo model);
    string UpdateCBO(Cbo model);
    string AreaOrder(string idcompany, string idarea, long order, bool sum);
    string ReorderGroupScope(string idcompany, string idgroup, string idscope, bool sum);
    string ReorderOccupationActivitie(string idcompany, string idoccupation, string idactivitie, bool sum);

    string ReorderGroupScopeManual(string idcompany, string idgroup, string idscope, long order);
    string ReorderOccupationActivitieManual(string idcompany, string idoccupation, string idactivitie, long order);
    List<Group> GetGroupsPrint(string idcompany);

    string GetCSVCompareGroup(string idcompany, string link);
    List<ViewOccupationListEdit> ListOccupationsEdit(string idcompany, string idarea, ref long total, string filter, int count, int page, string filterGroup);
    List<Course> GetCourseOccupation(string idoccuation, EnumTypeMandatoryTraining type);
    #endregion

  }
}
