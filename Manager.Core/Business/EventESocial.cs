using Manager.Core.Base;
using Manager.Views.BusinessCrud;

namespace Manager.Core.Business
{
  /// <summary>
  /// Coleção para curso esocial da turma
  /// </summary>
  public class EventESocial : BaseEntity
  {
    public ViewCrudCourseESocial CourseESocial { get; set; }
  }
}
