using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessView
{
  public class ViewExportStatusCertificationPerson
  {
    public string IdCertification { get; set; }
    public string NamePerson { get; set; }
    public string NameItem { get; set; }
    public EnumStatusCertification Status { get; set; }
    public DateTime? DateEnd { get; set; }
  }
}
