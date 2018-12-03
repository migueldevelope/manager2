using Manager.Core.Base;
using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceConfigurationNotifications
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    string New(ConfigurationNotifications view);
    string Update(ConfigurationNotifications view);
    string Remove(string id);
    ConfigurationNotifications Get(string id);
    List<ConfigurationNotifications> List(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
