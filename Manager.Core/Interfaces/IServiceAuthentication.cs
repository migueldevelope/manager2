using Manager.Core.Business;
using Manager.Core.Views;

namespace Manager.Core.Interfaces
{
  public interface IServiceAuthentication
  {
    ViewPerson Authentication(string mail, string password);
    ViewPerson AuthenticationEncrypt(string mail, string password);
    ViewPerson Logoff(string idPerson);
    void LogSave(Person user);
  }
}
