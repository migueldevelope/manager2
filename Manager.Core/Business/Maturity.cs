using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class Maturity : BaseEntity
  {
    public string _idPerson { get; set; }
    public long CountMonitoring { get; set; }
    public long CountPlan { get; set; }
    public long CountPraise { get; set; }
    public long CountCertification { get; set; }
    public byte LevelMonitoring { get; set; }
    public byte LevelPlan { get; set; }
    public byte LevelPraise { get; set; }
    public byte LevelCertification { get; set; }
    public byte Value { get; set; }
  }
}
