using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
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
    Task<string> RemoveTrainingPlan(string id);

    Task<string> AddOccupation(ViewCrudOccupationMandatory view);
    Task<string> AddPerson(ViewCrudPersonMandatory view);
    Task<string> AddCompany(ViewCrudCompanyMandatory view);
    Task<string> RemoveOccupation(string idcourse, string idoccupation);
    Task<string> RemovePerson(string idcourse, string idperson);
    Task<string> RemoveCompany(string idcourse, string idcompany);
    Task<List<ViewListOccupation>> ListOccupation(string idcourse, string idcompany,  int count = 10, int page = 1, string filter = "");
    Task<List<ViewListPerson>> ListPerson(string idcourse, string idcompany,  int count = 10, int page = 1, string filter = "");
    Task<List<ViewListCompany>> ListCompany(string idcourse,  int count = 10, int page = 1, string filter = "");
    Task<List<ViewTrainingPlan>> ListTrainingPlanPerson(string iduser,  int count = 10, int page = 1, string filter = "");
    Task<List<ViewTrainingPlanList>> ListTrainingPlanPersonList(string idmanager, EnumTypeUser typeUser, EnumOrigin origin,  int count = 10, int page = 1, string filter = "");
    Task<ViewCrudMandatoryTraining> GetMandatoryTraining(string idcourse);
    Task<List<ViewCrudMandatoryTraining>> List( int count = 10, int page = 1, string filter = "");
    Task<string> NewTrainingPlanInternal(TrainingPlan view);
    Task<string> UpdateTrainingPlanInternal(TrainingPlan view);
    Task<string> NewTrainingPlan(ViewCrudTrainingPlan view);
    Task<string> UpdateTrainingPlan(ViewCrudTrainingPlan view);
    Task<ViewCrudTrainingPlan> GetTrainingPlan(string id);
    Task<List<ViewCrudTrainingPlan>> ListTrainingPlan(string idcompany,  int count = 10, int page = 1, string filter = "");
    Task<List<ViewCrudTrainingPlan>> ListTrainingPlan(string idcompany, string idperson,  int count = 10, int page = 1, string filter = "");

    #endregion

    #region old
    MandatoryTraining GetMandatoryTrainingOld(string idcourse);
    List<MandatoryTraining> ListOld( int count = 10, int page = 1, string filter = "");
    string NewTrainingPlanOld(TrainingPlan view);
    string UpdateTrainingPlanOld(TrainingPlan view);
    TrainingPlan GetTrainingPlanOld(string id);
    List<TrainingPlan> ListTrainingPlanOld(string idcompany,  int count = 10, int page = 1, string filter = "");
    List<TrainingPlan> ListTrainingPlanOld(string idcompany, string idperson,  int count = 10, int page = 1, string filter = "");
    #endregion


  }
}
