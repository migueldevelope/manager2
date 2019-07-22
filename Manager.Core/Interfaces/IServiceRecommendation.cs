using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceRecommendation
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    string Delete(string id);
    string New(ViewCrudRecommendation view);
    string Update(ViewCrudRecommendation view);
    ViewCrudRecommendation Get(string id);
    List<ViewListRecommendation> List(ref long total, int count = 10, int page = 1, string filter = "");
    string NewRecommendationPerson(ViewCrudRecommendationPerson view);
    string UpdateRecommendationPerson(ViewCrudRecommendationPerson view);
    ViewCrudRecommendationPerson GetRecommendationPerson(string id);
    List<ViewListRecommendationPerson> ListRecommendationPerson(string idrecommendation, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListRecommendationPerson> ListRecommendationPerson(ref long total, int count = 10, int page = 1, string filter = "");
    string RemoveRecommendationPerson(string id);

    void SetImage(string idBaseHelp, string url, string fileName, string attachmentid);
  }
}
