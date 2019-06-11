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
    Task<List<ViewListOccupation>> ListOccupation(string idcourse, string idcompany,  ref long total, int count = 10, int page = 1, string filter = "");
    Task<List<ViewListPerson>> ListPerson(string idcourse, string idcompany,  ref long total, int count = 10, int page = 1, string filter = "");
    Task<List<ViewListCompany>> ListCompany(string idcourse,  ref long total, int count = 10, int page = 1, string filter = "");
    Task<List<ViewTrainingPlan>> ListTrainingPlanPerson(string iduser,  ref long total, int count = 10, int page = 1, string filter = "");
    Task<List<ViewTrainingPlanList>> ListTrainingPlanPersonList(string idmanager, EnumTypeUser typeUser, EnumOrigin origin,  ref long total, int count = 10, int page = 1, string filter = "");
    Task<ViewCrudMandatoryTraining> GetMandatoryTraining(string idcourse);
    Task<List<ViewCrudMandatoryTraining>> List( ref long total, int count = 10, int page = 1, string filter = "");
    Task<string> NewTrainingPlanInternal(TrainingPlan view);
    Task<string> UpdateTrainingPlanInternal(TrainingPlan view);
    Task<string> NewTrainingPlan(ViewCrudTrainingPlan view);
    Task<string> UpdateTrainingPlan(ViewCrudTrainingPlan view);
    Task<ViewCrudTrainingPlan> GetTrainingPlan(string id);
    Task<List<ViewCrudTrainingPlan>> ListTrainingPlan(string idcompany,  ref long total, int count = 10, int page = 1, string filter = "");
    Task<List<ViewCrudTrainingPlan>> ListTrainingPlan(string idcompany, string idperson,  ref long total, int count = 10, int page = 1, string filter = "");

    #endregion
    


  }
}
