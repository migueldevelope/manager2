using Manager.Core.Base;
using Manager.Core.Enumns;
using System;

namespace Manager.Core.Business
{
  public class CompanyMandatory: BaseEntity
  {
    public Course Course { get; set; }
    public Company Company { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
  }
}
