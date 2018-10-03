using Manager.Core.Business;
using Manager.Core.Enumns;
using System;

namespace Manager.Core.Views
{
  public class ViewAddOccupationMandatory
  {
    public Course Course { get; set; }
    public Occupation Occupation { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
  }
}
