using Manager.Core.Base;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class Account : BaseEntity
  {
    public string Name { get; set; }
    public string Nickname { get; set; }
  }
}
