using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
    public interface IServiceOffBoarding
    {
        void SetUser(IHttpContextAccessor contextAccessor);
        void SetUser(BaseUser user);
        string Delete(string id);
        string New(string idperson, EnumStepOffBoarding step);
        string Update(ViewCrudOffBoarding view);
        ViewCrudOffBoarding Get(string id);
        List<ViewListOffBoarding> List(ref long total, int count = 10, int page = 1, string filter = "");
    }
}
