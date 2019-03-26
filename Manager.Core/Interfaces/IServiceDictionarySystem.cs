﻿using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceDictionarySystem
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    string New(ViewCrudDictionarySystem view);
    string New(List<ViewListDictionarySystem> list);
    string Update(ViewCrudDictionarySystem view);
    string Remove(string id);
    ViewCrudDictionarySystem Get(string id);
    ViewListDictionarySystem GetName(string name);
    List<ViewListDictionarySystem> List(ref long total, int count = 10, int page = 1, string filter = "");
  }
}
