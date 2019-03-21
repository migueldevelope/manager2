using Manager.Core.Base;
namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class Establishment : BaseEntity
  {
    public Company Company { get; set; }
    public string Name { get; set; }
  }
}
