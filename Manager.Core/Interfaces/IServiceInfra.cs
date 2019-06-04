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

    Task<ViewMapOccupation> GetMapOccupation(string id);
    Task<ViewCrudProcessLevelTwo> GetListProcessLevelTwoById(string id);
    Task<List<ViewListProcessLevelOneByArea>> GetListProcessLevelOneByArea(string idarea);
    Task<List<ViewListSkill>> GetEssential(string idcompany);
    Task<ViewCrudArea> GetAreasById(string id);
    Task<ViewCrudAxis> GetAxisById(string id);
    Task<ViewCrudSphere> GetSphereById(string id);
    Task<ViewCrudSkill> GetSkillById(string id);
    Task<ViewCrudSchooling> GetSchoolingById(string id);
    Task<ViewCrudGroup> GetGroup(string id);
    Task<ViewMapGroup> GetMapGroup(string id);
    Task<ViewCrudMapGroupScope> GetMapGroupScopeById(string idgroup, string idscope);
    Task<ViewCrudGroup> GetGroup(string idcompany, string filterName);
    Task<ViewCrudOccupation> GetOccupation(string id);
    Task<string> AddOccupationActivitiesList(List<ViewCrudOccupationActivities> list);
    Task<ViewCrudOccupation> GetOccupation(string idcompany, string filterName);
    Task<List<ViewListGroup>> GetGroups();
    Task<List<ViewListOccupationView>> GetOccupations();
    Task<ViewCrudTextDefault> GetTextDefault(string idcompany, string name);
    Task<ViewCrudTextDefault> GetTextDefault(string id);
    Task<List<ViewListTextDefault>> ListTextDefault(string idcompany);
    Task<List<ViewListGroup>> GetGroupsPrint(string idcompany);
    Task<List<ViewListCourse>> GetCourseOccupation(string idoccuation, EnumTypeMandatoryTraining type);
    Task<List<ViewGetOccupation>> GetOccupations(string idcompany, string idarea);
    Task<List<ViewListSchooling>> GetSchooling();
    Task<List<ViewListSphere>> GetSpheres();
    Task<List<ViewListSphere>> GetSpheres(string idcompany);
    Task<List<ViewListAxis>> GetAxis();
    Task<List<ViewListCbo>> ListCBO();
    Task<ViewCrudCbo> GetCBO(string id);
    Task<List<ViewListAxis>> GetAxis(string idcompany);
    Task<List<ViewListArea>> GetAreas();
    Task<List<ViewListArea>> GetAreas(string idcompany);
    Task<List<ViewListQuestions>> ListQuestions(string idcompany);
    Task<ViewCrudQuestions> GetQuestions(string id);
    Task<ViewCrudProcessLevelTwo> GetProcessLevelTwo(string id);
    Task<List<ViewListProcessLevelTwo>> GetProcessLevelTwo();
    Task<List<ViewListProcessLevelTwo>> GetProcessLevelTwoFilter(string idarea);
    Task<List<ViewListCompany>> GetCompanies();
    Task<ViewCrudSkill> GetSkill(string filterName);
    Task<List<ViewListSkill>> GetSkills( string filter, int count, int page);
    Task<List<ViewSkills>> GetSkills(string company,  string filter, int count, int page);
    Task<List<ViewSkills>> GetSkillsGroup(string idgroup, string idcompany,  string filter, int count, int page);
    Task<List<ViewSkills>> GetSkillsOccupation(string idgroup, string idcompany, string idoccupation,  string filter, int count, int page);
    Task<string> AddSpecificRequirements(string idoccupation, ViewCrudSpecificRequirements view);
    Task<string> DeleteEssential(string idcompany, string id);
    Task<string> DeleteSkill(string idskill);
    Task<string> DeleteSphere(string idsphere);
    Task<string> DeleteAxis(string idaxis);
    Task<string> DeleteGroup(string idgroup);
    Task<string> DeleteMapGroupSkill(string idgroup, string id);
    Task<string> DeleteMapGroupSchooling(string idgroup, string id);
    Task<string> DeleteArea(string idarea);
    Task<string> DeleteQuestion(string idquestion);
    Task<string> DeleteOccupation(string idoccupation);
    Task<string> DeleteOccupationSkill(string idoccupation, string id);
    Task<string> DeleteOccupationActivities(string idoccupation, string idactivitie);
    Task<string> DeleteMapGroupScope(string idgroup, string idscope);
    Task<string> DeleteSchooling(string idschooling);
    Task<string> DeleteProcessLevelOne(string id);
    Task<string> DeleteProcessLevelTwo(string id);
    Task<string> DeleteTextDefault(string id);
    Task<string> DeleteCBO(string id);
    Task<string> AreaOrder(string idcompany, string idarea, long order, bool sum);
    Task<string> ReorderGroupScope(string idcompany, string idgroup, string idscope, bool sum);
    Task<string> ReorderOccupationActivitie(string idcompany, string idoccupation, string idactivitie, bool sum);
    Task<string> ReorderGroupScopeManual(string idcompany, string idgroup, string idscope, long order);
    Task<string> ReorderOccupationActivitieManual(string idcompany, string idoccupation, string idactivitie, long order);
    Task<string> GetCSVCompareGroup(string idcompany, string link);
    Task<List<ViewOccupationListEdit>> ListOccupationsEdit(string idcompany, string idarea,  string filter, int count, int page, string filterGroup);
    Task<List<ViewGroupListLO>> GetGroups(string idcompany);
    Task<ViewCrudSkill> AddSkill(ViewCrudSkill view);
    Task<ViewCrudGroup> AddGroup(ViewCrudGroup view);
    Task<string> AddMapGroupScope(ViewCrudMapGroupScope view);
    Task<string> AddMapGroupSchooling(ViewCrudMapGroupSchooling view);
    Task<string> AddEssential(ViewCrudEssential view);
    Task<string> AddSkills(List<ViewCrudSkill> view);
    Task<string> AddMapGroupSkill(ViewCrudMapGroupSkill view);
    Task<string> AddOccupationSkill(ViewCrudOccupationSkill view);
    Task<string> AddOccupationActivities(ViewCrudOccupationActivities view);
    Task<string> UpdateMapGroupSchooling(string idgroup, ViewCrudSchooling view);
    Task<string> UpdateMapOccupationSchooling(string idoccupation, ViewCrudSchooling view);
    Task<string> UpdateMapOccupationActivities(string idoccupation, ViewCrudActivities view);
    Task<string> UpdateMapGroupScope(string idgroup, ViewCrudScope view);
    Task<string> AddTextDefault(ViewCrudTextDefault model);
    Task<string> AddCBO(ViewCrudCbo model);
    Task<Schooling> AddSchooling(ViewCrudSchooling schooling);
    Task<string> AddAxis(ViewCrudAxis view);
    Task<string> AddArea(ViewCrudArea view);
    Task<string> AddSphere(ViewCrudSphere view);
    Task<string> AddOccupation(ViewCrudOccupation occupation);
    Task<string> AddProcessLevelOne(ViewCrudProcessLevelOne model);
    Task<string> AddProcessLevelTwo(ViewCrudProcessLevelTwo model);
    Task<string> AddQuestions(ViewCrudQuestions view);
    Task<string> UpdateTextDefault(ViewCrudTextDefault view);
    Task<string> UpdateSkill(ViewCrudSkill view);
    Task<string> UpdateSphere(ViewCrudSphere view);
    Task<string> UpdateAxis(ViewCrudAxis view);
    Task<string> UpdateQuestions(ViewCrudQuestions view);
    Task<string> UpdateGroup(ViewCrudGroup view);
    Task<string> UpdateArea(ViewCrudArea view);
    Task<string> UpdateOccupation(ViewCrudOccupation view);
    Task<string> UpdateSchooling(ViewCrudSchooling view);
    Task<string> UpdateProcessLevelOne(ViewCrudProcessLevelOne view);
    Task<string> UpdateProcessLevelTwo(ViewCrudProcessLevelTwo view);
    Task<string> UpdateCBO(ViewCrudCbo view);
    Task<ViewCrudProcessLevelOne> GetListProcessLevelOneById(string id);
  }
}
