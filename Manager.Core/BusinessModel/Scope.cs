using Manager.Core.Base;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  ///  Coleção para grupo de cargo
  /// </summary>
  public class Scope : BaseEntity
  {
    public string Name { get; set; }
    public long Order { get; set; }
  }
}
