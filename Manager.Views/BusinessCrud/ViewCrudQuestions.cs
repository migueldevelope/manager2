using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudQuestions: _ViewCrudBase
  {
    public string Content { get; set; }
    public EnumTypeQuestion TypeQuestion { get; set; }
    public long Order { get; set; }
    public EnumTypeRotine TypeRotine { get; set; }
  }
}
