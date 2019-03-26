using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessList
{
  public class ViewListIntegrationParameter : _ViewList
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
    // =========== Arquivo Excel ===========
    public string SheetName { get; set; }
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
