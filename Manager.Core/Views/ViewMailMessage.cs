using Manager.Views.Enumns;

namespace Manager.Core.Views
{
  public class ViewMailMessage
  {
    public string Name { get; set; }
    public string Url { get; set; }
    public string Body { get; set; }
    public EnumTypeMailMessage Type { get; set; }
    public string Token { get; set; }
  }
}
