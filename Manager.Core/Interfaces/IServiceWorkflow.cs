using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Views;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceWorkflow
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    List<Workflow> NewFlow(ViewFlow view);
    List<Workflow> Manager(ViewFlow view);
    Workflow Approved(ViewWorkflow view);
    Workflow Disapproved(ViewWorkflow view);
  }
}
