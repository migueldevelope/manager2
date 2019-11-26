using Manager.Core.Views;

namespace Manager.Core.Interfaces
{
  public interface IServiceAuthentication
  {
    ViewPerson Authentication(ViewAuthentication userLogin, bool manager);
    ViewPerson AuthenticationV2(ViewAuthentication userLogin, bool manager);
    string AlterContract(string idperson, string _idAccount);
  }
}
