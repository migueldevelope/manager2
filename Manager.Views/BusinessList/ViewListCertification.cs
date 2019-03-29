using Manager.Views.Enumns;
namespace Manager.Views.BusinessList
{
  public class ViewListCertification: _ViewListBase
  {
    public string idPerson { get; set; }
    public EnumStatusCertification StatusCertification { get; set; }
  }
}
