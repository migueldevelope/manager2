using Manager.Core.Enumns;
using System;

namespace Manager.Core.Views
{
  public class ViewCertificationStatus
  {
    public string _idCertification { get; set; }
    public EnumStatusCertificationPerson StatusCertificationPerson { get; set; }
    public string Comments { get; set; }
  }
}
