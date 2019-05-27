using Manager.Core.Base;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  public class MeritocracyScore: BaseEntity
  {
    public string Name { get; set; }
    public EnumStatus Enabled { get; set; }
    public decimal Weight { get; set; }
  }
}
