using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudCertificationPersonStatus
  {
    public string _idCertification { get; set; }
    public EnumStatusCertificationPerson StatusCertificationPerson { get; set; }
    public string Comments { get; set; }
  }
}