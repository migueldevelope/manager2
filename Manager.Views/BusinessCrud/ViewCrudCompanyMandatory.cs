using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudCompanyMandatory : _ViewCrudBase
  {
    public ViewListCourse Course { get; set; }
    public ViewListCompany Company { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
  }
}
