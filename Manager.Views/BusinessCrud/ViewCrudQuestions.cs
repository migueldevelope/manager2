using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudQuestions
  {
    public string _id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public ViewListCompany Company { get; set; }
    public EnumTypeQuestion TypeQuestion { get; set; }
    public long Order { get; set; }
    public EnumTypeRotine TypeRotine { get; set; }
  }
}
