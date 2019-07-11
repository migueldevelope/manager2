using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class Certification : BaseEntity
  {
    public ViewListPersonBaseManager Person { get; set; }
    public ViewListCertificationItem CertificationItem { get; set; }
    public List<ViewListCertificationQuestions> Questions { get; set; }
    public List<ViewCrudCertificationPerson> ListPersons { get; set; }
    public EnumStatusCertification StatusCertification { get; set; }
    public List<ViewCrudAttachmentField> Attachments { get; set; }
    public string TextDefault { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DateEnd { get; set; }
    public ViewListCertification GetViewList()
    {
      return new ViewListCertification()
      {
        _id = _id,
        Name = Person.Name,
        _idPerson = Person._id,
        StatusCertification = StatusCertification
      };
    }
  }
}
