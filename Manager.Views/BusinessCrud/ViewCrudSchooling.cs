using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudSchooling : _ViewCrudBase
  {
    public long Order { get; set; }
    public string Complement { get; set; }
    public EnumTypeSchooling Type { get; set; }
  }
}
