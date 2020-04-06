using Manager.Core.Base;
using Manager.Core.Views;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceReports
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    void RegisterOnMessageHandlerAndReceiveMesssages();
    string ListPersons();
    string ListTraining(string idevent);
    string ListCertificate(string idevent, string idperson);
    string ListOpportunityLine(string idcompany, string idarea);
    string ListHistoricTraining(ViewFilterDate date, string idperson);
    string ListMyAwareness(string idperson);
    string ListOnBoarding(string id);
    string ListOffBoarding(string id);
    string ListMonitoring(string id);
    string ListSalaryScale(string id);
  }
}
