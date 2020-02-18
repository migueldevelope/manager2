using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceMandatoryTraining
  {
    #region mandatorytrainning
    void SetUser(IHttpContextAccessor contextAccessor);
    string RemoveTrainingPlan(string id);

    List<ViewListCourse> ListOccupationGroup(string idoccupation, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListCourse> ListPersonGroup(string idperson, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListCourse> ListCompanyGroup(string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<string> AddOccupation(ViewCrudOccupationMandatory view);
    List<string> AddPerson(ViewCrudPersonMandatory view);
    List<string> AddCompany(ViewCrudCompanyMandatory view);
    string RemoveOccupation(string idcourse, string idoccupation);
    string RemovePerson(string idcourse, string idperson);
    string RemoveCompany(string idcourse, string idcompany);
    List<ViewListOccupation> ListOccupation(string idcourse, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListPersonBase> ListPerson(string idcourse, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListCompany> ListCompany(string idcourse, ref long total, int count = 10, int page = 1, string filter = "");
    ViewListTrainingPlan ListTrainingPlanPerson(string iduser, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListTrainingPlan> ListTrainingPlanPersonList(string idmanager, EnumTypeUser typeUser, EnumOrigin origin, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListTrainingPlanManager> ListTrainingPlanPersonManager(string idmanager, EnumTypeUser typeUser, EnumOrigin origin, ref long total, int count = 10, int page = 1, string filter = "");
    ViewCrudMandatoryTraining GetMandatoryTraining(string idcourse);
    List<ViewCrudMandatoryTraining> List(ref long total, int count = 10, int page = 1, string filter = "");
    string NewTrainingPlanInternal(TrainingPlan view);
    string UpdateTrainingPlanInternal(TrainingPlan view);
    string NewTrainingPlan(ViewCrudTrainingPlan view);
    string UpdateTrainingPlan(ViewCrudTrainingPlan view);
    ViewCrudTrainingPlan GetTrainingPlan(string id);
    List<ViewCrudTrainingPlan> ListTrainingPlan(string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewCrudTrainingPlan> ListTrainingPlan(string idcompany, string idperson, ref long total, int count = 10, int page = 1, string filter = "");

    #endregion

  }
}
