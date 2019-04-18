using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Business.Integration;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Integration;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceIntegration
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);

    #region ConfigurationController
    ViewCrudIntegrationParameter GetIntegrationParameter();
    ViewCrudIntegrationParameter SetIntegrationParameter(ViewCrudIntegrationParameter view);
    #endregion

    #region IntegrationController
    ViewIntegrationDashboard GetStatusDashboard();
    string GetStatusIntegration();

    List<ViewListIntegrationCompany> CompanyList(ref long total, int count = 10, int page = 1, string filter = "", bool all = false);
    ViewListIntegrationCompany CompanyUpdate(string idIntegration, string idCompany);
    string CompanyDelete(string idIntegration);
    List<ViewListIntegrationEstablishment> EstablishmentList(ref long total, int count = 10, int page = 1, string filter = "", bool all = false);
    ViewListIntegrationEstablishment EstablishmentUpdate(string idIntegration, string idEstablishment);
    string EstablishmentDelete(string idIntegration);
    List<ViewListIntegrationOccupation> OccupationList(ref long total, int count = 10, int page = 1, string filter = "", bool all = false);
    ViewListIntegrationOccupation OccupationUpdate(string idIntegration, string idOccupation);
    string OccupationDelete(string idIntegration);
    List<ViewListIntegrationSchooling> SchoolingList(ref long total, int count = 10, int page = 1, string filter = "", bool all = false);
    ViewListIntegrationSchooling SchoolingUpdate(string idIntegration, string idSchooling);
    string SchoolingDelete(string idIntegration);
    #endregion

    IntegrationCompany GetIntegrationCompany(string key, string name);
    IntegrationEstablishment GetIntegrationEstablishment(string key, string name, string idcompany);
    IntegrationSchooling GetIntegrationSchooling(string key, string name);
    IntegrationOccupation GetIntegrationOccupation(string key, string name, string idcompany);
    Person GetPersonByKey(string idcompany, string idestablishment, string document, long registration);
    User GetUserByKey(string document);
    ViewListSchooling GetSchooling(string id);
    ViewListCompany GetCompany(string id);
    ViewListEstablishment GetEstablishment(string id);
    ViewListOccupation GetOccupation(string id);

    IntegrationPerson GetIntegrationPerson(string key);
    void PostIntegrationPerson(IntegrationPerson integrationPerson);
    void PutIntegrationPerson(IntegrationPerson integrationPerson);
    List<string> EmployeeChange(ViewColaborador oldEmployee, ViewColaborador newEmployee);
  }
}
