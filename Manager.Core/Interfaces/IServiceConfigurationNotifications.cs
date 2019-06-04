using Manager.Core.Base;
using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceConfigurationNotifications
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    Task<string> New(ConfigurationNotification view);
    Task<string> Update(ConfigurationNotification view);
    Task<string> Remove(string id);
    Task<ConfigurationNotification> Get(string id);
    Task<List<ConfigurationNotification>> List( int count = 10, int page = 1, string filter = "");
  }
}
