using Manager.Core.Base;
using Manager.Core.Business;
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
    ViewPersonHead Head(string idperson);
    Person NewPerson(Person person);
    Person UpdatePerson(Person person);
    void SetPhoto(string idPerson, string url);
    ViewPersonDetail GetPersonDetail(string idPerson);
    List<ViewPersonTeam> GetPersonTeam(ref long total, string idPerson, string filter, int count, int page);
    string GetPhoto(string idPerson);
    string AlterPassword(ViewAlterPass resetPass, string idPerson);
    string AlterPasswordForgot(ViewAlterPass resetPass, string foreign);
    List<ViewPersonList> GetPersons(string filter);
    Person GetPerson(string id);
    Task<string> ForgotPassword(string mail, ViewForgotPassword forgotPassword, string pathSendGrid);
    Person GetAuthentication(string mail, string password);

  }
}
