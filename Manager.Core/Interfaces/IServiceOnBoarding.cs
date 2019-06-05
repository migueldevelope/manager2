using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceOnBoarding
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task<List<ViewListOnBoarding>> List(string idmanager, ref  long total,  string filter, int count, int page);
    Task<ViewListOnBoarding> PersonWait(string idperson);
    Task<ViewListOnBoarding> New(string idperson);
    Task<ViewCrudOnboarding> Get(string id);
    Task<string> Delete(string idperson);
    Task<string> DeleteComments(string idonboarding, string iditem, string idcomments);
    Task<string> UpdateCommentsView(string idonboarding, string iditem, EnumUserComment userComment);
    Task<List<ViewListOnBoarding>> ListEnded(string idmanager, ref  long total,  string filter, int count, int page);
    Task<ViewCrudOnboarding> GetOnBoardings(string id);
    Task<List<ViewListOnBoarding>> ListPersonEnd(string idmanager, ref  long total,  string filter, int count, int page);
    Task<string> Update(ViewCrudOnboarding onboarding);
    Task<List<ViewListOnBoarding>> ListExcluded(ref  long total,  string filter, int count, int page);
    Task<List<ViewCrudComment>> AddComments(string idonboarding, string iditem, ViewCrudComment comments);
    Task<string> UpdateComments(string idonboarding, string iditem, ViewCrudComment comments);
    Task<List<ViewCrudComment>> ListComments(string idonboarding, string iditem);
    Task<List<ViewListOnBoarding>> ListOnBoardingsWait(string idmanager, ref  long total,  string filter, int count, int page);
  }
}
