using Manager.Core.Base;
using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceCheckpoint
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);

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
