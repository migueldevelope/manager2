﻿using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServicePerson
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    List<ViewListPersonCrud> List(ref long total, int count, int page, string filter, EnumTypeUser type, EnumStatusUserFilter status);
    ViewCrudPerson Get(string id);
    ViewCrudPerson New(ViewCrudPerson view);
    string Update(ViewCrudPerson person);
    List<ViewListOccupationResume> ListOccupation(ref long total, string filter, int count, int page);
    List<ViewBaseFields> ListManager(ref long total, string filter, int count, int page);
    List<ViewListCompany> ListCompany(ref long total, string filter, int count, int page);
    List<ViewListPerson> GetPersons(string idcompany, string filter);
    string UpdatePersonUser(ViewCrudPersonUser view);
    List<ViewListPersonTeam> ListTeam(ref long total, string idPerson, string filter, int count, int page);
    List<ViewListPersonTeam> ListTeam_V2(ref long total, List<ViewListIdIndicators> persons, string filter, int count, int page);
    List<ViewListSalaryScalePerson> ListSalaryScale(string idoccupation);
    List<ViewListPerson> ListPersonsCompany(ref long total, string idcompany, string filter, int count, int page);
    Task<long> CountNewVersion(Expression<Func<Person, bool>> filter);
    Task<Person> GetNewVersion(Expression<Func<Person, bool>> filter);
    Task<List<Person>> GetAllNewVersion(Expression<Func<Person, bool>> filter);
    BaseFields UpdateManager(Person person, string _idManager, string _idManagerOld);
    List<ViewListIdIndicators> GetFilterPersons(List<_ViewList> idmanagers);
    List<Person> Load();
    ViewListJourney ListJourney(string idmanager, string filter, int count, int page);
    ViewListTeam ListTeam_V3(string idmanager, IServiceAutoManager serviceAutoManager, string filter, int count, int page);
    List<_ViewListBase> ListOccupationManager(string idmanager, ref long total, string filter, int count, int page);
    List<ViewListOccupationProcess> ListOccupationProcess(ref long total, string filter, int count, int page);
    List<_ViewListBase> GetPersons();
    string Delete(string idperson);
    List<ViewUserNickName> GetPersonNickName(string nickname);
  }
}
