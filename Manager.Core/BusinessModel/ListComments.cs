using Manager.Core.Base;
using Manager.Core.Enumns;
using System;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para comentarios onboarding/monitoring
  /// </summary>
  public class ListComments : BaseEntity
  {
    public string Comments { get; set; }
    public DateTime? Date { get; set; }
    public EnumStatusView StatusView { get; set; }
    public EnumUserComment UserComment { get; set; }
  }
}
