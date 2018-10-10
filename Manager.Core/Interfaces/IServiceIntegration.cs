using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Views;
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
    List<Schooling> GetSchoolingByName(string name);
    List<Company> GetCompanyByName(string name);
    List<Establishment> GetEstablishmentByName(string idcompany, string name);
    List<Occupation> GetOccupationByName(string idcompany, string name);
    Person GetPersonByKey(string document, string idcompany, string registration);
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
  }
}
