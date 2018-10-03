using Manager.Core.Business;
using Manager.Core.Enumns;
using System;

namespace Manager.Core.Views
{
  public class ViewAddPersonMandatory
  {
    public Course Course { get; set; }
    public Person Person { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
  }
}
