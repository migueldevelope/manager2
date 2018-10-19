using System.ComponentModel;

namespace IntegrationService.Enumns
{
  public enum EnumIntegrationType : byte
  {
    [Description("Básico")]
    Basic = 0,
    [Description("Completo")]
    Complete = 1,
    [Description("Customizado")]
    Custom = 2
  }
}
