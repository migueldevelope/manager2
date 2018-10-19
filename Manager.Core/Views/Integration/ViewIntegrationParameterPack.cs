using Manager.Core.Enumns;

namespace Manager.Core.Views.Integration
{
  public class ViewIntegrationParameterPack
  {
    public string _id { get; set; }
    public EnumIntegrationType Type { get; set; }
    public EnumIntegrationMode Mode { get; set; }
    // =========== Pack Version ===========
    public string VersionPackProgram { get; set; }
    public string LinkPackProgram { get; set; }
    public string VersionPackCustom { get; set; }
    public string LinkPackCustom { get; set; }
    public string MessageAtualization { get; set; }
  }
}
