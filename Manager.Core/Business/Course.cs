using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class Course : BaseEntity
  {
    public string Name { get; set; }
    public string Content { get; set; }
    public byte Periodicity { get; set; }
    public byte Deadline { get; set; }
    public ViewCrudCourseESocial CourseESocial { get; set; }
    public List<ViewListCourse> Prerequisites { get; set; }
    public List<ViewListCourse> Equivalents { get; set; }
    public decimal Wordkload { get; set; }
    public ViewListCourse GetViewList()
    {
      return new ViewListCourse()
      {
        _id = _id,
        Name = Name
      };
    }
  }
}
