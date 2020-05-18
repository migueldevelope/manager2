using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceFeelingDay
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    string Delete(string id);
    string New(EnumFeeling feeling, string plataform);
    string Update(ViewCrudFeelingDay view);
    ViewCrudFeelingDay Get(string id);
    List<ViewListFeelingDay> List(ref long total, int count = 10, int page = 1, string filter = "");
    ViewCrudFeelingDay GetFeelingDay();
    List<ViewFeelingQtd> GetQuantity(string idmanager, long days);
    List<ViewFeelingManager> GetManager(string idmanager, long days);
  }
}
