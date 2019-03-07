using Manager.Core.Business;
using Manager.Core.Views;

namespace Manager.Core.Interfaces
{
  public interface IServiceAuthentication
  {
    ViewPerson Authentication(ViewAuthentication userLogin);
    void LogSave(Person user);
  }
}
