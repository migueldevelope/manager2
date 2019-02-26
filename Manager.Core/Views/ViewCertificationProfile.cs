using Manager.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Views
{
  public class ViewCertificationProfile
  {
    public List<CertificationItem> ItemSkill { get; set; }
    public List<CertificationItem> ItemActivitie { get; set; }
    public string TextDefault { get; set; }
  }
}
