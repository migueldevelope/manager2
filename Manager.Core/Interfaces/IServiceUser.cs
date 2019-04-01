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
  public interface IServiceUser
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    List<ViewListUser> List(ref long total, int count, int page, string filter, EnumTypeUser type);
    ViewCrudUser Get(string iduser);
    ViewCrudUser New(ViewCrudUser view);
    ViewCrudUser Update(ViewCrudUser view);
    string GetPhoto(string idUser);
    string AlterPassword(ViewAlterPass resetPass, string idUser);
    string AlterPasswordForgot(ViewAlterPass resetPass, string foreign);
    Task<string> ForgotPassword(string mail, ViewForgotPassword forgotPassword, string pathSendGrid);

    #region Old
    List<User> ListUser(Expression<Func<User, bool>> filter);
    //BaseUser user { get; set; }
    User NewUser(User user);
    User NewUserView(User user);
    User UpdateUser(User user);
    User UpdateUserView(User user);
    void SetPhoto(string idUser, string url);
    List<User> GetUsers(string idcompany, string filter);
    List<User> GetUsersCrudOld(EnumTypeUser type, ref long total, string filter, int count, int page);
    User GetUserCrudOld(string iduser);
    User GetUser(string id);
    User GetAuthentication(string mail, string password);
    List<Occupation> ListOccupation(ref long total, string filter, int count, int page);
    List<User> ListManager(ref long total, string filter, int count, int page);
    List<Company> ListCompany(ref long total, string filter, int count, int page);
    List<User> ListAll();
    //string ScriptPerson();
    //string ScriptOnBoarding();
    //string ScriptCheckpoint();
    //string ScriptMonitoring();
    //string ScriptLog();
    List<Person> ListPerson(string iduser, ref long total, string filter, int count, int page);
    #endregion

  }
}
