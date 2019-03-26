using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceMandatoryTraining
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    string AddOccupation(ViewCrudOccupationMandatory view);
    string AddPerson(ViewCrudPersonMandatory view);
    string AddCompany(ViewCrudCompanyMandatory view);
    string RemoveOccupation(string idcourse, string idoccupation);
    string RemovePerson(string idcourse, string idperson);
    string RemoveCompany(string idcourse, string idcompany);
    MandatoryTraining GetMandatoryTraining(string idcourse);
    List<MandatoryTraining> List(ref long total, int count = 10, int page = 1, string filter = "");
    List<Occupation> ListOccupation(string idcourse, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<Person> ListPerson(string idcourse, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<Company> ListCompany(string idcourse, ref long total, int count = 10, int page = 1, string filter = "");
    string NewTrainingPlan(TrainingPlan view);
    string UpdateTrainingPlan(TrainingPlan view);
    string RemoveTrainingPlan(string id);
    TrainingPlan GetTrainingPlan(string id);
    List<TrainingPlan> ListTrainingPlan(string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<TrainingPlan> ListTrainingPlan(string idcompany, string idperson, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewTrainingPlan> ListTrainingPlanPerson(string idperson, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewTrainingPlanList> ListTrainingPlanPersonList(string idmanager, EnumTypeUser typeUser, EnumOrigin origin, ref long total, int count = 10, int page = 1, string filter = "");
  }
}
