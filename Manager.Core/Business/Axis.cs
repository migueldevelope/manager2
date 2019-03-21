using Manager.Core.Base;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class Axis : BaseEntity
  {
    public string Name { get; set; }
    public EnumTypeAxis TypeAxis { get; set; }
    public Sphere Sphere { get; set; }
    public Axis Template { get; set; }
  }
}
