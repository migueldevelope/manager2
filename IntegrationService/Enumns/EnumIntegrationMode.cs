using System.ComponentModel;

namespace IntegrationService.Enumns
{
  public enum EnumIntegrationMode : byte
  {
    [Description("Banco de Dados - Versão 1")]
    DataBaseV1 = 0,
    [Description("Arquivo CSV - Versão 1")]
    FileCsvV1 = 1,
    [Description("Microsoft Excel - Versão 1")]
    FileExcelV1 = 2
  }
}
