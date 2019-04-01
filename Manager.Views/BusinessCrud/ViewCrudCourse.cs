using Manager.Views.BusinessList;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudCourse: _ViewCrudBase
  {
    public string Content { get; set; }
    public byte Periodicity { get; set; }
    public byte Deadline { get; set; }
    public ViewCrudCourseESocial CourseESocial { get; set; }
    public List<ViewListCourse> Prerequisites { get; set; }
    public List<ViewListCourse> Equivalents { get; set; }
    public decimal Wordkload { get; set; }
  }
}
