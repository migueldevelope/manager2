using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Interfaces
{
  public interface IServiceEvent
  {
    #region Event
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
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
    List<ViewListEvent> List(ref long total, int count = 10, int page = 1, string filter = "");
    List<ViewListEventHistoric> ListEventHistoricPerson(string id, ref long total, int count = 10, int page = 1, string filter = "");
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

    //string NewEventHistoric(EventHistoric view);

    //string UpdateEventHistoric(EventHistoric view);

    #endregion

    #region Old
    Event NewOld(Event view);
    Event UpdateOld(Event view);
    Event GetOld(string id);
    List<Event> ListOld(ref long total, int count = 10, int page = 1, string filter = "");
    List<EventHistoric> ListEventHistoricPersonOld(string id, ref long total, int count = 10, int page = 1, string filter = "");
    string NewCourseOld(Course view);
    string UpdateCourseOld(Course view);
    Course GetCourseOld(string id);
    List<Course> ListCourseOld(ref long total, int count = 10, int page = 1, string filter = "");
    string NewCourseESocialOld(CourseESocial view);
    string UpdateCourseESocialOld(CourseESocial view);
    CourseESocial GetCourseESocialOld(string id);
    List<CourseESocial> ListCourseESocialOld(ref long total, int count = 10, int page = 1, string filter = "");
    string NewEventHistoricOld(EventHistoric view);
    string NewEventHistoricFrontEndOld(EventHistoric view);
    string UpdateEventHistoricOld(EventHistoric view);
    string UpdateEventHistoricFrontEndOld(EventHistoric view);
    EventHistoric GetEventHistoricOld(string id);
    List<EventHistoric> ListEventHistoricOld(ref long total, int count = 10, int page = 1, string filter = "");
    string AddParticipantOld(string idevent, Participant participant);
    string AddDaysOld(string idevent, DaysEvent days);
    string AddInstructorOld(string idevent, Instructor instructor);
    List<Entity> ListEntityOld(ref long total, int count = 10, int page = 1, string filter = "");
    List<Person> ListPersonInstructorOld(string idevent, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<Person> ListPersonParticipantsOld(string idevent, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<Participant> ListParticipantsOld(string idevent, ref long total, int count = 10, int page = 1, string filter = "");
    List<Event> ListEventOpenOld(ref long total, int count = 10, int page = 1, string filter = "");
    List<Event> ListEventEndOld(ref long total, int count = 10, int page = 1, string filter = "");
    List<Event> ListEventOpenSubscriptionOld(string idperson, ref long total, int count = 10, int page = 1, string filter = "");
    List<Event> ListEventSubscriptionOld(string idperson, ref long total, int count = 10, int page = 1, string filter = "");
    #endregion

  }
}
