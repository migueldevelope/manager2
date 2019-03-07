﻿using Manager.Core.Business;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceLog
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void NewLog(ViewLog view);
    List<Log> GetLogs(string idaccount, ref long total, int count , int page, string filter);
  }
}
