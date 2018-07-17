using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceWorkflow
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    List<Workflow> NewFlow(ViewFlow view);
    List<Workflow> Manager(ViewFlow view);
    Workflow Approved(ViewWorkflow view);
    Workflow Disapproved(ViewWorkflow view);
  }
}
