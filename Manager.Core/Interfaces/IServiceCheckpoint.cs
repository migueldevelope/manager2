using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceCheckpoint
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    List<ViewListCheckpoint> ListWaitManager(string idmanager, ref long total, string filter, int count, int page);
    ViewListCheckpoint ListWaitPerson(string idperson);
    ViewListCheckpoint NewCheckpoint(string idperson);
    ViewCrudCheckpoint GetCheckpoint(string id);
    string UpdateCheckpoint(ViewCrudCheckpoint view);
    ViewCrudCheckpoint PersonCheckpointEnd(string idperson);
    List<ViewListCheckpoint> ListEnded(ref long total, string filter, int count, int page);
    string DeleteCheckpoint(string idcheckpoint);

    #region Old
    List<Checkpoint> ListCheckpointsWaitOld(string idmanager, ref long total, string filter, int count, int page);
    List<Checkpoint> ListCheckpointsEndOld(string idmanager, ref long total, string filter, int count, int page);
    Checkpoint GetCheckpointsOld(string id);
    Checkpoint NewCheckpointOld(Checkpoint checkpoint, string idperson);
    string UpdateCheckpointOld(Checkpoint checkpoint, string idperson);
    string RemoveCheckpointOld(string idperson);
    List<Checkpoint> GetListExcludOld(ref long total, string filter, int count, int page);
    Checkpoint PersonCheckpointEndOld(string idperson);
    Checkpoint ListCheckpointsWaitPersonOld(string idperson);
    #endregion

  }
}
