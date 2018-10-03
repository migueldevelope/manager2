using Manager.Core.Base;
using Manager.Core.Enumns;
using System;

namespace Manager.Core.Business
{
  public class TrainingPlan : BaseEntity
  {
    public Person Person { get; set; }
    public Course Course { get; set; }
    public DateTime? Deadline { get; set; }
    public EnumOrigin Origin { get; set; }
    public DateTime? Include { get; set; }
    public EnumStatusTrainingPlan StatusTrainingPlan { get; set; }
    public string Observartion { get; set; }
  }
}
