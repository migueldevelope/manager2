using Manager.Views.Enumns;

namespace Manager.Views.BusinessList
{
  public class ViewListPerson : _ViewListBase 
  {
    public EnumStatusUser StatusUser { get; set; }
    public ViewListCompany Company { get; set; }
    public ViewListEstablishment Establishment { get; set; }
    public string Registration { get; set; }
    public ViewListUser User { get; set; }
  }
}
