using Manager.Views.Enumns;

namespace Manager.Core.Views
{
  public class ViewCSVLO
  {
    public string IdGroup { get; set; }
    public string Name { get; set; }
    public long Line { get; set; }
    public long Col { get; set; }
    public EnumTypeLineOpportunity Type { get; set; }
  }
}
