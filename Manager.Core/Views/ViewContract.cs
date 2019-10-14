using Manager.Views.Enumns;

namespace Manager.Core.Views
{
  public class ViewContract
  {
    public string IdPerson { get; set; }
    public EnumTypeUser TypeUser { get; set; }
    public string Logo { get; set; }
    public string Occupation { get; set; }
    public string Registration { get; set; }
    public string _idAccount { get; set; }
    public string NameAccount { get; set; }
  }
}
