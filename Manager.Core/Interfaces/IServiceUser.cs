using Manager.Core.Base;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceUser
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);

    void CheckTermOfService(string iduser);
    List<ViewCrudUser> List(int count, int page, string filter, EnumTypeUser type);
    ViewCrudUser Get(string iduser);
    ViewCrudUser New(ViewCrudUser view);
    ViewCrudUser Update(ViewCrudUser view);
    string GetPhoto(string idUser);
    void SetPhoto(string idUser, string url);
    string AlterPassword(ViewAlterPass resetPass, string idUser);
    string AlterPasswordForgot(ViewAlterPass resetPass, string foreign);
    string ForgotPassword(string mail, ViewForgotPassword forgotPassword, string pathSendGrid);
    List<ViewListPersonInfo> ListPerson(string iduser, ref long total, string filter, int count, int page);
    string Delete(string iduser);
  }
}
