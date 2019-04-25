using Manager.Core.Base;
using Manager.Views.BusinessList;
using System;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para objetivos da empresa
  /// </summary>
  public class GoalsCompanyItem: BaseEntity
  {
    public ViewListGoal Goals { get; set; }
    public byte Weight { get; set; }
    public DateTime? Deadline { get; set; }
    public string Realized { get; set; }
    public string Result { get; set; }
    public decimal Achievement { get; set; }
    public string Target { get; set; }
  }
}
