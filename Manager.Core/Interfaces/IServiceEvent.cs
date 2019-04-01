using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
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

    #endregion


    #region Old
    Event New(Event view);
    Event Update(Event view);
    Event Get(string id);
    List<Event> List(ref long total, int count = 10, int page = 1, string filter = "");
    List<EventHistoric> ListEventHistoricPerson(string id, ref long total, int count = 10, int page = 1, string filter = "");
    string NewCourse(Course view);
    string UpdateCourse(Course view);
    Course GetCourse(string id);
    List<Course> ListCourse(ref long total, int count = 10, int page = 1, string filter = "");
    string NewCourseESocial(CourseESocial view);
    string UpdateCourseESocial(CourseESocial view);
    CourseESocial GetCourseESocial(string id);
    List<CourseESocial> ListCourseESocial(ref long total, int count = 10, int page = 1, string filter = "");
    string NewEventHistoric(EventHistoric view);
    string NewEventHistoricFrontEnd(EventHistoric view);
    string UpdateEventHistoric(EventHistoric view);
    string UpdateEventHistoricFrontEnd(EventHistoric view);
    EventHistoric GetEventHistoric(string id);
    List<EventHistoric> ListEventHistoric(ref long total, int count = 10, int page = 1, string filter = "");
    string AddParticipant(string idevent, Participant participant);
    string AddDays(string idevent, DaysEvent days);
    string AddInstructor(string idevent, Instructor instructor);
    List<Entity> ListEntity(ref long total, int count = 10, int page = 1, string filter = "");
    List<Person> ListPersonInstructor(string idevent, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<Person> ListPersonParticipants(string idevent, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<Participant> ListParticipants(string idevent, ref long total, int count = 10, int page = 1, string filter = "");
    List<Event> ListEventOpen(ref long total, int count = 10, int page = 1, string filter = "");
    List<Event> ListEventEnd(ref long total, int count = 10, int page = 1, string filter = "");
    List<Event> ListEventOpenSubscription(string idperson, ref long total, int count = 10, int page = 1, string filter = "");
    List<Event> ListEventSubscription(string idperson, ref long total, int count = 10, int page = 1, string filter = "");
    #endregion

  }
}
