using Manager.Core.Business;
using Manager.Core.Views;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceAuthentication
  {
    ViewPerson Authentication(ViewAuthentication userLogin);
    void CheckTermOfService(string iduser);
  }
}
