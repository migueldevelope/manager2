using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceObjective
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);

    string Delete(string id);
    ViewCrudObjective New(ViewCrudObjective view);
    ViewCrudObjective Update(ViewCrudObjective view);
    ViewCrudObjective Get(string id);
    List<ViewListObjective> List(ref long total, int count = 10, int page = 1, string filter = "");

    ViewCrudKeyResult NewKeyResult(ViewCrudKeyResult view);
    ViewCrudKeyResult UpdateKeyResult(ViewCrudKeyResult view);
    ViewCrudKeyResult GetKeyResult(string id);
    List<ViewListKeyResult> ListKeyResult(string idobjective, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListKeyResult> ListKeyResult(ref long total, int count = 10, int page = 1, string filter = "");
    string DeleteKeyResult(string id);


    ViewCrudDimension NewDimension(ViewCrudDimension view);
    ViewCrudDimension UpdateDimension(ViewCrudDimension view);
    ViewCrudDimension GetDimension(string id);
    List<ViewListDimension> ListDimension(ref long total, int count = 10, int page = 1, string filter = "");
    string DeleteDimension(string id);

    ViewCrudPendingCheckinObjective NewPendingCheckinObjective(ViewCrudPendingCheckinObjective view);
    ViewCrudPendingCheckinObjective UpdatePendingCheckinObjective(ViewCrudPendingCheckinObjective view);
    ViewCrudPendingCheckinObjective GetPendingCheckinObjective(string id);
    List<ViewListPendingCheckinObjective> ListPendingCheckinObjective(ref long total, int count = 10, int page = 1, string filter = "");
    string DeletePendingCheckinObjective(string id);

    ViewListObjectiveParticipantCard GetParticipantCard();

    ViewListObjectiveParticipantCard GetParticipantCard(string idperson);

    ViewListObjectiveResponsibleCard GetResponsibleCard();

    List<ViewListDetailResposibleObjective> GetDetailResposibleObjective(ref long total, int count = 10, int page = 1, string filter = "");

    ViewCrudKeyResult UpdateResultKeyResult(string idkeyresult, string idcheckin, decimal achievement, decimal result, ViewText view);

    string AddParticipants(string idkeyresult, ViewCrudParticipantKeyResult view);

    string DeleteParticipants(string idkeyresult, string idperson);

    List<ViewListPersonPhoto> AddEditors(string idobjective, string idperson);

    string DeleteEditor(string idobjetctive, string idperson);

    List<ViewCrudImpedimentsIniciatives> AddImpediment(string idcheckin, ViewCrudImpedimentsIniciatives view);

    List<ViewCrudImpedimentsIniciatives> DeleteImpediment(string idcheckin, string idimpediment);

    List<ViewCrudImpedimentsIniciatives> AddInitiatives(string idcheckin, ViewCrudImpedimentsIniciatives view);

    List<ViewCrudImpedimentsIniciatives> DeleteIniciative(string idcheckin, string idiniciative);

    string LikeImpediment(string idimpediment, string idkeyresult, bool like);

    string LikeIniciative(string idiniciatives, string idkeyresult, bool like);

    List<ViewListPersonPhoto> GetListManager();

    List<ViewListPersonPhoto> GetListEmployee();

    List<ViewListObjectiveEdit> GetObjectiveEditParticipant(ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListObjectiveEdit> GetObjectiveEditParticipantRH(ref long total, int count = 10, int page = 1, string filter = "");

    List<ViewListImpedimentsIniciatives> GetImpedimentsIniciatives(string idkeyresult, ref long total, int count = 10, int page = 1, string filter = "");

    List<ViewListObjectiveEdit> GetObjectiveEditResponsible(string idobjective, ref long total, int count = 10, int page = 1, string filter = "");

    string DeleteLikeIniciative(string idiniciatives, string idkeyresult, bool like);

    string DeleteLikeImpediments(string idimpediments, string idkeyresult, bool like);
    ViewListObjectiveResponsibleCard GetResponsibleCard(string id);

    string ImportObjectiveModel1(Stream stream);

  }
}
