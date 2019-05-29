using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceMaturity
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    string Delete(string id);
    string New(ViewCrudMaturity view);
    string Update(ViewCrudMaturity view);
    ViewCrudMaturity Get(string id);
    List<ViewCrudMaturity> List(ref long total, int count = 10, int page = 1, string filter = "");
    string NewMaturityRegister(ViewCrudMaturityRegister view);
    string UpdateMaturityRegister(ViewCrudMaturityRegister view);
    ViewCrudMaturityRegister GetMaturityRegister(string id);
    string DeleteMaturityRegister(string id);
    List<ViewCrudMaturityRegister> ListMaturityRegister(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
