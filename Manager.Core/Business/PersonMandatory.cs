using Manager.Core.Enumns;
using System;

namespace Manager.Core.Business
{
  public class PersonMandatory
  {
    public Person Person { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
  }
}
