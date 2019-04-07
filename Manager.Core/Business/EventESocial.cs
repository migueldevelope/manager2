using Manager.Core.Base;

namespace Manager.Core.Business
{
  /// <summary>
  /// Coleção para curso esocial da turma
  /// </summary>
  public class EventESocial : BaseEntity
  {
    public CourseESocial CourseESocial { get; set; }
  }
}
