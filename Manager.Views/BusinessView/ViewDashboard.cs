namespace Manager.Views.BusinessView
{
  public class ViewDashboard
  {
    public long OnBoardingWait { get; set; }
    public long CheckpointWait { get; set; }
    public long MonitoringRealized { get; set; }
    //Total de ações de desenvolvimento(Programado, Finalizado e Vencido) nos últimos 12 meses.
    public long Plans { get; set; }
    public long CertificationRealized { get; set; }
    public long Recommendation { get; set; }
    public long OffBoardingRealized { get; set; }
  }
}
