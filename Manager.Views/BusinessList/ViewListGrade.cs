using Manager.Views.Enumns;

namespace Manager.Views.BusinessList
{
  public class ViewListGrade : _ViewListBase
  {
    public string IdCompany { get; set; }
    public string NameCompany { get; set; }
    public EnumSteps StepMedium { get; set; }
    public int Order { get; set; }
  }
}
