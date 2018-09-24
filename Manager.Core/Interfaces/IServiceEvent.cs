using Manager.Core.Base;
using Manager.Core.Business;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Interfaces
{
  public interface IServiceEvent
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser baseUser);
    Event New(Event view);
    Event Update(Event view);
    string Remove(string id);
    Event Get(string id);
    List<Event> List(ref long total, int count = 10, int page = 1, string filter = "");
    string NewCourse(Course view);
    string UpdateCourse(Course view);
    string RemoveCourse(string id);
    Course GetCourse(string id);
    List<Course> ListCourse(ref long total, int count = 10, int page = 1, string filter = "");
    string NewCourseESocial(CourseESocial view);
    string UpdateCourseESocial(CourseESocial view);
    string RemoveCourseESocial(string id);
    CourseESocial GetCourseESocial(string id);
    List<CourseESocial> ListCourseESocial(ref long total, int count = 10, int page = 1, string filter = "");

    string NewEventHistoric(EventHistoric view);
    string UpdateEventHistoric(EventHistoric view);
    string RemoveEventHistoric(string id);
    EventHistoric GetEventHistoric(string id);
    List<EventHistoric> ListEventHistoric(ref long total, int count = 10, int page = 1, string filter = "");
    string RemoveParticipant(string idevent, string idperson);
    string AddParticipant(string idevent, Person person);
    string RemoveDays(string idevent, string idday);
    string AddDays(string idevent, DaysEvent days);
    string AddInstructor(string idevent, Instructor instructor);
    string RemoveInstructor(string idevent, string id);
    List<Entity> ListEntity(ref long total, int count = 10, int page = 1, string filter = "");
    List<Person> ListPersonInstructor(string idevent, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
    List<Person> ListPersonParticipants(string idevent, string idcompany, ref long total, int count = 10, int page = 1, string filter = "");
  }
}
