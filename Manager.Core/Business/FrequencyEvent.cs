using Manager.Core.Base;

namespace Manager.Core.Business
{
  /// <summary>
  /// Coleção para presenças de treinamento
  /// </summary>
  public class FrequencyEvent : BaseEntity
  {
    public DaysEvent DaysEvent { get; set; }
    public bool Present { get; set; }
  }
}
