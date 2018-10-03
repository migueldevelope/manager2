using Manager.Core.Base;
using Manager.Core.Enumns;
using System;

namespace Manager.Core.Business
{
  public class OccupationMandatory : BaseEntity
  {
    public Occupation Occupation { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
  }
}
