using Manager.Core.Base;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class Sphere : BaseEntity
  {
    public string Name { get; set; }
    public EnumTypeSphere TypeSphere { get; set; }
    public Company Company { get; set; }
    public Sphere Template { get; set; }
  }
}
