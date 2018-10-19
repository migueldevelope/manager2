using Manager.Core.Enumns;
using System;

namespace Manager.Core.Views.Integration
{
  public class ViewIntegrationParameter
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
    // =========== Banco de dados ===========
    // string de conexão (Oracle ou SqlServer;nome servidor;usuario;senha;banco padrão)
    public string ConnectionString { get; set; }
    public string SqlCommand { get; set; }
    // =========== Arquivo CSV ou Excel ===========
    public string FilePathLocal { get; set; }
    // =========== Execução do Serviço ===========
    public DateTime? LastExecution { get; set; }
    public string StatusExecution { get; set; }
    public string ProgramVersionExecution { get; set; }
    public string CustomVersionExecution { get; set; }
    public string CriticalError { get; set; }
    public string MachineIdentity { get; set; }
    // Subir próximo LOG
    public bool UploadNextLog { get; set; }
  }
}
