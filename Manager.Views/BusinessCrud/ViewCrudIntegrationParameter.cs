using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudIntegrationParameter : _ViewCrudBase
  {
    public EnumIntegrationMode Mode { get; set; }
    public EnumIntegrationVersion Version { get; set; }
    // =========== Banco de dados ===========
    // string de conexão (Oracle ou SqlServer;nome servidor;usuario;senha;banco padrão)
    public string ConnectionString { get; set; }
    public string SqlCommand { get; set; }
    // =========== Arquivo CSV ou Excel ===========
    public string FilePathLocal { get; set; }
    public string SheetName { get; set; }
    // =========== Execução do Serviço ===========
    public DateTime? LastExecution { get; set; }
    public string StatusExecution { get; set; }
    // Versão do Programa
    public string ProgramVersionExecution { get; set; }
    // Mensagem de Erro crítico
    public string CriticalError { get; set; }
    // Nome da máquina local
    public string MachineIdentity { get; set; }
    // Identificação da API personalizada
    public string ApiIdentification { get; set; }
    public string ApiToken { get; set; }
    public EnumIntegrationKey IntegrationKey { get; set; }
    public string CultureDate { get; set; }
  }
}
