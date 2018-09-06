using Manager.Core.Base;
using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceParameters
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    string New(Parameter view);
    string Update(Parameter view);
    string Remove(string id);
    Parameter Get(string id);
    List<Parameter> List(ref long total, int count = 10, int page = 1, string filter = "");
    
  }
}
