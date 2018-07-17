using Manager.Core.Base;
using Microsoft.AspNetCore.Http;

namespace Manager.Core.Interfaces
{
  public interface IServiceCompany
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    void SetLogo(string idCompany, string url);
    string GetLogo(string idCompany);
  }
}
