using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceObjective
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);

    string Delete(string id);
    ViewCrudObjective New(ViewCrudObjective view);
    ViewCrudObjective Update(ViewCrudObjective view);
    ViewCrudObjective Get(string id);
    List<ViewListObjective> List(ref long total, int count = 10, int page = 1, string filter = "");

    ViewCrudKeyResult NewKeyResult(ViewCrudKeyResult view);
    ViewCrudKeyResult UpdateKeyResult(ViewCrudKeyResult view);
    ViewCrudKeyResult GetKeyResult(string id);
    List<ViewListKeyResult> ListKeyResult(string idobjective, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListKeyResult> ListKeyResult(ref long total, int count = 10, int page = 1, string filter = "");
    string DeleteKeyResult(string id);


    ViewCrudDimension NewDimension(ViewCrudDimension view);
    ViewCrudDimension UpdateDimension(ViewCrudDimension view);
    ViewCrudDimension GetDimension(string id);
    List<ViewListDimension> ListDimension(ref long total, int count = 10, int page = 1, string filter = "");
    string DeleteDimension(string id);
  }
}
