using Manager.Core.Base;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção utilizada paras entregas
  /// </summary>
  public class Activitie : BaseEntity
  {
    public string Name { get; set; }
    public long Order { get; set; }
  }
}
