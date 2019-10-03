using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceEvent
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    string ReopeningEvent(string idevent);
    void SetAttachment(string idevent, string url, string fileName, string idattachmentid);
    void SetAttachmentHistoric(string idevent, string url, string fileName, string idattachmentid);
    string Remove(string id);
    string RemoveCourse(string id);
    string RemoveCourseESocial(string id);
    string RemoveEventHistoric(string id);
    string RemoveInstructor(string idevent, string id);
    string RemoveParticipant(string idevent, string idperson);
    string RemoveDays(string idevent, string idday);
    string Present(string idevent, string idparticipant, string idday, bool present);
    string SetGrade(string idevent, string idparticipant, decimal grade);



    ViewListEvent New(ViewCrudEvent view);
    ViewListEvent Update(ViewCrudEvent view);
    ViewCrudEvent Get(string id);
    List<ViewListEventDetail> List(EnumTypeEvent type, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewCrudEventHistoric> ListEventHistoricPerson(string id, ref long total, int count = 10, int page = 1, string filter = "");
    string NewCourse(ViewCrudCourse view);
    string UpdateCourse(ViewCrudCourse view);
    ViewCrudCourse GetCourse(string id);
    List<ViewListCourse> ListCourse(ref long total, int count = 10, int page = 1, string filter = "");
    string NewCourseESocial(ViewCrudCourseESocial view);
    string UpdateCourseESocial(ViewCrudCourseESocial view);
    ViewCrudCourseESocial GetCourseESocial(string id);
    List<ViewCrudCourseESocial> ListCourseESocial(ref long total, int count = 10, int page = 1, string filter = "");
    string NewEventHistoricFrontEnd(ViewCrudEventHistoric view);
    string UpdateEventHistoricFrontEnd(ViewCrudEventHistoric view);
    ViewCrudEventHistoric GetEventHistoric(string id);
    List<ViewListEventHistoric> ListEventHistoric(ref long total, int count = 10, int page = 1, string filter = "");
    string AddParticipant(string idevent, ViewCrudParticipant view);
    string AddDays(string idevent, ViewCrudDaysEvent view);
    string AddInstructor(string idevent, ViewCrudInstructor view);
    List<ViewCrudEntity> ListEntity(ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListPersonResume> ListPersonInstructor(string idevent, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListPersonResume> ListPersonParticipants(string idevent, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewCrudParticipant> ListParticipants(string idevent, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListEvent> ListEventOpen(ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListEvent> ListEventEnd(ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListEvent> ListEventOpenSubscription(string idperson, ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListEvent> ListEventSubscription(string idperson, ref long total, int count = 10, int page = 1, string filter = "");
    string ImportTraning(Stream stream);
    List<ViewListHistoric> ListHistoric(string idperson, string idcourse, ViewFilterDate date);
  }
}
