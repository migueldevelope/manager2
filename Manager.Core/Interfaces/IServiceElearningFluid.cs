using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceElearningFluid
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);

    string Delete(string id);
    ViewCrudElearningFluid New(ViewCrudElearningFluid view);
    ViewCrudElearningFluid Update(ViewCrudElearningFluid view);
    ViewCrudElearningFluid Get(string id);
    List<ViewListElearningFluid> List(ref long total, int count = 10, int page = 1, string filter = "");

    ViewCrudElearningFluidQuestions NewElearningFluidQuestions(ViewCrudElearningFluidQuestions view);
    ViewCrudElearningFluidQuestions UpdateElearningFluidQuestions(ViewCrudElearningFluidQuestions view);
    ViewCrudElearningFluidQuestions GetElearningFluidQuestions(string id);
    string DeleteElearningFluidQuestions(string id);
    List<ViewListElearningFluidQuestions> ListElearningFluidQuestions(ref long total, int count = 10, int page = 1, string filter = "");
    string ElearningVideo();
    string ElearningCertificate();
  }
}
