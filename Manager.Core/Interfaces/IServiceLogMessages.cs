using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceLogMessages
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    List<ViewListLogMessages> ListPerson(string idperson, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListLogMessages> ListManager(string idmanager, ref long total, int count = 10, int page = 1, string filter = "");
    string New(ViewCrudLogMessages view);
    ViewCrudLogMessages Get(string id);
    string Update(ViewCrudLogMessages view);
    string Remove(string id);

    #region Old
    string New(LogMessages view);
    void NewLogMessage(string subject, string message, Person person);
    string Update(LogMessages view);
    string RemoveOld(string id);
    LogMessages GetOld(string id);
    List<LogMessages> List(ref long total, int count = 10, int page = 1, string filter = "");
    List<LogMessages> ListManagerOld(string idmanager, ref long total, int count = 10, int page = 1, string filter = "");
    List<LogMessages> ListPersonOld(string idperson, ref long total, int count = 10, int page = 1, string filter = "");
    #endregion

  }
}
