using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceMandatoryTraining
  {
    BaseUser user { get; set; }
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    string AddOccuaption(ViewAddOccupationMandatory view);
    string AddPerson(ViewAddPersonMandatory view);
    string AddCompany(ViewAddCompanyMandatory view);
    string RemoveOccupation(string idcourse, string idoccupation);
    string RemovePerson(string idcourse, string idperson);
    string RemoveCompany(string idcourse, string idcompany);
    List<MandatoryTraining> List(ref long total, int count = 10, int page = 1, string filter = "");
    List<Occupation> ListOccupation(string idcourse, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<Person> ListPerson(string idcourse, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<Company> ListCompany(string idcourse, ref long total, int count = 10, int page = 1, string filter = "");
    string NewTrainingPlan(TrainingPlan view);
    string UpdateTrainingPlan(TrainingPlan view);
    string RemoveTrainingPlan(string id);
    TrainingPlan GetTrainingPlan(string id);
    List<TrainingPlan> ListTrainingPlan(string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
  }
}
