using Manager.Core.Base;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceMailMessage
  {
    void SetUser(IHttpContextAccessor contextAccessor);
     ViewMailMessage GetMessage(string id);
  }
}
