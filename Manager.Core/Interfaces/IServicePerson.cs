using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServicePerson
  {
    List<Person> ListPerson(Expression<Func<Person, bool>> filter);
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    ViewPersonHead Head(string idperson);
    Person NewPerson(Person person);
    string NewPersonView(Person person);
    string UpdatePerson(string id, ViewPersonsCrud person);
    Person UpdatePerson(Person person);
    Person UpdatePersonView(Person person);
    void SetPhoto(string idPerson, string url);
    ViewPersonDetail GetPersonDetail(string idPerson);
    List<ViewPersonTeam> GetPersonTeam(ref long total, string idPerson, string filter, int count, int page);
    string GetPhoto(string idPerson);
    string AlterPassword(ViewAlterPass resetPass, string idPerson);
    string AlterPasswordForgot(ViewAlterPass resetPass, string foreign);
    List<ViewPersonList> GetPersons(string filter);
    List<Person> GetPersonsCrud(EnumTypeUser type, ref long total, string filter, int count, int page);
    Person GetPersonCrud(string idperson);
    Person GetPerson(string id);
    Task<string> ForgotPassword(string mail, ViewForgotPassword forgotPassword, string pathSendGrid);
    Person GetAuthentication(string mail, string password);
    List<Occupation> ListOccupation(ref long total, string filter, int count, int page);
    List<Person> ListManager(ref long total, string filter, int count, int page);
    List<Company> ListCompany(ref long total, string filter, int count, int page);
    List<Person> ListAll();

  }
}
