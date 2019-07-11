﻿using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceInfra
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);

     ViewMapOccupation GetMapOccupation(string id);
     ViewCrudProcessLevelTwo GetListProcessLevelTwoById(string id);
     List<ViewListProcessLevelOneByArea> GetListProcessLevelOneByArea(string idarea);
     List<ViewListSkill> GetEssential(string idcompany);
     ViewCrudArea GetAreasById(string id);
     ViewCrudAxis GetAxisById(string id);
     ViewCrudSphere GetSphereById(string id);
     ViewCrudSkill GetSkillById(string id);
     ViewCrudSchooling GetSchoolingById(string id);
     ViewCrudGroup GetGroup(string id);
     ViewMapGroup GetMapGroup(string id);
     ViewCrudMapGroupScope GetMapGroupScopeById(string idgroup, string idscope);
     ViewCrudGroup GetGroup(string idcompany, string filterName);
     ViewCrudOccupation GetOccupation(string id);
     string AddOccupationActivitiesList(List<ViewCrudOccupationActivities> list);
     ViewCrudOccupation GetOccupation(string idcompany, string filterName);
     List<ViewListGroup> GetGroups();
     List<ViewListOccupationView> GetOccupations();
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
     List<ViewListCbo> ListCbo();
     ViewCrudCbo GetCbo(string id);
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
     List<ViewListSkill> GetSkills(ref  long total,  string filter, int count, int page);
     List<ViewSkills> GetSkills(string company, ref  long total,  string filter, int count, int page);
     List<ViewSkills> GetSkillsGroup(string idgroup, string idcompany, ref  long total,  string filter, int count, int page);
     List<ViewSkills> GetSkillsOccupation(string idgroup, string idcompany, string idoccupation, ref  long total,  string filter, int count, int page);
     string AddSpecificRequirements(string idoccupation, ViewCrudSpecificRequirements view);
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
     string DeleteCbo(string id);
     string AreaOrder(string idcompany, string idarea, long order, bool sum);
     string ReorderGroupScope(string idcompany, string idgroup, string idscope, bool sum);
     string ReorderOccupationActivitie(string idcompany, string idoccupation, string idactivitie, bool sum);
     string ReorderGroupScopeManual(string idcompany, string idgroup, string idscope, long order);
     string ReorderOccupationActivitieManual(string idcompany, string idoccupation, string idactivitie, long order);
     string GetCSVCompareGroup(string idcompany, string link);
     List<ViewOccupationListEdit> ListOccupationsEdit(string idcompany, string idarea, ref  long total,  string filter, int count, int page, string filterGroup);
     List<ViewGroupListLO> GetGroups(string idcompany);
     ViewCrudSkill AddSkill(ViewCrudSkill view);
     ViewCrudGroup AddGroup(ViewCrudGroup view);
     string AddMapGroupScope(ViewCrudMapGroupScope view);
     string AddMapGroupSchooling(ViewCrudMapGroupSchooling view);
     string AddEssential(ViewCrudEssential view);
     string AddSkills(List<ViewCrudSkill> view);
     string AddMapGroupSkill(ViewCrudMapGroupSkill view);
     string AddOccupationSkill(ViewCrudOccupationSkill view);
     string AddOccupationActivities(ViewCrudOccupationActivities view);
     string UpdateMapGroupSchooling(string idgroup, ViewCrudSchooling view);
     string UpdateMapOccupationSchooling(string idoccupation, ViewCrudSchooling view);
     string UpdateMapOccupationActivities(string idoccupation, ViewCrudActivities view);
     string UpdateMapGroupScope(string idgroup, ViewCrudScope view);
     string AddTextDefault(ViewCrudTextDefault model);
     string AddCbo(ViewCrudCbo model);
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
     string UpdateCbo(ViewCrudCbo view);
     ViewCrudProcessLevelOne GetListProcessLevelOneById(string id);
  }
}
