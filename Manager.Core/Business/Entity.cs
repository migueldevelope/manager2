using Manager.Core.Base;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados - entidade da turma
  /// </summary>
  public class Entity : BaseEntity
  {
    public string Name { get; set; }
  }
}
