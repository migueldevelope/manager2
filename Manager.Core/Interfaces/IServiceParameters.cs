using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceParameters
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    string Remove(string id);
    string New(ViewCrudParameter view);
    string Update(ViewCrudParameter view);
    ViewCrudParameter Get(string id);
    ViewCrudParameter GetName(string name);
    List<ViewListParameter> List(ref long total, int count = 10, int page = 1, string filter = "");

    #region Old
    string NewOld(Parameter view);
    string UpdateOld(Parameter view);
    Parameter GetOld(string id);
    Parameter GetNameOld(string name);
    List<Parameter> ListOld(ref long total, int count = 10, int page = 1, string filter = "");
    #endregion

  }
}
