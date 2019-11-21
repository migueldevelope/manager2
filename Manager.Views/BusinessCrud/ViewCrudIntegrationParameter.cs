using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudIntegrationParameter : _ViewCrudBase
  {
    public EnumIntegrationType Type { get; set; }
    public EnumIntegrationProcess Process { get; set; }
    public EnumIntegrationMode Mode { get; set; }
    // =========== Pack Version ===========
    public string VersionPackProgram { get; set; }
    public string LinkPackProgram { get; set; }
    public string VersionPackCustom { get; set; }
    public string LinkPackCustom { get; set; }
    public string MessageAtualization { get; set; }
    // =========== Banco de dados ===========
    // string de conexão (Oracle ou SqlServer;nome servidor;usuario;senha;banco padrão)
    public string ConnectionString { get; set; }
    public string SqlCommand { get; set; }
    // =========== Arquivo CSV ou Excel ===========
    public string FilePathLocal { get; set; }
    public string SheetName { get; set; }
    public DateTime? LastExecution { get; set; }
    // =========== Execução do Serviço ===========
    public string StatusExecution { get; set; }
    // Versão do Programa
    public string ProgramVersionExecution { get; set; }
    // Versão da Customização
    public string CustomVersionExecution { get; set; }
    // Mensagem de Erro crítico
    public string CriticalError { get; set; }
    // Nome da máquina local
    public string MachineIdentity { get; set; }
    // Subir próximo LOG
    public bool UploadNextLog { get; set; }
    public string LinkLogExecution { get; set; }
    // Identificação da API personalizada
    public string ApiIdentification { get; set; }
    public EnumIntegrationKey IntegrationKey { get; set; }
    public string CultureDate { get; set; }
  }
}
