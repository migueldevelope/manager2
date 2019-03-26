using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceInfra
  {

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
    string AddMapGroupScope(ViewAddMapGroupScope view);
    string AddMapGroupSchooling(ViewAddMapGroupSchooling view);

    string AddEssential(ViewAddEssential view);
    string AddSkills(List<ViewAddSkill> view);
    string AddMapGroupSkill(ViewAddMapGroupSkill view);
    ViewCrudSkill AddSkill(ViewAddSkill view);
    ViewCrudGroup AddGroup(ViewAddGroup view);
    string AddOccupation(ViewAddOccupation view);
    string AddOccupationSkill(ViewAddOccupationSkill view);
    string AddOccupationActivities(ViewAddOccupationActivities view);
    string AddOccupationActivitiesList(List<ViewAddOccupationActivities> list);

    #region Old
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

    Group GetGroup(string id);
    Group GetGroup(string idcompany, string filterName);
    Occupation GetOccupation(string id);
    Occupation GetOccupation(string idcompany, string filterName);
    List<Group> GetGroups();
    List<Occupation> GetOccupations();
    TextDefault GetTextDefault(string idcompany, string name);
    TextDefault GetTextDefault(string id);
    List<TextDefault> ListTextDefault(string idcompany);
    //List<Occupation> GetOccupationsInfra(ref long total, string filter, int count, int page);
    List<ViewGetOccupation> GetOccupations(string idcompany, string idarea);
    //List<ProcessLevelTwo> GetProcessLevelTwo(string idarea);
    ProcessLevelTwo GetProcessLevelTwoOld(string id);
    List<ProcessLevelTwo> GetProcessLevelTwoOld();
    List<ProcessLevelTwo> GetProcessLevelTwoFilterOld(string idarea);

    Skill AddSkillOld(ViewAddSkill view);
    Group AddGroupOld(ViewAddGroup view);

    string AddTextDefault(TextDefault model);
    string AddCBO(Cbo model);
    Schooling AddSchooling(Schooling schooling);
    string AddAxis(Axis view);
    
   
    string AddArea(Area view);
    string AddSphere(Sphere view);
    string AddOccupation(Occupation occupation);
   
    string AddProcessLevelOne(ProcessLevelOne model);
    string AddProcessLevelTwo(ProcessLevelTwo model);
    string AddQuestions(Questions view);
   
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
    List<Group> GetGroupsPrint(string idcompany);
    List<Course> GetCourseOccupation(string idoccuation, EnumTypeMandatoryTraining type);
    #endregion

  }
}
