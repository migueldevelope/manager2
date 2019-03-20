namespace Manager.Views.BusinessList
{
  public class ViewListPerson : _ViewList
  {
    public ViewListCompany Company { get; set; }
    public ViewListEstablishment Establishment { get; set; }
    public string Registration { get; set; }
    public ViewListUser User { get; set; }
  }
}
