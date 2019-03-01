using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudSchoolingProfile : ViewCrudSchooling
  {
    public string Complement { get; set; }
    public EnumTypeSchooling Type { get; set; }
  }
}
