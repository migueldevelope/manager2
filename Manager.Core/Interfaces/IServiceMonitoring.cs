using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Core.Interfaces
{
  public interface IServiceMonitoring
  {
    List<Monitoring> ListMonitoringsWait(string idmanager, ref long total, string filter, int count, int page);
    List<Monitoring> ListMonitoringsEnd(string idmanager, ref long total, string filter, int count, int page);
    Monitoring GetMonitorings(string id);
    List<Skill> GetSkills(string idperson);
    Monitoring PersonMonitoringsWait(string idmanager);
    Monitoring PersonMonitoringsEnd(string idmanager);
    Monitoring NewMonitoring(Monitoring monitoring, string idperson);
    string UpdateMonitoring(Monitoring monitoring, string idperson);
    void SetUser(IHttpContextAccessor contextAccessor);
  }
}
