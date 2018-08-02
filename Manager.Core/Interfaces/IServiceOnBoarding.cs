using Manager.Core.Business;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceOnBoarding
  {
    List<OnBoarding> ListOnBoardingsWait(string idmanager, ref long total, string filter, int count, int page);
    List<OnBoarding> ListOnBoardingsEnd(string idmanager, ref long total, string filter, int count, int page);
    OnBoarding NewOnBoarding(OnBoarding onboarding, string idperson);
    string UpdateOnBoarding(OnBoarding onboarding, string idperson);
  }
}
