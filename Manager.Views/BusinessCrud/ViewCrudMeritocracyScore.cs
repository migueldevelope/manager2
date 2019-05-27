using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMeritocracyScore: _ViewCrudBase
  {
    public EnumStatus Enabled { get; set; }
    public decimal Weight { get; set; }
  }
}
