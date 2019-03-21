using Manager.Core.Base;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados e coleção
  /// </summary>
  public class ProcessLevelTwo : BaseEntity
  {
    public string Name { get; set; }
    public ProcessLevelOne ProcessLevelOne { get; set; }
    public string Comments { get; set; }
    public long Order { get; set; }
  }
}
