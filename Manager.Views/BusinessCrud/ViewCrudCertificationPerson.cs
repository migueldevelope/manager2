using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudCertificationPerson: _ViewCrudBase
  {
    public string TextDefault { get; set; }
    public string TextDefaultEnd { get; set; }
    public EnumStatusCertificationPerson StatusCertificationPerson { get; set; }
    public string Comments { get; set; }
    public DateTime? DateApprovation { get; set; }
  }
}
