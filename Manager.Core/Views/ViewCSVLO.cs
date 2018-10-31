using Manager.Core.Enumns;

namespace Manager.Core.Views
{
  public class ViewCSVLO
  {
    public string IdGroup { get; set; }
    public string Name { get; set; }
    public long Line { get; set; }
    public long Col { get; set; }
    public EnumTypeLO Type { get; set; }
  }
}
