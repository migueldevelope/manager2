using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class Course : BaseEntity
  {
    public string Name { get; set; }
    public string Content { get; set; }
    public byte Periodicity { get; set; }
    public byte Deadline { get; set; }
    public CourseESocial CourseESocial { get; set; }
  }
}
