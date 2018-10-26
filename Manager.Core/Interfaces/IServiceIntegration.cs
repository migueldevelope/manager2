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
    List<Schooling> GetIntegrationSchooling(string code, string name);
    List<Company> GetIntegrationCompany(string code, string name);
    List<Establishment> GetIntegrationEstablishment(string idcompany, string code, string name);
    List<Occupation> GetIntegrationOccupation(string idcompany, string code, string name);
    Schooling GetSchooling(string id);
    Company GetCompany(string id);
    Establishment GetEstablishment(string id);
    Occupation GetOccupation(string id);
    Person GetPersonByKey(string document, string idcompany, long registration);
    //Company CompanyGet(string name);
    //Person ManagerGet(string document);
    //string NewAccount(string name);
    //Company NewCompany(string name);
    //string AccountGet(string name);
    //Schooling SchoolingGet(string name);
    //Schooling NewSchooling(string name);
    //void UpdateManager();
    //List<ViewPersonImport> ListPersonJson(StreamReader file);
    //Task<string> SetItem(ViewPersonImport item);
    //Task<string> ImportPerson(List<ViewPersonImport> list);
    IntegrationParameter GetIntegrationParameter();
    IntegrationParameter SetIntegrationParameter(ViewIntegrationParameterMode view);
    IntegrationParameter SetIntegrationParameter(ViewIntegrationParameterPack view);
    IntegrationParameter SetIntegrationParameter(ViewIntegrationParameterExecution view);

  }
}
