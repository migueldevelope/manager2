using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudCertification: _ViewCrudBase
  {
    public string _idPerson { get; set; }
    public ViewListCertificationItem CertificationItem { get; set; }
    public List<ViewListCertificationQuestions> Questions { get; set; }
    public List<ViewCrudCertificationPerson> ListPersons { get; set; }
    public EnumStatusCertification StatusCertification { get; set; }
    public List<ViewCrudAttachmentField> Attachments { get; set; }
    public string TextDefault { get; set; }
  }
}
