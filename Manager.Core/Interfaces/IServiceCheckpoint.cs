using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceCheckpoint
  {
    List<Checkpoint> ListCheckpointsWait(string idmanager, ref long total, string filter, int count, int page);
    List<Checkpoint> ListCheckpointsEnd(string idmanager, ref long total, string filter, int count, int page);
    Checkpoint GetCheckpoints(string id);
    Checkpoint NewCheckpoint(Checkpoint checkpoint, string idperson);
    string UpdateCheckpoint(Checkpoint checkpoint, string idperson);
    string RemoveCheckpoint(string idperson);
    List<Checkpoint> GetListExclud(ref long total, string filter, int count, int page);
    void SetUser(IHttpContextAccessor contextAccessor);
  }
}
