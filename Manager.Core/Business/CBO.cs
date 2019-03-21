using Manager.Core.Base;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados1
  /// </summary>
  public class Cbo : BaseEntity
  {
    public string Name { get; set; }
    public string Code { get; set; }
  }
}
