using Manager.Views.Enumns;

namespace Manager.Views.Integration
{
  public class ViewIntegrationParameterMode
  {
    public EnumIntegrationProcess Process { get; set; }
    public EnumIntegrationType Type { get; set; }
    public EnumIntegrationMode Mode { get; set; }
    public string ConnectionString { get; set; }
    public string SqlCommand { get; set; }
    public string FilePathLocal { get; set; }
  }
}
