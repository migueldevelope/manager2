using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
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
    List<ViewListPersonCrud> GetPersons(ref long total, int count, int page, string filter, EnumTypeUser type);
    ViewCrudPerson GetPersonCrud(string id);
    ViewCrudPerson NewPerson(ViewCrudPerson view);
    ViewCrudPerson UpdatePerson(ViewCrudPerson person);
    List<SalaryScalePerson> ListSalaryScale(string idoccupation);

    #region Old
    List<Person> ListPerson(Expression<Func<Person, bool>> filter);
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    ViewPersonHead Head(string idperson);
    Person NewPersonOld(Person person);
    Person NewPersonView(Person person);
    string UpdatePersonOld(string id, ViewPersonsCrud person);
    Person UpdatePersonOld(Person person);
    Person UpdatePersonView(Person person);
    void SetPhoto(string idPerson, string url);
    ViewPersonDetail GetPersonDetail(string idPerson);
    List<ViewPersonTeam> GetPersonTeam(ref long total, string idPerson, string filter, int count, int page);
    string GetPhoto(string idPerson);
    List<ViewPersonList> GetPersons(string filter);
    List<Person> GetPersons(string idcompany, string filter);
    List<Person> GetPersonsCrud(EnumTypeUser type, ref long total, string filter, int count, int page);
    Person GetPersonCrudOld(string idperson);
    Person GetPerson(string id);
    List<Occupation> ListOccupation(ref long total, string filter, int count, int page);
    List<Person> ListManager(ref long total, string filter, int count, int page);
    List<Company> ListCompany(ref long total, string filter, int count, int page);
    List<Person> ListAll();

    string AddPersonUser(ViewPersonUser view);
    string UpdatePersonUser(ViewPersonUser view);
    #endregion
  }
}
