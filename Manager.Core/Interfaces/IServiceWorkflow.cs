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
    Task<List<Workflow>> NewFlow(ViewFlow view);
    Task<List<Workflow>> Manager(ViewFlow view);
    Task<Workflow> Approved(ViewWorkflow view);
    Task<Workflow> Disapproved(ViewWorkflow view);
  }
}
