using Manager.Core.Base;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business.Integration
{
    public class IntegrationParameter : BaseEntity
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
    // Identificação da API customizada do cliente
    public string ApiIdentification { get; set; }
    // Identificação da chave de colaborador
    // 0 - documento+empresa+estabelecimento+matricula
    // 1 - documento+empresa+matricula
    public EnumIntegrationKey IntegrationKey { get; set; }
    public string CultureDate { get; set; }
    // Token para autenticação da API
    public string ApiToken { get; set; }
  }
}
