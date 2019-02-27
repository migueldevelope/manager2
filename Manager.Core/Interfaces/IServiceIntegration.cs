using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Business.Integration;
using Manager.Views.Integration;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceIntegration
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    IntegrationCompany GetIntegrationCompany(string key, string name);
    IntegrationEstablishment GetIntegrationEstablishment(string key, string name, string idcompany);
    IntegrationSchooling GetIntegrationSchooling(string key, string name);
    IntegrationOccupation GetIntegrationOccupation(string key, string name, string idcompany);
    Person GetPersonByKey(string idcompany, string idestablishment, string document, long registration);
    User GetUserByKey(string document);
    Schooling GetSchooling(string id);
    Company GetCompany(string id);
    Establishment GetEstablishment(string id);
    Occupation GetOccupation(string id);
    IntegrationParameter GetIntegrationParameter();
    IntegrationParameter SetIntegrationParameter(ViewIntegrationParameterMode view);
    IntegrationParameter SetIntegrationParameter(ViewIntegrationParameterPack view);
    IntegrationParameter SetIntegrationParameter(ViewIntegrationParameterExecution view);
    IntegrationPerson GetIntegrationPerson(string key);
    void PostIntegrationPerson(IntegrationPerson integrationPerson);
    void PutIntegrationPerson(IntegrationPerson integrationPerson);
    List<string> EmployeeChange(ViewColaborador oldEmployee, ViewColaborador newEmployee);
    string GetStatusIntegration();
    List<ViewIntegrationCompany> CompanyList(ref long total, int count = 10, int page = 1, string filter = "", bool all = false);
    List<ViewIntegrationEstablishment> EstablishmentList(ref long total, int count = 10, int page = 1, string filter = "", bool all = false);
    List<ViewIntegrationOccupation> OccupationList(ref long total, int count = 10, int page = 1, string filter = "", bool all = false);
    List<ViewIntegrationSchooling> SchoolingList(ref long total, int count = 10, int page = 1, string filter = "", bool all = false);
    ViewIntegrationCompany CompanyUpdate(string idIntegration, string idCompany);
    ViewIntegrationEstablishment EstablishmentUpdate(string idIntegration, string idEstablishment);
    ViewIntegrationOccupation OccupationUpdate(string idIntegration, string idOccupation);
    ViewIntegrationSchooling SchoolingUpdate(string idIntegration, string idSchooling);
    ViewIntegrationDashboard GetStatusDashboard();
  }
}
