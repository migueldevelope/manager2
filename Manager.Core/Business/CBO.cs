using Manager.Core.Base;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class CBO : BaseEntity
  {
    public string Name { get; set; }
    public string Code { get; set; }
  }
}
