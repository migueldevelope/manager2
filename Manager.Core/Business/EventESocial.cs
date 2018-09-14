using Manager.Core.Base;
using Manager.Core.Enumns;

namespace Manager.Core.Business
{
  public class EventESocial : BaseEntity
  {
    public CourseESocial CourseESocial { get; set; }
    public EnumModalityESocial Modality { get; set; }
    public EnumTypeESocial TypeESocial { get; set; }
  }
}
