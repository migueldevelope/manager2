using Manager.Core.Base;
using Manager.Core.Enumns;

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
