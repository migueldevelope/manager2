
using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudTextDefault: _ViewCrudBase
  {
    public string Content { get; set; }
    public EnumTypeText TypeText { get; set; }
    public ViewListCompany Company { get; set; }
  }
}
