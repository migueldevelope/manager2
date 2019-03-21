using Manager.Core.Base;
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
    public CourseESocial CourseESocial { get; set; }
    public List<Course> Prerequisites { get; set; }
    public List<Course> Equivalents { get; set; }
    public decimal Wordkload { get; set; }
  }
}
