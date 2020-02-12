using Manager.Core.Base;
using Manager.Core.Business.Integration;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Integration;
using Manager.Views.Integration.V2;
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

    List<ViewListCompany> CompanyRootList(ref long total);
    List<ViewListIntegrationCompany> CompanyList(ref long total, int count = 10, int page = 1, string filter = "", bool all = false);
    ViewListIntegrationCompany CompanyUpdate(string idIntegration, string idCompany);
    string CompanyDelete(string idIntegration);
    List<ViewListIntegrationEstablishment> EstablishmentList( ref long total, int count = 10, int page = 1, string filter = "", bool all = false);
    ViewListIntegrationEstablishment EstablishmentUpdate(string idIntegration, string idEstablishment);
    string EstablishmentDelete(string idIntegration);
    List<ViewListIntegrationOccupation> OccupationList( ref long total, int count = 10, int page = 1, string filter = "", bool all = false);
    ViewListIntegrationOccupation OccupationUpdate(string idIntegration, string idOccupation);
    string OccupationSplit(string idIntegration);
    string OccupationJoin(string idIntegration);
    string OccupationDelete(string idIntegration);
    List<ViewListIntegrationSchooling> SchoolingList( ref long total, int count = 10, int page = 1, string filter = "", bool all = false);
    ViewListIntegrationSchooling SchoolingUpdate(string idIntegration, string idSchooling);
    string SchoolingDelete(string idIntegration);
    #endregion

    #region Integration
    IntegrationCompany GetIntegrationCompany(string key, string name);
    IntegrationEstablishment GetIntegrationEstablishment(string key, string name, string idcompany);
    IntegrationSchooling GetIntegrationSchooling(string key, string name);
    IntegrationOccupation GetIntegrationOccupation(string key, string name, string idcompany);
    IntegrationOccupation GetIntegrationOccupation(string key, string name, string idcompany, string costCenterKey, string costCenterName);
    ViewCrudPerson GetPersonByKey(string idcompany, string idestablishment, string document, string registration);
    ViewCrudUser GetUserByKey(string document);
    ViewListSchooling GetSchooling(string id);
    ViewListCompany GetCompany(string id);
    ViewListEstablishment GetEstablishment(string id);
    ViewListOccupationResume GetOccupation(string id);
    IntegrationPerson GetIntegrationPerson(string key);
    void PostIntegrationPerson(IntegrationPerson integrationPerson);
    void PutIntegrationPerson(IntegrationPerson integrationPerson);
    List<string> EmployeeChange(ViewColaborador oldEmployee, ViewColaborador newEmployee);
    #endregion

    #region Integration Skill, Occupation, Maps from ANALISA
    List<ViewListProcessLevelTwo> ProcessLevelTwoList(ref long total);
    ViewCrudSkill IntegrationSkill(ViewCrudSkill view);
    ViewIntegrationProfileOccupation IntegrationProfile(ViewIntegrationProfileOccupation view);
    #endregion

    #region Colaborador V2
    ColaboradorV2Retorno IntegrationV2(ColaboradorV2Completo view);
    ColaboradorV2Retorno IntegrationV2(ColaboradorV2Demissao view);
    ColaboradorV2 GetV2(ColaboradorV2Base view);
    ColaboradorV2 GetV2(string id);
    List<ColaboradorV2Ativo> GetActiveV2();
    string PerfilGestorV2(ColaboradorV2Base view);
    #endregion

    #region Atualização de Integração Incompleta
    ColaboradorV2Retorno IntegrationPayroll(string id);
    #endregion
  }
}
