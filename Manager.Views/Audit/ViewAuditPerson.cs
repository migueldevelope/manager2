using Manager.Views.Enumns;

namespace Manager.Views.Audit
{
  public class ViewAuditPerson : ViewAuditUser
  {
    public string IdPerson { get; set; }
    public string DisabledPerson { get; set; }
    public string IdCompany { get; set; }
    public string NameCompany { get; set; }
    public string IdEstablishment { get; set; }
    public string NameEstablishment { get; set; }
    public string Registration { get; set; }
    public string StatusUser { get; set; }
    public string IdManager { get; set; }
    public string NameManager { get; set; }
    public string IdOccupation { get; set; }
    public string NameOccupation { get; set; }
    public EnumTypeJourney TypeJorney { get; set; }
  }
}
