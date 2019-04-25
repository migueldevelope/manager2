using Manager.Core.Base;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class TrainingPlan : BaseEntity
  {
    public ViewListPersonPlan Person { get; set; }
    public ViewListCourse Course { get; set; }
    public DateTime? Deadline { get; set; }
    public EnumOrigin Origin { get; set; }
    public DateTime? Include { get; set; }
    public EnumStatusTrainingPlan StatusTrainingPlan { get; set; }
    public string Observartion { get; set; }
    public ViewListEvent Event { get; set; }
  }
}
