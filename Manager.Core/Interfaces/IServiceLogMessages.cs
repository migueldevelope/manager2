using Manager.Core.Base;
using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceLogMessages
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    string New(LogMessages view);
    void NewLogMessage(string subject, string message, Person person);
    string Update(LogMessages view);
    string Remove(string id);
    LogMessages Get(string id);
    List<LogMessages> List(ref long total, int count = 10, int page = 1, string filter = "");
    List<LogMessages> ListManager(string idmanager, ref long total, int count = 10, int page = 1, string filter = "");
    List<LogMessages> ListPerson(string idperson, ref long total, int count = 10, int page = 1, string filter = "");
  }
}
