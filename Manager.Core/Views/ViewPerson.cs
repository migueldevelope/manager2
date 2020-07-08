using Manager.Core.Business;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Core.Views
{
  public class ViewPerson
  {
    public string IdUser { get; set; }
    public string Name { get; set; }
    public string IdAccount { get; set; }
    public string NameAccount { get; set; }
    public string Token { get; set; }
    public string Photo { get; set; }
    public EnumChangePassword ChangePassword { get; set; }
    public List<DictionarySystem> DictionarySystem { get; set; }
    public List<ViewContract> Contracts { get; set; }
    public bool TermOfService { get; set; }
    public bool ExistsTermOfService { get; set; }
    public List<ViewListIdIndicators> Team { get; set; }
    public string ViewLO { get; set; }
    public string GoalProcess { get; set; }
    public string MeritocracyProcess { get; set; }
    public string ShowAutoManager { get; set; }
    public string ShowSalaryScaleManager { get; set; }
    public string ShowOffboardingProcess { get; set; }
    public string FeelingProcess { get; set; }
    public bool ShowSalary { get; set; }
    public bool ElearningVideo { get; set; }
    public bool ElearningCertificate { get; set; }
    public string ShowElearningEmeu { get; set; }
  }
}
