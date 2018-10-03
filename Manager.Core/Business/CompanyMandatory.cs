using Manager.Core.Enumns;
using System;

namespace Manager.Core.Business
{
  public class CompanyMandatory
  {
    public Company Company { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
  }
}
