using Manager.Core.Enumns;
using Manager.Views.Enumns;

namespace Manager.Core.Views
{
  public class ViewPerson
  {
    public string IdPerson { get; set; }
    public string Name { get; set; }
    public string IdAccount { get; set; }
    public string NameAccount { get; set; }
    public string Token { get; set; }
    public string Photo { get; set; }
    public string Logo { get; set; }
    public EnumChangePassword ChangePassword { get; set; }
    public EnumTypeUser TypeUser { get; set; }
  }
}
