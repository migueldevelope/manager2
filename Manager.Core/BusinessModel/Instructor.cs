using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para turma
  /// </summary>
  public class Instructor : BaseEntity
  {
    public Person Person { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public string Schooling { get; set; }
    public Cbo Cbo { get; set; }
    public string Content { get; set; }
    public EnumTypeInstructor TypeInstructor { get; set; }
  }
}
