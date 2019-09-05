using Manager.Core.Views;

namespace Manager.Core.Interfaces
{
  public interface IServiceAuthentication
  {
    ViewPerson Authentication(ViewAuthentication userLogin);
    string AlterContract(string idperson);
    // test
    void GetMaristasAsyncTest(string login, string password);
  }
}
