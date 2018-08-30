using Manager.Core.Base;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceIndicators
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    List<ViewIndicatorsNotes> GetNotes(string id);
    List<ViewIndicatorsNotes> GetNotesPerson(string id);
    bool VerifyAccount(string id);
    void SetUser(BaseUser baseUser);
    Task SendMessages(string link);
    List<ViewTagsCloud> ListTagsCloud(string idmanager);
    List<ViewTagsCloud> ListTagsCloudCompany(string idmanager);
  }
}
