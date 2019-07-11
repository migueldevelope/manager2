using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudPersonMandatory : _ViewCrudBase
  {
    public ViewListCourse Course { get; set; }
    public ViewListPersonBase Person { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
  }
}
