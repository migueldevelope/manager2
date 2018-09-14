﻿using Manager.Core.Base;
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
    string New(Event view);
    string Update(Event view);
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

  }
}
