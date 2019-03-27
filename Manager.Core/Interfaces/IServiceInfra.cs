using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceInfra
  {

    #region Infra
    ViewCrudAxis GetAxisById(string id);
    ViewCrudSphere GetSphereById(string id);
    ViewCrudSkill GetSkillById(string id);
    ViewCrudSchooling GetSchoolingById(string id);
    ViewCrudGroup GetGroup(string id);
    ViewCrudGroup GetGroup(string idcompany, string filterName);
    ViewCrudOccupation GetOccupation(string id);
    ViewCrudOccupation GetOccupation(string idcompany, string filterName);
    List<ViewListGroup> GetGroups();
    List<ViewListOccupation> GetOccupations();
    ViewCrudTextDefault GetTextDefault(string idcompany, string name);
    ViewCrudTextDefault GetTextDefault(string id);
    List<ViewListTextDefault> ListTextDefault(string idcompany);

    List<ViewListGroup> GetGroupsPrint(string idcompany);
    List<ViewListCourse> GetCourseOccupation(string idoccuation, EnumTypeMandatoryTraining type);

    List<ViewGetOccupation> GetOccupations(string idcompany, string idarea);


    List<ViewListSchooling> GetSchooling();
    List<ViewListSphere> GetSpheres();
    List<ViewListSphere> GetSpheres(string idcompany);
    List<ViewListAxis> GetAxis();
    List<ViewListCbo> ListCBO();
    ViewCrudCbo GetCBO(string id);
    List<ViewListAxis> GetAxis(string idcompany);
    List<ViewListArea> GetAreas();
    List<ViewListArea> GetAreas(string idcompany);
    List<ViewListQuestions> ListQuestions(string idcompany);
    ViewCrudQuestions GetQuestions(string id);
    ViewCrudProcessLevelTwo GetProcessLevelTwo(string id);
    List<ViewListProcessLevelTwo> GetProcessLevelTwo();
    List<ViewListProcessLevelTwo> GetProcessLevelTwoFilter(string idarea);
    List<ViewListCompany> GetCompanies();
    ViewCrudSkill GetSkill(string filterName);
    List<ViewListSkill> GetSkills(ref long total, string filter, int count, int page);

    void SetUser(IHttpContextAccessor contextAccessor);
    //List<Skill> GetSkillsInfra(ref long total, string filter, int count, int page);
    List<ViewSkills> GetSkills(string company, ref long total, string filter, int count, int page);
    List<ViewSkills> GetSkillsGroup(string idgroup, string idcompany, ref long total, string filter, int count, int page);
    List<ViewSkills> GetSkillsOccupation(string idgroup, string idcompany, string idoccupation, ref long total, string filter, int count, int page);
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
    string AreaOrder(string idcompany, string idarea, long order, bool sum);
    string ReorderGroupScope(string idcompany, string idgroup, string idscope, bool sum);
    string ReorderOccupationActivitie(string idcompany, string idoccupation, string idactivitie, bool sum);
    string ReorderGroupScopeManual(string idcompany, string idgroup, string idscope, long order);
    string ReorderOccupationActivitieManual(string idcompany, string idoccupation, string idactivitie, long order);
    string GetCSVCompareGroup(string idcompany, string link);
    List<ViewOccupationListEdit> ListOccupationsEdit(string idcompany, string idarea, ref long total, string filter, int count, int page, string filterGroup);
    List<ViewGroupList> GetGroups(string idcompany);
    ViewCrudSkill AddSkill(ViewAddSkill view);

    ViewCrudGroup AddGroup(ViewCrudGroup view);
    string AddMapGroupScope(ViewCrudMapGroupScope view);
    string AddMapGroupSchooling(ViewCrudMapGroupSchooling view);
    string AddEssential(ViewCrudEssential view);
    string AddSkills(List<ViewAddSkill> view);
    string AddMapGroupSkill(ViewCrudMapGroupSkill view);
    //string AddOccupation(ViewAddOccupation view);
    string AddOccupationSkill(ViewAddOccupationSkill view);
    string AddOccupationActivities(ViewCrudOccupationActivities view);
    string AddOccupationActivitiesList(List<ViewCrudOccupationActivities> list);


    string UpdateMapGroupSchooling(string idgroup, ViewCrudSchooling view);
    string UpdateMapOccupationSchooling(string idoccupation, ViewCrudSchooling view);
    string UpdateMapOccupationActivities(string idoccupation, ViewCrudActivities view);
    string UpdateMapGroupScope(string idgroup, ViewCrudScope view);


    string AddTextDefault(ViewCrudTextDefault model);
    string AddCBO(ViewCrudCbo model);
    Schooling AddSchooling(ViewCrudSchooling schooling);
    string AddAxis(ViewCrudAxis view);
    string AddArea(ViewCrudArea view);
    string AddSphere(ViewCrudSphere view);
    string AddOccupation(ViewCrudOccupation occupation);
    string AddProcessLevelOne(ViewCrudProcessLevelOne model);
    string AddProcessLevelTwo(ViewCrudProcessLevelTwo model);
    string AddQuestions(ViewCrudQuestions view);

    string UpdateTextDefault(ViewCrudTextDefault view);
    string UpdateSkill(ViewCrudSkill view);
    string UpdateSphere(ViewCrudSphere view);
    string UpdateAxis(ViewCrudAxis view);
    string UpdateQuestions(ViewCrudQuestions view);
    string UpdateGroup(ViewCrudGroup view);
    string UpdateArea(ViewCrudArea view);
    string UpdateOccupation(ViewCrudOccupation view);
    string UpdateSchooling(ViewCrudSchooling view);
    string UpdateProcessLevelOne(ViewCrudProcessLevelOne view);
    string UpdateProcessLevelTwo(ViewCrudProcessLevelTwo view);
    string UpdateCBO(ViewCrudCbo view);
    #endregion


    #region Old

    ViewCrudGroup AddGroupOld(ViewAddGroup view);
    string AddMapGroupScopeOld(ViewAddMapGroupScope view);
    string AddMapGroupSchoolingOld(ViewAddMapGroupSchooling view);
    string AddEssentialOld(ViewAddEssential view);
    string AddSkillsOld(List<ViewAddSkill> view);
    string AddMapGroupSkillOld(ViewAddMapGroupSkill view);
    string AddOccupationOld(ViewAddOccupation view);
    string AddOccupationSkillOld(ViewAddOccupationSkill view);
    string AddOccupationActivitiesOld(ViewAddOccupationActivities view);
    string AddOccupationActivitiesListOld(List<ViewAddOccupationActivities> list);

    string UpdateMapGroupSchoolingOld(string idgroup, Schooling schooling);
    string UpdateMapOccupationSchoolingOld(string idoccupation, Schooling schooling);
    string UpdateMapOccupationActivitiesOld(string idoccupation, Activitie activitie);
    string UpdateMapGroupScopeOld(string idgroup, Scope scope);

    List<Company> GetCompaniesOld();
    Skill GetSkillOld(string filterName);
    List<Skill> GetSkillsOld(ref long total, string filter, int count, int page);
    List<Schooling> GetSchoolingOld();
    List<Sphere> GetSpheresOld();
    List<Sphere> GetSpheresOld(string idcompany);
    List<Axis> GetAxisOld();
    List<Cbo> ListCBOOld();
    Cbo GetCBOOld(string id);
    List<Axis> GetAxisOld(string idcompany);
    List<Area> GetAreasOld();
    List<Area> GetAreasOld(string idcompany);
    List<Questions> ListQuestionsOld(string idcompany);
    Questions GetQuestionsOld(string id);
    ProcessLevelTwo GetProcessLevelTwoOld(string id);
    List<ProcessLevelTwo> GetProcessLevelTwoOld();
    List<ProcessLevelTwo> GetProcessLevelTwoFilterOld(string idarea);

    Group GetGroupOld(string id);
    Group GetGroupOld(string idcompany, string filterName);
    Occupation GetOccupationOld(string id);
    Occupation GetOccupationOld(string idcompany, string filterName);
    List<Group> GetGroupsOld();
    List<Occupation> GetOccupationsOld();
    TextDefault GetTextDefaultOld(string idcompany, string name);
    TextDefault GetTextDefaultOld(string id);
    List<TextDefault> ListTextDefaultOld(string idcompany);
    List<Group> GetGroupsPrintOld(string idcompany);
    List<Course> GetCourseOccupationOld(string idoccuation, EnumTypeMandatoryTraining type);

    Skill AddSkillOld(ViewAddSkill view);
    //Group AddGroupOld(ViewAddGroup view);

    string AddTextDefaultOld(TextDefault model);
    string AddCBOOld(Cbo model);
    Schooling AddSchoolingOld(Schooling schooling);
    string AddAxisOld(Axis view);
    string AddAreaOld(Area view);
    string AddSphereOld(Sphere view);
    string AddOccupationOld(Occupation occupation);
    string AddProcessLevelOneOld(ProcessLevelOne model);
    string AddProcessLevelTwoOld(ProcessLevelTwo model);
    string AddQuestionsOld(Questions view);

    string UpdateTextDefaultOld(TextDefault textDefault);
    string UpdateSkillOld(Skill skill);
    string UpdateSphereOld(Sphere sphere);
    string UpdateAxisOld(Axis axis);
    string UpdateQuestionsOld(Questions questions);
    string UpdateGroupOld(Group group);
    string UpdateAreaOld(Area area);
    string UpdateOccupationOld(Occupation occupation);
    string UpdateSchoolingOld(Schooling schooling);
    string UpdateProcessLevelOneOld(ProcessLevelOne model);
    string UpdateProcessLevelTwoOld(ProcessLevelTwo model);
    string UpdateCBOOld(Cbo model);
    #endregion

  }
}
