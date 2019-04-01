using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceMandatoryTraining
  {
    #region mandatorytrainning
    void SetUser(IHttpContextAccessor contextAccessor);
    string RemoveTrainingPlan(string id);

    string AddOccupation(ViewCrudOccupationMandatory view);
    string AddPerson(ViewCrudPersonMandatory view);
    string AddCompany(ViewCrudCompanyMandatory view);
    string RemoveOccupation(string idcourse, string idoccupation);
    string RemovePerson(string idcourse, string idperson);
    string RemoveCompany(string idcourse, string idcompany);
    List<ViewListOccupation> ListOccupation(string idcourse, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListPerson> ListPerson(string idcourse, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListCompany> ListCompany(string idcourse, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewTrainingPlan> ListTrainingPlanPerson(string idperson, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewTrainingPlanList> ListTrainingPlanPersonList(string idmanager, EnumTypeUser typeUser, EnumOrigin origin, ref long total, int count = 10, int page = 1, string filter = "");
    #endregion

    #region old
    MandatoryTraining GetMandatoryTraining(string idcourse);
    List<MandatoryTraining> List(ref long total, int count = 10, int page = 1, string filter = "");
    string NewTrainingPlan(TrainingPlan view);
    string UpdateTrainingPlan(TrainingPlan view);
    TrainingPlan GetTrainingPlan(string id);
    List<TrainingPlan> ListTrainingPlan(string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<TrainingPlan> ListTrainingPlan(string idcompany, string idperson, ref long total, int count = 10, int page = 1, string filter = "");

    #endregion

    #region old
    MandatoryTraining GetMandatoryTrainingOld(string idcourse);
    List<MandatoryTraining> ListOld(ref long total, int count = 10, int page = 1, string filter = "");
    string NewTrainingPlanOld(TrainingPlan view);
    string UpdateTrainingPlanOld(TrainingPlan view);
    TrainingPlan GetTrainingPlanOld(string id);
    List<TrainingPlan> ListTrainingPlanOld(string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<TrainingPlan> ListTrainingPlanOld(string idcompany, string idperson, ref long total, int count = 10, int page = 1, string filter = "");
    #endregion


  }
}
