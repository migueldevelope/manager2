using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessView
{
  public class ViewExportStatusOnboarding
  {
    public string IdOnboarding { get; set; }
    public string NamePerson { get; set; }
    public EnumStatusOnBoarding Status { get; set; }
    public string Occupation { get; set; }
    public DateTime? DataEnd { get; set; }
  }
}
