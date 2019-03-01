using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGrade : _ViewCrudBase
  {
    public ViewListCompany Company { get; set; }
    public EnumSteps StepMedium { get; set; }
    public int Order { get; set; }
  }
}
