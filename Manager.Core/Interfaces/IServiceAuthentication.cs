using Manager.Core.Views;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;

namespace Manager.Core.Interfaces
{
  public interface IServiceAuthentication
  {
    ViewPerson Authentication(ViewAuthentication userLogin, bool manager);
    ViewPerson AuthenticationV2(ViewAuthentication userLogin, EnumTypeAuth typeauth);
    string AlterContract(string idperson, string _idAccount);
  }
}
