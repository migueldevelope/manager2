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

    Task CheckTermOfService(string iduser);
    Task<List<ViewListUser>> List( int count, int page, string filter, EnumTypeUser type);
    Task<ViewCrudUser> Get(string iduser);
    Task<ViewCrudUser> New(ViewCrudUser view);
    Task<ViewCrudUser> Update(ViewCrudUser view);
    Task<string> GetPhoto(string idUser);
    Task SetPhoto(string idUser, string url);
    Task<string> AlterPassword(ViewAlterPass resetPass, string idUser);
    Task<string> AlterPasswordForgot(ViewAlterPass resetPass, string foreign);
    Task<string> ForgotPassword(string mail, ViewForgotPassword forgotPassword, string pathSendGrid);
    Task<List<ViewListPersonInfo>> ListPerson(string iduser,  string filter, int count, int page);
  }
}
