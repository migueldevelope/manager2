using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Views;
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
    List<User> ListUser(Expression<Func<User, bool>> filter);
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    User NewUser(User user);
    User NewUserView(User user);
    User UpdateUser(User user);
    User UpdateUserView(User user);
    void SetPhoto(string idUser, string url);
    string GetPhoto(string idUser);
    string AlterPassword(ViewAlterPass resetPass, string idUser);
    string AlterPasswordForgot(ViewAlterPass resetPass, string foreign);
    List<User> GetUsers(string idcompany, string filter);
    List<User> GetUsersCrud(EnumTypeUser type, ref long total, string filter, int count, int page);
    User GetUserCrud(string iduser);
    User GetUser(string id);
    Task<string> ForgotPassword(string mail, ViewForgotPassword forgotPassword, string pathSendGrid);
    User GetAuthentication(string mail, string password);
    List<Occupation> ListOccupation(ref long total, string filter, int count, int page);
    List<User> ListManager(ref long total, string filter, int count, int page);
    List<Company> ListCompany(ref long total, string filter, int count, int page);
    List<User> ListAll();
    string ScriptPerson();
    string ScriptOnBoarding();
    string ScriptCheckpoint();
    string ScriptMonitoring();
    string ScriptLog();

  }
}
