using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceEvent
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
    Task<string> ReopeningEvent(string idevent);
    Task SetAttachment(string idevent, string url, string fileName, string idattachmentid);
    Task SetAttachmentHistoric(string idevent, string url, string fileName, string idattachmentid);
    Task<string> Remove(string id);
    Task<string> RemoveCourse(string id);
    Task<string> RemoveCourseESocial(string id);
    Task<string> RemoveEventHistoric(string id);
    Task<string> RemoveInstructor(string idevent, string id);
    Task<string> RemoveParticipant(string idevent, string idperson);
    Task<string> RemoveDays(string idevent, string idday);
    Task<string> Present(string idevent, string idparticipant, string idday, bool present);
    Task<string> SetGrade(string idevent, string idparticipant, decimal grade);



    Task<ViewListEvent> New(ViewCrudEvent view);
    Task<ViewListEvent> Update(ViewCrudEvent view);
    Task<ViewCrudEvent> Get(string id);
    Task<List<ViewListEvent>> List( int count = 10, int page = 1, string filter = "");
    Task<List<ViewListEventHistoric>> ListEventHistoricPerson(string id,  int count = 10, int page = 1, string filter = "");
    Task<string> NewCourse(ViewCrudCourse view);
    Task<string> UpdateCourse(ViewCrudCourse view);
    Task<ViewCrudCourse> GetCourse(string id);
    Task<List<ViewListCourse>> ListCourse( int count = 10, int page = 1, string filter = "");
    Task<string> NewCourseESocial(ViewCrudCourseESocial view);
    Task<string> UpdateCourseESocial(ViewCrudCourseESocial view);
    Task<ViewCrudCourseESocial> GetCourseESocial(string id);
    Task<List<ViewCrudCourseESocial>> ListCourseESocial( int count = 10, int page = 1, string filter = "");
    Task<string> NewEventHistoricFrontEnd(ViewCrudEventHistoric view);
    Task<string> UpdateEventHistoricFrontEnd(ViewCrudEventHistoric view);
    Task<ViewCrudEventHistoric> GetEventHistoric(string id);
    Task<List<ViewListEventHistoric>> ListEventHistoric( int count = 10, int page = 1, string filter = "");
    Task<string> AddParticipant(string idevent, ViewCrudParticipant view);
    Task<string> AddDays(string idevent, ViewCrudDaysEvent view);
    Task<string> AddInstructor(string idevent, ViewCrudInstructor view);
    Task<List<ViewCrudEntity>> ListEntity( int count = 10, int page = 1, string filter = "");
    Task<List<ViewListPersonResume>> ListPersonInstructor(string idevent, string idcompany,  int count = 10, int page = 1, string filter = "");
    Task<List<ViewListPersonResume>> ListPersonParticipants(string idevent, string idcompany,  int count = 10, int page = 1, string filter = "");
    Task<List<ViewCrudParticipant>> ListParticipants(string idevent,  int count = 10, int page = 1, string filter = "");
    Task<List<ViewListEvent>> ListEventOpen( int count = 10, int page = 1, string filter = "");
    Task<List<ViewListEvent>> ListEventEnd( int count = 10, int page = 1, string filter = "");
    Task<List<ViewListEvent>> ListEventOpenSubscription(string idperson,  int count = 10, int page = 1, string filter = "");
    Task<List<ViewListEvent>> ListEventSubscription(string idperson,  int count = 10, int page = 1, string filter = "");
  }
}
