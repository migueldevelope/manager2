using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
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
     List<ViewListOnBoarding> List(string idmanager, ref  long total,  string filter, int count, int page);
     ViewListOnBoarding PersonWait(string idperson);
     ViewListOnBoarding New(string idperson);
     ViewCrudOnboarding Get(string id);
     string Delete(string idperson);
     string DeleteComments(string idonboarding, string iditem, string idcomments);
     string UpdateCommentsView(string idonboarding, string iditem, EnumUserComment userComment);
     List<ViewListOnBoarding> ListEnded(string idmanager, ref  long total,  string filter, int count, int page);
     ViewCrudOnboarding GetOnBoardings(string id);
     List<ViewListOnBoarding> ListPersonEnd(string idmanager, ref  long total,  string filter, int count, int page);
     string Update(ViewCrudOnboarding onboarding);
     List<ViewListOnBoarding> ListExcluded(ref  long total,  string filter, int count, int page);
     List<ViewCrudComment> AddComments(string idonboarding, string iditem, ViewCrudComment comments);
     string UpdateComments(string idonboarding, string iditem, ViewCrudComment comments);
     List<ViewCrudComment> ListComments(string idonboarding, string iditem);
     List<ViewListOnBoarding> ListOnBoardingsWait(string idmanager, ref  long total,  string filter, int count, int page);
    List<ViewExportStatusOnboardingGeral> ExportStatusOnboarding();
    List<ViewExportStatusOnboarding> ExportStatusOnboarding(string idperson);
    void MailTest();
  }
}
