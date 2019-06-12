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
     string New(ConfigurationNotification view);
     string Update(ConfigurationNotification view);
     string Remove(string id);
     ConfigurationNotification Get(string id);
     List<ConfigurationNotification> List( ref long total, int count = 10, int page = 1, string filter = "");
  }
}
