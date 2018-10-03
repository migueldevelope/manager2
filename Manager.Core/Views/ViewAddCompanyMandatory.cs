using Manager.Core.Business;
using Manager.Core.Enumns;
using System;

namespace Manager.Core.Views
{
  public class ViewAddCompanyMandatory
  {
    public Course Course { get; set; }
    public Company Company { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
  }
}
