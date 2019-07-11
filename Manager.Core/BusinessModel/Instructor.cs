using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para turma
  /// </summary>
  public class Instructor : BaseEntityId
  {
    public ViewListPersonBase Person { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public string Schooling { get; set; }
    public ViewListCbo Cbo { get; set; }
    public string Content { get; set; }
    public EnumTypeInstructor TypeInstructor { get; set; }
  }
}
