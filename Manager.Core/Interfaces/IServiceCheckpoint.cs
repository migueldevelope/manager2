using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceCheckpoint
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task<List<ViewListCheckpoint>> ListWaitManager(string idmanager,  string filter, int count, int page);
    Task<ViewListCheckpoint> ListWaitPerson(string idperson);
    Task<ViewListCheckpoint> NewCheckpoint(string idperson);
    Task<ViewCrudCheckpoint> GetCheckpoint(string id);
    Task<string> UpdateCheckpoint(ViewCrudCheckpoint view);
    Task<ViewCrudCheckpoint> PersonCheckpointEnd(string idperson);
    Task<List<ViewListCheckpoint>> ListEnded( string filter, int count, int page);
    Task<string> DeleteCheckpoint(string idcheckpoint);
  }
}
