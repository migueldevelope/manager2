namespace Manager.Views.Integration
{
  public class ViewIntegrationParameterExecution
  {
    public string _id { get; set; }
    // =========== Execução do Serviço ===========
    public string StatusExecution { get; set; }
    public string ProgramVersionExecution { get; set; }
    public string CustomVersionExecution { get; set; }
    public string CriticalError { get; set; }
    public string MachineIdentity { get; set; }
    // Subir próximo LOG
    public bool UploadNextLog { get; set; }
    public string LinkLogExecution { get; set; }
  }
}
