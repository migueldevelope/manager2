using Manager.Core.Business;
using Manager.Core.Views;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceAuthentication
  {
    ViewPerson Authentication(ViewAuthentication userLogin);
    string AlterContract(string idperson);
    //void GetUnimedAsync(string login, string passwordClient);
  }
}
