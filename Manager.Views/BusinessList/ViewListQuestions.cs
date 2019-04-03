using Manager.Views.Enumns;

namespace Manager.Views.BusinessList
{
  public class ViewListQuestions : _ViewListBase
  {
    public string Content { get; set; }
    public EnumTypeRotine TypeRotine { get; set; }
    public long Order { get; set; }
  }
}
