using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class Certification : BaseEntity
  {
    public Person Person { get; set; }
    public CertificationItem CertificationItem { get; set; }
    public List<CertificationQuestions> Questions { get; set; }
    public List<CertificationPerson> ListPersons { get; set; }
    public EnumStatusCertification StatusCertification { get; set; }
    public List<AttachmentField> Attachments { get; set; }
    public string TextDefault { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DateEnd { get; set; }
  }
}
