using Manager.Core.Base;
using Manager.Views.Audit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Interfaces
{
  public interface IServiceAudit
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser _user);

    List<ViewAuditPerson> ListPerson();
    List<ViewAuditUser> ListUser();
  }
}
