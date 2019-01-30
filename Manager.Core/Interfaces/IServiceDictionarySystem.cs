using Manager.Core.Base;
using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceDictionarySystem
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    string New(DictionarySystem view);
    string New(List<DictionarySystem> list);
    string Update(DictionarySystem view);
    string Remove(string id);
    DictionarySystem Get(string id);
    DictionarySystem GetName(string name);
    List<DictionarySystem> List(ref long total, int count = 10, int page = 1, string filter = "");

  }
}
