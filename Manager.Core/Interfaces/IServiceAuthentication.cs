using Manager.Core.Business;
using Manager.Core.Views;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceAuthentication
  {
    Task<ViewPerson> Authentication(ViewAuthentication userLogin);
    Task<string> AlterContract(string idperson);
  }
}
